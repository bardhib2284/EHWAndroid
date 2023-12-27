namespace MobileSales
{
    partial class frmArtikujtShitur
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridTextBoxColumn artEmriDataGridColumnStyleDataGridTextBoxColumn;
            System.Windows.Forms.DataGridTextBoxColumn cmimiDataGridColumnStyleDataGridTextBoxColumn;
            System.Windows.Forms.DataGridTextBoxColumn sasiaLiferuarDataGridColumnStyleDataGridTextBoxColumn;
            System.Windows.Forms.DataGridTextBoxColumn totaliDataGridColumnStyleDataGridTextBoxColumn;
            System.Windows.Forms.Label qytetiLabel;
            System.Windows.Forms.Label lblData;
            System.Windows.Forms.Label emriLokacionitLabel;
            System.Windows.Forms.Label label2;
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menyDalja = new System.Windows.Forms.MenuItem();
            this.menuPrinto = new System.Windows.Forms.MenuItem();
            this.btnKthimi = new System.Windows.Forms.MenuItem();
            this.artikujtShiturBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.myMobileDataset = new MobileSales.MyMobileDataSet();
            this.artikujtShiturDataGrid = new System.Windows.Forms.DataGrid();
            this.artikujtShiturTableStyleDataGridTableStyle = new System.Windows.Forms.DataGridTableStyle();
            this.dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.cmimiTotalLabel1 = new System.Windows.Forms.Label();
            this.nrLiferimitLabel1 = new System.Windows.Forms.Label();
            this.lblDataLiferimit = new System.Windows.Forms.Label();
            this.qytetiLabel1 = new System.Windows.Forms.Label();
            this.lblLine = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpDateLiferimit = new System.Windows.Forms.DateTimePicker();
            this.emriLokacionitLabel1 = new System.Windows.Forms.Label();
            this.artikujtShiturTableAdapter = new MobileSales.MyMobileDataSetTableAdapters.ArtikujtShiturTableAdapter();
            this.vizitatTableAdapter1 = new MobileSales.MyMobileDataSetTableAdapters.VizitatTableAdapter();
            artEmriDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            cmimiDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            sasiaLiferuarDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            totaliDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            qytetiLabel = new System.Windows.Forms.Label();
            lblData = new System.Windows.Forms.Label();
            emriLokacionitLabel = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.artikujtShiturBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDataset)).BeginInit();
            this.SuspendLayout();
            // 
            // artEmriDataGridColumnStyleDataGridTextBoxColumn
            // 
            artEmriDataGridColumnStyleDataGridTextBoxColumn.Format = "";
            artEmriDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            artEmriDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Artikulli";
            artEmriDataGridColumnStyleDataGridTextBoxColumn.MappingName = "ArtEmri";
            artEmriDataGridColumnStyleDataGridTextBoxColumn.Width = 100;
            // 
            // cmimiDataGridColumnStyleDataGridTextBoxColumn
            // 
            cmimiDataGridColumnStyleDataGridTextBoxColumn.Format = "0.00";
            cmimiDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            cmimiDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Ç.Njësi";
            cmimiDataGridColumnStyleDataGridTextBoxColumn.MappingName = "Cmimi";
            cmimiDataGridColumnStyleDataGridTextBoxColumn.Width = 45;
            // 
            // sasiaLiferuarDataGridColumnStyleDataGridTextBoxColumn
            // 
            sasiaLiferuarDataGridColumnStyleDataGridTextBoxColumn.Format = "0.000";
            sasiaLiferuarDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            sasiaLiferuarDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Porosi";
            sasiaLiferuarDataGridColumnStyleDataGridTextBoxColumn.MappingName = "SasiaLiferuar";
            sasiaLiferuarDataGridColumnStyleDataGridTextBoxColumn.Width = 45;
            // 
            // totaliDataGridColumnStyleDataGridTextBoxColumn
            // 
            totaliDataGridColumnStyleDataGridTextBoxColumn.Format = "0.00";
            totaliDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            totaliDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Totali";
            totaliDataGridColumnStyleDataGridTextBoxColumn.MappingName = "Totali";
            totaliDataGridColumnStyleDataGridTextBoxColumn.Width = 45;
            // 
            // qytetiLabel
            // 
            qytetiLabel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            qytetiLabel.Location = new System.Drawing.Point(0, 28);
            qytetiLabel.Name = "qytetiLabel";
            qytetiLabel.Size = new System.Drawing.Size(45, 14);
            qytetiLabel.Text = "Kontakt:";
            // 
            // lblData
            // 
            lblData.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            lblData.Location = new System.Drawing.Point(135, 6);
            lblData.Name = "lblData";
            lblData.Size = new System.Drawing.Size(31, 17);
            lblData.Text = "Data:";
            // 
            // emriLokacionitLabel
            // 
            emriLokacionitLabel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            emriLokacionitLabel.Location = new System.Drawing.Point(135, 29);
            emriLokacionitLabel.Name = "emriLokacionitLabel";
            emriLokacionitLabel.Size = new System.Drawing.Size(35, 13);
            emriLokacionitLabel.Text = "Vendi:";
            // 
            // label2
            // 
            label2.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            label2.Location = new System.Drawing.Point(0, 3);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(38, 14);
            label2.Text = "Nr.Fat:";
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menyDalja);
            this.mainMenu1.MenuItems.Add(this.menuPrinto);
            this.mainMenu1.MenuItems.Add(this.btnKthimi);
            // 
            // menyDalja
            // 
            this.menyDalja.Text = "Dalja";
            this.menyDalja.Click += new System.EventHandler(this.menyDalja_Click);
            // 
            // menuPrinto
            // 
            this.menuPrinto.Text = "Printo";
            this.menuPrinto.Click += new System.EventHandler(this.menuFatura_Click);
            // 
            // btnKthimi
            // 
            this.btnKthimi.Text = "Kthimi";
            this.btnKthimi.Click += new System.EventHandler(this.btnKthimi_Click);
            // 
            // artikujtShiturBindingSource
            // 
            this.artikujtShiturBindingSource.DataMember = "ArtikujtShitur";
            this.artikujtShiturBindingSource.DataSource = this.myMobileDataset;
            // 
            // myMobileDataset
            // 
            this.myMobileDataset.DataSetName = "MobileDatabaseDataSet";
            this.myMobileDataset.Prefix = "";
            this.myMobileDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // artikujtShiturDataGrid
            // 
            this.artikujtShiturDataGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.artikujtShiturDataGrid.DataSource = this.artikujtShiturBindingSource;
            this.artikujtShiturDataGrid.Location = new System.Drawing.Point(0, 46);
            this.artikujtShiturDataGrid.Name = "artikujtShiturDataGrid";
            this.artikujtShiturDataGrid.RowHeadersVisible = false;
            this.artikujtShiturDataGrid.Size = new System.Drawing.Size(240, 217);
            this.artikujtShiturDataGrid.TabIndex = 1;
            this.artikujtShiturDataGrid.TableStyles.Add(this.artikujtShiturTableStyleDataGridTableStyle);
            this.artikujtShiturDataGrid.MouseUp += new System.Windows.Forms.MouseEventHandler(this.artikujtShiturDataGrid_MouseUp);
            this.artikujtShiturDataGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.artikujtShiturDataGrid_KeyDown);
            // 
            // artikujtShiturTableStyleDataGridTableStyle
            // 
            this.artikujtShiturTableStyleDataGridTableStyle.GridColumnStyles.Add(artEmriDataGridColumnStyleDataGridTextBoxColumn);
            this.artikujtShiturTableStyleDataGridTableStyle.GridColumnStyles.Add(sasiaLiferuarDataGridColumnStyleDataGridTextBoxColumn);
            this.artikujtShiturTableStyleDataGridTableStyle.GridColumnStyles.Add(cmimiDataGridColumnStyleDataGridTextBoxColumn);
            this.artikujtShiturTableStyleDataGridTableStyle.GridColumnStyles.Add(totaliDataGridColumnStyleDataGridTextBoxColumn);
            this.artikujtShiturTableStyleDataGridTableStyle.GridColumnStyles.Add(this.dataGridTextBoxColumn1);
            this.artikujtShiturTableStyleDataGridTableStyle.MappingName = "ArtikujtShitur";
            // 
            // dataGridTextBoxColumn1
            // 
            this.dataGridTextBoxColumn1.Format = "";
            this.dataGridTextBoxColumn1.FormatInfo = null;
            this.dataGridTextBoxColumn1.HeaderText = "Seri";
            this.dataGridTextBoxColumn1.MappingName = "Seri";
            // 
            // cmimiTotalLabel1
            // 
            this.cmimiTotalLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.cmimiTotalLabel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cmimiTotalLabel1.ForeColor = System.Drawing.Color.Black;
            this.cmimiTotalLabel1.Location = new System.Drawing.Point(0, 269);
            this.cmimiTotalLabel1.Name = "cmimiTotalLabel1";
            this.cmimiTotalLabel1.Size = new System.Drawing.Size(240, 25);
            this.cmimiTotalLabel1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // nrLiferimitLabel1
            // 
            this.nrLiferimitLabel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.nrLiferimitLabel1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.artikujtShiturBindingSource, "NrLiferimit", true));
            this.nrLiferimitLabel1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.nrLiferimitLabel1.Location = new System.Drawing.Point(47, 3);
            this.nrLiferimitLabel1.Name = "nrLiferimitLabel1";
            this.nrLiferimitLabel1.Size = new System.Drawing.Size(83, 11);
            // 
            // lblDataLiferimit
            // 
            this.lblDataLiferimit.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblDataLiferimit.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblDataLiferimit.Location = new System.Drawing.Point(171, 6);
            this.lblDataLiferimit.Name = "lblDataLiferimit";
            this.lblDataLiferimit.Size = new System.Drawing.Size(69, 11);
            // 
            // qytetiLabel1
            // 
            this.qytetiLabel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.qytetiLabel1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.artikujtShiturBindingSource, "KontaktEmriMbiemri", true));
            this.qytetiLabel1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.qytetiLabel1.Location = new System.Drawing.Point(47, 28);
            this.qytetiLabel1.Name = "qytetiLabel1";
            this.qytetiLabel1.Size = new System.Drawing.Size(83, 12);
            // 
            // lblLine
            // 
            this.lblLine.BackColor = System.Drawing.Color.Black;
            this.lblLine.Location = new System.Drawing.Point(0, 19);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(130, 1);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(135, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 1);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(135, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 1);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(0, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(130, 1);
            // 
            // dtpDateLiferimit
            // 
            this.dtpDateLiferimit.CustomFormat = "dd-MM-yyyy";
            this.dtpDateLiferimit.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.artikujtShiturBindingSource, "DataLiferimit", true));
            this.dtpDateLiferimit.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateLiferimit.Location = new System.Drawing.Point(119, 201);
            this.dtpDateLiferimit.Name = "dtpDateLiferimit";
            this.dtpDateLiferimit.Size = new System.Drawing.Size(10, 22);
            this.dtpDateLiferimit.TabIndex = 17;
            this.dtpDateLiferimit.Visible = false;
            // 
            // emriLokacionitLabel1
            // 
            this.emriLokacionitLabel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.emriLokacionitLabel1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.artikujtShiturBindingSource, "EmriLokacionit", true));
            this.emriLokacionitLabel1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.emriLokacionitLabel1.Location = new System.Drawing.Point(171, 28);
            this.emriLokacionitLabel1.Name = "emriLokacionitLabel1";
            this.emriLokacionitLabel1.Size = new System.Drawing.Size(69, 12);
            // 
            // artikujtShiturTableAdapter
            // 
            this.artikujtShiturTableAdapter.ClearBeforeFill = true;
            // 
            // vizitatTableAdapter1
            // 
            this.vizitatTableAdapter1.ClearBeforeFill = true;
            // 
            // frmArtikujtShitur
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.cmimiTotalLabel1);
            this.Controls.Add(this.artikujtShiturDataGrid);
            this.Controls.Add(label2);
            this.Controls.Add(emriLokacionitLabel);
            this.Controls.Add(this.emriLokacionitLabel1);
            this.Controls.Add(this.dtpDateLiferimit);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblLine);
            this.Controls.Add(lblData);
            this.Controls.Add(qytetiLabel);
            this.Controls.Add(this.qytetiLabel1);
            this.Controls.Add(this.lblDataLiferimit);
            this.Controls.Add(this.nrLiferimitLabel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(0, 0);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "frmArtikujtShitur";
            this.Text = "ArtikujtShitur";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ArtikujtShitur_Load);
            ((System.ComponentModel.ISupportInitialize)(this.artikujtShiturBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDataset)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private MyMobileDataSet myMobileDataset;
        private System.Windows.Forms.BindingSource artikujtShiturBindingSource;
        private MobileSales.MyMobileDataSetTableAdapters.ArtikujtShiturTableAdapter artikujtShiturTableAdapter;
        private System.Windows.Forms.DataGrid artikujtShiturDataGrid;
        private System.Windows.Forms.DataGridTableStyle artikujtShiturTableStyleDataGridTableStyle;
        private System.Windows.Forms.MenuItem menyDalja;
        private System.Windows.Forms.Label cmimiTotalLabel1;
        private System.Windows.Forms.Label nrLiferimitLabel1;
        private System.Windows.Forms.Label lblDataLiferimit;
        private System.Windows.Forms.Label qytetiLabel1;
        private System.Windows.Forms.Label lblLine;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpDateLiferimit;
        private System.Windows.Forms.Label emriLokacionitLabel1;
        private System.Windows.Forms.MenuItem menuPrinto;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn1;
        private System.Windows.Forms.MenuItem btnKthimi;
        private MobileSales.MyMobileDataSetTableAdapters.VizitatTableAdapter vizitatTableAdapter1;
    }
}