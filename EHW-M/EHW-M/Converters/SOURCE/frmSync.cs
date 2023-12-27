using System;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using PnSync;
using PnUtils;
using System.Collections.Generic;
using System.Globalization;
using MobileSales.BL;

namespace MobileSales
{
    public partial class frmSync : Form
    {
        public frmSync()
        {
            InitializeComponent();
        }

        string strIDAgjenti = "";
        string strDepo = "";
        internal static bool ConfigSynced = false;
        internal static bool SyncConfigSynced = false;

        #region Initialize Instace

        frmRaportiSinkronizimit frmRaproti = null;

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            this.lblDeviceID.Text = "PSION: " + frmHome.DevIDConfig;
            this.lblDepo.Text = "DEPO: " + frmHome.Depo;
            Cursor.Current = Cursors.Default;

        }

        #region HelperFunctions

        private bool InsertLog(string DeviceID, string SessionID, string StartSessionDT, string EndSessionDT, string TableName, string StartTime, string EndTime, string SyncDir, string NrDownloadedRect, string NrRecsUpdate, string NrRecsInsert, string NrUploadedRecs, SqlCeConnection conn)
        {
            bool Success = false;

            if (StartSessionDT == "") StartSessionDT = "NULL";
            else StartSessionDT = "'" + StartSessionDT + "'";

            if (EndSessionDT == "") EndSessionDT = "NULL";
            else EndSessionDT = "'" + EndSessionDT + "'";

            if (StartTime == "") StartTime = "NULL";
            else StartTime = "'" + StartTime + "'";

            if (EndTime == "") EndTime = "NULL";
            else EndTime = "'" + EndTime + "'";

            using (SqlCeConnection con2 = new SqlCeConnection(conn.ConnectionString))
            using (SqlCeCommand kom = new SqlCeCommand("", con2))
            {
                kom.CommandText = "insert into LogSync_Session values (" +
                "'" + DeviceID + "','" + SessionID + "'," + StartSessionDT + "," + EndSessionDT + "," +
                "'" + TableName + "'," + StartTime + "," + EndTime + ",'" + SyncDir + "','" + NrDownloadedRect + "'," +
                "'" + NrRecsUpdate + "','" + NrRecsInsert + "','" + NrUploadedRecs + "')";
                con2.Open();
                try
                {
                    kom.ExecuteNonQuery();
                    Success = true;
                }
                catch (SqlCeException ex)
                {
                    Success = false;
                    DbUtils.WriteSQLCeErrorLog(ex, kom.CommandText.ToString());
                }
                catch (Exception ex2)
                {
                    Success = false;
                    DbUtils.WriteExeptionErrorLog(ex2);
                }
            }
            return Success;
        }

