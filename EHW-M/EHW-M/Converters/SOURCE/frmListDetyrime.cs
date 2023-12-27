using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PnUtils;

namespace MobileSales
{
    public partial class frmListDetyrime : Form
    {
        public frmListDetyrime()
        {
            InitializeComponent();
        }

        private void frmListDetyrime_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void Init()
        {
            string sql = "SELECT * FROM Detyrimet d";
            PnUtils.DbUtils.FillDataSet(dsDetyrimet, sql);
            grdDetyrimet.DataSource = dsDetyrimet.Tables[0];
            PnFunctions.CalcSumDataColumn("Detyrimi", lblDetyrimet, dsDetyrimet, 3);
        }

        private void menuDalja_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grdDetyrimet_MouseUp(object sender, MouseEventArgs e)
        {
            this.grdDetyrimet.Select(grdDetyrimet.CurrentRowIndex);
        }

        private void grdDetyrimet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                grdDetyrimet.Select(grdDetyrimet.CurrentRowIndex);
            }
        }
    }
}