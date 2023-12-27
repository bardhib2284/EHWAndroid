namespace MobileSales
{
    partial class RegjistroVizite
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
            this.mobileDatabaseDataSet = new MobileSales.MyMobileDataSet();
            this.klientDheLokacionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cmbKlienti = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dTPickerData = new System.Windows.Forms.DateTimePicker();
            this.lblDataPlanifikimit = new System.Windows.Forms.Label();
            this.vizitatTableAdapter1 = new MobileSales.MyMobileDataSetTableAdapters.VizitatTableAdapter();
            this.lblOraPlanifikimit = new System.Windows.Forms.Label();
            this.dTPickerKoha = new System.Windows.Forms.DateTimePicker();
            this.lblRegjistrimi = new System.Windows.Forms.Label();
            this.klientDheLokacionTableAdapter = new MobileSales.MyMobileDataSetTableAdapters.KlientDheLokacionTableAdapter();
            this.txtAdresa = new System.Windows.Forms.TextBox();
            this.dsKlient = new System.Data.DataSet();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menu_Save = new System.Windows.Forms.MenuItem();
            this.menu_Close = new System.Windows.Forms.MenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.mobileDatabaseDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.klientDheLokacionBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsKlient)).BeginInit();
            this.SuspendLayout();
            // 
            // mobileDatabaseDataSet
            // 
            this.mobileDatabaseDataSet.DataSetName = "MobileDatabaseDataSet";
            this.mobileDatabaseDataSet.Prefix = "";
            this.mobileDatabaseDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // klientDheLokacionBindingSource
            // 
            this.klientDheLokacionBindingSource.DataMember = "KlientDheLokacion";
            this.klientDheLokacionBindingSource.DataSource = this.mobileDatabaseDataSet;
            // 
            // cmbKlienti
            // 
            this.cmbKlienti.DisplayMember = "IDKlienti";
            this.cmbKlienti.Location = new System.Drawing.Point(64, 31);
            this.cmbKlienti.Name = "cmbKlienti";
            this.cmbKlienti.Size = new System.Drawing.Size(155, 22);
            this.cmbKlienti.TabIndex = 0;
            this.cmbKlienti.ValueMember = "IDKlienti";
            this.cmbKlienti.SelectedIndexChanged += new System.EventHandler(this.cmbKlienti_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(5, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 20);
            this.label1.Text = "Klienti:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(-1, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 20);
            this.label2.Text = "Adresa:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dTPickerData
            // 
            this.dTPickerData.Enabled = false;
            this.dTPickerData.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dTPickerData.Location = new System.Drawing.Point(65, 88);
            this.dTPickerData.Name = "dTPickerData";
            this.dTPickerData.Size = new System.Drawing.Size(115, 22);
            this.dTPickerData.TabIndex = 4;
            // 
            // lblDataPlanifikimit
            // 
            this.lblDataPlanifikimit.Location = new System.Drawing.Point(4, 90);
            this.lblDataPlanifikimit.Name = "lblDataPlanifikimit";
            this.lblDataPlanifikimit.Size = new System.Drawing.Size(53, 20);
            this.lblDataPlanifikimit.Text = "Data:";
            this.lblDataPlanifikimit.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // vizitatTableAdapter1
            // 
            this.vizitatTableAdapter1.ClearBeforeFill = true;
            // 
            // lblOraPlanifikimit
            // 
            this.lblOraPlanifikimit.Location = new System.Drawing.Point(4, 123);
            this.lblOraPlanifikimit.Name = "lblOraPlanifikimit";
            this.lblOraPlanifikimit.Size = new System.Drawing.Size(53, 20);
            this.lblOraPlanifikimit.Text = "Ora:";
            this.lblOraPlanifikimit.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dTPickerKoha
            // 
            this.dTPickerKoha.Enabled = false;
            this.dTPickerKoha.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dTPickerKoha.Location = new System.Drawing.Point(65, 121);
            this.dTPickerKoha.Name = "dTPickerKoha";
            this.dTPickerKoha.Size = new System.Drawing.Size(115, 22);
            this.dTPickerKoha.TabIndex = 10;
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
            // klientDheLokacionTableAdapter
            // 
            this.klientDheLokacionTableAdapter.ClearBeforeFill = true;
            // 
            // txtAdresa
            // 
            this.txtAdresa.Location = new System.Drawing.Point(65, 61);
            this.txtAdresa.Name = "txtAdresa";
            this.txtAdresa.ReadOnly = true;
            this.txtAdresa.Size = new System.Drawing.Size(154, 21);
            this.txtAdresa.TabIndex = 15;
            // 
            // dsKlient
            // 
            this.dsKlient.DataSetName = "dsKlient";
            this.dsKlient.Namespace = "";
            this.dsKlient.Prefix = "";
            this.dsKlient.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menu_Save);
            this.mainMenu1.MenuItems.Add(this.menu_Close);
            // 
            // menu_Save
            // 
            this.menu_Save.Text = "Regjistro";
            this.menu_Save.Click += new System.EventHandler(this.menu_Save_Click);
            // 
            // menu_Close
            // 
            this.menu_Close.Text = "Dalja";
            this.menu_Close.Click += new System.EventHandler(this.menu_Close_Click);
            // 
            // RegjistroVizite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.txtAdresa);
            this.Controls.Add(this.lblRegjistrimi);
            this.Controls.Add(this.dTPickerKoha);
            this.Controls.Add(this.lblOraPlanifikimit);
            this.Controls.Add(this.lblDataPlanifikimit);
            this.Controls.Add(this.dTPickerData);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbKlienti);
            this.Location = new System.Drawing.Point(0, 0);
            this.Menu = this.mainMenu1;
            this.Name = "RegjistroVizite";
            this.Text = "RegjistrimiVizites";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.RegjistroVizite_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mobileDatabaseDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.klientDheLokacionBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsKlient)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbKlienti;
        private MyMobileDataSet mobileDatabaseDataSet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dTPickerData;
        private System.Windows.Forms.Label lblDataPlanifikimit;
        private MobileSales.MyMobileDataSetTableAdapters.VizitatTableAdapter vizitatTableAdapter1;
        private System.Windows.Forms.Label lblOraPlanifikimit;
        private System.Windows.Forms.DateTimePicker dTPickerKoha;
        private System.Windows.Forms.Label lblRegjistrimi;
        private System.Windows.Forms.BindingSource klientDheLokacionBindingSource;
        private MobileSales.MyMobileDataSetTableAdapters.KlientDheLokacionTableAdapter klientDheLokacionTableAdapter;
        private System.Windows.Forms.TextBox txtAdresa;
        private System.Data.DataSet dsKlient;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menu_Save;
        private System.Windows.Forms.MenuItem menu_Close;
    }
}