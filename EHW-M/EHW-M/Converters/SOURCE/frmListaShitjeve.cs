using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Data.SqlServerCe;
using PnReports;
using System.Reflection;
using System.IO;
using MobileSales;
using MobileSales.BL;

namespace MobileSales
{
    public partial class frmListaShitjeve : Form
    {
        private int currentDay;
        private DateTime dtLastDate;
        public static DateTime dtFisrtDate;
        private bool isSelected, Key;
        public static Guid IDVizita, IDLiferimi;
        public static DateTime DataLiferimit;
        public static string strIDPorosia, strPortName = "", ListShitjeve_IDKlienti, NrFatures;
        public static decimal TotaliPaguar, TotaliFature, _inkasimiMeHershem;
        private string EmriAgjentit, Lokacioni, KontaktEmri, KlientiEmri;
        public PnDevice devPort;

        int _colIndexTot = 4;
        int _colIndexInkasuar = 5;
        int _colIndexNrFatures = 3;

        #region Intialize Instace

        frmArtikujtShitur frmArtikujtShitur = null;
        frmListInkasimet frmListInkasimet = null;
        frmMessage msg = null;
        dlgMbylljaFatures MbylljaFatures = null;
        SmartDevicePrintEngine sd = null;

        #endregion

        public frmListaShitjeve()
        {
            InitializeComponent();
        }

        private void ListaShitjeve_Load(object sender, EventArgs e)
        {
            listaShitjeveTableAdapter.Connection.ConnectionString = frmHome.AppDBConnectionsStr;
            if (MyMobileDataSetUtil.DesignerUtil.IsRunTime())
            {
                this.myMobileDataset.EnforceConstraints = false;
                this.listaShitjeveTableAdapter.SearchLiferimet(this.myMobileDataset.ListaShitjeve, frmHome.AprovimFaturash);
            }
            CultureInfo myCI = new CultureInfo("fr-FR");//FS. cdo here qe hapet forma instancohet ky objekt, shkakton memoryLeak
            System.Globalization.Calendar myCal = myCI.Calendar;
            currentDay = (int)myCal.GetDayOfWeek(DateTime.Now);
            if (currentDay == 0)
            {
                currentDay = 7;
                cmbDitet.SelectedIndex = 7;

            }
            else
            {
                cmbDitet.SelectedIndex = currentDay;
            }
            if (cmbDitet.SelectedIndex != 0)
            {
                lblData.Text = dtFisrtDate.ToString("dd-MM-yyyy");
            }

            CalcAllSUM();
        }

        private void cmbDitet_SelectedIndexChanged(object sender, EventArgs e)
        {
            int numRows;
            isSelected = false;
            dtFisrtDate = DateTime.Now.Date.AddDays(-(currentDay));
            string str = dtFisrtDate.ToString("dd/MM/yyyy hh:mm:ss tt");
            dtFisrtDate.AddDays(cmbDitet.SelectedIndex);
            dtLastDate = dtFisrtDate.AddDays(6);
            string fltstr = "";

            if (cmbDitet.SelectedIndex == 0)
            {
                dtFisrtDate = dtFisrtDate.AddDays(1);
                dtLastDate = dtFisrtDate.AddDays(6);
                fltstr =
                    "DataLiferimit >='" + dtFisrtDate.ToShortDateString() + "'And DataLiferimit <='" + dtLastDate.ToShortDateString() + "'";
                this.listaShitjeveBindingSource.Filter = fltstr;
                numRows = listaShitjeveDataGrid.VisibleRowCount;
                lblData.Visible = false;
                lblDt.Visible = false;
                Line2.Visible = false;
            }
            else
            {
                dtFisrtDate = dtFisrtDate.AddDays(cmbDitet.SelectedIndex);
                fltstr = "DataLiferimit = '" + dtFisrtDate.ToShortDateString() + "'";
                listaShitjeveBindingSource.Filter = fltstr;
                lblData.Text = dtFisrtDate.ToString("dd-MM-yyyy");

                lblData.Visible = true;
                lblDt.Visible = true;
                Line2.Visible = true;
            }
            CalcAllSUM();


        }

