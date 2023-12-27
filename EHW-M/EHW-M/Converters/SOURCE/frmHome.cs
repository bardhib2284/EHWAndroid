using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//using PnSyncTestForm;
using System.IO;
using System.Reflection;
using PnUtils;
using Microsoft.WindowsMobile.Samples.Location;
using MobileSales.BL;

namespace MobileSales
{
    public partial class frmHome : Form
    {
        public static bool AprovimFaturash = false, UserSessionEnvLoaded, GetPayment, PrintBill, DayLock;
        public static SqlCeConnection AppDBConnection;
        public static SqlCeDataAdapter AppDBAdapter;
        public static string AppDBConnectionsStr, IDAgjenti, EmriAgjendit, MbiemriAgjendit, Perdoruesi, DevIDConfig, Depo, DevID, TCRCode, OperatorCode, BusinessUnitCode, WebServerFiskalizimiUrl, TAGNR_text, Viti;
        public const int ConfStateOK = 1, ConfStateInit = 2, ConfStateInvalid = 3;
        public static string ApplicationMode, LLOJDOK, PnPrintPort = "", LocalDbPassword = "";
        public static int ConfigurationState;
        private string CurrentDir;
        public static TipiMagazines _magType;
        private int retlogin;
        frmMeny frmAksioni = null;
        frmRegisterCashDeposit frmRegisterCashDeposit = null;
        public static DateTime LastFiskalizationSync;
        /*
      GPS Coordinates
      */

        public static EventHandler updateDataHandler;
        public static GpsDeviceState device = null;
        public static GpsPosition position = null;
        public static Gps gps = new Gps();
        public static string LatitudeCur = "";
        public static string LongitudeCur = "";


        public frmHome()
        {
            InitializeComponent();
        }

