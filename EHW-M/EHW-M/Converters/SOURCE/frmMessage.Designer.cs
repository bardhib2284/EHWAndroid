namespace MobileSales
{
    partial class frmMessage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMessage));
            this.btnBluetoothPort = new System.Windows.Forms.Button();
            this.btnMultiplicatorPort = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.buttonQRCode = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnBluetoothPort
            // 
            this.btnBluetoothPort.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnBluetoothPort.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.btnBluetoothPort, "btnBluetoothPort");
            this.btnBluetoothPort.Name = "btnBluetoothPort";
            this.btnBluetoothPort.Click += new System.EventHandler(this.btnBluetoothPort_Click);
            // 
            // btnMultiplicatorPort
            // 
            this.btnMultiplicatorPort.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnMultiplicatorPort.DialogResult = System.Windows.Forms.DialogResult.Yes;
            resources.ApplyResources(this.btnMultiplicatorPort, "btnMultiplicatorPort");
            this.btnMultiplicatorPort.Name = "btnMultiplicatorPort";
            this.btnMultiplicatorPort.Click += new System.EventHandler(this.btnMultiplicatorPort_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Abort;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            resources.ApplyResources(this.btnPreview, "btnPreview");
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // buttonQRCode
            // 
            this.buttonQRCode.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonQRCode.DialogResult = System.Windows.Forms.DialogResult.Abort;
            resources.ApplyResources(this.buttonQRCode, "buttonQRCode");
            this.buttonQRCode.Name = "buttonQRCode";
            this.buttonQRCode.Click += new System.EventHandler(this.buttonQRCode_Click);
            // 
            // frmMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.buttonQRCode);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnMultiplicatorPort);
            this.Controls.Add(this.btnBluetoothPort);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "frmMessage";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMessage_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBluetoothPort;
        private System.Windows.Forms.Button btnMultiplicatorPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button buttonQRCode;

    }
}