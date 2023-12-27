namespace MobileSales
{
    partial class frmListDetyrime
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
            this.menuDalja = new System.Windows.Forms.MenuItem();
            this.grdDetyrimet = new System.Windows.Forms.DataGrid();
            this.dsDetyrimet = new System.Data.DataSet();
            this.lblDetyrimet = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dsDetyrimet)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuDalja);
            // 
            // menuDalja
            // 
            this.menuDalja.Text = "Dalja";
            this.menuDalja.Click += new System.EventHandler(this.menuDalja_Click);
            // 
            // grdDetyrimet
            // 
            this.grdDetyrimet.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.grdDetyrimet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdDetyrimet.Location = new System.Drawing.Point(0, 0);
            this.grdDetyrimet.Name = "grdDetyrimet";
            this.grdDetyrimet.RowHeadersVisible = false;
            this.grdDetyrimet.Size = new System.Drawing.Size(240, 274);
            this.grdDetyrimet.TabIndex = 0;
            this.grdDetyrimet.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdDetyrimet_MouseUp);
            this.grdDetyrimet.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdDetyrimet_KeyDown);
            // 
            // dsDetyrimet
            // 
            this.dsDetyrimet.DataSetName = "NewDataSet";
            this.dsDetyrimet.Namespace = "";
            this.dsDetyrimet.Prefix = "";
            this.dsDetyrimet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // lblDetyrimet
            // 
            this.lblDetyrimet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblDetyrimet.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblDetyrimet.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblDetyrimet.ForeColor = System.Drawing.Color.Black;
            this.lblDetyrimet.Location = new System.Drawing.Point(0, 274);
            this.lblDetyrimet.Name = "lblDetyrimet";
            this.lblDetyrimet.Size = new System.Drawing.Size(240, 20);
            this.lblDetyrimet.Text = "0";
            this.lblDetyrimet.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // frmListDetyrime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.grdDetyrimet);
            this.Controls.Add(this.lblDetyrimet);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(0, 0);
            this.Menu = this.mainMenu1;
            this.Name = "frmListDetyrime";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmListDetyrime_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dsDetyrimet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGrid grdDetyrimet;
        private System.Data.DataSet dsDetyrimet;
        private System.Windows.Forms.MenuItem menuDalja;
        private System.Windows.Forms.Label lblDetyrimet;
    }
}