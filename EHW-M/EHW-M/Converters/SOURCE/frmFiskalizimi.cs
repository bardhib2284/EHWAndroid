using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PnUtils;
using System.Data.SqlServerCe;
using System.IO;
using System.Reflection;
using PnReports;
using MobileSales.BL;

namespace MobileSales
{
    public partial class frmFiskalizimi : Form
    {
        private int _cmbSelectedIndex;
        private DataTable _dataTable;
        public frmFiskalizimi()
        {
            InitializeComponent();
        }

        private void menu_EXIT_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmFiskalizimi_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            _cmbSelectedIndex = 0;
            fill_cmb();
            fillGrid();
            Cursor.Current = Cursors.Default;
        }


        private void menu_Rififkalizo_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            switch (_cmbSelectedIndex)
            {
                case 0:
                    FiskalizimiBL.SyncTCRCashRegisterTable(1);
                    break;
                case 1:
                    FiskalizimiBL.SyncTCRInvoices(1);
                    break;
                case 2:
                    FiskalizimiBL.SyncTCRInvoices(2);
                    break;
                case 3:
                    FiskalizimiBL.SyncTCRWTN(1);
                    break;
            }
            fillGrid();
            Cursor.Current = Cursors.Default;
            MessageBox.Show("Procesi i fiskalizimit (Offline) përfundoj!", "Shënim!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
        }


        #region Methods

        private void fill_cmb()
        {
            List<ComboBoxLlojiItem> cmbItems = new List<ComboBoxLlojiItem>();
            cmbItems.Add(new ComboBoxLlojiItem() { Id = 0, Description = "Regjistrimi i arkës" });
            cmbItems.Add(new ComboBoxLlojiItem() { Id = 1, Description = "Shitjet" });
            cmbItems.Add(new ComboBoxLlojiItem() { Id = 2, Description = "Kthimet" });
            cmbItems.Add(new ComboBoxLlojiItem() { Id = 3, Description = "Lëvizjet" });


            cmb_TRANSFER.Items.Clear();
            cmb_TRANSFER.DataSource = cmbItems.ToList();
            cmb_TRANSFER.DisplayMember = "Description";
            cmb_TRANSFER.ValueMember = "Id";

        }

        private void fillGrid()
        {
            Cursor.Current = Cursors.WaitCursor;
            grdDetails.DataSource = null;
            string sqlQuery = "";
            int width = 100;

            switch (_cmbSelectedIndex)
            {
                case 0:
                    sqlQuery = @"Select RegisterDate as ""Data"", Cashamount Shuma, Message as Mesazhi from CashRegister
                                 where TCRSyncStatus is null or TCRSyncStatus <= 0";
                    width = 220;
                    break;
                case 1:
                    sqlQuery = @"Select NrLiferimit ""Nr. Shitjes"", Message as Mesazhi  from Liferimi
                                 where (TCRSyncStatus is null or TCRSyncStatus <=0) and CmimiTotal >= 0";
                    width = 155;
                    break;
                case 2:
                    sqlQuery = @"Select NrLiferimit ""Nr. Kthimit"", Message as Mesazhi  from Liferimi
                                 where (TCRSyncStatus is null or TCRSyncStatus <=0) and CmimiTotal < 0";
                    width = 155;
                    break;
                case 3:
                    sqlQuery = @"Select distinct lh.Numri_Levizjes as ""Numri i Lëvizjes"", lh.Message as Mesazhi from Levizjet_header lh
                                    inner join LEVIZJET_DETAILS l on l.Numri_Levizjes = lh.Numri_Levizjes
                                    where (lh.TCRSyncStatus is null or lh.TCRSyncStatus <=0) and RTRIM(LTRIM(lh.Levizje_Nga)) ='" + frmHome.IDAgjenti + @"' 
                                        and l.Sasia >= 0.1";
                    width = 175;
                    break;

            }
            _dataTable = new DataTable();

            PnUtils.DbUtils.FillDataTable(_dataTable, sqlQuery);
            grdDetails.DataSource = _dataTable;

            #region Set DataGrid Layout
            grdDetails.TableStyles.Clear();
            DataGridTableStyle tableStyle = new DataGridTableStyle();
            tableStyle.MappingName = _dataTable.TableName;

            foreach (DataColumn item in _dataTable.Columns)
            {
                DataGridTextBoxColumn tbcName = new DataGridTextBoxColumn();
                if (item.ColumnName == "Mesazhi")
                    tbcName.Width = 500;
                else
                    tbcName.Width = width;
                tbcName.MappingName = item.ColumnName;
                tbcName.HeaderText = item.ColumnName;
                tableStyle.GridColumnStyles.Add(tbcName);
            }
            grdDetails.TableStyles.Add(tableStyle);
            grdDetails.Refresh();
            Application.DoEvents();

            #endregion
            Cursor.Current = Cursors.Default;
        }

        #endregion

        private void cmb_TRANSFER_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _cmbSelectedIndex = (int)(cmb_TRANSFER.SelectedValue);

            }
            catch (Exception ex)
            {
            }

            fillGrid();
            if (grdDetails.VisibleRowCount > 0)
            {
                menu_Rifiskalizo.Enabled = true;
            }
            else
            {
                menu_Rifiskalizo.Enabled = false;
            }
        }

        private void grdDetails_DoubleClick(object sender, EventArgs e)
        {
            if (grdDetails.VisibleRowCount > 0 && _cmbSelectedIndex > 0)
            {
                try
                {
                    string message = grdDetails[grdDetails.CurrentRowIndex, 2].ToString();
                    MessageBox.Show(message, "Mesazhi i gabimit!", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                }
                catch (Exception ex)
                {
                }
            }
        }
    }


    public class ComboBoxLlojiItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
