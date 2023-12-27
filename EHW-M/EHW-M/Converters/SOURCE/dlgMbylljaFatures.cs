

using PnUtils;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace MobileSales
{
    public partial class dlgMbylljaFatures : Form
    {
        public static string KMON;
        public static string totali, paguar;
        private decimal _paguarMeHeret;
        public static string _PayType = "";
        public dlgMbylljaFatures()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(string IDKlienti, decimal ShumaTotaleFatures, decimal ShumaEInkasuarMeHeret)
        {
            strIDKlienti = IDKlienti;
            ShumaTotale = ShumaTotaleFatures;
            KMON = cmbMoney.Text;
            _paguarMeHeret = ShumaEInkasuarMeHeret;
            return this.ShowDialog();
        }

        private string strIDKlienti = "";
        public static decimal ShumaTotale = 0;
        public static decimal ShumaPaguar = 0;


        #region HelperFunctions
        //functions checks if Client has debt, returns TRUE if it HAS otherwise FALSE
        private bool HasDebt(string IDKlienti)
        {
            bool res = false;

            if (IDKlienti.Trim() == "")
                return false;

            string strKom =
            "SELECT SUM(Borxhi) FROM EvidencaPagesave WHERE IDKlienti='" + strIDKlienti + "'";

            SqlCeConnection conn = new SqlCeConnection(frmHome.AppDBConnectionsStr);
            SqlCeCommand kom = new SqlCeCommand(strKom, conn);
            try
            {
                conn.Open();
                string a = kom.ExecuteScalar().ToString();
                if (a != "")
                {

                    float DebtSum = float.Parse(a);
                    conn.Close();
                    if (DebtSum > 0)
                        res = true;
                    else
                        res = false;
                }
                else res = false;
            }
            finally
            {
                conn.Dispose();
                kom.Dispose();
            }


            return res;
        }
        #endregion

        private void rbtnPartial_CheckedChanged(object sender, EventArgs e)
        {
            txtShumaPaguar.Enabled = rbtnPartial.Checked;
            dtDataPerPagese.Enabled = rbtnPartial.Checked;
            txtShumaPaguar.Text = rbtnPartial.Checked ? ShumaPaguar.ToString() : ShumaTotale.ToString();
        }

        frmInkasimiMobil Inkasimi = null;
        private void btnInkasimiMobil_Click(object sender, EventArgs e)
        {
            if (Inkasimi == null) Inkasimi = new frmInkasimiMobil();
            Inkasimi.cmbKlienti.Enabled = false;
            KMON = cmbMoney.Text;
            //            Inkasimi.strIDKlienti = strIDKlienti;
            Inkasimi.ShowDialog(strIDKlienti);
        }

        private void dlgMbylljaFatures_Load(object sender, EventArgs e)
        {
            cmbMenyraPageses.SelectedIndex = 0;
            if (_paguarMeHeret != 0)
            {
                lblShumaFatures.Text = "Shuma e faturës = " + String.Format("{0:#,0.00}", ShumaTotale);
                lblGjelbrimi.Text = "Shuma per kthim =" + String.Format("{0:#,0.00}", _paguarMeHeret);
                lblShumaPaguar.Text = "Shume e kthyer";
                txtShumaPaguar.Text = (ShumaTotale - _paguarMeHeret).ToString();
                if (_paguarMeHeret < 0)
                {
                    cmbMenyraPageses.Enabled = false;
                }
            }
            else
            {
                lblShumaFatures.Text = "Shuma e faturës = " + String.Format("{0:#,0.00}", ShumaTotale);
                lblGjelbrimi.Visible = false;
                lblShumaPaguar.Text = "Shume e paguar";
                txtShumaPaguar.Text = ShumaTotale.ToString();
            }

            cmbMoney.SelectedIndex = 0;
            totali = ShumaTotale.ToString();

        }

        private void txtShumaPaguar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ShumaPaguar = Math.Round(decimal.Parse(txtShumaPaguar.Text), 2);
            }
            catch (Exception ex)
            {
                DbUtils.WriteExeptionErrorLog(ex);
                MessageBox.Show("Vetëm numrat lejohen të shkruhen!");
                txtShumaPaguar.Text = "0";
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("Jeni të sigurtë të inkasoni vlerën: " + lblShumaFatures.Text + " ?", "Verejtje", MessageBoxButtons.YesNo, MessageBoxIcon.Hand,
            //          MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            //{

            //}
            //else
            //{
            //    rbtnPartial_CheckedChanged(null, null);
            //    return;
            //}
        }

        private void cmbMoney_SelectedIndexChanged(object sender, EventArgs e)
        {
            KMON = cmbMoney.Text;
        }

        private void dlgMbylljaFatures_Closing(object sender, CancelEventArgs e)
        {
            _PayType = cmbMenyraPageses.Text;
            frmInkasimiMobil._pType = cmbMenyraPageses.Text;
            cmbMenyraPageses.Enabled = true;
        }

        private void menu_OK_Click(object sender, EventArgs e)
        {

        }

        private void cmbMenyraPageses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMenyraPageses.Text == "Zgjedh")
            {
                btnOK.Enabled = false;
            }
            else
            {
                btnOK.Enabled = true;
            }

            if (cmbMenyraPageses.Text == "Bank")
            {
                txtShumaPaguar.Text = "0";
            }
            else
            {
                txtShumaPaguar.Text = ShumaTotale.ToString();
            }
            decimal _ShumaParaprake = 0;
            if (cmbMenyraPageses.Text == "KESH")
            {

                string _getKlinetiNIPT = @"SELECT top(1) k.NIPT FROM Vizitat v 
                                                        inner join Klientet k on k.IDKlienti = v.IDKlientDheLokacion
                                                        WHERE v.IDVizita = '" + frmVizitat.CurrIDV + "'";
                string _KlinetiNIPT = DbUtils.ExecSqlScalar(_getKlinetiNIPT).ToString();

                //if (!frmVizitat.KthimMall)
                //{
                    string _getLiferimiTotal = @"select sum(l.CmimiTotal) _ShumaParaprake from Liferimi l
                                                 inner join Klientet k on k.IDKlienti = l.IDKlienti
                                                 where l.PayType = 'KESH' and 
                                                      l.DataLiferimit = DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0) and 
                                                      k.NIPT = '" + _KlinetiNIPT + "'";

                    try
                    {
                        _ShumaParaprake = decimal.Parse(DbUtils.ExecSqlScalar(_getLiferimiTotal).ToString());
                    }
                    catch { }
//                }
//                else
//                {
//                    string _getLiferimiTotal = @"select sum(l.CmimiTotal) _ShumaParaprake from Liferimi l
//                                                 inner join Klientet k on k.IDKlienti = l.IDKlienti
//                                                 where l.PayType = 'KESH' and 
//                                                 l.CmimiTotal < 0 AND 
//                                                 l.DataLiferimit = DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0) and 
//                                                 k.NIPT = '" + _KlinetiNIPT + "'";

//                    try
//                    {
//                        _ShumaParaprake = decimal.Parse(DbUtils.ExecSqlScalar(_getLiferimiTotal).ToString());
//                    }
//                    catch { }
//                }

                decimal total = _ShumaParaprake + ShumaTotale;

                if (total > 150000 || total < -150000)
                {
                    MessageBox.Show("Nuk lejohet faturimi me mënyren e pagesës 'KESH' nëse vlera totale e shitjes ditore është më e madhe se 150000 LEK!");

                    cmbMenyraPageses.Text = "Bank";
                    txtShumaPaguar.Text = "0";
                }
            }
        }
    }
}