        private void menuDalja_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuFatura_Click(object sender, EventArgs e)
        {
            if (isSelected)
            {
                if (listaShitjeveBindingSource.Count != 0)
                {
                    string IDV = lblIDVizita.Text;
                    string IDL = lblIDLiferimi.Text;
                    IDVizita = new Guid(); //??!! FS. instancim i njepasnjeshem i objekt variables se njejte
                    IDVizita = new Guid(IDV);
                    IDLiferimi = new Guid();//??!! FS. instancim i njepasnjeshem i objekt variables se njejte
                    IDLiferimi = new Guid(IDL);
                    DataLiferimit = dtpDataLiferimit.Value.Date;
                    Cursor.Current = Cursors.WaitCursor;
                    TotaliFature = decimal.Parse(this.listaShitjeveDataGrid[listaShitjeveDataGrid.CurrentRowIndex, _colIndexTot].ToString());
                    NrFatures = this.listaShitjeveDataGrid[listaShitjeveDataGrid.CurrentRowIndex, _colIndexNrFatures].ToString();
                    DataRow rw = ((DataRowView)(this.listaShitjeveBindingSource.Current)).Row;
                    strIDPorosia = rw["IDPorosia"].ToString();
                    if (frmArtikujtShitur == null) frmArtikujtShitur = new frmArtikujtShitur();
                    frmArtikujtShitur.pos = "ListaShitjeve";
                    frmArtikujtShitur.ShowDialog();

                }

            }
        }

