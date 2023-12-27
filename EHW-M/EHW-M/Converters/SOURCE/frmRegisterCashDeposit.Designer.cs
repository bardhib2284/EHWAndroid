namespace MobileSales
{
    partial class frmRegisterCashDeposit
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
            this.txtSasia = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblHVersion = new System.Windows.Forms.Label();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menu_regjistro = new System.Windows.Forms.MenuItem();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtSasia
            // 
            this.txtSasia.Location = new System.Drawing.Point(102, 153);
            this.txtSasia.Name = "txtSasia";
            this.txtSasia.Size = new System.Drawing.Size(116, 21);
            this.txtSasia.TabIndex = 0;
            this.txtSasia.TextChanged += new System.EventHandler(this.txtSasia_TextChanged);
            this.txtSasia.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSasia_KeyDown);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.label1.Location = new System.Drawing.Point(17, 131);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 19);
            this.label1.Text = "Shuma për regjistrim";
            // 
            // lblHVersion
            // 
            this.lblHVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblHVersion.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblHVersion.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblHVersion.Location = new System.Drawing.Point(0, 280);
            this.lblHVersion.Name = "lblHVersion";
            this.lblHVersion.Size = new System.Drawing.Size(240, 14);
            this.lblHVersion.Text = "Versioni";
            this.lblHVersion.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menu_regjistro);
            // 
            // menu_regjistro
            // 
            this.menu_regjistro.Text = "Regjistro";
            this.menu_regjistro.Click += new System.EventHandler(this.menu_Regjistro_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(17, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(201, 37);
            this.label3.Text = "Regjistro gjendjen fillestare të Arkës";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // frmRegisterCashDeposit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblHVersion);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSasia);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "frmRegisterCashDeposit";
            this.Text = " ";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmRegisterCashDeposit_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtSasia;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblHVersion;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menu_regjistro;
        private System.Windows.Forms.Label label3;
    
    }
}

