namespace MobileSales
{
    partial class frmListInkasimet
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
            System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
            System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn1;
            this.grdListInkasimet = new System.Windows.Forms.DataGrid();
            this.dsListInkasimet = new System.Data.DataSet();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuDalja = new System.Windows.Forms.MenuItem();
            this.menuPrinto = new System.Windows.Forms.MenuItem();
            this.lblTot = new System.Windows.Forms.Label();
            dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
            dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dsListInkasimet)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridTableStyle1
            // 
            dataGridTableStyle1.GridColumnStyles.Add(dataGridTextBoxColumn1);
            dataGridTableStyle1.MappingName = "dsListInkasimet";
            // 
            // dataGridTextBoxColumn1
            // 
            dataGridTextBoxColumn1.Format = "";
            dataGridTextBoxColumn1.FormatInfo = null;
            dataGridTextBoxColumn1.HeaderText = "Fatura";
            dataGridTextBoxColumn1.MappingName = "NrFatures";
            dataGridTextBoxColumn1.Width = 100;
            // 
            // grdListInkasimet
            // 
            this.grdListInkasimet.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.grdListInkasimet.DataSource = this.dsListInkasimet;
            this.grdListInkasimet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdListInkasimet.Location = new System.Drawing.Point(0, 0);
            this.grdListInkasimet.Name = "grdListInkasimet";
            this.grdListInkasimet.RowHeadersVisible = false;
            this.grdListInkasimet.Size = new System.Drawing.Size(240, 274);
            this.grdListInkasimet.TabIndex = 2;
            this.grdListInkasimet.TableStyles.Add(dataGridTableStyle1);
            this.grdListInkasimet.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdListInkasimet_MouseUp);
            this.grdListInkasimet.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdListInkasimet_KeyDown);
            // 
            // dsListInkasimet
            // 
            this.dsListInkasimet.DataSetName = "dsListInkasimet";
            this.dsListInkasimet.Namespace = "";
            this.dsListInkasimet.Prefix = "";
            this.dsListInkasimet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuDalja);
            this.mainMenu1.MenuItems.Add(this.menuPrinto);
            // 
            // menuDalja
            // 
            this.menuDalja.Text = "Dalja";
            this.menuDalja.Click += new System.EventHandler(this.menuDalja_Click);
            // 
            // menuPrinto
            // 
            this.menuPrinto.Text = "Printo";
            this.menuPrinto.Click += new System.EventHandler(this.menuPrinto_Click);
            // 
            // lblTot
            // 
            this.lblTot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblTot.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTot.ForeColor = System.Drawing.Color.Black;
            this.lblTot.Location = new System.Drawing.Point(0, 274);
            this.lblTot.Name = "lblTot";
            this.lblTot.Size = new System.Drawing.Size(240, 20);
            this.lblTot.Text = "0.00";
            this.lblTot.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // frmListInkasimet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.grdListInkasimet);
            this.Controls.Add(this.lblTot);
            this.Location = new System.Drawing.Point(0, 0);
            this.MaximizeBox = false;
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "frmListInkasimet";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmListInkasimet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dsListInkasimet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuDalja;
        private System.Windows.Forms.MenuItem menuPrinto;
        private System.Windows.Forms.Label lblTot;
        private System.Data.DataSet dsListInkasimet;
        private System.Windows.Forms.DataGrid grdListInkasimet;
    }
}