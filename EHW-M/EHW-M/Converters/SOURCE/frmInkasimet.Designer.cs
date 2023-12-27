namespace MobileSales
{
    partial class frmInkasimet
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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menDalja = new System.Windows.Forms.MenuItem();
            this.menuPrinto = new System.Windows.Forms.MenuItem();
            this.menuOne = new System.Windows.Forms.MenuItem();
            this.menuAll = new System.Windows.Forms.MenuItem();
            this.dsKlientet = new System.Data.DataSet();
            this.grdDetyrimet = new System.Windows.Forms.DataGrid();
            this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
            this.dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn2 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn3 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dsDetyrimet = new System.Data.DataSet();
            ((System.ComponentModel.ISupportInitialize)(this.dsKlientet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsDetyrimet)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menDalja);
            this.mainMenu1.MenuItems.Add(this.menuPrinto);
            // 
            // menDalja
            // 
            this.menDalja.Text = "Dalja";
            this.menDalja.Click += new System.EventHandler(this.menuDalja_Click);
            // 
            // menuPrinto
            // 
            this.menuPrinto.MenuItems.Add(this.menuOne);
            this.menuPrinto.MenuItems.Add(this.menuAll);
            this.menuPrinto.Text = "Printo";
            // 
            // menuOne
            // 
            this.menuOne.Text = "Fletpagesën";
            this.menuOne.Click += new System.EventHandler(this.menuPrinto_Click);
            // 
            // menuAll
            // 
            this.menuAll.Text = "Raportin";
            this.menuAll.Click += new System.EventHandler(this.menuAll_Click);
            // 
            // dsKlientet
            // 
            this.dsKlientet.DataSetName = "NewDataSet";
            this.dsKlientet.Namespace = "";
            this.dsKlientet.Prefix = "";
            this.dsKlientet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // grdDetyrimet
            // 
            this.grdDetyrimet.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.grdDetyrimet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdDetyrimet.Location = new System.Drawing.Point(0, 0);
            this.grdDetyrimet.Name = "grdDetyrimet";
            this.grdDetyrimet.RowHeadersVisible = false;
            this.grdDetyrimet.Size = new System.Drawing.Size(240, 295);
            this.grdDetyrimet.TabIndex = 2;
            this.grdDetyrimet.TableStyles.Add(this.dataGridTableStyle1);
            this.grdDetyrimet.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdDetyrimet_MouseUp);
            this.grdDetyrimet.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdDetyrimet_KeyDown);
            // 
            // dataGridTableStyle1
            // 
            this.dataGridTableStyle1.GridColumnStyles.Add(this.dataGridTextBoxColumn1);
            this.dataGridTableStyle1.GridColumnStyles.Add(this.dataGridTextBoxColumn2);
            this.dataGridTableStyle1.GridColumnStyles.Add(this.dataGridTextBoxColumn3);
            this.dataGridTableStyle1.MappingName = "dsDetyrimet";
            // 
            // dataGridTextBoxColumn1
            // 
            this.dataGridTextBoxColumn1.Format = "";
            this.dataGridTextBoxColumn1.FormatInfo = null;
            this.dataGridTextBoxColumn1.HeaderText = "Klienti";
            this.dataGridTextBoxColumn1.MappingName = "Emri";
            // 
            // dataGridTextBoxColumn2
            // 
            this.dataGridTextBoxColumn2.Format = "";
            this.dataGridTextBoxColumn2.FormatInfo = null;
            this.dataGridTextBoxColumn2.HeaderText = "Telefoni";
            this.dataGridTextBoxColumn2.MappingName = "Telefoni";
            // 
            // dataGridTextBoxColumn3
            // 
            this.dataGridTextBoxColumn3.Format = "";
            this.dataGridTextBoxColumn3.FormatInfo = null;
            this.dataGridTextBoxColumn3.HeaderText = "Detyrimi";
            this.dataGridTextBoxColumn3.MappingName = "DetyrimiAktual";
            // 
            // dsDetyrimet
            // 
            this.dsDetyrimet.DataSetName = "NewDataSet";
            this.dsDetyrimet.Namespace = "";
            this.dsDetyrimet.Prefix = "";
            this.dsDetyrimet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // frmInkasimet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 295);
            this.ControlBox = false;
            this.Controls.Add(this.grdDetyrimet);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(0, 0);
            this.MaximizeBox = false;
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "frmInkasimet";
            this.Text = "Inkasimiet";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmInkasimi_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dsKlientet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsDetyrimet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menDalja;
        private System.Data.DataSet dsKlientet;
        private System.Windows.Forms.DataGrid grdDetyrimet;
        private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn1;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn2;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn3;
        private System.Data.DataSet dsDetyrimet;
        private System.Windows.Forms.MenuItem menuPrinto;
        private System.Windows.Forms.MenuItem menuOne;
        private System.Windows.Forms.MenuItem menuAll;
    }
}