        private void frmHome_Load(object sender, EventArgs e)
        {
            LastFiskalizationSync = DateTime.Now.AddMinutes(1);
            /*GPS Events*/
            updateDataHandler = new EventHandler(UpdateData);
            gps.DeviceStateChanged += new DeviceStateChangedEventHandler(gps_DeviceStateChanged);
            gps.LocationChanged += new LocationChangedEventHandler(gps_LocationChanged);
            startGps();

            /* Kontrollohet nese Konfigurimi eshte valid: 
             * mund te jete vetem me DevID, atehere eshte Init dhe duhet se pari te kalohet ne sync.
             * Pas sync, thirret prape configuration(). Nese prape constate != OK, atehere exit app.
             * Perndryshe, Kycu=enabled.
            */
            menu_LogIn.Enabled = false;
            ConfigurationState = Configuration();
            agjentetTableAdapter.Connection.ConnectionString = AppDBConnectionsStr;

            //@BE 06.03.2021 Added
            if (!CheckOrRegisterTCRCode())//Kontrollo ose Regjistro Pajisijen per fiskalizim
            {
                MessageBox.Show("Mungon Konfigurimi dhe pajisja nuk mund të regjistrohet për fiskalizim!" + Environment.NewLine + "Dalja nga Aplikacioni.", "Konfigurimi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                Application.Exit();
            }
            if (ConfigurationState == ConfStateInvalid)//ConfigurationState=3
            {
                MessageBox.Show("Mungon Konfigurimi." + Environment.NewLine + "Dalja nga Aplikacioni.", "Konfigurimi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                Application.Exit();
            }
            if (ConfigurationState == ConfStateInit)//ConfigurationState=2
            {
                if (DialogResult.Yes == MessageBox.Show("Kjo pajisje duhet sinkronizuar." + Environment.NewLine + "Doni të filloni sinkronizimin tani ?", "EHWMobile - Sinkronizimi", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    frmSync SyncForm = null;//Instancim objekti
                    try
                    {
                        SyncForm = new frmSync();
                        SyncForm.ShowDialog();
                        GC.Collect();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        if (SyncForm != null)
                            SyncForm.Dispose();
                    }

                    ConfigurationState = Configuration();//Rekonfigurimi
                }
            }
            if (ConfigurationState != ConfStateOK) //still not OK
            {
                MessageBox.Show("Konfigurimi i pamundur, dalja nga aplikacioni.", "TobaKos - Sinkronizimi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                Application.Exit();
            }

            menu_LogIn.Enabled = true;
            Assembly asm = Assembly.GetExecutingAssembly();//Get Version
            lblHVersion.Text = asm.GetName().Version.ToString();

            PerformDeleteLogFiles();
        }

        /***-- Configuration -> Metode e cila kthen konfigurimin e psionit --***
         *   @ConfState -> parameter i cili tregon gjendjen e konfigurimit     *
         *   @ConfState = 1 -> ConfStateOK                                     *
         *   @ConfState = 3 -> ConfStateInvalid                                */

        private int Configuration()
        {
            int ConfState = ConfStateInvalid;
            DataSet dsConfig = null;
            try
            {
                CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
                dsConfig = new DataSet();
                dsConfig.ReadXml(CurrentDir + "\\Config.xpn");
                DevIDConfig = dsConfig.Tables[0].Rows[0]["DeviceID"].ToString();
                PnPrintPort = dsConfig.Tables[0].Rows[0]["PrintPort"].ToString();
                if (dsConfig.Tables[0].Rows[0]["TCRCode"] != null)
                {
                    TCRCode = dsConfig.Tables[0].Rows[0]["TCRCode"].ToString();
                }
                if (dsConfig.Tables[0].Rows[0]["OperatorCode"] != null)
                {
                    OperatorCode = dsConfig.Tables[0].Rows[0]["OperatorCode"].ToString();
                }
                if (dsConfig.Tables[0].Rows[0]["BusinessUnitCode"] != null)
                {
                    BusinessUnitCode = dsConfig.Tables[0].Rows[0]["BusinessUnitCode"].ToString();
                }
                if (dsConfig.Tables[0].Rows[0]["WebServerFiskalizimiUrl"] != null)
                {
                    WebServerFiskalizimiUrl = dsConfig.Tables[0].Rows[0]["WebServerFiskalizimiUrl"].ToString();
                }
                if (dsConfig.Tables[0].Columns["LocalDbPassword"] != null)
                    LocalDbPassword = dsConfig.Tables[0].Rows[0]["LocalDbPassword"].ToString();

                //Basic DevID got from conf file. 
                ConfState = ConfStateInit;

                AppDBConnectionsStr = "Datasource=" + CurrentDir + "\\MobileDatabase.sdf";
                if (LocalDbPassword.Trim() != "")
                {
                    LocalDbPassword = CryptorEngine.Decrypt(LocalDbPassword, "testKey", true);
                    AppDBConnectionsStr += ";Password=" + LocalDbPassword;
                }

                //set to all adapters new connction string
                konfigurimiTableAdapter.Connection.ConnectionString = AppDBConnectionsStr;
                agjentetTableAdapter.Connection.ConnectionString = AppDBConnectionsStr;
                agjendetTableAdapter1.Connection.ConnectionString = AppDBConnectionsStr;
                log_SyncErrorsTableAdapter.Connection.ConnectionString = AppDBConnectionsStr;
                konfigurimiTableAdapter1.Connection.ConnectionString = AppDBConnectionsStr;
                //Get the remaining profile from the table Konfiguration
                if (this.konfigurimiTableAdapter1.Permittance(this.myMobileDataSet1.Konfigurimi, frmHome.DevIDConfig) != 0)
                {
                    GetPayment = (bool)myMobileDataSet1.Konfigurimi[0]["GetPayment"];
                    PrintBill = (bool)myMobileDataSet1.Konfigurimi[0]["PrintBill"];
                    //DayLock = (bool)myMobileDataSet1.Konfigurimi[0]["DayLock"];
                    ApplicationMode = myMobileDataSet1.Konfigurimi[0]["ApplicationMode"].ToString();
                    frmHome.Depo = myMobileDataSet1.Konfigurimi[0]["Depo"].ToString();
                    if (Depo == "M07" || Depo == "M08" || Depo == "AT")
                    {
                        _magType = TipiMagazines.Fikse;
                    }
                    else
                    {
                        _magType = TipiMagazines.Levizes;
                    }
                    //from confInit goto OK state                    
                    ConfState = ConfStateOK;//ConfState=1
                }
                try
                {
                    TAGNR_text = DbUtils.ExecSqlScalar("SELECT d.TAGNR FROM Depot d WHERE d.Depo='" + frmHome.Depo + "'");
                    Viti = DbUtils.ExecSqlScalar("SELECT top(1) Viti FROM NumriFisk").ToString();
                }
                catch { }

            }
            catch (Exception ex)
            {
                DbUtils.WriteExeptionErrorLog(ex);
                ConfState = ConfStateInvalid;//ConfState=3

                MessageBox.Show(ex.Message);
            }
            finally
            {
                dsConfig.Dispose();
                GC.Collect();
            }
            return ConfState;
        }

        private void txtPerdoruesi_GotFocus(object sender, EventArgs e)
        {
            txtPerdoruesi.BackColor = Color.LightCyan;
        }

        private void txtPerdoruesi_LostFocus(object sender, EventArgs e)
        {
            txtPerdoruesi.BackColor = Color.White;
        }

        private void txtFjalekalimi_GotFocus(object sender, EventArgs e)
        {
            txtFjalekalimi.BackColor = Color.LightCyan;
        }

        private void txtFjalekalimi_LostFocus(object sender, EventArgs e)
        {
            txtFjalekalimi.BackColor = Color.White;
        }

        private void perdoruesi_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtPerdoruesi.Text != "" && e.KeyData == Keys.Enter)
            { txtFjalekalimi.Focus(); }//Kalimi në fushën "Fjalëkalimit" me ENTER
        }

        private void fjalekalimi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                menu_LogIn_Click(null, null);//Kycja me ane te tastin ENTER
            }
        }

