namespace MobileSales
{
    partial class RegjistrimiVizites
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegjistrimiVizites));
            this.myMobileDataset = new MobileSales.MyMobileDataSet();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblRegjistrimi = new System.Windows.Forms.Label();
            this.lblKl = new System.Windows.Forms.Label();
            this.lblLok = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.lblStatusi = new System.Windows.Forms.Label();
            this.statusiVizitesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cmbStatusi = new System.Windows.Forms.ComboBox();
            this.statusiVizitesTableAdapter = new MobileSales.MyMobileDataSetTableAdapters.StatusiVizitesTableAdapter();
            this.vizitatTableAdapter1 = new MobileSales.MyMobileDataSetTableAdapters.VizitatTableAdapter();
            this.cmbNewStatus = new System.Windows.Forms.ComboBox();
            this.lblNewStatus = new System.Windows.Forms.Label();
            this.txtKlienti = new System.Windows.Forms.TextBox();
            this.txtLokacioni = new System.Windows.Forms.TextBox();
            this.dsStatuset = new System.Data.DataSet();
            this.cmbStatusiVjeter = new System.Windows.Forms.ComboBox();
            this.dsNewStatus = new System.Data.DataSet();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menu_Save = new System.Windows.Forms.MenuItem();
            this.menu_Close = new System.Windows.Forms.MenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDataset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusiVizitesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsStatuset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsNewStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // myMobileDataset
            // 
            this.myMobileDataset.DataSetName = "MobileDatabaseDataSet";
            this.myMobileDataset.Prefix = "";
            this.myMobileDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(19, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 21);
            this.label1.Text = "Klienti:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(2, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 21);
            this.label2.Text = "Lokacioni:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblRegjistrimi
            // 
            this.lblRegjistrimi.BackColor = System.Drawing.Color.Gainsboro;
            this.lblRegjistrimi.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRegjistrimi.Location = new System.Drawing.Point(0, 0);
            this.lblRegjistrimi.Name = "lblRegjistrimi";
            this.lblRegjistrimi.Size = new System.Drawing.Size(240, 20);
            this.lblRegjistrimi.Text = "Regjistrimi i vizitës";
            this.lblRegjistrimi.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblKl
            // 
            this.lblKl.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblKl.ForeColor = System.Drawing.Color.Red;
            this.lblKl.Location = new System.Drawing.Point(217, 43);
            this.lblKl.Name = "lblKl";
            this.lblKl.Size = new System.Drawing.Size(10, 11);
            this.lblKl.Text = "*";
            this.lblKl.Visible = false;
            // 
            // lblLok
            // 
            this.lblLok.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblLok.ForeColor = System.Drawing.Color.Red;
            this.lblLok.Location = new System.Drawing.Point(217, 71);
            this.lblLok.Name = "lblLok";
            this.lblLok.Size = new System.Drawing.Size(10, 11);
            this.lblLok.Text = "*";
            this.lblLok.Visible = false;
            // 
            // dtpDate
            // 
            this.dtpDate.CustomFormat = "dd-MM-yyyy / h:mm";
            this.dtpDate.Enabled = false;
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDate.Location = new System.Drawing.Point(101, 80);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(115, 22);
            this.dtpDate.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(4, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 22);
            this.label4.Text = "Data dhe koha:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblStatusi
            // 
            this.lblStatusi.Location = new System.Drawing.Point(10, 109);
            this.lblStatusi.Name = "lblStatusi";
            this.lblStatusi.Size = new System.Drawing.Size(84, 22);
            this.lblStatusi.Text = "Statusi aktual:";
            this.lblStatusi.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // statusiVizitesBindingSource
            // 
            this.statusiVizitesBindingSource.DataMember = "StatusiVizites";
            this.statusiVizitesBindingSource.DataSource = this.myMobileDataset;
            // 
            // cmbStatusi
            // 
            this.cmbStatusi.DataSource = this.statusiVizitesBindingSource;
            this.cmbStatusi.DisplayMember = "Gjendja";
            this.cmbStatusi.Location = new System.Drawing.Point(14, 36);
            this.cmbStatusi.Name = "cmbStatusi";
            this.cmbStatusi.Size = new System.Drawing.Size(13, 22);
            this.cmbStatusi.TabIndex = 27;
            this.cmbStatusi.ValueMember = "IDStatusiVizites";
            this.cmbStatusi.Visible = false;
            // 
            // statusiVizitesTableAdapter
            // 
            this.statusiVizitesTableAdapter.ClearBeforeFill = true;
            // 
            // vizitatTableAdapter1
            // 
            this.vizitatTableAdapter1.ClearBeforeFill = true;
            // 
            // cmbNewStatus
            // 
            this.cmbNewStatus.Items.Add("");
            this.cmbNewStatus.Items.Add("Klienti mungon ");
            this.cmbNewStatus.Items.Add("Klienti zanun");
            this.cmbNewStatus.Items.Add("Shtyhet per me vone");
            this.cmbNewStatus.Items.Add("AddHoc");
            this.cmbNewStatus.Location = new System.Drawing.Point(101, 137);
            this.cmbNewStatus.Name = "cmbNewStatus";
            this.cmbNewStatus.Size = new System.Drawing.Size(115, 22);
            this.cmbNewStatus.TabIndex = 35;
            // 
            // lblNewStatus
            // 
            this.lblNewStatus.Location = new System.Drawing.Point(20, 137);
            this.lblNewStatus.Name = "lblNewStatus";
            this.lblNewStatus.Size = new System.Drawing.Size(74, 22);
            this.lblNewStatus.Text = "Statusi i ri:";
            this.lblNewStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtKlienti
            // 
            this.txtKlienti.Location = new System.Drawing.Point(101, 25);
            this.txtKlienti.Name = "txtKlienti";
            this.txtKlienti.Size = new System.Drawing.Size(115, 21);
            this.txtKlienti.TabIndex = 43;
            // 
            // txtLokacioni
            // 
            this.txtLokacioni.Location = new System.Drawing.Point(101, 53);
            this.txtLokacioni.Name = "txtLokacioni";
            this.txtLokacioni.Size = new System.Drawing.Size(115, 21);
            this.txtLokacioni.TabIndex = 44;
            // 
            // dsStatuset
            // 
            this.dsStatuset.DataSetName = "NewDataSet";
            this.dsStatuset.Namespace = "";
            this.dsStatuset.Prefix = "";
            this.dsStatuset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // cmbStatusiVjeter
            // 
            this.cmbStatusiVjeter.Enabled = false;
            this.cmbStatusiVjeter.Location = new System.Drawing.Point(101, 109);
            this.cmbStatusiVjeter.Name = "cmbStatusiVjeter";
            this.cmbStatusiVjeter.Size = new System.Drawing.Size(115, 22);
            this.cmbStatusiVjeter.TabIndex = 53;
            // 
            // dsNewStatus
            // 
            this.dsNewStatus.DataSetName = "NewDataSet";
            this.dsNewStatus.Namespace = "";
            this.dsNewStatus.Prefix = "";
            this.dsNewStatus.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menu_Save);
            this.mainMenu1.MenuItems.Add(this.menu_Close);
            // 
            // menu_Save
            // 
            this.menu_Save.Text = "Ndrysho";
            this.menu_Save.Click += new System.EventHandler(this.menu_Save_Click);
            // 
            // menu_Close
            // 
            this.menu_Close.Text = "Dalja";
            this.menu_Close.Click += new System.EventHandler(this.menu_Close_Click);
            // 
            // RegjistrimiVizites
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.cmbStatusiVjeter);
            this.Controls.Add(this.txtLokacioni);
            this.Controls.Add(this.txtKlienti);
            this.Controls.Add(this.lblNewStatus);
            this.Controls.Add(this.cmbNewStatus);
            this.Controls.Add(this.cmbStatusi);
            this.Controls.Add(this.lblStatusi);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.lblLok);
            this.Controls.Add(this.lblKl);
            this.Controls.Add(this.lblRegjistrimi);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ForeColor = System.Drawing.Color.Red;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Menu = this.mainMenu1;
            this.Name = "RegjistrimiVizites";
            this.Text = "RegjistrimiVizites";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.RegjistrimiVizites_Load);
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDataset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusiVizitesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsStatuset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsNewStatus)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private MyMobileDataSet myMobileDataset;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblRegjistrimi;
        private System.Windows.Forms.Label lblKl;
        private System.Windows.Forms.Label lblLok;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblStatusi;
        private System.Windows.Forms.ComboBox cmbStatusi;
        private System.Windows.Forms.BindingSource statusiVizitesBindingSource;
        private MobileSales.MyMobileDataSetTableAdapters.StatusiVizitesTableAdapter statusiVizitesTableAdapter;
        private MobileSales.MyMobileDataSetTableAdapters.VizitatTableAdapter vizitatTableAdapter1;
        private System.Windows.Forms.ComboBox cmbNewStatus;
        private System.Windows.Forms.Label lblNewStatus;
        private System.Windows.Forms.TextBox txtKlienti;
        private System.Windows.Forms.TextBox txtLokacioni;
        private System.Data.DataSet dsStatuset;
        private System.Windows.Forms.ComboBox cmbStatusiVjeter;
        private System.Data.DataSet dsNewStatus;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menu_Save;
        private System.Windows.Forms.MenuItem menu_Close;
    }
}