        private bool InsertErrorLog(string DeviceID, string SessionID, string Err_Code, string Err_Message, string Err_Module, string Err_Line, SqlCeConnection conn)
        {
            bool Success = false;
            string st = "insert into Log_SyncErrors values ('" + DeviceID + "','" + SessionID + "','" + Err_Code + "','" + Err_Message + "','" + Err_Module + "','" + Err_Line + "')";
            using (SqlCeCommand kom = new SqlCeCommand(st, conn))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) { conn.Open(); }
                    kom.ExecuteNonQuery();
                    Success = true;
                    conn.Close();
                }
                catch (SqlCeException ex)
                {
                    DbUtils.WriteSQLCeErrorLog(ex);
                    Success = false;
                }
                catch (Exception ex)
                {
                    DbUtils.WriteExeptionErrorLog(ex);
                    Success = false;
                }
            }
            return Success;
        }

        private DataTable GetServerData(string LocalTable, string RemoteSqlQuery)
        {
            ConfigXpnParams SyncParams;
            ConfigXpn.ReadConfiguration(out SyncParams);
            Sync Snc = new Sync(SyncParams.LocalDbPath,
                                SyncParams.LocalDbPassword,
                                SyncParams.ServerName,
                                SyncParams.MobileServerUrl,
                                SyncParams.DbName,
                                SyncParams.UserName,
                                SyncParams.Password);

            DataTable dt = Snc.GetCustomData(LocalTable, RemoteSqlQuery);
            return dt;
        }

        private int GetStoqetCount(string Depo)
        {
            int _Count = -1;
            ConfigXpnParams Config;
            ConfigXpn.ReadConfiguration(out Config);
            string _TempTable = "tmpStoqetCount";

            DataTable dt = GetServerData(_TempTable, "SELECT COUNT(*) AS RecordCount FROM Stoqet where Depo='" + Depo + "'");
            if (dt.Rows.Count > 0)
            {
                _Count = Convert.ToInt16(dt.Rows[0][0]);
            }


            SqlCeConnection con = new SqlCeConnection(Config.ConnectionString);
            ExecuteSql("DROP TABLE " + _TempTable, con);
            dt.Dispose();
            return _Count;
        }

        private bool SafeDelete(string Tablename, string PKField)
        {
            bool a = false;

            #region Initialize
            ConfigXpnParams SyncParams;
            ConfigXpn.ReadConfiguration(out SyncParams);
            Sync Snc = new Sync(SyncParams.LocalDbPath,
                                SyncParams.LocalDbPassword,
                                SyncParams.ServerName,
                                SyncParams.MobileServerUrl,
                                SyncParams.DbName,
                                SyncParams.UserName,
                                SyncParams.Password);

            string ConnStr = "Data Source=" + SyncParams.LocalDbPath;
            if (SyncParams.LocalDbPassword.Trim() != "") ConnStr += ";Password=" + SyncParams.LocalDbPassword;
            StringBuilder[] strPK_Values = null;
            SqlCeConnection con = new SqlCeConnection(ConnStr);
            SqlCeDataAdapter da = null;
            DataTable dt = null;
            string Seperator = ",";

            #endregion

            #region BuildScript of PKs

            string[] PKs = PKField.Split(',');
            strPK_Values = new StringBuilder[PKs.Length];


            try
            {
                da = new SqlCeDataAdapter("SELECT " + PKField + " FROM " + Tablename, con);
                dt = new DataTable();

                for (int i = 0; i < strPK_Values.Length; i++)
                {
                    strPK_Values[i] = new StringBuilder();
                    dt.Reset();
                    da.SelectCommand.CommandText = "SELECT " + PKs[i] + " FROM " + Tablename;
                    da.Fill(dt);

                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (j < dt.Rows.Count - 1) Seperator = ","; else Seperator = "";
                        strPK_Values[i].Append(("'" + dt.Rows[j][0].ToString() + "'" + Seperator).ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                //DbUtils(ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                da.Dispose();
                dt.Dispose();
            }



            #endregion

            #region SafeDelete

            string tempTableName = "tmpServerData" + Tablename + frmHome.Depo;
            string PsionDeleteCommand = "";

            try
            {
                //Create TempTable in Server, copy data from original table
                a = Snc.ExecuteServerQuery("IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES t WHERE t.TABLE_NAME='" + tempTableName + "') \n"
                                           + "BEGIN \n"
                                           + "	DROP TABLE " + tempTableName + " \n"
                                           + "END \n"
                                           + " SELECT * INTO " + tempTableName + " FROM " + Tablename);
                if (a)
                {
                    //execute snapshot to get tempTable with data filtered by Psion request (PKeys in Psion)
                    string Condition = "";
                    if (strPK_Values[0].Length > 0)
                    {
                        if (strPK_Values.Length == 2)
                        {
                            Condition = PKs[0] + " IN (" + strPK_Values[0].ToString() + ") AND " + PKs[1] + " IN (" + strPK_Values[1].ToString() + ")";
                        }
                        else
                        {
                            Condition = PKs[0] + " IN (" + strPK_Values[0].ToString() + ")";
                        }
                    }
                    else
                    {
                        Condition = "1=2";
                    }

                    a = Snc.SyncTable(tempTableName, null, PnSyncDirection.SnapshotDownload, Condition, "", "");
                }

                if (a)
                {
                    //perform delete in PSION (of records  that exist in Server)
                    PsionDeleteCommand = "DELETE FROM " + Tablename + " WHERE ";
                    if (PKs.Length >= 2)
                    {
                        PsionDeleteCommand = "DELETE FROM " + Tablename + " WHERE EXISTS ( Select * from " + tempTableName + " as tmp Where (";


                        for (int i = 0; i < strPK_Values.Length; i++)
                        {
                            if (i < strPK_Values.Length - 1) Seperator = " AND "; else Seperator = "";
                            PsionDeleteCommand += PKs[i] + "=" + Tablename + "." + PKs[i] + " )" + Seperator;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < strPK_Values.Length; i++)
                        {
                            if (i < strPK_Values.Length - 1) Seperator = " AND "; else Seperator = "";
                            PsionDeleteCommand += PKs[i] + " IN (SELECT " + PKs[i] + " FROM " + tempTableName + ") " + Seperator;
                        }
                    }




                    if (ExecuteSql(PsionDeleteCommand, con))
                    {
                        //if for any reason some records couldn't be deleted (they don't exist in server) 
                        //set their syncStatus to "0" to sync next time
                        ExecuteSql("UPDATE " + Tablename + " SET SyncStatus=0", con);
                    }
                }
                //drop tempTable in Server
                Snc.ExecuteServerQuery("IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES t WHERE t.TABLE_NAME='" + tempTableName + "') \n"
                                       + "BEGIN \n"
                                       + "	DROP TABLE " + tempTableName + " \n"
                                       + "END");

                //drop tempTable in PSION after compare
                ExecuteSql("DROP TABLE " + tempTableName, con);
            }
            catch (Exception)
            {
                a = false;
            }
            #endregion

            return a;
        }
        //**********************************SYNCALL METHOD (Application Specific)*********************************************
        private void SyncAll(Label lblProgress, ProgressBar pbProgress)
        {

            #region InitializeSync
            GC.Collect();
            ConfigXpnParams SyncParams;
            ConfigXpn.ReadConfiguration(out SyncParams);
            Sync Snc = new Sync(SyncParams.LocalDbPath,
                                SyncParams.LocalDbPassword,
                                SyncParams.ServerName,
                                SyncParams.MobileServerUrl,
                                SyncParams.DbName,
                                SyncParams.UserName,
                                SyncParams.Password);

            StringBuilder strbSyncLog = new StringBuilder();//used for Log in TextBox Control
            string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            string[] KeyFields = null;//Used in sync
            string strFilterDwn = "1=1";//Used in sync
            string strFilterUp = "1=1"; //Used in sync           
            string strTableName = "";//Used in sync            

            int _CurrNrFat = 0; //used for fixNrKufijt
            int _CurrNrFatJT = 0;//used for fixNrKufijt
            int _CurrNrFat_D = 0; //used for fixNrKufijt

            bool SyncSuccesful = true; //Used in sync
            bool SyncSendSuccessful = true; //Used in Sync
            MobSellSyncDirection MobSyncDirection; //Used in Sync
            int _stoqetServer = 0;


            string ConnString = "Data Source=" + SyncParams.LocalDbPath; //Used in SqlCeConnections below
            if (SyncParams.LocalDbPassword.Trim() != "")
                ConnString += ";Password=" + SyncParams.LocalDbPassword;

            GC.Collect();


            //*****************Configs SYNC******************************************************************
            lblProgress.Text = "Inicializimi...";
            Application.DoEvents();

            SyncConfigSynced = Snc.SyncTable("SyncConfiguration", null, PnSyncDirection.SnapshotDownload, "", "", "");
            //MessageBox.Show("SyncConfigSynced " + SyncConfigSynced.ToString());
            ConfigSynced = Snc.SyncTable("Konfigurimi", null, PnSyncDirection.SnapshotDownload, "", "", "");
           // MessageBox.Show("ConfigSynced " + ConfigSynced.ToString());
            if ((!ConfigSynced) || (!SyncConfigSynced))
            {
                MessageBox.Show("Sinkronizimi nuk mund te vazhdoje [Komunikimi me SqlMobileServer deshtoi!]", "Gabim", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                DbUtils.WriteErrorLog(PnSync.Sync.SyncLog.ErrorMsg);
                DbUtils.WriteErrorLog(PnSync.Sync.SyncLog.ErrorCode);

                lblProgresi.Text = "";
                lblProgresi.Visible = false;
                Cursor.Current = Cursors.Default;
                return;
            }

            #endregion


            #region GetDepoDeviceIDAgent
            //*********************get Depo and DeviceID and IDAgent****************************************
            using (SqlCeConnection con = new SqlCeConnection(ConnString))
            using (SqlCeDataAdapter daKonfig = new SqlCeDataAdapter("", con))
            using (DataTable dtKonfig = new DataTable())
            using (DataSet ds = new DataSet())
            {
                try
                {
                    daKonfig.SelectCommand.CommandText = "select IDAgent from Konfigurimi where DeviceID='" + SyncParams.DeviceID + "'";
                    daKonfig.Fill(dtKonfig);
                    if (dtKonfig.Rows.Count > 0)
                    {
                        strIDAgjenti = dtKonfig.Rows[0]["IDAgent"].ToString();
                    }
                    daKonfig.SelectCommand.CommandText = "select Depo from Konfigurimi where DeviceID='" + SyncParams.DeviceID + "'";
                    dtKonfig.Clear();
                    daKonfig.Fill(dtKonfig);

                    if (dtKonfig.Rows.Count > 0)
                    {
                        strDepo = dtKonfig.Rows[0]["Depo"].ToString().Trim();
                    }
                }
                catch (SqlCeException a) { DbUtils.WriteSQLCeErrorLog(a); Cursor.Current = Cursors.Default; }
                catch (Exception a) { DbUtils.WriteExeptionErrorLog(a); Cursor.Current = Cursors.Default; }

                daKonfig.SelectCommand.CommandText = "select * from SyncConfiguration order by SyncOrder Asc";



                StreamWriter fOut = new StreamWriter(CurrentDir + "\\log.txt");
                fOut.WriteLine("StartTime=" + DateTime.Now);

                //*********************Sync Tables***************************************************************
                try
                {

                    daKonfig.Fill(ds);
                    if (ds.Tables[0].Rows.Count < 1)
                    {
                        MessageBox.Show("Nuk ka tabela per sinkronizim ne SyncConfig");
                        return;
                    }

                    lblProgress.Visible = true;
                    pbProgress.Maximum = ds.Tables[0].Rows.Count;
                    pbProgress.Value = 1;
                    Application.DoEvents();

                    string strSessionID = System.Guid.NewGuid().ToString();


                    //InserLog START SESSION
                    InsertLog(SyncParams.DeviceID, strSessionID, DateTime.Now.ToString("yyyy.MM.dd hh:mm:ss"), "", "START_SESSION", "", "", "", "", "", "", "", con);

                    #region FOREACH_TABLE_IN_SYNCCONFIG
                    //******************FOR EACH TABLE IN SyncConfig*******************************************
                    double ProgressPercent = 0;

                    //this var is used later to know which table has succeeded or failed, number of uploaded down. insert or updt records.
                    //SyncInfo = SyncRecords.GetTables(con.ConnectionString); 

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        MobSyncDirection = (MobSellSyncDirection)Convert.ToInt16(ds.Tables[0].Rows[i]["SyncDirection"]);
                        strTableName = ds.Tables[0].Rows[i]["TableName"].ToString();
                        KeyFields = ds.Tables[0].Rows[i]["PK_FieldName"].ToString().Split(',');//get PK_Field1,PK_Field2 in array, if more than 1                    

                        #region BuildFilters
                        //******************Filters*******************************************
                        //if FilterUp FilterDown, are null, make 1=1, which is no filter just to fill the gap

                        strFilterUp = ds.Tables[0].Rows[i]["FilterUp"].ToString();
                        strFilterDwn = ds.Tables[0].Rows[i]["FilterDwn"].ToString();

                        if (strFilterUp.Trim() == "")
                            strFilterUp = "1=1";

                        if (strFilterDwn.Trim() == "")
                            strFilterDwn = "1=1";

                        switch (strTableName)
                        {

                            case "KlientDheLokacion":
                            case "Klientet":
                            case "Stoqet":
                                {
                                    strFilterDwn = "(Depo='" + strDepo + "' OR Depo='' OR Depo IS NULL)";
                                    strFilterUp = "(Depo='" + strDepo + "' OR Depo='' OR Depo IS NULL)";
                                    break;
                                }
                            case "SalesPrice":
                                {
                                    string getOnlyThisWeekKlients = @"SalesCode IN 
                                                                    (  SELECT IDKlientDheLokacion FROM Vizitat WHERE IDAgjenti ='" + strIDAgjenti + @"' ) OR SalesCode='STANDARD' ";
                                    strFilterDwn = "(Depo='" + strDepo + "' OR Depo='' OR Depo IS NULL) and (" + getOnlyThisWeekKlients + ")";
                                    strFilterUp = "(Depo='" + strDepo + "' OR Depo='' OR Depo IS NULL) and (" + getOnlyThisWeekKlients + ")";
                                    break;
                                }
                            case "Vizitat":
                                {
                                    string AddUPFilter = ds.Tables[0].Rows[i]["FilterUp"].ToString();
                                    string AddDwnFilter = ds.Tables[0].Rows[i]["FilterDwn"].ToString();

                                    strFilterDwn = "DeviceID='" + SyncParams.DeviceID + "'";
                                    strFilterUp = "DeviceID='" + SyncParams.DeviceID + "'";

                                    if (AddDwnFilter.Trim() != "")
                                        strFilterDwn += " AND '" + AddDwnFilter + "'";
                                    if (AddUPFilter.Trim() != "") //if Filter from SyncCOnfig not empty
                                        strFilterUp += " AND '" + AddUPFilter + "'";

                                    break;
                                }
                            case "Malli_Mbetur":
                                {
                                    UpdateMalli_Mbetur(con);
                                    break;
                                }

                        }

                        #endregion

                        #region RefreshSyncProgress

                        ProgressPercent = (((i + 1) * 100) / pbProgress.Maximum);
                        ProgressPercent = Math.Round(ProgressPercent, 2);
                        lblProgress.Text = "Sinkronizimi [" + ProgressPercent.ToString() + "%]" + strTableName + "...";
                        pbProgress.Value = i + 1;
                        textBox1.Text = strbSyncLog.ToString(); //logText
                        if (textBox1.Text.Length > 0)
                        {
                            textBox1.SelectionStart = textBox1.Text.Length - 1;
                            textBox1.SelectionLength = 1;
                        }
                        textBox1.ScrollToCaret();
                        Application.DoEvents();

                        #endregion

                        SaveValuesKufijtFaturave(ref strFilterUp, strTableName, con, ref _CurrNrFat, ref _CurrNrFatJT, ref _CurrNrFat_D);

                        #region DoPnSync

                        switch (MobSyncDirection)
                        {

                            case MobSellSyncDirection.SnapshotSpecial:
                                {
                                    //Wont't do the snapshot if server returns nothing
                                    if (strTableName == "Stoqet")
                                    {
                                        _stoqetServer = GetStoqetCount(frmHome.Depo);
                                    }
                                    SyncSuccesful = Snc.SyncTable(strTableName, KeyFields, PnSyncDirection.SnapshotSpecial, strFilterDwn, "SyncStatus", strDepo);
                                    if ((int)PnFunctions.SumDataColumn("Malli_Mbetur", "SasiaShitur", con) == 0 && (int)PnFunctions.SumDataColumn("Malli_Mbetur", "SasiaKthyer", con) == 0)
                                    {
                                        if (_stoqetServer != 0) //Nese ka stoke per ngarkim
                                        {
                                            CopyStock(con);
                                        }
                                    }
                                    break;
                                }

                            case MobSellSyncDirection.Snapshot:
                                {
                                    SyncSuccesful = Snc.SyncTable(strTableName, null, PnSyncDirection.SnapshotDownload, strFilterUp, "", "");
                                    break;
                                }

                            case MobSellSyncDirection.Download:  //download with insert and Update
                                {
                                    SyncSuccesful = Snc.SyncTable(strTableName, KeyFields, PnSyncDirection.Download, strFilterDwn, "SyncStatus", "");
                                    break;
                                }
                            case MobSellSyncDirection.Upload: //upload with insert and update
                                {
                                    string StatusField = "SyncStatus";
                                    if (strTableName == "Malli_Mbetur" )
                                    {
                                        StatusField = "0";
                                    }
                                    else
                                    {
                                        KeyFields = null;
                                    }
                                    SyncSuccesful = Snc.SyncTable(strTableName, KeyFields, PnSyncDirection.Upload, strFilterUp, StatusField, "");
                                    if (SyncSuccesful)
                                    {
                                        switch (strTableName)
                                        {
                                            case "Liferimi":
                                                {
                                                    SafeDelete("Liferimi", "IDLiferimi");
                                                    Snc.ExecuteServerQuery("Update Liferimi set Export_Status =0 where DeviceID = '" + SyncParams.DeviceID + "' AND Export_Status =2");
                                                    break;
                                                }
                                            case "LiferimiArt":
                                                {
                                                    SafeDelete("LiferimiArt", "IDLiferimi,IDArtikulli");
                                                    break;

                                                }
                                            case "Porosite":
                                                {
                                                    SafeDelete("Porosite", "IDPorosia");
                                                    break;
                                                }
                                            case "PorosiaArt":
                                                {
                                                    SafeDelete("PorosiaArt", "IDPorosia,IDArtikulli");
                                                    break;
                                                }
                                            case "LEVIZJET_HEADER":
                                                {
                                                    SafeDelete("LEVIZJET_HEADER", "TransferID");
                                                    Snc.ExecuteServerQuery("UPDATE LEVIZJET_HEADER set ImpStatus = 0 where Depo = '" + SyncParams.DeviceID.Replace("-", "") + "' AND ImpStatus =2");
                                                    break;
                                                }
                                            case "LEVIZJET_DETAILS":
                                                {
                                                    SafeDelete("LEVIZJET_DETAILS", "Numri_Levizjes,IDArtikulli");
                                                    break;
                                                }
                                            case "Krijimi_Porosive":
                                                {
                                                    SafeDelete("Krijimi_Porosive", "KPID");
                                                    break;
                                                }
                                            case "Orders":
                                                {
                                                    Snc.ExecuteServerQuery("UPDATE Orders set ImpStatus = 0 where DeviceID = '" + SyncParams.DeviceID + "' AND ImpStatus =2 ");
                                                    break;
                                                }
                                            case "Order_Details":
                                                {
                                                    Snc.ExecuteServerQuery("UPDATE d set ImpStatus = 0 from Order_Details d inner join Orders o on o.IDOrder = d.IDOrder where o.DeviceID = '" + SyncParams.DeviceID + "' AND d.ImpStatus =2");
                                                    break;
                                                }

                                        }
                                    }

                                    break;
                                }
                            case MobSellSyncDirection.Bidirectional: //Download and Upload with Update and insert (bidirectional)
                                {
                                    //***Upload***
                                    SyncSuccesful = Snc.SyncTable(strTableName, KeyFields, PnSyncDirection.Upload, strFilterUp, "SyncStatus", "");
                                    LiveLog(SyncParams, strbSyncLog, strTableName, SyncSuccesful, MobSyncDirection, con, strSessionID);

                                    if (strTableName == "EvidencaPagesave")
                                    {
                                        SafeDelete("EvidencaPagesave", "NrPageses");

                                    }
                                    //****Now download****
                                    SyncSuccesful = Snc.SyncTable(strTableName, KeyFields, PnSyncDirection.Download, strFilterDwn, "SyncStatus", "");
                                    LiveLog(SyncParams, strbSyncLog, strTableName, SyncSuccesful, MobSyncDirection, con, strSessionID);
                                    break;
                                }

                            case MobSellSyncDirection.BidirectionalSpecial: //this Downloads to Device (insert,update) to spec. table, Uploads to tableName+"_upl" (insert,update)
                                {
                                    //***Upload***SEND DATA TO SPECIFIED TABLENAME+"_upl" WITH INSERT AND UPDATE
                                    SyncSendSuccessful = Snc.SendData(strTableName, strTableName + "_upl", KeyFields, strFilterUp, "SyncStatus");
                                    //****Now download**** NORMAL DOWNLOAD WITH INSERT AND UPDATE
                                    SyncSuccesful = Snc.SyncTable(strTableName, KeyFields, PnSyncDirection.Download, strFilterDwn, "SyncStatus", "");
                                    break;
                                }
                        } //END SWRITCH MobSellDirection
                        #endregion

                        FixKufijtFaturave(SyncSuccesful, strTableName, con, ref _CurrNrFat, ref _CurrNrFatJT, ref _CurrNrFat_D);

                        #region LiveLogProcedures


                        if (MobSyncDirection != MobSellSyncDirection.Bidirectional)
                        {
                            LiveLog(SyncParams, strbSyncLog, strTableName, SyncSuccesful, MobSyncDirection, con, strSessionID);
                        }

                        fOut.Write(strbSyncLog.ToString());
                        if ((!SyncSuccesful) || (!SyncSendSuccessful))
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Sinkronizimi deshtoi" + (char)13 + "Tabela në procesim [" + strTableName + "]");
                            break;
                        }

                        #endregion

                    }//******************FOR EACH TABLE IN SyncConfig END*******************************************             
                    #endregion

                    #region SendLogToRemoteServer

                    strbSyncLog = null;
                    //InserLog END SESSION                              
                    InsertLog(SyncParams.DeviceID, strSessionID, "", DateTime.Now.ToString("yyyy.MM.dd hh:mm:ss"), "END_SESSION", "", "", "", "", "", "", "", con);

                    KeyFields = ("SessionID").Split(',');

                    ////Now after sync Complete try to send the log to Server and delete in local if BOTH sync processes went OK
                    if ((Snc.SyncTable("LogSync_Session", KeyFields, PnSyncDirection.Upload, "1=1", "0", "")) &&
                    (Snc.SyncTable("Log_SyncErrors", KeyFields, PnSyncDirection.Upload, "1=1", "0", "")))
                    {
                        DeleteFromTable("LogSync_Session", con, "");
                        DeleteFromTable("Log_SyncErrors", con, "");

                    }
                    #endregion

                    #region CompactLocalDB

                    //daKonfig.SelectCommand.CommandText = "select DBPassword from Konfigurimi where DeviceID='" + SyncParams.DeviceID + "'";
                    //dtKonfig.Clear();
                    //daKonfig.Fill(dtKonfig);
                    //if (dtKonfig.Rows.Count == 0) { MessageBox.Show("Mungon Passwordi i bazes \n Sinkronizimi deshtoi"); Cursor.Current = Cursors.Default; return; }

                    //SyncParams.LocalDbPassword = dtKonfig.Rows[0]["DBPassword"].ToString();
                    //ConfigXpn.WriteConfiguration(SyncParams);

                    GC.Collect();
                    //using (SqlCeEngine engDb = new SqlCeEngine(ConnString))
                    //{
                    //    try
                    //    {
                    //        engDb.Shrink();
                    //        engDb.Compact("Data Source=" + SyncParams.LocalDbPath + ";Password=" + SyncParams.LocalDbPassword);
                    //    }
                    //    catch (SqlCeException ex) { DbUtils.WriteSQLCeErrorLog(ex); DbUtils.WriteErrorLog("Gabim gjate vendosjes se password-it ne DB: "); }
                    //    catch (Exception ex) { DbUtils.WriteExeptionErrorLog(ex); }
                    //}

                    #endregion

                    #region Smart Delete

                    if (SyncSuccesful)
                    {
                        GC.Collect();
                        //if (con.State == ConnectionState.Open) { con.Close(); con.Dispose(); }
                        //GC.Collect();
                        //con.ConnectionString = ConnString + ";Password=" + SyncParams.LocalDbPassword;
                        //Nese i kemi marre stoqet ne PSion i fshime ne Server 

                        int lifcount = (int)PnFunctions.CountRows("Liferimi", con);
                        //UpdateTable("Vizitat", "IDStatusiVizites", 1, con);
                        //ExecuteSql("Update Vizitat set IDStatusiVizites =1 ", con);

                        if ((int)PnFunctions.CountRows("Malli_Mbetur", con) > 0)
                        {
                            int _shitur = (int)PnFunctions.SumDataColumn("Malli_Mbetur", "SasiaShitur", con);
                            int _kthyer = (int)PnFunctions.SumDataColumn("Malli_Mbetur", "SasiaKthyer", con);
                            if (_shitur == 0 && _kthyer == 0)
                            {
                                if (_stoqetServer != 0) //Nese ka stoke per ngarkim
                                {
                                    //DeleteFromTable("Malli_Mbetur", con, "");
                                    //CopyStock(con);
                                }
                            }
                            else
                            {
                                if (_stoqetServer != 0) //Nese ka stoke per ringarkim
                                {
                                    Merge_Malli_Mbetur_Stoku(con);
                                }
                            }
                        }
                        else
                        {
                            CopyStock(con);
                        }

                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Fund i sinkronizimit");
                        DbUtils.ExecSql("Update Orders set SyncStatus = 1 ");
                        DbUtils.ExecSql("Update Order_Details set SyncStatus = 1 ");
                    }

                    #endregion

                    lblProgress.Visible = false;
                    pbProgress.Value = 0;
                    fOut.WriteLine("FinishTime=" + DateTime.Now);

                } //try
                catch (SqlCeException ex)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(ex.Message);
                    DbUtils.WriteSQLCeErrorLog(ex);
                }
                catch (Exception ex)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(ex.Message);
                    DbUtils.WriteExeptionErrorLog(ex);
                }
                finally
                {
                    fOut.Close();
                    fOut.Dispose();
                    Cursor.Current = Cursors.Default;
                }
            }
            #endregion


        }  //**********************************SYNCALL METHOD END*********************************************

        //**********************************SYNCKLIENTET METHOD (Application Specific)*********************************************
        private void SyncKlientet(Label lblProgress, ProgressBar pbProgress)
        {

            #region InitializeSync
            GC.Collect();
            ConfigXpnParams SyncParams;
            ConfigXpn.ReadConfiguration(out SyncParams);
            Sync Snc = new Sync(SyncParams.LocalDbPath,
                                SyncParams.LocalDbPassword,
                                SyncParams.ServerName,
                                SyncParams.MobileServerUrl,
                                SyncParams.DbName,
                                SyncParams.UserName,
                                SyncParams.Password);

            StringBuilder strbSyncLog = new StringBuilder();//used for Log in TextBox Control
            string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            string[] KeyFields = null;//Used in sync
            string strFilterDwn = "1=1";//Used in sync
            string strFilterUp = "1=1"; //Used in sync           
            string strTableName = "";//Used in sync            

            int _CurrNrFat = 0; //used for fixNrKufijt
            int _CurrNrFatJT = 0;//used for fixNrKufijt
            int _CurrNrFat_D = 0; //used for fixNrKufijt

            bool SyncSuccesful = true; //Used in sync
            bool SyncSendSuccessful = true; //Used in Sync
            MobSellSyncDirection MobSyncDirection; //Used in Sync
            int _stoqetServer = 0;


            string ConnString = "Data Source=" + SyncParams.LocalDbPath; //Used in SqlCeConnections below
            if (SyncParams.LocalDbPassword.Trim() != "")
                ConnString += ";Password=" + SyncParams.LocalDbPassword;

            GC.Collect();


            //*****************Configs SYNC******************************************************************
            lblProgress.Text = "Inicializimi...";
            Application.DoEvents();

            SyncConfigSynced = Snc.SyncTable("SyncConfiguration", null, PnSyncDirection.SnapshotDownload, "", "", "");
            //MessageBox.Show("SyncConfigSynced " + SyncConfigSynced.ToString());
            ConfigSynced = Snc.SyncTable("Konfigurimi", null, PnSyncDirection.SnapshotDownload, "", "", "");
            // MessageBox.Show("ConfigSynced " + ConfigSynced.ToString());
            if ((!ConfigSynced) || (!SyncConfigSynced))
            {
                MessageBox.Show("Sinkronizimi nuk mund te vazhdoje [Komunikimi me SqlMobileServer deshtoi!]", "Gabim", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                DbUtils.WriteErrorLog(PnSync.Sync.SyncLog.ErrorMsg);
                DbUtils.WriteErrorLog(PnSync.Sync.SyncLog.ErrorCode);

                lblProgresi.Text = "";
                lblProgresi.Visible = false;
                Cursor.Current = Cursors.Default;
                return;
            }

            #endregion


            #region GetDepoDeviceIDAgent
            //*********************get Depo and DeviceID and IDAgent****************************************
            using (SqlCeConnection con = new SqlCeConnection(ConnString))
            using (SqlCeDataAdapter daKonfig = new SqlCeDataAdapter("", con))
            using (DataTable dtKonfig = new DataTable())
            using (DataSet ds = new DataSet())
            {
                try
                {
                    daKonfig.SelectCommand.CommandText = "select IDAgent from Konfigurimi where DeviceID='" + SyncParams.DeviceID + "'";
                    daKonfig.Fill(dtKonfig);
                    if (dtKonfig.Rows.Count > 0)
                    {
                        strIDAgjenti = dtKonfig.Rows[0]["IDAgent"].ToString();
                    }
                    daKonfig.SelectCommand.CommandText = "select Depo from Konfigurimi where DeviceID='" + SyncParams.DeviceID + "'";
                    dtKonfig.Clear();
                    daKonfig.Fill(dtKonfig);

                    if (dtKonfig.Rows.Count > 0)
                    {
                        strDepo = dtKonfig.Rows[0]["Depo"].ToString().Trim();
                    }
                }
                catch (SqlCeException a) { DbUtils.WriteSQLCeErrorLog(a); Cursor.Current = Cursors.Default; }
                catch (Exception a) { DbUtils.WriteExeptionErrorLog(a); Cursor.Current = Cursors.Default; }

                daKonfig.SelectCommand.CommandText = "select * from SyncConfiguration where ID in (2,3,4) order by SyncOrder Asc";



                StreamWriter fOut = new StreamWriter(CurrentDir + "\\log.txt");
                fOut.WriteLine("StartTime=" + DateTime.Now);

                //*********************Sync Tables***************************************************************
                try
                {

                    daKonfig.Fill(ds);
                    if (ds.Tables[0].Rows.Count < 1)
                    {
                        MessageBox.Show("Nuk ka tabela per sinkronizim ne SyncConfig");
                        return;
                    }

                    lblProgress.Visible = true;
                    pbProgress.Maximum = ds.Tables[0].Rows.Count;
                    pbProgress.Value = 1;
                    Application.DoEvents();

                    string strSessionID = System.Guid.NewGuid().ToString();


                    //InserLog START SESSION
                    InsertLog(SyncParams.DeviceID, strSessionID, DateTime.Now.ToString("yyyy.MM.dd hh:mm:ss"), "", "START_SESSION", "", "", "", "", "", "", "", con);

                    #region FOREACH_TABLE_IN_SYNCCONFIG
                    //******************FOR EACH TABLE IN SyncConfig*******************************************
                    double ProgressPercent = 0;

                    //this var is used later to know which table has succeeded or failed, number of uploaded down. insert or updt records.
                    //SyncInfo = SyncRecords.GetTables(con.ConnectionString); 

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        MobSyncDirection = (MobSellSyncDirection)Convert.ToInt16(ds.Tables[0].Rows[i]["SyncDirection"]);
                        strTableName = ds.Tables[0].Rows[i]["TableName"].ToString();
                        KeyFields = ds.Tables[0].Rows[i]["PK_FieldName"].ToString().Split(',');//get PK_Field1,PK_Field2 in array, if more than 1                    

                        #region BuildFilters
                        //******************Filters*******************************************
                        //if FilterUp FilterDown, are null, make 1=1, which is no filter just to fill the gap

                        strFilterUp = ds.Tables[0].Rows[i]["FilterUp"].ToString();
                        strFilterDwn = ds.Tables[0].Rows[i]["FilterDwn"].ToString();

                        if (strFilterUp.Trim() == "")
                            strFilterUp = "1=1";

                        if (strFilterDwn.Trim() == "")
                            strFilterDwn = "1=1";

                        switch (strTableName)
                        {

                            case "KlientDheLokacion":
                                {
                                    strFilterDwn = "(Depo='" + strDepo + "' OR Depo='' OR Depo IS NULL)";
                                    strFilterUp = "(Depo='" + strDepo + "' OR Depo='' OR Depo IS NULL)";
                                    break;
                                }
                            case "Klientet":
                                {
                                    strFilterDwn = "(Depo='" + strDepo + "' OR Depo='' OR Depo IS NULL)";
                                    strFilterUp = "(Depo='" + strDepo + "' OR Depo='' OR Depo IS NULL)";
                                    break;
                                }
                            case "Vizitat":
                                {
                                    string AddUPFilter = ds.Tables[0].Rows[i]["FilterUp"].ToString();
                                    string AddDwnFilter = ds.Tables[0].Rows[i]["FilterDwn"].ToString();

                                    strFilterDwn = "DeviceID='" + SyncParams.DeviceID + "'";
                                    strFilterUp = "DeviceID='" + SyncParams.DeviceID + "'";

                                    if (AddDwnFilter.Trim() != "")
                                        strFilterDwn += " AND '" + AddDwnFilter + "'";
                                    if (AddUPFilter.Trim() != "") //if Filter from SyncCOnfig not empty
                                        strFilterUp += " AND '" + AddUPFilter + "'";

                                    break;
                                }

                        }

                        #endregion

                        #region RefreshSyncProgress

                        ProgressPercent = (((i + 1) * 100) / pbProgress.Maximum);
                        ProgressPercent = Math.Round(ProgressPercent, 2);
                        lblProgress.Text = "Sinkronizimi [" + ProgressPercent.ToString() + "%]" + strTableName + "...";
                        pbProgress.Value = i + 1;
                        textBox1.Text = strbSyncLog.ToString(); //logText
                        if (textBox1.Text.Length > 0)
                        {
                            textBox1.SelectionStart = textBox1.Text.Length - 1;
                            textBox1.SelectionLength = 1;
                        }
                        textBox1.ScrollToCaret();
                        Application.DoEvents();

                        #endregion

                        //SaveValuesKufijtFaturave(ref strFilterUp, strTableName, con, ref _CurrNrFat, ref _CurrNrFatJT, ref _CurrNrFat_D);

                        #region DoPnSync

                        switch (MobSyncDirection)
                        {

                            case MobSellSyncDirection.SnapshotSpecial:
                                {
                                    SyncSuccesful = Snc.SyncTable(strTableName, KeyFields, PnSyncDirection.SnapshotSpecial, strFilterDwn, "SyncStatus", strDepo);
                                   
                                    break;
                                }

                            case MobSellSyncDirection.Snapshot:
                                {
                                    SyncSuccesful = Snc.SyncTable(strTableName, null, PnSyncDirection.SnapshotDownload, strFilterUp, "", "");
                                    break;
                                }

                            case MobSellSyncDirection.Download:  //download with insert and Update
                                {
                                    SyncSuccesful = Snc.SyncTable(strTableName, KeyFields, PnSyncDirection.Download, strFilterDwn, "SyncStatus", "");
                                    break;
                                }
                            case MobSellSyncDirection.Upload: //upload with insert and update
                                {
                                    string StatusField = "SyncStatus";
                                 
                                        KeyFields = null;
                                    
                                    SyncSuccesful = Snc.SyncTable(strTableName, KeyFields, PnSyncDirection.Upload, strFilterUp, StatusField, "");

                                    break;
                                }
                            case MobSellSyncDirection.Bidirectional: //Download and Upload with Update and insert (bidirectional)
                                {
                                    //***Upload***
                                    SyncSuccesful = Snc.SyncTable(strTableName, KeyFields, PnSyncDirection.Upload, strFilterUp, "SyncStatus", "");
                                    LiveLog(SyncParams, strbSyncLog, strTableName, SyncSuccesful, MobSyncDirection, con, strSessionID);

                                    //****Now download****
                                    SyncSuccesful = Snc.SyncTable(strTableName, KeyFields, PnSyncDirection.Download, strFilterDwn, "SyncStatus", "");
                                    LiveLog(SyncParams, strbSyncLog, strTableName, SyncSuccesful, MobSyncDirection, con, strSessionID);
                                    break;
                                }

                            case MobSellSyncDirection.BidirectionalSpecial: //this Downloads to Device (insert,update) to spec. table, Uploads to tableName+"_upl" (insert,update)
                                {
                                    //***Upload***SEND DATA TO SPECIFIED TABLENAME+"_upl" WITH INSERT AND UPDATE
                                    SyncSendSuccessful = Snc.SendData(strTableName, strTableName + "_upl", KeyFields, strFilterUp, "SyncStatus");
                                    //****Now download**** NORMAL DOWNLOAD WITH INSERT AND UPDATE
                                    SyncSuccesful = Snc.SyncTable(strTableName, KeyFields, PnSyncDirection.Download, strFilterDwn, "SyncStatus", "");
                                    break;
                                }
                        } //END SWRITCH MobSellDirection
                        #endregion

                        FixKufijtFaturave(SyncSuccesful, strTableName, con, ref _CurrNrFat, ref _CurrNrFatJT, ref _CurrNrFat_D);

                        #region LiveLogProcedures


                        if (MobSyncDirection != MobSellSyncDirection.Bidirectional)
                        {
                            LiveLog(SyncParams, strbSyncLog, strTableName, SyncSuccesful, MobSyncDirection, con, strSessionID);
                        }

                        fOut.Write(strbSyncLog.ToString());
                        if ((!SyncSuccesful) || (!SyncSendSuccessful))
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Sinkronizimi deshtoi" + (char)13 + "Tabela në procesim [" + strTableName + "]");
                            break;
                        }

                        #endregion

                    }//******************FOR EACH TABLE IN SyncConfig END*******************************************             
                    #endregion

                    #region SendLogToRemoteServer

                    strbSyncLog = null;
                    //InserLog END SESSION                              
                    InsertLog(SyncParams.DeviceID, strSessionID, "", DateTime.Now.ToString("yyyy.MM.dd hh:mm:ss"), "END_SESSION", "", "", "", "", "", "", "", con);

                    KeyFields = ("SessionID").Split(',');

                    ////Now after sync Complete try to send the log to Server and delete in local if BOTH sync processes went OK
                    if ((Snc.SyncTable("LogSync_Session", KeyFields, PnSyncDirection.Upload, "1=1", "0", "")) &&
                    (Snc.SyncTable("Log_SyncErrors", KeyFields, PnSyncDirection.Upload, "1=1", "0", "")))
                    {
                        DeleteFromTable("LogSync_Session", con, "");
                        DeleteFromTable("Log_SyncErrors", con, "");

                    }
                    #endregion

                    #region CompactLocalDB

                    GC.Collect();

                    #endregion

                    #region Smart Delete

                    if (SyncSuccesful)
                    {
                        GC.Collect();
                        
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Fund i sinkronizimit");
                    }

                    #endregion

                    lblProgress.Visible = false;
                    pbProgress.Value = 0;
                    fOut.WriteLine("FinishTime=" + DateTime.Now);

                } //try
                catch (SqlCeException ex)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(ex.Message);
                    DbUtils.WriteSQLCeErrorLog(ex);
                }
                catch (Exception ex)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(ex.Message);
                    DbUtils.WriteExeptionErrorLog(ex);
                }
                finally
                {
                    fOut.Close();
                    fOut.Dispose();
                    Cursor.Current = Cursors.Default;
                }
            }
            #endregion


        }  //**********************************SYNCKLIENTET METHOD END*********************************************


        //**********************************SYNCCMIMORET METHOD (Application Specific)*********************************************
        private void SyncCmimoret(Label lblProgress, ProgressBar pbProgress)
        {

            #region InitializeSync
            GC.Collect();
            ConfigXpnParams SyncParams;
            ConfigXpn.ReadConfiguration(out SyncParams);
            Sync Snc = new Sync(SyncParams.LocalDbPath,
                                SyncParams.LocalDbPassword,
                                SyncParams.ServerName,
                                SyncParams.MobileServerUrl,
                                SyncParams.DbName,
                                SyncParams.UserName,
                                SyncParams.Password);

            StringBuilder strbSyncLog = new StringBuilder();//used for Log in TextBox Control
            string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            string[] KeyFields = null;//Used in sync
            string strFilterDwn = "1=1";//Used in sync
            string strFilterUp = "1=1"; //Used in sync           
            string strTableName = "";//Used in sync            

            int _CurrNrFat = 0; //used for fixNrKufijt
            int _CurrNrFatJT = 0;//used for fixNrKufijt
            int _CurrNrFat_D = 0; //used for fixNrKufijt

            bool SyncSuccesful = true; //Used in sync
            bool SyncSendSuccessful = true; //Used in Sync
            MobSellSyncDirection MobSyncDirection; //Used in Sync
            int _stoqetServer = 0;


            string ConnString = "Data Source=" + SyncParams.LocalDbPath; //Used in SqlCeConnections below
            if (SyncParams.LocalDbPassword.Trim() != "")
                ConnString += ";Password=" + SyncParams.LocalDbPassword;

            GC.Collect();


            //*****************Configs SYNC******************************************************************
            lblProgress.Text = "Inicializimi...";
            Application.DoEvents();

            SyncConfigSynced = Snc.SyncTable("SyncConfiguration", null, PnSyncDirection.SnapshotDownload, "", "", "");
            //MessageBox.Show("SyncConfigSynced " + SyncConfigSynced.ToString());
            ConfigSynced = Snc.SyncTable("Konfigurimi", null, PnSyncDirection.SnapshotDownload, "", "", "");
            // MessageBox.Show("ConfigSynced " + ConfigSynced.ToString());
            if ((!ConfigSynced) || (!SyncConfigSynced))
            {
                MessageBox.Show("Sinkronizimi nuk mund te vazhdoje [Komunikimi me SqlMobileServer deshtoi!]", "Gabim", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                DbUtils.WriteErrorLog(PnSync.Sync.SyncLog.ErrorMsg);
                DbUtils.WriteErrorLog(PnSync.Sync.SyncLog.ErrorCode);

                lblProgresi.Text = "";
                lblProgresi.Visible = false;
                Cursor.Current = Cursors.Default;
                return;
            }

            #endregion


            #region GetDepoDeviceIDAgent
            //*********************get Depo and DeviceID and IDAgent****************************************
            using (SqlCeConnection con = new SqlCeConnection(ConnString))
            using (SqlCeDataAdapter daKonfig = new SqlCeDataAdapter("", con))
            using (DataTable dtKonfig = new DataTable())
            using (DataSet ds = new DataSet())
            {
                try
                {
                    daKonfig.SelectCommand.CommandText = "select IDAgent from Konfigurimi where DeviceID='" + SyncParams.DeviceID + "'";
                    daKonfig.Fill(dtKonfig);
                    if (dtKonfig.Rows.Count > 0)
                    {
                        strIDAgjenti = dtKonfig.Rows[0]["IDAgent"].ToString();
                    }
                    daKonfig.SelectCommand.CommandText = "select Depo from Konfigurimi where DeviceID='" + SyncParams.DeviceID + "'";
                    dtKonfig.Clear();
                    daKonfig.Fill(dtKonfig);

                    if (dtKonfig.Rows.Count > 0)
                    {
                        strDepo = dtKonfig.Rows[0]["Depo"].ToString().Trim();
                    }
                }
                catch (SqlCeException a) { DbUtils.WriteSQLCeErrorLog(a); Cursor.Current = Cursors.Default; }
                catch (Exception a) { DbUtils.WriteExeptionErrorLog(a); Cursor.Current = Cursors.Default; }

                daKonfig.SelectCommand.CommandText = "select * from SyncConfiguration where ID in (15) order by SyncOrder Asc";



                StreamWriter fOut = new StreamWriter(CurrentDir + "\\log.txt");
                fOut.WriteLine("StartTime=" + DateTime.Now);

                //*********************Sync Tables***************************************************************
                try
                {

                    daKonfig.Fill(ds);
                    if (ds.Tables[0].Rows.Count < 1)
                    {
                        MessageBox.Show("Nuk ka tabela per sinkronizim ne SyncConfig");
                        return;
                    }

                    lblProgress.Visible = true;
                    pbProgress.Maximum = ds.Tables[0].Rows.Count;
                    pbProgress.Value = 1;
                    Application.DoEvents();

                    string strSessionID = System.Guid.NewGuid().ToString();


                    //InserLog START SESSION
                    InsertLog(SyncParams.DeviceID, strSessionID, DateTime.Now.ToString("yyyy.MM.dd hh:mm:ss"), "", "START_SESSION", "", "", "", "", "", "", "", con);

                    #region FOREACH_TABLE_IN_SYNCCONFIG
                    //******************FOR EACH TABLE IN SyncConfig*******************************************
                    double ProgressPercent = 0;

                    //this var is used later to know which table has succeeded or failed, number of uploaded down. insert or updt records.
                    //SyncInfo = SyncRecords.GetTables(con.ConnectionString); 

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        MobSyncDirection = (MobSellSyncDirection)Convert.ToInt16(ds.Tables[0].Rows[i]["SyncDirection"]);
                        strTableName = ds.Tables[0].Rows[i]["TableName"].ToString();
                        KeyFields = ds.Tables[0].Rows[i]["PK_FieldName"].ToString().Split(',');//get PK_Field1,PK_Field2 in array, if more than 1                    

                        #region BuildFilters
                        //******************Filters*******************************************
                        //if FilterUp FilterDown, are null, make 1=1, which is no filter just to fill the gap

                        strFilterUp = ds.Tables[0].Rows[i]["FilterUp"].ToString();
                        strFilterDwn = ds.Tables[0].Rows[i]["FilterDwn"].ToString();

                        if (strFilterUp.Trim() == "")
                            strFilterUp = "1=1";

                        if (strFilterDwn.Trim() == "")
                            strFilterDwn = "1=1";

                        switch (strTableName)
                        {
                            case "SalesPrice":
                                {
                                    string getOnlyThisWeekKlients = @"SalesCode IN 
                                                                    (  SELECT IDKlientDheLokacion FROM Vizitat WHERE IDAgjenti ='" + strIDAgjenti + @"' ) OR SalesCode='STANDARD' ";
                                    strFilterDwn = "(Depo='" + strDepo + "' OR Depo='' OR Depo IS NULL) and (" + getOnlyThisWeekKlients + ")";
                                    strFilterUp = "(Depo='" + strDepo + "' OR Depo='' OR Depo IS NULL) and (" + getOnlyThisWeekKlients + ")";
                                    break;
                                }
                        }

                        #endregion

                        #region RefreshSyncProgress

                        ProgressPercent = (((i + 1) * 100) / pbProgress.Maximum);
                        ProgressPercent = Math.Round(ProgressPercent, 2);
                        lblProgress.Text = "Sinkronizimi [" + ProgressPercent.ToString() + "%]" + strTableName + "...";
                        pbProgress.Value = i + 1;
                        textBox1.Text = strbSyncLog.ToString(); //logText
                        if (textBox1.Text.Length > 0)
                        {
                            textBox1.SelectionStart = textBox1.Text.Length - 1;
                            textBox1.SelectionLength = 1;
                        }
                        textBox1.ScrollToCaret();
                        Application.DoEvents();

                        #endregion

                        //SaveValuesKufijtFaturave(ref strFilterUp, strTableName, con, ref _CurrNrFat, ref _CurrNrFatJT, ref _CurrNrFat_D);

                        #region DoPnSync

                        switch (MobSyncDirection)
                        {

                            case MobSellSyncDirection.SnapshotSpecial:
                                {
                                    SyncSuccesful = Snc.SyncTable(strTableName, KeyFields, PnSyncDirection.SnapshotSpecial, strFilterDwn, "SyncStatus", strDepo);

                                    break;
                                }

                            case MobSellSyncDirection.Snapshot:
                                {
                                    SyncSuccesful = Snc.SyncTable(strTableName, null, PnSyncDirection.SnapshotDownload, strFilterUp, "", "");
                                    break;
                                }

                            case MobSellSyncDirection.Download:  //download with insert and Update
                                {
                                    SyncSuccesful = Snc.SyncTable(strTableName, KeyFields, PnSyncDirection.Download, strFilterDwn, "SyncStatus", "");
                                    break;
                                }
                            case MobSellSyncDirection.Upload: //upload with insert and update
                                {
                                    string StatusField = "SyncStatus";
                                    KeyFields = null;
                                    SyncSuccesful = Snc.SyncTable(strTableName, KeyFields, PnSyncDirection.Upload, strFilterUp, StatusField, "");

                                    break;
                                }
                            case MobSellSyncDirection.Bidirectional: //Download and Upload with Update and insert (bidirectional)
                                {
                                    //***Upload***
                                    SyncSuccesful = Snc.SyncTable(strTableName, KeyFields, PnSyncDirection.Upload, strFilterUp, "SyncStatus", "");
                                    LiveLog(SyncParams, strbSyncLog, strTableName, SyncSuccesful, MobSyncDirection, con, strSessionID);

                                    //****Now download****
                                    SyncSuccesful = Snc.SyncTable(strTableName, KeyFields, PnSyncDirection.Download, strFilterDwn, "SyncStatus", "");
                                    LiveLog(SyncParams, strbSyncLog, strTableName, SyncSuccesful, MobSyncDirection, con, strSessionID);
                                    break;
                                }

                            case MobSellSyncDirection.BidirectionalSpecial: //this Downloads to Device (insert,update) to spec. table, Uploads to tableName+"_upl" (insert,update)
                                {
                                    //***Upload***SEND DATA TO SPECIFIED TABLENAME+"_upl" WITH INSERT AND UPDATE
                                    SyncSendSuccessful = Snc.SendData(strTableName, strTableName + "_upl", KeyFields, strFilterUp, "SyncStatus");
                                    //****Now download**** NORMAL DOWNLOAD WITH INSERT AND UPDATE
                                    SyncSuccesful = Snc.SyncTable(strTableName, KeyFields, PnSyncDirection.Download, strFilterDwn, "SyncStatus", "");
                                    break;
                                }
                        } //END SWRITCH MobSellDirection
                        #endregion

                        FixKufijtFaturave(SyncSuccesful, strTableName, con, ref _CurrNrFat, ref _CurrNrFatJT, ref _CurrNrFat_D);

                        #region LiveLogProcedures


                        if (MobSyncDirection != MobSellSyncDirection.Bidirectional)
                        {
                            LiveLog(SyncParams, strbSyncLog, strTableName, SyncSuccesful, MobSyncDirection, con, strSessionID);
                        }

                        fOut.Write(strbSyncLog.ToString());
                        if ((!SyncSuccesful) || (!SyncSendSuccessful))
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Sinkronizimi deshtoi" + (char)13 + "Tabela në procesim [" + strTableName + "]");
                            break;
                        }

                        #endregion

                    }//******************FOR EACH TABLE IN SyncConfig END*******************************************             
                    #endregion

                    #region SendLogToRemoteServer

                    strbSyncLog = null;
                    //InserLog END SESSION                              
                    InsertLog(SyncParams.DeviceID, strSessionID, "", DateTime.Now.ToString("yyyy.MM.dd hh:mm:ss"), "END_SESSION", "", "", "", "", "", "", "", con);

                    KeyFields = ("SessionID").Split(',');

                    ////Now after sync Complete try to send the log to Server and delete in local if BOTH sync processes went OK
                    if ((Snc.SyncTable("LogSync_Session", KeyFields, PnSyncDirection.Upload, "1=1", "0", "")) &&
                    (Snc.SyncTable("Log_SyncErrors", KeyFields, PnSyncDirection.Upload, "1=1", "0", "")))
                    {
                        DeleteFromTable("LogSync_Session", con, "");
                        DeleteFromTable("Log_SyncErrors", con, "");

                    }
                    #endregion

                    #region CompactLocalDB

                    GC.Collect();

                    #endregion

                    #region Smart Delete

                    if (SyncSuccesful)
                    {
                        GC.Collect();

                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Fund i sinkronizimit");
                    }

                    #endregion

                    lblProgress.Visible = false;
                    pbProgress.Value = 0;
                    fOut.WriteLine("FinishTime=" + DateTime.Now);

                } //try
                catch (SqlCeException ex)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(ex.Message);
                    DbUtils.WriteSQLCeErrorLog(ex);
                }
                catch (Exception ex)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(ex.Message);
                    DbUtils.WriteExeptionErrorLog(ex);
                }
                finally
                {
                    fOut.Close();
                    fOut.Dispose();
                    Cursor.Current = Cursors.Default;
                }
            }
            #endregion


        }  //**********************************SYNCCMIMORET METHOD END*********************************************

        //this procedure writes a log in textFile displays it live in TextBox, and also Inserts in DB
        private void LiveLog(ConfigXpnParams SyncParams, StringBuilder strbSyncLog, string strTableName, bool SyncSuccesful, MobSellSyncDirection MobSyncDirection, SqlCeConnection con, string strSessionID)
        {
            strbSyncLog.AppendLine(Sync.SyncLog.TableName + ",Direction=" + MobSyncDirection.ToString() + ",Downloaded=" + Sync.SyncLog.DownloadedRecCount +
            ",Inserted=" + Sync.SyncLog.Inserted + ",Updated=" + Sync.SyncLog.Updated + ",Uploaded=" +
            Sync.SyncLog.UploadedRecCount);

            InsertLog
            (
              SyncParams.DeviceID, strSessionID, "", "", strTableName, Sync.SyncLog.StartTime, Sync.SyncLog.EndTime,
              ((int)MobSyncDirection).ToString(), Sync.SyncLog.DownloadedRecCount, Sync.SyncLog.Updated, Sync.SyncLog.Inserted,
              Sync.SyncLog.UploadedRecCount, con
             );

            if (!SyncSuccesful)
            {
                InsertErrorLog(SyncParams.DeviceID, strSessionID, Sync.SyncLog.ErrorCode,
                    Sync.SyncLog.ErrorMsg, Sync.SyncLog.ErrorModule, "", con);
            }
        }

        private void FixKufijtFaturave(bool SyncSuccess, string strTable, SqlCeConnection con, ref int _CurrNrFat, ref int _CurrNrFatJT, ref int _CurrNrFat_D)
        {

            if (strTable == "NumriFaturave")
            {
                string _getNRKUFIP = "SELECT nf.NRKUFIP FROM NumriFaturave nf WHERE nf.KOD='" + strDepo + "'";
                int tempNRKUFIP = int.Parse(ExecuteSqlScalar(_getNRKUFIP, con).ToString());

                string _getNRKUFIS = "SELECT nf.NRKUFIS FROM NumriFaturave nf WHERE nf.KOD='" + strDepo + "'";
                int tempNRKUFIS = int.Parse(ExecuteSqlScalar(_getNRKUFIS, con).ToString());

                string _getNRKUFIPJT = "SELECT nf.NRKUFIPJT FROM NumriFaturave nf WHERE nf.KOD='" + strDepo + "'";
                int tempNRKUFIPJT = int.Parse(ExecuteSqlScalar(_getNRKUFIPJT, con).ToString());

                string _getNRKUFISJT = "SELECT nf.NRKUFISJT FROM NumriFaturave nf WHERE nf.KOD='" + strDepo + "'";
                int tempNRKUFISJT = int.Parse(ExecuteSqlScalar(_getNRKUFISJT, con).ToString());

                string _getNRKUFIP_D = "SELECT nf.NRKUFIP_D FROM NumriFaturave nf WHERE nf.KOD='" + strDepo + "'";
                int tempNRKUFIP_D = int.Parse(ExecuteSqlScalar(_getNRKUFIP_D, con).ToString());

                string _getNRKUFIS_D = "SELECT nf.NRKUFIS_D FROM NumriFaturave nf WHERE nf.KOD='" + strDepo + "'";
                int tempNRKUFIS_D = int.Parse(ExecuteSqlScalar(_getNRKUFIS_D, con).ToString());

                if (_CurrNrFat < tempNRKUFIP || _CurrNrFat > tempNRKUFIS)
                    _CurrNrFat = 0;
                else
                    _CurrNrFat = _CurrNrFat - tempNRKUFIP;

                if (_CurrNrFatJT < tempNRKUFIPJT || _CurrNrFatJT > tempNRKUFISJT)
                    _CurrNrFatJT = 0;
                else
                    _CurrNrFatJT = _CurrNrFatJT - tempNRKUFIPJT;

                if (_CurrNrFat_D < tempNRKUFIP_D || _CurrNrFat_D > tempNRKUFIS_D)
                    _CurrNrFat_D = 0;
                else
                    _CurrNrFat_D = _CurrNrFat_D - tempNRKUFIP_D;

                string _UpdateNrFaturave = @"UPDATE NumriFaturave SET    
                                                    CurrNrFat   = " + _CurrNrFat + @", 
                                                    CurrNrFatJT = " + _CurrNrFatJT + @",
                                                    CurrNrFat_D   = " + _CurrNrFat_D + @"
                                            WHERE  KOD = '" + strDepo + "'";
                ExecuteSqlScalar(_UpdateNrFaturave, con);
            }
        }

        private void SaveValuesKufijtFaturave(ref string strFilterUp, string strTableName, SqlCeConnection con, ref int _CurrNrFat, ref int _CurrNrFatJT, ref int _CurrNrFat_D)
        {
            if (strTableName == "NumriFaturave")
            {
                if (int.Parse(ExecuteSqlScalar("Select count(*) FROM NumriFaturave where KOD='" + strDepo + "'", con).ToString()) == 0)
                {
                    //Do SnapShot
                    strFilterUp = "";
                    _CurrNrFat = 0;
                    _CurrNrFatJT = 0;
                }
                else
                {
                    string _getNRKUFIP = "SELECT nf.NRKUFIP FROM NumriFaturave nf WHERE nf.KOD='" + strDepo + "'";
                    string _getNRKUFIPJT = "SELECT nf.NRKUFIPJT FROM NumriFaturave nf WHERE nf.KOD='" + strDepo + "'";
                    string _getNRKUFIP_D = "SELECT nf.NRKUFIP_D FROM NumriFaturave nf WHERE nf.KOD='" + strDepo + "'";

                    int tempNRKUFIP = int.Parse(ExecuteSqlScalar(_getNRKUFIP, con).ToString());
                    int tempNRKUFIPJT = int.Parse(ExecuteSqlScalar(_getNRKUFIPJT, con).ToString());
                    int tempNRKUFIP_D = int.Parse(ExecuteSqlScalar(_getNRKUFIP_D, con).ToString());


                    _CurrNrFat = int.Parse(ExecuteSqlScalar("Select CurrNrFat FROM NumriFaturave where KOD='" + strDepo + "'", con).ToString());
                    _CurrNrFat_D = int.Parse(ExecuteSqlScalar("Select CurrNrFat_D FROM NumriFaturave where KOD='" + strDepo + "'", con).ToString());
                    _CurrNrFatJT = int.Parse(ExecuteSqlScalar("Select CurrNrFatJT FROM NumriFaturave where KOD='" + strDepo + "'", con).ToString());
                    _CurrNrFat = _CurrNrFat + tempNRKUFIP;
                    _CurrNrFatJT = _CurrNrFatJT + tempNRKUFIPJT;
                    _CurrNrFat_D = _CurrNrFat_D + tempNRKUFIP_D;
                }
            }
        }

        private object ExecuteSqlScalar(string CommandText, SqlCeConnection conn)
        {
            object a = "";
            using (SqlCeCommand kom = new SqlCeCommand(CommandText, conn))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    a = kom.ExecuteScalar();
                    conn.Close();
                }
                catch (SqlCeException ex)
                {
                    DbUtils.WriteSQLCeErrorLog(ex, CommandText);
                    MessageBox.Show(ex.Message);
                }
            }
            return a;
        }

        private bool ExecuteSql(string CommandText, SqlCeConnection conn)
        {
            bool a = false;
            SqlCeCommand kom = null;
            try
            {
                kom = new SqlCeCommand(CommandText, conn);
                if (conn.State == ConnectionState.Closed) conn.Open();
                kom.ExecuteNonQuery();
                a = true;
            }
            catch (SqlCeException ex)
            {
                DbUtils.WriteSQLCeErrorLog(ex, CommandText);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                kom.Dispose();
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return a;
        }

        private void ShkarkoMalliMbetur()
        {

            try
            {
                ConfigXpnParams c;

                ConfigXpn.ReadConfiguration(out c);

                string[] KeyFields = new string[1];
                string strSessionID = new System.Guid().ToString();
                KeyFields[0] = "PKeyIDArtDepo";

                Sync MalliMbeturSync = new Sync(c.LocalDbPath,
                                                c.LocalDbPassword,
                                                c.ServerName,
                                                c.MobileServerUrl,
                                                c.DbName,
                                                c.UserName,
                                                c.Password);
                bool success = false;
                
                using (SqlCeConnection con = new SqlCeConnection(frmHome.AppDBConnectionsStr))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    btnSinkronizo.Enabled = false;
                    btnSinkronizoKlientet.Enabled = false;
                    btnSinkronizoCmimoren.Enabled = false;
                    btnShkarko.Enabled = false;

                    if (!UpdateMalli_Mbetur(con))
                    {
                        MessageBox.Show("Gabmi gjate azhurimit te [Malli_Mbetur]. Shkarkimi nuk mund te vazhdoje");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                        //@BE 02.11.2018 Rreshti Poshte eshte shtuar ashtu qe te behet se pari fshirja e mallit te mbetur dhe tek me pas edhe sinkronizimi nga pajisja
                        MalliMbeturSync.ExecuteServerQuery("DELETE FROM Malli_Mbetur where Depo = '" + c.DeviceID.Replace("-", "")+ "'" );
                        
                        //SyncSuccesful = Snc.SyncTable(strTableName, KeyFields, PnSyncDirection.Upload, strFilterUp, StatusField, "");
                        success = MalliMbeturSync.SyncTable("Malli_Mbetur", KeyFields, PnSyncDirection.UploadWithInsert, "1=1", "0", "");
                        //@BE KOMENTUAR ME 27.08.2018 eshte kthyer prap rreshti lart //bool success = MalliMbeturSync.SyncTable("Malli_Mbetur", null, PnSyncDirection.Upload, "1=1", "0", "");

                        //@BE 02.11.2018 rreshti poshte eshte komentuar dhe zevendesuar me ate pasardhes
                        //if (success && MalliMbeturSync.ExecuteServerQuery("UPDATE Malli_Mbetur SET Export_Status = 0 where Depo = '" + c.DeviceID.Replace("-", "") + "' AND Export_Status = 2 "))
                        if (success && MalliMbeturSync.ExecuteServerQuery("UPDATE Malli_Mbetur SET Export_Status = 0 where Depo = '" + c.DeviceID.Replace("-", "") + "'"))
                        {
                            if (Delete_Malli_Mbetur_Stoqet(con))
                            {

                                MessageBox.Show("Te gjitha mallrat jane shkarkuar");
                                btnSinkronizo.Enabled = true;
                                btnSinkronizoKlientet.Enabled = true;
                                btnSinkronizoCmimoren.Enabled = true;
                                Cursor.Current = Cursors.Default;

                            }
                            else
                            {
                                MessageBox.Show("Mallrat jane shkarkuar por nuk jane rifreskuar");
                                Cursor.Current = Cursors.Default;
                            }
                        }
                        else
                        {
                            if (Sync.SyncLog.ErrorCode == "-2147012889")
                            {
                                MessageBox.Show("Sinkronizimi nuk mund te vazhdoje [Komunikimi me SqlMobileServer deshtoi!]\n Tabela Malli_Mbetur ");
                            }
                            else
                            {
                                MessageBox.Show("Sinkronizimi nuk mund te vazhdoje \n Tabela: Malli_Mbetur");
                            }

                            InsertErrorLog(c.DeviceID, strSessionID, Sync.SyncLog.ErrorCode, Sync.SyncLog.ErrorMsg, Sync.SyncLog.ErrorModule, "", con);
                        }
                    Cursor.Current = Cursors.Default;

                }
            }
            catch (SqlCeException scex)
            {
                DbUtils.WriteSQLCeErrorLog(scex, "");
                MessageBox.Show("Gabim gjate shkarkimit:" + scex.Message);
            }
            catch (Exception ex)
            {
                DbUtils.WriteExeptionErrorLog(ex);
                MessageBox.Show("Gabim gjate shkarkimit:" + ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;

            }

        }

        public bool Delete_Malli_Mbetur_Stoqet(SqlCeConnection con)
        {
            bool result = false;
            using (SqlCeCommand cmd = new SqlCeCommand("", con))
            {
                try
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }

                    cmd.CommandText = "Delete  from Malli_Mbetur";
                    cmd.ExecuteNonQuery();


                    cmd.CommandText = "Delete  from Stoqet";
                    cmd.ExecuteNonQuery();

                    result = true;
                    con.Close();

                }

                catch (SqlCeException ex)
                {
                    result = false;
                    MessageBox.Show(ex.Message);
                    DbUtils.WriteSQLCeErrorLog(ex, cmd.CommandText);

                }
                catch (Exception es)
                {
                    result = false;
                    MessageBox.Show(es.Message);
                    DbUtils.WriteExeptionErrorLog(es);
                }
            }
            return result;
        }

        /// <summary>
        /// Delete query, that delete * from tabel
        /// </summary>
        /// <param name="tableName">Tabel name </param>
        /// <param name="conn">Connection</param>
        private static bool DeleteFromTable(string tableName, SqlCeConnection conn, string Condition)
        {
            bool a = false;
            using (SqlCeCommand cm = new SqlCeCommand("", conn))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) { conn.Open(); }
                    cm.CommandText = "delete from " + tableName + " " + Condition;
                    cm.ExecuteNonQuery();
                    conn.Close();
                    a = true;
                }
                catch (SqlCeException ex1)
                {
                    a = false;
                    DbUtils.WriteSQLCeErrorLog(ex1);
                }
                catch (Exception ex2)
                {
                    a = false;
                    DbUtils.WriteExeptionErrorLog(ex2);
                }
            }
            return a;
        }



        /// <summary>
        /// Update query only for column with DataType(int)
        /// </summary>
        /// <param name="tableName">Emri i tabeles</param>
        /// <param name="colName">Emri i kolones</param>
        /// <param name="value">Vlera</param>
        /// <param name="conn">Koneksioni</param>
        private static void UpdateTable(string tableName, string colName, int value, SqlCeConnection conn)
        {
            using (SqlCeCommand cm = new SqlCeCommand("", conn))
            {
                string tm = "Update " + tableName + " set " + colName + "=" + value;
                cm.CommandText = tm;
                if (cm.Connection.State == ConnectionState.Closed) { cm.Connection.Open(); }
                try
                {

                    cm.ExecuteNonQuery();
                    cm.Connection.Close();

                }
                catch (SqlCeException sx)
                {
                    DbUtils.WriteSQLCeErrorLog(sx, tm);
                    throw sx;
                }
                catch (Exception ex)
                {
                    DbUtils.WriteExeptionErrorLog(ex);
                    throw ex;
                }
            }
        }

        private static bool UpdateMalli_Mbetur(SqlCeConnection conn)
        {
            bool a = false;
            string sql = "UPDATE Malli_Mbetur SET    Data = '" + DateTime.Now.ToString("yyyy-MM-dd") + "', SyncStatus=0";
            using (SqlCeCommand cm = new SqlCeCommand(sql, conn))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) { conn.Open(); }
                    cm.ExecuteNonQuery();
                    conn.Close();
                    a = true;
                }
                catch (SqlCeException ex)
                {
                    MessageBox.Show(ex.Message);
                    PnUtils.DbUtils.WriteSQLCeErrorLog(ex, cm.CommandText);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    PnUtils.DbUtils.WriteExeptionErrorLog(ex);
                }
            }
            return a;
        }

        private static void CopyStock(SqlCeConnection conn)
        {
            string depo = "";
            string CurrentDir;
            string DevIDConfig;

            using (SqlCeCommand cm = new SqlCeCommand("", conn))
            using (DataSet dsConfig = new DataSet())
            {
                try
                {
                    CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);

                    dsConfig.ReadXml(CurrentDir + "\\Config.xpn");
                    DevIDConfig = dsConfig.Tables[0].Rows[0]["DeviceID"].ToString();
                    string sql = "Select k.Depo from Konfigurimi k where DeviceID='" + DevIDConfig + "'";

                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    cm.CommandText = sql;
                    depo = cm.ExecuteScalar().ToString();


                    cm.CommandText = "DELETE FROM MALLI_MBETUR;";
                    cm.ExecuteNonQuery();

                    string sql2 =
                               "INSERT INTO Malli_Mbetur \n"
                               + "  ( \n"
                               + "    IDArtikulli, \n"
                               + "    Emri, \n"
                               + "    SasiaPranuar, \n"
                               + "    NrDoc, \n"
                               + "    SyncStatus, \n"
                               + "    LLOJDOK, \n"
                               + "    SasiaMbetur, \n"
                               + "    Depo, \n"
                               + "    Export_Status, \n"
                               + "    PKeyIDArtDepo, \n"
                               + "    Seri"
                               + "  ) \n"
                               + "SELECT s.Shifra AS IDArtikulli, \n"
                               + "       a.Emri, \n"
                               + "       s.Sasia, \n"
                               + "       CONVERT(INT, '119' + CONVERT(NCHAR(15),s.NRDOK ) ), \n"
                               + "       0, \n"
                               + "       'KM', \n"
                               + "       s.Sasia, \n"
                               + "       s.Depo, \n"
                               + "       0, \n"
                               + "       s.Shifra+s.Depo+case when s.Seri is null then '' else s.Seri end, \n"
                               + "       s.Seri \n"
                                + "  FROM Stoqet s INNER JOIN Artikujt a ON a.IDArtikulli = s.Shifra WHERE  \n"
                               + "       s.Depo = '" + depo + "'";
                    cm.CommandText = sql2;
                    cm.ExecuteNonQuery();
                    conn.Close();

                }
                catch (SqlCeException ex)
                {
                    MessageBox.Show(ex.Message);
                    PnUtils.DbUtils.WriteSQLCeErrorLog(ex, cm.CommandText);
                }

            }


        }

        private static bool Merge_Malli_Mbetur_Stoku(SqlCeConnection conn)
        {
            SqlCeCommand cmd = null;

            bool a = false;

            try
            {
                cmd = new SqlCeCommand("", conn);
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                cmd.Transaction = conn.BeginTransaction();

                DataTable dtMalliMbetur = new DataTable();
                DataTable dtStoqet = new DataTable();
                DataTable _tempStoqet = new DataTable();

                SqlCeDataAdapter da = new SqlCeDataAdapter(@"SELECT *
                                                                   FROM   Malli_Mbetur mm
                                                                    WHERE  mm.IDArtikulli IN (SELECT Shifra
                                                                                              FROM   Stoqet s)", conn);
                da.Fill(dtMalliMbetur);

                da.SelectCommand.CommandText = "Select * from stoqet";
                da.Fill(dtStoqet);


                dtStoqet.PrimaryKey = dtMalliMbetur.PrimaryKey = null;

                dtMalliMbetur.PrimaryKey = new DataColumn[] { dtMalliMbetur.Columns[0] };
                dtStoqet.PrimaryKey = new DataColumn[] { dtStoqet.Columns[0] };
                dtStoqet.Columns[0].ColumnName = "IDArtikulli";
                dtStoqet.Merge(dtMalliMbetur);

                dtStoqet.Columns.Add(new DataColumn("Calc", typeof(decimal), "ISNULL(SasiaMbetur , 0.00)+ ISNULL(Sasia , 0.00)"));


                int count = dtStoqet.Rows.Count;
                string _updateQuery = "";
                string _insetQuery = "";
                //Update Sasia ne Stok nga MalliMbetur
                for (int i = 0; i < count; i++)
                {
                    _updateQuery = @"update stoqet set sasia = " + dtStoqet.Rows[i]["Calc"].ToString() + " where Shifra= '" + dtStoqet.Rows[i][0].ToString() + "' AND Seri = '" + dtStoqet.Rows[i]["Seri"].ToString() + "'";
                    cmd.CommandText = _updateQuery;
                    cmd.ExecuteNonQuery();
                }


                //Update MalliMbetur SasiaPranuar nga Stoku
                for (int i = 0; i < count; i++)
                {
                    //_updateQuery = @"update Malli_Mbetur set SasiaPranuar = " + dtStoqet.Rows[i]["Calc"].ToString() + " where IDArtikulli= '" + dtStoqet.Rows[i][0].ToString() + "'";
                    _updateQuery = @"update Malli_Mbetur set SasiaMbetur = " + dtStoqet.Rows[i]["Calc"].ToString() + " , SyncStatus=0 where IDArtikulli= '" + dtStoqet.Rows[i][0].ToString() + "' AND Seri = '" + dtStoqet.Rows[i]["Seri"].ToString() + "'";
                    cmd.CommandText = _updateQuery;
                    cmd.ExecuteNonQuery();
                }

                _insetQuery = @"INSERT INTO Stoqet
                                (
	                                Shifra,
	                                Depo,
	                                Sasia,	
	                                SyncStatus,
	                                Dhurate,
	                                NRDOK,
	                                LLOJDOK,
                                    Seri
                                )
                                SELECT mm.IDArtikulli,
                                        mm.Depo,
                                        mm.SasiaMbetur,
                                        0,
                                        0,
                                        SUBSTRING(CAST(mm.NrDoc AS NCHAR),4,LEN(CAST(mm.NrDoc AS NCHAR))),
                                        'FU',
                                        mm.Seri
                                FROM   Malli_Mbetur mm 
                                WHERE mm.IDArtikulli+isnull(mm.Seri,'') NOT in (SELECT Shifra+isnull(Seri,'') FROM Stoqet)
                                ";
                cmd.CommandText = _insetQuery;
                cmd.ExecuteNonQuery();

                _insetQuery = @"INSERT INTO Malli_Mbetur
                                (
	                                IDArtikulli,
	                                Emri,
	                                SasiaPranuar,
	                                SasiaShitur,
	                                SasiaKthyer,
	                                SasiaMbetur,
	                                NrDoc,
	                                SyncStatus,
                                	
	                                LLOJDOK,
	                                Depo,
	                                Export_Status,
	                                PKeyIDArtDepo,
                                    Data,
                                    Seri
                                )
                                SELECT 
	                                s.Shifra,
	                                a.Emri,
	                                s.Sasia,
	                                0,
	                                0,
	                                s.Sasia,
	                                '119'+CONVERT(NCHAR (10),s.NRDOK),
	                                0,
	                                'KM',
	                                S.Depo,
	                                0,
	                                S.Shifra+S.Depo+case when s.Seri is null then '' else s.Seri end,
                                    GetDate(),
                                    s.Seri
                                FROM Stoqet s 
                                INNER JOIN Artikujt a ON a.IDArtikulli = s.Shifra 
                                WHERE s.Shifra NOT IN (SELECT mm.IDArtikulli FROM Malli_Mbetur mm)";
                cmd.CommandText = _insetQuery;
                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();


                //_updateQuery = @"UPDATE Malli_Mbetur SET SasiaMbetur = SasiaPranuar-SasiaShitur-SasiaKthyer";
                _updateQuery = @"UPDATE Malli_Mbetur SET SasiaPranuar = SasiaShitur+SasiaKthyer+SasiaMbetur, SyncStatus=0";
                cmd.CommandText = _updateQuery;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Sinkronizimi perfundoi me sukses");
                a = true;
            }
            catch (SqlCeException ex)
            {
                a = false;
                cmd.Transaction.Rollback();
                MessageBox.Show(ex.Message);
                PnUtils.DbUtils.WriteSQLCeErrorLog(ex, cmd.CommandText);

            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
            return a;
        }

        #endregion

        private void btnSinkronizo_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            int rows = (int)PnFunctions.CountRows("Malli_Mbetur");
            if (rows > 0)
            {
                int _shitur = (int)PnFunctions.SumDataColumn("Malli_Mbetur", "SasiaShitur");
                int _kthyer = (int)PnFunctions.SumDataColumn("Malli_Mbetur", "SasiaKthyer");
                if ((double)_shitur > 0 || (double)_kthyer < 0)
                {
                    MessageBox.Show("Nuk lejohet ringarkimi i shënimeve të reja \n Së pari duhet të shkarkoni shënimet \n ekzistojn fatura të pa zëruara", "Sinkronizimi", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }
            var sync = SyncFiskalizimi.SinkronizoFiskalizimin();
            if (sync.hasError)
            {
                MessageBox.Show(sync.message);
                Cursor.Current = Cursors.Default;
                return;
            }
            if (MessageBox.Show("Ky proçes shkarkon sasitë e mbetura të artikujve nga Palmi \n dhe bën zerimin e sasive në Palm. \n  A dëshironi të vazhdoni?", "Konfirmo", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                btnSinkronizo.Enabled = false;
                btnSinkronizoKlientet.Enabled = false;
                btnSinkronizoCmimoren.Enabled = false;
                btnShkarko.Enabled = false;
                try
                {
                    if (DbUtils.MainSqlConnection != null)
                    {
                        DbUtils.MainSqlConnection.Close();
                    }
                    SyncAll(lblProgresi, progressBar1);
                }
                finally
                {
                    btnSinkronizo.Enabled = true;
                    btnSinkronizoKlientet.Enabled = true;
                    btnSinkronizoCmimoren.Enabled = true;
                    Cursor.Current = Cursors.Default;
                }

                //this function fill textBox with log
                string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
                StreamReader fReader = null;
                try
                {
                    fReader = new StreamReader(CurrentDir + "\\log.txt");
                    textBox1.Text = fReader.ReadToEnd();

                    btnSinkronizo.Enabled = true;
                    btnSinkronizoKlientet.Enabled = true;
                    btnSinkronizoCmimoren.Enabled = true;
                }
                catch (Exception ex)
                {
                    DbUtils.WriteExeptionErrorLog(ex);
                }
                finally
                {
                    if (fReader != null)
                        fReader.Dispose();
                    Cursor.Current = Cursors.Default;
                }
            }
            FiskalizimiBL.SyncTCRTables();
        }

        private void btnShkarko_Click(object sender, EventArgs e)
        {
            DataTable _getAllLiferimet = new DataTable();
            //DataTable _getInitialCashRegiser = new DataTable();
            PnUtils.DbUtils.FillDataTable(_getAllLiferimet, @"select top(1) * from Liferimi");
            if (_getAllLiferimet.Rows.Count > 0)
            {
                _getAllLiferimet.Clear();
                double? _shitur = PnFunctions.SumDataColumn("Malli_Mbetur", "SasiaShitur");
                double? _kthyer = PnFunctions.SumDataColumn("Malli_Mbetur", "SasiaKthyer");
                if (_kthyer != null && _shitur != null)
                {
                    if (_shitur >= 0 && _kthyer <= 0)
                    {
                        ConfigXpnParams csh;
                        ConfigXpn.ReadConfiguration(out csh);
                        Sync MalliMbeturSync = new Sync(csh.LocalDbPath,
                                                        csh.LocalDbPassword,
                                                        csh.ServerName,
                                                        csh.MobileServerUrl,
                                                        csh.DbName,
                                                        csh.UserName,
                                                        csh.Password);
                        DataTable dtm = new DataTable();

                        dtm = GetServerData("EHWMalliMbeturShkarko", "Select count(*) AS Nr from Malli_Mbetur WHERE Export_Status = 0 AND DEPO = '" + csh.DeviceID.Replace("-", "") + "'");

                        if (dtm.Rows[0][0].ToString() == "0")
                        {
                            if (MessageBox.Show("Ky proçes shkarkon të gjtha faturat, \n inkasimet nga Palmi dhe rifreskon sasite e artikujve \n nese ka ngarkime të reja për magazinë. \n A dëshironi të vazhdoni?", "Konfirmo", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                            {
                                btnSinkronizo.Enabled = false;
                                btnSinkronizoKlientet.Enabled = false;
                                btnSinkronizoCmimoren.Enabled = false;
                                btnShkarko.Enabled = false;
                                decimal _LiferimetShuma = 0;
                                //decimal _getInitialCash = 0;
                                DataTable _getLiferimet = new DataTable();
                                //DataTable _getInitialCashRegiser = new DataTable();
                                PnUtils.DbUtils.FillDataTable(_getLiferimet, @"select Round(sum(ShumaPaguar),2) as Shuma from Liferimi where PayType = 'KESH'");
                                if (_getLiferimet.Rows.Count > 0 && _getLiferimet.Rows[0]["Shuma"].ToString() != "")
                                {
                                    _LiferimetShuma = decimal.Parse(_getLiferimet.Rows[0]["Shuma"].ToString());
                                }
                                else
                                {
                                    _LiferimetShuma = 0;

                                }

                                //PnUtils.DbUtils.FillDataTable(_getInitialCashRegiser, @"select Round(sum(Cashamount),2) as Shuma from CashRegister where convert(varchar(10), RegisterDate, 102) = convert(varchar(10), getdate(), 102) and DepositType = 0");
                                //if (_getInitialCashRegiser.Rows.Count > 0 && _getInitialCashRegiser.Rows[0]["Shuma"].ToString() != "")
                                //{
                                //    _getInitialCash = decimal.Parse(_getInitialCashRegiser.Rows[0]["Shuma"].ToString());
                                //}
                                //else
                                //{
                                //    _getInitialCash = 0;

                                //}


                                DataTable checkCashRegister = new DataTable();

                                PnUtils.DbUtils.FillDataTable(checkCashRegister,
                                    @"Select top(1) RegisterDate from CashRegister where DepositType = 1 order by RegisterDate desc");

                                DataTable checkMalliMbetur = new DataTable();

                                PnUtils.DbUtils.FillDataTable(checkMalliMbetur,
                                    @"Select top(1) Data from LEVIZJET_HEADER where ImpStatus = 3 order by Data desc");

                                DateTime DataCashRegister = DateTime.Now.Date.AddDays(-5);
                                if (checkCashRegister.Rows.Count > 0)
                                {
                                    DataCashRegister = Convert.ToDateTime(checkCashRegister.Rows[0]["RegisterDate"].ToString()).Date;
                                }

                                DateTime DataLevizjes = DateTime.Now.Date.AddDays(-5);
                                if (checkMalliMbetur.Rows.Count > 0)
                                {
                                    DataLevizjes = Convert.ToDateTime(checkMalliMbetur.Rows[0]["Data"].ToString()).Date;
                                }

                                decimal CashShumaTotale = _LiferimetShuma; //+_getInitialCash;
                                try
                                {
                                    if (DateTime.Now.Date > DataCashRegister.Date)
                                    {
                                        FiskalizimiBL.RegisterCashDeposit(CashShumaTotale, MobileSales.FiskalizimiWebService.CashDepositOperationSType.WITHDRAW);
                                    }

                                    //if (!(checkMalliMbetur.Rows.Count > 0))
                                    if (DateTime.Now.Date > DataLevizjes.Date)
                                    {
                                        FiskalizoMalliMbetur();
                                    }


                                    FiskalizimiBL.SyncTCRInvoices(1);
                                    FiskalizimiBL.SyncTCRInvoices(2);
                                    FiskalizimiBL.SyncTCRWTN(1);
                                    FiskalizimiBL.SyncTCRCashRegisterTable(1);
                                }
                                catch
                                {

                                }
                                ShkarkoMalliMbetur();


                                Cursor.Current = Cursors.Default;
                                dtm.Dispose();
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Nuk mund të vazhdohet me shkarkim! \n Shkarkimi paraprak për këtë pajisje nuk është eksportuar akoma në Financa5!", "Gabim!", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                            Cursor.Current = Cursors.Default;
                            dtm.Dispose();
                            return;

                        }

                    }

                }
                else
                {
                    MessageBox.Show("Psioni është i zëruar \n Nuk ka shënime për zërim", "Shkarkimi", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                    Cursor.Current = Cursors.Default;
                    return;

                }
            }
            else
            {
                MessageBox.Show("Nuk mund të vazhdohet me shkarkim! \n Duhet të jetë së paku një shitje për të vazhduar me shkarkim!", "Shkarkimi", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                Cursor.Current = Cursors.Default;
                return;

            }
        }

        private void FiskalizoMalliMbetur()
        {
            DataTable _getLevizjeMalliMbetur = new DataTable();
            PnUtils.DbUtils.FillDataTable(_getLevizjeMalliMbetur, @"select * from LEVIZJET_HEADER where Levizje_Ne = 'PG1' and Longitude = '8' and Latitude = '8'");
            if (_getLevizjeMalliMbetur.Rows.Count == 0)
            {
                DataTable _getShumaLiferimet = new DataTable();
                PnUtils.DbUtils.FillDataTable(_getShumaLiferimet, @"select sum(Round(ss.UnitPrice * m.SasiaMbetur,2)) as Totali from Malli_Mbetur m
                                inner join SalesPrice ss on m.IDArtikulli = ss.ItemNo_
                                where SalesCode = 'STANDARD' and m.SasiaMbetur > 0 and m.Depo = '" + frmHome.Depo + "'");
                if (_getShumaLiferimet.Rows.Count > 0 && _getShumaLiferimet.Rows[0]["Totali"].ToString() != "")
                {
                    decimal _TOTALI = Math.Round(decimal.Parse(_getShumaLiferimet.Rows[0]["Totali"].ToString()), 2);


                    //            string _getTotal = @"select sum(Round(ss.UnitPrice * m.SasiaMbetur,2)) as Totali from Malli_Mbetur m
                    //                                inner join SalesPrice ss on m.IDArtikulli = ss.ItemNo_
                    //                                where SalesCode = 'STANDARD' and m.SasiaMbetur > 0 and m.Depo = '" + frmHome.Depo + "'";
                    //            decimal _TOTALI = decimal.Parse(DbUtils.ExecSqlScalar(_getTotal).ToString());

                    string _getLevizjeIDN = "SELECT top(1) v.LevizjeIDN FROM NumriFisk v WHERE v.Depo = '" + frmHome.IDAgjenti + "'";
                    int _LevizjeIDN = int.Parse(DbUtils.ExecSqlScalar(_getLevizjeIDN).ToString());

                    _LevizjeIDN = _LevizjeIDN + 1;

                    string _getNrFatures = "Select NRKUFIP_D+CurrNrFat_D from NumriFaturave where KOD='" + frmHome.IDAgjenti + "'";
                    int _NrFatures = int.Parse(DbUtils.ExecSqlScalar(_getNrFatures).ToString());


                    string _getHeader = @"Insert into [LEVIZJET_HEADER] ([Numri_Levizjes],[Levizje_Nga],[Levizje_Ne],[Data],[Totali],[IDAgjenti],[SyncStatus],[ImpStatus],[Kodi_Dyqanit],[Numri_Daljes],[Depo], [Longitude], [Latitude], [NumriFisk], TCR, TCROperatorCode, TCRBusinessUnitCode)
                                                      Values('" + _NrFatures + @"','" + frmHome.IDAgjenti + "','PG1',getDate()," + _TOTALI + ",'" + frmHome.IDAgjenti + "',0,0,'" + frmHome.Depo + "','','" + frmHome.Depo + "', '8', '8', " + _LevizjeIDN + ", '" + frmHome.TCRCode + "', '" + frmHome.OperatorCode + "', '" + frmHome.BusinessUnitCode + "')";
                    DbUtils.ExecSql(_getHeader);

                    string _getDetails = @"Insert into [LEVIZJET_DETAILS] ([Numri_Levizjes],[Artikulli],[IDArtikulli],[Sasia],[Cmimi],[Njesia_matese],[Totali],[SyncStatus],[Seri],[Depo])
                            select '" + _NrFatures + @"', m.Emri, m.IDArtikulli, m.SasiaMbetur, ss.UnitPrice, a.BUM, Round(ss.UnitPrice * m.SasiaMbetur,2), 0, CASE WHEN m.Seri IS NOT NULL THEN M.Seri ELSE 'EMPTY' END, '" + frmHome.Depo + @"' from Malli_Mbetur m
                            inner join SalesPrice ss on m.IDArtikulli = ss.ItemNo_
                            inner join Artikujt a on a.IDArtikulli = m.IDArtikulli
                            where SalesCode = 'STANDARD' and m.SasiaMbetur > 0 and m.Depo = '" + frmHome.Depo + "'";

                    DbUtils.ExecSql(_getDetails);

                    string tryToupadteNumriFisk = @"UPDATE NumriFisk SET LevizjeIDN = " + (_LevizjeIDN) + " where Depo = '" + frmHome.IDAgjenti + "'";
                    DbUtils.ExecSql(tryToupadteNumriFisk);

                    string tryToupadteNrFatures = "Update NumriFaturave set CurrNrFat_D=CurrNrFat_D+1 where KOD='" + frmHome.IDAgjenti + "'";
                    DbUtils.ExecSql(tryToupadteNrFatures);

                    FiskalizimiBL.RegisterTCRWTN(_NrFatures.ToString());

                    string _getIDN = "SELECT top(1) v.IDN FROM NumriFisk v WHERE v.Depo = '" + frmHome.IDAgjenti + "'";
                    int _IDN = int.Parse(DbUtils.ExecSqlScalar(_getIDN).ToString());

                    bool InsertedNumriFisk = SyncFiskalizimi.UpdateNumriFiskByIDAgjenti(frmHome.IDAgjenti, frmHome.TCRCode, _IDN, _LevizjeIDN);
                }
            }
        }
        private void frmSync_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            btnShkarko.Enabled = false;
            btnSinkronizo.Enabled = true;
            btnSinkronizoKlientet.Enabled = true;
            btnSinkronizoCmimoren.Enabled = true;
        }

        private void menuDalja_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuRaporti_Click(object sender, EventArgs e)
        {
            if (frmHome.IDAgjenti == null)
            {
                MessageBox.Show("Mungon konfgurimi i Psionit");
                return;
            }
            if (frmRaproti == null) frmRaproti = new frmRaportiSinkronizimit();
            frmRaproti.ShowDialog();

        }

        private void btnSinkronizoKlientet_Click(object sender, EventArgs e)
        {
            btnSinkronizo.Enabled = false;
            btnSinkronizoKlientet.Enabled = false;
            btnSinkronizoCmimoren.Enabled = false;
            btnShkarko.Enabled = false;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (DbUtils.MainSqlConnection != null)
                {
                    DbUtils.MainSqlConnection.Close();
                }
                SyncKlientet(lblProgresi, progressBar1);
            }
            finally
            {
                btnSinkronizoKlientet.Enabled = true;
                Cursor.Current = Cursors.Default;
            }

            //this function fill textBox with log
            string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            StreamReader fReader = null;
            try
            {
                fReader = new StreamReader(CurrentDir + "\\log.txt");
                textBox1.Text = fReader.ReadToEnd();

                btnSinkronizoKlientet.Enabled = true;
            }
            catch (Exception ex)
            {
                DbUtils.WriteExeptionErrorLog(ex);
            }
            finally
            {
                if (fReader != null)
                    fReader.Dispose();
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSinkronizoCmimoren_Click(object sender, EventArgs e)
        {
            btnSinkronizo.Enabled = false;
            btnSinkronizoKlientet.Enabled = false;
            btnSinkronizoCmimoren.Enabled = false;
            btnShkarko.Enabled = false;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (DbUtils.MainSqlConnection != null)
                {
                    DbUtils.MainSqlConnection.Close();
                }
                SyncCmimoret(lblProgresi, progressBar1);
            }
            finally
            {
                btnSinkronizoCmimoren.Enabled = true;
                Cursor.Current = Cursors.Default;
            }

            //this function fill textBox with log
            string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            StreamReader fReader = null;
            try
            {
                fReader = new StreamReader(CurrentDir + "\\log.txt");
                textBox1.Text = fReader.ReadToEnd();

                btnSinkronizoCmimoren.Enabled = true;
            }
            catch (Exception ex)
            {
                DbUtils.WriteExeptionErrorLog(ex);
            }
            finally
            {
                if (fReader != null)
                    fReader.Dispose();
                Cursor.Current = Cursors.Default;
            }
        }

        //private void btnSyncConfig_Click(object sender, EventArgs e)
        //{
        //    Cursor.Current = Cursors.WaitCursor;
        //    var sync = SyncFiskalizimi.SinkronizoFiskalizimin();
        //    Cursor.Current = Cursors.Default;

        //    if (sync.hasError)
        //    {
        //        MessageBox.Show(sync.message);
        //        return;
        //    }
        //    else
        //    {
        //        MessageBox.Show(sync.message);
        //        return;
        //    }
        //}

        private void btnAzhurno_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            //bool success = SyncBL.SyncTablesWithGPS();
            if (GPSSyncBL.SyncTablesWithGPS())
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Azhurnimi është procesuar me sukses!");
            }
            else
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Azhurnimi dështoi!");
            }
        }
    }
}
