namespace MobileSales
{
    partial class dlgMbylljaFatures
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
            this.label1 = new System.Windows.Forms.Label();
            this.rbtnTotal = new System.Windows.Forms.RadioButton();
            this.rbtnPartial = new System.Windows.Forms.RadioButton();
            this.lblShumaPaguar = new System.Windows.Forms.Label();
            this.txtShumaPaguar = new System.Windows.Forms.TextBox();
            this.dtDataPerPagese = new System.Windows.Forms.DateTimePicker();
            this.lblDataPerPagese = new System.Windows.Forms.Label();
            this.lblShumaFatures = new System.Windows.Forms.Label();
            this.cmbMoney = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblMenyraPagese = new System.Windows.Forms.Label();
            this.cmbMenyraPageses = new System.Windows.Forms.ComboBox();
            this.lblGjelbrimi = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Gainsboro;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(240, 20);
            this.label1.Text = "Pagesa";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // rbtnTotal
            // 
            this.rbtnTotal.Checked = true;
            this.rbtnTotal.Location = new System.Drawing.Point(0, 134);
            this.rbtnTotal.Name = "rbtnTotal";
            this.rbtnTotal.Size = new System.Drawing.Size(104, 20);
            this.rbtnTotal.TabIndex = 1;
            this.rbtnTotal.Text = "Totale";
            // 
            // rbtnPartial
            // 
            this.rbtnPartial.Location = new System.Drawing.Point(128, 134);
            this.rbtnPartial.Name = "rbtnPartial";
            this.rbtnPartial.Size = new System.Drawing.Size(112, 20);
            this.rbtnPartial.TabIndex = 2;
            this.rbtnPartial.TabStop = false;
            this.rbtnPartial.Text = "Parciale";
            this.rbtnPartial.Visible = false;
            this.rbtnPartial.CheckedChanged += new System.EventHandler(this.rbtnPartial_CheckedChanged);
            // 
            // lblShumaPaguar
            // 
            this.lblShumaPaguar.Location = new System.Drawing.Point(0, 161);
            this.lblShumaPaguar.Name = "lblShumaPaguar";
            this.lblShumaPaguar.Size = new System.Drawing.Size(104, 20);
            this.lblShumaPaguar.Text = "Shuma e paguar";
            // 
            // txtShumaPaguar
            // 
            this.txtShumaPaguar.Enabled = false;
            this.txtShumaPaguar.Location = new System.Drawing.Point(0, 181);
            this.txtShumaPaguar.Name = "txtShumaPaguar";
            this.txtShumaPaguar.Size = new System.Drawing.Size(240, 21);
            this.txtShumaPaguar.TabIndex = 8;
            this.txtShumaPaguar.Text = "0.00";
            this.txtShumaPaguar.TextChanged += new System.EventHandler(this.txtShumaPaguar_TextChanged);
            // 
            // dtDataPerPagese
            // 
            this.dtDataPerPagese.Enabled = false;
            this.dtDataPerPagese.Location = new System.Drawing.Point(0, 233);
            this.dtDataPerPagese.Name = "dtDataPerPagese";
            this.dtDataPerPagese.Size = new System.Drawing.Size(240, 22);
            this.dtDataPerPagese.TabIndex = 14;
            // 
            // lblDataPerPagese
            // 
            this.lblDataPerPagese.Location = new System.Drawing.Point(0, 213);
            this.lblDataPerPagese.Name = "lblDataPerPagese";
            this.lblDataPerPagese.Size = new System.Drawing.Size(104, 20);
            this.lblDataPerPagese.Text = "Data për pagesë";
            // 
            // lblShumaFatures
            // 
            this.lblShumaFatures.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblShumaFatures.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblShumaFatures.Location = new System.Drawing.Point(0, 74);
            this.lblShumaFatures.Name = "lblShumaFatures";
            this.lblShumaFatures.Size = new System.Drawing.Size(240, 20);
            this.lblShumaFatures.Text = "Shuma Faturës = 0.00";
            // 
            // cmbMoney
            // 
            this.cmbMoney.Items.Add("LEK");
            this.cmbMoney.Items.Add("EUR");
            this.cmbMoney.Items.Add("USD");
            this.cmbMoney.Location = new System.Drawing.Point(0, 42);
            this.cmbMoney.Name = "cmbMoney";
            this.cmbMoney.Size = new System.Drawing.Size(100, 22);
            this.cmbMoney.TabIndex = 17;
            this.cmbMoney.SelectedIndexChanged += new System.EventHandler(this.cmbMoney_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(0, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 20);
            this.label2.Text = "Monedha";
            // 
            // lblMenyraPagese
            // 
            this.lblMenyraPagese.Location = new System.Drawing.Point(119, 21);
            this.lblMenyraPagese.Name = "lblMenyraPagese";
            this.lblMenyraPagese.Size = new System.Drawing.Size(121, 20);
            this.lblMenyraPagese.Text = "Mënyra Pagesës";
            this.lblMenyraPagese.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cmbMenyraPageses
            // 
            this.cmbMenyraPageses.Items.Add("Zgjedh");
            this.cmbMenyraPageses.Items.Add("KESH");
            this.cmbMenyraPageses.Items.Add("Bank");
            this.cmbMenyraPageses.Location = new System.Drawing.Point(128, 42);
            this.cmbMenyraPageses.Name = "cmbMenyraPageses";
            this.cmbMenyraPageses.Size = new System.Drawing.Size(112, 22);
            this.cmbMenyraPageses.TabIndex = 24;
            this.cmbMenyraPageses.SelectedIndexChanged += new System.EventHandler(this.cmbMenyraPageses_SelectedIndexChanged);
            // 
            // lblGjelbrimi
            // 
            this.lblGjelbrimi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblGjelbrimi.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblGjelbrimi.Location = new System.Drawing.Point(0, 103);
            this.lblGjelbrimi.Name = "lblGjelbrimi";
            this.lblGjelbrimi.Size = new System.Drawing.Size(240, 20);
            this.lblGjelbrimi.Text = "Shuma Paguar = 0.00";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(0, 263);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(240, 20);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "Vazhdo";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // dlgMbylljaFatures
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.lblGjelbrimi);
            this.Controls.Add(this.cmbMenyraPageses);
            this.Controls.Add(this.lblMenyraPagese);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbMoney);
            this.Controls.Add(this.lblShumaFatures);
            this.Controls.Add(this.lblDataPerPagese);
            this.Controls.Add(this.dtDataPerPagese);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblShumaPaguar);
            this.Controls.Add(this.txtShumaPaguar);
            this.Controls.Add(this.rbtnPartial);
            this.Controls.Add(this.rbtnTotal);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Location = new System.Drawing.Point(0, 0);
            this.Menu = this.mainMenu1;
            this.Name = "dlgMbylljaFatures";
            this.Text = "Mbyllja e faturës";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.dlgMbylljaFatures_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.dlgMbylljaFatures_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblShumaPaguar;
        public System.Windows.Forms.TextBox txtShumaPaguar;
        public System.Windows.Forms.RadioButton rbtnTotal;
        public System.Windows.Forms.RadioButton rbtnPartial;
        public System.Windows.Forms.DateTimePicker dtDataPerPagese;
        private System.Windows.Forms.Label lblDataPerPagese;
        private System.Windows.Forms.Label lblShumaFatures;
        private System.Windows.Forms.ComboBox cmbMoney;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblMenyraPagese;
        public System.Windows.Forms.ComboBox cmbMenyraPageses;
        private System.Windows.Forms.Label lblGjelbrimi;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.MainMenu mainMenu1;
    }
}