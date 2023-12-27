namespace MobileSales
{
    partial class frmListaShitjeve
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
            System.Windows.Forms.DataGridTextBoxColumn emriLokacionitDataGridColumnStyleDataGridTextBoxColumn;
            System.Windows.Forms.DataGridTextBoxColumn nrPorosiseDataGridColumnStyleDataGridTextBoxColumn;
            System.Windows.Forms.DataGridTextBoxColumn dataPlanifikimitDataGridColumnStyleDataGridTextBoxColumn;
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuDalja = new System.Windows.Forms.MenuItem();
            this.menPrinto = new System.Windows.Forms.MenuItem();
            this.menyShfaq = new System.Windows.Forms.MenuItem();
            this.menuFatura = new System.Windows.Forms.MenuItem();
            this.menuInkasimet = new System.Windows.Forms.MenuItem();
            this.manuInkasimi = new System.Windows.Forms.MenuItem();
            this.cmbDitet = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listaShitjeveBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.myMobileDataset = new MobileSales.MyMobileDataSet();
            this.listaShitjeveDataGrid = new System.Windows.Forms.DataGrid();
            this.listaShitjeveTableStyleDataGridTableStyle = new System.Windows.Forms.DataGridTableStyle();
            this.dataGridTextBoxColumn2 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn3 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.lblIDVizita = new System.Windows.Forms.Label();
            this.dtpDataLiferimit = new System.Windows.Forms.DateTimePicker();
            this.lblIDLiferimi = new System.Windows.Forms.Label();
            this.lblData = new System.Windows.Forms.Label();
            this.lblDt = new System.Windows.Forms.Label();
            this.Line2 = new System.Windows.Forms.Label();
            this.line1 = new System.Windows.Forms.Label();
            this.lblTot = new System.Windows.Forms.Label();
            this.lblTotali = new System.Windows.Forms.Label();
            this.lbTotaliBind = new System.Windows.Forms.Label();
            this.lblTotInk = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTotKthimet = new System.Windows.Forms.Label();
            this.iDKlientiLabel1 = new System.Windows.Forms.Label();
            this.listaShitjeveTableAdapter = new MobileSales.MyMobileDataSetTableAdapters.ListaShitjeveTableAdapter();
            emriLokacionitDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            nrPorosiseDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            dataPlanifikimitDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.listaShitjeveBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDataset)).BeginInit();
            this.SuspendLayout();
            // 
            // emriLokacionitDataGridColumnStyleDataGridTextBoxColumn
            // 
            emriLokacionitDataGridColumnStyleDataGridTextBoxColumn.Format = "";
            emriLokacionitDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            emriLokacionitDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Klienti";
            emriLokacionitDataGridColumnStyleDataGridTextBoxColumn.MappingName = "EmriLokacionit";
            // 
            // nrPorosiseDataGridColumnStyleDataGridTextBoxColumn
            // 
            nrPorosiseDataGridColumnStyleDataGridTextBoxColumn.Format = "";
            nrPorosiseDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            nrPorosiseDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Nr.Fat";
            nrPorosiseDataGridColumnStyleDataGridTextBoxColumn.MappingName = "NrLiferimit";
            nrPorosiseDataGridColumnStyleDataGridTextBoxColumn.Width = 63;
            // 
            // dataPlanifikimitDataGridColumnStyleDataGridTextBoxColumn
            // 
            dataPlanifikimitDataGridColumnStyleDataGridTextBoxColumn.Format = "dd-MM-yyyy";
            dataPlanifikimitDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            dataPlanifikimitDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Data";
            dataPlanifikimitDataGridColumnStyleDataGridTextBoxColumn.MappingName = "DataLiferimit";
            dataPlanifikimitDataGridColumnStyleDataGridTextBoxColumn.Width = 56;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuDalja);
            this.mainMenu1.MenuItems.Add(this.menPrinto);
            this.mainMenu1.MenuItems.Add(this.menyShfaq);
            this.mainMenu1.MenuItems.Add(this.manuInkasimi);
            // 
            // menuDalja
            // 
            this.menuDalja.Text = "Dalja";
            this.menuDalja.Click += new System.EventHandler(this.menuDalja_Click);
            // 
            // menPrinto
            // 
            this.menPrinto.Text = "Printo";
            this.menPrinto.Click += new System.EventHandler(this.menuPrinto_Click);
            // 
            // menyShfaq
            // 
            this.menyShfaq.MenuItems.Add(this.menuFatura);
            this.menyShfaq.MenuItems.Add(this.menuInkasimet);
            this.menyShfaq.Text = "Shfaq";
            this.menyShfaq.Click += new System.EventHandler(this.menyShfaq_Click);
            // 
            // menuFatura
            // 
            this.menuFatura.Text = "Faturën";
            this.menuFatura.Click += new System.EventHandler(this.menuFatura_Click);
            // 
            // menuInkasimet
            // 
            this.menuInkasimet.Text = "Inkasimet";
            this.menuInkasimet.Click += new System.EventHandler(this.menuInkasimet_Click);
            // 
            // manuInkasimi
            // 
            this.manuInkasimi.Text = "Inkaso";
            this.manuInkasimi.Click += new System.EventHandler(this.manuRiInkasimi_Click);
            // 
            // cmbDitet
            // 
            this.cmbDitet.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.cmbDitet.Items.Add("Java");
            this.cmbDitet.Items.Add("E hënë");
            this.cmbDitet.Items.Add("E martë");
            this.cmbDitet.Items.Add("E mërkurë");
            this.cmbDitet.Items.Add("E enjte");
            this.cmbDitet.Items.Add("E premte");
            this.cmbDitet.Items.Add("E shtunë");
            this.cmbDitet.Items.Add("E diel");
            this.cmbDitet.Location = new System.Drawing.Point(37, 1);
            this.cmbDitet.Name = "cmbDitet";
            this.cmbDitet.Size = new System.Drawing.Size(84, 20);
            this.cmbDitet.TabIndex = 0;
            this.cmbDitet.SelectedIndexChanged += new System.EventHandler(this.cmbDitet_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.Text = "Dita:";
            // 
            // listaShitjeveBindingSource
            // 
            this.listaShitjeveBindingSource.DataMember = "ListaShitjeve";
            this.listaShitjeveBindingSource.DataSource = this.myMobileDataset;
            // 
            // myMobileDataset
            // 
            this.myMobileDataset.DataSetName = "MobileDatabaseDataSet";
            this.myMobileDataset.Prefix = "";
            this.myMobileDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // listaShitjeveDataGrid
            // 
            this.listaShitjeveDataGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.listaShitjeveDataGrid.DataSource = this.listaShitjeveBindingSource;
            this.listaShitjeveDataGrid.Location = new System.Drawing.Point(0, 24);
            this.listaShitjeveDataGrid.Name = "listaShitjeveDataGrid";
            this.listaShitjeveDataGrid.RowHeadersVisible = false;
            this.listaShitjeveDataGrid.Size = new System.Drawing.Size(240, 199);
            this.listaShitjeveDataGrid.TabIndex = 2;
            this.listaShitjeveDataGrid.TableStyles.Add(this.listaShitjeveTableStyleDataGridTableStyle);
            this.listaShitjeveDataGrid.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listaShitjeveDataGrid_MouseUp);
            this.listaShitjeveDataGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listaShitjeveDataGrid_KeyDown);
            // 
            // listaShitjeveTableStyleDataGridTableStyle
            // 
            this.listaShitjeveTableStyleDataGridTableStyle.GridColumnStyles.Add(this.dataGridTextBoxColumn2);
            this.listaShitjeveTableStyleDataGridTableStyle.GridColumnStyles.Add(emriLokacionitDataGridColumnStyleDataGridTextBoxColumn);
            this.listaShitjeveTableStyleDataGridTableStyle.GridColumnStyles.Add(dataPlanifikimitDataGridColumnStyleDataGridTextBoxColumn);
            this.listaShitjeveTableStyleDataGridTableStyle.GridColumnStyles.Add(nrPorosiseDataGridColumnStyleDataGridTextBoxColumn);
            this.listaShitjeveTableStyleDataGridTableStyle.GridColumnStyles.Add(this.dataGridTextBoxColumn1);
            this.listaShitjeveTableStyleDataGridTableStyle.GridColumnStyles.Add(this.dataGridTextBoxColumn3);
            this.listaShitjeveTableStyleDataGridTableStyle.MappingName = "ListaShitjeve";
            // 
            // dataGridTextBoxColumn2
            // 
            this.dataGridTextBoxColumn2.Format = "";
            this.dataGridTextBoxColumn2.FormatInfo = null;
            this.dataGridTextBoxColumn2.HeaderText = "Kontakt";
            this.dataGridTextBoxColumn2.MappingName = "KontaktEmriMbiemri";
            // 
            // dataGridTextBoxColumn1
            // 
            this.dataGridTextBoxColumn1.Format = "0.00";
            this.dataGridTextBoxColumn1.FormatInfo = null;
            this.dataGridTextBoxColumn1.HeaderText = "Totali";
            this.dataGridTextBoxColumn1.MappingName = "CmimiTotal";
            this.dataGridTextBoxColumn1.Width = 40;
            // 
            // dataGridTextBoxColumn3
            // 
            this.dataGridTextBoxColumn3.Format = "0.00";
            this.dataGridTextBoxColumn3.FormatInfo = null;
            this.dataGridTextBoxColumn3.HeaderText = "Inkas";
            this.dataGridTextBoxColumn3.MappingName = "ShumaPaguar";
            this.dataGridTextBoxColumn3.Width = 40;
            // 
            // lblIDVizita
            // 
            this.lblIDVizita.BackColor = System.Drawing.Color.DarkRed;
            this.lblIDVizita.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.listaShitjeveBindingSource, "IDVizita", true));
            this.lblIDVizita.Location = new System.Drawing.Point(57, 68);
            this.lblIDVizita.Name = "lblIDVizita";
            this.lblIDVizita.Size = new System.Drawing.Size(8, 20);
            this.lblIDVizita.Visible = false;
            // 
            // dtpDataLiferimit
            // 
            this.dtpDataLiferimit.CustomFormat = "dd/MM/yyyy hh:mm:ss tt";
            this.dtpDataLiferimit.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.listaShitjeveBindingSource, "DataLiferimit", true));
            this.dtpDataLiferimit.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.listaShitjeveBindingSource, "DataLiferimit", true));
            this.dtpDataLiferimit.DataBindings.Add(new System.Windows.Forms.Binding("Tag", this.listaShitjeveBindingSource, "DataLiferimit", true));
            this.dtpDataLiferimit.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDataLiferimit.Location = new System.Drawing.Point(117, 66);
            this.dtpDataLiferimit.Name = "dtpDataLiferimit";
            this.dtpDataLiferimit.Size = new System.Drawing.Size(10, 22);
            this.dtpDataLiferimit.TabIndex = 6;
            this.dtpDataLiferimit.Visible = false;
            // 
            // lblIDLiferimi
            // 
            this.lblIDLiferimi.BackColor = System.Drawing.Color.DarkRed;
            this.lblIDLiferimi.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.listaShitjeveBindingSource, "IDLiferimi", true));
            this.lblIDLiferimi.Location = new System.Drawing.Point(91, 68);
            this.lblIDLiferimi.Name = "lblIDLiferimi";
            this.lblIDLiferimi.Size = new System.Drawing.Size(9, 20);
            this.lblIDLiferimi.Visible = false;
            // 
            // lblData
            // 
            this.lblData.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblData.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblData.Location = new System.Drawing.Point(157, 4);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(77, 12);
            // 
            // lblDt
            // 
            this.lblDt.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblDt.Location = new System.Drawing.Point(123, 4);
            this.lblDt.Name = "lblDt";
            this.lblDt.Size = new System.Drawing.Size(31, 12);
            this.lblDt.Text = "Data:";
            // 
            // Line2
            // 
            this.Line2.BackColor = System.Drawing.Color.Black;
            this.Line2.Location = new System.Drawing.Point(123, 19);
            this.Line2.Name = "Line2";
            this.Line2.Size = new System.Drawing.Size(114, 1);
            // 
            // line1
            // 
            this.line1.BackColor = System.Drawing.Color.Black;
            this.line1.Location = new System.Drawing.Point(0, 20);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(121, 1);
            // 
            // lblTot
            // 
            this.lblTot.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblTot.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblTot.ForeColor = System.Drawing.Color.Black;
            this.lblTot.Location = new System.Drawing.Point(14, 247);
            this.lblTot.Name = "lblTot";
            this.lblTot.Size = new System.Drawing.Size(113, 20);
            this.lblTot.Text = "Pagesat/Kthimet:";
            this.lblTot.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblTotali
            // 
            this.lblTotali.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblTotali.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblTotali.ForeColor = System.Drawing.Color.Black;
            this.lblTotali.Location = new System.Drawing.Point(132, 224);
            this.lblTotali.Name = "lblTotali";
            this.lblTotali.Size = new System.Drawing.Size(105, 20);
            this.lblTotali.Text = "0.00";
            this.lblTotali.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbTotaliBind
            // 
            this.lbTotaliBind.BackColor = System.Drawing.Color.Silver;
            this.lbTotaliBind.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.listaShitjeveBindingSource, "CmimiTotal", true));
            this.lbTotaliBind.Location = new System.Drawing.Point(41, 202);
            this.lbTotaliBind.Name = "lbTotaliBind";
            this.lbTotaliBind.Size = new System.Drawing.Size(10, 20);
            this.lbTotaliBind.Visible = false;
            // 
            // lblTotInk
            // 
            this.lblTotInk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblTotInk.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblTotInk.ForeColor = System.Drawing.Color.Black;
            this.lblTotInk.Location = new System.Drawing.Point(132, 271);
            this.lblTotInk.Name = "lblTotInk";
            this.lblTotInk.Size = new System.Drawing.Size(105, 18);
            this.lblTotInk.Text = "0.00";
            this.lblTotInk.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(57, 271);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 20);
            this.label3.Text = "Inkasimet:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(64, 224);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 20);
            this.label2.Text = "Faturimi:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblTotKthimet
            // 
            this.lblTotKthimet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblTotKthimet.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblTotKthimet.ForeColor = System.Drawing.Color.Black;
            this.lblTotKthimet.Location = new System.Drawing.Point(132, 247);
            this.lblTotKthimet.Name = "lblTotKthimet";
            this.lblTotKthimet.Size = new System.Drawing.Size(105, 20);
            this.lblTotKthimet.Text = "0.00";
            this.lblTotKthimet.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // iDKlientiLabel1
            // 
            this.iDKlientiLabel1.BackColor = System.Drawing.Color.DarkRed;
            this.iDKlientiLabel1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.listaShitjeveBindingSource, "IDKlientDheLokacion", true));
            this.iDKlientiLabel1.Location = new System.Drawing.Point(137, 68);
            this.iDKlientiLabel1.Name = "iDKlientiLabel1";
            this.iDKlientiLabel1.Size = new System.Drawing.Size(17, 20);
            this.iDKlientiLabel1.Visible = false;
            // 
            // listaShitjeveTableAdapter
            // 
            this.listaShitjeveTableAdapter.ClearBeforeFill = true;
            // 
            // frmListaShitjeve
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(240, 293);
            this.ControlBox = false;
            this.Controls.Add(this.iDKlientiLabel1);
            this.Controls.Add(this.lblTotKthimet);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblTotInk);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblTotali);
            this.Controls.Add(this.lblTot);
            this.Controls.Add(this.line1);
            this.Controls.Add(this.Line2);
            this.Controls.Add(this.lblDt);
            this.Controls.Add(this.lblData);
            this.Controls.Add(this.lblIDLiferimi);
            this.Controls.Add(this.dtpDataLiferimit);
            this.Controls.Add(this.lblIDVizita);
            this.Controls.Add(this.listaShitjeveDataGrid);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbDitet);
            this.Controls.Add(this.lbTotaliBind);
            this.Location = new System.Drawing.Point(0, 0);
            this.Menu = this.mainMenu1;
            this.Name = "frmListaShitjeve";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ListaShitjeve_Load);
            ((System.ComponentModel.ISupportInitialize)(this.listaShitjeveBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDataset)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private MyMobileDataSet myMobileDataset;
        private System.Windows.Forms.ComboBox cmbDitet;
        private System.Windows.Forms.Label label1;
        
        private System.Windows.Forms.BindingSource listaShitjeveBindingSource;
        private MobileSales.MyMobileDataSetTableAdapters.ListaShitjeveTableAdapter listaShitjeveTableAdapter;
        private System.Windows.Forms.DataGrid listaShitjeveDataGrid;
        private System.Windows.Forms.DataGridTableStyle listaShitjeveTableStyleDataGridTableStyle;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn1;
        private System.Windows.Forms.MenuItem menuDalja;
        private System.Windows.Forms.MenuItem menyShfaq;
        private System.Windows.Forms.Label lblIDVizita;
        private System.Windows.Forms.DateTimePicker dtpDataLiferimit;
        private System.Windows.Forms.Label lblIDLiferimi;
        private System.Windows.Forms.Label lblData;
        private System.Windows.Forms.Label lblDt;
        private System.Windows.Forms.Label Line2;
        private System.Windows.Forms.Label line1;
        private System.Windows.Forms.Label lblTot;
        private System.Windows.Forms.Label lblTotali;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn2;
        private System.Windows.Forms.Label lbTotaliBind;
        private System.Windows.Forms.MenuItem menPrinto;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn3;
        private System.Windows.Forms.Label lblTotInk;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTotKthimet;
        private System.Windows.Forms.MenuItem manuInkasimi;
        private System.Windows.Forms.Label iDKlientiLabel1;
        private System.Windows.Forms.MenuItem menuFatura;
        private System.Windows.Forms.MenuItem menuInkasimet;
    }
}