namespace MobileSales
{
    partial class frmStoku
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
            System.Windows.Forms.DataGridTextBoxColumn emriDataGridColumnStyleDataGridTextBoxColumn;
            System.Windows.Forms.DataGridTextBoxColumn sasiaDataGridColumnStyleDataGridTextBoxColumn;
            System.Windows.Forms.DataGridTextBoxColumn iDArtikulliDataGridColumnStyleDataGridTextBoxColumn;
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menDalja = new System.Windows.Forms.MenuItem();
            this.artikujDepoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.myMobileDataSet = new MobileSales.MyMobileDataSet();
            this.artikujDepoDataGrid = new System.Windows.Forms.DataGrid();
            this.artikujDepoTableStyleDataGridTableStyle = new System.Windows.Forms.DataGridTableStyle();
            this.txtArtikulli = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.artikujDepoTableAdapter = new MobileSales.MyMobileDataSetTableAdapters.ArtikujDepoTableAdapter();
            this.lblSum = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
            emriDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            sasiaDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            iDArtikulliDataGridColumnStyleDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.artikujDepoBindingSource)).BeginInit();
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
            emriDataGridColumnStyleDataGridTextBoxColumn.Width = 172;
            // 
            // sasiaDataGridColumnStyleDataGridTextBoxColumn
            // 
            sasiaDataGridColumnStyleDataGridTextBoxColumn.Format = "0.000";
            sasiaDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            sasiaDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Sasia";
            sasiaDataGridColumnStyleDataGridTextBoxColumn.MappingName = "Sasia";
            // 
            // iDArtikulliDataGridColumnStyleDataGridTextBoxColumn
            // 
            iDArtikulliDataGridColumnStyleDataGridTextBoxColumn.Format = "";
            iDArtikulliDataGridColumnStyleDataGridTextBoxColumn.FormatInfo = null;
            iDArtikulliDataGridColumnStyleDataGridTextBoxColumn.HeaderText = "Shifra";
            iDArtikulliDataGridColumnStyleDataGridTextBoxColumn.MappingName = "IDArtikulli";
            iDArtikulliDataGridColumnStyleDataGridTextBoxColumn.Width = 55;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menDalja);
            // 
            // menDalja
            // 
            this.menDalja.Text = "Dalja";
            this.menDalja.Click += new System.EventHandler(this.menDalja_Click);
            // 
            // artikujDepoBindingSource
            // 
            this.artikujDepoBindingSource.DataMember = "ArtikujDepo";
            this.artikujDepoBindingSource.DataSource = this.myMobileDataSet;
            // 
            // myMobileDataSet
            // 
            this.myMobileDataSet.DataSetName = "MyMobileDataSet";
            this.myMobileDataSet.Prefix = "";
            this.myMobileDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // artikujDepoDataGrid
            // 
            this.artikujDepoDataGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.artikujDepoDataGrid.DataSource = this.artikujDepoBindingSource;
            this.artikujDepoDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.artikujDepoDataGrid.Location = new System.Drawing.Point(0, 2);
            this.artikujDepoDataGrid.Name = "artikujDepoDataGrid";
            this.artikujDepoDataGrid.RowHeadersVisible = false;
            this.artikujDepoDataGrid.Size = new System.Drawing.Size(240, 274);
            this.artikujDepoDataGrid.TabIndex = 1;
            this.artikujDepoDataGrid.TableStyles.Add(this.artikujDepoTableStyleDataGridTableStyle);
            this.artikujDepoDataGrid.MouseUp += new System.Windows.Forms.MouseEventHandler(this.artikujDepoDataGrid_MouseUp);
            this.artikujDepoDataGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.artikujDepoDataGrid_KeyDown);
            // 
            // artikujDepoTableStyleDataGridTableStyle
            // 
            this.artikujDepoTableStyleDataGridTableStyle.GridColumnStyles.Add(iDArtikulliDataGridColumnStyleDataGridTextBoxColumn);
            this.artikujDepoTableStyleDataGridTableStyle.GridColumnStyles.Add(emriDataGridColumnStyleDataGridTextBoxColumn);
            this.artikujDepoTableStyleDataGridTableStyle.GridColumnStyles.Add(sasiaDataGridColumnStyleDataGridTextBoxColumn);
            this.artikujDepoTableStyleDataGridTableStyle.GridColumnStyles.Add(this.dataGridTextBoxColumn1);
            this.artikujDepoTableStyleDataGridTableStyle.MappingName = "ArtikujDepo";
            // 
            // txtArtikulli
            // 
            this.txtArtikulli.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtArtikulli.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.txtArtikulli.Location = new System.Drawing.Point(32, 0);
            this.txtArtikulli.Name = "txtArtikulli";
            this.txtArtikulli.Size = new System.Drawing.Size(208, 19);
            this.txtArtikulli.TabIndex = 3;
            this.txtArtikulli.TextChanged += new System.EventHandler(this.txtArtikulli_TextChanged);
            this.txtArtikulli.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtArtikulli_KeyDown);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 19);
            this.label1.Text = "Filtër ";
            // 
            // artikujDepoTableAdapter
            // 
            this.artikujDepoTableAdapter.ClearBeforeFill = true;
            // 
            // lblSum
            // 
            this.lblSum.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblSum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSum.Location = new System.Drawing.Point(0, 0);
            this.lblSum.Name = "lblSum";
            this.lblSum.Size = new System.Drawing.Size(240, 19);
            this.lblSum.Text = "0.0";
            this.lblSum.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gainsboro;
            this.panel1.Controls.Add(this.txtArtikulli);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(240, 19);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Gainsboro;
            this.panel2.Controls.Add(this.lblSum);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 276);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(240, 19);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(240, 2);
            // 
            // dataGridTextBoxColumn1
            // 
            this.dataGridTextBoxColumn1.Format = "";
            this.dataGridTextBoxColumn1.FormatInfo = null;
            this.dataGridTextBoxColumn1.HeaderText = "Seri";
            this.dataGridTextBoxColumn1.MappingName = "Seri";
            // 
            // frmStoku
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(240, 295);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.artikujDepoDataGrid);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(0, 0);
            this.Menu = this.mainMenu1;
            this.Name = "frmStoku";
            this.Text = "Stoku";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Stoku_Load);
            ((System.ComponentModel.ISupportInitialize)(this.artikujDepoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDataSet)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MyMobileDataSet myMobileDataSet;
        private System.Windows.Forms.BindingSource artikujDepoBindingSource;
        private MobileSales.MyMobileDataSetTableAdapters.ArtikujDepoTableAdapter artikujDepoTableAdapter;
        private System.Windows.Forms.DataGrid artikujDepoDataGrid;
        private System.Windows.Forms.DataGridTableStyle artikujDepoTableStyleDataGridTableStyle;
        private System.Windows.Forms.TextBox txtArtikulli;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuItem menDalja;
        private System.Windows.Forms.Label lblSum;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn1;
    }
}