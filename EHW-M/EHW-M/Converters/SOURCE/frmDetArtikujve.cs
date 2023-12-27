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
    public partial class frmDetArtikujve : Form
    {
        public static string IDArtikulli;

        public frmDetArtikujve()
        {
            InitializeComponent();
        }

        private void frmDetArtikujve_Load(object sender, EventArgs e)
        {
            artikujDepoTableAdapter.Connection.ConnectionString = frmHome.AppDBConnectionsStr;
            try
            {
                this.artikujDepoTableAdapter.FillByIDArtikulli(this.myMobileDataset.ArtikujDepo, IDArtikulli);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }
        
        private void menDlaja_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}