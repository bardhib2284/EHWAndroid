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
    public partial class frmMessage : Form
    {
        public static bool isBlutooth, isMultiplicatior, isPreview, isQRCode;
        public string[] _qrCodeData;

        //public frmMessage()
        //{
        //    InitializeComponent();
        //}

        public frmMessage(bool showPreviewButton)
        {
            InitializeComponent();
        }

        public frmMessage(bool showPreviewButton, string[] qrCodeData)
        {
            InitializeComponent();
            _qrCodeData = qrCodeData;
        }

        private void frmMessage_Load(object sender, EventArgs e)
        {
            if (_qrCodeData.Length > 0 && _qrCodeData[0] != null)
            {
                buttonQRCode.Visible = true;
            }
            else
            {
                buttonQRCode.Visible = false;
            }
        }

        private void btnBluetoothPort_Click(object sender, EventArgs e)
        {
            isBlutooth = true;
            this.Close();
            Cursor.Current = Cursors.WaitCursor;
        }

        private void btnMultiplicatorPort_Click(object sender, EventArgs e)
        {
            isMultiplicatior = true;
            this.Close();
            Cursor.Current = Cursors.WaitCursor;
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            isPreview = true;
            this.Close();
            Cursor.Current = Cursors.WaitCursor;

        }

        private void buttonQRCode_Click(object sender, EventArgs e)
        {
            if (_qrCodeData[0] != null)
            {
                frmQRCode frm = new frmQRCode(_qrCodeData[0], _qrCodeData[1], _qrCodeData[2]);
                frm.ShowDialog();
            }
            _qrCodeData = null;
            Cursor.Current = Cursors.Default;
            this.Close();
        }
    }
}