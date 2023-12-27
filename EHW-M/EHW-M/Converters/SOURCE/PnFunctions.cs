using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Data;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Threading;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;
using Microsoft.WindowsCE.Forms;
using PnReports;
using System.Drawing.Imaging;
using MobileSales;
using PnUtils;
using MobileSales.BL;

namespace MobileSales
{
    class PnFunctions
    {
        /// <summary>
        /// Calculate SUM of value for specific colum, Set calculated valu in label as text
        /// </summary>
        /// <param name="_columName">Name of colum as nemed in DataSet</param>
        /// <param name="_labname">Label name where to set calculated value</param>
        /// <param name="_dsData">DataSet name </param>
        /// <param name="_decimalPlace">The number of decimal places in the return value </param>
        public static void CalcSumDataColumn(string _columName, Label _labname, DataSet _dsData, int _decimalPlace)
        {
            double SUM = 0;
            double _tmpSum = 0;
            int numRows = _dsData.Tables[0].Rows.Count;
            if (numRows > 0)
            {
                for (int j = 0; j < numRows; j++)
                {
                    try
                    {
                        _tmpSum = Convert.ToDouble(_dsData.Tables[0].Rows[j][_columName].ToString());

                    }
                    catch (Exception EX)
                    {
                        _tmpSum = 0;
                        DbUtils.WriteExeptionErrorLog(EX);
                    }
                    if (_tmpSum != null)
                    {
                        SUM = SUM + Convert.ToDouble(_dsData.Tables[0].Rows[j][_columName].ToString());

                    }
                }

            }
            StringBuilder myFormat = new StringBuilder("{0:#,0.");
            myFormat.Append('0', _decimalPlace);
            myFormat.Append('}', 1);
            _labname.Text = String.Format(myFormat.ToString(), SUM).ToString();

        }

        public static void CalcSumDataColumn(string _columName, Label _labname, DataTable _dtData, int _decimalPlace, string textBefor)
        {
            double SUM = 0;
            double _tmpSum = 0;
            int numRows = _dtData.Rows.Count;
            if (numRows > 0)
            {
                for (int j = 0; j < numRows; j++)
                {
                    try
                    {
                        _tmpSum = Convert.ToDouble(_dtData.Rows[j][_columName].ToString());

                    }
                    catch (Exception EX)
                    {
                        _tmpSum = 0;
                        DbUtils.WriteExeptionErrorLog(EX);
                    }
                    if (_tmpSum != null)
                    {
                        SUM = SUM + Convert.ToDouble(_dtData.Rows[j][_columName].ToString());

                    }
                }

            }
            StringBuilder myFormat = new StringBuilder("{0:#,0.");
            myFormat.Append('0', _decimalPlace);
            myFormat.Append('}', 1);
            _labname.Text = textBefor + " " + String.Format(myFormat.ToString(), SUM).ToString();

        }

        public static void UpateMalli_Mbetur_SHITUR(string ConnString)
        {
            DataSet _dsLiferimiART = null;
            string _selectQuery = "";


            SqlCeConnection con = null;
            SqlCeCommand cmd = null;
            try
            {
                con = new SqlCeConnection(ConnString);
                cmd = new SqlCeCommand("UPDATE TEST", con);
                if (con.State == ConnectionState.Closed)

                    con.Open();
                cmd.Transaction = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;

                _dsLiferimiART = new DataSet();
                _selectQuery = @"SELECT ROUND(SUM(la.SasiaLiferuar),3) as SumShitur,
                                           la.IDArtikulli
                                    FROM   LiferimiArt la WHERE la.SasiaLiferuar>0
                                    GROUP BY
                                           la.IDArtikulli";

                PnUtils.DbUtils.FillDataSet(_dsLiferimiART, _selectQuery);

                if (_dsLiferimiART.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < _dsLiferimiART.Tables[0].Rows.Count; i++)
                    {
                        string _idArtikulli = _dsLiferimiART.Tables[0].Rows[i]["IDArtikulli"].ToString();
                        double _SasiaShitur = double.Parse(_dsLiferimiART.Tables[0].Rows[i]["SumShitur"].ToString());
                        string _Seri = _dsLiferimiART.Tables[0].Rows[i]["Seri"].ToString();

                        cmd.CommandText = @"Update Malli_Mbetur
                                            SET SasiaShitur =@SasiaShitur 
                                            where IDArtikulli=@IDArtikulli and Seri = @Seri";
                        cmd.Parameters.AddWithValue("@SasiaShitur", _SasiaShitur);
                        cmd.Parameters.AddWithValue("IDArtikulli", _idArtikulli);
                        cmd.Parameters.AddWithValue("Seri", _Seri);

                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();//Release parameters

                    }

                    //Finally Applies pending changes to the underlying data

                    cmd.Transaction.Commit();
                }
            }
            catch (SqlCeException ex)
            {
                cmd.Transaction.Rollback();
                cmd.Connection.Close();
                WriteSQLCeErrorLog(ex, cmd.CommandText);
            }

            finally
            {
                _dsLiferimiART.Dispose();
                cmd.Connection.Close();
                con.Dispose();
                cmd.Dispose();
            }
        }

