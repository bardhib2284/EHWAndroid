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
    public partial class frmVerejtjet : Form
    {
        public static int IDVerejtja;
        public frmVerejtjet()
        {
            InitializeComponent();
        }

        private void menDalja_Click(object sender, EventArgs e)
        {
            this.Close();
            frmShitja.IDArsyeja = 0;
        }

        private void frmVerejtjet_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            if (cmbArsyeja.SelectedIndex != -1)
            {
                try
                {
                   frmShitja.IDArsyeja = int.Parse(cmbArsyeja.SelectedValue.ToString());
                }
                catch (FormatException ex)
                {
                    MessageBox.Show(ex.Message+"\n Gabim gjate selektimit te vërejtjes");
                    return;
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Selektoni nje arsye!");
                return;
            }



        }

        #region Methods

        private void Init()
        {
            string command = "Select * from Arsyejet";
            PnUtils.DbUtils.FillDataSet(dsVerejtjet, command);
            PnUtils.DbUtils.FillCombo(cmbArsyeja, dsVerejtjet, command, "Pershkrimi", "IDArsyeja");

        }

        #endregion

        
    }
}