namespace MobileSales
{
    partial class frmLiferimi
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
            System.Windows.Forms.Label dataLiferuarLabel;
            System.Windows.Forms.Label titulliLiferimitLabel;
            System.Windows.Forms.DataGridTextBoxColumn cmimiDataGridColumnStyleDataGridTextBoxColumn;
            System.Windows.Forms.DataGridTextBoxColumn sasiaLiferuarDataGridColumnStyleDataGridTextBoxColumn;
            System.Windows.Forms.DataGridTextBoxColumn artEmriDataGridColumnStyleDataGridTextBoxColumn;
            this.iDLiferimiTextBox = new System.Windows.Forms.TextBox();
            this.liferimiBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.myMobileDataSet = new MobileSales.MyMobileDataSet();
            this.dataLiferimitTextBox = new System.Windows.Forms.TextBox();
            this.iDPorosiaTextBox = new System.Windows.Forms.TextBox();
            this.liferuarTextBox = new System.Windows.Forms.TextBox();
            this.dtpLfr = new System.Windows.Forms.DateTimePicker();
            this.kohaLiferuarTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.liferimiArtBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.liferimiArtDataGrid = new System.Windows.Forms.DataGrid();
            this.liferimiArtTableStyleDataGridTableStyle = new System.Windows.Forms.DataGridTableStyle();
            this.dataGridTextBoxColumn2 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.btnMbyllja = new System.Windows.Forms.Button();
            this.nrLiferimitLabel1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.liferimiTableAdapter = new MobileSales.MyMobileDataSetTableAdapters.LiferimiTableAdapter();
            this.liferimiArtTableAdapter = new MobileSales.MyMobileDataSetTableAdapters.LiferimiArtTableAdapter();
            this.vizitatTableAdapter1 = new MobileSales.MyMobileDataSetTableAdapters.VizitatTableAdapter();
            this.vizitatBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.porosiaArtTableAdapter1 = new MobileSales.MyMobileDataSetTableAdapters.PorosiaArtTableAdapter();
            this.lblTotali = new System.Windows.Forms.Label();
            this.lblDataLiferimit = new System.Windows.Forms.Label();
            this.btnFletePagesa = new System.Windows.Forms.Button();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menu_Print = new System.Windows.Forms.MenuItem();
            this.menu_Close = new System.Windows.Forms.MenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTot = new System.Windows.Forms.Label();
            this.lblAdresa = new System.Windows.Forms.Label();
            dataLiferuarLabel = new System.Windows.Forms.Label();
            titulliLiferimitLabel = new System.Windows.Forms.Label();
            cmimiDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            sasiaLiferuarDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            artEmriDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.liferimiBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.liferimiArtBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vizitatBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataLiferuarLabel
            // 
            dataLiferuarLabel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            dataLiferuarLabel.Location = new System.Drawing.Point(145, 3);
            dataLiferuarLabel.Name = "dataLiferuarLabel";
            dataLiferuarLabel.Size = new System.Drawing.Size(31, 13);
            dataLiferuarLabel.Text = "Data:";
            // 
            // titulliLiferimitLabel
            // 
            titulliLiferimitLabel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            titulliLiferimitLabel.Location = new System.Drawing.Point(0, 3);
            titulliLiferimitLabel.Name = "titulliLiferimitLabel";
            titulliLiferimitLabel.Size = new System.Drawing.Size(58, 15);
            titulliLiferimitLabel.Text = "Nr.Faturës:";
            // 
            // cmimiDataGridColumnStyleDataGridTextBoxColumn
            // 
            cmimiDataGridColumnStyleDataGridTextBoxColumn.Format = "0.00";
            cmimiDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            cmimiDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Ç.Njësi";
            cmimiDataGridColumnStyleDataGridTextBoxColumn.MappingName = "Cmimi";
            cmimiDataGridColumnStyleDataGridTextBoxColumn.Width = 40;
            // 
            // sasiaLiferuarDataGridColumnStyleDataGridTextBoxColumn
            // 
            sasiaLiferuarDataGridColumnStyleDataGridTextBoxColumn.Format = "0.000";
            sasiaLiferuarDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            sasiaLiferuarDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Porosi";
            sasiaLiferuarDataGridColumnStyleDataGridTextBoxColumn.MappingName = "SasiaPorositur";
            sasiaLiferuarDataGridColumnStyleDataGridTextBoxColumn.Width = 40;
            // 
            // artEmriDataGridColumnStyleDataGridTextBoxColumn
            // 
            artEmriDataGridColumnStyleDataGridTextBoxColumn.Format = "";
            artEmriDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            artEmriDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Artikulli";
            artEmriDataGridColumnStyleDataGridTextBoxColumn.MappingName = "ArtEmri";
            artEmriDataGridColumnStyleDataGridTextBoxColumn.Width = 120;
            // 
            // iDLiferimiTextBox
            // 
            this.iDLiferimiTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.liferimiBindingSource, "IDLiferimi", true));
            this.iDLiferimiTextBox.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.iDLiferimiTextBox.Location = new System.Drawing.Point(48, 78);
            this.iDLiferimiTextBox.Name = "iDLiferimiTextBox";
            this.iDLiferimiTextBox.Size = new System.Drawing.Size(3, 19);
            this.iDLiferimiTextBox.TabIndex = 2;
            this.iDLiferimiTextBox.Visible = false;
            // 
            // liferimiBindingSource
            // 
            this.liferimiBindingSource.DataMember = "Liferimi";
            this.liferimiBindingSource.DataSource = this.myMobileDataSet;
            // 
            // myMobileDataSet
            // 
            this.myMobileDataSet.DataSetName = "MobileDatabaseDataSet";
            this.myMobileDataSet.Prefix = "";
            this.myMobileDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dataLiferimitTextBox
            // 
            this.dataLiferimitTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.liferimiBindingSource, "DataLiferimit", true));
            this.dataLiferimitTextBox.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.dataLiferimitTextBox.Location = new System.Drawing.Point(47, 84);
            this.dataLiferimitTextBox.Name = "dataLiferimitTextBox";
            this.dataLiferimitTextBox.Size = new System.Drawing.Size(4, 19);
            this.dataLiferimitTextBox.TabIndex = 6;
            this.dataLiferimitTextBox.Visible = false;
            this.dataLiferimitTextBox.WordWrap = false;
            // 
            // iDPorosiaTextBox
            // 
            this.iDPorosiaTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.liferimiBindingSource, "IDPorosia", true));
            this.iDPorosiaTextBox.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.iDPorosiaTextBox.Location = new System.Drawing.Point(47, 84);
            this.iDPorosiaTextBox.Name = "iDPorosiaTextBox";
            this.iDPorosiaTextBox.Size = new System.Drawing.Size(5, 19);
            this.iDPorosiaTextBox.TabIndex = 10;
            this.iDPorosiaTextBox.Visible = false;
            // 
            // liferuarTextBox
            // 
            this.liferuarTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.liferimiBindingSource, "Liferuar", true));
            this.liferuarTextBox.Location = new System.Drawing.Point(47, 74);
            this.liferuarTextBox.Name = "liferuarTextBox";
            this.liferuarTextBox.Size = new System.Drawing.Size(5, 21);
            this.liferuarTextBox.TabIndex = 16;
            this.liferuarTextBox.Visible = false;
            // 
            // dtpLfr
            // 
            this.dtpLfr.CalendarFont = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.dtpLfr.CustomFormat = "dd-MM-yyyy";
            this.dtpLfr.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.liferimiBindingSource, "DataLiferuar", true));
            this.dtpLfr.Enabled = false;
            this.dtpLfr.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.dtpLfr.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpLfr.Location = new System.Drawing.Point(69, 84);
            this.dtpLfr.MaxDate = new System.DateTime(2999, 9, 19, 0, 0, 0, 0);
            this.dtpLfr.MinDate = new System.DateTime(2008, 9, 19, 0, 0, 0, 0);
            this.dtpLfr.Name = "dtpLfr";
            this.dtpLfr.Size = new System.Drawing.Size(78, 20);
            this.dtpLfr.TabIndex = 18;
            this.dtpLfr.Value = new System.DateTime(2008, 9, 19, 0, 0, 0, 0);
            this.dtpLfr.Visible = false;
            this.dtpLfr.ValueChanged += new System.EventHandler(this.dtpLfr_ValueChanged);
            // 
            // kohaLiferuarTextBox
            // 
            this.kohaLiferuarTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.liferimiBindingSource, "KohaLiferuar", true));
            this.kohaLiferuarTextBox.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.kohaLiferuarTextBox.Location = new System.Drawing.Point(47, 84);
            this.kohaLiferuarTextBox.Name = "kohaLiferuarTextBox";
            this.kohaLiferuarTextBox.Size = new System.Drawing.Size(3, 19);
            this.kohaLiferuarTextBox.TabIndex = 23;
            this.kohaLiferuarTextBox.Visible = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.label1.Location = new System.Drawing.Point(0, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 15);
            this.label1.Text = "Lokacioni:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // liferimiArtBindingSource
            // 
            this.liferimiArtBindingSource.DataMember = "LiferimiArt";
            this.liferimiArtBindingSource.DataSource = this.myMobileDataSet;
            this.liferimiArtBindingSource.CurrentChanged += new System.EventHandler(this.liferimiArtBindingSource_CurrentChanged);
            // 
            // liferimiArtDataGrid
            // 
            this.liferimiArtDataGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.liferimiArtDataGrid.DataSource = this.liferimiArtBindingSource;
            this.liferimiArtDataGrid.Location = new System.Drawing.Point(0, 50);
            this.liferimiArtDataGrid.Name = "liferimiArtDataGrid";
            this.liferimiArtDataGrid.RowHeadersVisible = false;
            this.liferimiArtDataGrid.Size = new System.Drawing.Size(240, 219);
            this.liferimiArtDataGrid.TabIndex = 37;
            this.liferimiArtDataGrid.TableStyles.Add(this.liferimiArtTableStyleDataGridTableStyle);
            this.liferimiArtDataGrid.MouseUp += new System.Windows.Forms.MouseEventHandler(this.liferimiArtDataGrid_MouseUp);
            this.liferimiArtDataGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.liferimiArtDataGrid_KeyDown);
            // 
            // liferimiArtTableStyleDataGridTableStyle
            // 
            this.liferimiArtTableStyleDataGridTableStyle.GridColumnStyles.Add(artEmriDataGridColumnStyleDataGridTextBoxColumn);
            this.liferimiArtTableStyleDataGridTableStyle.GridColumnStyles.Add(sasiaLiferuarDataGridColumnStyleDataGridTextBoxColumn);
            this.liferimiArtTableStyleDataGridTableStyle.GridColumnStyles.Add(cmimiDataGridColumnStyleDataGridTextBoxColumn);
            this.liferimiArtTableStyleDataGridTableStyle.GridColumnStyles.Add(this.dataGridTextBoxColumn2);
            this.liferimiArtTableStyleDataGridTableStyle.MappingName = "LiferimiArt";
            // 
            // dataGridTextBoxColumn2
            // 
            this.dataGridTextBoxColumn2.Format = "0.00";
            this.dataGridTextBoxColumn2.FormatInfo = null;
            this.dataGridTextBoxColumn2.HeaderText = "Totali";
            this.dataGridTextBoxColumn2.MappingName = "Totali";
            this.dataGridTextBoxColumn2.Width = 40;
            // 
            // btnMbyllja
            // 
            this.btnMbyllja.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnMbyllja.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.btnMbyllja.Location = new System.Drawing.Point(69, 110);
            this.btnMbyllja.Name = "btnMbyllja";
            this.btnMbyllja.Size = new System.Drawing.Size(63, 20);
            this.btnMbyllja.TabIndex = 50;
            this.btnMbyllja.Text = "Mbyllja";
            this.btnMbyllja.Click += new System.EventHandler(this.btnMbyllja_Click);
            // 
            // nrLiferimitLabel1
            // 
            this.nrLiferimitLabel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.nrLiferimitLabel1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.liferimiBindingSource, "NrLiferimit", true));
            this.nrLiferimitLabel1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Underline);
            this.nrLiferimitLabel1.Location = new System.Drawing.Point(58, 2);
            this.nrLiferimitLabel1.Name = "nrLiferimitLabel1";
            this.nrLiferimitLabel1.Size = new System.Drawing.Size(87, 16);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(1, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(239, 2);
            // 
            // liferimiTableAdapter
            // 
            this.liferimiTableAdapter.ClearBeforeFill = true;
            // 
            // liferimiArtTableAdapter
            // 
            this.liferimiArtTableAdapter.ClearBeforeFill = true;
            // 
            // vizitatTableAdapter1
            // 
            this.vizitatTableAdapter1.ClearBeforeFill = true;
            // 
            // vizitatBindingSource
            // 
            this.vizitatBindingSource.DataMember = "Vizitat";
            this.vizitatBindingSource.DataSource = this.myMobileDataSet;
            // 
            // porosiaArtTableAdapter1
            // 
            this.porosiaArtTableAdapter1.ClearBeforeFill = true;
            // 
            // lblTotali
            // 
            this.lblTotali.BackColor = System.Drawing.Color.Gray;
            this.lblTotali.Location = new System.Drawing.Point(134, 110);
            this.lblTotali.Name = "lblTotali";
            this.lblTotali.Size = new System.Drawing.Size(3, 20);
            this.lblTotali.Text = "label4";
            this.lblTotali.Visible = false;
            // 
            // lblDataLiferimit
            // 
            this.lblDataLiferimit.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblDataLiferimit.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Underline);
            this.lblDataLiferimit.Location = new System.Drawing.Point(179, 2);
            this.lblDataLiferimit.Name = "lblDataLiferimit";
            this.lblDataLiferimit.Size = new System.Drawing.Size(61, 16);
            // 
            // btnFletePagesa
            // 
            this.btnFletePagesa.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnFletePagesa.Location = new System.Drawing.Point(76, 224);
            this.btnFletePagesa.Name = "btnFletePagesa";
            this.btnFletePagesa.Size = new System.Drawing.Size(61, 20);
            this.btnFletePagesa.TabIndex = 75;
            this.btnFletePagesa.Text = "P/Fletë";
            this.btnFletePagesa.Visible = false;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menu_Print);
            this.mainMenu1.MenuItems.Add(this.menu_Close);
            // 
            // menu_Print
            // 
            this.menu_Print.Text = "Printo";
            this.menu_Print.Click += new System.EventHandler(this.menu_Print_Click);
            // 
            // menu_Close
            // 
            this.menu_Close.Text = "Dalja";
            this.menu_Close.Click += new System.EventHandler(this.menu_Close_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblTot);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 269);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(240, 21);
            // 
            // lblTot
            // 
            this.lblTot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblTot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTot.Location = new System.Drawing.Point(0, 0);
            this.lblTot.Name = "lblTot";
            this.lblTot.Size = new System.Drawing.Size(240, 21);
            this.lblTot.Text = "0.00";
            this.lblTot.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblAdresa
            // 
            this.lblAdresa.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblAdresa.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Underline);
            this.lblAdresa.Location = new System.Drawing.Point(58, 26);
            this.lblAdresa.Name = "lblAdresa";
            this.lblAdresa.Size = new System.Drawing.Size(182, 16);
            // 
            // frmLiferimi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(240, 290);
            this.ControlBox = false;
            this.Controls.Add(this.lblAdresa);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.liferimiArtDataGrid);
            this.Controls.Add(this.lblDataLiferimit);
            this.Controls.Add(this.lblTotali);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nrLiferimitLabel1);
            this.Controls.Add(this.btnMbyllja);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.kohaLiferuarTextBox);
            this.Controls.Add(this.dtpLfr);
            this.Controls.Add(this.iDLiferimiTextBox);
            this.Controls.Add(titulliLiferimitLabel);
            this.Controls.Add(this.dataLiferimitTextBox);
            this.Controls.Add(this.iDPorosiaTextBox);
            this.Controls.Add(dataLiferuarLabel);
            this.Controls.Add(this.liferuarTextBox);
            this.Controls.Add(this.btnFletePagesa);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.Menu = this.mainMenu1;
            this.Name = "frmLiferimi";
            this.Text = "test";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmLiferimi_Load);
            ((System.ComponentModel.ISupportInitialize)(this.liferimiBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.liferimiArtBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vizitatBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private MyMobileDataSet myMobileDataSet;
        private System.Windows.Forms.BindingSource liferimiBindingSource;
        private MobileSales.MyMobileDataSetTableAdapters.LiferimiTableAdapter liferimiTableAdapter;
        private System.Windows.Forms.TextBox iDLiferimiTextBox;
        private System.Windows.Forms.TextBox dataLiferimitTextBox;
        private System.Windows.Forms.TextBox iDPorosiaTextBox;
        private System.Windows.Forms.TextBox liferuarTextBox;
        private System.Windows.Forms.DateTimePicker dtpLfr;
       // private System.Windows.Forms.BindingSource liferimi_LiferimiArtBindingSource;
        private MobileSales.MyMobileDataSetTableAdapters.LiferimiArtTableAdapter liferimiArtTableAdapter;
        private System.Windows.Forms.TextBox kohaLiferuarTextBox;
        private MobileSales.MyMobileDataSetTableAdapters.VizitatTableAdapter vizitatTableAdapter1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.BindingSource liferimiArtBindingSource;
        private System.Windows.Forms.DataGrid liferimiArtDataGrid;
        private System.Windows.Forms.DataGridTableStyle liferimiArtTableStyleDataGridTableStyle;
        private System.Windows.Forms.Button btnMbyllja;
        private System.Windows.Forms.Label nrLiferimitLabel1;
        private System.Windows.Forms.BindingSource vizitatBindingSource;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn2;
        private MobileSales.MyMobileDataSetTableAdapters.PorosiaArtTableAdapter porosiaArtTableAdapter1;
        private System.Windows.Forms.Label lblTotali;
        private System.Windows.Forms.Label lblDataLiferimit;
        private System.Windows.Forms.Button btnFletePagesa;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menu_Print;
        private System.Windows.Forms.MenuItem menu_Close;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblAdresa;
        private System.Windows.Forms.Label lblTot;


    }
}