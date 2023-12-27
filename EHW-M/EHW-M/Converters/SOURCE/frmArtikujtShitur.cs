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
using PnUtils;
using MobileSales.BL;
using System.Globalization;
using PnSync;

namespace MobileSales
{
    public partial class frmArtikujtShitur : Form
    {
        private bool AnyKey, IsSelected;
        public string pos;
        
        frmMessage msg = null;
        frmListaShitjeve frmListaShitjeve = null;

        public frmArtikujtShitur(string position)
        {
            pos = position;
            InitializeComponent();
        }

        public frmArtikujtShitur()
        {
            InitializeComponent();
        }

        private void menyDalja_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ArtikujtShitur_Load(object sender, EventArgs e)
        {

            if (frmListaShitjeve.TotaliFature < 0)
            {
                btnKthimi.Enabled = false;
                btnKthimi.Text = "";
            }
            else
            {
                btnKthimi.Enabled = true;
                btnKthimi.Text = "Kthimi";
            }
            artikujtShiturTableAdapter.Connection.ConnectionString = frmHome.AppDBConnectionsStr;
            switch (pos)
            {
                case ("Klientet")://Nëse forma "Artikujt e Shitur" hapet nga butoni "Klientet"
                    {
                        try
                        {
                            this.artikujtShiturTableAdapter.Fill(this.myMobileDataset.ArtikujtShitur, frmKlientet.IDVizita, frmKlientet.DataLiferimit, frmKlientet.IDLiferimi);
                        }
                        catch (System.Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show(ex.Message);
                        }
                        break;
                    }
                case ("ListaShitjeve"): //Nëse forma "Artikujt e Shitur" hapet nga butoni "Lista e Shitjeve"
                    {
                        try
                        {
                            this.myMobileDataset.EnforceConstraints = false;
                            this.artikujtShiturTableAdapter.Fill(this.myMobileDataset.ArtikujtShitur, frmListaShitjeve.IDVizita, frmListaShitjeve.DataLiferimit, frmListaShitjeve.IDLiferimi);
                        }
                        catch (System.Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show(ex.Message);
                        }

                    }
                    break;
            }

            /*** Kontrollojm nëse kemi artikuj për porosin e zgjedhur ***/
            if (artikujtShiturDataGrid.VisibleRowCount >= 1)
            {
                artikujtShiturDataGrid.Select(0);
            }

            lblDataLiferimit.Text = dtpDateLiferimit.Value.ToString("dd-MM-yyyy");
            cmimiTotalLabel1.Text = "Totali: " + String.Format("{0:#,0.00}", decimal.Parse(myMobileDataset.ArtikujtShitur[0]["CmimiTotal"].ToString()));
            Cursor.Current = Cursors.Default;

        }

        private void menuFatura_Click(object sender, EventArgs e)
        {
            if (!excistTVSH())
            {
                MessageBox.Show("Vlera e TVSH-se nuk është e përcaktuar \n Tabela:CompanyInfo");
                return;
            }
            SqlCeDataAdapter da = null;
            PnPrint Rpt = null;
            string strPortName = "";
            string _IDPorosia = frmListaShitjeve.strIDPorosia;
            PnDevice devPort = PnDevice.BluetoothPort;
            PnType PT = PnType.Print;
            DataTable details = new DataTable();
            DataTable header = new DataTable();
            DataTable footer = new DataTable();
            string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);

