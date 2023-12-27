using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;
//using PsionTeklogix.Serial;
using System.IO.Ports;
using PnReports;
using System.IO;
using System.Reflection;
using PnUtils;

namespace MobileSales
{
    public partial class frmMbetja : Form
    {
        public static string strPortName = "";
        public PnDevice devPort;

        #region InitializeInstance

        frmMessage msg = null;

        #endregion

        public frmMbetja()
        {
            InitializeComponent();
        }

        private void menuDalja_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMbetja_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            lblShitesi.Text = frmHome.EmriAgjendit;
            lblDepo.Text = frmHome.Depo;
            //UpdateAllValues();
            Update();
            FillGrid();
            #region ClacValues
            PnFunctions.CalcSumDataColumn("Pranuar", lblPranuar, dsMbetja, 3);
            PnFunctions.CalcSumDataColumn("Shitur", lblShitur, dsMbetja, 3);
            PnFunctions.CalcSumDataColumn("Kthyer", lblKthyer, dsMbetja, 3);
            PnFunctions.CalcSumDataColumn("Mbetur", lblMbetur, dsMbetja, 3);
            PnFunctions.CalcSumDataColumn("Levizje", lblLevizja, dsMbetja, 3);
            #endregion
            Cursor.Current = Cursors.Default;
        }

        private void FillGrid()
        {
            string _selectComand = "";
            try
            {
                //                _selectComand = @"SELECT mm.Emri, 
                //                                         CONVERT (decimal(15,3),mm.SasiaPranuar )AS Pranuar,
                //                                         CONVERT (decimal(15,3),mm.SasiaShitur )AS Shitur, 
                //                                         CONVERT (decimal(15,3),mm.SasiaKthyer )AS Kthyer, 
                //                                         CONVERT (decimal(15,3),mm.LevizjeStoku) AS Levizje, 
                //                                         CONVERT (decimal(15,3),mm.SasiaMbetur) AS Mbetur
                //                                  FROM   Malli_Mbetur mm 
                //                                         Where SasiaPranuar!= 0 OR SasiaKthyer <0   ";

                _selectComand = @"SELECT mm.Emri, 
                                         CONVERT (decimal(15,3),mm.SasiaPranuar )AS Pranuar,
                                         CONVERT (decimal(15,3),mm.SasiaShitur )AS Shitur, 
                                         CONVERT (decimal(15,3),mm.SasiaKthyer )AS Kthyer, 
                                         CONVERT (decimal(15,3),mm.LevizjeStoku) AS Levizje, 
                                         CONVERT (decimal(15,3),mm.SasiaMbetur) AS Mbetur,
                                         mm.IDArtikulli,
                                         mm.Seri
                                  FROM   [Malli_Mbetur] mm 
                                         Where SasiaPranuar> 0 OR SasiaKthyer <0 OR LevizjeStoku!=0";

                bool result = PnUtils.DbUtils.FillDataSet(dsMbetja, _selectComand);
                grdMbetja.DataSource = null;
                grdMbetja.DataSource = dsMbetja.Tables[0];
            }
            catch (SqlCeException ex)
            {
                MessageBox.Show(ex.Message);
                PnUtils.DbUtils.WriteSQLCeErrorLog(ex, _selectComand);
            }
        }

        private void menPrinto_Click(object sender, EventArgs e)
        {
            string _updateCommand = "";
            bool _updateState = false;
            int _numRows = dsMbetja.Tables[0].Rows.Count;
            if (_numRows > 0)
            {
                try
                {
                    _updateCommand = @"UPDATE Malli_Mbetur 
                                          SET    [Data] = GETDATE(), SyncStatus=0";

                    _updateState = PnUtils.DbUtils.ExecSql(_updateCommand);
                }
                catch (SqlCeException ex)
                {
                    MessageBox.Show(ex.Message);
                    PnUtils.DbUtils.WriteSQLCeErrorLog(ex, _updateCommand);
                }
                if (_updateState)
                {
                    Print();
                }
                else
                {
                    MessageBox.Show("Printimi nuk munde te vazhdoje. \n Problem gjate freskimit të shenimeve");
                }

            }
            else
            {
                MessageBox.Show("Nuk keni artikuj ne Mall te mbetur per Printim");
                return;
            }
        }