        private void menuPrinto_Click(object sender, EventArgs e)
        {
            int _aprovuar = 0;
            if (frmHome.AprovimFaturash) _aprovuar = 1;


            if (listaShitjeveDataGrid.VisibleRowCount != 0)
            {
                DataTable details = new DataTable();
                DataTable header = new DataTable();

                SqlCeDataAdapter da = null;
                PnPrint Rpt = null;
                PnType PT = PnType.Print;
                PnDevice devPort = PnDevice.BluetoothPort;
                string commHeader = "";
                string commDetail = "";
                try
                {
                    commHeader = @"SELECT [Agjendet].[Emri]+' '+[Agjendet].[Mbiemri] AS Agjenti , 
										CONVERT(NCHAR(8),GETDATE(),3)+' ' +CONVERT(NCHAR(8),GETDATE(),8)AS DataPrintimit 
										FROM [Agjendet] WHERE [Depo]='" + frmHome.Depo + "'";

                    if (cmbDitet.SelectedIndex != 0)
                    {

                        commDetail = @"SELECT     kl.KontaktEmriMbiemri AS Klienti,
							                      kl.[Adresa] AS Adresa , 
							                      Convert(NCHAR(12),l.[NumriFisk]) AS NumriFisk,  
							                      Convert(NCHAR(12),l.[NrLiferimit]) AS NrLiferimit, 
                                                  --'0' AS Rab,
							                      Convert(Decimal(15,2) ,l.CmimiTotal)AS Totali,
                                                  case when l.PayType = 'KESH' then Convert(Decimal(15,2) ,l.ShumaPaguar) else 0 end AS Paguar,   
							                      CONVERT(NCHAR(8),l.KohaLiferimit,3)+' '+CONVERT(NCHAR(8),l.KohaLiferimit,8) AS Data
							            FROM      Liferimi l INNER JOIN
                                                  KlientDheLokacion kl ON kl.IDKlientDheLokacion = l.IDKlienti
                                                  WHERE l.[DataLiferimit]='" + dtFisrtDate.ToShortDateString() + "' AND l.Aprovuar=" + _aprovuar + "" +
                            //" ORDER BY l.[DataLiferimit]  "; @BE 298.09.2018 Ndryshuar me rreshtin poshte
                                                  " ORDER BY l.[NrLiferimit] ";
                    }
                    else
                    {
                        commDetail = @"SELECT     kl.KontaktEmriMbiemri AS Klienti,
							                      kl.[Adresa] AS Adresa , 
							                      Convert(NCHAR(12),l.[NumriFisk]) AS NumriFisk,
							                      Convert(NCHAR(12),l.[NrLiferimit]) AS NrLiferimit,   
                                                  --'0' AS Rab,
							                      Convert(Decimal(15,2) ,l.CmimiTotal)AS Totali,
                                                  case when l.PayType = 'KESH' then Convert(Decimal(15,2) ,l.ShumaPaguar) else 0 end AS Paguar,   
							                      CONVERT(NCHAR(8),l.KohaLiferimit,3)+' '+CONVERT(NCHAR(8),l.KohaLiferimit,8) AS Data
							            FROM      Liferimi l INNER JOIN
                                                  KlientDheLokacion kl ON kl.IDKlientDheLokacion = l.IDKlienti
                                                  WHERE l.[DataLiferimit] >= '" + dtFisrtDate.ToShortDateString() + "' AND l.[DataLiferimit] <='" + dtLastDate.ToShortDateString() + "' AND l.Aprovuar=" + _aprovuar + "" +
                            //" ORDER BY l.[DataLiferimit]  "; @BE 298.09.2018 Ndryshuar me rreshtin poshte
                                                  " ORDER BY l.[NrLiferimit]";

                    }
                    //PnUtils.DbUtils.WriteQueryLog(commHeader);
                    // PnUtils.DbUtils.WriteQueryLog(commDetail);
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
                /*Return
                 * IDArticle,ArtName,Sasia,BUM,UPrice,Rab,Tot
                 */
                try
                {
                    da = new SqlCeDataAdapter(commDetail, frmHome.AppDBConnectionsStr);
                    da.Fill(details);
                    da.SelectCommand.CommandText = commHeader;
                    da.Fill(header);
                    string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);

                    string nrFatureParam = this.listaShitjeveDataGrid[listaShitjeveDataGrid.CurrentRowIndex, _colIndexNrFatures].ToString();
                    DataTable TCRQRCodeTbl = new DataTable();
                    //@BE QR Code
                    PnUtils.DbUtils.FillDataTable(TCRQRCodeTbl, @"
                                                            Select NrLiferimit, CmimiTotal, TCRQRCodeLink 
															from Liferimi
															where NrLiferimit = '" + nrFatureParam + @"'
															and TCRSyncStatus >= 1
                                                        ");
                    if (TCRQRCodeTbl.Rows.Count > 0)
                    {
                        if (msg == null) msg = new frmMessage(true, new string[] { TCRQRCodeTbl.Rows[0]["CmimiTotal"].ToString(),
                                                        TCRQRCodeTbl.Rows[0]["NrLiferimit"].ToString(), 
                                                        TCRQRCodeTbl.Rows[0]["TCRQRCodeLink"].ToString() });
                    }
                    else
                    {
                        if (msg == null) msg = new frmMessage(true, new string[3]);
                    }
                    DialogResult a = msg.ShowDialog();
                    if (a == DialogResult.Abort)
                    {
                        return;
                    }
                    string strPortName = "";

                    if (frmMessage.isBlutooth)
                    {
                        PT = PnType.Print;
                        devPort = PnDevice.BluetoothPort;
                        strPortName = frmHome.PnPrintPort + ":";
                    }
                    //if (frmMessage.isMultiplicatior)
                    //{
                    //    PT = PnType.Print;
                    //    devPort = PnDevice.MultiplicatorPort;
                    //    strPortName = frmHome.PnPrintPort + ":";

                    //}
                    if (frmMessage.isPreview)
                    {
                        PT = PnType.Preview;
                        devPort = PnDevice.PreviewOnly;
                    }

                    Rpt = new PnPrint(devPort, strPortName, CurrentDir + "\\RaportiShitjeve.pnrep", details, header);

                    //@BE 21.10.2018 Shtuar per te sortuar ne fature
                    //details.DefaultView.Sort = "NrLiferimit ASC";
                    Rpt.PrintTable = details.DefaultView.ToTable();
                    //@BE perfundimi i shtimit te 21.10.2018

                    Rpt.PrintPnReport(PT, true, " tre ", false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    details.Dispose();
                    header.Dispose();
                    if (Rpt != null) Rpt.Dispose();
                    if (da != null) da.Dispose();
                    if (msg != null) msg.Dispose();
                    msg = null;
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void manuRiInkasimi_Click(object sender, EventArgs e)
        {
            if (isSelected)
            {
                TotaliFature = decimal.Parse(this.listaShitjeveDataGrid[listaShitjeveDataGrid.CurrentRowIndex, _colIndexTot].ToString());
                NrFatures = this.listaShitjeveDataGrid[listaShitjeveDataGrid.CurrentRowIndex, _colIndexNrFatures].ToString();
                _inkasimiMeHershem = decimal.Parse(this.listaShitjeveDataGrid[listaShitjeveDataGrid.CurrentRowIndex, _colIndexInkasuar].ToString());
                if (TotaliFature == _inkasimiMeHershem)
                {
                    MessageBox.Show("Inkasimi eshte mbyllur");
                    return;
                }


                if (MbylljaFatures == null) MbylljaFatures = new dlgMbylljaFatures();
                if (TotaliFature < 0)
                {
                    MbylljaFatures.cmbMenyraPageses.Enabled = false;
                }
                if (MbylljaFatures.ShowDialog(ListShitjeve_IDKlienti, TotaliFature, _inkasimiMeHershem) == DialogResult.OK)
                {
                    TotaliPaguar = decimal.Parse(MbylljaFatures.txtShumaPaguar.Text);
                    bool _insertEvidenca =
                       InsertEvidencaPagesave
                           (
                            NrFatures,
                            DateTime.Now,
                            MbylljaFatures.dtDataPerPagese.Value,
                            TotaliFature,
                            TotaliPaguar,
                            dlgMbylljaFatures.KMON, MbylljaFatures.cmbMenyraPageses.Text
                            );
                    if (_insertEvidenca)
                    {
                        if (MessageBox.Show("Pagesa përfunoi me sukses \n Dëshironi të shtypni fletëpagesën ? \n", "Shtypja", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                              MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            PrintoFletePagesn();
                            //  MessageBox.Show("Printimi perfundoi me sukses");

                        }

                        Cursor.Current = Cursors.Default;

                    }
                    else
                    {
                        MessageBox.Show("Pages per fature deshtoi");
                        return;
                    }

                }
                isSelected = false;
            }
            else
            {
                MessageBox.Show("Selektoni nje fature");
            }
        }

        #region Methods && Functions

        private bool InsertEvidencaPagesave(string NrFatures, DateTime DataPageses, DateTime DataPerPages, decimal ShumaFatures, decimal ShumaPaguar, string KMON, string PayType)
        {
            bool result;
            DateTime n = DateTime.Now;
            string hr = n.Hour.ToString();
            string min = n.Minute.ToString();
            string sec = n.Second.ToString();

            //ie. if Hour is 4 then 04; if minute is 3 then 03             
            if (hr.Length == 1)
                hr = "0" + hr;
            if (min.Length == 1)
                min = "0" + min;
            if (sec.Length == 1)
                sec = "0" + sec;

            string PagesatID = frmHome.DevID + "-|-" + n.Year.ToString() + n.Month.ToString() + n.Day.ToString() +
                hr + min + sec;


            string strInsertCommand =
                "INSERT INTO EvidencaPagesave(NrPageses,IDKlienti,IDAgjenti,NrFatures,DataPageses,DataPerPagese,ShumaTotale,ShumaPaguar,Borxhi,DeviceID,SyncStatus,KMON,Export_Status,PayType ) " +
                "VALUES ('" + PagesatID + "','" + ListShitjeve_IDKlienti + "','" + frmHome.IDAgjenti + "','" + NrFatures + "','" + DataPageses.ToString("yyyy-MM-dd HH:mm:ss") + "','" + DataPerPages.ToString("yyyy-MM-dd") + "',"
                + ShumaFatures.ToString() + "," + ShumaPaguar.ToString() + "," + (ShumaFatures - ShumaPaguar).ToString() + ",'" + frmHome.DevID + "',0,'" + KMON + "',0,'" + PayType + "')";

            SqlCeConnection con = null;
            SqlCeCommand kom = null;

            try
            {
                con = new SqlCeConnection(frmHome.AppDBConnectionsStr);
                kom = new SqlCeCommand(strInsertCommand, con);
                con.Open();
                kom.ExecuteNonQuery();
                con.Close();
                result = true;
            }
            catch (SqlCeException ex)
            {
                MessageBox.Show(ex.Message);
                result = false;
            }
            finally
            {
                con.Dispose();
                kom.Dispose();
            }
            return result;

        }

        /// <Shuma ne kolone>
        /// Llogarit shumen e cell-save ne grid per nje kolone
        /// Rrezulatin e vendose ne nje label
        /// </summary>
        /// <param name="numColumns">Numri i kolnës në të cilën duhet të llogaritet shum </param>
        /// <param name="labname">labela në të cilën do të vendoset rrezultati i shumës</param>
        /// <param name="Orders">Lloji i sherbimit, te cilit do ti llogaritet totali</param>
        private void CalcTot(int numColumns, Label labname, string Orders)
        {
            double SUM = 0;
            int numRows = int.Parse(listaShitjeveBindingSource.Count.ToString());
            if (numRows > 0)
            {
                switch (Orders)
                {
                    case "Fatura":
                        {

                            for (int j = 0; j < numRows; j++)
                            {
                                double _tmpSum = Convert.ToDouble(this.listaShitjeveDataGrid[j, numColumns].ToString());
                                if (_tmpSum > 0)//Mbledhim vetem faturat e shitjes
                                {
                                    SUM = SUM + Convert.ToDouble(this.listaShitjeveDataGrid[j, numColumns].ToString());
                                }


                            }
                            labname.Text = String.Format("{0:#,0.00}", SUM);
                            break;
                        }
                    case "Pagesa/Inkasimet":
                        {
                            for (int j = 0; j < numRows; j++)
                            {
                                double _tmpSum = Convert.ToDouble(this.listaShitjeveDataGrid[j, numColumns].ToString());
                                if (_tmpSum < 0)//Mbledhim vetem faturat e shitjes
                                {
                                    SUM = SUM + Convert.ToDouble(this.listaShitjeveDataGrid[j, numColumns].ToString());
                                }


                            }
                            labname.Text = String.Format("{0:#,0.00}", SUM);
                            break;
                        }
                }


            }
            else
            {
                labname.Text = "0.00 ";
            }
        }

        private void CalcAllSUM()
        {
            CalcTot(4, lblTotali, "Fatura");//4-Numri i kolones Totali
            CalcTot(5, lblTotInk, "Fatura");//5-Numri i kolones Inkas
            CalcTot(4, lblTotKthimet, "Pagesa/Inkasimet");
        }

        private void PrintDialog()
        {
            DataTable details = new DataTable();
            DataTable header = new DataTable();

            SqlCeDataAdapter da = null;
            PnPrint Rpt = null;
            try
            {

                string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);

                //@BE QR Code
                if (msg == null) msg = new frmMessage(false, new string[] { null });
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

                System.IO.Ports.SerialPort sp = new System.IO.Ports.SerialPort(strPortName);
                try
                {
                    sp.Open();
                    // MessageBox.Show("Porti eshte hapur me sukses");
                }
                catch (Exception ex)
                {
                    if (ex.Message == @"The port '" + strPortName + "' does not exist.")
                    {
                        MessageBox.Show("Porti:" + strPortName + " nuk mund të hapet");
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                finally
                {
                    sp.Dispose();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == @"Could not find file '\Flash Disk\mobilesales\FaturaParapaguese2.pnrep'." || ex.Message == @"Could not find file '\Flash Disk\mobilesales\RaportiKthimit.pnrep'.")
                {
                    MessageBox.Show("Mungon Fajlli i raportit");
                    return;
                }
                else
                {
                    MessageBox.Show(ex.Message);
                    return;
                }


            }
            finally
            {
                details.Dispose();
                header.Dispose();
                if (Rpt != null) Rpt.Dispose();
                if (da != null) da.Dispose();
                if (msg != null) msg.Dispose();
                msg = null;
                Cursor.Current = Cursors.Default;
            }
        }

        private void PrintoFletePagesn()
        {
            SqlCeDataAdapter da2 = null;
            string NrPageses = "";

            da2 = new SqlCeDataAdapter("", frmHome.AppDBConnectionsStr);
            DataTable dtTemp = new DataTable();
            da2.SelectCommand.CommandText = "select * from Agjendet where IDAgjenti='" + frmHome.IDAgjenti + "'";
            da2.Fill(dtTemp);
            if (dtTemp.Rows.Count > 0)
                EmriAgjentit = dtTemp.Rows[0]["Emri"].ToString() + " " + dtTemp.Rows[0]["Mbiemri"].ToString();

            da2.SelectCommand.CommandText = "Select * from KlientDheLokacion where IDKlientDheLokacion='" + ListShitjeve_IDKlienti + "'";
            dtTemp.Clear();
            da2.Fill(dtTemp);
            if (dtTemp.Rows.Count > 0)
                Lokacioni = dtTemp.Rows[0]["EmriLokacionit"].ToString();
            KontaktEmri = dtTemp.Rows[0]["KontaktEmriMbiemri"].ToString();

            dtTemp.Clear();
            da2.SelectCommand.CommandText = "SELECT TOP(1) CASE  \n"
                                           + "                   WHEN ep.NrFatures IS NOT NULL THEN ep.NrFatures \n"
                                           + "                   ELSE '' \n"
                                           + "              END AS NrFatures, \n"
                                           + "       ep.NrPageses \n"
                                           + "FROM   EvidencaPagesave ep \n"
                                           + "ORDER BY \n"
                                           + "       ep.DataPageses DESC";

            da2.Fill(dtTemp);
            if (dtTemp.Rows.Count > 0)
            {
                NrPageses = dtTemp.Rows[0]["NrPageses"].ToString();

            }

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
                (NrFatures, NrPageses, DateTime.Now, companyName, Nipt, EmriAgjentit, frmHome.IDAgjenti,
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

            DataTable details = new DataTable();
            DataTable header = new DataTable();

            try
            {
                string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);

                //@BE DUHET NDRYSHOHET
                BxlPrint.PrinterOpen("COM1:115200", 1000);
                BxlPrint.PrintBitmap(CurrentDir + "\\Image\\EHW-logo.png", 100, BxlPrint.BXL_ALIGNMENT_CENTER, 30);
                BxlPrint.LineFeed(1);

                if (msg == null) msg = new frmMessage(false);
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

                if (!frmMessage.isPreview)
                {
                    if (!PnFunctions.AvailablePort(strPortName))
                    {
                        MessageBox.Show("Porti: " + strPortName + " nuk munde te hapet");
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                Cursor.Current = Cursors.Default;
                sd = new SmartDevicePrintEngine(devPort, strPortName);
                sd.CreatePage(PrintWidth, PrintHeight);
                int c = 0;

                x = 20;
                c += 1;

                buffer = "F L E T E  -  P A G E S E ";

                //prevX = (PrintWidth / 2) - (buffer.Length / 2);
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
                buffer = "     Shuma e fatures: " + Math.Round(double.Parse(strShuma), 2).ToString() + " " + KMON;
                sd.WriteToLogicalPage(x, y, buffer);

                y += 1;
                buffer = "     Pagesa : " + Math.Round(double.Parse(strInkasuar), 2).ToString() + " " + KMON + " " + dlgMbylljaFatures._PayType;
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


                y = y + 3;
                WriteSeparator(ref x, ref y, ref sd, "="); y = y + 1;

                buffer = "     Fletepagesa eshte e hartuar ne tre kopje";
                sd.WriteToLogicalPage(x, y, buffer);

                y += 1;
                buffer = "     Printuar nga Sistemi MobSell Pronet - Kosove +381 38 557799";
                sd.WriteToLogicalPage(x, y, buffer);

                y += 5;

                sd.Print();
                MessageBox.Show("Printimi perfundoi me sukses!");

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

        #region DataGrid

        private void listaShitjeveDataGrid_MouseUp(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.DataGrid.HitTestInfo myHitTest = this.listaShitjeveDataGrid.HitTest(e.X, e.Y);
            if (myHitTest.Type == DataGrid.HitTestType.Cell)
            {
                this.listaShitjeveDataGrid.Select(myHitTest.Row);
                isSelected = true;
                double TotaliFatures = Convert.ToDouble(lbTotaliBind.Text);
                ListShitjeve_IDKlienti = iDKlientiLabel1.Text;
                if (TotaliFatures == 0)
                {
                    menyShfaq.Enabled = false;
                }
                else
                {
                    menyShfaq.Enabled = true;
                }
            }

        }

        private void listaShitjeveDataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            Key = (e.KeyData == Keys.Enter || e.KeyData == Keys.Up || e.KeyData == Keys.Down || e.KeyData == Keys.Left || e.KeyData == Keys.Right);
            if (isSelected == true)
            {
                if (Key)
                {
                    isSelected = true;
                    double TotaliFatures = Convert.ToDouble(lbTotaliBind.Text);
                    ListShitjeve_IDKlienti = iDKlientiLabel1.Text;
                    if (TotaliFatures == 0)
                    {
                        menyShfaq.Enabled = false;
                    }
                    else
                    {
                        menyShfaq.Enabled = true;
                    }
                    this.listaShitjeveDataGrid.Select(this.listaShitjeveDataGrid.CurrentRowIndex);

                }

            }
        }

        #endregion

        private void menuInkasimet_Click(object sender, EventArgs e)
        {
            if (isSelected)
            {
                NrFatures = this.listaShitjeveDataGrid[listaShitjeveDataGrid.CurrentRowIndex, _colIndexNrFatures].ToString();
                if (frmListInkasimet == null) frmListInkasimet = new frmListInkasimet();
                frmListInkasimet.ShowDialog();

            }
        }

        private void menyShfaq_Click(object sender, EventArgs e)
        {

        }



    }
}