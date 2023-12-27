using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using PnReports;
using System.IO;
using System.Reflection;
//using PsionTeklogix.Serial;
using System.IO.Ports;
using PnUtils;
using System.Runtime.InteropServices;
using MobileSales.BL;

namespace MobileSales
{
    public partial class frmLiferimi : Form
    {
        private bool Key, ISTheFirstTime, isSelected = false;
        private string EmriAgjentit, Lokacioni, KontaktEmri, KlientiEmri;
        private double Totali;
        public static string strPortName = "";
        public PnDevice devPort;
        private const UInt32 STILL_ACTIVE = 0x00000103;
        public static string curentDir;

        #region Initialize Instance

        SmartDevicePrintEngine sd = null;
        frmMessage msg = null;

        #endregion

        public frmLiferimi()
        {
            InitializeComponent();
        }

        private void InitConnection()
        {
            try
            {
                if (frmHome.AppDBConnection.State == ConnectionState.Closed)
                {
                    frmHome.AppDBConnection.Open();
                }
                liferimiArtTableAdapter.Connection.ConnectionString = frmHome.AppDBConnectionsStr;
                liferimiTableAdapter.Connection.ConnectionString = frmHome.AppDBConnectionsStr;
                vizitatTableAdapter1.Connection.ConnectionString = frmHome.AppDBConnectionsStr;
                porosiaArtTableAdapter1.Connection.ConnectionString = frmHome.AppDBConnectionsStr;
            }
            catch (SqlCeException EX)
            {
                DbUtils.WriteSQLCeErrorLog(EX);
            }
            catch (Exception EX)
            {
                DbUtils.WriteExeptionErrorLog(EX);
            }
        }

