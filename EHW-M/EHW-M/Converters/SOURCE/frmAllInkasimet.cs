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
    public partial class frmAllInkasimet : Form
    {
        public frmAllInkasimet()
        {
            InitializeComponent();
        }

        private void menuDalja_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAllInkasimet_Load(object sender, EventArgs e)
        {
            init();
            CaclTotal();
        }

        #region Methods

        private void init()
        {
            lblEmri.Text = frmHome.EmriAgjendit;
            lblMbiemri.Text = frmHome.MbiemriAgjendit;
            lblDepo.Text = frmHome.Depo;

            string sumBorxhKESH = "SELECT CASE  \n"
           + "            WHEN SUM(ep.ShumaPaguar) IS NOT NULL THEN CONVERT(DECIMAL(15, 2), SUM(ep.ShumaPaguar)) \n"
           + "            ELSE 0  \n"
           + "       END  AS Shuma \n"
           + "FROM   EvidencaPagesave ep \n"
           + "WHERE  ep.PayType = 'KESH' \n"
           + "       AND EP.ShumaPaguar > 0 \n"
           + "       AND ep.NrFatures IS NULL";

            lblBorxhet.Text = PnUtils.DbUtils.ExecSqlScalar(sumBorxhKESH);
            lblBorxhet.Text = String.Format("{0:#,0.00}", decimal.Parse(lblBorxhet.Text));


            string sumBorxhBank = "SELECT CASE  \n"
           + "            WHEN SUM(ep.ShumaPaguar) IS NOT NULL THEN CONVERT(DECIMAL(15, 2), SUM(ep.ShumaPaguar)) \n"
           + "            ELSE 0  \n"
           + "       END  AS Shuma \n"
           + "FROM   EvidencaPagesave ep \n"
           + "WHERE  ep.PayType = 'Bank' \n"
           + "       AND EP.ShumaPaguar > 0 \n"
           + "       AND ep.NrFatures IS NULL";

            lblBorxhetBank.Text = PnUtils.DbUtils.ExecSqlScalar(sumBorxhBank);
            lblBorxhetBank.Text = String.Format("{0:#,0.00}", decimal.Parse(lblBorxhetBank.Text));



            string sumFatKESH = "SELECT CASE  \n"
           + "            WHEN SUM(ep.ShumaPaguar) IS NOT NULL THEN CONVERT(DECIMAL(15, 2), SUM(ep.ShumaPaguar)) \n"
           + "            ELSE 0  \n"
           + "       END  AS Shuma \n"
           + "FROM   EvidencaPagesave ep \n"
           + "WHERE  ep.PayType = 'KESH' \n"
           + "       AND EP.ShumaPaguar > 0 \n"
           + "       AND ep.NrFatures IS NOT NULL ";

            lblFatKESH.Text = PnUtils.DbUtils.ExecSqlScalar(sumFatKESH);
            lblFatKESH.Text = String.Format("{0:#,0.00}", decimal.Parse(lblFatKESH.Text));


            string sumFatBank = "SELECT CASE  \n"
         + "            WHEN SUM(ep.ShumaPaguar) IS NOT NULL THEN CONVERT(DECIMAL(15, 2), SUM(ep.ShumaPaguar)) \n"
         + "            ELSE 0  \n"
         + "       END  AS Shuma \n"
         + "FROM   EvidencaPagesave ep \n"
         + "WHERE  ep.PayType = 'Bank' \n"
         + "       AND EP.ShumaPaguar > 0 \n"
         + "       AND ep.NrFatures IS NOT NULL ";

            lblFatBank.Text = PnUtils.DbUtils.ExecSqlScalar(sumFatBank);
            lblFatBank.Text = String.Format("{0:#,0.00}", decimal.Parse(lblFatBank.Text));

            string sumKthimet = " \n"
           + "SELECT CASE  \n"
           + "            WHEN SUM(ep.ShumaPaguar) IS NOT NULL THEN CONVERT(DECIMAL(15, 2), SUM(ep.ShumaPaguar)) \n"
           + "            ELSE 0  \n"
           + "       END  AS Shuma \n"
           + "FROM   EvidencaPagesave ep \n"
           + "WHERE   EP.ShumaPaguar<0";
            lblSumKthimet.Text = PnUtils.DbUtils.ExecSqlScalar(sumKthimet);


            lblSumKthimet.Text = String.Format("{0:#,0.00}", decimal.Parse(lblSumKthimet.Text));
        }

        private void CaclTotal()
        {
            decimal _inkasBorxhet = 0;
            decimal _inkasFaturat = 0;

            try
            {
                _inkasBorxhet = Math.Round(decimal.Parse(lblBorxhet.Text), 2);


            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }

            try
            {
                _inkasFaturat = Math.Round(decimal.Parse(lblFatBank.Text), 2);

            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message);

            }


        }

        #endregion
    }
}