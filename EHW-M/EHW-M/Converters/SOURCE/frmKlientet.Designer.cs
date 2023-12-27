namespace MobileSales
{
    partial class frmKlientet
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
            System.Windows.Forms.Label lblKlienti;
            System.Windows.Forms.DataGridTextBoxColumn lokacioniDataGridColumnStyleDataGridTextBoxColumn;
            System.Windows.Forms.DataGridTextBoxColumn qytetiDataGridColumnStyleDataGridTextBoxColumn;
            System.Windows.Forms.DataGridTextBoxColumn adresaDataGridColumnStyleDataGridTextBoxColumn;
            System.Windows.Forms.DataGridTextBoxColumn testStatDataGridColumnStyleDataGridTextBoxColumn;
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menShitja = new System.Windows.Forms.MenuItem();
            this.menShfaq = new System.Windows.Forms.MenuItem();
            this.menDalja = new System.Windows.Forms.MenuItem();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.klientetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.myMobileDataset = new MobileSales.MyMobileDataSet();
            this.emriComboBox = new System.Windows.Forms.ComboBox();
            this.listaVizitaveBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.listaVizitaveDataGrid = new System.Windows.Forms.DataGrid();
            this.listaVizitaveTableStyleDataGridTableStyle = new System.Windows.Forms.DataGridTableStyle();
            this.dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.txtIDKlientDheLokacion = new System.Windows.Forms.TextBox();
            this.lblIDVizita = new System.Windows.Forms.Label();
            this.testStatLabel1 = new System.Windows.Forms.Label();
            this.lfrStatLabel1 = new System.Windows.Forms.Label();
            this.listaVizitaveTableAdapter = new MobileSales.MyMobileDataSetTableAdapters.ListaVizitaveTableAdapter();
            this.vizitatTableAdapter1 = new MobileSales.MyMobileDataSetTableAdapters.VizitatTableAdapter();
            this.klientetTableAdapter = new MobileSales.MyMobileDataSetTableAdapters.KlientetTableAdapter();
            this.klientDheLokacionTableAdapter1 = new MobileSales.MyMobileDataSetTableAdapters.KlientDheLokacionTableAdapter();
            lblKlienti = new System.Windows.Forms.Label();
            lokacioniDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            qytetiDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            adresaDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            testStatDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.klientetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDataset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listaVizitaveBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // lblKlienti
            // 
            lblKlienti.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            lblKlienti.Location = new System.Drawing.Point(3, 33);
            lblKlienti.Name = "lblKlienti";
            lblKlienti.Size = new System.Drawing.Size(49, 14);
            lblKlienti.Text = "Klienti:";
            // 
            // lokacioniDataGridColumnStyleDataGridTextBoxColumn
            // 
            lokacioniDataGridColumnStyleDataGridTextBoxColumn.Format = "";
            lokacioniDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            lokacioniDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Lokacioni";
            lokacioniDataGridColumnStyleDataGridTextBoxColumn.MappingName = "Lokacioni";
            lokacioniDataGridColumnStyleDataGridTextBoxColumn.Width = 70;
            // 
            // qytetiDataGridColumnStyleDataGridTextBoxColumn
            // 
            qytetiDataGridColumnStyleDataGridTextBoxColumn.Format = "";
            qytetiDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            qytetiDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Qyteti";
            qytetiDataGridColumnStyleDataGridTextBoxColumn.MappingName = "Qyteti";
            qytetiDataGridColumnStyleDataGridTextBoxColumn.Width = 55;
            // 
            // adresaDataGridColumnStyleDataGridTextBoxColumn
            // 
            adresaDataGridColumnStyleDataGridTextBoxColumn.Format = "";
            adresaDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            adresaDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Adresa";
            adresaDataGridColumnStyleDataGridTextBoxColumn.MappingName = "Adresa";
            adresaDataGridColumnStyleDataGridTextBoxColumn.Width = 60;
            // 
            // testStatDataGridColumnStyleDataGridTextBoxColumn
            // 
            testStatDataGridColumnStyleDataGridTextBoxColumn.Format = "";
            testStatDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            testStatDataGridColumnStyleDataGridTextBoxColumn.MappingName = "TestStat";
            testStatDataGridColumnStyleDataGridTextBoxColumn.Width = 13;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menShitja);
            this.mainMenu1.MenuItems.Add(this.menShfaq);
            this.mainMenu1.MenuItems.Add(this.menDalja);
            // 
            // menShitja
            // 
            this.menShitja.Text = "Shitje";
            this.menShitja.Click += new System.EventHandler(this.menuShitja_Click);
            // 
            // menShfaq
            // 
            this.menShfaq.Text = "Shfaq";
            this.menShfaq.Click += new System.EventHandler(this.menuShfaq_Click);
            // 
            // menDalja
            // 
            this.menDalja.Text = "Dalja";
            this.menDalja.Click += new System.EventHandler(this.menuDalja_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(54, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 15);
            this.label3.Text = "Klientët dhe Faturat";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(0, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(240, 1);
            this.label4.Text = "lblLine";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(49, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(144, 1);
            this.label5.Text = "lblLine";
            // 
            // klientetBindingSource
            // 
            this.klientetBindingSource.DataMember = "Klientet";
            this.klientetBindingSource.DataSource = this.myMobileDataset;
            // 
            // myMobileDataset
            // 
            this.myMobileDataset.DataSetName = "MobileDatabaseDataSet";
            this.myMobileDataset.Prefix = "";
            this.myMobileDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // emriComboBox
            // 
            this.emriComboBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.listaVizitaveBindingSource, "Emri", true));
            this.emriComboBox.DataSource = this.klientetBindingSource;
            this.emriComboBox.DisplayMember = "Emri";
            this.emriComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.emriComboBox.Location = new System.Drawing.Point(57, 31);
            this.emriComboBox.Name = "emriComboBox";
            this.emriComboBox.Size = new System.Drawing.Size(106, 22);
            this.emriComboBox.TabIndex = 14;
            this.emriComboBox.ValueMember = "IDKlienti";
            // 
            // listaVizitaveBindingSource
            // 
            this.listaVizitaveBindingSource.DataMember = "ListaVizitave";
            this.listaVizitaveBindingSource.DataSource = this.myMobileDataset;
            // 
            // listaVizitaveDataGrid
            // 
            this.listaVizitaveDataGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.listaVizitaveDataGrid.DataSource = this.listaVizitaveBindingSource;
            this.listaVizitaveDataGrid.Location = new System.Drawing.Point(0, 60);
            this.listaVizitaveDataGrid.Name = "listaVizitaveDataGrid";
            this.listaVizitaveDataGrid.RowHeadersVisible = false;
            this.listaVizitaveDataGrid.Size = new System.Drawing.Size(237, 229);
            this.listaVizitaveDataGrid.TabIndex = 15;
            this.listaVizitaveDataGrid.TableStyles.Add(this.listaVizitaveTableStyleDataGridTableStyle);
            this.listaVizitaveDataGrid.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listaVizitaveDataGrid_MouseUp);
            this.listaVizitaveDataGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listaVizitaveDataGrid_KeyDown);
            // 
            // listaVizitaveTableStyleDataGridTableStyle
            // 
            this.listaVizitaveTableStyleDataGridTableStyle.GridColumnStyles.Add(lokacioniDataGridColumnStyleDataGridTextBoxColumn);
            this.listaVizitaveTableStyleDataGridTableStyle.GridColumnStyles.Add(adresaDataGridColumnStyleDataGridTextBoxColumn);
            this.listaVizitaveTableStyleDataGridTableStyle.GridColumnStyles.Add(qytetiDataGridColumnStyleDataGridTextBoxColumn);
            this.listaVizitaveTableStyleDataGridTableStyle.GridColumnStyles.Add(this.dataGridTextBoxColumn1);
            this.listaVizitaveTableStyleDataGridTableStyle.GridColumnStyles.Add(testStatDataGridColumnStyleDataGridTextBoxColumn);
            this.listaVizitaveTableStyleDataGridTableStyle.MappingName = "ListaVizitave";
            // 
            // dataGridTextBoxColumn1
            // 
            this.dataGridTextBoxColumn1.Format = "dd-MM-yy";
            this.dataGridTextBoxColumn1.FormatInfo = null;
            this.dataGridTextBoxColumn1.HeaderText = "Data";
            this.dataGridTextBoxColumn1.MappingName = "DataPlanifikimit";
            // 
            // txtIDKlientDheLokacion
            // 
            this.txtIDKlientDheLokacion.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.listaVizitaveBindingSource, "IDKlientDheLokacion", true));
            this.txtIDKlientDheLokacion.Location = new System.Drawing.Point(3, 3);
            this.txtIDKlientDheLokacion.Name = "txtIDKlientDheLokacion";
            this.txtIDKlientDheLokacion.Size = new System.Drawing.Size(10, 21);
            this.txtIDKlientDheLokacion.TabIndex = 17;
            this.txtIDKlientDheLokacion.Visible = false;
            // 
            // lblIDVizita
            // 
            this.lblIDVizita.BackColor = System.Drawing.Color.LightGray;
            this.lblIDVizita.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.listaVizitaveBindingSource, "IDVizita", true));
            this.lblIDVizita.Location = new System.Drawing.Point(19, 3);
            this.lblIDVizita.Name = "lblIDVizita";
            this.lblIDVizita.Size = new System.Drawing.Size(10, 20);
            this.lblIDVizita.Visible = false;
            // 
            // testStatLabel1
            // 
            this.testStatLabel1.BackColor = System.Drawing.Color.Gainsboro;
            this.testStatLabel1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.listaVizitaveBindingSource, "TestStat", true));
            this.testStatLabel1.Location = new System.Drawing.Point(216, 4);
            this.testStatLabel1.Name = "testStatLabel1";
            this.testStatLabel1.Size = new System.Drawing.Size(10, 20);
            this.testStatLabel1.Visible = false;
            // 
            // lfrStatLabel1
            // 
            this.lfrStatLabel1.BackColor = System.Drawing.Color.Gainsboro;
            this.lfrStatLabel1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.listaVizitaveBindingSource, "LfrStat", true));
            this.lfrStatLabel1.Location = new System.Drawing.Point(33, 3);
            this.lfrStatLabel1.Name = "lfrStatLabel1";
            this.lfrStatLabel1.Size = new System.Drawing.Size(10, 20);
            this.lfrStatLabel1.Visible = false;
            // 
            // listaVizitaveTableAdapter
            // 
            this.listaVizitaveTableAdapter.ClearBeforeFill = true;
            // 
            // vizitatTableAdapter1
            // 
            this.vizitatTableAdapter1.ClearBeforeFill = true;
            // 
            // klientetTableAdapter
            // 
            this.klientetTableAdapter.ClearBeforeFill = true;
            // 
            // klientDheLokacionTableAdapter1
            // 
            this.klientDheLokacionTableAdapter1.ClearBeforeFill = true;
            // 
            // frmKlientet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.lfrStatLabel1);
            this.Controls.Add(this.testStatLabel1);
            this.Controls.Add(this.lblIDVizita);
            this.Controls.Add(this.txtIDKlientDheLokacion);
            this.Controls.Add(this.listaVizitaveDataGrid);
            this.Controls.Add(lblKlienti);
            this.Controls.Add(this.emriComboBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Location = new System.Drawing.Point(0, 0);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "frmKlientet";
            this.Text = "Klientet";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Klientet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.klientetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDataset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listaVizitaveBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private MyMobileDataSet myMobileDataset; 
        private System.Windows.Forms.MenuItem menShitja;
        private System.Windows.Forms.MenuItem menShfaq;
        private System.Windows.Forms.MenuItem menDalja;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.BindingSource listaVizitaveBindingSource;
        private MobileSales.MyMobileDataSetTableAdapters.ListaVizitaveTableAdapter listaVizitaveTableAdapter;
        private MobileSales.MyMobileDataSetTableAdapters.VizitatTableAdapter vizitatTableAdapter1;
        private System.Windows.Forms.ComboBox emriComboBox;
        private System.Windows.Forms.DataGrid listaVizitaveDataGrid;
        private System.Windows.Forms.DataGridTableStyle listaVizitaveTableStyleDataGridTableStyle;
        private System.Windows.Forms.TextBox txtIDKlientDheLokacion;
        private System.Windows.Forms.Label lblIDVizita;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn1;
        private System.Windows.Forms.Label testStatLabel1;
        private System.Windows.Forms.Label lfrStatLabel1;
        private System.Windows.Forms.BindingSource klientetBindingSource;
        private MobileSales.MyMobileDataSetTableAdapters.KlientetTableAdapter klientetTableAdapter;
        private MobileSales.MyMobileDataSetTableAdapters.KlientDheLokacionTableAdapter klientDheLokacionTableAdapter1;
    }
}