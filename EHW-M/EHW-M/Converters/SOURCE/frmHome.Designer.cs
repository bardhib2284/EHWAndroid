namespace MobileSales
{
    partial class frmHome
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHome));
            this.txtPerdoruesi = new System.Windows.Forms.TextBox();
            this.txtFjalekalimi = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblHVersion = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.dsStoqet = new System.Data.DataSet();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menu_LogIn = new System.Windows.Forms.MenuItem();
            this.menu_Close = new System.Windows.Forms.MenuItem();
            this.myMobileDatabase = new MobileSales.MyMobileDataSet();
            this.konfigurimiTableAdapter = new MobileSales.MyMobileDataSetTableAdapters.KonfigurimiTableAdapter();
            this.agjentetTableAdapter = new MobileSales.MyMobileDataSetTableAdapters.AgjendetTableAdapter();
            this.log_SyncErrorsTableAdapter = new MobileSales.MyMobileDataSetTableAdapters.Log_SyncErrorsTableAdapter();
            this.konfigurimiTableAdapter1 = new MobileSales.MyMobileDataSetTableAdapters.KonfigurimiTableAdapter();
            this.myMobileDataSet1 = new MobileSales.MyMobileDataSet();
            this.agjendetTableAdapter1 = new MobileSales.MyMobileDataSetTableAdapters.AgjendetTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dsStoqet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDatabase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDataSet1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtPerdoruesi
            // 
            this.txtPerdoruesi.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.txtPerdoruesi.Location = new System.Drawing.Point(88, 185);
            this.txtPerdoruesi.Name = "txtPerdoruesi";
            this.txtPerdoruesi.Size = new System.Drawing.Size(116, 19);
            this.txtPerdoruesi.TabIndex = 0;
            this.txtPerdoruesi.GotFocus += new System.EventHandler(this.txtPerdoruesi_GotFocus);
            this.txtPerdoruesi.KeyDown += new System.Windows.Forms.KeyEventHandler(this.perdoruesi_KeyDown);
            this.txtPerdoruesi.LostFocus += new System.EventHandler(this.txtPerdoruesi_LostFocus);
            // 
            // txtFjalekalimi
            // 
            this.txtFjalekalimi.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.txtFjalekalimi.Location = new System.Drawing.Point(88, 209);
            this.txtFjalekalimi.Name = "txtFjalekalimi";
            this.txtFjalekalimi.PasswordChar = '*';
            this.txtFjalekalimi.Size = new System.Drawing.Size(116, 19);
            this.txtFjalekalimi.TabIndex = 1;
            this.txtFjalekalimi.GotFocus += new System.EventHandler(this.txtFjalekalimi_GotFocus);
            this.txtFjalekalimi.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fjalekalimi_KeyDown);
            this.txtFjalekalimi.LostFocus += new System.EventHandler(this.txtFjalekalimi_LostFocus);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.label1.Location = new System.Drawing.Point(27, 188);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 17);
            this.label1.Text = "Përdoruesi";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.label2.Location = new System.Drawing.Point(27, 212);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 17);
            this.label2.Text = "Fjalëkalimi";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblHVersion
            // 
            this.lblHVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblHVersion.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblHVersion.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblHVersion.Location = new System.Drawing.Point(0, 280);
            this.lblHVersion.Name = "lblHVersion";
            this.lblHVersion.Size = new System.Drawing.Size(240, 14);
            this.lblHVersion.Text = "Versioni";
            this.lblHVersion.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(34, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(164, 159);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // dsStoqet
            // 
            this.dsStoqet.DataSetName = "dsStoqet";
            this.dsStoqet.Namespace = "";
            this.dsStoqet.Prefix = "";
            this.dsStoqet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menu_LogIn);
            this.mainMenu1.MenuItems.Add(this.menu_Close);
            // 
            // menu_LogIn
            // 
            this.menu_LogIn.Text = "Kyçu";
            this.menu_LogIn.Click += new System.EventHandler(this.menu_LogIn_Click);
            // 
            // menu_Close
            // 
            this.menu_Close.Text = "Dalja";
            this.menu_Close.Click += new System.EventHandler(this.menu_Close_Click);
            // 
            // myMobileDatabase
            // 
            this.myMobileDatabase.DataSetName = "myMobileDataSet";
            this.myMobileDatabase.Prefix = "";
            this.myMobileDatabase.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // konfigurimiTableAdapter
            // 
            this.konfigurimiTableAdapter.ClearBeforeFill = true;
            // 
            // agjentetTableAdapter
            // 
            this.agjentetTableAdapter.ClearBeforeFill = true;
            // 
            // log_SyncErrorsTableAdapter
            // 
            this.log_SyncErrorsTableAdapter.ClearBeforeFill = true;
            // 
            // konfigurimiTableAdapter1
            // 
            this.konfigurimiTableAdapter1.ClearBeforeFill = true;
            // 
            // myMobileDataSet1
            // 
            this.myMobileDataSet1.DataSetName = "MyMobileDataSet";
            this.myMobileDataSet1.Prefix = "";
            this.myMobileDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // agjendetTableAdapter1
            // 
            this.agjendetTableAdapter1.ClearBeforeFill = true;
            // 
            // frmHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblHVersion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFjalekalimi);
            this.Controls.Add(this.txtPerdoruesi);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "frmHome";
            this.Text = " ";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmHome_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dsStoqet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDatabase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDataSet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private MyMobileDataSet myMobileDatabase; 
        private System.Windows.Forms.TextBox txtPerdoruesi;
        private System.Windows.Forms.TextBox txtFjalekalimi;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private MobileSales.MyMobileDataSetTableAdapters.KonfigurimiTableAdapter konfigurimiTableAdapter;
        private MobileSales.MyMobileDataSetTableAdapters.AgjendetTableAdapter agjentetTableAdapter;
        private MobileSales.MyMobileDataSetTableAdapters.Log_SyncErrorsTableAdapter log_SyncErrorsTableAdapter;
        private MobileSales.MyMobileDataSetTableAdapters.KonfigurimiTableAdapter konfigurimiTableAdapter1;
        private MyMobileDataSet myMobileDataSet1;
        private MobileSales.MyMobileDataSetTableAdapters.AgjendetTableAdapter agjendetTableAdapter1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Data.DataSet dsStoqet;
        private System.Windows.Forms.Label lblHVersion;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menu_LogIn;
        private System.Windows.Forms.MenuItem menu_Close;
    
    }
}

