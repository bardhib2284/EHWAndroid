using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PnUtils;
using PnReports;
using System.Data.SqlServerCe;
using System.Reflection;
using System.IO;

namespace MobileSales
{
    public partial class frmPrintInkasimet : Form
    {
        public frmPrintInkasimet()
        {
            InitializeComponent();
        }

        #region Initialize Instance

        frmMessage frmMessage = null;

        #endregion

        private void menuDalja_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuPrinto_Click(object sender, EventArgs e)
        {
            int _numRows = dsInkasimet.Tables[0].Rows.Count;
            if (_numRows > 0)
            {
                Print();
            }
            else
            {
                MessageBox.Show("Nuk keni Inkasime per printim");
                return;
            }
        }

        private void frmPrintInkasimet_Load(object sender, EventArgs e)
        {
            FillGrid();
        }

        #region Methods
       
        private void FillGrid()
        {
            int aprovuar = 0;
            if (frmHome.AprovimFaturash) aprovuar = 1;
            string _fillQuery = @"SELECT KDL.KontaktEmriMbiemri,
                                           ep.NrFatures AS Fatura,
                                           ep.ShumaTotale AS Totali,
                                           ep.ShumaPaguar AS Paguar,
                                           ep.KMON,
                                           ep.PayType AS Tipi
                                    FROM   EvidencaPagesave ep
                                           INNER JOIN KlientDheLokacion kdl
                                                ON  ep.IDKlienti = kdl.IDKlienti
                                           INNER JOIN Liferimi l
                                                ON  L.NrLiferimit = EP.NrFatures
                                    WHERE  ep.NrFatures IS NOT NULL
                                           AND L.Aprovuar = " + aprovuar + "";
            DbUtils.FillGrid(dsInkasimet, grdListInkasimet, _fillQuery);
            if (dsInkasimet.Tables[0].Rows.Count != 0)
            {
                grdListInkasimet.UnSelect(0);
            }
        }

