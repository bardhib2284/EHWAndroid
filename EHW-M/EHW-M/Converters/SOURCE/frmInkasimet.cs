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

namespace MobileSales
{
    public partial class frmInkasimet : Form
    {
        private bool isSelected;
        private PnDevice devPort;

        #region Initialize Instance
        frmMessage msg = null;

        #endregion

        public frmInkasimet()
        {
            InitializeComponent();
        }

        private void menuDalja_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmInkasimi_Load(object sender, EventArgs e)
        {
            FillGrid();
        }

        private void FillGrid()
        {
            string sql = "SELECT ep.IDKlienti, k.Emri AS Klienti, \n"
                    + "       CONVERT(decimal(15,2),d.Detyrimi) as  Detyrimi,\n"
                    + "       CONVERT(DECIMAL(15,2), ep.ShumaPaguar)AS Paguar, \n"
                    + "       ep.KMON,ep.NrPageses \n"
                    + "FROM   EvidencaPagesave ep \n"
                    + "       INNER JOIN Klientet k \n"
                    + "            ON  k.IDKlienti = ep.IDKlienti \n"
                    + "       INNER JOIN Detyrimet d \n"
                    + "            ON  d.KOD = ep.IDKlienti \n"
                    + "WHERE  ep.NrFatures IS NULL \n"
                    + "       AND ep.DeviceID = '"+frmHome.DevID+"'";

            PnUtils.DbUtils.FillGrid(dsDetyrimet, grdDetyrimet, sql);
            if (dsDetyrimet.Tables[0].Rows.Count != 0)
            {
                grdDetyrimet.UnSelect(0);
            }
            
        
        }

        private void menuPrinto_Click(object sender, EventArgs e)
        {
            if (isSelected)
            {
                string nrPageses = grdDetyrimet[grdDetyrimet.CurrentRowIndex, 5].ToString();
                string IDKlienti = grdDetyrimet[grdDetyrimet.CurrentRowIndex, 0].ToString();
                PnFunctions.PrintoFletePagesen("", IDKlienti, nrPageses,true,devPort);
                grdDetyrimet.UnSelect(grdDetyrimet.CurrentRowIndex);
                isSelected = false;
                Cursor.Current = Cursors.Default;
            }
            else
            {
                MessageBox.Show("Selektoni nje fletë pages");
            }
        }

        #region DataGrid and BindingSource

        private void grdDetyrimet_MouseUp(object sender, MouseEventArgs e)
        {
            if (grdDetyrimet.CurrentRowIndex >= 0)
            {
                grdDetyrimet.Select(grdDetyrimet.CurrentRowIndex);
                isSelected = true;
            }
        }