            try
            {
                #region PrintParam

                DataTable TCRQRCodeTbl = new DataTable();
                //@BE QR Code
                PnUtils.DbUtils.FillDataTable(TCRQRCodeTbl, @"
                                                            Select NrLiferimit, CmimiTotal, TCRQRCodeLink, PayType, Message 
															from Liferimi
															where IDPorosia = '" + _IDPorosia + @"'
															and TCRNSLF <> ''
                                                        ");
                if (TCRQRCodeTbl.Rows.Count > 0)
                {
                    if (msg == null) msg = new frmMessage(true, new string[] { TCRQRCodeTbl.Rows[0]["CmimiTotal"].ToString(),
                                                                               TCRQRCodeTbl.Rows[0]["NrLiferimit"].ToString(), 
                                                                               TCRQRCodeTbl.Rows[0]["TCRQRCodeLink"].ToString(), 
                                                                               TCRQRCodeTbl.Rows[0]["Message"].ToString() });
                }
                else
                {
                    if (msg == null) msg = new frmMessage(true, new string[4]);
                }
                DialogResult a = msg.ShowDialog();
                if (a == DialogResult.Abort)
                {
                    return;//cancel printing
                }
                Cursor.Current = Cursors.WaitCursor;

                if (frmMessage.isBlutooth == true)
                {
                    devPort = PnDevice.BluetoothPort;
                    PT = PnType.Print;
                    strPortName = frmHome.PnPrintPort + ":";

                }
                if (frmMessage.isPreview == true)
                {
                    PT = PnType.Preview;
                    devPort = PnDevice.PreviewOnly;

                }
                //if (frmMessage.isMultiplicatior == true)
                //{
                //    PT = PnType.Print;
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

                #endregion

                DataTable[] Header_Details = PnFunctions.GenerateBill(frmHome._magType, _IDPorosia);
                details = Header_Details[0];
                header = Header_Details[1];
                footer = Header_Details[2];

                /*@BE 17.09.2018 ADDED for sorting by the first column*/
                //DataView dvs = header.DefaultView;
                //dvs.Sort = "Fatura ASC";
                //header = dvs.ToTable();
                /*@BE 17.09.2018 ADDED for sorting by the first column*/

                if (details.Rows.Count > 0 && header.Rows.Count > 0)
                {
                    if (frmListaShitjeve.TotaliFature < 0)
                    {
                        Rpt = new PnPrint(devPort, strPortName, CurrentDir + "\\FaturaKthim.pnrep", details, header, footer);
                    }
                    else
                    {
                        Rpt = new PnPrint(devPort, strPortName, CurrentDir + "\\FaturaParapaguese2.pnrep", details, header, footer);
                    }

                    Rpt.PrintPnReport(PT, true, " tre ", false);

                    //@BE DUHET NDRYSHOHET
                    BxlPrint.PrinterOpen("COM1:115200", 1000);


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
                                    dt_barcode.Rows[0]["Data"].ToString() + ";" + String.Format("{0:###0.00}", decimal.Parse((dt_barcode.Rows[0]["Shuma"].ToString()))) + "ALL;;;";

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
                            BxlPrint.LineFeed(4);
                        }
                    }
                    else
                    {
                        //byte[] QRbytes = Encoding.UTF8.GetBytes("DOKUMENT I PAFISKALIZUAR!");
                        //BxlPrint.PrintBarcode(QRbytes, BxlPrint.BXL_BCS_QRCODE_MODEL2, 0, 6, BxlPrint.BXL_ALIGNMENT_CENTER, 0);
                        BxlPrint.PrintText("DOKUMENT I PAFISKALIZUAR! \n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                        BxlPrint.LineFeed(4);
                    }
                }
                else
                {
                    MessageBox.Show("Gabim gjate krijimit te faturs");
                    return;
                }

            }
            catch (FileNotFoundException fex)
            {
                MessageBox.Show("Mungon Fajlli: \n FaturaParapaguese.pnrep");
                PnUtils.DbUtils.WriteExeptionErrorLog(fex);

            }
            catch (System.IO.IOException ioex)
            {
                MessageBox.Show("Porti:" + strPortName + " nuk mund të hapet");
                PnUtils.DbUtils.WriteExeptionErrorLog(ioex);

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
                if (da != null) da.Dispose();
                if (msg != null) msg.Dispose();
                msg = null;
                Cursor.Current = Cursors.Default;
            }

        }

        #region DataGrid && BindingSource

        private void artikujtShiturDataGrid_MouseUp(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.DataGrid.HitTestInfo myHitTest = this.artikujtShiturDataGrid.HitTest(e.X, e.Y);
            if (myHitTest.Type == DataGrid.HitTestType.Cell)
            {
                this.artikujtShiturDataGrid.Select(myHitTest.Row);
                IsSelected = true;
            }
        }

