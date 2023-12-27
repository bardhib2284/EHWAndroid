namespace MobileSales
{
    partial class frm_Gjenerimi_Porosive
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
            System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn2;
            System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn3;
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuExit = new System.Windows.Forms.MenuItem();
            this.menuNew = new System.Windows.Forms.MenuItem();
            this.menuAnulo = new System.Windows.Forms.MenuItem();
            this.grdData = new System.Windows.Forms.DataGrid();
            this.pnl_Detalet = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.cmbKlienti = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
            dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
            dataGridTextBoxColumn2 = new System.Windows.Forms.DataGridTextBoxColumn();
            dataGridTextBoxColumn3 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.pnl_Detalet.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridTableStyle1
            // 
            dataGridTableStyle1.GridColumnStyles.Add(dataGridTextBoxColumn1);
            dataGridTableStyle1.GridColumnStyles.Add(dataGridTextBoxColumn2);
            dataGridTableStyle1.GridColumnStyles.Add(dataGridTextBoxColumn3);
            // 
            // dataGridTextBoxColumn1
            // 
            dataGridTextBoxColumn1.Format = "";
            dataGridTextBoxColumn1.FormatInfo = null;
            dataGridTextBoxColumn1.HeaderText = "ID";
            dataGridTextBoxColumn1.MappingName = "IDKlienti";
            // 
            // dataGridTextBoxColumn2
            // 
            dataGridTextBoxColumn2.Format = "";
            dataGridTextBoxColumn2.FormatInfo = null;
            dataGridTextBoxColumn2.HeaderText = "Klienti";
            dataGridTextBoxColumn2.MappingName = "Emri";
            dataGridTextBoxColumn2.Width = 190;
            // 
            // dataGridTextBoxColumn3
            // 
            dataGridTextBoxColumn3.Format = "";
            dataGridTextBoxColumn3.FormatInfo = null;
            dataGridTextBoxColumn3.HeaderText = "KPID";
            dataGridTextBoxColumn3.MappingName = "KPID";
            dataGridTextBoxColumn3.Width = 0;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuExit);
            this.mainMenu1.MenuItems.Add(this.menuNew);
            this.mainMenu1.MenuItems.Add(this.menuAnulo);
            // 
            // menuExit
            // 
            this.menuExit.Text = "Dalja";
            this.menuExit.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menuNew
            // 
            this.menuNew.Text = "I ri";
            this.menuNew.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuAnulo
            // 
            this.menuAnulo.Enabled = false;
            this.menuAnulo.Text = "Anulo";
            this.menuAnulo.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // grdData
            // 
            this.grdData.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.grdData.Location = new System.Drawing.Point(0, 43);
            this.grdData.Name = "grdData";
            this.grdData.RowHeadersVisible = false;
            this.grdData.Size = new System.Drawing.Size(240, 248);
            this.grdData.TabIndex = 0;
            this.grdData.TableStyles.Add(dataGridTableStyle1);
            this.grdData.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdData_MouseUp);
            this.grdData.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdData_KeyDown);
            // 
            // pnl_Detalet
            // 
            this.pnl_Detalet.BackColor = System.Drawing.Color.Gainsboro;
            this.pnl_Detalet.Controls.Add(this.btnSave);
            this.pnl_Detalet.Controls.Add(this.cmbKlienti);
            this.pnl_Detalet.Controls.Add(this.label1);
            this.pnl_Detalet.Location = new System.Drawing.Point(0, 0);
            this.pnl_Detalet.Name = "pnl_Detalet";
            this.pnl_Detalet.Size = new System.Drawing.Size(240, 43);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular);
            this.btnSave.Location = new System.Drawing.Point(199, 11);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(40, 20);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Ruaj";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cmbKlienti
            // 
            this.cmbKlienti.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular);
            this.cmbKlienti.Location = new System.Drawing.Point(44, 11);
            this.cmbKlienti.Name = "cmbKlienti";
            this.cmbKlienti.Size = new System.Drawing.Size(149, 21);
            this.cmbKlienti.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular);
            this.label1.Location = new System.Drawing.Point(0, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 20);
            this.label1.Text = "Klienti:";
            // 
            // frm_Gjenerimi_Porosive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.pnl_Detalet);
            this.Controls.Add(this.grdData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(0, 0);
            this.Menu = this.mainMenu1;
            this.Name = "frm_Gjenerimi_Porosive";
            this.Text = "frm_Gjenerimi_Porosive";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frm_Gjenerimi_Porosive_Load);
            this.pnl_Detalet.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuExit;
        private System.Windows.Forms.MenuItem menuNew;
        private System.Windows.Forms.MenuItem menuAnulo;
        private System.Windows.Forms.DataGrid grdData;
        private System.Windows.Forms.Panel pnl_Detalet;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox cmbKlienti;
        private System.Windows.Forms.Label label1;
    }
}