        private void Print()
        {
            DataTable details = new DataTable();
            DataTable header = new DataTable();
            DataTable footer = new DataTable();

            string strPortName = "";
            PnDevice devPort = PnDevice.BluetoothPort ;

            SqlCeDataAdapter da = null;
            PnPrint Rpt = null;
            string commHeader = "";
            string commDetail = "";
            string commFooter = "";

            try
            {
                int aprovuar = 0;
                if (frmHome.AprovimFaturash) aprovuar = 1;
                commDetail = @"SELECT KDL.KontaktEmriMbiemri as Klienti,
                                           ep.NrFatures AS Fatura,
                                           CONVERT (DECIMAL (15,2),ep.ShumaTotale) AS Totali,
                                           CONVERT (DECIMAL (15,2),ep.ShumaPaguar) AS Paguar,
                                           ep.KMON,
                                           ep.PayType AS Tipi
                                    FROM   EvidencaPagesave ep
                                           INNER JOIN KlientDheLokacion kdl
                                                ON  ep.IDKlienti = kdl.IDKlienti
                                           INNER JOIN Liferimi l
                                                ON  L.NrLiferimit = EP.NrFatures
                                    WHERE  ep.NrFatures IS NOT NULL
                                           AND L.Aprovuar = " + aprovuar + " ORDER BY ep.NrFatures ASC ";

                da = new SqlCeDataAdapter(commDetail, frmHome.AppDBConnectionsStr);
                da.Fill(details);

                commHeader = @"SELECT [Agjendet].[Emri]+' '+[Agjendet].[Mbiemri] AS Shitesi , 
										CONVERT(NCHAR(8),GETDATE(),3)+' ' +CONVERT(NCHAR(8),GETDATE(),8)AS Data,[Agjendet].IDAgjenti as Depo 
										FROM [Agjendet] WHERE [Depo]='" + frmHome.Depo + "'";
                da.SelectCommand.CommandText = commHeader;
                da.Fill(header);


                commFooter = @"
                                SELECT 'Shuma e paguar ne EURO: ' + CASE 
                                                                         WHEN SUM(ep.ShumaPaguar) IS NOT NULL THEN 
                                                                              CONVERT(NCHAR(14), SUM(ep.ShumaPaguar))
                                                                         ELSE '          0.00'
                                                                    END AS Pagesat
                                FROM   EvidencaPagesave AS ep
                                       INNER JOIN KlientDheLokacion AS kdl
                                            ON  ep.IDKlienti = kdl.IDKlienti
                                       INNER JOIN Liferimi l
                                            ON  L.NrLiferimit = EP.NrFatures
                                WHERE  ep.NrFatures IS NOT NULL
                                       AND L.Aprovuar = " + aprovuar+""+ @"
                                       AND EP.KMON = 'EUR'

                                                                UNION ALL
                                SELECT 'Shuma e paguar ne LEK: ' + CASE 
                                                                        WHEN SUM(ep.ShumaPaguar) IS NOT NULL THEN 
                                                                             CONVERT(NCHAR(15), SUM(ep.ShumaPaguar))
                                                                        ELSE '           0.00'
                                                                   END AS Pagesat
                                FROM   EvidencaPagesave AS ep
                                       INNER JOIN KlientDheLokacion AS kdl
                                            ON  ep.IDKlienti = kdl.IDKlienti
                                       INNER JOIN Liferimi l
                                            ON  L.NrLiferimit = EP.NrFatures
                                WHERE  ep.NrFatures IS NOT NULL
                                       AND L.Aprovuar = " + aprovuar+""+@"
                                       AND EP.KMON = 'LEK'
                                                                UNION ALL
                                SELECT 'Shuma e paguar ne USD: ' + CASE 
                                                                        WHEN SUM(ep.ShumaPaguar) IS NOT NULL THEN 
                                                                             CONVERT(NCHAR(15), SUM(ep.ShumaPaguar))
                                                                        ELSE '           0.00'
                                                                   END AS Pagesat
                                FROM   EvidencaPagesave AS ep
                                       INNER JOIN KlientDheLokacion AS kdl
                                            ON  ep.IDKlienti = kdl.IDKlienti
                                       INNER JOIN Liferimi l
                                            ON  L.NrLiferimit = EP.NrFatures
                                WHERE  ep.NrFatures IS NOT NULL
                                       AND L.Aprovuar = " + aprovuar + "" + @"
                                       AND EP.KMON = 'USD'";

                da.SelectCommand.CommandText = commFooter;
                da.Fill(footer);

                string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);

                //@BE QR Code
                if (frmMessage == null) frmMessage = new frmMessage(true, new string[] { "", "", "" });
                DialogResult a = frmMessage.ShowDialog();
                if (a == DialogResult.Abort)
                {
                    return;//cancel printing
                }

                if (frmMessage.isBlutooth)
                {
                    devPort = PnDevice.BluetoothPort;
                    strPortName = frmHome.PnPrintPort + ":";

                }

                if (frmMessage.isPreview)
                {
                    devPort = PnDevice.PreviewOnly;
                }
                //if (frmMessage.isMultiplicatior)
                //{
                //    devPort = PnDevice.MultiplicatorPort;
                //    strPortName = frmHome.PnPrintPort + ":";
                //}

                if (!frmMessage.isPreview)
                {
                    System.IO.Ports.SerialPort sp = new System.IO.Ports.SerialPort(strPortName);
                    try
                    {
                        sp.Open();

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

                    // PrintBarcode(devPort, strPortName);//Print Barcode
                    Cursor.Current = Cursors.WaitCursor;
                    Rpt = new PnPrint(devPort, strPortName, CurrentDir + "\\RaportiInkasimeve.pnrep", details, header,footer);
                    Rpt.PrintPnReport(PnType.Print, true, "tre",false);
                }
                else
                {
                    Rpt = new PnPrint(devPort, strPortName, CurrentDir + "\\RaportiInkasimeve.pnrep", details, header,footer);
                    Rpt.PrintPnReport(PnType.Preview, true, "tre",false);
                }

            }
            catch (Exception ex)
            {
                if (ex.Message == @"Could not find file '\Flash Disk\mobilesales\RaportiInkasimeve.pnrep'.")
                {
                    MessageBox.Show("Mungon RaportiMbetjes");
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }

            }
            finally
            {
                details.Dispose();
                header.Dispose();
                if (Rpt != null) Rpt.Dispose();
                if (da != null) da.Dispose();
                if (frmMessage != null) frmMessage.Dispose();
                frmMessage = null;
                Cursor.Current = Cursors.Default;
            }

        }

        #endregion

        #region DataGrid && BindingSource

        private void grdListInkasimet_MouseUp(object sender, MouseEventArgs e)
        {
            if (grdListInkasimet.CurrentRowIndex >= 0)
            {
                grdListInkasimet.Select(grdListInkasimet.CurrentRowIndex);
            }
        }

        private void grdListInkasimet_KeyDown(object sender, KeyEventArgs e)
        {
            bool Key = (e.KeyData == Keys.Enter || e.KeyData == Keys.Up || e.KeyData == Keys.Down || e.KeyData == Keys.Left || e.KeyData == Keys.Right);
            if (Key &&grdListInkasimet.CurrentRowIndex>=0)
            {
                grdListInkasimet.Select(grdListInkasimet.CurrentRowIndex);
            }
        }

        #endregion

    }
}