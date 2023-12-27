namespace MobileSales
{
    partial class frmPorosite
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
            System.Windows.Forms.DataGridTextBoxColumn iDOrderDataGridColumnStyleDataGridTextBoxColumn;
            System.Windows.Forms.DataGridTextBoxColumn dataDataGridColumnStyleDataGridTextBoxColumn;
            System.Windows.Forms.DataGridTextBoxColumn syncStatusDataGridColumnStyleDataGridTextBoxColumn;
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuDalja = new System.Windows.Forms.MenuItem();
            this.menuPorosite = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuKonvert = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menu_New = new System.Windows.Forms.MenuItem();
            this.menu_Edit = new System.Windows.Forms.MenuItem();
            this.dsPorosia = new System.Data.DataSet();
            this.myMobileDataSet = new MobileSales.MyMobileDataSet();
            this.porositeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.porositeTableAdapter = new MobileSales.MyMobileDataSetTableAdapters.PorositeTableAdapter();
            this.orderTableAdapter = new MobileSales.MyMobileDataSetTableAdapters.OrdersTableAdapter();
            this.orderBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.orderDataGrid = new System.Windows.Forms.DataGrid();
            this.orderTableStyleDataGridTableStyle = new System.Windows.Forms.DataGridTableStyle();
            this.dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.lblData = new System.Windows.Forms.Label();
            iDOrderDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            dataDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            syncStatusDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dsPorosia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.porositeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // iDOrderDataGridColumnStyleDataGridTextBoxColumn
            // 
            iDOrderDataGridColumnStyleDataGridTextBoxColumn.Format = "";
            iDOrderDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            iDOrderDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Porosia";
            iDOrderDataGridColumnStyleDataGridTextBoxColumn.MappingName = "IDOrder";
            iDOrderDataGridColumnStyleDataGridTextBoxColumn.Width = 110;
            // 
            // dataDataGridColumnStyleDataGridTextBoxColumn
            // 
            dataDataGridColumnStyleDataGridTextBoxColumn.Format = "dd/MM/yyyy";
            dataDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            dataDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Data e porosis";
            dataDataGridColumnStyleDataGridTextBoxColumn.MappingName = "Data";
            dataDataGridColumnStyleDataGridTextBoxColumn.Width = 90;
            // 
            // syncStatusDataGridColumnStyleDataGridTextBoxColumn
            // 
            syncStatusDataGridColumnStyleDataGridTextBoxColumn.Format = "";
            syncStatusDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            syncStatusDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Sync";
            syncStatusDataGridColumnStyleDataGridTextBoxColumn.MappingName = "SyncStatus";
            syncStatusDataGridColumnStyleDataGridTextBoxColumn.Width = 30;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuDalja);
            this.mainMenu1.MenuItems.Add(this.menuPorosite);
            this.mainMenu1.MenuItems.Add(this.menuKonvert);
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            this.mainMenu1.MenuItems.Add(this.menu_New);
            this.mainMenu1.MenuItems.Add(this.menu_Edit);
            // 
            // menuDalja
            // 
            this.menuDalja.Text = "Dalja";
            this.menuDalja.Click += new System.EventHandler(this.menuDalja_Click);
            // 
            // menuPorosite
            // 
            this.menuPorosite.MenuItems.Add(this.menuItem2);
            this.menuPorosite.MenuItems.Add(this.menuItem3);
            this.menuPorosite.Text = "Vizualizo";
            this.menuPorosite.Click += new System.EventHandler(this.menuPorosite_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Text = "Të gjithat";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Text = "Detalet";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuKonvert
            // 
            this.menuKonvert.Text = "Faturo";
            this.menuKonvert.Click += new System.EventHandler(this.menuKonvert_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Text = "Krijo listë";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menu_New
            // 
            this.menu_New.Text = "Shto";
            this.menu_New.Click += new System.EventHandler(this.menu_New_Click);
            // 
            // menu_Edit
            // 
            this.menu_Edit.Text = "Edito";
            this.menu_Edit.Click += new System.EventHandler(this.menu_Edit_Click);
            // 
            // dsPorosia
            // 
            this.dsPorosia.DataSetName = "NewDataSet";
            this.dsPorosia.Namespace = "";
            this.dsPorosia.Prefix = "";
            this.dsPorosia.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // myMobileDataSet
            // 
            this.myMobileDataSet.DataSetName = "MyMobileDataSet";
            this.myMobileDataSet.Prefix = "";
            this.myMobileDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // porositeBindingSource
            // 
            this.porositeBindingSource.DataMember = "Porosite";
            this.porositeBindingSource.DataSource = this.myMobileDataSet;
            // 
            // porositeTableAdapter
            // 
            this.porositeTableAdapter.ClearBeforeFill = true;
            // 
            // orderTableAdapter
            // 
            this.orderTableAdapter.ClearBeforeFill = true;
            // 
            // orderBindingSource
            // 
            this.orderBindingSource.DataMember = "Orders";
            this.orderBindingSource.DataSource = this.myMobileDataSet;
            // 
            // orderDataGrid
            // 
            this.orderDataGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.orderDataGrid.DataSource = this.orderBindingSource;
            this.orderDataGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.orderDataGrid.Location = new System.Drawing.Point(0, 20);
            this.orderDataGrid.Name = "orderDataGrid";
            this.orderDataGrid.RowHeadersVisible = false;
            this.orderDataGrid.Size = new System.Drawing.Size(240, 267);
            this.orderDataGrid.TabIndex = 15;
            this.orderDataGrid.TableStyles.Add(this.orderTableStyleDataGridTableStyle);
            this.orderDataGrid.MouseUp += new System.Windows.Forms.MouseEventHandler(this.orderDataGrid_MouseUp);
            this.orderDataGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.orderDataGrid_KeyDown);
            // 
            // orderTableStyleDataGridTableStyle
            // 
            this.orderTableStyleDataGridTableStyle.GridColumnStyles.Add(iDOrderDataGridColumnStyleDataGridTextBoxColumn);
            this.orderTableStyleDataGridTableStyle.GridColumnStyles.Add(dataDataGridColumnStyleDataGridTextBoxColumn);
            this.orderTableStyleDataGridTableStyle.GridColumnStyles.Add(syncStatusDataGridColumnStyleDataGridTextBoxColumn);
            this.orderTableStyleDataGridTableStyle.GridColumnStyles.Add(this.dataGridTextBoxColumn1);
            this.orderTableStyleDataGridTableStyle.MappingName = "Orders";
            // 
            // dataGridTextBoxColumn1
            // 
            this.dataGridTextBoxColumn1.Format = "";
            this.dataGridTextBoxColumn1.FormatInfo = null;
            this.dataGridTextBoxColumn1.HeaderText = "IDKlientDheLokacion";
            this.dataGridTextBoxColumn1.MappingName = "IDKlientDheLokacion";
            this.dataGridTextBoxColumn1.Width = 0;
            // 
            // lblData
            // 
            this.lblData.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblData.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular);
            this.lblData.Location = new System.Drawing.Point(0, 0);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(240, 20);
            this.lblData.Text = "12/12/2009";
            // 
            // frmPorosite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(240, 290);
            this.ControlBox = false;
            this.Controls.Add(this.orderDataGrid);
            this.Controls.Add(this.lblData);
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.Menu = this.mainMenu1;
            this.Name = "frmPorosite";
            this.Text = "Porosite";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmPorosite_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPorosite_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dsPorosia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.porositeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuDalja;
        private System.Data.DataSet dsPorosia;
        private MyMobileDataSet myMobileDataSet;
        private System.Windows.Forms.BindingSource porositeBindingSource;
        private MobileSales.MyMobileDataSetTableAdapters.PorositeTableAdapter porositeTableAdapter;
        private MobileSales.MyMobileDataSetTableAdapters.OrdersTableAdapter orderTableAdapter;
        private System.Windows.Forms.BindingSource orderBindingSource;
        private System.Windows.Forms.DataGridTableStyle orderTableStyleDataGridTableStyle;
        private System.Windows.Forms.DataGrid orderDataGrid;
        private System.Windows.Forms.MenuItem menuPorosite;
        private System.Windows.Forms.MenuItem menuKonvert;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.Label lblData;
        private System.Windows.Forms.MenuItem menu_New;
        private System.Windows.Forms.MenuItem menu_Edit;
    }
}