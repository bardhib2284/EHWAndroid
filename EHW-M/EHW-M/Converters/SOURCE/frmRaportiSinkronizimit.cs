using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;

namespace MobileSales
{
    public partial class frmRaportiSinkronizimit : Form
    {
        private void frmRaportiSinkronizimit_Load(object sender, EventArgs e)
        {
            FillGrid(frmHome.IDAgjenti);
        }

        public frmRaportiSinkronizimit()
        {
            InitializeComponent();
        }

        private void menuDalja_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuPrinto_Click(object sender, EventArgs e)
        {
           PnFunctions.RaportiSinkronizimit(frmHome.IDAgjenti);
        }

        #region Methods

        private void FillGrid(string IDAgjenti)
        {
            DataTable details = null;
            try
            {
                lblShitesi.Text = frmHome.EmriAgjendit +" "+ frmHome.MbiemriAgjendit;
                lblDepo.Text = frmHome.Depo;
                string _fillDetails = @"SELECT COUNT(*) AS Nr,
                                                   'Hedera te faturave per sinkronizim' AS Raporti
                                            FROM   Liferimi l
                                                                                        UNION  ALL
                                            SELECT COUNT(*),
                                                   'Lines te faturave per sinkronizim'
                                            FROM   LiferimiArt la
                                                                                        UNION ALL
                                            SELECT COUNT(*),
                                                   'Inkasime per sinkronzim'
                                            FROM   EvidencaPagesave ep
                                                                                        UNION  ALL
                                            SELECT COUNT(*),
                                                   'Artikuj per shkarkim'
                                            FROM   Malli_Mbetur";

                details = new DataTable();
                PnUtils.DbUtils.FillDataTable(details, _fillDetails);

                lbl1.Text = details.Rows[0][0].ToString();
                lbl2.Text = details.Rows[1][0].ToString();
                lbl3.Text = details.Rows[2][0].ToString();
                lbl4.Text = details.Rows[3][0].ToString();

                lblS1.Text = details.Rows[0][1].ToString();
                lblS2.Text = details.Rows[1][1].ToString();
                lblS3.Text = details.Rows[2][1].ToString();
                lblS4.Text = details.Rows[3][1].ToString();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                PnUtils.DbUtils.WriteExeptionErrorLog(ex);

            }
            finally
            {
                details.Dispose();
            }


        }

        #endregion

       
    }
}