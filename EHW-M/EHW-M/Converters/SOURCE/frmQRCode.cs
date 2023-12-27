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
    public partial class frmQRCode : Form
    {
        private string _priceWithVAT;
        private string _transactionId;
        private string _qrText;

        public frmQRCode(string priceWithVAT, string transactionId, string qrText)
        {
            InitializeComponent();
            _priceWithVAT = priceWithVAT;
            _transactionId = transactionId;
            _qrText = qrText;
            GenerateQR();
        }

        private void InitFields()
        {
            _priceWithVAT = _transactionId = _qrText = null;

        }
        public void GenerateQR()
        {
            labelFatura.Text = _transactionId;
            labelTotali.Text = _priceWithVAT + (_priceWithVAT.Contains(".")
                                                ? " LEK"
                                                : ".00 LEK");
            //Temporally suspend drawing
            this.barcodeTest.SuspendDrawing();
            this.barcodeTest.Symbology = Neodynamic.CF.Barcode2D.Symbology2D.QRCode;
            this.barcodeTest.QRCodeVersion = Neodynamic.CF.Barcode2D.QRCodeVersion.Auto;
            this.barcodeTest.QRCodeErrorCorrectionLevel = Neodynamic.CF.Barcode2D.QRCodeErrorCorrectionLevel.Q;
            this.barcodeTest.QRCodeEncoding = Neodynamic.CF.Barcode2D.QRCodeEncoding.AlphaNumeric;
            this.barcodeTest.Code = _qrText;

            #region Define Size
            //Big Size
            //barcodeTest.QRCodeModuleSize = 0.04F;
            //barcodeTest.QuietZoneWidth = 0.8F;

            //Small Size
            barcodeTest.QRCodeModuleSize = 0.02F;
            barcodeTest.QuietZoneWidth = 0.15F;
            #endregion

            this.barcodeTest.ResumeDrawing();
        }

        private void menuItem_Dalja_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmQRCode_Closing(object sender, CancelEventArgs e)
        {
            InitFields();
        }
    }
}