        private void artikujtShiturDataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            AnyKey =
           (e.KeyData == Keys.Enter || e.KeyData == Keys.Up || e.KeyData == Keys.Down || e.KeyData == Keys.Left || e.KeyData == Keys.Right);
            if (IsSelected == true)
            {
                if (AnyKey)
                {
                    this.artikujtShiturDataGrid.Select(this.artikujtShiturDataGrid.CurrentRowIndex);

                }
            }
        }

        #endregion

        private bool excistTVSH()
        {
            bool _result = false;
            string _selectQuery = "";
            try
            {
                _selectQuery = @"SELECT CASE 
                                             WHEN ci.[Value] IS NOT NULL THEN ci.[Value] 
                                          ELSE 0 
                                       END AS VA 
                                       FROM   CompanyInfo ci 
                                       WHERE  ci.Item = 'TVSH'";

                frmVizitat._TVHS = int.Parse(PnUtils.DbUtils.ExecSqlScalar(_selectQuery));
                if (frmVizitat._TVHS == 0)
                {
                    _result = false;
                }
                else
                {
                    _result = true;
                }
            }
            catch (Exception ex)
            {
                DbUtils.WriteExeptionErrorLog(ex);

            }
            return _result;
        }

        private void btnKthimi_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("A jeni të sigurt për kthim të faturës? \n", "Kthimi", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DataTable _checkLiferimi = new DataTable();

                PnUtils.DbUtils.FillDataTable(_checkLiferimi, "SELECT top(1) NumriFisk FROM Liferimi l WHERE l.NrLiferimit = '" + frmListaShitjeve.NrFatures + "'");

                int _checkNumriFisk = -1;

                if (_checkLiferimi.Rows.Count > 0)
                {
                    _checkNumriFisk = int.Parse(_checkLiferimi.Rows[0]["NumriFisk"].ToString());
                }

                DataTable _checkExistLiferimi = new DataTable();
                //@BE QR Code
                PnUtils.DbUtils.FillDataTable(_checkExistLiferimi, @"SELECT top(1) NumriFisk FROM Liferimi l WHERE l.IDKthimi = '" + _checkNumriFisk + "'");
                if (!(_checkExistLiferimi.Rows.Count > 0))
                {


                    Cursor.Current = Cursors.WaitCursor;
                    string _selectQuery = @"SELECT top(1) v.NrRendor
                                       FROM Vizitat v
                                      order by NrRendor desc";

                    int NrRendor = int.Parse(PnUtils.DbUtils.ExecSqlScalar(_selectQuery).ToString());

                    int newNrRendor = NrRendor + 1;

                    this.vizitatTableAdapter1.KrijoVizite(frmHome.IDAgjenti, newNrRendor, "6", frmHome.DevID, 0, DateTime.Now.Date, DateTime.Now.Date, frmListaShitjeve.ListShitjeve_IDKlienti, frmHome.LongitudeCur, frmHome.LatitudeCur);


                    Guid IDVizita = new Guid(
                            DbUtils.ExecSqlScalar("Select IDVizita from Vizitat where NrRendor = '" + newNrRendor + @"'"));

                    Guid IDPorosia = Guid.NewGuid();

                    Guid old_IDPorosia = new Guid(
                            PnUtils.DbUtils.ExecSqlScalar("Select IDPorosia from Porosite where IDVizita = '" + frmListaShitjeve.IDVizita + "'"));


                    SqlCeConnection con = null;
                    SqlCeCommand cmd = null;
                    try
                    {
                        con = new SqlCeConnection(frmHome.AppDBConnectionsStr + " ; Default Lock Timeout = 50000");
                        cmd = new SqlCeCommand("UPDATE TEST", con);

                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Transaction = con.BeginTransaction();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = @"INSERT INTO Porosite
                                        (
            	                            IDVizita,
            	                            DataPerLiferim,
            	                            IDPorosia,
            	                            DataPorosise,
            	                            OraPorosise,
            	                            StatusiPorosise,
            	                            NrPorosise,
            	                            DeviceID,
            	                            SyncStatus,
            	                            NrDetalet
                                        )
                                        VALUES
                                        (
            	                            '" + IDVizita + @"',
            	                            '" + DateTime.Now.Date + @"',
            	                            '" + IDPorosia + @"',
            	                            '" + DateTime.Now.Date + @"',
            	                            GETDATE(),
            	                            1,
            	                            '',
            	                            '" + frmHome.DevID + @"',
            	                            0,
            	                            0
                                        )";
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        cmd.CommandText = @"insert into PorosiaArt (
                                                    [IDArtikulli], 
                                                    [SasiaPorositur], 
                                                    [CmimiAktual], 
                                                    [Rabatet], 
                                                    [IDPorosia], 
                                                    [SasiLiferuar], 
                                                    [Emri], 
                                                    [Gratis], 
                                                    [SasiaPako], 
                                                    [DeviceID], 
                                                    [SyncStatus], 
                                                    [IDArsyeja], 
                                                    [CmimiPaTVSH], 
                                                    [BUM], 
                                                    [Seri]
                                                    )
                                             select 
                                                    [IDArtikulli], 
                                                    -1 * [SasiaPorositur], 
                                                    [CmimiAktual], 
                                                    [Rabatet], 
                                                    '" + IDPorosia + @"', 
                                                    -1 * [SasiLiferuar], 
                                                    [Emri], 
                                                    [Gratis], 
                                                    [SasiaPako], 
                                                    [DeviceID], 
                                                    [SyncStatus], 
                                                    [IDArsyeja], 
                                                    [CmimiPaTVSH], 
                                                    [BUM], 
                                                    [Seri] 
                                            from PorosiaArt
                                            where IDPorosia = '" + old_IDPorosia + "'";
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        cmd.Transaction.Commit();
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Transaction = con.BeginTransaction();

                        string _llojDok = "KD";


                        DataTable dtArtikujt = new DataTable();

                        PnUtils.DbUtils.FillDataTable(dtArtikujt, @"select 
                                                    [IDArtikulli], 
                                                    [SasiaPorositur], 
                                                    [CmimiAktual], 
                                                    [Rabatet], 
                                                    [IDPorosia], 
                                                    [SasiLiferuar], 
                                                    [Emri], 
                                                    [Gratis], 
                                                    [SasiaPako], 
                                                    [DeviceID], 
                                                    [SyncStatus], 
                                                    [IDArsyeja], 
                                                    [CmimiPaTVSH], 
                                                    [BUM], 
                                                    [Seri] 
                                            from PorosiaArt
                                            where IDPorosia = '" + IDPorosia + "'");
                        int NumRows = dtArtikujt.Rows.Count;
                        if (NumRows > 0) // Nese kemi artikuj per klientin
                        {
                            for (int i = 0; i < NumRows; i++)
                            {
                                decimal _SasiaKthyer = Math.Round(decimal.Parse(dtArtikujt.Rows[i]["SasiaPorositur"].ToString()), 3);

                                string _IDArtikulli = dtArtikujt.Rows[i]["IDArtikulli"].ToString();

                                string _Seri = dtArtikujt.Rows[i]["Seri"].ToString();


                                cmd.CommandText = @"UPDATE Stoqet
                                                                       SET    Sasia = Round(Sasia - @Sasia,3)
                                                                       WHERE  (Shifra = @IDArtikuli)
                                                                       AND (Depo = @Depo)
                                                                       AND (Seri = @Seri)";
                                cmd.Parameters.AddWithValue("@Sasia", _SasiaKthyer);
                                cmd.Parameters.AddWithValue("@IDArtikuli", _IDArtikulli);
                                cmd.Parameters.AddWithValue("@Depo", frmHome.Depo);
                                cmd.Parameters.AddWithValue("@Seri", _Seri);
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();

                                cmd.CommandText = @"UPDATE Malli_Mbetur
                                                                SET    SasiaKthyer = Round(SasiaKthyer + @Sasia_Kthyer,3), SyncStatus=0
                                                                WHERE  (IDArtikulli = @IDArtikulli)
                                                                       AND (Seri = @Seri)";
                                cmd.Parameters.AddWithValue("@Sasia_Kthyer", _SasiaKthyer);
                                cmd.Parameters.AddWithValue("@IDArtikulli", _IDArtikulli);
                                cmd.Parameters.AddWithValue("@Seri", _Seri);
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();

                                cmd.CommandText = @"Update Malli_Mbetur 
                                                               Set SasiaMbetur=Round(SasiaPranuar-(SasiaShitur+SasiaKthyer-LevizjeStoku),3), SyncStatus=0
                                                                   WHERE  (IDArtikulli = @IDArtikulli)
                                                                       AND (Seri = @Seri)";
                                cmd.Parameters.AddWithValue("@IDArtikulli", _IDArtikulli);
                                cmd.Parameters.AddWithValue("@Seri", _Seri);
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();

                            }
                        }


                        //dlgMbylljaFatures MbylljaFatures = null;
                        //Cursor.Current = Cursors.Default;
                        //if (MbylljaFatures == null) MbylljaFatures = new dlgMbylljaFatures();
                        string NrFatures = frmVizitat.getNurFatures(frmHome.Depo);
                        DataTable _getoldLiferimi = new DataTable();

                        PnUtils.DbUtils.FillDataTable(_getoldLiferimi, @"Select IDLiferimi, NumriFisk, CmimiTotal, TotaliPaTVSH, ShumaPaguar, PayType 
															from Liferimi
															where IDPorosia = '" + old_IDPorosia + @"'");
                        int oldNrFisk = 0;
                        string PayType = "";
                        decimal TotaliFautresMeTVSH = 0;
                        decimal TotaliFaturesPaTVSH = 0;
                        decimal ShumaPaguar = 0;
                        if (_getoldLiferimi.Rows.Count > 0)
                        {
                            oldNrFisk = int.Parse(_getoldLiferimi.Rows[0]["NumriFisk"].ToString());
                            PayType = _getoldLiferimi.Rows[0]["PayType"].ToString();
                            TotaliFautresMeTVSH = -Math.Round(decimal.Parse(_getoldLiferimi.Rows[0]["CmimiTotal"].ToString()), 3);
                            TotaliFaturesPaTVSH = -Math.Round(decimal.Parse(_getoldLiferimi.Rows[0]["TotaliPaTVSH"].ToString()), 3);
                            ShumaPaguar = -Math.Round(decimal.Parse(_getoldLiferimi.Rows[0]["ShumaPaguar"].ToString()), 3);
                        }

                        //if (MbylljaFatures.ShowDialog(frmListaShitjeve.ListShitjeve_IDKlienti, Math.Round(TotaliFature, 2), 0) == DialogResult.OK)
                        //{
                        //    TotaliPaguar = decimal.Parse(MbylljaFatures.txtShumaPaguar.Text);
                        //    PayType = MbylljaFatures.cmbMenyraPageses.Text;
                        //    InsertEvidencaPagesave
                        //           (
                        //            _tempNrFatures.ToString(),
                        //            DateTime.Now,
                        //            MbylljaFatures.dtDataPerPagese.Value,
                        //            TotaliFature,
                        //            TotaliPaguar,
                        //            dlgMbylljaFatures.KMON,
                        //            PayType, cmd
                        //            );

                        //}



                        //fArt = null;
                        //                cmd.CommandText = @"UPDATE Vizitat
                        //                                                SET    IDStatusiVizites = 6,
                        //                                                       DataRealizimit = @Data_Realizimit,
                        //                                                       OraRealizimit = @Ora_Realizimit, 
                        //                                                       SyncStatus = 0
                        //                                                WHERE  IDVizita = @IDVizita";
                        //                cmd.Parameters.AddWithValue("Data_Realizimit", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        //                cmd.Parameters.AddWithValue("Ora_Realizimit", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        //                cmd.Parameters.AddWithValue("IDVizita", IDVizita);
                        //                cmd.ExecuteNonQuery();
                        //                cmd.Parameters.Clear();


                        cmd.CommandText = @"Select Count(*) from PorosiaArt where IDPorosia='" + IDPorosia + "'";
                        int _nrDetaleve = int.Parse(cmd.ExecuteScalar().ToString());


                        cmd.CommandText = @"UPDATE Porosite 
                                               SET    NrPorosise = @Nr_Porosise,
                                                      NrDetalet = @Nr_Detaleve,
                                                      Longitude = @Longitude,
                                                      Latitude = @Latitude 
                                               WHERE  IDPorosia = @IDPorosia";
                        cmd.Parameters.AddWithValue("Nr_Porosise", NrFatures.ToString());
                        cmd.Parameters.AddWithValue("@Nr_Detaleve", _nrDetaleve);
                        cmd.Parameters.AddWithValue("@IDPorosia", IDPorosia);
                        cmd.Parameters.AddWithValue("@Longitude", frmHome.LongitudeCur);
                        cmd.Parameters.AddWithValue("@Latitude", frmHome.LatitudeCur);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        //                cmd.CommandText = @"UPDATE PorosiaArt
                        //                                                SET    DeviceID = '" + frmHome.DevID + "'," +
                        //                                           "IDArsyeja = " + 1 + "" +
                        //                                    "WHERE  SasiaPorositur < 0 AND IDArsyeja IS NULL";
                        //                cmd.ExecuteNonQuery();



                        cmd.CommandText = @"UPDATE NumriFaturave
                                               SET    CurrNrFat = CurrNrFat + 1
                                                 WHERE  KOD = @IDAgjenti";
                        cmd.Parameters.AddWithValue("@IDAgjenti", frmHome.IDAgjenti);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();




                        string _getNumriFisk = "SELECT top(1) v.IDN FROM NumriFisk v WHERE v.Depo = '" + frmHome.IDAgjenti + "'";
                        int _NumriFisk = int.Parse(DbUtils.ExecSqlScalar(_getNumriFisk).ToString());
                        _NumriFisk = _NumriFisk + 1;

                        string _getVitiNumriFisk = "SELECT top(1) v.Viti FROM NumriFisk v WHERE v.Depo = '" + frmHome.IDAgjenti + "'";
                        int _VitiNumriFisk = int.Parse(DbUtils.ExecSqlScalar(_getVitiNumriFisk).ToString());

                        Guid IDLiferimi; //Generate new Guid();
                        IDLiferimi = Guid.NewGuid();
                        frmListaShitjeve.IDLiferimi = IDLiferimi;
                        frmVizitat.IDLiferimi = IDLiferimi;
                        //                string _getTotaliFatures = @"SELECT        SUM(CONVERT (Decimal(10,2),ROUND(la.Totali,2)))
                        //                            FROM          PorosiaArt pa
                        //                             INNER JOIN Liferimi l ON  pa.IDPorosia = l.IDPorosia
                        //                            INNER JOIN LiferimiArt la   ON  pa.IDArtikulli = la.IDArtikulli and pa.Seri = la.Seri
                        //
                        //                                                     
                        //                            WHERE        (pa.IDPorosia = '" + IDPorosia + "') AND la.IDLiferimi = l.IDLiferimi";





                        /*Krojojme Hederin e Liferimi*/

                        string _getIDKlientDheLokacion = "SELECT top(1) v.IDKlientDheLokacion FROM Vizitat v WHERE v.IDVizita = '" + IDVizita + "'";
                        string _IDKlientDheLokacion = DbUtils.ExecSqlScalar(_getIDKlientDheLokacion).ToString();


                        #region Insertimi i pageses

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


                        string strInsertCommand = "";
                        try
                        {
                            strInsertCommand = @"INSERT INTO EvidencaPagesave
                                                    (
                                                        NrPageses,
                                                        IDKlienti,
                                                        IDAgjenti,
                                                        NrFatures,
                                                        DataPageses,
                                                        DataPerPagese,
                                                        ShumaTotale,
                                                        ShumaPaguar,
                                                        Borxhi,
                                                        DeviceID,
                                                        SyncStatus,
                                                        KMON,
                                                        Export_Status,
                                                        PayType
                                                     ) 
                                                     VALUES 
                                                     (
                                                        '" + PagesatID + @"',
                                                        '" + _IDKlientDheLokacion + @"',
                                                        '" + frmHome.IDAgjenti + @"',
                                                        '" + _NumriFisk.ToString() + "/" + _VitiNumriFisk.ToString() + @"',
                                                        '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"',
                                                        '" + DateTime.Now.ToString("yyyy-MM-dd") + @"',
                                                        " + TotaliFautresMeTVSH.ToString() + @",
                                                        " + ShumaPaguar.ToString() + @",
                                                        " + (TotaliFautresMeTVSH - ShumaPaguar).ToString() + @",
                                                        '" + frmHome.DevID + @"',
                                                        0,
                                                        'LEK',
                                                        0,
                                                        '" + PayType + @"'
                                                      )";

                            cmd.CommandText = strInsertCommand;
                            cmd.ExecuteNonQuery();
                        }
                        catch (SqlCeException sce)
                        {
                            MessageBox.Show(sce.Message + ": Gabim gjate regjistrimit te Pageses");
                            DbUtils.WriteSQLCeErrorLog(sce, strInsertCommand);
                        }
                        catch (Exception EX)
                        {
                            MessageBox.Show(EX.Message);
                            DbUtils.WriteExeptionErrorLog(EX);
                        }

                        #endregion


                        cmd.CommandText = @"INSERT INTO Liferimi
                                                            (
	                                                            IDLiferimi,
	                                                            DataLiferuar,
	                                                            KohaLiferuar,
	                                                            TitulliLiferimit,
	                                                            DataLiferimit,
	                                                            KohaLiferimit,
	                                                            IDPorosia,
	                                                            Liferuar,
	                                                            NrLiferimit,
	                                                            CmimiTotal,
	                                                            DeviceID,
	                                                            SyncStatus,
	                                                            ShumaPaguar,
	                                                            Aprovuar,
	                                                            LLOJDOK,
	                                                            PayType,
	                                                            TotaliPaTVSH,
                                                                NrDetalet,
                                                                IDKlienti,
                                                                Depo,
                                                                Longitude ,
                                                                Latitude,
                                                                IDKthimi,
                                                                NumriFisk,
                                                                TCR,
                                                                TCROperatorCode,
                                                                TCRBusinessCode
                                                            )
                                                            VALUES
                                                            (
	                                                            '" + IDLiferimi.ToString() + "','" + "','" +
                                                                        DateTime.Now.Date + "','" +
                                                                        DateTime.Now.ToLocalTime() + "'," +
                                                                        ' ' + "'" +
                                                                        DateTime.Now.Date + "','" +
                                                                        DateTime.Now.ToLocalTime() + "','" +
                                                                        IDPorosia + "'," +
                                                                        "1," +
                                                                        NrFatures + "," +
                                                                        TotaliFautresMeTVSH + ",'" +
                                                                        frmHome.DevID + "'," +
                                                                        "0," +
                                                                        ShumaPaguar + "," +
                                                                        1 + ",'" +
                                                                        _llojDok + "','" +
                                                                        PayType + "'," +
                                                                        TotaliFaturesPaTVSH + "," +
                                                                        _nrDetaleve + ",'" +
                                                                        _IDKlientDheLokacion + "','" +
                                                                        frmHome.Depo + "','" +
                                                                        frmHome.LongitudeCur + "','" +
                                                                        frmHome.LatitudeCur + "'," +
                                                                        oldNrFisk + ","
                                                                        + _NumriFisk + ",'" +
                                                                        frmHome.TCRCode + "','" +
                                                                        frmHome.OperatorCode + "','" +
                                                                        frmHome.BusinessUnitCode + "'" +
                                                                    ")";


                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();


                        /*Copy PorosiaArt to LiferimiArt*/
                        cmd.CommandText = @"INSERT INTO LiferimiArt  
                                                      (  
                                                        IDLiferimi,  
                                                        IDArtikulli,  
                                                        Cmimi,  
                                                        SasiaLiferuar,  
                                                        SasiaPorositur,  
                                                        ArtEmri,  
                                                        Totali, 
                                                        DeviceID,  
                                                        Gratis, 
                                                        SyncStatus, 
                                                        IDArsyeja,  
                                                        CmimiPaTVSH,  
                                                        TotaliPaTVSH,  
                                                        VlefteTVSH,
                                                        Seri  
                                                      )  
                                                    SELECT '" + IDLiferimi.ToString() + "'," + @" 
                                                           pa.IDArtikulli,  
                                                           pa.CmimiAktual,  
                                                           Round(pa.SasiaPorositur,3),  
                                                           Round(pa.SasiaPorositur,3),  
                                                           pa.Emri,  
                                                           Round(pa.SasiaPorositur * pa.CmimiAktual,2),  
                                                           '" + frmHome.DevID + "'," + @" 
                                                           pa.Gratis,  
                                                           0,  
                                                           pa.IDArsyeja,  
                                                           Round(pa.CmimiAktual / 1.20,2)," +
                                                                   "Round((pa.SasiaPorositur * pa.CmimiAktual) / 1.20,2)," +
                                                                   "Round(pa.SasiaPorositur * pa.CmimiAktual,2)- Round(((pa.SasiaPorositur * pa.CmimiAktual) / 1.20),2)," +
                                                                   "pa.Seri " +
                                                "FROM   PorosiaArt pa  " +
                                                "WHERE  pa.IDPorosia = '" + IDPorosia + "'";

                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();



                        cmd.CommandText = @"UPDATE NumriFisk
                                                                 SET IDN = " + (_NumriFisk) + " where Depo = '" + frmHome.IDAgjenti + "'";
                        cmd.ExecuteNonQuery();

                        cmd.Transaction.Commit();

                        FiskalizimiBL.RegisterTCRInvoice(IDLiferimi.ToString());

                        Cursor.Current = Cursors.Default;

                        frmListaShitjeve.IDLiferimi = IDLiferimi;
                        frmListaShitjeve.IDVizita = IDVizita;
                        frmListaShitjeve.strIDPorosia = IDPorosia.ToString();
                        frmListaShitjeve.TotaliFature = TotaliFautresMeTVSH;
                        frmListaShitjeve.TotaliPaguar = ShumaPaguar;

                        if (MessageBox.Show("Kthimi i faturës përfunoi me sukses \n Dëshironi të shtypni fletëpagesën? \n", "Shtypja", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                 MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            menuFatura_Click(sender, e);

                        }
                        if (frmListaShitjeve == null) frmListaShitjeve = new frmListaShitjeve();
                        frmListaShitjeve.ShowDialog();
                        Cursor.Current = Cursors.Default;

                    }
                    catch (SqlCeException ex)
                    {
                        cmd.Transaction.Rollback();
                        //DeletePorosi();
                        PnUtils.DbUtils.WriteSQLCeErrorLog(ex, cmd.CommandText);
                        MessageBox.Show(" Realizimi i kthimit dështoi\n Kthimi duhet të përsëritet");
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        cmd.Transaction.Rollback();
                        //DeletePorosi();
                        PnUtils.DbUtils.WriteExeptionErrorLog(ex);
                        MessageBox.Show(" Realizimi i kthimit dështoi\n Kthimi duhet të përsëritet");
                        this.Close();
                    }
                    finally
                    {
                        con.Dispose();
                        cmd.Dispose();
                        //BL.SyncBL.SyncTablesWithGPS();
                        Cursor.Current = Cursors.Default;
                    }
                }
                else
                {
                    MessageBox.Show(" Realizimi i kthimit dështoi\n Kthimi për këtë faturë është realizuar më parë!");
                }
            }
        }
    }
}