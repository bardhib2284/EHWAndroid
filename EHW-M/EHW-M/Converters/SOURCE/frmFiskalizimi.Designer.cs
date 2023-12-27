namespace MobileSales
{
    partial class frmFiskalizimi
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menu_EXIT = new System.Windows.Forms.MenuItem();
            this.menu_Rifiskalizo = new System.Windows.Forms.MenuItem();
            this.cmb_TRANSFER = new System.Windows.Forms.ComboBox();
            this.lbl_Transfer = new System.Windows.Forms.Label();
            this.grdDetails = new System.Windows.Forms.DataGrid();
            this.pnlAll = new System.Windows.Forms.Panel();
            this.pnlAll.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menu_EXIT);
            this.mainMenu1.MenuItems.Add(this.menu_Rifiskalizo);
            // 
            // menu_EXIT
            // 
            this.menu_EXIT.Text = "Dalja";
            this.menu_EXIT.Click += new System.EventHandler(this.menu_EXIT_Click);
            // 
            // menu_Rifiskalizo
            // 
            this.menu_Rifiskalizo.Text = "Fiskalizo";
            this.menu_Rifiskalizo.Click += new System.EventHandler(this.menu_Rififkalizo_Click);
            // 
            // cmb_TRANSFER
            // 
            this.cmb_TRANSFER.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular);
            this.cmb_TRANSFER.Location = new System.Drawing.Point(3, 24);
            this.cmb_TRANSFER.Name = "cmb_TRANSFER";
            this.cmb_TRANSFER.Size = new System.Drawing.Size(147, 21);
            this.cmb_TRANSFER.TabIndex = 0;
            this.cmb_TRANSFER.SelectedValueChanged += new System.EventHandler(this.cmb_TRANSFER_SelectedIndexChanged);
            // 
            // lbl_Transfer
            // 
            this.lbl_Transfer.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular);
            this.lbl_Transfer.Location = new System.Drawing.Point(0, 4);
            this.lbl_Transfer.Name = "lbl_Transfer";
            this.lbl_Transfer.Size = new System.Drawing.Size(191, 20);
            this.lbl_Transfer.Text = "Tento fiskalizim (Offline) për:";
            // 
            // grdDetails
            // 
            this.grdDetails.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.grdDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdDetails.Location = new System.Drawing.Point(0, 0);
            this.grdDetails.Name = "grdDetails";
            this.grdDetails.Size = new System.Drawing.Size(234, 240);
            this.grdDetails.TabIndex = 13;
            this.grdDetails.DoubleClick += new System.EventHandler(this.grdDetails_DoubleClick);
            // 
            // pnlAll
            // 
            this.pnlAll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlAll.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlAll.Controls.Add(this.grdDetails);
            this.pnlAll.Location = new System.Drawing.Point(3, 51);
            this.pnlAll.Name = "pnlAll";
            this.pnlAll.Size = new System.Drawing.Size(234, 240);
            // 
            // frmFiskalizimi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.pnlAll);
            this.Controls.Add(this.cmb_TRANSFER);
            this.Controls.Add(this.lbl_Transfer);
            this.Location = new System.Drawing.Point(0, 0);
            this.Menu = this.mainMenu1;
            this.Name = "frmFiskalizimi";
            this.Text = "Fiskalizimi";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmFiskalizimi_Load);
            this.pnlAll.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menu_EXIT;
        private System.Windows.Forms.ComboBox cmb_TRANSFER;
        private System.Windows.Forms.Label lbl_Transfer;
        private System.Windows.Forms.MenuItem menu_Rifiskalizo;
        public System.Windows.Forms.DataGrid grdDetails;
        private System.Windows.Forms.Panel pnlAll;
    }
}