namespace MobileSales
{
    partial class frmLevizjet
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
            System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn5;
            System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn3;
            System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn4;
            this.dataGridTextBoxColumn6 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn7 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menu_EXIT = new System.Windows.Forms.MenuItem();
            this.menu_Anulo = new System.Windows.Forms.MenuItem();
            this.menu_REGJISTRO = new System.Windows.Forms.MenuItem();
            this.cmb_TRANSFER = new System.Windows.Forms.ComboBox();
            this.lbl_Transfer = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtArtikulli = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSasia = new System.Windows.Forms.TextBox();
            this.btnShto = new System.Windows.Forms.Button();
            this.grdDetails = new System.Windows.Forms.DataGrid();
            this.check_NGA = new System.Windows.Forms.CheckBox();
            this.check_NE = new System.Windows.Forms.CheckBox();
            this.pnlAll = new System.Windows.Forms.Panel();
            this.lbl_NUMRI = new System.Windows.Forms.Label();
            this.txt_Numri = new System.Windows.Forms.TextBox();
            this.lblTotali = new System.Windows.Forms.Label();
            this.lblSeri = new System.Windows.Forms.Label();
            this.txtSeri = new System.Windows.Forms.TextBox();
            dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
            dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
            dataGridTextBoxColumn2 = new System.Windows.Forms.DataGridTextBoxColumn();
            dataGridTextBoxColumn5 = new System.Windows.Forms.DataGridTextBoxColumn();
            dataGridTextBoxColumn3 = new System.Windows.Forms.DataGridTextBoxColumn();
            dataGridTextBoxColumn4 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.pnlAll.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridTableStyle1
            // 
            dataGridTableStyle1.GridColumnStyles.Add(dataGridTextBoxColumn1);
            dataGridTableStyle1.GridColumnStyles.Add(dataGridTextBoxColumn2);
            dataGridTableStyle1.GridColumnStyles.Add(dataGridTextBoxColumn5);
            dataGridTableStyle1.GridColumnStyles.Add(dataGridTextBoxColumn3);
            dataGridTableStyle1.GridColumnStyles.Add(dataGridTextBoxColumn4);
            dataGridTableStyle1.GridColumnStyles.Add(this.dataGridTextBoxColumn6);
            dataGridTableStyle1.GridColumnStyles.Add(this.dataGridTextBoxColumn7);
            // 
            // dataGridTextBoxColumn1
            // 
            dataGridTextBoxColumn1.Format = "";
            dataGridTextBoxColumn1.FormatInfo = null;
            dataGridTextBoxColumn1.HeaderText = "AID";
            dataGridTextBoxColumn1.MappingName = "IDArtikulli";
            dataGridTextBoxColumn1.Width = 60;
            // 
            // dataGridTextBoxColumn2
            // 
            dataGridTextBoxColumn2.Format = "";
            dataGridTextBoxColumn2.FormatInfo = null;
            dataGridTextBoxColumn2.HeaderText = "Artikulli";
            dataGridTextBoxColumn2.MappingName = "Artikulli";
            dataGridTextBoxColumn2.Width = 60;
            // 
            // dataGridTextBoxColumn5
            // 
            dataGridTextBoxColumn5.Format = "";
            dataGridTextBoxColumn5.FormatInfo = null;
            dataGridTextBoxColumn5.HeaderText = "Nj.Matëse";
            dataGridTextBoxColumn5.MappingName = "Njesia_matese";
            dataGridTextBoxColumn5.Width = 60;
            // 
            // dataGridTextBoxColumn3
            // 
            dataGridTextBoxColumn3.Format = "0.000";
            dataGridTextBoxColumn3.FormatInfo = null;
            dataGridTextBoxColumn3.HeaderText = "Sasia";
            dataGridTextBoxColumn3.MappingName = "Sasia";
            dataGridTextBoxColumn3.Width = 60;
            // 
            // dataGridTextBoxColumn4
            // 
            dataGridTextBoxColumn4.Format = "0.00";
            dataGridTextBoxColumn4.FormatInfo = null;
            dataGridTextBoxColumn4.HeaderText = "Cmimi";
            dataGridTextBoxColumn4.MappingName = "Cmimi";
            dataGridTextBoxColumn4.Width = 60;
            // 
            // dataGridTextBoxColumn6
            // 
            this.dataGridTextBoxColumn6.Format = "0.00";
            this.dataGridTextBoxColumn6.FormatInfo = null;
            this.dataGridTextBoxColumn6.HeaderText = "Totali";
            this.dataGridTextBoxColumn6.MappingName = "Totali";
            // 
            // dataGridTextBoxColumn7
            // 
            this.dataGridTextBoxColumn7.Format = "";
            this.dataGridTextBoxColumn7.FormatInfo = null;
            this.dataGridTextBoxColumn7.HeaderText = "Seri";
            this.dataGridTextBoxColumn7.MappingName = "Seri";
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menu_EXIT);
            this.mainMenu1.MenuItems.Add(this.menu_Anulo);
            this.mainMenu1.MenuItems.Add(this.menu_REGJISTRO);
            // 
            // menu_EXIT
            // 
            this.menu_EXIT.Text = "Dalja";
            this.menu_EXIT.Click += new System.EventHandler(this.menu_EXIT_Click);
            // 
            // menu_Anulo
            // 
            this.menu_Anulo.Text = "Anulo artikullin";
            this.menu_Anulo.Click += new System.EventHandler(this.menu_Anulo_Click);
            // 
            // menu_REGJISTRO
            // 
            this.menu_REGJISTRO.Text = "Regjistro";
            this.menu_REGJISTRO.Click += new System.EventHandler(this.menu_REGJISTRO_Click);
            // 
            // cmb_TRANSFER
            // 
            this.cmb_TRANSFER.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cmb_TRANSFER.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular);
            this.cmb_TRANSFER.Location = new System.Drawing.Point(0, 20);
            this.cmb_TRANSFER.Name = "cmb_TRANSFER";
            this.cmb_TRANSFER.Size = new System.Drawing.Size(87, 21);
            this.cmb_TRANSFER.TabIndex = 0;
            this.cmb_TRANSFER.LostFocus += new System.EventHandler(this.cmb_TRANSFER_LostFocus);
            this.cmb_TRANSFER.SelectedIndexChanged += new System.EventHandler(this.cmb_TRANSFER_SelectedIndexChanged);
            // 
            // lbl_Transfer
            // 
            this.lbl_Transfer.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular);
            this.lbl_Transfer.Location = new System.Drawing.Point(0, 0);
            this.lbl_Transfer.Name = "lbl_Transfer";
            this.lbl_Transfer.Size = new System.Drawing.Size(70, 20);
            this.lbl_Transfer.Text = "Transfer nga";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular);
            this.label3.Location = new System.Drawing.Point(0, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(160, 15);
            this.label3.Text = "Artikulli";
            // 
            // txtArtikulli
            // 
            this.txtArtikulli.Enabled = false;
            this.txtArtikulli.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular);
            this.txtArtikulli.Location = new System.Drawing.Point(0, 55);
            this.txtArtikulli.Name = "txtArtikulli";
            this.txtArtikulli.Size = new System.Drawing.Size(140, 20);
            this.txtArtikulli.TabIndex = 7;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.Gainsboro;
            this.btnAdd.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.btnAdd.Location = new System.Drawing.Point(140, 55);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(20, 20);
            this.btnAdd.TabIndex = 8;
            this.btnAdd.Text = "...";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular);
            this.label4.Location = new System.Drawing.Point(160, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 15);
            this.label4.Text = "Sasia";
            // 
            // txtSasia
            // 
            this.txtSasia.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular);
            this.txtSasia.Location = new System.Drawing.Point(160, 55);
            this.txtSasia.Name = "txtSasia";
            this.txtSasia.Size = new System.Drawing.Size(40, 20);
            this.txtSasia.TabIndex = 11;
            this.txtSasia.TextChanged += new System.EventHandler(this.txtSasia_TextChanged);
            this.txtSasia.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSasia_KeyDown);
            // 
            // btnShto
            // 
            this.btnShto.BackColor = System.Drawing.Color.Gainsboro;
            this.btnShto.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.btnShto.Location = new System.Drawing.Point(200, 55);
            this.btnShto.Name = "btnShto";
            this.btnShto.Size = new System.Drawing.Size(40, 20);
            this.btnShto.TabIndex = 12;
            this.btnShto.Text = "Shto";
            this.btnShto.Click += new System.EventHandler(this.btnShto_Click);
            // 
            // grdDetails
            // 
            this.grdDetails.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.grdDetails.Location = new System.Drawing.Point(0, 75);
            this.grdDetails.Name = "grdDetails";
            this.grdDetails.RowHeadersVisible = false;
            this.grdDetails.Size = new System.Drawing.Size(240, 172);
            this.grdDetails.TabIndex = 13;
            this.grdDetails.TableStyles.Add(dataGridTableStyle1);
            this.grdDetails.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdDetails_MouseUp);
            this.grdDetails.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdDetails_KeyDown);
            // 
            // check_NGA
            // 
            this.check_NGA.BackColor = System.Drawing.Color.Gainsboro;
            this.check_NGA.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular);
            this.check_NGA.Location = new System.Drawing.Point(0, 1);
            this.check_NGA.Name = "check_NGA";
            this.check_NGA.Size = new System.Drawing.Size(120, 20);
            this.check_NGA.TabIndex = 14;
            this.check_NGA.CheckStateChanged += new System.EventHandler(this.check_NGA_CheckStateChanged);
            // 
            // check_NE
            // 
            this.check_NE.BackColor = System.Drawing.Color.Gainsboro;
            this.check_NE.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular);
            this.check_NE.Location = new System.Drawing.Point(120, 1);
            this.check_NE.Name = "check_NE";
            this.check_NE.Size = new System.Drawing.Size(120, 20);
            this.check_NE.TabIndex = 15;
            this.check_NE.CheckStateChanged += new System.EventHandler(this.check_NE_CheckStateChanged);
            // 
            // pnlAll
            // 
            this.pnlAll.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlAll.Controls.Add(this.lblSeri);
            this.pnlAll.Controls.Add(this.txtSeri);
            this.pnlAll.Controls.Add(this.lbl_NUMRI);
            this.pnlAll.Controls.Add(this.txt_Numri);
            this.pnlAll.Controls.Add(this.lblTotali);
            this.pnlAll.Controls.Add(this.grdDetails);
            this.pnlAll.Controls.Add(this.txtArtikulli);
            this.pnlAll.Controls.Add(this.cmb_TRANSFER);
            this.pnlAll.Controls.Add(this.btnShto);
            this.pnlAll.Controls.Add(this.lbl_Transfer);
            this.pnlAll.Controls.Add(this.txtSasia);
            this.pnlAll.Controls.Add(this.label3);
            this.pnlAll.Controls.Add(this.label4);
            this.pnlAll.Controls.Add(this.btnAdd);
            this.pnlAll.Enabled = false;
            this.pnlAll.Location = new System.Drawing.Point(0, 21);
            this.pnlAll.Name = "pnlAll";
            this.pnlAll.Size = new System.Drawing.Size(240, 271);
            // 
            // lbl_NUMRI
            // 
            this.lbl_NUMRI.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular);
            this.lbl_NUMRI.Location = new System.Drawing.Point(90, 0);
            this.lbl_NUMRI.Name = "lbl_NUMRI";
            this.lbl_NUMRI.Size = new System.Drawing.Size(84, 20);
            this.lbl_NUMRI.Text = "Nr.Lëvizjes";
            // 
            // txt_Numri
            // 
            this.txt_Numri.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular);
            this.txt_Numri.Location = new System.Drawing.Point(87, 20);
            this.txt_Numri.Name = "txt_Numri";
            this.txt_Numri.Size = new System.Drawing.Size(84, 20);
            this.txt_Numri.TabIndex = 19;
            // 
            // lblTotali
            // 
            this.lblTotali.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblTotali.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTotali.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblTotali.ForeColor = System.Drawing.Color.Black;
            this.lblTotali.Location = new System.Drawing.Point(0, 247);
            this.lblTotali.Name = "lblTotali";
            this.lblTotali.Size = new System.Drawing.Size(240, 24);
            this.lblTotali.Text = "0.00";
            this.lblTotali.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblSeri
            // 
            this.lblSeri.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular);
            this.lblSeri.Location = new System.Drawing.Point(172, 0);
            this.lblSeri.Name = "lblSeri";
            this.lblSeri.Size = new System.Drawing.Size(66, 20);
            this.lblSeri.Text = "Seri";
            // 
            // txtSeri
            // 
            this.txtSeri.Enabled = false;
            this.txtSeri.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular);
            this.txtSeri.Location = new System.Drawing.Point(172, 20);
            this.txtSeri.Name = "txtSeri";
            this.txtSeri.Size = new System.Drawing.Size(68, 20);
            this.txtSeri.TabIndex = 25;
            // 
            // frmLevizjet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.pnlAll);
            this.Controls.Add(this.check_NE);
            this.Controls.Add(this.check_NGA);
            this.Location = new System.Drawing.Point(0, 0);
            this.Menu = this.mainMenu1;
            this.Name = "frmLevizjet";
            this.Text = "frmLevizjet";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmLevizjet_Load);
            this.pnlAll.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menu_EXIT;
        private System.Windows.Forms.ComboBox cmb_TRANSFER;
        private System.Windows.Forms.Label lbl_Transfer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MenuItem menu_REGJISTRO;
        private System.Windows.Forms.MenuItem menu_Anulo;
        private System.Windows.Forms.CheckBox check_NGA;
        private System.Windows.Forms.CheckBox check_NE;
        public System.Windows.Forms.Button btnAdd;
        public System.Windows.Forms.Button btnShto;
        public System.Windows.Forms.DataGrid grdDetails;
        public System.Windows.Forms.TextBox txtArtikulli;
        public System.Windows.Forms.TextBox txtSasia;
        private System.Windows.Forms.Panel pnlAll;
        private System.Windows.Forms.Label lblTotali;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn6;
        private System.Windows.Forms.Label lbl_NUMRI;
        private System.Windows.Forms.TextBox txt_Numri;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn7;
        private System.Windows.Forms.Label lblSeri;
        public System.Windows.Forms.TextBox txtSeri;
    }
}