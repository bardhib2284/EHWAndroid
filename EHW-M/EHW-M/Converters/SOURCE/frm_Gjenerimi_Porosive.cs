using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MobileSales
{
    public partial class frm_Gjenerimi_Porosive : Form
    {
        DataSet _tempDS;
        DataTable _tempDT;
        BindingSource _bs;
        string KPID = "-1";
        bool IsSelected, AnyKey;
        public frm_Gjenerimi_Porosive()
        {
            InitializeComponent();
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            pnl_Detalet.Enabled = true;
            grdData.Enabled=false;
            menuAnulo.Enabled=true;
            menuExit.Enabled=false;
            menuNew.Enabled=false;
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            if (grdData.VisibleRowCount != 0)
            {
                if (MessageBox.Show("Jeni të sigurt për anulim të porosisë?", "Anulimi", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (PnUtils.DbUtils.ExecSql("Delete from Krijimi_Porosive where KPID='" + KPID + "'"))
                    {
                        fillGrid();
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(PnUtils.DbUtils.ExecSql(@"INSERT INTO Krijimi_Porosive
                                                (
	                                                IDKlienti,
	                                                Emri,
	                                                IDAgjenti,
	                                                Data_Regjistrimit,
	                                                Imp_Status,
                                                    SyncStatus,
                                                    Depo,
                                                    DeviceID
                                                )
                                                VALUES
                                                (
	                                               '" + cmbKlienti.SelectedValue.ToString()+"','"+cmbKlienti.Text+"','"+frmHome.IDAgjenti+@"',getDate(),0,0,'"+frmHome.Depo+"','"+frmHome.DevID+@"'
                                                )
                                     "))
            {
                grdData.Enabled = true;
                pnl_Detalet.Enabled = false;
                menuNew.Enabled = true;
                menuExit.Enabled = true;
                menuAnulo.Enabled = true;
            }
            fillGrid();
        }

        private void fillGrid()
        {
             _tempDT = new DataTable();
            PnUtils.DbUtils.FillDataTable(_tempDT, "Select IDKlienti,Emri,KPID from Krijimi_Porosive");
            //_bs.DataSource = _tempDT;
            grdData.DataSource = _tempDT;
        }

        private void FillCmb()
        {
            _tempDS = new DataSet();
            _bs = new BindingSource();
            PnUtils.DbUtils.FillCombo(cmbKlienti, _bs, _tempDS, "Select IDKlienti, Emri from Klientet", "Emri", "IDKlienti");
        }

        private void frm_Gjenerimi_Porosive_Load(object sender, EventArgs e)
        {
            pnl_Detalet.Enabled=false;
            FillCmb();
            fillGrid();
        }

        private void grdData_MouseUp(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.DataGrid.HitTestInfo myHitTest = this.grdData.HitTest(e.X, e.Y);
            if (myHitTest.Type == DataGrid.HitTestType.Cell)
            {
                this.grdData.Select(myHitTest.Row);
                IsSelected = true;
                KPID = grdData[grdData.CurrentRowIndex, 2].ToString();
                menuAnulo.Enabled = true;
            }
        }

        private void grdData_KeyDown(object sender, KeyEventArgs e)
        {
            AnyKey =
          (e.KeyData == Keys.Enter || e.KeyData == Keys.Up || e.KeyData == Keys.Down || e.KeyData == Keys.Left || e.KeyData == Keys.Right);
            if (IsSelected == true)
            {
                if (AnyKey)
                {
                    this.grdData.Select(this.grdData.CurrentRowIndex);
                    KPID = grdData[grdData.CurrentRowIndex, 2].ToString();
                    menuAnulo.Enabled = true;
                }
            }
        }
    }
}