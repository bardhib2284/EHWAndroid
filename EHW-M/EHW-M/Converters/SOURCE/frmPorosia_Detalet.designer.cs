namespace MobileSales
{
    partial class frmPorosia_Detalet
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
            System.Windows.Forms.DataGridTextBoxColumn nrRendorDataGridColumnStyleDataGridTextBoxColumn;
            System.Windows.Forms.DataGridTextBoxColumn emriDataGridColumnStyleDataGridTextBoxColumn;
            System.Windows.Forms.DataGridTextBoxColumn sasia_PorositurDataGridColumnStyleDataGridTextBoxColumn;
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnShto = new System.Windows.Forms.Button();
            this.dsPorosia = new System.Data.DataSet();
            this.txtSasia = new System.Windows.Forms.TextBox();
            this.orderDetailsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.myMobileDataSet = new MobileSales.MyMobileDataSet();
            this.label3 = new System.Windows.Forms.Label();
            this.txtArtikulli = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuAnulo = new System.Windows.Forms.MenuItem();
            this.menuRegjistro = new System.Windows.Forms.MenuItem();
            this.menuPrint = new System.Windows.Forms.MenuItem();
            this.menuDalja = new System.Windows.Forms.MenuItem();
            this.cmbKlientet = new System.Windows.Forms.ComboBox();
            this.dsKlientet = new System.Data.DataSet();
            this.label4 = new System.Windows.Forms.Label();
            this.lblNrPorosise = new System.Windows.Forms.Label();
            this.order_DetailsDataGrid = new System.Windows.Forms.DataGrid();
            this.order_DetailsTableStyleDataGridTableStyle = new System.Windows.Forms.DataGridTableStyle();
            this.dataGridTextBoxColumn2 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.order_DetailsTableAdapter = new MobileSales.MyMobileDataSetTableAdapters.Order_DetailsTableAdapter();
            this.orderBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.orderTableAdapter = new MobileSales.MyMobileDataSetTableAdapters.OrdersTableAdapter();
            nrRendorDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            emriDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            sasia_PorositurDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dsPorosia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderDetailsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsKlientet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // nrRendorDataGridColumnStyleDataGridTextBoxColumn
            // 
            nrRendorDataGridColumnStyleDataGridTextBoxColumn.Format = "";
            nrRendorDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            nrRendorDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Nr";
            nrRendorDataGridColumnStyleDataGridTextBoxColumn.MappingName = "NrRendor";
            nrRendorDataGridColumnStyleDataGridTextBoxColumn.Width = 20;
            // 
            // emriDataGridColumnStyleDataGridTextBoxColumn
            // 
            emriDataGridColumnStyleDataGridTextBoxColumn.Format = "";
            emriDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            emriDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Emri";
            emriDataGridColumnStyleDataGridTextBoxColumn.MappingName = "Emri";
            emriDataGridColumnStyleDataGridTextBoxColumn.Width = 150;
            // 
            // sasia_PorositurDataGridColumnStyleDataGridTextBoxColumn
            // 
            sasia_PorositurDataGridColumnStyleDataGridTextBoxColumn.Format = "0.00";
            sasia_PorositurDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            sasia_PorositurDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Sasia";
            sasia_PorositurDataGridColumnStyleDataGridTextBoxColumn.MappingName = "Sasia_Porositur";
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.Gainsboro;
            this.btnAdd.Location = new System.Drawing.Point(130, 63);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(23, 22);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "...";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnShto
            // 
            this.btnShto.BackColor = System.Drawing.Color.Gainsboro;
            this.btnShto.Enabled = false;
            this.btnShto.Location = new System.Drawing.Point(193, 62);
            this.btnShto.Name = "btnShto";
            this.btnShto.Size = new System.Drawing.Size(46, 23);
            this.btnShto.TabIndex = 2;
            this.btnShto.Text = "Shto";
            this.btnShto.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dsPorosia
            // 
            this.dsPorosia.DataSetName = "saPorosia";
            this.dsPorosia.Namespace = "";
            this.dsPorosia.Prefix = "";
            this.dsPorosia.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // txtSasia
            // 
            this.txtSasia.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.orderDetailsBindingSource, "Sasia_Porositur", true));
            this.txtSasia.Location = new System.Drawing.Point(155, 63);
            this.txtSasia.Name = "txtSasia";
            this.txtSasia.Size = new System.Drawing.Size(36, 21);
            this.txtSasia.TabIndex = 8;
            this.txtSasia.TextChanged += new System.EventHandler(this.txtSasia_TextChanged);
            this.txtSasia.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSasia_KeyDown);
            // 
            // orderDetailsBindingSource
            // 
            this.orderDetailsBindingSource.DataMember = "Order_Details";
            this.orderDetailsBindingSource.DataSource = this.myMobileDataSet;
            // 
            // myMobileDataSet
            // 
            this.myMobileDataSet.DataSetName = "MyMobileDataSet";
            this.myMobileDataSet.Prefix = "";
            this.myMobileDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(2, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 15);
            this.label3.Text = "Artikulli";
            // 
            // txtArtikulli
            // 
            this.txtArtikulli.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.orderDetailsBindingSource, "Emri", true));
            this.txtArtikulli.Enabled = false;
            this.txtArtikulli.Location = new System.Drawing.Point(2, 63);
            this.txtArtikulli.Name = "txtArtikulli";
            this.txtArtikulli.ReadOnly = true;
            this.txtArtikulli.Size = new System.Drawing.Size(126, 21);
            this.txtArtikulli.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(156, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 15);
            this.label2.Text = "Sasia";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 20);
            this.label1.Text = "Klienti";
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuAnulo);
            this.mainMenu1.MenuItems.Add(this.menuRegjistro);
            this.mainMenu1.MenuItems.Add(this.menuPrint);
            this.mainMenu1.MenuItems.Add(this.menuDalja);
            // 
            // menuAnulo
            // 
            this.menuAnulo.Text = "Anulo";
            this.menuAnulo.Click += new System.EventHandler(this.menuAnulo_Click);
            // 
            // menuRegjistro
            // 
            this.menuRegjistro.Text = "Regjistro";
            this.menuRegjistro.Click += new System.EventHandler(this.menuRegjistro_Click);
            // 
            // menuPrint
            // 
            this.menuPrint.Text = "Printo";
            this.menuPrint.Click += new System.EventHandler(this.menuPrint_Click);
            // 
            // menuDalja
            // 
            this.menuDalja.Text = "Dalja";
            this.menuDalja.Click += new System.EventHandler(this.menuDalja_Click);
            // 
            // cmbKlientet
            // 
            this.cmbKlientet.Location = new System.Drawing.Point(83, 19);
            this.cmbKlientet.Name = "cmbKlientet";
            this.cmbKlientet.Size = new System.Drawing.Size(157, 22);
            this.cmbKlientet.TabIndex = 12;
            // 
            // dsKlientet
            // 
            this.dsKlientet.DataSetName = "NewDataSet";
            this.dsKlientet.Namespace = "";
            this.dsKlientet.Prefix = "";
            this.dsKlientet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 20);
            this.label4.Text = "Nr.Porosisë";
            // 
            // lblNrPorosise
            // 
            this.lblNrPorosise.Location = new System.Drawing.Point(81, 1);
            this.lblNrPorosise.Name = "lblNrPorosise";
            this.lblNrPorosise.Size = new System.Drawing.Size(158, 17);
            this.lblNrPorosise.Text = "0";
            this.lblNrPorosise.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // order_DetailsDataGrid
            // 
            this.order_DetailsDataGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.order_DetailsDataGrid.DataSource = this.orderDetailsBindingSource;
            this.order_DetailsDataGrid.Location = new System.Drawing.Point(0, 88);
            this.order_DetailsDataGrid.Name = "order_DetailsDataGrid";
            this.order_DetailsDataGrid.RowHeadersVisible = false;
            this.order_DetailsDataGrid.Size = new System.Drawing.Size(242, 198);
            this.order_DetailsDataGrid.TabIndex = 21;
            this.order_DetailsDataGrid.TableStyles.Add(this.order_DetailsTableStyleDataGridTableStyle);
            this.order_DetailsDataGrid.MouseUp += new System.Windows.Forms.MouseEventHandler(this.order_DetailsDataGrid_MouseUp);
            this.order_DetailsDataGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.order_DetailsDataGrid_KeyDown);
            // 
            // order_DetailsTableStyleDataGridTableStyle
            // 
            this.order_DetailsTableStyleDataGridTableStyle.GridColumnStyles.Add(nrRendorDataGridColumnStyleDataGridTextBoxColumn);
            this.order_DetailsTableStyleDataGridTableStyle.GridColumnStyles.Add(this.dataGridTextBoxColumn2);
            this.order_DetailsTableStyleDataGridTableStyle.GridColumnStyles.Add(emriDataGridColumnStyleDataGridTextBoxColumn);
            this.order_DetailsTableStyleDataGridTableStyle.GridColumnStyles.Add(sasia_PorositurDataGridColumnStyleDataGridTextBoxColumn);
            this.order_DetailsTableStyleDataGridTableStyle.GridColumnStyles.Add(this.dataGridTextBoxColumn1);
            this.order_DetailsTableStyleDataGridTableStyle.MappingName = "Order_Details";
            // 
            // dataGridTextBoxColumn2
            // 
            this.dataGridTextBoxColumn2.Format = "";
            this.dataGridTextBoxColumn2.FormatInfo = null;
            this.dataGridTextBoxColumn2.HeaderText = "Shifra";
            this.dataGridTextBoxColumn2.MappingName = "IDArtikulli";
            this.dataGridTextBoxColumn2.Width = 40;
            // 
            // dataGridTextBoxColumn1
            // 
            this.dataGridTextBoxColumn1.Format = "";
            this.dataGridTextBoxColumn1.FormatInfo = null;
            this.dataGridTextBoxColumn1.HeaderText = "Njësia";
            this.dataGridTextBoxColumn1.MappingName = "BUM";
            this.dataGridTextBoxColumn1.Width = 40;
            // 
            // order_DetailsTableAdapter
            // 
            this.order_DetailsTableAdapter.ClearBeforeFill = true;
            // 
            // orderBindingSource
            // 
            this.orderBindingSource.DataMember = "Orders";
            this.orderBindingSource.DataSource = this.myMobileDataSet;
            // 
            // orderTableAdapter
            // 
            this.orderTableAdapter.ClearBeforeFill = true;
            // 
            // frmPorosia_Detalet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(240, 290);
            this.ControlBox = false;
            this.Controls.Add(this.order_DetailsDataGrid);
            this.Controls.Add(this.lblNrPorosise);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbKlientet);
            this.Controls.Add(this.txtSasia);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnShto);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtArtikulli);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Location = new System.Drawing.Point(0, 0);
            this.Menu = this.mainMenu1;
            this.Name = "frmPorosia_Detalet";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmPorosia_Detalet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dsPorosia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderDetailsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsKlientet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Data.DataSet dsPorosia;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuDalja;
        private System.Windows.Forms.MenuItem menuRegjistro;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbKlientet;
        private System.Windows.Forms.MenuItem menuAnulo;
        private System.Data.DataSet dsKlientet;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblNrPorosise;
        private MyMobileDataSet myMobileDataSet;
        private MobileSales.MyMobileDataSetTableAdapters.Order_DetailsTableAdapter order_DetailsTableAdapter;
        public System.Windows.Forms.Button btnShto;
        public System.Windows.Forms.TextBox txtSasia;
        public System.Windows.Forms.BindingSource orderDetailsBindingSource;
        public System.Windows.Forms.Button btnAdd;
        public System.Windows.Forms.TextBox txtArtikulli;
        private System.Windows.Forms.DataGridTableStyle order_DetailsTableStyleDataGridTableStyle;
        public System.Windows.Forms.DataGrid order_DetailsDataGrid;
        private System.Windows.Forms.BindingSource orderBindingSource;
        private MobileSales.MyMobileDataSetTableAdapters.OrdersTableAdapter orderTableAdapter;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn1;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn2;
        private System.Windows.Forms.MenuItem menuPrint;
    }
}