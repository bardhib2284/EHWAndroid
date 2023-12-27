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

namespace MobileSales
{
    public partial class frmListInkasimet : Form
    {
        private PnDevice devPort;
        private bool isSelected = false;

        public frmListInkasimet()
        {
            InitializeComponent();
        }

        private void FillGrid(string _nrFatures)
        {
            string _fillQuery = @"SELECT ep.NrFatures AS Fatura , ep.ShumaTotale as Totali, ep.ShumaPaguar as Paguar , ep.KMON  ,ep.PayType as Tipi, ep.NrPageses FROM EvidencaPagesave ep
                                   WHERE ep.NrFatures=" + _nrFatures + "";
            DbUtils.FillGrid(dsListInkasimet, grdListInkasimet, _fillQuery);
            if (dsListInkasimet.Tables[0].Rows.Count != 0)
            {
                grdListInkasimet.UnSelect(0);
            }
            PnFunctions.CalcSumDataColumn("Paguar", lblTot, dsListInkasimet, 3);
            lblTot.Text = "Totali: " + String.Format("{0:#,0.00}", decimal.Parse(lblTot.Text));
        }

        private void frmListInkasimet_Load(object sender, EventArgs e)
        {
            FillGrid(frmListaShitjeve.NrFatures);
        }

        private void menuDalja_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuPrinto_Click(object sender, EventArgs e)
        {
            if (isSelected)
            {
                string nrPageses = grdListInkasimet[grdListInkasimet.CurrentRowIndex, 5].ToString();//5 Index of Column[NrPageses]
                PnFunctions.PrintoFletePagesen(frmListaShitjeve.NrFatures, frmListaShitjeve.ListShitjeve_IDKlienti, nrPageses, true, devPort);
                isSelected = false;
                grdListInkasimet.UnSelect(grdListInkasimet.CurrentRowIndex);
                Cursor.Current = Cursors.Default;
            }
            else
            {
                MessageBox.Show("Selektoni nje fletë pages");
            }
        }

        #region DataGrid and BindingSource

        private void grdListInkasimet_MouseUp(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.DataGrid.HitTestInfo myHitTest = this.grdListInkasimet.HitTest(e.X, e.Y);
            if (myHitTest.Type == DataGrid.HitTestType.Cell)
            {
                this.grdListInkasimet.Select(myHitTest.Row);
                isSelected = true;

            }
        }
        private void grdListInkasimet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Right || e.KeyCode == Keys.Left)
            {
                grdListInkasimet.Select(grdListInkasimet.CurrentRowIndex);
                isSelected = true;
            }
        }

        #endregion
    }
}