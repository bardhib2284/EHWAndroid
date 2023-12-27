namespace MobileSales
{
    partial class rptLEVIZJET_DETAILS
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainDa;

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
            System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn4;
            this.dataGridTextBoxColumn2 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn3 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.mainDa = new System.Windows.Forms.MainMenu();
            this.menuDalja = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.grdDetalet = new System.Windows.Forms.DataGrid();
            dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
            dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
            dataGridTextBoxColumn4 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.SuspendLayout();
            // 
            // dataGridTableStyle1
            // 
            dataGridTableStyle1.GridColumnStyles.Add(this.dataGridTextBoxColumn2);
            dataGridTableStyle1.GridColumnStyles.Add(this.dataGridTextBoxColumn3);
            dataGridTableStyle1.GridColumnStyles.Add(dataGridTextBoxColumn1);
            dataGridTableStyle1.GridColumnStyles.Add(dataGridTextBoxColumn4);
            // 
            // dataGridTextBoxColumn2
            // 
            this.dataGridTextBoxColumn2.Format = "";
            this.dataGridTextBoxColumn2.FormatInfo = null;
            this.dataGridTextBoxColumn2.HeaderText = "Artikulli";
            this.dataGridTextBoxColumn2.MappingName = "Artikulli";
            this.dataGridTextBoxColumn2.Width = 65;
            // 
            // dataGridTextBoxColumn3
            // 
            this.dataGridTextBoxColumn3.Format = "0.00";
            this.dataGridTextBoxColumn3.FormatInfo = null;
            this.dataGridTextBoxColumn3.HeaderText = "Sasia";
            this.dataGridTextBoxColumn3.MappingName = "Sasia";
            this.dataGridTextBoxColumn3.Width = 65;
            // 
            // dataGridTextBoxColumn1
            // 
            dataGridTextBoxColumn1.Format = "0.00";
            dataGridTextBoxColumn1.FormatInfo = null;
            dataGridTextBoxColumn1.HeaderText = "Cmimi";
            dataGridTextBoxColumn1.MappingName = "Cmimi";
            dataGridTextBoxColumn1.Width = 45;
            // 
            // dataGridTextBoxColumn4
            // 
            dataGridTextBoxColumn4.Format = "0.00";
            dataGridTextBoxColumn4.FormatInfo = null;
            dataGridTextBoxColumn4.HeaderText = "Totali";
            dataGridTextBoxColumn4.MappingName = "Totali";
            // 
            // mainDa
            // 
            this.mainDa.MenuItems.Add(this.menuDalja);
            this.mainDa.MenuItems.Add(this.menuItem1);
            // 
            // menuDalja
            // 
            this.menuDalja.Text = "Dalja";
            this.menuDalja.Click += new System.EventHandler(this.menuDalja_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Text = "Printo";
            this.menuItem1.Click += new System.EventHandler(this.menuPrinto_Click);
            // 
            // grdDetalet
            // 
            this.grdDetalet.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.grdDetalet.Location = new System.Drawing.Point(0, 0);
            this.grdDetalet.Name = "grdDetalet";
            this.grdDetalet.RowHeadersVisible = false;
            this.grdDetalet.Size = new System.Drawing.Size(240, 295);
            this.grdDetalet.TabIndex = 0;
            this.grdDetalet.TableStyles.Add(dataGridTableStyle1);
            this.grdDetalet.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdDetalet_MouseUp);
            this.grdDetalet.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdDetalet_KeyDown);
            // 
            // rptLEVIZJET_DETAILS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(240, 295);
            this.ControlBox = false;
            this.Controls.Add(this.grdDetalet);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(0, 0);
            this.Menu = this.mainDa;
            this.Name = "rptLEVIZJET_DETAILS";
            this.Text = "rptLEVIZJET_HEADER";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.rptLEVIZJET_DETAILS_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuDalja;
        private System.Windows.Forms.DataGrid grdDetalet;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn2;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn3;
        private System.Windows.Forms.MenuItem menuItem1;
    }
}