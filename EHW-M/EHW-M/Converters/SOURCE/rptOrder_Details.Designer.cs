namespace MobileSales
{
    partial class rptOrder_Details
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
            System.Windows.Forms.DataGridTextBoxColumn iDArtikulliDataGridColumnStyleDataGridTextBoxColumn;
            System.Windows.Forms.DataGridTextBoxColumn emriDataGridColumnStyleDataGridTextBoxColumn;
            System.Windows.Forms.DataGridTextBoxColumn sasiaDataGridColumnStyleDataGridTextBoxColumn;
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.myMobileDataSet = new MobileSales.MyMobileDataSet();
            this.rptOrder_DetailsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.rptOrder_DetailsTableAdapter = new MobileSales.MyMobileDataSetTableAdapters.rptOrder_DetailsTableAdapter();
            this.rptOrder_DetailsDataGrid = new System.Windows.Forms.DataGrid();
            this.rptOrder_DetailsTableStyleDataGridTableStyle = new System.Windows.Forms.DataGridTableStyle();
            iDArtikulliDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            emriDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            sasiaDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rptOrder_DetailsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // iDArtikulliDataGridColumnStyleDataGridTextBoxColumn
            // 
            iDArtikulliDataGridColumnStyleDataGridTextBoxColumn.Format = "";
            iDArtikulliDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            iDArtikulliDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Shifra";
            iDArtikulliDataGridColumnStyleDataGridTextBoxColumn.MappingName = "IDArtikulli";
            iDArtikulliDataGridColumnStyleDataGridTextBoxColumn.Width = 80;
            // 
            // emriDataGridColumnStyleDataGridTextBoxColumn
            // 
            emriDataGridColumnStyleDataGridTextBoxColumn.Format = "";
            emriDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            emriDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Artikulli";
            emriDataGridColumnStyleDataGridTextBoxColumn.MappingName = "Emri";
            emriDataGridColumnStyleDataGridTextBoxColumn.Width = 80;
            // 
            // sasiaDataGridColumnStyleDataGridTextBoxColumn
            // 
            sasiaDataGridColumnStyleDataGridTextBoxColumn.Format = "0.00";
            sasiaDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            sasiaDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Sasia";
            sasiaDataGridColumnStyleDataGridTextBoxColumn.MappingName = "Sasia";
            sasiaDataGridColumnStyleDataGridTextBoxColumn.Width = 80;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            // 
            // menuItem1
            // 
            this.menuItem1.Text = "Dalja";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // myMobileDataSet
            // 
            this.myMobileDataSet.DataSetName = "MyMobileDataSet";
            this.myMobileDataSet.Prefix = "";
            this.myMobileDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // rptOrder_DetailsBindingSource
            // 
            this.rptOrder_DetailsBindingSource.DataMember = "rptOrder_Details";
            this.rptOrder_DetailsBindingSource.DataSource = this.myMobileDataSet;
            // 
            // rptOrder_DetailsTableAdapter
            // 
            this.rptOrder_DetailsTableAdapter.ClearBeforeFill = true;
            // 
            // rptOrder_DetailsDataGrid
            // 
            this.rptOrder_DetailsDataGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.rptOrder_DetailsDataGrid.DataSource = this.rptOrder_DetailsBindingSource;
            this.rptOrder_DetailsDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rptOrder_DetailsDataGrid.Location = new System.Drawing.Point(0, 0);
            this.rptOrder_DetailsDataGrid.Name = "rptOrder_DetailsDataGrid";
            this.rptOrder_DetailsDataGrid.RowHeadersVisible = false;
            this.rptOrder_DetailsDataGrid.Size = new System.Drawing.Size(240, 295);
            this.rptOrder_DetailsDataGrid.TabIndex = 1;
            this.rptOrder_DetailsDataGrid.TableStyles.Add(this.rptOrder_DetailsTableStyleDataGridTableStyle);
            // 
            // rptOrder_DetailsTableStyleDataGridTableStyle
            // 
            this.rptOrder_DetailsTableStyleDataGridTableStyle.GridColumnStyles.Add(iDArtikulliDataGridColumnStyleDataGridTextBoxColumn);
            this.rptOrder_DetailsTableStyleDataGridTableStyle.GridColumnStyles.Add(emriDataGridColumnStyleDataGridTextBoxColumn);
            this.rptOrder_DetailsTableStyleDataGridTableStyle.GridColumnStyles.Add(sasiaDataGridColumnStyleDataGridTextBoxColumn);
            this.rptOrder_DetailsTableStyleDataGridTableStyle.MappingName = "rptOrder_Details";
            // 
            // rptOrder_Details
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 295);
            this.ControlBox = false;
            this.Controls.Add(this.rptOrder_DetailsDataGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(0, 0);
            this.Menu = this.mainMenu1;
            this.Name = "rptOrder_Details";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.rptOrder_Details_Load);
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rptOrder_DetailsBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuItem1;
        private MyMobileDataSet myMobileDataSet;
        private System.Windows.Forms.BindingSource rptOrder_DetailsBindingSource;
        private MobileSales.MyMobileDataSetTableAdapters.rptOrder_DetailsTableAdapter rptOrder_DetailsTableAdapter;
        private System.Windows.Forms.DataGrid rptOrder_DetailsDataGrid;
        private System.Windows.Forms.DataGridTableStyle rptOrder_DetailsTableStyleDataGridTableStyle;
    }
}