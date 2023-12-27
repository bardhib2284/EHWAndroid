namespace MobileSales
{
    partial class frmInkasimiMobil
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
            this.menuDalja = new System.Windows.Forms.MenuItem();
            this.menuVizualizo = new System.Windows.Forms.MenuItem();
            this.menuRaporti = new System.Windows.Forms.MenuItem();
            this.menuInkasimet = new System.Windows.Forms.MenuItem();
            this.manuListaDetyrimeve = new System.Windows.Forms.MenuItem();
            this.grdFaturatPagesat = new System.Windows.Forms.DataGrid();
            this.cmbKlienti = new System.Windows.Forms.ComboBox();
            this.lblKlienti = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabInkasimi = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbPayType = new System.Windows.Forms.ComboBox();
            this.cmbKMON = new System.Windows.Forms.ComboBox();
            this.btnPrinto = new System.Windows.Forms.Button();
            this.btnInkaso = new System.Windows.Forms.Button();
            this.lblDataPageses = new System.Windows.Forms.Label();
            this.dtDataPageses = new System.Windows.Forms.DateTimePicker();
            this.lblVlera = new System.Windows.Forms.Label();
            this.txtShumaPaguar = new System.Windows.Forms.TextBox();
            dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
            dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
            dataGridTextBoxColumn2 = new System.Windows.Forms.DataGridTextBoxColumn();
            dataGridTextBoxColumn3 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.tabInkasimi.SuspendLayout();
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
            dataGridTextBoxColumn1.HeaderText = "Klienti";
            dataGridTextBoxColumn1.MappingName = "Emri";
            // 
            // dataGridTextBoxColumn2
            // 
            dataGridTextBoxColumn2.Format = "";
            dataGridTextBoxColumn2.FormatInfo = null;
            dataGridTextBoxColumn2.HeaderText = "Detyrimi";
            dataGridTextBoxColumn2.MappingName = "Detyrimi";
            // 
            // dataGridTextBoxColumn3
            // 
            dataGridTextBoxColumn3.Format = "";
            dataGridTextBoxColumn3.FormatInfo = null;
            dataGridTextBoxColumn3.HeaderText = "KMON";
            dataGridTextBoxColumn3.MappingName = "KMON";
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuDalja);
            this.mainMenu1.MenuItems.Add(this.menuVizualizo);
            // 
            // menuDalja
            // 
            this.menuDalja.Text = "Dalja";
            this.menuDalja.Click += new System.EventHandler(this.menuDalja_Click);
            // 
            // menuVizualizo
            // 
            this.menuVizualizo.MenuItems.Add(this.menuRaporti);
            this.menuVizualizo.MenuItems.Add(this.menuInkasimet);
            this.menuVizualizo.MenuItems.Add(this.manuListaDetyrimeve);
            this.menuVizualizo.Text = "Vizualizo";
            // 
            // menuRaporti
            // 
            this.menuRaporti.Text = "Raporti";
            this.menuRaporti.Click += new System.EventHandler(this.menuRaporti_Click);
            // 
            // menuInkasimet
            // 
            this.menuInkasimet.Text = "Inkasimet";
            this.menuInkasimet.Click += new System.EventHandler(this.menuInkasimet_Click);
            // 
            // manuListaDetyrimeve
            // 
            this.manuListaDetyrimeve.Text = "Lista e detyrimeve";
            this.manuListaDetyrimeve.Click += new System.EventHandler(this.menuListaDetyrimeve_Click);
            // 
            // grdFaturatPagesat
            // 
            this.grdFaturatPagesat.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.grdFaturatPagesat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdFaturatPagesat.Location = new System.Drawing.Point(0, 151);
            this.grdFaturatPagesat.Name = "grdFaturatPagesat";
            this.grdFaturatPagesat.RowHeadersVisible = false;
            this.grdFaturatPagesat.Size = new System.Drawing.Size(240, 144);
            this.grdFaturatPagesat.TabIndex = 0;
            this.grdFaturatPagesat.TableStyles.Add(dataGridTableStyle1);
            this.grdFaturatPagesat.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdFaturatPagesat_MouseUp_1);
            // 
            // cmbKlienti
            // 
            this.cmbKlienti.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cmbKlienti.Location = new System.Drawing.Point(47, 4);
            this.cmbKlienti.Name = "cmbKlienti";
            this.cmbKlienti.Size = new System.Drawing.Size(191, 22);
            this.cmbKlienti.TabIndex = 0;
            this.cmbKlienti.SelectedIndexChanged += new System.EventHandler(this.cmbKlienti_SelectedIndexChanged);
            // 
            // lblKlienti
            // 
            this.lblKlienti.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblKlienti.Location = new System.Drawing.Point(7, 9);
            this.lblKlienti.Name = "lblKlienti";
            this.lblKlienti.Size = new System.Drawing.Size(34, 13);
            this.lblKlienti.Text = "Klienti";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabInkasimi);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(240, 151);
            this.tabControl1.TabIndex = 10;
            // 
            // tabInkasimi
            // 
            this.tabInkasimi.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabInkasimi.Controls.Add(this.label2);
            this.tabInkasimi.Controls.Add(this.lblKlienti);
            this.tabInkasimi.Controls.Add(this.label1);
            this.tabInkasimi.Controls.Add(this.cmbPayType);
            this.tabInkasimi.Controls.Add(this.cmbKMON);
            this.tabInkasimi.Controls.Add(this.btnPrinto);
            this.tabInkasimi.Controls.Add(this.btnInkaso);
            this.tabInkasimi.Controls.Add(this.lblDataPageses);
            this.tabInkasimi.Controls.Add(this.dtDataPageses);
            this.tabInkasimi.Controls.Add(this.lblVlera);
            this.tabInkasimi.Controls.Add(this.txtShumaPaguar);
            this.tabInkasimi.Location = new System.Drawing.Point(0, 0);
            this.tabInkasimi.Name = "tabInkasimi";
            this.tabInkasimi.Size = new System.Drawing.Size(240, 128);
            this.tabInkasimi.Text = "Inkasimi";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.label2.Location = new System.Drawing.Point(170, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 16);
            this.label2.Text = "Tipi Pagesës";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.label1.Location = new System.Drawing.Point(97, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.Text = "Monedha";
            // 
            // cmbPayType
            // 
            this.cmbPayType.Enabled = false;
            this.cmbPayType.Items.Add("KESH");
            this.cmbPayType.Items.Add("Bank");
            this.cmbPayType.Location = new System.Drawing.Point(170, 53);
            this.cmbPayType.Name = "cmbPayType";
            this.cmbPayType.Size = new System.Drawing.Size(67, 22);
            this.cmbPayType.TabIndex = 11;
            // 
            // cmbKMON
            // 
            this.cmbKMON.Items.Add("LEK");
            this.cmbKMON.Items.Add("EUR");
            this.cmbKMON.Items.Add("USD");
            this.cmbKMON.Location = new System.Drawing.Point(97, 53);
            this.cmbKMON.Name = "cmbKMON";
            this.cmbKMON.Size = new System.Drawing.Size(67, 22);
            this.cmbKMON.TabIndex = 8;
            // 
            // btnPrinto
            // 
            this.btnPrinto.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.btnPrinto.Location = new System.Drawing.Point(4, 104);
            this.btnPrinto.Name = "btnPrinto";
            this.btnPrinto.Size = new System.Drawing.Size(72, 21);
            this.btnPrinto.TabIndex = 5;
            this.btnPrinto.Text = "Printo";
            this.btnPrinto.Visible = false;
            this.btnPrinto.Click += new System.EventHandler(this.btnPrinto_Click);
            // 
            // btnInkaso
            // 
            this.btnInkaso.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.btnInkaso.Location = new System.Drawing.Point(165, 105);
            this.btnInkaso.Name = "btnInkaso";
            this.btnInkaso.Size = new System.Drawing.Size(72, 21);
            this.btnInkaso.TabIndex = 2;
            this.btnInkaso.Text = "Inkaso";
            this.btnInkaso.Click += new System.EventHandler(this.btnInkaso_Click);
            // 
            // lblDataPageses
            // 
            this.lblDataPageses.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblDataPageses.Location = new System.Drawing.Point(3, 85);
            this.lblDataPageses.Name = "lblDataPageses";
            this.lblDataPageses.Size = new System.Drawing.Size(84, 16);
            this.lblDataPageses.Text = "Data aktuale";
            this.lblDataPageses.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dtDataPageses
            // 
            this.dtDataPageses.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.dtDataPageses.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtDataPageses.Location = new System.Drawing.Point(94, 81);
            this.dtDataPageses.Name = "dtDataPageses";
            this.dtDataPageses.Size = new System.Drawing.Size(140, 20);
            this.dtDataPageses.TabIndex = 1;
            // 
            // lblVlera
            // 
            this.lblVlera.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblVlera.Location = new System.Drawing.Point(3, 36);
            this.lblVlera.Name = "lblVlera";
            this.lblVlera.Size = new System.Drawing.Size(84, 13);
            this.lblVlera.Text = "Shuma pagesës";
            // 
            // txtShumaPaguar
            // 
            this.txtShumaPaguar.Location = new System.Drawing.Point(4, 54);
            this.txtShumaPaguar.Name = "txtShumaPaguar";
            this.txtShumaPaguar.Size = new System.Drawing.Size(83, 21);
            this.txtShumaPaguar.TabIndex = 0;
            this.txtShumaPaguar.TextChanged += new System.EventHandler(this.txtShumaPaguar_TextChanged);
            // 
            // frmInkasimiMobil
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(240, 295);
            this.ControlBox = false;
            this.Controls.Add(this.cmbKlienti);
            this.Controls.Add(this.grdFaturatPagesat);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(0, 0);
            this.Menu = this.mainMenu1;
            this.Name = "frmInkasimiMobil";
            this.Text = "Inkasimi Mobil (borxhet)";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmInkasimiMobil_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmInkasimiMobil_Closing);
            this.tabControl1.ResumeLayout(false);
            this.tabInkasimi.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblKlienti;
        public System.Windows.Forms.ComboBox cmbKlienti;
        private System.Windows.Forms.MenuItem menuDalja;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabInkasimi;
        private System.Windows.Forms.ComboBox cmbKMON;
        private System.Windows.Forms.Button btnPrinto;
        private System.Windows.Forms.Button btnInkaso;
        private System.Windows.Forms.Label lblDataPageses;
        private System.Windows.Forms.DateTimePicker dtDataPageses;
        private System.Windows.Forms.Label lblVlera;
        private System.Windows.Forms.TextBox txtShumaPaguar;
        public System.Windows.Forms.DataGrid grdFaturatPagesat;
        private System.Windows.Forms.MenuItem menuVizualizo;
        private System.Windows.Forms.MenuItem menuRaporti;
        private System.Windows.Forms.MenuItem menuInkasimet;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbPayType;
        private System.Windows.Forms.MenuItem manuListaDetyrimeve;



    }
}