        private void frmLiferimi_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            InitConnection();
            try
            {
                this.liferimiTableAdapter.FillByIDPorosia(this.myMobileDataSet.Liferimi, frmVizitat.IDPorosi);
                this.liferimiArtTableAdapter.Fill(this.myMobileDataSet.LiferimiArt, frmVizitat.IDLiferimi);

                this.vizitatTableAdapter1.SelectLokacioni(this.myMobileDataSet.Vizitat, frmVizitat.CurrIDV);
                lblAdresa.Text = (myMobileDataSet.Vizitat[0]["EmriLokacionit"]).ToString();// 0 is Index of Lokacioni column
                lblDataLiferimit.Text = DateTime.Now.ToString("dd-MM-yyyy");
                lblTot.Text = "Totali: " + String.Format("{0:#,0.00}", decimal.Parse((myMobileDataSet.Liferimi[0]["CmimiTotal"]).ToString()));// 0 is Index of Lokacioni column

            }
            catch (SqlCeException sce)
            {
                MessageBox.Show(sce.Message);
                DbUtils.WriteSQLCeErrorLog(sce);
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message);
                DbUtils.WriteExeptionErrorLog(EX);
            }
            ISTheFirstTime = true;
            Cursor.Current = Cursors.Default;
        }



        private void btnMbyllja_Click(object sender, EventArgs e)
        {
            if (Totali == 0)
            {
                MessageBox.Show("Nuk lejohet mbyllja e liferimit pa liferuar.");

            }
            else
            {
                if (MessageBox.Show("Jeni të sigurt për mbylljen e liferimit ?", "Liferimi", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    try
                    {
                        this.liferimiBindingSource.EndEdit();


                        /*** Ndryshimi i statusit të Liferimit ***/
                        this.liferimiTableAdapter.ChangeStatusLiferimi(1, frmVizitat.IDLiferimi);
                        this.Close();
                    }
                    catch (SqlCeException sqc)
                    {
                        MessageBox.Show(sqc.Message);
                    }
                    finally
                    {
                        this.Close();
                    }
                }
                else
                { //Do nothing
                }
            }

        }

        private void dtpLfr_ValueChanged(object sender, EventArgs e)
        {
            dtpLfr.MinDate = Convert.ToDateTime(System.DateTime.Now.ToShortDateString());
        }



        #region DataGrid && BindingSource

        private void liferimiArtDataGrid_MouseUp(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.DataGrid.HitTestInfo myHitTest = this.liferimiArtDataGrid.HitTest(e.X, e.Y);
            if (myHitTest.Type == DataGrid.HitTestType.Cell)
            {
                this.liferimiArtDataGrid.Select(myHitTest.Row);
                isSelected = true;
            }
        }

        private void liferimiArtDataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            Key = (e.KeyData == Keys.Enter || e.KeyData == Keys.Up || e.KeyData == Keys.Down || e.KeyData == Keys.Left || e.KeyData == Keys.Right);
            if (isSelected == true)
            {
                if (Key)
                {
                    this.liferimiArtDataGrid.Select(this.liferimiArtDataGrid.CurrentRowIndex);

                }

            }
        }

        private void liferimiArtBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            Totali = Convert.ToDouble(this.porosiaArtTableAdapter1.Totali(frmVizitat.IDPorosi).ToString());
            lblTotali.Text = String.Format("{0:0.00 }", Totali);
        }

        #endregion

        #region Methods and Functions

        private void PrintoFletePagesn()
        {

            string NrPageses = "";
            SqlCeDataAdapter da2 = null;
            da2 = new SqlCeDataAdapter("", frmHome.AppDBConnectionsStr);

            DataTable dtTemp = new DataTable();
            da2.SelectCommand.CommandText = "select * from Agjendet where IDAgjenti='" + frmHome.IDAgjenti + "'";
            da2.Fill(dtTemp);
            if (dtTemp.Rows.Count > 0)
                EmriAgjentit = dtTemp.Rows[0]["Emri"].ToString() + " " + dtTemp.Rows[0]["Mbiemri"].ToString();

            da2.SelectCommand.CommandText = "Select * from KlientDheLokacion where IDKlientDheLokacion='" + frmVizitat.Vizitat_IDKlienti + "'";
            dtTemp.Clear();
            da2.Fill(dtTemp);
            if (dtTemp.Rows.Count > 0)
                Lokacioni = dtTemp.Rows[0]["EmriLokacionit"].ToString();
            KontaktEmri = dtTemp.Rows[0]["KontaktEmriMbiemri"].ToString();

            dtTemp.Clear();

            da2.SelectCommand.CommandText = "SELECT * FROM Klientet where IDKlienti='" + frmVizitat.Vizitat_IDKlienti + "'";
            dtTemp.Clear();
            da2.Fill(dtTemp);
            if (dtTemp.Rows.Count > 0)
            {
                KlientiEmri = dtTemp.Rows[0]["Emri"].ToString();

            }
            else
            {
                KlientiEmri = "";
            }

            da2.SelectCommand.CommandText = "SELECT TOP(1) CASE  \n"
                                           + "                   WHEN ep.NrFatures IS NOT NULL THEN ep.NrFatures \n"
                                           + "                   ELSE '' \n"
                                           + "              END AS NrFatures, \n"
                                           + "       ep.NrPageses \n"
                                           + "FROM   EvidencaPagesave ep \n"
                                           + "ORDER BY \n"
                                           + "       ep.DataPageses DESC";
            dtTemp.Clear();
            da2.Fill(dtTemp);
            if (dtTemp.Rows.Count > 0)
            {
                NrPageses = dtTemp.Rows[0]["NrPageses"].ToString();

            }
            DataSet temp = new DataSet();
            string companyName = "";
            string Nipt = "";
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
               (frmShitja._tempNrFatures.ToString(), NrPageses, DateTime.Now, companyName, Nipt, EmriAgjentit, frmHome.IDAgjenti,
               KlientiEmri, Lokacioni, KontaktEmri, DateTime.Now, dlgMbylljaFatures.ShumaTotale.ToString(), dlgMbylljaFatures.ShumaPaguar.ToString(), dlgMbylljaFatures.KMON);
        }

        private void PirntoInkasimin(string strNrFatures, string strNrPageses, DateTime dtDataPageses, string strKompania, string strBusinessNo,
                                     string strEmriAgjentit, string strIDAgjenti, string strIDKlienti, string strLokacioni, string strKontaktEmri,
                                     DateTime dtPagesatArritura, string strShuma, string strInkasuar, string KMON)
        {

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
                sd.WriteToLogicalPage(prevX, y, titleSep);


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
                buffer = "     Shuma e fatures: " + Math.Round(decimal.Parse(strShuma), 2) + " " + KMON;
                sd.WriteToLogicalPage(x, y, buffer);

                y += 1;
                buffer = "     Pagesa e inkasuar: " + Math.Round(decimal.Parse(strInkasuar), 2) + " " + KMON + " " + dlgMbylljaFatures._PayType;
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
                buffer = "     Printuar nga Sistemi MobSell Asseco - Kosove +383 38 40 77 99";
                sd.WriteToLogicalPage(x, y, buffer);

                y += 5;

                // if (c == 1) goto a;

                //sd.AddToPreview();
                // sd.Preview();
                sd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            finally
            {
                if (sd != null) sd.Dispose();
                if (msg != null) msg.Dispose();
                msg = null;
            }

        }
        private void WriteSeparator(ref int x, ref int y, ref SmartDevicePrintEngine PrintEngine, string CharSeperator)
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

        #endregion

        #region dllImport

        internal static int MbToWc(uint CodePage, byte[] MultiByteStr, int MbLen, char[] WideCharStr, int WcLen)
        {
            return CEMultiByteToWideChar(CodePage /*CP_ACP*/, 0, MultiByteStr, MbLen, WideCharStr, WcLen);
        }

        internal static int WcToMb(uint CodePage, String WideCharStr, int WcLen, byte[] MultiByteStr, int MbLen)
        {
            return CEWideCharToMultiByte(CodePage /*CP_ACP*/, 0, WideCharStr, WcLen, MultiByteStr, MbLen, IntPtr.Zero, IntPtr.Zero);
        }

        [DllImport("coredll.dll", EntryPoint = "WideCharToMultiByte", SetLastError = true)]
        private static extern int CEWideCharToMultiByte(uint CodePage, uint dwFlags,
            String lpWideCharStr, int cchWideChar, byte[] lpMultiByteStr, int cbMultiByte,
            IntPtr lpDefaultChar, IntPtr lpUsedDefaultChar);

        [DllImport("coredll.dll", EntryPoint = "MultiByteToWideChar", SetLastError = true)]
        private static extern int CEMultiByteToWideChar(uint CodePage, uint dwFlags,
            byte[] lpMultiByteStr, int cbMultiByte, char[] lpWideCharStr, int cchWideChar);

        [DllImport("coredll.dll", SetLastError = true)]
        private static extern bool CreateProcess(String imageName,
                                         String cmdLine,
                                         IntPtr lpProcessAttributes,
                                         IntPtr lpThreadAttributes,
                                         bool boolInheritHandles,
                                         Int32 dwCreationFlags,
                                         IntPtr lpEnvironment,
                                         IntPtr lpszCurrentDir,
                                         byte[] si,
                                         ProcessInfo pi);

        [DllImport("coredll.dll", SetLastError = true)]
        private static extern bool GetExitCodeProcess(IntPtr hProcess, ref UInt32 lpExitCode);

        #endregion

        private class ProcessInfo
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public Int32 ProcessId;
            public Int32 ThreadId;
        }

        private void Print()
        {
            try
            {
                SqlCeDataAdapter da2 = new SqlCeDataAdapter("", frmHome.AppDBConnectionsStr);
                DataTable dtTemp = new DataTable();
                string IDKlienti = frmVizitat.Vizitat_IDKlienti;
                string NrFatures = frmShitja._tempNrFatures.ToString();
                string NrPageses = "";
                da2.SelectCommand.CommandText = "SELECT TOP(1) CASE  \n"
                                               + "                   WHEN ep.NrFatures IS NOT NULL THEN ep.NrFatures \n"
                                               + "                   ELSE '' \n"
                                               + "              END AS NrFatures, \n"
                                               + "       ep.NrPageses \n"
                                               + "FROM   EvidencaPagesave ep \n"
                                               + "ORDER BY \n"
                                               + "       ep.DataPageses DESC";
                dtTemp.Clear();
                da2.Fill(dtTemp);
                if (dtTemp.Rows.Count > 0)
                {
                    NrPageses = dtTemp.Rows[0]["NrPageses"].ToString();

                }

                PnFunctions.PrintoFletePagesen(NrFatures, IDKlienti, NrPageses, false, devPort);
            }
            catch (Exception ex)
            { DbUtils.WriteExeptionErrorLog(ex); }
        }

        private void Print_To_Fiscal(string nrFatures, string totali)
        {
            byte[] BytesToWrite = new byte[200];
            FileStream hFile = new FileStream(curentDir + "\\mySample.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            string t = nrFatures.PadLeft(9);
            string SellItem = string.Format("\"P2\";\"{7}\";\n\r\"S\";\"{0}\";\"{1}\";\"{2}\";\"{3}\";\"{4}\";\"{5}\";\"{6}\";\n\r", "ARKETIM", "", "1", (int.Parse(totali)), "2", "", "", "Fatura " + t);
            WcToMb(0, SellItem, -1, BytesToWrite, 200);
            hFile.Write(BytesToWrite, 0, SellItem.Length);
            WcToMb(0, "\"T\";\"\";\"\";\"0\";\"0\";\n\r\"F\";", -1, BytesToWrite, 200);
            hFile.Write(BytesToWrite, 0, 22);
            hFile.Close();
            ProcessInfo ProcInfo = new ProcessInfo();

            try
            {
                byte[] si = new byte[128];

                CreateProcess("\\Program Files\\Datecs Applications\\FPrintPPC\\FPrintPPC.exe",
                              "\"" + curentDir + "\\mySample.txt\"",
                              IntPtr.Zero, IntPtr.Zero, false, 0,
                              IntPtr.Zero, IntPtr.Zero, si, ProcInfo);

                UInt32 ec = 0;
                Cursor.Current = Cursors.WaitCursor;
                for (; ; )
                {
                    GetExitCodeProcess(ProcInfo.hProcess, ref ec);
                    if (ec == STILL_ACTIVE)
                        System.Threading.Thread.Sleep(300);
                    else
                        break;
                }
                Cursor.Current = Cursors.Default;
                if (ec == 0)
                {
                    byte[] read = new byte[200];

                    FileStream hFileZ1 = new FileStream("\\Program Files\\Datecs Applications\\FPrintPPC\\result.txt", FileMode.Open, FileAccess.Read, FileShare.None, 200);
                    hFileZ1.Read(read, 0, 200);
                    hFileZ1.Close();

                    string retstr = ASCIIEncoding.ASCII.GetString(read, 0, 200);

                    string[] arr = retstr.Split(';');
                    MessageBox.Show("FPrintPPC Returned: " + arr[2]);
                }
                else
                {
                    MessageBox.Show("FPrintPPC exited with code: " + ec.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void menu_Print_Click(object sender, EventArgs e)
        {
            string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            PnPrint Rpt = null;
            PnType PT = PnType.Print;
            string _IDPorosia = frmVizitat.IDPorosi.ToString();
            DataTable details = new DataTable();
            DataTable header = new DataTable();
            DataTable footer = new DataTable();
            DataTable TCRQRCodeTbl = new DataTable();

            //@BE QR Code
            PnUtils.DbUtils.FillDataTable(TCRQRCodeTbl, @"
                                                            Select NrLiferimit, CmimiTotal, TCRQRCodeLink, PayType, Message 
															from Liferimi
															where NrLiferimit = '" + nrLiferimitLabel1.Text + @"'
															and TCRNSLF <> ''
                                                        ");
            if (TCRQRCodeTbl.Rows.Count > 0)
            {
                if (msg == null)
                {
                    msg = new frmMessage(true, new string[] { TCRQRCodeTbl.Rows[0]["CmimiTotal"].ToString(),
                                                              TCRQRCodeTbl.Rows[0]["NrLiferimit"].ToString(), 
                                                              TCRQRCodeTbl.Rows[0]["TCRQRCodeLink"].ToString(), 
                                                              TCRQRCodeTbl.Rows[0]["Message"].ToString() });
                }
            }
            else
            {
                if (msg == null) msg = new frmMessage(true, new string[4]);

            }

            try
            {

                #region PrintParams
                DialogResult a = msg.ShowDialog();

                if (a == DialogResult.Abort)
                {
                    return;//cancel printing
                }

                if (frmMessage.isBlutooth)
                {
                    PT = PnType.Print;
                    devPort = PnDevice.BluetoothPort;
                    strPortName = frmHome.PnPrintPort + ":";
                }
                if (frmMessage.isPreview)
                {
                    PT = PnType.Preview;
                    devPort = PnDevice.PreviewOnly;
                }
                //if (frmMessage.isMultiplicatior)
                //{
                //    devPort = PnDevice.MultiplicatorPort;
                //    strPortName = frmHome.PnPrintPort + ":";
                //    PT = PnType.Print;
                //}
                if (!frmMessage.isPreview)
                {
                    if (!PnFunctions.AvailablePort(strPortName))
                    {
                        MessageBox.Show("Porti: " + strPortName + " nuk munde te hapet");
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                #endregion

                DataTable[] Header_Details = PnFunctions.GenerateBill(frmHome._magType, _IDPorosia);
                details = Header_Details[0];
                header = Header_Details[1];
                footer = Header_Details[2];

                if (details.Rows.Count > 0 && header.Rows.Count > 0)
                {
                    /*@BE 17.09.2018 ADDED for sorting by the first column*/
                    //DataView dvs = header.DefaultView;
                    //dvs.Sort = "Fatura ASC";
                    //header = dvs.ToTable();
                    /*@BE 17.09.2018 ADDED for sorting by the first column*/


                    Cursor.Current = Cursors.WaitCursor;
                    if (frmVizitat.KthimMall)
                    {
                        Rpt = new PnPrint(devPort, strPortName, CurrentDir + "\\FaturaKthim.pnrep", details, header, footer);
                        Rpt.PrintPnReport(PT, true, " tre ", false);
                    }
                    else
                    {
                        Rpt = new PnPrint(devPort, strPortName, CurrentDir + "\\FaturaParapaguese2.pnrep", details, header, footer);
                        Rpt.PrintPnReport(PT, true, " tre ", false);
                    }

//                    DataTable dtFooter2 = new DataTable();

//                    string sr = @"SELECT        SUM(CONVERT (Decimal(10,2),ROUND(la.Totali,2)))
//                            FROM          PorosiaArt pa
//                             INNER JOIN Liferimi l ON  pa.IDPorosia = l.IDPorosia
//                            INNER JOIN LiferimiArt la   ON  pa.IDArtikulli = la.IDArtikulli and pa.Seri = la.Seri
//
//                                                     
//                            WHERE        (pa.IDPorosia = '" + _IDPorosia + "') AND la.IDLiferimi = l.IDLiferimi";



//                    string s = String.Format("{0:#,0.00#}", float.Parse((DbUtils.ExecSqlScalar(sr).ToString())));


//                    string commFooter2 = @" 
//                                      select ('NSLF: ' + CASE 
//                                                              WHEN l.TCRNSLF IS NOT NULL THEN l.TCRNSLF
//                                                              ELSE ''
//                                                         END) AS [FooterText]
//                                      FROM  Liferimi l  
//                                      WHERE  l.IDPorosia = '" + _IDPorosia + @"'
//
//                                      Union All
//
//                                      select ('NIVF: ' + CASE 
//                                                              WHEN l.TCRNIVF IS NOT NULL THEN l.TCRNIVF
//                                                              ELSE ''
//                                                         END) AS [FooterText]
//                                      FROM  Liferimi l  
//                                      WHERE  l.IDPorosia = '" + _IDPorosia + @"'
//                                      
//                                      UNION ALL  
//
//                                      select '            ' as [FooterText]
//
//                                      Union All
//
//                                      select 'Menyra e pageses: ' AS [FooterText]
//
//                                      Union All
//
//                                      
//                                      SELECT 'Lloji: ' + (case when l.PayType = 'KESH' then 'Kartemonedha dhe monedha'
//                                                                    else 'Llogari e transaksionit' end) AS [FooterText]
//                                      FROM  Liferimi l  
//                                      WHERE  l.IDPorosia = '" + _IDPorosia + @"'
//
//                                      Union All
//
//                                      SELECT ('Sasi(LEK): ' + '" + s + @"')  as [FooterText]
//                                      ";

//                    PnUtils.DbUtils.FillDataTable(dtFooter2, commFooter2);

//                    if (dtFooter2.Rows.Count > 0)
//                    {
//                        for (int i = 0; i < dtFooter2.Rows.Count; i++)
//                        {
//                            BxlPrint.PrintText(dtFooter2.Rows[i]["FooterText"].ToString(), BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
//                            BxlPrint.LineFeed(1);
//                        }
//                    }

                    DataTable totalet = new DataTable();
                    string totaliPaTVSH = "";
                    string totaliTVSH = "";
                    string totaliMeTVSH = "";
                    string menyraPageses = "";

                    PnUtils.DbUtils.FillDataTable(totalet, @"SELECT  ('S-VAT') AS [Kodi],
                                                    ('20 ') AS [Shkalla], 
                                                    SUM(CONVERT (Decimal(10,2),ROUND(la.TotaliPaTVSH,2)))as TotaliPaTVSH,
                                                    SUM(CONVERT (Decimal(10,2),ROUND(la.Totali,2))) - SUM(CONVERT (Decimal(10,2),ROUND(la.TotaliPaTVSH,2))) TVSH,
                                                    SUM(CONVERT (Decimal(10,2),ROUND(la.Totali,2))) Totali,
                                                    case when min(l.PayType) = 'Bank' then 'Afati pageses: ' + (CONVERT(NCHAR(10), GETDATE() + 30, 104))
                                                    else '' end as MenyraPageses
                                                    FROM          PorosiaArt pa
                                                     INNER JOIN Liferimi l ON  pa.IDPorosia = l.IDPorosia
                                                    INNER JOIN LiferimiArt la   ON  pa.IDArtikulli = la.IDArtikulli and pa.Seri = la.Seri
                        
                                                                             
                                                    WHERE (pa.IDPorosia = '" + _IDPorosia + @"') AND la.IDLiferimi = l.IDLiferimi");




                    if (totalet.Rows.Count > 0)
                    {
                        totaliPaTVSH = String.Format("{0:##0.00#}", decimal.Parse((totalet.Rows[0]["TotaliPaTVSH"].ToString())));
                        totaliTVSH = String.Format("{0:##0.00#}", decimal.Parse((totalet.Rows[0]["TVSH"].ToString())));
                        totaliMeTVSH = String.Format("{0:##0.00#}", decimal.Parse((totalet.Rows[0]["Totali"].ToString())));
                        menyraPageses = totalet.Rows[0]["MenyraPageses"].ToString();
                    }
                    else
                    {
                        totaliPaTVSH = "";
                        totaliTVSH = "";
                        totaliMeTVSH = "";
                    }

                    if (totaliTVSH.Length <= 6)
                        totaliTVSH = "  " + totaliTVSH;

                    if (totaliMeTVSH.Length <= 6)
                        totaliMeTVSH = "   " + totaliMeTVSH.PadLeft(2);

                    if (totaliTVSH.Length <= 7)
                        totaliTVSH = " " + totaliTVSH;

                    if (totaliMeTVSH.Length <= 7)
                        totaliMeTVSH = "  " + totaliMeTVSH.PadLeft(2);

                    if (totaliMeTVSH.Length <= 8)
                        totaliMeTVSH = " " + totaliMeTVSH.PadLeft(2);

//                    commFooter = @"         
//                                            SELECT  
//                                             ('Shtese ne nivel') AS [Kodi], 
//                                             ('fature') AS [Shkalla], 
//                                             ('') AS [Vlera], 
//                                             ('') AS [TVSH] 
//    
//                                       
//                                            UNION ALL
//    
//                                            SELECT  
//                                             ('===============') AS [Kodi], 
//                                             ('================') AS [Shkalla], 
//                                             ('==================') AS [Vlera], 
//                                             ('================') AS [TVSH]
//    
//                                       
//                                            UNION ALL
//    
//                                            SELECT  
//                                             ('Kodi TVSH-se') AS [Kodi], 
//                                             ('Shkalla TVSH-se') AS [Shkalla], 
//                                             ('Vlera tatueshme') AS [Vlera], 
//                                             ('Shuma TVSH-se') AS [TVSH]
//    
//                                       
//                                            UNION ALL
//    
//                                            SELECT  
//                                             ('---------------') AS [Kodi], 
//                                             ('----------------') AS [Shkalla], 
//                                             ('------------------') AS [Vlera], 
//                                             ('----------------') AS [TVSH]" 


//                                            @"
//                                            UNION ALL
//                                            SELECT  
//                                             ('S-VAT') AS [Kodi], 
//                                             ('" + Shkalla20 + @"') AS [Shkalla], 
//                                             ('" + totaliPaTVSH20 + @"') AS [Vlera], 
//                                             ('" + totaliTVSH20 + @"') AS [TVSH]
//    
//                                            UNION ALL
//                                            SELECT  
//                                             ('----------------') AS [Kodi], 
//                                             ('-----------------') AS [Shkalla], 
//                                             ('-------------------') AS [Vlera], 
//                                             ('-----------------') AS [TVSH]
//    
//                                            UNION ALL
//    
//                                            SELECT  
//                                             (' ') AS [Kodi], 
//                                             (' ') AS [Shkalla], 
//                                             (' ') AS [Vlera], 
//                                             (' ') AS [TVSH]
//    
//                                            UNION ALL
//    
//                                            SELECT  
//                                             ('Afati i pageses:') AS Kodi, 
//                                             (CONVERT(NCHAR(10), GETDATE() + 30, 104)) AS Shkalla, 
//                                             ('') AS Vlera, 
//                                             ('') AS TVSH"; 

                   
                    BxlPrint.PrintText("Shtese ne nivel fature", BxlPrint.BXL_ALIGNMENT_LEFT, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                    BxlPrint.LineFeed(1);
                    BxlPrint.PrintText("=====================================================================", BxlPrint.BXL_ALIGNMENT_LEFT, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                    BxlPrint.LineFeed(1);
                    BxlPrint.PrintText("Kodi TVSH-se    Shk.TVSH-se   Vl. tatueshme    TVSH          Vlera", BxlPrint.BXL_ALIGNMENT_LEFT, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                    BxlPrint.LineFeed(1);
                    BxlPrint.PrintText("---------------------------------------------------------------------", BxlPrint.BXL_ALIGNMENT_LEFT, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                    BxlPrint.LineFeed(1);
                    BxlPrint.PrintText("  S-VAT            20         " + totaliPaTVSH + "        " + totaliTVSH + "     " + totaliMeTVSH, BxlPrint.BXL_ALIGNMENT_LEFT, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                    BxlPrint.LineFeed(1);
                    BxlPrint.PrintText("---------------------------------------------------------------------", BxlPrint.BXL_ALIGNMENT_LEFT, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);

                    BxlPrint.LineFeed(1);
                    BxlPrint.PrintText(menyraPageses, BxlPrint.BXL_ALIGNMENT_LEFT, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                    BxlPrint.LineFeed(2);

                    if (TCRQRCodeTbl.Rows.Count > 0)
                    {
                        string errorMessage = TCRQRCodeTbl.Rows[0]["Message"].ToString();

                        if (errorMessage.Contains("Gabim: NIPT jo aktiv në Regjistrin e Tatimpaguesve (RTP)!"))
                        {
                            BxlPrint.PrintText("Gabim: NIPT jo aktiv në Regjistrin e Tatimpaguesve (RTP)!", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                            BxlPrint.LineFeed(5);
                        }
                        else
                        {

                            byte[] bytes = Encoding.UTF8.GetBytes(TCRQRCodeTbl.Rows[0]["TCRQRCodeLink"].ToString());
                            BxlPrint.PrintBarcode(bytes, BxlPrint.BXL_BCS_QRCODE_MODEL2, 0, 4, BxlPrint.BXL_ALIGNMENT_CENTER, 0);
                            //BxlPrint.PrintText("Te gjitha informacionet ne lidhje me kete fature mund te shihen ne \n kete Kod QR", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                            BxlPrint.LineFeed(2);


                            if (TCRQRCodeTbl.Rows[0]["PayType"].ToString() == "Bank")
                            {
                                DataTable dt_barcode = new DataTable();
                                //@BE QR Code
                                PnUtils.DbUtils.FillDataTable(dt_barcode, @"SELECT cl.Value as NIPT, c.[Value] as Emri, l.TCRNIVF as NIVF, 
                                                    SUM(CONVERT (Decimal(10,2),ROUND(la.Totali,2))) as Shuma,
                                                    CONVERT(NCHAR(10), GETDATE(), 104) + ' '+CONVERT(NCHAR(5), GETDATE(), 8) as Data        
                                                    FROM Liferimi l
                                                    INNER JOIN LiferimiArt la on la.IDLiferimi = l.IDLiferimi, CompanyInfo c, CompanyInfo cl 
                                                    WHERE  c.Item = 'Shitesi' and cl.Item = 'NIPT' 
                        							and l.IDPorosia = '" + _IDPorosia + @"'
                        							group by cl.Value, c.Value, l.TCRNIVF");


                                string barcodeString = dt_barcode.Rows[0]["NIPT"].ToString() + ";" + dt_barcode.Rows[0]["Emri"].ToString() + ";" + dt_barcode.Rows[0]["NIVF"].ToString() + ";" +
                                    dt_barcode.Rows[0]["Data"].ToString() + ";" + String.Format("{0:###0.00}", float.Parse((dt_barcode.Rows[0]["Shuma"].ToString()))) + "ALL;;;";

                                BxlPrint.PrintText("ISP        AL 8820 81120 400000 3044 9935302", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                                BxlPrint.LineFeed(1);
                                BxlPrint.PrintText("RZB        AL 2420 2111 7800 0000 0001 313616", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                                BxlPrint.LineFeed(1);
                                BxlPrint.PrintText("PCB        AL 4420 9111 0800 0010 5418 3900 01", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                                BxlPrint.LineFeed(1);
                                BxlPrint.PrintText("BKT        AL 0820 51111 79063 36CL PRCLALLF", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                                BxlPrint.LineFeed(2);

                                byte[] secondBarcodeBytes = Encoding.UTF8.GetBytes(barcodeString);
                                BxlPrint.PrintBarcode(secondBarcodeBytes, BxlPrint.BXL_BCS_QRCODE_MODEL2, 0, 4, BxlPrint.BXL_ALIGNMENT_CENTER, 0);
                            }
                            BxlPrint.PrintText("Te gjitha informacionet ne lidhje me kete fature mund te shihen ne \n kete Kod QR", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                            BxlPrint.LineFeed(5);
                        }
                    }
                    else
                    {
                        //byte[] QRbytes = Encoding.UTF8.GetBytes("DOKUMENT I PAFISKALIZUAR!");
                        //BxlPrint.PrintBarcode(QRbytes, BxlPrint.BXL_BCS_QRCODE_MODEL2, 0, 6, BxlPrint.BXL_ALIGNMENT_CENTER, 0);
                        BxlPrint.PrintText("DOKUMENT I PAFISKALIZUAR! \n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                        BxlPrint.LineFeed(4);
                    }
                    
                    if (PT == PnType.Print)
                    {
                        if (MessageBox.Show("Dëshironi të shtypni fletëpagesën ? \n sigurohuni që fatura është e shtypur", "Shtypja", MessageBoxButtons.YesNo, MessageBoxIcon.Hand,
                            MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            //PrintoFletePagesn();
                            //  Print_To_Fiscal(nrLiferimitLabel1.Text, lblTotali.Text);
                            Print();

                        }
                    }
                }
                else
                {
                    MessageBox.Show("Gabim gjatë gjenerimit te Fatures \n Printimin nuk munde të vazhdoj");
                    return;
                }
            }
            catch (FileNotFoundException fex)
            {
                if (frmVizitat.KthimMall)
                {
                    MessageBox.Show("Mungon Fajlli: \n FaturaKthim.pnrep");
                }
                else
                {
                    MessageBox.Show("Mungon Fajlli: \n FaturaParapaguese2.pnrep");
                }
                PnUtils.DbUtils.WriteExeptionErrorLog(fex);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                PnUtils.DbUtils.WriteExeptionErrorLog(ex);

            }
            finally
            {
                details.Dispose();
                header.Dispose();
                if (Rpt != null) Rpt.Dispose();
                if (msg != null) msg.Dispose();
                //frmMessage.isPreview = false;
                //frmMessage.isBlutooth = false;
                //frmMessage.isMultiplicatior = false;
                msg = null;
            }
            Cursor.Current = Cursors.Default;
        }

        private void menu_Close_Click(object sender, EventArgs e)
        {
            if (ISTheFirstTime)//ISTheFirstTime=true
            {
                try
                {
                    this.Close();
                }
                catch (SqlCeException sqc)
                {
                    MessageBox.Show(sqc.Message);
                }
                finally
                {
                    this.Close();
                }
            }
            else //ISTheFirstTime=false
            { //Liferimi është i krijuar më herët dhe tani hapet vetëm për rishikim
                this.Close();
            }
            Cursor.Current = Cursors.WaitCursor;
        }

    }


    public enum TipiMagazines
    {
        Fikse,
        Levizes
    }

    enum TipiFatures
    {
        Shitje,
        Kthim
    }

}