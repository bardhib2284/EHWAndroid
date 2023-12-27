namespace MobileSales
{
    partial class frmSync
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
            this.btnSinkronizo = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lblDeviceID = new System.Windows.Forms.Label();
            this.lblDepo = new System.Windows.Forms.Label();
            this.btnShkarko = new System.Windows.Forms.Button();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuDalja = new System.Windows.Forms.MenuItem();
            this.menuRaporti = new System.Windows.Forms.MenuItem();
            this.btnSinkronizoKlientet = new System.Windows.Forms.Button();
            this.btnSinkronizoCmimoren = new System.Windows.Forms.Button();
            this.lblProgresi = new System.Windows.Forms.Label();
            this.btnAzhurno = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSinkronizo
            // 
            this.btnSinkronizo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.btnSinkronizo.Location = new System.Drawing.Point(0, 173);
            this.btnSinkronizo.Name = "btnSinkronizo";
            this.btnSinkronizo.Size = new System.Drawing.Size(240, 28);
            this.btnSinkronizo.TabIndex = 1;
            this.btnSinkronizo.Text = "Sinkronizo";
            this.btnSinkronizo.Click += new System.EventHandler(this.btnSinkronizo_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(0, 257);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(240, 20);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(0, 24);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(240, 64);
            this.textBox1.TabIndex = 10;
            // 
            // lblDeviceID
            // 
            this.lblDeviceID.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Underline);
            this.lblDeviceID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblDeviceID.Location = new System.Drawing.Point(0, 0);
            this.lblDeviceID.Name = "lblDeviceID";
            this.lblDeviceID.Size = new System.Drawing.Size(110, 21);
            this.lblDeviceID.Text = "Dev=home.devid";
            // 
            // lblDepo
            // 
            this.lblDepo.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Underline);
            this.lblDepo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblDepo.Location = new System.Drawing.Point(124, 0);
            this.lblDepo.Name = "lblDepo";
            this.lblDepo.Size = new System.Drawing.Size(108, 21);
            this.lblDepo.Text = "Depo=home.Depo";
            this.lblDepo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnShkarko
            // 
            this.btnShkarko.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.btnShkarko.Enabled = false;
            this.btnShkarko.Location = new System.Drawing.Point(0, 207);
            this.btnShkarko.Name = "btnShkarko";
            this.btnShkarko.Size = new System.Drawing.Size(240, 28);
            this.btnShkarko.TabIndex = 13;
            this.btnShkarko.Text = "Shkarko";
            this.btnShkarko.Click += new System.EventHandler(this.btnShkarko_Click);
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuDalja);
            this.mainMenu1.MenuItems.Add(this.menuRaporti);
            // 
            // menuDalja
            // 
            this.menuDalja.Text = "Dalja";
            this.menuDalja.Click += new System.EventHandler(this.menuDalja_Click);
            // 
            // menuRaporti
            // 
            this.menuRaporti.Text = "Raporti";
            this.menuRaporti.Click += new System.EventHandler(this.menuRaporti_Click);
            // 
            // btnSinkronizoKlientet
            // 
            this.btnSinkronizoKlientet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSinkronizoKlientet.Location = new System.Drawing.Point(120, 103);
            this.btnSinkronizoKlientet.Name = "btnSinkronizoKlientet";
            this.btnSinkronizoKlientet.Size = new System.Drawing.Size(120, 28);
            this.btnSinkronizoKlientet.TabIndex = 19;
            this.btnSinkronizoKlientet.Text = "Sink. klientet";
            this.btnSinkronizoKlientet.Click += new System.EventHandler(this.btnSinkronizoKlientet_Click);
            // 
            // btnSinkronizoCmimoren
            // 
            this.btnSinkronizoCmimoren.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSinkronizoCmimoren.Location = new System.Drawing.Point(0, 103);
            this.btnSinkronizoCmimoren.Name = "btnSinkronizoCmimoren";
            this.btnSinkronizoCmimoren.Size = new System.Drawing.Size(116, 28);
            this.btnSinkronizoCmimoren.TabIndex = 20;
            this.btnSinkronizoCmimoren.Text = "Sink. çmimet";
            this.btnSinkronizoCmimoren.Click += new System.EventHandler(this.btnSinkronizoCmimoren_Click);
            // 
            // lblProgresi
            // 
            this.lblProgresi.Location = new System.Drawing.Point(0, 238);
            this.lblProgresi.Name = "lblProgresi";
            this.lblProgresi.Size = new System.Drawing.Size(232, 16);
            this.lblProgresi.Text = "Progresi";
            // 
            // btnAzhurno
            // 
            this.btnAzhurno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnAzhurno.Location = new System.Drawing.Point(0, 137);
            this.btnAzhurno.Name = "btnAzhurno";
            this.btnAzhurno.Size = new System.Drawing.Size(240, 28);
            this.btnAzhurno.TabIndex = 33;
            this.btnAzhurno.Text = "Azhurno";
            this.btnAzhurno.Click += new System.EventHandler(this.btnAzhurno_Click);
            // 
            // frmSync
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.btnAzhurno);
            this.Controls.Add(this.lblProgresi);
            this.Controls.Add(this.btnSinkronizoCmimoren);
            this.Controls.Add(this.btnSinkronizoKlientet);
            this.Controls.Add(this.btnShkarko);
            this.Controls.Add(this.lblDepo);
            this.Controls.Add(this.lblDeviceID);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnSinkronizo);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.Location = new System.Drawing.Point(0, 0);
            this.MaximizeBox = false;
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "frmSync";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmSync_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox textBox1;
				private System.Windows.Forms.Label lblDeviceID;
                private System.Windows.Forms.Label lblDepo;
                public System.Windows.Forms.Button btnShkarko;
                public System.Windows.Forms.Button btnSinkronizo;
                private System.Windows.Forms.MainMenu mainMenu1;
                private System.Windows.Forms.MenuItem menuDalja;
                private System.Windows.Forms.MenuItem menuRaporti;
                public System.Windows.Forms.Button btnSinkronizoKlientet;
                public System.Windows.Forms.Button btnSinkronizoCmimoren;
                private System.Windows.Forms.Label lblProgresi;
                public System.Windows.Forms.Button btnAzhurno;
    }
}

