using PnUtils;
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
    public partial class frmArtikujtStoqet : Form
    {
        private bool isSelected, TastetUD, needRabat;
        public string position;
        private int _colSasiaIndex = 1;
        private double CmimiBoxPaRabat;

        #region Initialize Instance

        public frmShitja ShitjaCrossForm = null;
        frmDetArtikujve frmDetArtikujve = null;
        public frmPorosia_Detalet PorosiaCrossForm = null;
        public frmLevizjet LevizjetCrossForm = null;
        #endregion

        public frmArtikujtStoqet()
        {
            InitializeComponent();
        }

        public frmArtikujtStoqet(string POS, bool rabat)
        {
            position = POS;
            needRabat = rabat;
            InitializeComponent();
        }

        public frmArtikujtStoqet(string POS)
        {
            position = POS;
            InitializeComponent();
        }

        private void txtArtFilter_TextChanged(object sender, EventArgs e)
        {
            string Emri = txtArtFilter.Text;
            try
            {
                switch (position)
                {
                    case "Shitja":
                    case "Lëvizje":
                        {
                            CalcSUM(lblSum, 1);
                            break;
                        }
                    case "Porosi":
                        {
                            lblSum.Text = "";
                            break;
                        }
                }
                this.artikujtStoqetBindingSource.Filter = "Emri LIKE '" + Emri + "%'";
                isSelected = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Gabim në sistem!");
            }
        }

        private void ArtikujtStoqet_Load(object sender, EventArgs e)
        {
            artikujtDepoTableAdapter.Connection.ConnectionString = frmHome.AppDBConnectionsStr;
            artikujtStoqetTableAdapter.Connection.ConnectionString = frmHome.AppDBConnectionsStr;
            switch (position)
            {
                case "Shitja":// Nëse forma hapet nga butoni "Shitja"
                    {
                        menu_Open.Text = "Shto";
                        menu_Open.Enabled = true;
                        if (frmVizitat.KthimMall)
                        {
                            this.artikujtStoqetTableAdapter.FillByKlienti2(this.myMobileDataSet.ArtikujtStoqet, frmVizitat.Vizitat_IDKlienti);
                        }
                        else
                        {
                            this.artikujtStoqetTableAdapter.Fill_For_Sale(this.myMobileDataSet.ArtikujtStoqet, frmVizitat.Vizitat_IDKlienti);
                        }

                        CalcSUM(lblSum, _colSasiaIndex);
                        break;
                    }
                case "Porosi": //Nëse forma hapet nga butoni "Porosite"
                    {
                        menu_Open.Text = "Shto";
                        menu_Open.Enabled = true;
                        artikujtStoqetDataGrid.TableStyles[0].GridColumnStyles.RemoveAt(1);
                        artikujtStoqetDataGrid.TableStyles[0].GridColumnStyles.RemoveAt(1);
                        artikujtStoqetDataGrid.TableStyles[0].GridColumnStyles.RemoveAt(1);
                        artikujtStoqetDataGrid.TableStyles[0].GridColumnStyles.RemoveAt(1);
                        artikujtStoqetDataGrid.TableStyles[0].GridColumnStyles[0].Width = 500;
                        this.artikujtStoqetTableAdapter.FillFor_Orders(this.myMobileDataSet.ArtikujtStoqet);
                        break;
                    }
                case "Lëvizje": //Nëse forma hapet nga butoni "Levizjet"
                    {
                        menu_Open.Text = "Shto";
                        menu_Open.Enabled = true;
                        if (needRabat)
                        {
                            if (frmHome.Depo == "AT")
                            {

                                this.artikujtStoqetTableAdapter.Fill_For_Sale_AT(this.myMobileDataSet.ArtikujtStoqet, "STANDARD");
                            }
                            else
                            {
                                this.artikujtStoqetTableAdapter.FillForSale(this.myMobileDataSet.ArtikujtStoqet, "STANDARD");
                            }
                        }
                        else
                        {
                            if (frmHome.Depo == "AT")
                            {
                                this.artikujtStoqetTableAdapter.Fill_Levizja_Hyrje_AT(this.myMobileDataSet.ArtikujtStoqet, "STANDARD");
                            }
                            else
                            {
                                this.artikujtStoqetTableAdapter.Fill_Levizje_Hyrje(this.myMobileDataSet.ArtikujtStoqet, "STANDARD");
                            }
                        }

                        CalcSUM(lblSum, _colSasiaIndex);
                        break;
                    }

            }

            lblSum.Text = "Totali: " + String.Format("{0:#,0.00}", decimal.Parse(lblSum.Text));

            Cursor.Current = Cursors.Default;
        }

        private void txtArtFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtArtFilter.Text.Length == 13)
            {
                if (e.KeyData == Keys.Enter)
                {
                    try
                    {
                        string barcode = txtArtFilter.Text;
                        string Emri = txtArtFilter.Text;
                        switch (position)
                        {
                            case "Shitja":
                            case "Lëvizje":
                                {
                                    this.artikujtStoqetBindingSource.Filter = "Barkod LIKE '" + barcode + "%'";
                                    lblSum.Text = artikujtStoqetBindingSource.Count.ToString();
                                    break;
                                }
                            case "Porosi":
                                this.artikujtStoqetBindingSource.Filter = "Barkod LIKE '" + barcode + "%'";
                                lblSum.Text = "";
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ka ndodhur një gabim gjate filtrimit! " + ex.Message);
                    }
                }
            }
        }

        #region DataGrid && BindingSource

        private void artikujtStoqetDataGrid_MouseUp(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.DataGrid.HitTestInfo myHitTest = this.artikujtStoqetDataGrid.HitTest(e.X, e.Y);
            if (myHitTest.Type == DataGrid.HitTestType.Cell)
            {
                this.artikujtStoqetDataGrid.Select(myHitTest.Row);
                isSelected = true;
            }

        }

        private void artikujtStoqetDataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            /***Nese shtypim tastin ENTER pas selektimi te artikullit***/
            if (e.KeyData == Keys.Enter && isSelected)
            {
                switch (position)
                {
                    case ("Shitja"):// Nëse forma hapet nga butoni "Shitja"
                        {
                            frmShitja.IDArtikulli = lblIDArtikulli.Text;
                            frmShitja.prod_name = lblEmri.Text;
                            frmShitja.Seri = lblSeri.Text;
                            CmimiBoxPaRabat = Convert.ToDouble(lblCmimi.Text);
                            try
                            {
                                frmShitja.CopGratis = float.Parse(lblGratis.Text);
                            }
                            catch (Exception ex)
                            {
                                DbUtils.WriteExeptionErrorLog(ex);
                                frmShitja.CopGratis = 0;
                            }

                            frmShitja.MaxCop = System.Single.Parse(Math.Round(decimal.Parse(lblMaxSasia.Text), 3).ToString());
                            KontrolloSasine();
                            frmShitja.CmimiMeTVSH = System.Single.Parse(Math.Round(decimal.Parse(lblCmimi.Text), 3).ToString()); //System.Convert.ToSingle(lblCmimi.Text);
                            frmShitja.BUM = lblBUM.Text;
                            ShitjaCrossForm.lblCPako.Visible = false;
                            ShitjaCrossForm.lblPako.Visible = true;
                            ShitjaCrossForm.txtPako.SelectAll();
                            ShitjaCrossForm.txtArtikulli.Text = frmShitja.prod_name;
                            break;
                        }

                    case "Aksion":// Nëse forma hapet nga forma:Aksioni, butoni:"Artikujt"
                        {
                            frmDetArtikujve.IDArtikulli = lblIDArtikulli2.Text;
                            if (frmDetArtikujve == null) frmDetArtikujve = new frmDetArtikujve();
                            frmDetArtikujve.ShowDialog();
                            break;
                        }
                }

            }
            /*** TastetUD ->Variabel e cila tregon nese eshte shtypur ndonjeri nga tastet e kahjeve(UP,DOWN,LEFT,RIGHT) ***/
            TastetUD = (e.KeyData == Keys.Up || e.KeyData == Keys.Down || e.KeyData == Keys.Left || e.KeyData == Keys.Right);
            if (isSelected == true)
            {
                if (TastetUD)
                {
                    this.artikujtStoqetDataGrid.Select(this.artikujtStoqetDataGrid.CurrentRowIndex);
                }

            }
            txtArtFilter.Text = "";
        }

        #endregion

        #region Methods

        private void CalcSUM(Label labName, int ColIndex)
        {
            double SUM = 0;
            int numRows = int.Parse(artikujtStoqetBindingSource.Count.ToString());
            if (numRows > 0)
            {
                for (int j = 0; j < numRows; j++)
                {
                    double _tmpSum = Convert.ToDouble(this.artikujtStoqetDataGrid[j, ColIndex].ToString());
                    if (_tmpSum > 0)//Mbledhim vetem faturat e shitjes
                    {
                        SUM = SUM + Convert.ToDouble(this.artikujtStoqetDataGrid[j, ColIndex].ToString());
                    }

                }
                labName.Text = String.Format("{0:0.00 }", SUM);

            }
            else
            {
                labName.Text = "0.00";
            }
        }

        private void KontrolloSasine()
        {
            if (!frmVizitat.KthimMall)
            {
                if (frmShitja.MaxCop == 0)
                {
                    MessageBox.Show("Nuk keni sasi të mjaftueshme \n për shitjen e artikullit:" + frmShitja.prod_name);
                    return;
                }
            }
        }

        #endregion

        private void menu_Open_Click(object sender, EventArgs e)
        {
            if (isSelected == true)//Nëse selektojmë një artikull 
            {
                
                try
                {
                    switch (position)
                    {
                        case ("Shitja"):// Nëse forma hapet nga butoni Shitja
                            {
                                frmShitja.IDArtikulli = lblIDArtikulli.Text;
                                frmShitja.prod_name = lblEmri.Text;
                                frmShitja.Seri = lblSeri.Text;
                                CmimiBoxPaRabat = Convert.ToDouble(lblCmimi.Text);
                                frmShitja.MaxCop = float.Parse(lblMaxSasia.Text);
                                
                                if (!frmVizitat.KthimMall)
                                {
                                    if (frmShitja.MaxCop == 0)
                                    {
                                        MessageBox.Show("Nuk keni sasi të mjaftueshme \n për shitjen e artikullit:" + frmShitja.prod_name);
                                        break;
                                    }
                                }
                                try
                                {
                                    frmShitja.CopGratis = float.Parse(lblGratis.Text);
                                }
                                catch (Exception ex)
                                {
                                    DbUtils.WriteExeptionErrorLog(ex);
                                    frmShitja.CopGratis = 0;//Nese kemi NULL
                                }
                                frmShitja.CmimiMeTVSH = System.Convert.ToSingle(lblCmimi.Text);
                                frmShitja.BUM = lblBUM.Text;
                                ShitjaCrossForm.lblCPako.Visible = false;
                                ShitjaCrossForm.lblPako.Visible = true;
                                ShitjaCrossForm.txtPako.SelectAll();
                                ShitjaCrossForm.txtArtikulli.Text = frmShitja.prod_name;
                                ShitjaCrossForm.txtSeri.Text = frmShitja.Seri;
                                ShitjaCrossForm.btnShto.Enabled = true;
                                ShitjaCrossForm.porosiaArtDataGrid.Enabled = false;
                                ShitjaCrossForm.txtPako.TextChanged += ShitjaCrossForm.txtPako_TextChanged;
                                if (frmVizitat.KthimMall)
                                {
                                    ShitjaCrossForm.txtSeri.Enabled = true;
                                    ShitjaCrossForm.txtPako.Text = (-frmShitja.CopPako).ToString();
                                }
                                else
                                {
                                    ShitjaCrossForm.txtPako.Text = frmShitja.CopPako.ToString();
                                    if (frmShitja.Seri == null || frmShitja.Seri == "")
                                    {
                                        ShitjaCrossForm.txtSeri.Enabled = true;
                                    }
                                    else
                                    {
                                        ShitjaCrossForm.txtSeri.Enabled = false;
                                    }
                                }

                                frmShitja.ISFromaList = true;
                                ShitjaCrossForm.btnShto.Enabled = true;
                                ShitjaCrossForm.porosiaArtDataGrid.Enabled = false;
                                Hide();
                                txtArtFilter.Text = "";
                                ShitjaCrossForm.txtPako.Focus();
                                break;
                            }

                        case "Aksion": // Nëse forma hapet nga forma "Aksioni", butoni:"Artikujt"
                            {
                                frmDetArtikujve.IDArtikulli = lblIDArtikulli2.Text;
                                if (frmDetArtikujve == null) frmDetArtikujve = new frmDetArtikujve();
                                frmDetArtikujve.ShowDialog();
                                txtArtFilter.Text = "";

                                break;
                            }
                        case "Porosi":
                            {
                                PorosiaCrossForm._artName = lblEmri.Text;
                                PorosiaCrossForm._idartikulli = lblIDArtikulli.Text;
                                PorosiaCrossForm.txtArtikulli.Text = lblEmri.Text;
                                ((DataRowView)(PorosiaCrossForm.orderDetailsBindingSource.Current)).Row["IDArtikulli"] = lblIDArtikulli.Text;
                                PorosiaCrossForm.txtSasia.Text = "1";
                                PorosiaCrossForm.Porosia_value = 1;
                                PorosiaCrossForm.btnShto.Enabled = true;
                                Hide();
                                txtArtFilter.Text = "";
                                PorosiaCrossForm.txtSasia.Focus();
                                break;
                            }
                        case "Lëvizje":
                            {
                                LevizjetCrossForm.seri = lblSeri.Text;
                                LevizjetCrossForm.artikulli = lblEmri.Text;
                                LevizjetCrossForm.idArtikulli = lblIDArtikulli.Text;
                                LevizjetCrossForm.txtArtikulli.Text = lblEmri.Text;
                                LevizjetCrossForm.txtSasia.Text = "1";
                                LevizjetCrossForm.txtSeri.Text = lblSeri.Text;
                                LevizjetCrossForm.btnShto.Enabled = true;
                                LevizjetCrossForm.MaxSasia = decimal.Parse(lblMaxSasia.Text);
                                LevizjetCrossForm.Cmimi = decimal.Parse(lblCmimi.Text);
                                LevizjetCrossForm.njesia_matese = lblBUM.Text;
                                Hide();
                                txtArtFilter.Text = "";
                                LevizjetCrossForm.txtSasia.Focus();
                                break;
                            }
                    }
                }
                catch (Exception ex)
                {
                    DbUtils.WriteExeptionErrorLog(ex);
                    MessageBox.Show("Gabim në artikull");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Zgjedhni një Artikull !");
            }
           
        }

        private void menu_Close_Click(object sender, EventArgs e)
        {
            txtArtFilter.Text = "";

            switch (position)
            {
                case "Aksion":
                    ShitjaCrossForm.btnShto.Enabled = false;
                    this.Close();
                    break;
                case "Shitja":
                    ShitjaCrossForm.btnShto.Enabled = false;
                    ShitjaCrossForm.porosiaArtBindingSource.RemoveCurrent();
                    ShitjaCrossForm.porosiaArtDataGrid.Enabled = true;
                    ShitjaCrossForm.txtPako.TextChanged += ShitjaCrossForm.txtPako_TextChanged;
                    Hide();
                    break;
                case "Kthim":
                    ShitjaCrossForm.btnShto.Enabled = false;
                    ShitjaCrossForm.porosiaArtBindingSource.RemoveCurrent();
                    ShitjaCrossForm.porosiaArtDataGrid.Enabled = true;
                    ShitjaCrossForm.txtPako.TextChanged += ShitjaCrossForm.txtPako_TextChanged;
                    Hide();
                    break;
                case "Porosi":
                    PorosiaCrossForm.orderDetailsBindingSource.RemoveCurrent();
                    PorosiaCrossForm.btnShto.Enabled = false;
                    PorosiaCrossForm.btnAdd.Enabled = true;
                    PorosiaCrossForm.order_DetailsDataGrid.Enabled = true;
                    PorosiaCrossForm.txtSasia.Text = "1";
                    PorosiaCrossForm.Porosia_value = 1;
                    PorosiaCrossForm.txtSasia.Focus();
                    Hide();
                    break;
                case "Lëvizje":
                    LevizjetCrossForm.btnShto.Enabled = false;
                    LevizjetCrossForm.btnAdd.Enabled = true;
                    LevizjetCrossForm.grdDetails.Enabled = true;
                    Hide();
                    break;
            }
        }
    }
}