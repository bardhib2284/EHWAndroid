using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using PnReports;

namespace MobileSales
{
    public partial class frmRpt_Porosit : Form
    {
        string _NRPOROSISE;

        private frmMessage msg = null;

        public frmRpt_Porosit()
        {
            InitializeComponent();
        }

        public frmRpt_Porosit(string _nrPorosise)
        {
            _NRPOROSISE = _nrPorosise;
            InitializeComponent();
        }

        private void menuDalja_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmRpt_Porosit_Load(object sender, EventArgs e)
        {
            InitConnectionn();

            myMobileDataSet.EnforceConstraints = false;
              rptOrder_DetailsTableAdapter.Fill_rpOrderDetails(myMobileDataSet.rptOrder_Details, _NRPOROSISE);
        }

        private void InitConnectionn()
        {
            rptOrder_DetailsTableAdapter.Connection = frmHome.AppDBConnection;
        }

        private void Print()
        {
            string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            PnPrint Rpt = null;
            PnType PT = PnType.Print;
            string _IDPorosia = frmVizitat.IDPorosi.ToString();
            DataTable details = new DataTable();
            DataTable header = new DataTable();
            DataTable footer = new DataTable();
            string strPortName = "";
            PnDevice devPort = PnDevice.BluetoothPort;
            #region PrintParams

            //@BE QR Code
            if (msg == null) msg = new frmMessage(true, new string[]{"","",""});
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

            try
            {
                DataTable[] Header_Details = PnFunctions.CreateOrdersBill(_NRPOROSISE);
                header = Header_Details[0];
                details= Header_Details[1];
                footer = Header_Details[2];

                if (details.Rows.Count > 0 && header.Rows.Count > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Rpt = new PnPrint(devPort, strPortName, CurrentDir + "\\rptPorosia.pnrep", details, header, footer);

                    Rpt.PrintPnReport(PT, true, " tre ",true);
                }
                else
                {
                    MessageBox.Show("Gabim gjatë gjenerimit te Fatures \n Printimin nuk munde të vazhdoj");
                    return;
                }
            }
            catch (FileNotFoundException fex)
            {
                MessageBox.Show("Mungon Fajlli: \n Porosia.pnrep");
                PnUtils.DbUtils.WriteExeptionErrorLog(fex);

            }
            catch (TypeLoadException)
            {
            }
            catch (MissingMethodException)
            {

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
                frmMessage.isPreview = false;
                frmMessage.isBlutooth = false;
                frmMessage.isMultiplicatior = false;
                msg = null;
                Cursor.Current = Cursors.Default;
            }

        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            Print();
        }


    }
}