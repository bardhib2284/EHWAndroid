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
    public partial class rptOrder_Details : Form
    {
        string _PID="";

        public rptOrder_Details()
        {
            InitializeComponent();
        }

        public rptOrder_Details(string pid)
        {
            InitializeComponent();
            _PID = pid;
        }

        private void rptOrder_Details_Load(object sender, EventArgs e)
        {
            try
            {
                this.rptOrder_DetailsTableAdapter.FillBy_ID(this.myMobileDataSet.rptOrder_Details, _PID);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}