        private void PerformDeleteLogFiles()
        {
            string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            string fullyPath = CurrentDir + @"\Logs";
            if (Directory.Exists(fullyPath))
            {
                DateTime createdOn;
                try
                {
                    if (File.Exists(fullyPath + "\\ExceptionErrorLog.txt"))
                    {
                        createdOn = File.GetCreationTime(fullyPath + "\\ExceptionErrorLog.txt");
                        if (createdOn < DateTime.Now.AddDays(-10))
                        {
                            File.Delete(fullyPath + "\\ExceptionErrorLog.txt");
                        }
                    }


                    if (File.Exists(fullyPath + "\\SQLCeErrorLog.txt"))
                    {
                        createdOn = File.GetCreationTime(fullyPath + "\\SQLCeErrorLog.txt");
                        if (createdOn < DateTime.Now.AddDays(-10))
                        {
                            File.Delete(fullyPath + "\\SQLCeErrorLog.txt");
                        }
                    }
                }
                catch (Exception ex)
                {
                    DbUtils.WriteExeptionErrorLog(ex);
                    GC.Collect();
                }
            }
        }

        private void menu_LogIn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Boolean result = false; UserSessionEnvLoaded = false;
            myMobileDatabase.EnforceConstraints = false;
            try
            {
                // retlogin = Convert.ToInt16(this.agjentetTableAdapter.FillByPerdoruesi(this.myMobileDatabase.Agjendet, txtPerdoruesi.Text, txtFjalekalimi.Text));
                retlogin = Convert.ToInt16(this.agjentetTableAdapter.FillByMeAprovim(this.myMobileDatabase.Agjendet, txtPerdoruesi.Text, txtFjalekalimi.Text));
                if (retlogin == 0)
                {
                    retlogin = Convert.ToInt16(this.agjentetTableAdapter.FillByPaAprovim(this.myMobileDatabase.Agjendet, txtPerdoruesi.Text, txtFjalekalimi.Text));
                    if (retlogin == 0)
                    {
                        MessageBox.Show("Përdoruesi ose Fjalëkalimi pasakt", "Kyçja", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        txtFjalekalimi.Text = "";
                        txtPerdoruesi.Focus();
                    }
                    else
                    {
                        AprovimFaturash = false;
                    }
                }
                else
                {
                    AprovimFaturash = true;
                }
            }

            catch (SqlCeException ex)
            {
                DbUtils.WriteSQLCeErrorLog(ex);
            }
            catch (Exception ex)
            {
                DbUtils.WriteExeptionErrorLog(ex);
            }

            Cursor.Current = Cursors.Default;
            if (retlogin != 1) result = false; else result = true;
            if (result == false)//result=false
            {
                MessageBox.Show("Përdoruesi ose Fjalëkalimi pasakt", "Kyçja", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                txtFjalekalimi.Text = "";
                txtPerdoruesi.Focus();
            }
            else	//result=True
            {
                /***Ndahen vlerat varablave nga konfig fajlli***/
                Perdoruesi = myMobileDatabase.Agjendet[0]["Perdoruesi"].ToString();
                IDAgjenti = myMobileDatabase.Agjendet[0]["IDAgjenti"].ToString();
                EmriAgjendit = myMobileDatabase.Agjendet[0]["Emri"].ToString();
                MbiemriAgjendit = myMobileDatabase.Agjendet[0]["Mbiemri"].ToString();


                int retConfig = Convert.ToInt16(this.konfigurimiTableAdapter1.FillByIDAgjentiAndaDeviId(this.myMobileDataSet1.Konfigurimi, IDAgjenti, DevIDConfig));
                if (retConfig != 1)//retConfig <> 1
                {

                    MessageBox.Show("Ky Psion nuk është i konfiguruar për shitësin: " + Perdoruesi + "", "Verejtje", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                    txtPerdoruesi.Text = "";
                    txtFjalekalimi.Text = "";
                    txtPerdoruesi.Focus();
                    return;


                }
                else// retConfig=1
                {
                    DevID = DevIDConfig;
                    //Init adapter for later ad-hoc use.
                    UserSessionEnvLoaded = true;
                    try
                    {
                        // AppDBConnectionsStr = "Datasource=" + CurrentDir + "\\MobileDatabase.sdf";
                        AppDBConnection = new SqlCeConnection(AppDBConnectionsStr);
                        PnUtils.DbUtils.MainSqlConnection = AppDBConnection;
                        AppDBConnection.Open();
                        AppDBAdapter = new SqlCeDataAdapter();

                        string sql = "SELECT * FROM Stoqet s WHERE s.Depo='" + Depo + "'";
                        PnUtils.DbUtils.FillDataSet(dsStoqet, sql);
                        if (dsStoqet.Tables[0].Rows.Count != 0)
                        {
                            LLOJDOK = dsStoqet.Tables[0].Rows[0]["LLOJDOK"].ToString();
                        }
                        //@BE 06.11.2018 Added
                        DeleteOldVisits();

                        //@BE 06.03.2021 Added
                        if (!RegisterCashDepositIsRegistered())
                        {
                            frmRegisterCashDeposit frmRegCashDeposit = new frmRegisterCashDeposit();
                            frmRegCashDeposit.ShowDialog();
                        }

                        if (frmAksioni == null)
                            frmAksioni = new frmMeny();
                        frmAksioni.Show();
                        this.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        DbUtils.WriteExeptionErrorLog(ex);
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        GC.Collect();
                    }
                }
            }
        }


        private void menu_Close_Click(object sender, EventArgs e)
        {
            Application.Exit();//Dalja nga aplikacioni
        }



        public static void startGps()
        {
            try
            {
                if (!gps.Opened)
                {
                    gps.Open();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void gps_DeviceStateChanged(object sender, DeviceStateChangedEventArgs args)
        {
            device = args.DeviceState;

            // call the UpdateData method via the updateDataHandler so that we
            // update the UI on the UI thread
            Invoke(updateDataHandler);//.Invoke(sender, null);
        }

        public static void UpdateData(object sender, System.EventArgs args)
        {
            FiskalizimiBL.SyncTCRTables();
            if (gps.Opened)
            {
                string str = "";
                if (device != null)
                {
                    str = device.FriendlyName + " " + device.ServiceState + ", " + device.DeviceState + "\n";
                }

                if (position != null)
                {

                    if (position.LatitudeValid)
                    {
                        str += "Latitude (DD):\n   " + position.Latitude + "\n";
                        LatitudeCur = position.LatitudeBesi.ToString() ?? "";
                        //str += "Latitude (D,M,S):\n   " + position.LatitudeInDegreesMinutesSeconds + "\n";
                    }

                    if (position.LongitudeValid)
                    {
                        str += "Longitude (DD):\n   " + position.Longitude + "\n";
                        LongitudeCur = position.LongitudeBesi.ToString() ?? "";
                        //str += "Longitude (D,M,S):\n   " + position.LongitudeInDegreesMinutesSeconds + "\n";
                    }

                    /*
                    if (position.SatellitesInSolutionValid &&
                        position.SatellitesInViewValid &&
                        position.SatelliteCountValid)
                    {
                        str += "Satellite Count:\n   " + position.GetSatellitesInSolution().Length + "/" +
                            position.GetSatellitesInView().Length + " (" +
                            position.SatelliteCount + ")\n";
                    }
                    */

                    if (position.TimeValid)
                    {
                        str += "Time:\n   " + position.Time.ToString() + "\n";
                    }

                }

                //status.Text = str;

            }
        }

        public void gps_LocationChanged(object sender, LocationChangedEventArgs args)
        {
            position = args.Position;

            // call the UpdateData method via the updateDataHandler so that we
            // update the UI on the UI thread
            Invoke(updateDataHandler);//.Invoke(sender, null);
            //Invoke(updateDataHandler);

        }

        /// <summary>
        //@BE 06.11.2018 - THis method is added in order to delete Visits older that where planned before 21 days
        /// </summary>
        private void DeleteOldVisits()
        {
            DateTime dateFilter = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-10);
            try
            {
                DbUtils.ExecSql("Delete from Vizitat where DataPlanifikimit <= '" + dateFilter.ToString("yyy-MM-dd") + "' ");
                //MB - update IDStatusiVizites per viziten e fundit te dites kaluar 
                DbUtils.ExecSql("update Vizitat set IDStatusiVizites = 6 where IDStatusiVizites = 0 and (DATEADD(dd, 0, DATEDIFF(dd, 0, DataAritjes))) = (DATEADD(dd, -1, DATEDIFF(dd, 0, GETDATE())))");
            }
            catch
            {
            }
        }

        private bool CheckOrRegisterTCRCode()
        {
            bool sukses = true;
            Cursor.Current = Cursors.WaitCursor;
            sukses = FiskalizimiBL.CheckOrRegisterTCRCode();
            Cursor.Current = Cursors.Default;
            return sukses;
        }


        private bool RegisterCashDepositIsRegistered()
        {
            bool sukses = true;
            sukses = FiskalizimiBL.RegisterCashDepositIsRegistered();
            return sukses;
        }

    }
}