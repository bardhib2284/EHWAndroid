namespace MobileSales
{
    partial class frmQRCode
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label lblNrPorosise;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem_Dalja = new System.Windows.Forms.MenuItem();
            this.barcodeTest = new Neodynamic.CF.Barcode2D.Barcode2D();
            this.labelFatura = new System.Windows.Forms.Label();
            this.labelTotali = new System.Windows.Forms.Label();
            lblNrPorosise = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblNrPorosise
            // 
            lblNrPorosise.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            lblNrPorosise.Location = new System.Drawing.Point(17, 18);
            lblNrPorosise.Name = "lblNrPorosise";
            lblNrPorosise.Size = new System.Drawing.Size(69, 12);
            lblNrPorosise.Text = "Nr.Faturës";
            // 
            // label3
            // 
            label3.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            label3.Location = new System.Drawing.Point(17, 40);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(128, 12);
            label3.Text = "Shuma totale e Faturës";
            // 
            // label4
            // 
            label4.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            label4.Location = new System.Drawing.Point(17, 72);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(209, 47);
            label4.Text = "Për detajet e faturës së fiskalizuar, ju lutem referohuni përmes QR Code!";
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem_Dalja);
            // 
            // menuItem_Dalja
            // 
            this.menuItem_Dalja.Text = "Dajla";
            this.menuItem_Dalja.Click += new System.EventHandler(this.menuItem_Dalja_Click);
            // 
            // barcodeTest
            // 
            this.barcodeTest.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.barcodeTest.AztecCodeErrorCorrection = 23;
            this.barcodeTest.AztecCodeFormat = Neodynamic.CF.Barcode2D.AztecCodeFormat.Auto;
            this.barcodeTest.AztecCodeModuleSize = 0.02F;
            this.barcodeTest.AztecCodeRune = -1;
            this.barcodeTest.BarColor = System.Drawing.Color.Black;
            this.barcodeTest.BarHeight = 0.4F;
            this.barcodeTest.BarRatio = 2;
            this.barcodeTest.BarWidth = 0.01F;
            this.barcodeTest.BorderColor = System.Drawing.Color.Black;
            this.barcodeTest.BorderWidth = 0;
            this.barcodeTest.BottomMargin = 0F;
            this.barcodeTest.Code = "123456789";
            this.barcodeTest.Code16kMode = Neodynamic.CF.Barcode2D.Code16k.Mode0;
            this.barcodeTest.DataMatrixEncoding = Neodynamic.CF.Barcode2D.DataMatrixEncoding.Auto;
            this.barcodeTest.DataMatrixFormat = Neodynamic.CF.Barcode2D.DataMatrixFormat.Auto;
            this.barcodeTest.DataMatrixModuleSize = 0.04F;
            this.barcodeTest.DataMatrixProcessTilde = false;
            this.barcodeTest.Location = new System.Drawing.Point(17, 103);
            this.barcodeTest.MicroPdf417Version = Neodynamic.CF.Barcode2D.MicroPdf417Version.Auto;
            this.barcodeTest.Name = "barcodeTest";
            this.barcodeTest.Pdf417AspectRatio = 0F;
            this.barcodeTest.Pdf417Columns = 5;
            this.barcodeTest.Pdf417CompactionType = Neodynamic.CF.Barcode2D.Pdf417CompactionType.Binary;
            this.barcodeTest.Pdf417ErrorCorrectionLevel = Neodynamic.CF.Barcode2D.Pdf417ErrorCorrection.Level2;
            this.barcodeTest.Pdf417FileId = "000";
            this.barcodeTest.Pdf417Rows = 0;
            this.barcodeTest.Pdf417SegmentCount = 0;
            this.barcodeTest.Pdf417SegmentIndex = 0;
            this.barcodeTest.Pdf417Truncated = false;
            this.barcodeTest.QRCodeEncoding = Neodynamic.CF.Barcode2D.QRCodeEncoding.Auto;
            this.barcodeTest.QRCodeErrorCorrectionLevel = Neodynamic.CF.Barcode2D.QRCodeErrorCorrectionLevel.M;
            this.barcodeTest.QRCodeModuleSize = 0.08F;
            this.barcodeTest.QRCodeVersion = Neodynamic.CF.Barcode2D.QRCodeVersion.Auto;
            this.barcodeTest.QuietZoneWidth = 0.8F;
            this.barcodeTest.Size = new System.Drawing.Size(206, 184);
            this.barcodeTest.Symbology = Neodynamic.CF.Barcode2D.Symbology2D.DataMatrix;
            this.barcodeTest.TabIndex = 0;
            this.barcodeTest.Text = "barcode2D1";
            this.barcodeTest.TextAlignment = Neodynamic.CF.Barcode2D.Alignment.AboveCenter;
            this.barcodeTest.TopMargin = 0F;
            // 
            // labelFatura
            // 
            this.labelFatura.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.labelFatura.Location = new System.Drawing.Point(141, 18);
            this.labelFatura.Name = "labelFatura";
            this.labelFatura.Size = new System.Drawing.Size(85, 20);
            this.labelFatura.Text = "label1";
            this.labelFatura.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelTotali
            // 
            this.labelTotali.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.labelTotali.Location = new System.Drawing.Point(141, 38);
            this.labelTotali.Name = "labelTotali";
            this.labelTotali.Size = new System.Drawing.Size(85, 20);
            this.labelTotali.Text = "label2";
            this.labelTotali.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // frmQRCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Snow;
            this.ClientSize = new System.Drawing.Size(240, 290);
            this.ControlBox = false;
            this.Controls.Add(this.labelTotali);
            this.Controls.Add(this.labelFatura);
            this.Controls.Add(label4);
            this.Controls.Add(label3);
            this.Controls.Add(lblNrPorosise);
            this.Controls.Add(this.barcodeTest);
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "frmQRCode";
            this.Text = "QR Code";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmQRCode_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private Neodynamic.CF.Barcode2D.Barcode2D barcodeTest;
        private System.Windows.Forms.Label labelFatura;
        private System.Windows.Forms.Label labelTotali;
        public System.Windows.Forms.MainMenu mainMenu1;
        public System.Windows.Forms.MenuItem menuItem_Dalja;

    }
}