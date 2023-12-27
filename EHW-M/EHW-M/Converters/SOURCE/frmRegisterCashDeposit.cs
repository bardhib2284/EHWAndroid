using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using PnUtils;
using Microsoft.WindowsMobile.Samples.Location;
using MobileSales.BL;

namespace MobileSales
{
    public partial class frmRegisterCashDeposit : Form
    {
        public static int Porosia_value;
        public frmRegisterCashDeposit()
        {
            InitializeComponent();
        }

        private void frmRegisterCashDeposit_Load(object sender, EventArgs e)
        {
            menu_regjistro.Enabled = false;
            Assembly asm = Assembly.GetExecutingAssembly();//Get Version
            lblHVersion.Text = asm.GetName().Version.ToString();
            Porosia_value = 0;

        }


        private void menu_Regjistro_Click(object sender, EventArgs e)
        {

            //DataTable checkMalliMbetur = new DataTable();

            //PnUtils.DbUtils.FillDataTable(checkMalliMbetur,
            //    @"Select top(1) NrLiferimit FROM Liferimi");

            //if (checkMalliMbetur.Rows.Count == 0)
            //{
            //    SyncFiskalizimi.SinkronizoFiskalizimin();
            //}

            Cursor.Current = Cursors.WaitCursor;
            FiskalizimiBL.RegisterCashDeposit((decimal)Porosia_value, MobileSales.FiskalizimiWebService.CashDepositOperationSType.INITIAL);
            Cursor.Current = Cursors.Default;
            this.Close();
        }


        private void txtSasia_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Porosia_value = int.Parse(txtSasia.Text);
                txtSasia.Text = txtSasia.Text.Trim();
                menu_regjistro.Enabled = true;
            }
            catch (Exception)
            {
                menu_regjistro.Enabled = false;
                txtSasia.Text = "";
                txtSasia.Focus();
            }
        }

        private void txtSasia_KeyDown(object sender, KeyEventArgs e)
        {
            #region Keys.UP && Keys.Down

            if (e.KeyCode == Keys.Enter && menu_regjistro.Enabled)
            {
                this.menu_Regjistro_Click(sender, null);
            }

            if (e.KeyCode == Keys.Up)
            {
                Porosia_value = Porosia_value + 1;
                txtSasia.Text = Porosia_value.ToString();
            }

            if (e.KeyCode == Keys.Down)
            {
                if (Porosia_value > 1)
                {
                    Porosia_value = Porosia_value - 1;
                }

                txtSasia.Text = Porosia_value.ToString();
            }

            #endregion
        }

    }
}