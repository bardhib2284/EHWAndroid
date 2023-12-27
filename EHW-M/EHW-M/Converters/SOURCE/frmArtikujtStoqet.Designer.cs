namespace MobileSales
{
    partial class frmArtikujtStoqet
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
            System.Windows.Forms.DataGridTextBoxColumn emriDataGridColumnStyleDataGridTextBoxColumn;
            System.Windows.Forms.DataGridTextBoxColumn cmimiNjesiDataGridColumnStyleDataGridTextBoxColumn;
            System.Windows.Forms.DataGridTextBoxColumn sasiaDataGridColumnStyleDataGridTextBoxColumn;
            System.Windows.Forms.Label bUMLabel;
            this.txtArtFilter = new System.Windows.Forms.TextBox();
            this.artikujtStoqetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.myMobileDataSet = new MobileSales.MyMobileDataSet();
            this.artikujtStoqetDataGrid = new System.Windows.Forms.DataGrid();
            this.artikujtStoqetTableStyleDataGridTableStyle = new System.Windows.Forms.DataGridTableStyle();
            this.dataGridTextBoxColumn2 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.lblIDArtikulli = new System.Windows.Forms.Label();
            this.lblEmri = new System.Windows.Forms.Label();
            this.lblCmimi = new System.Windows.Forms.Label();
            this.lblMaxSasia = new System.Windows.Forms.Label();
            this.lblRabati = new System.Windows.Forms.Label();
            this.lblGratis = new System.Windows.Forms.Label();
            this.lblSasiaPako = new System.Windows.Forms.Label();
            this.lblPriceWithRabat = new System.Windows.Forms.Label();
            this.lblIDArtikulli2 = new System.Windows.Forms.Label();
            this.lblFilter = new System.Windows.Forms.Label();
            this.lblBUM = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSum = new System.Windows.Forms.Label();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menu_Close = new System.Windows.Forms.MenuItem();
            this.menu_Open = new System.Windows.Forms.MenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSeri = new System.Windows.Forms.Label();
            this.artikujtStoqetTableAdapter = new MobileSales.MyMobileDataSetTableAdapters.ArtikujtStoqetTableAdapter();
            this.artikujtDepoTableAdapter = new MobileSales.MyMobileDataSetTableAdapters.ArtikujDepoTableAdapter();
            emriDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            cmimiNjesiDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            sasiaDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            bUMLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.artikujtStoqetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDataSet)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // emriDataGridColumnStyleDataGridTextBoxColumn
            // 
            emriDataGridColumnStyleDataGridTextBoxColumn.Format = "";
            emriDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            emriDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Artikulli";
            emriDataGridColumnStyleDataGridTextBoxColumn.MappingName = "Emri";
            emriDataGridColumnStyleDataGridTextBoxColumn.Width = 123;
            // 
            // cmimiNjesiDataGridColumnStyleDataGridTextBoxColumn
            // 
            cmimiNjesiDataGridColumnStyleDataGridTextBoxColumn.Format = "0.00";
            cmimiNjesiDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            cmimiNjesiDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Cmimi";
            cmimiNjesiDataGridColumnStyleDataGridTextBoxColumn.MappingName = "UnitPrice";
            cmimiNjesiDataGridColumnStyleDataGridTextBoxColumn.Width = 36;
            // 
            // sasiaDataGridColumnStyleDataGridTextBoxColumn
            // 
            sasiaDataGridColumnStyleDataGridTextBoxColumn.Format = "0.000";
            sasiaDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            sasiaDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Sasia";
            sasiaDataGridColumnStyleDataGridTextBoxColumn.MappingName = "Sasia";
            sasiaDataGridColumnStyleDataGridTextBoxColumn.Width = 35;
            // 
            // bUMLabel
            // 
            bUMLabel.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            bUMLabel.Location = new System.Drawing.Point(62, 130);
            bUMLabel.Name = "bUMLabel";
            bUMLabel.Size = new System.Drawing.Size(45, 17);
            bUMLabel.Text = "BUM:";
            bUMLabel.Visible = false;
            // 
            // txtArtFilter
            // 
            this.txtArtFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtArtFilter.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.txtArtFilter.Location = new System.Drawing.Point(37, 0);
            this.txtArtFilter.Name = "txtArtFilter";
            this.txtArtFilter.Size = new System.Drawing.Size(203, 19);
            this.txtArtFilter.TabIndex = 2;
            this.txtArtFilter.TextChanged += new System.EventHandler(this.txtArtFilter_TextChanged);
            this.txtArtFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtArtFilter_KeyDown);
            // 
            // artikujtStoqetBindingSource
            // 
            this.artikujtStoqetBindingSource.DataMember = "ArtikujtStoqet";
            this.artikujtStoqetBindingSource.DataSource = this.myMobileDataSet;
            // 
            // myMobileDataSet
            // 
            this.myMobileDataSet.DataSetName = "MyMobileDataSet";
            this.myMobileDataSet.Prefix = "";
            this.myMobileDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // artikujtStoqetDataGrid
            // 
            this.artikujtStoqetDataGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.artikujtStoqetDataGrid.DataSource = this.artikujtStoqetBindingSource;
            this.artikujtStoqetDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.artikujtStoqetDataGrid.Location = new System.Drawing.Point(0, 22);
            this.artikujtStoqetDataGrid.Name = "artikujtStoqetDataGrid";
            this.artikujtStoqetDataGrid.RowHeadersVisible = false;
            this.artikujtStoqetDataGrid.Size = new System.Drawing.Size(240, 252);
            this.artikujtStoqetDataGrid.TabIndex = 6;
            this.artikujtStoqetDataGrid.TableStyles.Add(this.artikujtStoqetTableStyleDataGridTableStyle);
            this.artikujtStoqetDataGrid.MouseUp += new System.Windows.Forms.MouseEventHandler(this.artikujtStoqetDataGrid_MouseUp);
            this.artikujtStoqetDataGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.artikujtStoqetDataGrid_KeyDown);
            // 
            // artikujtStoqetTableStyleDataGridTableStyle
            // 
            this.artikujtStoqetTableStyleDataGridTableStyle.GridColumnStyles.Add(emriDataGridColumnStyleDataGridTextBoxColumn);
            this.artikujtStoqetTableStyleDataGridTableStyle.GridColumnStyles.Add(sasiaDataGridColumnStyleDataGridTextBoxColumn);
            this.artikujtStoqetTableStyleDataGridTableStyle.GridColumnStyles.Add(cmimiNjesiDataGridColumnStyleDataGridTextBoxColumn);
            this.artikujtStoqetTableStyleDataGridTableStyle.GridColumnStyles.Add(this.dataGridTextBoxColumn2);
            this.artikujtStoqetTableStyleDataGridTableStyle.GridColumnStyles.Add(this.dataGridTextBoxColumn1);
            this.artikujtStoqetTableStyleDataGridTableStyle.MappingName = "ArtikujtStoqet";
            // 
            // dataGridTextBoxColumn2
            // 
            this.dataGridTextBoxColumn2.Format = "";
            this.dataGridTextBoxColumn2.FormatInfo = null;
            this.dataGridTextBoxColumn2.HeaderText = "Seri";
            this.dataGridTextBoxColumn2.MappingName = "Seri";
            // 
            // dataGridTextBoxColumn1
            // 
            this.dataGridTextBoxColumn1.Format = "";
            this.dataGridTextBoxColumn1.FormatInfo = null;
            this.dataGridTextBoxColumn1.HeaderText = "Dhuratë";
            this.dataGridTextBoxColumn1.MappingName = "Dhurate";
            this.dataGridTextBoxColumn1.Width = 43;
            // 
            // lblIDArtikulli
            // 
            this.lblIDArtikulli.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lblIDArtikulli.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.artikujtStoqetBindingSource, "IDArtikulli", true));
            this.lblIDArtikulli.Location = new System.Drawing.Point(133, 143);
            this.lblIDArtikulli.Name = "lblIDArtikulli";
            this.lblIDArtikulli.Size = new System.Drawing.Size(8, 20);
            this.lblIDArtikulli.Visible = false;
            // 
            // lblEmri
            // 
            this.lblEmri.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lblEmri.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.artikujtStoqetBindingSource, "Emri", true));
            this.lblEmri.Location = new System.Drawing.Point(50, 161);
            this.lblEmri.Name = "lblEmri";
            this.lblEmri.Size = new System.Drawing.Size(57, 20);
            this.lblEmri.Visible = false;
            // 
            // lblCmimi
            // 
            this.lblCmimi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lblCmimi.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.artikujtStoqetBindingSource, "UnitPrice", true));
            this.lblCmimi.Location = new System.Drawing.Point(182, 110);
            this.lblCmimi.Name = "lblCmimi";
            this.lblCmimi.Size = new System.Drawing.Size(6, 20);
            this.lblCmimi.Visible = false;
            // 
            // lblMaxSasia
            // 
            this.lblMaxSasia.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lblMaxSasia.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.artikujtStoqetBindingSource, "Sasia", true));
            this.lblMaxSasia.Location = new System.Drawing.Point(58, 110);
            this.lblMaxSasia.Name = "lblMaxSasia";
            this.lblMaxSasia.Size = new System.Drawing.Size(20, 20);
            this.lblMaxSasia.Visible = false;
            // 
            // lblRabati
            // 
            this.lblRabati.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lblRabati.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.artikujtStoqetBindingSource, "Rabatet", true));
            this.lblRabati.Location = new System.Drawing.Point(120, 143);
            this.lblRabati.Name = "lblRabati";
            this.lblRabati.Size = new System.Drawing.Size(7, 20);
            this.lblRabati.Visible = false;
            // 
            // lblGratis
            // 
            this.lblGratis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lblGratis.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.artikujtStoqetBindingSource, "Dhurate", true));
            this.lblGratis.Location = new System.Drawing.Point(116, 150);
            this.lblGratis.Name = "lblGratis";
            this.lblGratis.Size = new System.Drawing.Size(15, 20);
            this.lblGratis.Visible = false;
            // 
            // lblSasiaPako
            // 
            this.lblSasiaPako.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lblSasiaPako.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.artikujtStoqetBindingSource, "Paketimi", true));
            this.lblSasiaPako.Location = new System.Drawing.Point(130, 181);
            this.lblSasiaPako.Name = "lblSasiaPako";
            this.lblSasiaPako.Size = new System.Drawing.Size(6, 20);
            this.lblSasiaPako.Visible = false;
            // 
            // lblPriceWithRabat
            // 
            this.lblPriceWithRabat.BackColor = System.Drawing.Color.LightGray;
            this.lblPriceWithRabat.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.artikujtStoqetBindingSource, "Totali", true));
            this.lblPriceWithRabat.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblPriceWithRabat.Location = new System.Drawing.Point(133, 163);
            this.lblPriceWithRabat.Name = "lblPriceWithRabat";
            this.lblPriceWithRabat.Size = new System.Drawing.Size(10, 18);
            this.lblPriceWithRabat.Visible = false;
            // 
            // lblIDArtikulli2
            // 
            this.lblIDArtikulli2.BackColor = System.Drawing.Color.Gainsboro;
            this.lblIDArtikulli2.Location = new System.Drawing.Point(82, 110);
            this.lblIDArtikulli2.Name = "lblIDArtikulli2";
            this.lblIDArtikulli2.Size = new System.Drawing.Size(6, 20);
            this.lblIDArtikulli2.Visible = false;
            // 
            // lblFilter
            // 
            this.lblFilter.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblFilter.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular);
            this.lblFilter.Location = new System.Drawing.Point(0, 0);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(37, 20);
            this.lblFilter.Text = "Filtër ";
            // 
            // lblBUM
            // 
            this.lblBUM.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.artikujtStoqetBindingSource, "BUM", true));
            this.lblBUM.Location = new System.Drawing.Point(114, 130);
            this.lblBUM.Name = "lblBUM";
            this.lblBUM.Size = new System.Drawing.Size(100, 20);
            this.lblBUM.Visible = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 20);
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblSum
            // 
            this.lblSum.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblSum.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblSum.Location = new System.Drawing.Point(120, 0);
            this.lblSum.Name = "lblSum";
            this.lblSum.Size = new System.Drawing.Size(120, 20);
            this.lblSum.Text = "0.0";
            this.lblSum.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menu_Close);
            this.mainMenu1.MenuItems.Add(this.menu_Open);
            // 
            // menu_Close
            // 
            this.menu_Close.Text = "Dalja";
            this.menu_Close.Click += new System.EventHandler(this.menu_Close_Click);
            // 
            // menu_Open
            // 
            this.menu_Open.Text = "Shfaq";
            this.menu_Open.Click += new System.EventHandler(this.menu_Open_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtArtFilter);
            this.panel1.Controls.Add(this.lblFilter);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(240, 20);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.lblSum);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 274);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(240, 20);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(240, 2);
            // 
            // lblSeri
            // 
            this.lblSeri.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lblSeri.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.artikujtStoqetBindingSource, "Seri", true));
            this.lblSeri.Location = new System.Drawing.Point(150, 226);
            this.lblSeri.Name = "lblSeri";
            this.lblSeri.Size = new System.Drawing.Size(38, 31);
            this.lblSeri.Visible = false;
            // 
            // artikujtStoqetTableAdapter
            // 
            this.artikujtStoqetTableAdapter.ClearBeforeFill = true;
            // 
            // artikujtDepoTableAdapter
            // 
            this.artikujtDepoTableAdapter.ClearBeforeFill = true;
            // 
            // frmArtikujtStoqet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.artikujtStoqetDataGrid);
            this.Controls.Add(this.lblSeri);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblIDArtikulli2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblPriceWithRabat);
            this.Controls.Add(this.lblSasiaPako);
            this.Controls.Add(this.lblGratis);
            this.Controls.Add(this.lblRabati);
            this.Controls.Add(this.lblMaxSasia);
            this.Controls.Add(this.lblCmimi);
            this.Controls.Add(this.lblEmri);
            this.Controls.Add(this.lblIDArtikulli);
            this.Controls.Add(bUMLabel);
            this.Controls.Add(this.lblBUM);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(0, 0);
            this.Menu = this.mainMenu1;
            this.Name = "frmArtikujtStoqet";
            this.Text = "ArtikujtStoqet";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ArtikujtStoqet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.artikujtStoqetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDataSet)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MyMobileDataSet myMobileDataSet;
        private System.Windows.Forms.BindingSource artikujtStoqetBindingSource;
        private MobileSales.MyMobileDataSetTableAdapters.ArtikujtStoqetTableAdapter artikujtStoqetTableAdapter;
        private MobileSales.MyMobileDataSetTableAdapters.ArtikujDepoTableAdapter artikujtDepoTableAdapter;
        private System.Windows.Forms.DataGrid artikujtStoqetDataGrid;
        private System.Windows.Forms.DataGridTableStyle artikujtStoqetTableStyleDataGridTableStyle;
        private System.Windows.Forms.Label lblIDArtikulli;
        private System.Windows.Forms.Label lblEmri;
        private System.Windows.Forms.Label lblCmimi;
        private System.Windows.Forms.Label lblMaxSasia;
        private System.Windows.Forms.Label lblRabati;
        private System.Windows.Forms.Label lblGratis;
        private System.Windows.Forms.Label lblSasiaPako;
        private System.Windows.Forms.Label lblPriceWithRabat;
        //private System.Windows.Forms.BindingSource ArtikujtDepoBindingSource;
        private System.Windows.Forms.Label lblIDArtikulli2;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn1;
        private System.Windows.Forms.Label lblBUM;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSum;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menu_Close;
        private System.Windows.Forms.MenuItem menu_Open;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox txtArtFilter;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn2;
        private System.Windows.Forms.Label lblSeri;
    }
}