        public static void UpdateMalli_Mbetur_KTHYER()
        {
            DataSet _dsLiferimiArt = null;
            string _selectQuery = "";
            SqlCeConnection con = null;
            SqlCeCommand cmd = null;
            try
            {
                // con = new SqlCeConnection(ConnString);
                cmd = new SqlCeCommand("UPDATE TEST", con);
                con.Open();
                cmd.Transaction = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;

                _dsLiferimiArt = new DataSet();



                _selectQuery = @"SELECT ROUND(SUM(la.SasiaLiferuar),3) as SumKthyer,
                                           la.IDArtikulli
                                    FROM   LiferimiArt la WHERE la.SasiaLiferuar<0
                                    GROUP BY
                                           la.IDArtikulli";
                PnUtils.DbUtils.FillDataSet(_dsLiferimiArt, _selectQuery);

                if (_dsLiferimiArt.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < _dsLiferimiArt.Tables[0].Rows.Count; i++)
                    {
                        string _idArtikulli = _dsLiferimiArt.Tables[0].Rows[i]["IDArtikulli"].ToString();
                        double _SasiaKthyer = double.Parse(_dsLiferimiArt.Tables[0].Rows[i]["SumKthyer"].ToString());
                        string _Seri = _dsLiferimiArt.Tables[0].Rows[i]["Seri"].ToString();
                        cmd.CommandText = @"Update Malli_Mbetur
                                            SET SasiaKthyer =@SasiaKthyer
                                            where IDArtikulli=@IDArtikulli and Seri = @Seri";
                        cmd.Parameters.AddWithValue("@SasiaKthyer", _SasiaKthyer);
                        cmd.Parameters.AddWithValue("@IDArtikulli", _idArtikulli);
                        cmd.Parameters.AddWithValue("Seri", _Seri);

                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();//Release parameters
                    }

                    //Finally Applies pending changes to the underlying data
                    cmd.Transaction.Commit();
                }



            }
            catch (SqlCeException ex)
            {
                cmd.Transaction.Rollback();
                WriteSQLCeErrorLog(ex, cmd.CommandText);
            }
            finally
            {
                _dsLiferimiArt.Dispose();
                cmd.Connection.Close();
                con.Dispose();
                cmd.Dispose();
            }

        }

        public static void UpdateMalli_Mbetur_Mbetur()
        {
            string _UpdateQuery = "";
            try
            {

                _UpdateQuery = @"UPDATE Malli_Mbetur
                 SET
                 SasiaMbetur = Round((SasiaPranuar - SasiaShitur -SasiaKthyer),3) ";
                PnUtils.DbUtils.ExecSql(_UpdateQuery);

            }
            catch
                 (SqlCeException ex)
            {

                MessageBox.Show(ex.Message);
                WriteSQLCeErrorLog(ex, _UpdateQuery);
            }




        }



        public static void WriteSQLCeErrorLog(SqlCeException ex, string sqlcmdtext)
        {
            string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            StreamWriter LogText = new StreamWriter(CurrentDir + "\\SQLCeErrorLog.txt", true);

            LogText.WriteLine("DT:" + DateTime.Now.ToString());
            LogText.WriteLine("Message:" + ex.Message);
            LogText.WriteLine("InnerException" + ex.InnerException);
            LogText.WriteLine("Source" + ex.Source);
            LogText.WriteLine("StackTrace" + ex.StackTrace);
            LogText.WriteLine("sqlcmdtext=" + sqlcmdtext);
            LogText.WriteLine("");
            LogText.Close();
            LogText.Dispose();

        }

        public static void PrintoFletePagesen(string _nrFatures, string _IDKlienti, string _nrPageses, bool showMessagePrint, PnDevice _PMode)
        {
            string NrPageses = "";
            string EmriAgjentit = "";
            string Lokacioni = "";
            string KontaktEmri = "";
            string KlientiEmri = "";
            string companyName = "";
            string Nipt = "";
            string ShumaTotale = "";
            string ShumaPaguar = "";
            string _payType = "";
            string _kmon = "";
            SqlCeDataAdapter da2 = null;
            da2 = new SqlCeDataAdapter("", frmHome.AppDBConnectionsStr);
            DataTable dtTemp = new DataTable();

            EmriAgjentit = frmHome.EmriAgjendit + " " + frmHome.MbiemriAgjendit;


            da2.SelectCommand.CommandText = "Select * from KlientDheLokacion where IDKlientDheLokacion='" + _IDKlienti + "'";
            dtTemp.Clear();
            da2.Fill(dtTemp);
            if (dtTemp.Rows.Count > 0)
                Lokacioni = dtTemp.Rows[0]["EmriLokacionit"].ToString();
            KontaktEmri = dtTemp.Rows[0]["KontaktEmriMbiemri"].ToString();
            dtTemp.Clear();
            string condition = "";
            if (_nrFatures == "")
            {
                condition = "NrFatures IS NULL And NrPageses='" + _nrPageses + "'";
            }
            else
            {
                condition = "NrFatures=" + _nrFatures + " And NrPageses='" + _nrPageses + "'";
            }
            da2.SelectCommand.CommandText = "Select * From EvidencaPagesave where " + condition;
            da2.Fill(dtTemp);
            if (dtTemp.Rows.Count > 0)
            {
                NrPageses = dtTemp.Rows[0]["NrPageses"].ToString();
                ShumaPaguar = dtTemp.Rows[0]["ShumaPaguar"].ToString();
                if (_nrFatures == "")
                {
                    ShumaTotale = ShumaPaguar;
                }
                else
                {
                    ShumaTotale = dtTemp.Rows[0]["ShumaTotale"].ToString();

                }
                _payType = dtTemp.Rows[0]["PayType"].ToString();
                _kmon = dtTemp.Rows[0]["KMON"].ToString();

            }

            dtTemp.Clear();
            da2.SelectCommand.CommandText = "SELECT * FROM Klientet where IDKlienti='" + _IDKlienti + "'";
            da2.Fill(dtTemp);
            if (dtTemp.Rows.Count > 0)
            {
                KlientiEmri = dtTemp.Rows[0]["Emri"].ToString();

            }
            else
            {
                KlientiEmri = "";
            }

            DataSet temp = new DataSet();
            try
            {
                string sql = "SELECT ci.[Value] as Value FROM CompanyInfo ci WHERE ci.Item='Shitesi' OR ci.Item='Nipt'";

                PnUtils.DbUtils.FillDataSet(temp, sql);
                Nipt = temp.Tables[0].Rows[0]["Value"].ToString();
                companyName = temp.Tables[0].Rows[1]["Value"].ToString();
            }
            catch
            {
                MessageBox.Show("Nuk mund te merren informatata rreth kompnis EHW!");
            }
            finally
            {
                temp.Dispose();
                if (da2 != null) da2.Dispose();
            }

            PirntoInkasimin
               (_nrFatures.ToString(), NrPageses, DateTime.Now, companyName, Nipt, EmriAgjentit, frmHome.IDAgjenti,
               KlientiEmri, Lokacioni, KontaktEmri, DateTime.Now, ShumaTotale, ShumaPaguar, _kmon, _payType, showMessagePrint, _PMode);
        }

        public static void PirntoInkasimin(string strNrFatures, string strNrPageses, DateTime dtDataPageses, string strKompania, string strBusinessNo,
                                     string strEmriAgjentit, string strIDAgjenti, string strIDKlienti, string strLokacioni, string strKontaktEmri,
                                     DateTime dtPagesatArritura, string strShuma, string strInkasuar, string KMON, string PayType, bool msgShow, PnDevice _PrintMode)
        {
            SmartDevicePrintEngine sd = null;
            PnDevice devPort = PnDevice.BluetoothPort;
            frmMessage msg = null;
            string strPortName = "";

            if (msgShow)
            {
                //@BE QR Code
                if (msg == null) msg = new frmMessage(false, new string[]{null});
                DialogResult a = msg.ShowDialog();
                if (a == DialogResult.Abort)
                {
                    return;//cancel printing
                }
                if (frmMessage.isBlutooth == true)
                {

                    devPort = PnDevice.BluetoothPort;
                    strPortName = frmHome.PnPrintPort + ":";

                }
                //else
                //{
                //    devPort = PnDevice.MultiplicatorPort;
                //    strPortName = frmHome.PnPrintPort + ":";
                //}
            }
            else
            {

                devPort = _PrintMode;
                strPortName = frmHome.PnPrintPort + ":";

            }


            string buffer = "";
            int PrintHeight = 55; int PrintWidth = 130;
            int x = 40; int y = 1;
            int prevX = 0;
            string titleSep = "";


            try
            {
                string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);

                //@BE DUHET NDRYSHOHET
                BxlPrint.PrinterOpen("COM1:115200", 1000);
                BxlPrint.PrintBitmap(CurrentDir + "\\Image\\EHW-logo.png", 100, BxlPrint.BXL_ALIGNMENT_CENTER, 30);
                BxlPrint.LineFeed(1);

                sd = new SmartDevicePrintEngine(devPort, strPortName);

                sd.CreatePage(PrintWidth, PrintHeight);
                int c = 0;
                x = 20;
                c += 1;

                buffer = "F L E T E  -  P A G E S E";
                //StringBuilder tmpBuffer = new StringBuilder("");
                //tmpBuffer.Append(' ', PrintWidth / 2 - buffer.Length / 2);
                //tmpBuffer.Append(buffer);
                //buffer = tmpBuffer.ToString();
                prevX = 1;
                sd.WriteToLogicalPage(x, y, buffer);
                y++;

                WriteSeparator(ref x, ref y, ref sd, "-"); //E shtuar

                //------------------TITLE SEPERATOR-------------------------
                sd.WriteToLogicalPage(x, y, titleSep);


                y += 1;
                buffer = "     Numri fletepageses: " + strNrPageses;
                sd.WriteToLogicalPage(x, y, buffer);
                y += 1;

                buffer = "     Nr.: " + strNrFatures;
                sd.WriteToLogicalPage(x, y, buffer);

                y += 1;
                buffer = "     Data: " + dtDataPageses.ToString("dd/MM/yyyy HH:mm:ss");
                sd.WriteToLogicalPage(x, y, buffer);
                //-------------------------------------------------------------
                WriteSeparator(ref x, ref y, ref sd, "-");

                y += 1;
                buffer = "     Kompania: " + strKompania;
                sd.WriteToLogicalPage(x, y, buffer);

                y += 1;
                buffer = "     Nr.Biznesit: " + strBusinessNo;
                sd.WriteToLogicalPage(x, y, buffer);

                y += 1;
                buffer = "     Emri agjentit: " + strEmriAgjentit;
                sd.WriteToLogicalPage(x, y, buffer);


                y += 1;
                buffer = "     IDAgjenti: " + strIDAgjenti;
                sd.WriteToLogicalPage(x, y, buffer);

                //-------------------------------------------------------------
                WriteSeparator(ref x, ref y, ref sd, "-");

                y += 1;
                buffer = "     Klienti: " + strIDKlienti;
                sd.WriteToLogicalPage(x, y, buffer);

                y += 1;
                buffer = "     Lokacioni: " + strLokacioni;
                sd.WriteToLogicalPage(x, y, buffer);

                y += 1;
                buffer = "     Kontakt Emri: " + strKontaktEmri;
                sd.WriteToLogicalPage(x, y, buffer);

                //-------------------------------------------------------------
                WriteSeparator(ref x, ref y, ref sd, "-");

                y += 1;
                buffer = "     Data e pageses:" + dtPagesatArritura.ToString("dd/MM/yyyy HH:mm:ss");
                sd.WriteToLogicalPage(x, y, buffer);

                y += 1;
                buffer = "     Shuma e fatures: " + String.Format("{0:0.00}", Math.Round(decimal.Parse(strShuma), 2)) + " " + KMON;
                sd.WriteToLogicalPage(x, y, buffer);

                y += 1;
                buffer = "     Pagesa e inkasuar: " + String.Format("{0:0.00}", Math.Round(decimal.Parse(strInkasuar), 2)) + " " + KMON + " " + PayType;
                sd.WriteToLogicalPage(x, y, buffer);

                //-------------------------------------------------------------
                WriteSeparator(ref x, ref y, ref sd, "-");

                x = 50;
                y += 3;
                buffer = "     Per Kompanine: ______________________"; //18
                sd.WriteToLogicalPage(x, y, buffer);

                y += 1;
                buffer = "     " + strEmriAgjentit;
                sd.WriteToLogicalPage(x, y, buffer);

                //prevX = buffer.Length + x + 50;

                y += 2;
                buffer = "     Per Klientin: ______________________";
                prevX = sd.PageWidth - buffer.Length;
                sd.WriteToLogicalPage(prevX, y, buffer);

                y += 3;

                WriteSeparator(ref x, ref y, ref sd, "="); y = y + 1;

                buffer = "     Fletepagesa eshte e hartuar ne tre kopje";
                sd.WriteToLogicalPage(x, y, buffer);

                y += 1;
                buffer = "     Printuar nga Sistemi MobSell Asseco - Kosove +383 38 40 77 99 ";
                sd.WriteToLogicalPage(x, y, buffer);

                y += 5;
                sd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                DbUtils.WriteExeptionErrorLog(ex);
                return;
            }
            finally
            {
                if (sd != null) sd.Dispose();
                if (msg != null) msg.Dispose();
                msg = null;
            }

        }
        public static void WriteSeparator(ref int x, ref int y, ref SmartDevicePrintEngine PrintEngine, string CharSeperator)
        {
            int prevX = x;
            string Seperator = "";
            x = 1;//reset x-coordinate            
            y++;
            //for (int i = 0; i < PrintEngine.PageWidth; i++) Seperator += CharSeperator;
            for (int i = 0; i < 69; i++) Seperator += CharSeperator;
            PrintEngine.WriteToLogicalPage(x, y, Seperator);
            x = prevX;
        }


        public static void TESTinSTANCE()
        {
            // Allow running single instance
            string processName = Process.GetCurrentProcess().StartInfo.FileName;
            Process instances = Process.GetCurrentProcess();

            if (instances.StartInfo.FileName != null)
            {
                MessageBox.Show("Application already Running", "Error 1001 - Application Running");
                return;
            }
        }

        public static bool AvailablePort(string portName)
        {
            bool result = false;
            System.IO.Ports.SerialPort sp = new System.IO.Ports.SerialPort(portName);
            try
            {
                sp.Open();
                result = true;

            }
            catch (System.IO.IOException )
            {
                result = false;
            }
            finally
            {
                sp.Dispose();
            }

            return result;
        }

        public static DataTable[] GenerateBill(TipiMagazines _mType, string IDPorosia)
        {
            DataTable dt_Header = new DataTable();
            DataTable dt_Details = new DataTable();
            DataTable dt_Footer = new DataTable();
            DataTable[] dt = new DataTable[3];
            dt[0] = dt_Details;
            dt[1] = dt_Header;
            dt[2] = dt_Footer;
            SqlCeDataAdapter da = null;
            string targaMjetit = "";
            string drejtuesi = "";
            string transportuesi = "";
            string adresa = "";
            string Nipt_KF = "";
            string commHeader = "";
            string commDetail = "";
            string commFooter = "";
            string llojiFatures = "' '";
            string llojiProcesit = "' '";

            try
            {
                string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);

                //@BE DUHET NDRYSHOHET
                BxlPrint.PrinterOpen("COM1:115200", 1000);
                BxlPrint.PrintBitmap(CurrentDir + "\\Image\\EHW-logo.png", 100, BxlPrint.BXL_ALIGNMENT_CENTER, 30);
                BxlPrint.LineFeed(1);

                string sqp_nipt = "SELECT ci.[Value] FROM CompanyInfo ci WHERE ci.Item='KF'";
                string _nipt = PnUtils.DbUtils.ExecSqlScalar(sqp_nipt);
                string _idKlienti = PnUtils.DbUtils.ExecSqlScalar("Select IDKlienti from Liferimi where IDPorosia='" + IDPorosia + "'");

                if (_mType == TipiMagazines.Fikse)
                {
                    //Ndryshimet nese Magazina eshte Fikse
                    transportuesi = "k.Emri";
                    targaMjetit = "' '";
                    drejtuesi = "k.Emri";
                    adresa = "kl.EmriLokacionit";
                    Nipt_KF = "'NIPT:' + k.NIPT + '   KF:'";

                }
                else
                {
                    //Ndryshimet nese Magazina eshte Levizese
                    transportuesi = "c.[Value]";
                    targaMjetit = "d.TAGNR";
                    drejtuesi = "a.Emri +' '+a.Mbiemri";
                    adresa = "C1.[Value]";
                    Nipt_KF = "'Nipt: ' + c1.[Value]+' KF:" + _nipt + "'";
                }

                if (frmVizitat.KthimMall)
                {
                    llojiFatures = "'380 / Fature korrigjuese'";
                    llojiProcesit = "'P10 Faturimi korrigjues (anulimi / korrigjimi i nje fature)'";
                }
                else if (frmVizitat.ShitjeKorrigjim)
                {
                    llojiFatures = "'380 / Fature korrigjuese'";
                    llojiProcesit = "'P10 Faturimi korrigjues (anulimi / korrigjimi i nje fature)'";
                }
                else
                {
                    llojiFatures = "'388 / Fature tatimore'";
                    llojiProcesit = "'P3 Faturimi i dorezimit te porosise se blerjes se rastesishme'";
                }


                string sr = @"SELECT        CONVERT (Decimal(10,2),ROUND(la.Totali,2))
                            FROM          PorosiaArt pa
                             INNER JOIN Liferimi l ON  pa.IDPorosia = l.IDPorosia
                            INNER JOIN LiferimiArt la   ON  pa.IDArtikulli = la.IDArtikulli and pa.Seri = la.Seri

                                                     
                            WHERE        (pa.IDPorosia = '" + IDPorosia + "') AND la.IDLiferimi = l.IDLiferimi";



                string s = String.Format("{0:0.00}", decimal.Parse((DbUtils.ExecSqlScalar(sr).ToString())));


                commHeader = @"       SELECT ('Shitesi: ' + c.[Value] + '   ' + cl.[Value]) AS [FirstHeader]
                                      FROM   CompanyInfo c, CompanyInfo cl
                                      WHERE  c.Item = 'Shitesi' and cl.Item = 'NIPT'

                                      UNION ALL  


                                      SELECT ('Tel: 048 200 711      web: www.ehwgmbh.com') AS [FirstHeader] 


                                      UNION ALL  

                                      SELECT ('Adresa: ' + " + targaMjetit + @" + '   9923') AS [FirstHeader]
                                      FROM   Liferimi l 
                                             INNER JOIN Klientet k 
                                                   ON  l.IDKlienti  = k.IDKlienti 
                                             INNER JOIN KlientDheLokacion kl 
                                                   ON  k.IDKlienti  = kl.IDKlienti 
                                             INNER JOIN Depot d 
                                                  ON  d.Depo = l.Depo
                                             INNER JOIN Agjendet a 
                                                ON a.IDAgjenti=d.Depo ,
                                              
                                             CompanyInfo c
                                      WHERE  l.IDPorosia = '" + IDPorosia + @"' 
                                             AND c.Item = 'Adresa'       
                                      

                                      UNION ALL             
                                      

                                      select 'Qyteti / Shteti: Tirana, Albania' as FirstHeader
                                      

                                      UNION ALL  

                                      select '----------------------------------------------------------------------' as [FirstHeader] 
                                      

                                      UNION ALL   

                                      
                                      SELECT ('Kodi / Emri i llojit te fatures: ' + " + llojiFatures + @") AS [FirstHeader]

                                      

                                      UNION ALL   

                                      
                                      SELECT ('Llojit i procesit: ') AS [FirstHeader]
                                      

                                      UNION ALL   

                                      
                                      SELECT (" + llojiProcesit + @") AS [FirstHeader]
                                      

                                      UNION ALL  

                                      select '----------------------------------------------------------------------' as [FirstHeader]
                                      

                                      UNION ALL  


                                      SELECT ('Numri i fatures: ' + CASE 
                                                            WHEN l.NumriFisk IS NOT NULL THEN CONVERT(NCHAR(20),l.NumriFisk) + '/' + '" + frmHome.Viti + @"'
                                                            ELSE ''
                                                       END) AS [FirstHeader] 
                                      FROM  Liferimi l  
                                      WHERE  l.IDPorosia = '" + IDPorosia + @"' 


                                      UNION ALL  
                                      
                                      SELECT  'Data dhe ora e leshimit te fatures: ' + CONVERT(NCHAR(10), l.KohaLiferimit, 104) + 
                                                 ' '+CONVERT(NCHAR(10), l.KohaLiferimit, 8) AS [FirstHeader]  
                                      FROM  Liferimi l  
                                      WHERE  l.IDPorosia = '" + IDPorosia + @"'         
                                      

                                      UNION ALL   

                                      
                                      SELECT ('Menyra e pageses: ' + CASE 
                                                            WHEN l.PayType IS NOT NULL THEN l.PayType
                                                            ELSE ''
                                                       END) AS [FirstHeader] 
                                      FROM  Liferimi l  
                                      WHERE  l.IDPorosia = '" + IDPorosia + @"' 
                                      

                                      UNION ALL   

                                      
                                      SELECT 'Monedha e fatures: ALL' AS [FirstHeader]   


                                      UNION ALL   

                                      
                                      SELECT ('Kodi i vendit te ushtrimit te veprimtarise se biznesit: ' + CASE 
                                                            WHEN l.TCRBusinessCode IS NOT NULL THEN l.TCRBusinessCode
                                                            ELSE ''
                                                       END) AS [FirstHeader] 
                                      FROM  Liferimi l  
                                      WHERE  l.IDPorosia = '" + IDPorosia + @"' 


                                      UNION ALL   

                                      
                                      SELECT ('Kodi i operatori: ' + CASE 
                                                            WHEN l.TCROperatorCode IS NOT NULL THEN l.TCROperatorCode
                                                            ELSE ''
                                                       END) AS [FirstHeader] 
                                      FROM  Liferimi l  
                                      WHERE  l.IDPorosia = '" + IDPorosia + @"' 


                                      UNION ALL   

                                      select ('NIVF: ' + CASE 
                                                              WHEN l.TCRNIVF IS NOT NULL THEN l.TCRNIVF
                                                              ELSE ''
                                                         END) AS [FirstHeader]
                                      FROM  Liferimi l  
                                      WHERE  l.IDPorosia = '" + IDPorosia + @"'


                                      UNION ALL   

                                      select ('NSLF: ' + CASE 
                                                              WHEN l.TCRNSLF IS NOT NULL THEN l.TCRNSLF
                                                              ELSE ''
                                                         END) AS [FirstHeader]
                                      FROM  Liferimi l  
                                      WHERE  l.IDPorosia = '" + IDPorosia + @"'
                                      

                                      UNION ALL  

                                      select '----------------------------------------------------------------------' as [FirstHeader] 
                                      

                                      UNION ALL  

                                      SELECT ('Bleresi: ' + k.Emri + '  ' + k.NIPT) AS [FirstHeader] 
                                      FROM   Klientet k 
                                      WHERE  k.IDKlienti = '" + _idKlienti + @"'   
                                      

                                      UNION ALL   

                                      
                                      SELECT ('Adresa: ' + kl.Adresa + '  Albania       9923') AS [Klienti]  
                                      FROM  Liferimi l  
                                             INNER JOIN KlientDheLokacion kl  
                                                  ON  kl.IDKlienti = l.IDKlienti    
                                              INNER JOIN Depot d  
                                                   ON  d.Depo = l.Depo
                                      WHERE  l.IDPorosia = '" + IDPorosia + @"'   
                                      

                                      UNION ALL  

                                      select '----------------------------------------------------------------------' as [FirstHeader] 
                                      

                                      UNION ALL  

                                      SELECT ('Transportuesi: ' + c.[Value] + '   ' + cl.[Value]) AS [FirstHeader]
                                      FROM   CompanyInfo c, CompanyInfo cl
                                      WHERE  c.Item = 'Shitesi' and cl.Item = 'NIPT'


                                      UNION ALL   

                                      SELECT ('Adresa: ' + " + targaMjetit + @" + '  ('+ a.Emri+' '+a.Mbiemri +')') AS [FirstHeader]
                                      FROM   Liferimi l 
                                             INNER JOIN Klientet k 
                                                   ON  l.IDKlienti  = k.IDKlienti 
                                             INNER JOIN KlientDheLokacion kl 
                                                   ON  k.IDKlienti  = kl.IDKlienti 
                                             INNER JOIN Depot d 
                                                  ON  d.Depo = l.Depo
                                             INNER JOIN Agjendet a 
                                                ON a.IDAgjenti=d.Depo ,
                                              
                                             CompanyInfo c
                                      WHERE  l.IDPorosia = '" + IDPorosia + @"' 
                                             AND c.Item = 'Adresa'


                                      UNION ALL  
                                      
                                      SELECT  'Data dhe ora e furnizimit: ' + + CONVERT(NCHAR(10), l.KohaLiferimit, 104) + 
                                                 ' '+CONVERT(NCHAR(10), l.KohaLiferimit, 8) AS [FirstHeader]  
                                      FROM  Liferimi l  
                                      WHERE  l.IDPorosia = '" + IDPorosia + @"'        

                                      ";


                //DataTable dtFooter2 = new DataTable();



//                DataTable totalet = new DataTable();
//                string Shkalla20 = "";
//                string totaliPaTVSH20 = "";
//                string totaliTVSH20 = "";

//                PnUtils.DbUtils.FillDataTable(totalet, @"SELECT  ('S-VAT') AS [Kodi],
//                                                ('20 ') AS [Shkalla], 
//                                                SUM(CONVERT (Decimal(10,2),ROUND(la.TotaliPaTVSH,2)))as TotaliPaTVSH,
//                                                SUM(CONVERT (Decimal(10,2),ROUND(la.Totali,2), 0)) - SUM(CONVERT (Decimal(10,2),ROUND(la.TotaliPaTVSH,2))) TVSH,
//                                                SUM(CONVERT (Decimal(10,2),ROUND(la.Totali,2), 0)) - SUM(CONVERT (Decimal(10,2),ROUND(la.TotaliPaTVSH,2))) Totali
//                                                FROM          PorosiaArt pa
//                                                 INNER JOIN Liferimi l ON  pa.IDPorosia = l.IDPorosia
//                                                INNER JOIN LiferimiArt la   ON  pa.IDArtikulli = la.IDArtikulli and pa.Seri = la.Seri
//                    
//                                                                         
//                                                WHERE (pa.IDPorosia = '" + IDPorosia + @"') AND la.IDLiferimi = l.IDLiferimi");


                    

                //if (totalet.Rows.Count > 0)
                //{
                //    totaliPaTVSH = String.Format("{0:##0.00#}", float.Parse((totalet.Rows[0]["ShumaPaTVSH"].ToString())));
                //    totaliTVSH = String.Format("{0:##0.00#}", float.Parse((totalet.Rows[0]["TVSH"].ToString())));
                //}
                //else
                //{
                //    totaliPaTVSH = "";
                //    totaliTVSH = "";
                //}

//                commFooter = @"         SELECT  
//                                         ('Shtese ne nivel') AS [Kodi], 
//                                         ('fature') AS [Shkalla], 
//                                         ('') AS [Vlera], 
//                                         ('') AS [TVSH] 
//
//                                   
//                                        UNION ALL
//
//                                        SELECT  
//                                         ('===============') AS [Kodi], 
//                                         ('================') AS [Shkalla], 
//                                         ('==================') AS [Vlera], 
//                                         ('================') AS [TVSH]
//
//                                   
//                                        UNION ALL
//
//                                        SELECT  
//                                         ('Kodi TVSH-se') AS [Kodi], 
//                                         ('Shkalla TVSH-se') AS [Shkalla], 
//                                         ('Vlera tatueshme') AS [Vlera], 
//                                         ('Shuma TVSH-se') AS [TVSH]
//
//                                   
//                                        UNION ALL
//
//                                        SELECT  
//                                         ('---------------') AS [Kodi], 
//                                         ('----------------') AS [Shkalla], 
//                                         ('------------------') AS [Vlera], 
//                                         ('----------------') AS [TVSH]" 
                    
                    
//                                        @"
//                                        UNION ALL
//                                        SELECT  
//                                         ('S-VAT') AS [Kodi], 
//                                         ('" + Shkalla20 + @"') AS [Shkalla], 
//                                         ('" + totaliPaTVSH20 + @"') AS [Vlera], 
//                                         ('" + totaliTVSH20 + @"') AS [TVSH]
//
//                                        UNION ALL
//                                        SELECT  
//                                         ('----------------') AS [Kodi], 
//                                         ('-----------------') AS [Shkalla], 
//                                         ('-------------------') AS [Vlera], 
//                                         ('-----------------') AS [TVSH]
//
//                                        UNION ALL
//
//                                        SELECT  
//                                         (' ') AS [Kodi], 
//                                         (' ') AS [Shkalla], 
//                                         (' ') AS [Vlera], 
//                                         (' ') AS [TVSH]
//
//                                        UNION ALL
//
//                                        SELECT  
//                                         ('Afati i pageses:') AS Kodi, 
//                                         (CONVERT(NCHAR(10), GETDATE() + 30, 104)) AS Shkalla, 
//                                         ('') AS Vlera, 
//                                         ('') AS TVSH";  



                commFooter = @"     SELECT  
                                         ('Bleresi') AS [Blersi], 
                                         ('') AS [Transportuesi] , 
                                         ('Shitesi') AS [Agjenti] 

                                   
                                      UNION ALL  

                                      SELECT  

                                              (k.Emri) AS Blersi, 
                                              ('') AS Transportuesi, 
                                              (a.Emri+' '+a.Mbiemri) AS Agjenti 
                                      FROM   Liferimi l 
                                             INNER JOIN Klientet k 
                                                   ON  l.IDKlienti  = k.IDKlienti 
                                             INNER JOIN KlientDheLokacion kl 
                                                   ON  k.IDKlienti  = kl.IDKlienti 
                                             INNER JOIN Depot d 
                                                  ON  d.Depo = l.Depo
                                             INNER JOIN Agjendet a 
                                                ON a.IDAgjenti=d.Depo , CompanyInfo c 
                                       WHERE  l.IDPorosia = '" + IDPorosia + @"' AND c.Item='Shitesi'  


                                         UNION ALL  


                                       SELECT  
                                             ('') AS Blersi, 
                                             ('') AS Transportuesi, 
                                             ('') AS Agjenti 

                                      UNION ALL  
                                      

                                      SELECT  
                                                   ('_________________________') AS Blersi, 
                                                   ('') AS Transportuesi, 
                                                   ('_________________________') AS Agjenti 

                                   
                                      UNION ALL  

                                      SELECT  
                                        ('(emri,mbiemri,nenshkrimi)') AS Blersi,
                                        ('') AS [Transportuesi], 
                                        ('(emri,mbiemri,nenshkrimi)') AS Agjenti ";
                                        

                ///////////////////// Testimi i fiskalizimit mb ///////////////////////
                commDetail = "SELECT pa.IDArtikulli AS Shifra, \n"
                                    + " (pa.Emri + '   ' + CASE WHEN pa.Seri IS NOT NULL THEN pa.Seri ELSE '' END) AS Pershkrimi,  \n"
                                    + " a.BUM AS Njesia,  \n"
                                    + " CONVERT (Decimal(10,3),ROUND(la.SasiaLiferuar,3)) AS Sasia,  \n"
                                    + " CONVERT (Decimal(15,2),ROUND(la.CmimiPaTVSH,2)) AS [CmimiPaTVSh],  \n"
                                    + " CONVERT (Decimal(10,2),ROUND(la.TotaliPaTVSH,2)) AS [VleftaPaTVSh],  \n"
                                    + " CONVERT (Decimal(10,2),ROUND(la.Totali - la.TotaliPaTVSH,2)) AS [VlefteTVSh],  \n"
                                    + " CONVERT (Decimal(10,2),ROUND(la.Totali,2)) AS [VleftaMeTVSh],  \n"
                                    + " CONVERT (Decimal(10,2),ROUND(la.Cmimi,3)) AS [CmimiMeTVSh]  \n"
                                    + "  FROM   PorosiaArt pa \n"
                                    + "  INNER JOIN Artikujt a  \n"
                                    + "  ON  pa.IDArtikulli = a.IDArtikulli  \n"
                                    + "  INNER JOIN LiferimiArt la  \n"
                                    + "   ON  pa.IDArtikulli = la.IDArtikulli and pa.Seri = la.Seri \n"
                                    + "   INNER JOIN Liferimi l  \n"
                                    + "   ON  pa.IDPorosia = l.IDPorosia  \n"
                                    + "   WHERE     (pa.IDPorosia = '" + IDPorosia + "')   \n"
                                    + @"   AND la.IDLiferimi = l.IDLiferimi
                         ";// ORDER BY l.[DataLiferimit],l.[KohaLiferimit]";//@BE Shtuar per sortim te Faturave 28.09.2018
                ////////////////////////////////////////////////////////////////////////////

//                commDetail = "SELECT pa.IDArtikulli AS Shifra, \n"
//                                    + " (pa.Emri + '   ' + CASE WHEN pa.Seri IS NOT NULL THEN pa.Seri ELSE '' END) AS Pershkrimi,  \n"
//                                    + " a.BUM AS Njesia,  \n"
//                                    + " CONVERT (Decimal(10,3),ROUND(la.SasiaLiferuar,3)) AS Sasia,  \n"
//                                    + " CONVERT (Decimal(15,2),ROUND(100 * (la.Cmimi) / (100 + " + frmVizitat._TVHS + "),2)) AS [CmimiPaTVSh],  \n"
//                                    + " CONVERT (Decimal(10,2),ROUND(100 * (la.Totali) / (100 + " + frmVizitat._TVHS + "),2)) AS [VleftaPaTVSh],  \n"
//                                    + " CONVERT (Decimal(10,2),ROUND(la.Totali -100 * (la.Totali) / (100 + " + frmVizitat._TVHS + "),2)) AS [VlefteTVSh],  \n"
//                                    + " CONVERT (Decimal(10,2),ROUND(la.Totali,2)) AS [VleftaMeTVSh],  \n"
//                                    + " CONVERT (Decimal(10,2),ROUND(la.Cmimi,3)) AS [CmimiMeTVSh]  \n"
//                                    + "  FROM   PorosiaArt pa \n"
//                                    + "  INNER JOIN Artikujt a  \n"
//                                    + "  ON  pa.IDArtikulli = a.IDArtikulli  \n"
//                                    + "  INNER JOIN LiferimiArt la  \n"
//                                    + "   ON  pa.IDArtikulli = la.IDArtikulli and pa.Seri = la.Seri \n"
//                                    + "   INNER JOIN Liferimi l  \n"
//                                    + "   ON  pa.IDPorosia = l.IDPorosia  \n"
//                                    + "   WHERE     (pa.IDPorosia = '" + IDPorosia + "')   \n"
//                                    + @"   AND la.IDLiferimi = l.IDLiferimi
//                         ";// ORDER BY l.[DataLiferimit],l.[KohaLiferimit]";//@BE Shtuar per sortim te Faturave 28.09.2018



                //DbUtils.WriteQueryLog(commHeader);
                //DbUtils.WriteQueryLog(commFooter);
                //DbUtils.WriteQueryLog(commDetail);

                da = new SqlCeDataAdapter(commDetail, frmHome.AppDBConnectionsStr);
                da.Fill(dt_Details);
                da.SelectCommand.CommandText = commHeader;
                da.Fill(dt_Header);
                da.SelectCommand.CommandText = commFooter;
                da.Fill(dt_Footer);

            }
            catch (SqlCeException EX)
            {
                MessageBox.Show(EX.Message);
                DbUtils.WriteSQLCeErrorLog(EX);

            }
            finally
            {
                if (da != null) da.Dispose();
            }
            return dt;
        }

        public static DataTable[] CreateOrdersBill(string nrPorosise)
        {
            DataTable dt_Header = new DataTable();
            DataTable dt_Details = new DataTable();
            DataTable dt_Footer = new DataTable();
            DataTable[] result = new DataTable[3];
            result[0] = dt_Header;
            result[1] = dt_Details;
            result[2] = dt_Footer;
            SqlCeDataAdapter da = null;

            #region Header Query
            string HedaerQRY = @"SELECT 'Shitesi: ' + c.Value AS FirstHeader
                                        FROM   CompanyInfo AS c
                                        WHERE  (c.Item = 'Shitesi')

                                    UNION ALL

                                    SELECT 'Adresa: ' + c.Value AS FirstHeader
                                        FROM   CompanyInfo AS c
                                        WHERE  c.Item = 'Adresa'
                                        
                                    UNION ALL


                                    SELECT 'Tel: ' + c.Value AS FirstHeader
                                        FROM  CompanyInfo AS c
                                        WHERE  (c.Item = 'Tel')

                                    UNION ALL 

                                    SELECT 'Numri i Porosise: ' + o.IDOrder AS FirstHeader
                                        FROM   Orders AS o
                                        WHERE  (o.IDOrder = '" + nrPorosise + @"')

                                    UNION ALL 


                                    select '----------------------------------------------------------------------' as [FirstHeader]  

                                    UNION ALL 


                                    SELECT 'Bleresi: ' + CASE 
                                                              WHEN k.Emri IS NOT NULL THEN k.Emri
                                                              ELSE ''
                                                         END AS FirstHeader
                                        FROM   Klientet AS k
                                               RIGHT OUTER JOIN Orders AS o
                                                    ON  o.IDKlientDheLokacion = k.IDKlienti
                                        WHERE  (o.IDOrder = '" + nrPorosise + @"')

                                    UNION ALL 

                                    SELECT 'Adresa:' + CASE 
                                                            WHEN kl.Adresa IS NOT NULL THEN kl.Adresa
                                                            ELSE ''
                                                       END AS FirstHeader
                                        FROM   Orders AS o
                                               LEFT OUTER JOIN KlientDheLokacion AS kl
                                                    ON  kl.IDKlienti = o.IDKlientDheLokacion
                                        WHERE  (o.IDOrder = '" + nrPorosise + @"')

                                    UNION ALL 


                                    SELECT 'NRB:' + CASE 
                                                         WHEN k.NIPT IS NOT NULL THEN k.NIPT
                                                         ELSE ''
                                                    END + '   NRF:' AS FirstHeader
                                    FROM   Klientet AS k
                                           RIGHT OUTER JOIN Orders AS o
                                                ON  o.IDKlientDheLokacion = k.IDKlienti
                                    WHERE  (o.IDOrder = '" + nrPorosise + @"')";
            #endregion
            #region Details Query

            string DetailsQRY = @"SELECT od.NrRendor AS Nr,
                                   CASE 
                                                          WHEN a.Barkod IS NOT NULL THEN a.Barkod
                                                          ELSE ''
                                                     END AS Barcode,
                                   '  '+od.IDArtikulli AS Shifra,
                                   '  '+REPLACE(REPLACE(REPLACE(REPLACE(od.Emri,'ç', 'c'), 'Ç', 'C'), 'ë', 'e'),'Ë','E')  AS Pershkrimi,
     
                                   CONVERT(DECIMAL(15, 2), od.Sasia_Porositur) AS Sasia
                            FROM   Order_Details od
                                   LEFT JOIN Artikujt a
                                        ON  a.IDArtikulli = od.IDArtikulli
                            WHERE  od.IDOrder = '" + nrPorosise + @"'";

            #endregion
            #region Footer Query

            string FooterQRY = @" 
                                SELECT  
                                         (' Faturoi') AS [Faturoi], 
                                         (' Kontrolloi') AS [Kontrolloi], 
                                         (' Vozitesi') AS [Vozitesi], 
                                         (' Pranoi') AS [Pranoi] 

                                   
                                      UNION ALL  
                                      

                                      SELECT  
                                                   ('________________') AS Faturoi, 
                                                   ('________________') AS Kontrolloi, 
                                                   ('________________') AS Vozitesi, 
                                                   ('________________') AS [Pranoi]";

            #endregion

            da = new SqlCeDataAdapter("", frmHome.AppDBConnectionsStr);
            //Set CommendText to fill DataTable for Header
            da.SelectCommand.CommandText = HedaerQRY;
            da.Fill(result[0]);

            //Set CommendText to fill DataTable for Details
            da.SelectCommand.CommandText = DetailsQRY;
            da.Fill(result[1]);

            //Set CommendText to fill DataTable for Footer
            da.SelectCommand.CommandText = FooterQRY;
            da.Fill(result[2]);

            return result;
        }

        public string GetBarcode(string input)
        {
            string barcode = "";
            string[] tempArray = new string[5];
            tempArray[0] = input.Substring(0, 4);
            tempArray[1] = input.Substring(4, 2);
            tempArray[2] = input.Substring(6, 2);
            tempArray[3] = input.Substring(8, 2);
            tempArray[4] = input.Substring(10, 4);
            string[] barArray = new string[4];

            barArray[1] = tempArray[2];
            switch (tempArray[1].ToUpper())
            {
                case "PR":
                    {
                        barArray[0] = "21"; //Prishtina
                        break;
                    }
                case "GJ":
                    {
                        barArray[0] = "22"; //Gjilan
                        break;
                    }
                case "PZ":
                    {
                        barArray[0] = "23"; //Prizren
                        break;
                    }
                case "FR":
                    {
                        barArray[0] = "31"; //Ferizaj
                        break;
                    }

                case "GJK":
                    {
                        barArray[0] = "32"; //Gjakov
                        break;
                    }

                case "MT":
                    {
                        barArray[0] = "33"; //Mitrovic
                        break;
                    }

                case "KM":
                    {
                        barArray[0] = "41"; //Kamenic
                        break;
                    }

                case "PJ":
                    {
                        barArray[0] = "42"; //Pej
                        break;
                    }

            }
            switch (tempArray[3].ToUpper())
            {
                case "-L":
                    {
                        barArray[2] = "1";
                        break;
                    }
                case "-K":
                    {
                        barArray[2] = "2";
                        break;
                    }
            }
            barArray[3] = tempArray[4];
            for (int i = 0; i < barArray.Length; i++)
            {
                barcode = barcode + barArray[i];
            }
            barcode = barcode + "1111";
            return barcode;
        }

        private void PrintBarcode(PnDevice portType, string portName)
        {
            System.IO.Ports.SerialPort blutooth = null;
            switch (portType)
            {
                case PnDevice.BluetoothPort:
                    {
                        try
                        {
                            blutooth = new System.IO.Ports.SerialPort(portName.Substring(0, 4));
                            blutooth.Open();
                            byte[] a = new byte[4];

                            a[0] = 0x1B; //ESC 
                            a[1] = 0x10; //DLE             
                            a[2] = 0x42; //B
                            a[3] = 0x0C; //Ean13 Type
                            string barcode = GetBarcode(frmVizitat.NrFatures);
                            byte[] c = System.Text.ASCIIEncoding.ASCII.GetBytes(barcode);

                            //Write data to SerailPort
                            blutooth.Write(a, 0, a.Length);
                            blutooth.Write(c, 0, c.Length);
                            blutooth.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            blutooth.Close();
                        }
                        break;
                    }
            }

        }
        /// <summary>
        /// Calc sum for the cpecific column in table
        /// </summary>
        /// <param name="conn">SqlConnection</param>
        /// <param name="TableName">Name of table</param>
        /// <param name="Column">Name of column to perform Sum function</param>
        /// <returns>Sum of values else ,-1 for error during executing query</returns>
        public static Nullable<double> SumDataColumn(string TableName, string Column)
        {
            ConfigXpnParams SyncParams;
            ConfigXpn.ReadConfiguration(out SyncParams);
            Nullable<double> result = null;
            using (SqlCeConnection con = new SqlCeConnection(SyncParams.ConnectionString))
            using (SqlCeCommand cm = new SqlCeCommand("", con))
            {
                try
                {
                    string _SumQuery = @"SELECT CASE 
                                        WHEN SUM(" + Column + ") IS NOT NULL THEN CONVERT(DECIMAL(15, 2), SUM(" + Column + "))" + @"
                                        ELSE Null
                                     END AS Temp
                                         FROM   " + TableName;
                    cm.CommandText = _SumQuery;
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    string resstring = cm.ExecuteScalar().ToString();
                    if (resstring == "") { result = null; }
                    else
                    {

                        result = double.Parse(resstring);
                    }
                    con.Close();
                }
                catch (SqlCeException ex)
                {
                    result = null;
                    DbUtils.WriteSQLCeErrorLog(ex, cm.CommandText);

                }
                catch (FormatException fex)
                {
                    result = null;
                    DbUtils.WriteExeptionErrorLog(fex);
                }
            }
            return result;
        }


        /// <summary>
        /// Calc sum for the cpecific column in table
        /// </summary>
        /// <param name="conn">SqlConnection</param>
        /// <param name="TableName">Name of table</param>
        /// <param name="Column">Name of column to perform Sum function</param>
        /// <returns>Sum of values else ,-1 for error during executing query</returns>
        public static Nullable<double> SumDataColumn(string TableName, string Column, SqlCeConnection con)
        {
            Nullable<double> result = null;
            using (SqlCeCommand cm = new SqlCeCommand("", con))
            {
                try
                {
                    string _SumQuery = @"SELECT CASE 
                                        WHEN SUM(" + Column + ") IS NOT NULL THEN CONVERT(DECIMAL(15, 2), SUM(" + Column + "))" + @"
                                        ELSE Null
                                     END AS Temp
                                         FROM   " + TableName;
                    cm.CommandText = _SumQuery;
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    string qres = cm.ExecuteScalar().ToString();
                    if (qres == "") result = 0;
                    else
                    {
                        result = double.Parse(cm.ExecuteScalar().ToString());
                    }
                    con.Close();
                }
                catch (SqlCeException ex)
                {
                    result = null;
                    DbUtils.WriteSQLCeErrorLog(ex, cm.CommandText);
                }
                catch (FormatException fex)
                {
                    result = null;
                    DbUtils.WriteExeptionErrorLog(fex);
                }
                finally
                {
                    cm.Dispose();
                }
            }
            return result;

            //            SqlCeConnection con = DbUtils.MainSqlConnection;
            //            SqlCeCommand cm = new SqlCeCommand("", con);
            //            //double result = -1;

            //            Nullable<double> result = null;
            //            try
            //            {
            //                string _SumQuery = @"SELECT CASE 
            //                                        WHEN SUM(" + Column + ") IS NOT NULL THEN CONVERT(DECIMAL(15, 2), SUM(" + Column + "))" + @"
            //                                        ELSE Null
            //                                     END AS Temp
            //                                         FROM   " + TableName;
            //                cm.CommandText = _SumQuery;
            //                if (con.State == ConnectionState.Closed)
            //                {
            //                    con.Open();
            //                }

            //                result = double.Parse(cm.ExecuteScalar().ToString());


            //            }
            //            catch (SqlCeException ex)
            //            {
            //                result = null;
            //                DbUtils.WriteSQLCeErrorLog(ex, cm.CommandText);

            //            }
            //            catch (FormatException fex)
            //            {
            //                result = null;
            //                DbUtils.WriteExeptionErrorLog(fex);
            //            }
            //            finally
            //            {
            //                cm.Dispose();
            //            }
            //            return result;
        }

        public static double SumSasiaKthyer(SqlCeConnection conn)
        {
            SqlCeCommand cm = null;
            double result = -1;
            try
            {
                string _CountLiferimi = "Select  Sum(SasiaKthyer) from Malli_Mbetur";
                cm = new SqlCeCommand(_CountLiferimi, conn);
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                result = double.Parse(cm.ExecuteScalar().ToString());
            }
            catch (SqlCeException ex)
            {
                result = -1;
                DbUtils.WriteSQLCeErrorLog(ex, cm.CommandText);

            }
            finally
            {
                cm.Dispose();
            }
            return result;
        }

        public static Nullable<int> CountRows(string TableName, SqlCeConnection con)
        {
            Nullable<int> result = null;

            using (SqlCeCommand cm = new SqlCeCommand("", con))
            {
                try
                {
                    string _CountLiferimi = "Select Count(*) from [" + TableName + "]";
                    cm.CommandText = _CountLiferimi;
                    if (cm.Connection.State == ConnectionState.Closed)
                    {
                        cm.Connection.Open();
                    }
                    result = int.Parse(cm.ExecuteScalar().ToString());
                    cm.Connection.Close();
                }
                catch (SqlCeException ex)
                {
                    result = null;
                    DbUtils.WriteSQLCeErrorLog(ex, cm.CommandText);
                }
                catch (FormatException )
                {
                    result = null;
                }

            }
            return result;

            //ConfigXpnParams SyncParams;
            //ConfigXpn.ReadConfiguration(out SyncParams);
            //SqlCeConnection con = new SqlCeConnection(SyncParams.ConnectionString);
            //SqlCeCommand cm = new SqlCeCommand("", con);
            //Nullable<int> result = null;
            //try
            //{
            //    string _CountLiferimi = "Select Count(*) from Malli_Mbetur";
            //    cm = new SqlCeCommand(_CountLiferimi, con);
            //    if (con.State == ConnectionState.Closed)
            //    {
            //        con.Open();
            //    }
            //    result = int.Parse(cm.ExecuteScalar().ToString());
            //}
            //catch (SqlCeException ex)
            //{
            //    result = null;
            //    DbUtils.WriteSQLCeErrorLog(ex, cm.CommandText);

            //}
            //catch (FormatException fex)
            //{
            //    result = null;
            //}
            //finally
            //{
            //    cm.Dispose();
            //    con.Dispose();

            //}
            //return result;
        }

        public static Nullable<int> CountRows(string TableName)
        {
            ConfigXpnParams SyncParams;
            ConfigXpn.ReadConfiguration(out SyncParams);
            Nullable<int> result = null;
            using (SqlCeConnection con = new SqlCeConnection(SyncParams.ConnectionString))
            using (SqlCeCommand cm = new SqlCeCommand("", con))
            {
                try
                {
                    string _CountLiferimi = "Select Count(*) from [" + TableName + "]";
                    cm.CommandText = _CountLiferimi;
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    result = int.Parse(cm.ExecuteScalar().ToString());
                    con.Close();
                }
                catch (SqlCeException ex)
                {
                    result = null;
                    DbUtils.WriteSQLCeErrorLog(ex, cm.CommandText);
                }
                catch (FormatException )
                {
                    result = null;
                }
            }
            return result;



        }

        public static void RaportiSinkronizimit(string IDAgjenti)
        {
            string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            SmartDevicePrintEngine sd = null;
            PnPrint Rpt = null;
            PnType PT = PnType.Print;
            PnDevice devPort = PnDevice.BluetoothPort;
            frmMessage msg = null;
            string strPortName = "";

            if (msg == null) msg = new frmMessage(true, new string[]{null});
            DialogResult a = msg.ShowDialog();
            if (a == DialogResult.Abort)
            {
                return;//cancel printing
            }
            if (frmMessage.isBlutooth)
            {
                devPort = PnDevice.BluetoothPort;
                strPortName = frmHome.PnPrintPort + ":";
                PT = PnType.Print;

            }
            //if (frmMessage.isMultiplicatior)
            //{
            //    devPort = PnDevice.MultiplicatorPort;
            //    strPortName = frmHome.PnPrintPort + ":";
            //    PT = PnType.Print;

            //}
            if (frmMessage.isPreview)
            {
                devPort = PnDevice.PreviewOnly;
                strPortName = frmHome.PnPrintPort + ":";
                PT = PnType.Preview;

            }

            if (!frmMessage.isPreview)
            {
                if (!PnFunctions.AvailablePort(strPortName))
                {
                    MessageBox.Show("Porti: " + strPortName + " nuk munde te hapet");
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }
            DataTable header = new DataTable();
            DataTable detalis = new DataTable();
            SqlCeDataAdapter da = null;
            string buffer = "";
            int PrintHeight = 55; int PrintWidth = 130;
            int x = 40; int y = 1;
            int prevX = 0;
            string titleSep = "";

            try
            {

                ConfigXpnParams SyncParams;
                ConfigXpn.ReadConfiguration(out SyncParams);
                SqlCeConnection con = new SqlCeConnection(SyncParams.ConnectionString);
                string _fillHeader = @"SELECT A.Emri + ' ' + A.Mbiemri AS Shitesi,
                                                   a.Depo,
                                                   GETDATE() AS DATA
                                            FROM   Agjendet a
                                            WHERE  a.IDAgjenti = '" + IDAgjenti + "' ";
                string _fillDetails = @"SELECT  Convert (NCHAR (5),COUNT(*)) +'     Hedera te faturave per sinkronizim' AS Raporti FROM Liferimi l
                                            UNION All
                                            SELECT Convert (NCHAR (5),COUNT(*)) +'     Lines te faturave per sinkronizim' FROM LiferimiArt la
                                            UNION ALL
                                            SELECT Convert (NCHAR (5),COUNT(*)) +'     Inkasime per sinkronzim' FROM EvidencaPagesave ep
                                            UNION ALL 
                                            SELECT Convert (NCHAR (5),COUNT(*)) +'     Artikuj per shkarkim' FROM Malli_Mbetur mm";

                da = new SqlCeDataAdapter("", con);
                da.SelectCommand.CommandText = _fillHeader;
                da.Fill(header);

                da.SelectCommand.CommandText = _fillDetails;
                da.Fill(detalis);

                if (header.Rows.Count == 0)
                {
                    MessageBox.Show("Mungon konfigurimi i Psionit \n");
                    return;
                }

                sd = new SmartDevicePrintEngine(devPort, strPortName);

                sd.CreatePage(PrintWidth, PrintHeight);
                int c = 0;
                x = 20;
                c += 1;

                buffer = "R A P O R T I  I  S I N K R O N I Z I M I T";
                StringBuilder tmpBuffer = new StringBuilder("");
                tmpBuffer.Append(' ', PrintWidth / 2 - buffer.Length / 2);
                tmpBuffer.Append(buffer);
                buffer = tmpBuffer.ToString();
                prevX = 1;
                sd.WriteToLogicalPage(prevX, y, buffer);
                y++;

                WriteSeparator(ref x, ref y, ref sd, "-"); //E shtuar

                //------------------TITLE SEPERATOR-------------------------
                sd.WriteToLogicalPage(prevX, y, titleSep);


                y += 1;
                buffer = "     Shitesi: " + header.Rows[0][0].ToString();
                sd.WriteToLogicalPage(x, y, buffer);


                y += 1;
                buffer = "     Data e printimit: " + header.Rows[0][2].ToString();
                sd.WriteToLogicalPage(x, y, buffer);


                y += 1;
                buffer = "     Depo: " + header.Rows[0][1].ToString();
                sd.WriteToLogicalPage(x, y, buffer);


                y += 1;
                WriteSeparator(ref x, ref y, ref sd, "=");
                sd.WriteToLogicalPage(prevX, y, titleSep);


                y += 1;
                buffer = "     Nr    Sinkronizimi";
                sd.WriteToLogicalPage(x, y, buffer);

                WriteSeparator(ref x, ref y, ref sd, "-");
                sd.WriteToLogicalPage(prevX, y, titleSep);

                y += 1;
                buffer = "     " + detalis.Rows[0][0].ToString();
                sd.WriteToLogicalPage(x, y, buffer);

                y += 1;
                buffer = "     " + detalis.Rows[1][0].ToString();
                sd.WriteToLogicalPage(x, y, buffer);

                y += 1;
                buffer = "     " + detalis.Rows[2][0].ToString();
                sd.WriteToLogicalPage(x, y, buffer);

                y += 1;
                buffer = "     " + detalis.Rows[3][0].ToString();
                sd.WriteToLogicalPage(x, y, buffer);

                y += 1;
                WriteSeparator(ref x, ref y, ref sd, "-");
                sd.WriteToLogicalPage(prevX, y, titleSep);
                y += 1;

                buffer = "     Printuar nga Sistemi MobSell   Pronet - Kosove +381 38 557799";
                sd.WriteToLogicalPage(x, y, buffer);

                sd.Print();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                PnUtils.DbUtils.WriteExeptionErrorLog(ex);

            }
            finally
            {
                detalis.Dispose();
                header.Dispose();
                da.Dispose();
                if (Rpt != null) Rpt.Dispose();
                if (msg != null) msg.Dispose();
                frmMessage.isPreview = false;
                frmMessage.isBlutooth = false;
                frmMessage.isMultiplicatior = false;
                msg = null;
                Cursor.Current = Cursors.Default;
            }
        }

        public static string getNurFatures(string KMAG)
        {
            string result = "";
            int NrFat = 0;
            bool aprovim = frmHome.AprovimFaturash;
            string sql = "SELECT Count(*) FROM NumriFaturave nf WHERE nf.KOD='" + KMAG + "'";
            int _numRows = int.Parse(DbUtils.ExecSqlScalar(sql));
            if (_numRows == 0)
            {
                result = "Error:Konfigurim";

            }
            else
            {

                if (aprovim)//aprovim=1
                {

                    /*(NumriFaturave = nf)
                     *Lexo nf.CurrNrFat, nf.NRKUFIP, nf.NRKUFIS where nf.KOD='" + KMAG + "'"
                     *NrFatures = nf.CurrNrFat + nf.NRKUFIP
                     *nese NrFatures > nf.NRKUFIS, ndalet shitja.
                     *nf.CurrNrFat = nf.CurrNrFat + 1
                     **/

                    ///*Marrim sa fatura deri me tani i kemi realizuarme kete brez
                    string _getCurrNrFat = "SELECT nf.CurrNrFat FROM NumriFaturave nf WHERE nf.KOD='" + KMAG + "'";
                    int _CurrNrFat = int.Parse(DbUtils.ExecSqlScalar(_getCurrNrFat).ToString());

                    ///*Marrim NRKUFIP per shitesin konkret */
                    string _getNRKUFIP = "SELECT nf.NRKUFIP FROM NumriFaturave nf WHERE nf.KOD='" + KMAG + "'";
                    int tempNRKUFIP = int.Parse(DbUtils.ExecSqlScalar(_getNRKUFIP));

                    ///*Marrim NRKUFIS per shitesin konkret */
                    string _getNRKUFIS = "SELECT nf.NRKUFIS FROM NumriFaturave nf WHERE nf.KOD='" + KMAG + "'";
                    int tempNRKUFIS = int.Parse(DbUtils.ExecSqlScalar(_getNRKUFIS));

                    NrFat = _CurrNrFat + tempNRKUFIP;

                    if (NrFat > tempNRKUFIS)
                    {
                        MessageBox.Show("Tejkalimi i numrit te faturave");
                        result = "Tejkalim";
                    }
                    else
                    {
                        result = NrFat.ToString();

                    }

                }
                else //aprovim=0
                {

                    ///*Marrim sa fatura deri me tani i kemi realizuarme kete brez
                    string _getCurrNrFat = "SELECT nf.CurrNrFatJT FROM NumriFaturave nf WHERE nf.KOD='" + KMAG + "'";
                    int _CurrNrFatJT = int.Parse(DbUtils.ExecSqlScalar(_getCurrNrFat).ToString());

                    /*Marrim NRKUFIPJT per shitesin konkret */
                    string _getNRKUFIPJT = "SELECT nf.NRKUFIPJT FROM NumriFaturave nf WHERE nf.KOD='" + KMAG + "'";
                    int tempNRKUFIPJT = int.Parse(DbUtils.ExecSqlScalar(_getNRKUFIPJT));

                    /*Marrim NRKUFISJT per shitesin konkret */
                    string _getNRKUFISJT = "SELECT nf.NRKUFISJT FROM NumriFaturave nf WHERE nf.KOD='" + KMAG + "'";
                    int temoNRKUFISJT = int.Parse(DbUtils.ExecSqlScalar(_getNRKUFISJT));

                    /*Kontrollojme tejkalimin eventual ne NRKUFIP dhe NRKUFIS */
                    NrFat = _CurrNrFatJT + tempNRKUFIPJT;

                    if (NrFat > temoNRKUFISJT)
                    {
                        MessageBox.Show("Tejkalimi i numrit te faturave");
                        result = "Tejkalim";
                    }
                    else
                    {
                        result = NrFat.ToString();

                    }
                }
            }
            return result;
        }

    }

    //Used in SyncAll method in frmSync.cs
    public enum MobSellSyncDirection : int
    {
        SnapshotSpecial = -2,//won't do the snapshot if the server returns no record
        Snapshot = -1, //snapshot tableData returned by server
        Download = 0, //downloads with Insert if new, and Update if existing
        Upload = 1,   //Uploads with Insert if new, and Update if existing
        Bidirectional = 2, //a).Uploads b.)Downloads with Insert if new, and Update if existing
        BidirectionalSpecial = 3, //a.)Uploads to server in TableName+ _upl b.) Downloads to original table in device
        DownloadWithInsert = 4, //Downloads with insert if new, (no update)
        DownloadSpecial = 5,//downloads with insert new records and update exisiting except update exsiting changes to syncSatus=2
        LogUpload = 6,//When Uploading Log this is the indicator
        UploadWithInsert = 7 //uploads records and insert new only (no update)
    }

    #region ApplicationConfigXpn

    public struct ConfigXpnParams
    {
        public string DeviceID;
        public string OperatorCode;
        public string LocalDbPath;
        public string MobileServerUrl;
        public string WebServiceURL;
        public string PrintPort;
        public string LocalDbPassword;
        public string ServerName;
        public string DbName;
        public string UserName;
        public string Password;
        public string ConnectionString { get; set; }
        public string WebServerFiskalizimiUrl;

    }

    public class ConfigXpn
    {
        public static void ReadConfiguration(out ConfigXpnParams config)
        {
            ConfigXpnParams configXpn = new ConfigXpnParams();
            string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            string localDbPassSnc = "";

            DataSet dsConfig = new DataSet();
            try
            {
                dsConfig.ReadXml(CurrentDir + "\\Config.xpn");
                configXpn.DeviceID = dsConfig.Tables[0].Rows[0]["DeviceID"].ToString().Trim();
                configXpn.LocalDbPath = dsConfig.Tables[0].Rows[0]["LocalDbPath"].ToString();
                configXpn.MobileServerUrl = dsConfig.Tables[0].Rows[0]["MobileServerUrl"].ToString();
                configXpn.PrintPort = dsConfig.Tables[0].Rows[0]["PrintPort"].ToString();
                configXpn.WebServiceURL = dsConfig.Tables[0].Rows[0]["WebServerUrl"].ToString();
                configXpn.WebServerFiskalizimiUrl = dsConfig.Tables[0].Rows[0]["WebServerFiskalizimiUrl"].ToString();
                configXpn.OperatorCode = dsConfig.Tables[0].Rows[0]["OperatorCode"].ToString();

                try
                {
                    configXpn.LocalDbPassword = dsConfig.Tables[0].Rows[0]["LocalDbPassword"].ToString();
                    localDbPassSnc = configXpn.LocalDbPassword;
                    if (localDbPassSnc.Trim() != "")
                        localDbPassSnc = CryptorEngine.Decrypt(configXpn.LocalDbPassword, "testKey", true);
                    configXpn.LocalDbPassword = localDbPassSnc;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gabim ne ReadConfiguration:" + ex.Message);
                    //DbUtils.WriteSQLCeErrorLog(scex, "");
                }
                configXpn.ServerName = dsConfig.Tables[1].Rows[0]["ServerName"].ToString();
                configXpn.DbName = dsConfig.Tables[1].Rows[0]["DbName"].ToString();
                configXpn.UserName = dsConfig.Tables[1].Rows[0]["UserName"].ToString();
                configXpn.Password = dsConfig.Tables[1].Rows[0]["Password"].ToString();
                configXpn.Password = CryptorEngine.Decrypt(configXpn.Password, "testKey", true);

                configXpn.ConnectionString = "DATA SOURCE=" + configXpn.LocalDbPath;

                if (configXpn.LocalDbPassword.Trim() != "")
                    configXpn.ConnectionString += ";Password=" + configXpn.LocalDbPassword;



            }
            finally
            {
                config = configXpn;
                dsConfig.Dispose();
            }
        }

        public static void WriteConfiguration(ConfigXpnParams config)
        {
            string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);

            DataSet dsConfig = new DataSet();

            try
            {
                dsConfig.ReadXml(CurrentDir + "\\Config.xpn");
                dsConfig.Tables[0].Rows[0]["DeviceID"] = config.DeviceID;
                dsConfig.Tables[0].Rows[0]["LocalDbPath"] = config.LocalDbPath;
                dsConfig.Tables[0].Rows[0]["MobileServerUrl"] = config.MobileServerUrl;
                dsConfig.Tables[0].Rows[0]["PrintPort"] = config.PrintPort;

                dsConfig.Tables[1].Rows[0]["ServerName"] = config.ServerName;
                dsConfig.Tables[1].Rows[0]["DbName"] = config.DbName;
                dsConfig.Tables[1].Rows[0]["UserName"] = config.UserName;
                dsConfig.Tables[0].Rows[0]["OperatorCode"] = config.OperatorCode;
                dsConfig.Tables[0].Rows[0]["WebServerFiskalizimiUrl"] = config.WebServerFiskalizimiUrl;

                try
                {
                    dsConfig.Tables[0].Rows[0]["LocalDbPassword"] = CryptorEngine.Encrypt(config.LocalDbPassword, "testKey", true);
                    dsConfig.Tables[1].Rows[0]["Password"] = CryptorEngine.Encrypt(config.Password, "testKey", true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Gabim ne WriteConfiguration:" + ex.Message);
                }

                dsConfig.WriteXml(CurrentDir + "\\Config.xpn");

            }
            finally
            {
                dsConfig.Dispose();
            }

        }

    }

    #endregion
}
