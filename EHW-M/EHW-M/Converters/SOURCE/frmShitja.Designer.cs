namespace MobileSales
{
    partial class frmShitja
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
            System.Windows.Forms.Label dataPorosiseLabel;
            System.Windows.Forms.Label dataPerLiferimLabel;
            System.Windows.Forms.Label emriLabel;
            System.Windows.Forms.Label lblNrPorosise;
            System.Windows.Forms.DataGridTextBoxColumn cmimiAktualDataGridColumnStyleDataGridTextBoxColumn;
            System.Windows.Forms.DataGridTextBoxColumn sasiaPorositurDataGridColumnStyleDataGridTextBoxColumn;
            System.Windows.Forms.DataGridTextBoxColumn emriDataGridColumnStyleDataGridTextBoxColumn;
            System.Windows.Forms.Label label1;
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuAnulo = new System.Windows.Forms.MenuItem();
            this.menuRegjistro = new System.Windows.Forms.MenuItem();
            this.menuDalja = new System.Windows.Forms.MenuItem();
            this.dtpdataPorosise = new System.Windows.Forms.DateTimePicker();
            this.porositeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.myMobileDataSet = new MobileSales.MyMobileDataSet();
            this.dtpDateLiferim = new System.Windows.Forms.DateTimePicker();
            this.lblLine = new System.Windows.Forms.Label();
            this.btnArtikujt = new System.Windows.Forms.Button();
            this.btnShto = new System.Windows.Forms.Button();
            this.txtArtikulli = new System.Windows.Forms.TextBox();
            this.porosiaArtBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblStatusiPorosise = new System.Windows.Forms.Label();
            this.lblIDPorosia = new System.Windows.Forms.Label();
            this.lblIDVizita = new System.Windows.Forms.Label();
            this.lblTotali = new System.Windows.Forms.Label();
            this.lblNrPorosis = new System.Windows.Forms.Label();
            this.txtRabati = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCop = new System.Windows.Forms.TextBox();
            this.txtPako = new System.Windows.Forms.TextBox();
            this.lblCPako = new System.Windows.Forms.Label();
            this.porosiaArtTableStyleDataGridTableStyle = new System.Windows.Forms.DataGridTableStyle();
            this.dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn2 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn3 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.porosiaArtDataGrid = new System.Windows.Forms.DataGrid();
            this.lblPako = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblData = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.porosiaArtTableAdapter = new MobileSales.MyMobileDataSetTableAdapters.PorosiaArtTableAdapter();
            this.porositeTableAdapter = new MobileSales.MyMobileDataSetTableAdapters.PorositeTableAdapter();
            this.numerimInternTableAdapter = new MobileSales.MyMobileDataSetTableAdapters.NumerimInternTableAdapter();
            this.stoqetTableAdapter1 = new MobileSales.MyMobileDataSetTableAdapters.StoqetTableAdapter();
            this.dsNumriFaturave = new System.Data.DataSet();
            this.myMobileDataSet1 = new MobileSales.MyMobileDataSet();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtSeri = new System.Windows.Forms.TextBox();
            this.textBoxKthimi = new System.Windows.Forms.TextBox();
            this.lblFaturaKthim = new System.Windows.Forms.Label();
            dataPorosiseLabel = new System.Windows.Forms.Label();
            dataPerLiferimLabel = new System.Windows.Forms.Label();
            emriLabel = new System.Windows.Forms.Label();
            lblNrPorosise = new System.Windows.Forms.Label();
            cmimiAktualDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            sasiaPorositurDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            emriDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.porositeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.porosiaArtBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsNumriFaturave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDataSet1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataPorosiseLabel
            // 
            dataPorosiseLabel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            dataPorosiseLabel.Location = new System.Drawing.Point(1, 2);
            dataPorosiseLabel.Name = "dataPorosiseLabel";
            dataPorosiseLabel.Size = new System.Drawing.Size(35, 11);
            dataPorosiseLabel.Text = "Data";
            // 
            // dataPerLiferimLabel
            // 
            dataPerLiferimLabel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            dataPerLiferimLabel.Location = new System.Drawing.Point(113, 9);
            dataPerLiferimLabel.Name = "dataPerLiferimLabel";
            dataPerLiferimLabel.Size = new System.Drawing.Size(5, 21);
            dataPerLiferimLabel.Text = "Data Liferimit";
            dataPerLiferimLabel.Visible = false;
            // 
            // emriLabel
            // 
            emriLabel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            emriLabel.Location = new System.Drawing.Point(1, 42);
            emriLabel.Name = "emriLabel";
            emriLabel.Size = new System.Drawing.Size(42, 12);
            emriLabel.Text = "Artikulli";
            // 
            // lblNrPorosise
            // 
            lblNrPorosise.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            lblNrPorosise.Location = new System.Drawing.Point(158, 1);
            lblNrPorosise.Name = "lblNrPorosise";
            lblNrPorosise.Size = new System.Drawing.Size(82, 12);
            lblNrPorosise.Text = "Nr.Faturës";
            lblNrPorosise.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cmimiAktualDataGridColumnStyleDataGridTextBoxColumn
            // 
            cmimiAktualDataGridColumnStyleDataGridTextBoxColumn.Format = "0.00";
            cmimiAktualDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            cmimiAktualDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Çmimi";
            cmimiAktualDataGridColumnStyleDataGridTextBoxColumn.MappingName = "CmimiAktual";
            cmimiAktualDataGridColumnStyleDataGridTextBoxColumn.Width = 45;
            // 
            // sasiaPorositurDataGridColumnStyleDataGridTextBoxColumn
            // 
            sasiaPorositurDataGridColumnStyleDataGridTextBoxColumn.Format = "0.000";
            sasiaPorositurDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            sasiaPorositurDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Sasia";
            sasiaPorositurDataGridColumnStyleDataGridTextBoxColumn.MappingName = "SasiaPorositur";
            sasiaPorositurDataGridColumnStyleDataGridTextBoxColumn.Width = 38;
            // 
            // emriDataGridColumnStyleDataGridTextBoxColumn
            // 
            emriDataGridColumnStyleDataGridTextBoxColumn.Format = "";
            emriDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            emriDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Artikulli";
            emriDataGridColumnStyleDataGridTextBoxColumn.MappingName = "Emri";
            emriDataGridColumnStyleDataGridTextBoxColumn.Width = 110;
            // 
            // label1
            // 
            label1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            label1.Location = new System.Drawing.Point(1, 61);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(42, 12);
            label1.Text = "Seri";
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuAnulo);
            this.mainMenu1.MenuItems.Add(this.menuRegjistro);
            this.mainMenu1.MenuItems.Add(this.menuDalja);
            // 
            // menuAnulo
            // 
            this.menuAnulo.Text = "AnuloArt";
            this.menuAnulo.Click += new System.EventHandler(this.menuAnulo_Click);
            // 
            // menuRegjistro
            // 
            this.menuRegjistro.Text = "Regjistro";
            this.menuRegjistro.Click += new System.EventHandler(this.menuRegjistro_Click);
            // 
            // menuDalja
            // 
            this.menuDalja.Text = "Dalja";
            this.menuDalja.Click += new System.EventHandler(this.menuDalja_Click);
            // 
            // dtpdataPorosise
            // 
            this.dtpdataPorosise.CustomFormat = "dd-MM-yyyy";
            this.dtpdataPorosise.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.porositeBindingSource, "DataPorosise", true));
            this.dtpdataPorosise.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.dtpdataPorosise.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpdataPorosise.Location = new System.Drawing.Point(3, 125);
            this.dtpdataPorosise.MinDate = new System.DateTime(2008, 9, 19, 0, 0, 0, 0);
            this.dtpdataPorosise.Name = "dtpdataPorosise";
            this.dtpdataPorosise.Size = new System.Drawing.Size(10, 20);
            this.dtpdataPorosise.TabIndex = 5;
            this.dtpdataPorosise.Visible = false;
            // 
            // porositeBindingSource
            // 
            this.porositeBindingSource.DataMember = "Porosite";
            this.porositeBindingSource.DataSource = this.myMobileDataSet;
            // 
            // myMobileDataSet
            // 
            this.myMobileDataSet.DataSetName = "MobileDatabaseDataSet";
            this.myMobileDataSet.Prefix = "";
            this.myMobileDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dtpDateLiferim
            // 
            this.dtpDateLiferim.CustomFormat = "dd-MM-yy";
            this.dtpDateLiferim.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.porositeBindingSource, "DataPerLiferim", true));
            this.dtpDateLiferim.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.dtpDateLiferim.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateLiferim.Location = new System.Drawing.Point(96, 10);
            this.dtpDateLiferim.MinDate = new System.DateTime(2008, 9, 22, 0, 0, 0, 0);
            this.dtpDateLiferim.Name = "dtpDateLiferim";
            this.dtpDateLiferim.Size = new System.Drawing.Size(4, 20);
            this.dtpDateLiferim.TabIndex = 9;
            this.dtpDateLiferim.Visible = false;
            // 
            // lblLine
            // 
            this.lblLine.BackColor = System.Drawing.Color.Black;
            this.lblLine.ForeColor = System.Drawing.SystemColors.Desktop;
            this.lblLine.Location = new System.Drawing.Point(0, 36);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(240, 2);
            // 
            // btnArtikujt
            // 
            this.btnArtikujt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnArtikujt.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.btnArtikujt.Location = new System.Drawing.Point(141, 59);
            this.btnArtikujt.Name = "btnArtikujt";
            this.btnArtikujt.Size = new System.Drawing.Size(19, 20);
            this.btnArtikujt.TabIndex = 11;
            this.btnArtikujt.Text = "...";
            this.btnArtikujt.Click += new System.EventHandler(this.btnArtikujt_Click);
            // 
            // btnShto
            // 
            this.btnShto.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnShto.Enabled = false;
            this.btnShto.Location = new System.Drawing.Point(199, 46);
            this.btnShto.Name = "btnShto";
            this.btnShto.Size = new System.Drawing.Size(41, 32);
            this.btnShto.TabIndex = 12;
            this.btnShto.Text = "Shto";
            this.btnShto.Click += new System.EventHandler(this.btnShto_Click);
            // 
            // txtArtikulli
            // 
            this.txtArtikulli.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.porosiaArtBindingSource, "Emri", true));
            this.txtArtikulli.Enabled = false;
            this.txtArtikulli.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.txtArtikulli.Location = new System.Drawing.Point(44, 39);
            this.txtArtikulli.Name = "txtArtikulli";
            this.txtArtikulli.Size = new System.Drawing.Size(116, 19);
            this.txtArtikulli.TabIndex = 15;
            // 
            // porosiaArtBindingSource
            // 
            this.porosiaArtBindingSource.DataMember = "PorosiaArt";
            this.porosiaArtBindingSource.DataSource = this.myMobileDataSet;
            this.porosiaArtBindingSource.CurrentChanged += new System.EventHandler(this.porosiaArtBindingSource_CurrentChanged);
            this.porosiaArtBindingSource.PositionChanged += new System.EventHandler(this.porosiaArtBindingSource_PositionChanged);
            this.porosiaArtBindingSource.CurrentItemChanged += new System.EventHandler(this.porosiaArtBindingSource_CurrentItemChanged);
            // 
            // lblStatusiPorosise
            // 
            this.lblStatusiPorosise.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblStatusiPorosise.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.porositeBindingSource, "StatusiPorosise", true));
            this.lblStatusiPorosise.Location = new System.Drawing.Point(89, 10);
            this.lblStatusiPorosise.Name = "lblStatusiPorosise";
            this.lblStatusiPorosise.Size = new System.Drawing.Size(4, 20);
            this.lblStatusiPorosise.Visible = false;
            // 
            // lblIDPorosia
            // 
            this.lblIDPorosia.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblIDPorosia.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.porositeBindingSource, "IDPorosia", true));
            this.lblIDPorosia.Location = new System.Drawing.Point(108, 10);
            this.lblIDPorosia.Name = "lblIDPorosia";
            this.lblIDPorosia.Size = new System.Drawing.Size(3, 20);
            this.lblIDPorosia.Visible = false;
            // 
            // lblIDVizita
            // 
            this.lblIDVizita.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblIDVizita.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.porositeBindingSource, "IDVizita", true));
            this.lblIDVizita.Location = new System.Drawing.Point(100, 10);
            this.lblIDVizita.Name = "lblIDVizita";
            this.lblIDVizita.Size = new System.Drawing.Size(3, 20);
            this.lblIDVizita.Visible = false;
            // 
            // lblTotali
            // 
            this.lblTotali.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblTotali.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotali.ForeColor = System.Drawing.Color.Black;
            this.lblTotali.Location = new System.Drawing.Point(0, 0);
            this.lblTotali.Name = "lblTotali";
            this.lblTotali.Size = new System.Drawing.Size(240, 21);
            this.lblTotali.Text = "0.00";
            this.lblTotali.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblNrPorosis
            // 
            this.lblNrPorosis.BackColor = System.Drawing.Color.Gainsboro;
            this.lblNrPorosis.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblNrPorosis.Location = new System.Drawing.Point(152, 15);
            this.lblNrPorosis.Name = "lblNrPorosis";
            this.lblNrPorosis.Size = new System.Drawing.Size(88, 13);
            this.lblNrPorosis.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtRabati
            // 
            this.txtRabati.BackColor = System.Drawing.Color.White;
            this.txtRabati.Enabled = false;
            this.txtRabati.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.txtRabati.Location = new System.Drawing.Point(96, 128);
            this.txtRabati.Name = "txtRabati";
            this.txtRabati.ReadOnly = true;
            this.txtRabati.Size = new System.Drawing.Size(23, 19);
            this.txtRabati.TabIndex = 41;
            this.txtRabati.Visible = false;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.label2.Location = new System.Drawing.Point(162, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 11);
            this.label2.Text = "Porosi";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtCop
            // 
            this.txtCop.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.txtCop.BackColor = System.Drawing.Color.White;
            this.txtCop.Enabled = false;
            this.txtCop.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.txtCop.Location = new System.Drawing.Point(-287, 92);
            this.txtCop.Name = "txtCop";
            this.txtCop.ReadOnly = true;
            this.txtCop.Size = new System.Drawing.Size(3, 19);
            this.txtCop.TabIndex = 75;
            this.txtCop.Visible = false;
            // 
            // txtPako
            // 
            this.txtPako.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.porosiaArtBindingSource, "SasiaPorositur", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged, "0"));
            this.txtPako.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.txtPako.Location = new System.Drawing.Point(163, 59);
            this.txtPako.Name = "txtPako";
            this.txtPako.Size = new System.Drawing.Size(32, 19);
            this.txtPako.TabIndex = 76;
            this.txtPako.TextChanged += new System.EventHandler(this.txtPako_TextChanged);
            this.txtPako.GotFocus += new System.EventHandler(this.txtPako_GotFocus);
            this.txtPako.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPako_KeyDown);
            this.txtPako.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPako_KeyUp);
            this.txtPako.LostFocus += new System.EventHandler(this.txtPako_LostFocus);
            // 
            // lblCPako
            // 
            this.lblCPako.BackColor = System.Drawing.Color.Gainsboro;
            this.lblCPako.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.porosiaArtBindingSource, "SasiaPako", true));
            this.lblCPako.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.lblCPako.ForeColor = System.Drawing.Color.Black;
            this.lblCPako.Location = new System.Drawing.Point(138, 130);
            this.lblCPako.Name = "lblCPako";
            this.lblCPako.Size = new System.Drawing.Size(22, 13);
            this.lblCPako.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // porosiaArtTableStyleDataGridTableStyle
            // 
            this.porosiaArtTableStyleDataGridTableStyle.GridColumnStyles.Add(emriDataGridColumnStyleDataGridTextBoxColumn);
            this.porosiaArtTableStyleDataGridTableStyle.GridColumnStyles.Add(sasiaPorositurDataGridColumnStyleDataGridTextBoxColumn);
            this.porosiaArtTableStyleDataGridTableStyle.GridColumnStyles.Add(cmimiAktualDataGridColumnStyleDataGridTextBoxColumn);
            this.porosiaArtTableStyleDataGridTableStyle.GridColumnStyles.Add(this.dataGridTextBoxColumn1);
            this.porosiaArtTableStyleDataGridTableStyle.GridColumnStyles.Add(this.dataGridTextBoxColumn2);
            this.porosiaArtTableStyleDataGridTableStyle.GridColumnStyles.Add(this.dataGridTextBoxColumn3);
            this.porosiaArtTableStyleDataGridTableStyle.MappingName = "PorosiaArt";
            // 
            // dataGridTextBoxColumn1
            // 
            this.dataGridTextBoxColumn1.Format = "0.00";
            this.dataGridTextBoxColumn1.FormatInfo = null;
            this.dataGridTextBoxColumn1.HeaderText = "Totali";
            this.dataGridTextBoxColumn1.MappingName = "ShumaTotale";
            this.dataGridTextBoxColumn1.Width = 48;
            // 
            // dataGridTextBoxColumn2
            // 
            this.dataGridTextBoxColumn2.Format = "";
            this.dataGridTextBoxColumn2.FormatInfo = null;
            this.dataGridTextBoxColumn2.MappingName = "IDArtikulli";
            this.dataGridTextBoxColumn2.Width = 0;
            // 
            // dataGridTextBoxColumn3
            // 
            this.dataGridTextBoxColumn3.Format = "";
            this.dataGridTextBoxColumn3.FormatInfo = null;
            this.dataGridTextBoxColumn3.HeaderText = "Seri";
            this.dataGridTextBoxColumn3.MappingName = "Seri";
            // 
            // porosiaArtDataGrid
            // 
            this.porosiaArtDataGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.porosiaArtDataGrid.DataSource = this.porosiaArtBindingSource;
            this.porosiaArtDataGrid.Location = new System.Drawing.Point(0, 106);
            this.porosiaArtDataGrid.Name = "porosiaArtDataGrid";
            this.porosiaArtDataGrid.RowHeadersVisible = false;
            this.porosiaArtDataGrid.Size = new System.Drawing.Size(240, 160);
            this.porosiaArtDataGrid.TabIndex = 0;
            this.porosiaArtDataGrid.TableStyles.Add(this.porosiaArtTableStyleDataGridTableStyle);
            this.porosiaArtDataGrid.MouseUp += new System.Windows.Forms.MouseEventHandler(this.porosiaArtDataGrid_MouseUp);
            this.porosiaArtDataGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.porosiaArtDataGrid_KeyDown);
            // 
            // lblPako
            // 
            this.lblPako.BackColor = System.Drawing.Color.Gainsboro;
            this.lblPako.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.lblPako.ForeColor = System.Drawing.Color.Black;
            this.lblPako.Location = new System.Drawing.Point(149, 143);
            this.lblPako.Name = "lblPako";
            this.lblPako.Size = new System.Drawing.Size(22, 13);
            this.lblPako.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblPako.Visible = false;
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Gainsboro;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(85, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(61, 13);
            this.lblTitle.Text = "Shitja";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Black;
            this.label6.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label6.Location = new System.Drawing.Point(81, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 1);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Black;
            this.label7.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label7.Location = new System.Drawing.Point(82, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 1);
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Black;
            this.label8.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label8.Location = new System.Drawing.Point(149, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(1, 19);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Black;
            this.label9.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label9.Location = new System.Drawing.Point(81, 11);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(1, 20);
            // 
            // lblData
            // 
            this.lblData.BackColor = System.Drawing.Color.Gainsboro;
            this.lblData.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblData.Location = new System.Drawing.Point(1, 15);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(75, 13);
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Black;
            this.label10.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label10.Location = new System.Drawing.Point(0, 30);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 2);
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Black;
            this.label11.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label11.Location = new System.Drawing.Point(152, 30);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(88, 2);
            // 
            // porosiaArtTableAdapter
            // 
            this.porosiaArtTableAdapter.ClearBeforeFill = true;
            // 
            // porositeTableAdapter
            // 
            this.porositeTableAdapter.ClearBeforeFill = true;
            // 
            // numerimInternTableAdapter
            // 
            this.numerimInternTableAdapter.ClearBeforeFill = true;
            // 
            // stoqetTableAdapter1
            // 
            this.stoqetTableAdapter1.ClearBeforeFill = true;
            // 
            // dsNumriFaturave
            // 
            this.dsNumriFaturave.DataSetName = "NewDataSet";
            this.dsNumriFaturave.Namespace = "";
            this.dsNumriFaturave.Prefix = "";
            this.dsNumriFaturave.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // myMobileDataSet1
            // 
            this.myMobileDataSet1.DataSetName = "MyMobileDataSet";
            this.myMobileDataSet1.Prefix = "";
            this.myMobileDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblTotali);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 269);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(240, 21);
            // 
            // txtSeri
            // 
            this.txtSeri.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.txtSeri.Location = new System.Drawing.Point(44, 59);
            this.txtSeri.Name = "txtSeri";
            this.txtSeri.Size = new System.Drawing.Size(94, 19);
            this.txtSeri.TabIndex = 110;
            // 
            // textBoxKthimi
            // 
            this.textBoxKthimi.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.textBoxKthimi.Location = new System.Drawing.Point(96, 81);
            this.textBoxKthimi.Name = "textBoxKthimi";
            this.textBoxKthimi.Size = new System.Drawing.Size(83, 19);
            this.textBoxKthimi.TabIndex = 134;
            this.textBoxKthimi.Visible = false;
            // 
            // lblFaturaKthim
            // 
            this.lblFaturaKthim.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblFaturaKthim.Location = new System.Drawing.Point(3, 85);
            this.lblFaturaKthim.Name = "lblFaturaKthim";
            this.lblFaturaKthim.Size = new System.Drawing.Size(90, 20);
            this.lblFaturaKthim.Text = "Nr. Fat. Kthim";
            // 
            // frmShitja
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(240, 290);
            this.ControlBox = false;
            this.Controls.Add(this.lblFaturaKthim);
            this.Controls.Add(this.textBoxKthimi);
            this.Controls.Add(this.txtSeri);
            this.Controls.Add(label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.porosiaArtDataGrid);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.lblData);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblPako);
            this.Controls.Add(this.lblCPako);
            this.Controls.Add(this.txtPako);
            this.Controls.Add(this.txtCop);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtRabati);
            this.Controls.Add(lblNrPorosise);
            this.Controls.Add(this.lblNrPorosis);
            this.Controls.Add(this.lblIDVizita);
            this.Controls.Add(this.lblIDPorosia);
            this.Controls.Add(this.lblStatusiPorosise);
            this.Controls.Add(emriLabel);
            this.Controls.Add(this.txtArtikulli);
            this.Controls.Add(this.btnShto);
            this.Controls.Add(this.btnArtikujt);
            this.Controls.Add(this.lblLine);
            this.Controls.Add(dataPerLiferimLabel);
            this.Controls.Add(this.dtpDateLiferim);
            this.Controls.Add(dataPorosiseLabel);
            this.Controls.Add(this.dtpdataPorosise);
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "frmShitja";
            this.Text = "PorosiaDetalet";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmShitja_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Shitja_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.porositeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.porosiaArtBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsNumriFaturave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDataSet1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button btnArtikujt;
        public System.Windows.Forms.TextBox txtArtikulli;
        public System.Windows.Forms.TextBox txtRabati;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox txtPako;
        public System.Windows.Forms.Label lblTitle;
        public System.Windows.Forms.TextBox txtCop;
        public System.Windows.Forms.MainMenu mainMenu1;
        public MyMobileDataSet myMobileDataSet;
        public System.Windows.Forms.MenuItem menuAnulo;
        public System.Windows.Forms.MenuItem menuRegjistro;
        public System.Windows.Forms.BindingSource porosiaArtBindingSource;
        public MobileSales.MyMobileDataSetTableAdapters.PorosiaArtTableAdapter porosiaArtTableAdapter;
        public System.Windows.Forms.BindingSource porositeBindingSource;
        public MobileSales.MyMobileDataSetTableAdapters.PorositeTableAdapter porositeTableAdapter;
        public System.Windows.Forms.DateTimePicker dtpdataPorosise;
        public System.Windows.Forms.DateTimePicker dtpDateLiferim;
        public System.Windows.Forms.Label lblLine;
        public System.Windows.Forms.Button btnShto;
        public System.Windows.Forms.Label lblStatusiPorosise;
        public System.Windows.Forms.Label lblIDPorosia;
        public System.Windows.Forms.Label lblIDVizita;
        public System.Windows.Forms.Label lblTotali;
        public System.Windows.Forms.Label lblNrPorosis;
        public System.Windows.Forms.MenuItem menuDalja;
        public System.Windows.Forms.Label lblCPako;
        public System.Windows.Forms.DataGridTableStyle porosiaArtTableStyleDataGridTableStyle;
        public System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn1;
        public System.Windows.Forms.DataGrid porosiaArtDataGrid;
        public System.Windows.Forms.Label lblPako;
        public System.Windows.Forms.Label label6;
        public System.Windows.Forms.Label label7;
        public System.Windows.Forms.Label label8;
        public System.Windows.Forms.Label label9;
        public MobileSales.MyMobileDataSetTableAdapters.NumerimInternTableAdapter numerimInternTableAdapter;
        public System.Windows.Forms.Label lblData;
        public System.Windows.Forms.Label label10;
        public System.Windows.Forms.Label label11;
        public MobileSales.MyMobileDataSetTableAdapters.StoqetTableAdapter stoqetTableAdapter1;
        public System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn2;
        private System.Data.DataSet dsNumriFaturave;
       // private MobileSales.MyMobileDataSetTableAdapters.Malli_MbeturTableAdapter malli_MbeturTableAdapter1;
        private MyMobileDataSet myMobileDataSet1;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.TextBox txtSeri;
        public System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn3;
        public System.Windows.Forms.TextBox textBoxKthimi;
        private System.Windows.Forms.Label lblFaturaKthim;

    }
}