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
    public partial class frmStoku : Form
    {
        private bool Key, isSelected;
        private int _colSasiaIndex = 2;

        public frmStoku()
        {
            InitializeComponent();
        }

        private void Stoku_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            InitConnection();

            try
            {
                //Mbushja e listës së artikujve
                this.artikujDepoTableAdapter.FillArtikujtDepo(this.myMobileDataSet.ArtikujDepo, frmHome.Depo);
                if (artikujDepoDataGrid.VisibleRowCount >= 1)//Nëse lista e artikujve ka më shumë se një artikull
                {
                    //Selektojmë rreshtin e parë 
                    artikujDepoDataGrid.Select(0);
                    CalcSUM(lblSum, _colSasiaIndex);

                    lblSum.Text = "Totali: " + string.Format("{0:#,0.00}", decimal.Parse(lblSum.Text));
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+": Gabim gjate leximit te artikujve");
                DbUtils.WriteExeptionErrorLog(ex);
            }
            Cursor.Current = Cursors.Default;
        }

        private void txtArtikulli_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.artikujDepoBindingSource.Filter = "Emri LIKE '" + txtArtikulli.Text + "%'";
                CalcSUM(lblSum, _colSasiaIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+": Gabim gjate filtrimi");
                DbUtils.WriteExeptionErrorLog(ex);
                return;
            }
        }

        private void menDalja_Click(object sender, EventArgs e)
        {
            txtArtikulli.Text = "";
            this.Close();
        }

        private void txtArtikulli_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtArtikulli.Text.Length == 13)
            {
                if (e.KeyData == Keys.Enter)
                {
                    try
                    {

                        string barcode = txtArtikulli.Text;
                        string Emri = txtArtikulli.Text;
                        this.artikujDepoBindingSource.Filter = "Barkod LIKE '" + barcode + "%'";
                        DataRow rw = ((DataRowView)(this.artikujDepoBindingSource.Current)).Row;
                        
                        if (artikujDepoDataGrid.VisibleRowCount != 0)
                        {
                            this.artikujDepoDataGrid.Select(0);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Gabim gjate filtrimi: " + ex.Message);
                        DbUtils.WriteExeptionErrorLog(ex);
                    }
                }
            }

        }

        #region Methods

        private void CalcSUM(Label labName, int ColIndex)
        {
            double SUM = 0;
            int numRows = int.Parse(artikujDepoBindingSource.Count.ToString());
            if (numRows > 0)
            {
                try
                {
                    for (int j = 0; j < numRows; j++)
                    {
                        double _tmpSum = Convert.ToDouble(this.artikujDepoDataGrid[j, ColIndex].ToString());
                        if (_tmpSum > 0)//Mbledhim vetem faturat e shitjes
                        {
                            SUM = SUM + Convert.ToDouble(this.artikujDepoDataGrid[j, ColIndex].ToString());
                        }


                    }
                    labName.Text = String.Format("{0:0.00 }", SUM);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message+": Gabim gjate mbledhjes");
                    DbUtils.WriteExeptionErrorLog(ex);
                }

            }
            else
            {
                labName.Text = "0.00";
            }
        }
        
        private void InitConnection()
        {
            artikujDepoTableAdapter.Connection.ConnectionString = frmHome.AppDBConnectionsStr;
        }

        #endregion

        #region DataGrid && BindingSource

        private void artikujDepoDataGrid_MouseUp(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.DataGrid.HitTestInfo myHitTest = this.artikujDepoDataGrid.HitTest(e.X, e.Y);
            if (myHitTest.Type == DataGrid.HitTestType.Cell)
            {
                this.artikujDepoDataGrid.Select(myHitTest.Row);
                isSelected = true;
            }

        }

        private void artikujDepoDataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            Key = (e.KeyData == Keys.Enter || e.KeyData == Keys.Up || e.KeyData == Keys.Down || e.KeyData == Keys.Left || e.KeyData == Keys.Right);
            if (isSelected == true)
            {
                if (Key)
                {
                    this.artikujDepoDataGrid.Select(this.artikujDepoDataGrid.CurrentRowIndex);

                }

            }
        }

        #endregion


    }
}