        private void grdDetyrimet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode== Keys.Up ||e.KeyCode== Keys.Down ||e.KeyCode==Keys.Left||e.KeyCode== Keys.Right)
            {
                grdDetyrimet.Select(grdDetyrimet.CurrentRowIndex);
                isSelected = true;
            }
        }

        #endregion

        private void menuAll_Click(object sender, EventArgs e)
        {
            int _numRows =  dsDetyrimet.Tables[0].Rows.Count;
            if (_numRows > 0)
            {
                    Print();
                    Cursor.Current = Cursors.Default;
            }
            else
            {
                MessageBox.Show("Nuk keni fletë pagesa për printim");
                return;
            }
        }

        private void Print()
        {
            DataTable details = new DataTable();
            DataTable header = new DataTable();
            DataTable footer = new DataTable();
            PnDevice devPort = PnDevice.BluetoothPort;
            string strPortName = "";
            SqlCeDataAdapter da = null;
            PnPrint Rpt = null;
            string commHeader = "";
            string commDetail = "";
            string commFooter = "";

            try
            {
                commDetail = @"SELECT KDL.KontaktEmriMbiemri as Klienti,
                                           ep.NrFatures AS Fatura,
                                           CONVERT (DECIMAL (15,2),ep.ShumaTotale) AS Totali,
                                           CONVERT (DECIMAL (15,2),ep.ShumaPaguar) AS Paguar,
                                           ep.KMON,
                                           ep.PayType AS Tipi
                                    FROM   EvidencaPagesave ep
                                           INNER JOIN KlientDheLokacion kdl
                                                ON  ep.IDKlienti = kdl.IDKlienti
                                          WHERE  ep.NrFatures IS NULL";
                                           

                da = new SqlCeDataAdapter(commDetail, frmHome.AppDBConnectionsStr);
                da.Fill(details);


                commHeader = @"SELECT [Agjendet].[Emri]+' '+[Agjendet].[Mbiemri] AS Shitesi , 
										CONVERT(NCHAR(8),GETDATE(),3)+' ' +CONVERT(NCHAR(8),GETDATE(),8)AS Data,[Agjendet].IDAgjenti as Depo 
										FROM [Agjendet] WHERE [Depo]='" + frmHome.Depo + "'";
                da.SelectCommand.CommandText = commHeader;
                da.Fill(header);

                commFooter = @"SELECT     'Shuma e paguar ne EURO: ' + CASE WHEN SUM(ep.ShumaPaguar) IS NOT NULL THEN CONVERT(NCHAR(14), SUM(ep.ShumaPaguar)) 
                                                      ELSE '          0.00' END AS Pagesat
                                FROM         EvidencaPagesave AS ep INNER JOIN
                                                      KlientDheLokacion AS kdl ON ep.IDKlienti = kdl.IDKlienti
                                WHERE     (ep.NrFatures IS NULL) AND (ep.KMON = 'EUR')
                                UNION ALL
                                SELECT     'Shuma e paguar ne LEK: ' + CASE WHEN SUM(ep.ShumaPaguar) IS NOT NULL THEN CONVERT(NCHAR(15), SUM(ep.ShumaPaguar)) 
                                                      ELSE '           0.00' END AS Pagesat
                                FROM         EvidencaPagesave AS ep INNER JOIN
                                                      KlientDheLokacion AS kdl ON ep.IDKlienti = kdl.IDKlienti
                                WHERE     (ep.NrFatures IS NULL) AND (ep.KMON = 'LEK')
                                UNION ALL
                                SELECT     'Shuma e paguar ne USD: ' + CASE WHEN SUM(ep.ShumaPaguar) IS NOT NULL THEN CONVERT(NCHAR(15), SUM(ep.ShumaPaguar)) 
                                                      ELSE '           0.00' END AS Pagesat
                                FROM         EvidencaPagesave AS ep INNER JOIN
                                                      KlientDheLokacion AS kdl ON ep.IDKlienti = kdl.IDKlienti
                                WHERE     (ep.NrFatures IS NULL) AND (ep.KMON = 'USD')";
                da.SelectCommand.CommandText = commFooter;
                da.Fill(footer);
                string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);

                //@BE QR Code
                if (msg == null) msg = new frmMessage(true, new string[]{null});
                DialogResult a = msg.ShowDialog();
                if (a == DialogResult.Abort)
                {
                    return;//cancel printing
                }
                if (frmMessage.isPreview)
                {
                    devPort = PnDevice.PreviewOnly;

                }
                if (frmMessage.isBlutooth == true)
                {
                    devPort = PnDevice.BluetoothPort;
                    strPortName = frmHome.PnPrintPort + ":";
                }
                //if(frmMessage.isMultiplicatior)
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

                    Rpt = new PnPrint(devPort, strPortName, CurrentDir + "\\RaportiInkasimeveBorxheve.pnrep", details, header,footer);
                    Rpt.PrintPnReport(PnType.Print, true, "tre",true);
                }
                else
                {
                    Rpt = new PnPrint(devPort, strPortName, CurrentDir + "\\RaportiInkasimeveBorxheve.pnrep", details, header,footer);
                    Rpt.PrintPnReport(PnType.Preview, true, "tre",true);
                }

                // PrintBarcode(devPort, strPortName);//Print Barcode
                Cursor.Current = Cursors.WaitCursor;
                //Rpt = new PnPrint(devPort, strPortName, CurrentDir + "\\RaportiInkasimeveBorxheve.pnrep", details, header);
                //Rpt.PrintPnReport(PnType.Print, true, "tre");

            }
            catch (Exception ex)
            {
                if (ex.Message == @"Could not find file '\Flash Disk\mobilesales\RaportiInkasimeveBorxheve.pnrep'.")
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
                if (msg != null) msg.Dispose();
                msg = null;
                Cursor.Current = Cursors.Default;
            }

        }

    }
}