        private void grdMbetja_MouseUp(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.DataGrid.HitTestInfo myHitTest = this.grdMbetja.HitTest(e.X, e.Y);
            if (myHitTest.Type == DataGrid.HitTestType.Cell)
            {
                this.grdMbetja.Select(myHitTest.Row);

            }
        }

        private void Print()
        {
            DataTable details = new DataTable();
            DataTable header = new DataTable();
            PnType PT = PnType.Print;
            SqlCeDataAdapter da = null;
            PnPrint Rpt = null;
            string commHeader = "";
            string commDetail = "";

            try
            {
                commDetail = @"SELECT  (MM.Emri + '  ' + mm.Seri) as Emri, 
                                       MM.IDArtikulli, 
                                       CONVERT (decimal(15,3),mm.SasiaPranuar) AS SasiaPranuar, 
                                       CONVERT (decimal(15,3),mm.SasiaShitur) AS SasiaShitur, 
                                       CONVERT (decimal(15,3),mm.SasiaKthyer) AS SasiaKthyer, 
                                       CONVERT (decimal(15,3),mm.LevizjeStoku) AS Levizje,
                                       CONVERT (decimal(15,3),mm.SasiaMbetur) AS SasiaMbetur 
                                FROM   Malli_Mbetur mm 
                                       Where SasiaPranuar>0 OR SasiaKthyer < 0 OR LevizjeStoku!=0";

                da = new SqlCeDataAdapter(commDetail, frmHome.AppDBConnectionsStr);
                da.Fill(details);


                commHeader = @"SELECT [Agjendet].[Emri]+' '+[Agjendet].[Mbiemri] AS Shitesi , 
										CONVERT(NCHAR(8),GETDATE(),3)+' ' +CONVERT(NCHAR(8),GETDATE(),8)AS Data,[Agjendet].IDAgjenti as Depo 
										FROM [Agjendet] WHERE [Depo]='" + frmHome.Depo + "'";
                da.SelectCommand.CommandText = commHeader;
                da.Fill(header);
                string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);

                //@BE QR Code
                if (msg == null) msg = new frmMessage(false, new string[] { null });
                DialogResult a = msg.ShowDialog();
                if (a == DialogResult.Abort)
                {
                    return;//cancel printing
                }
                if (frmMessage.isPreview)
                {
                    PT = PnType.Preview;
                    devPort = PnDevice.PreviewOnly;
                }

                if (frmMessage.isBlutooth == true)
                {
                    PT = PnType.Print;
                    devPort = PnDevice.BluetoothPort;
                    strPortName = frmHome.PnPrintPort + ":";
                }
                //if(frmMessage.isMultiplicatior==true)
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
                Cursor.Current = Cursors.WaitCursor;
                Rpt = new PnPrint(devPort, strPortName, CurrentDir + "\\RaportiMbetjes.pnrep", details, header);
                Rpt.PrintPnReport(PT, true, "tre", false);
            }
            catch (Exception ex)
            {
                if (ex.Message == @"Could not find file '\Flash Disk\mobilesales\RaportiMbetjes.pnrep'.")
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

        /// <summary>
        /// Metode e cila Freskon shenime e kolonave Shitur|Kthyer|Mbetur para gjenerimit te raportit
        /// </summary>
        private void UpdateAllValues()
        {
            PnUtils.DbUtils.UpateMalli_Mbetur_SHITUR();
            PnUtils.DbUtils.UpdateMalli_Mbetur_KTHYER();
            PnUtils.DbUtils.UpdateMalli_Mbetur_Mbetur();
        }



    }
}