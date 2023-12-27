namespace MobileSales
{
    partial class frmAllInkasimet
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblEmri = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblMbiemri = new System.Windows.Forms.Label();
            this.lblDepo = new System.Windows.Forms.Label();
            this.lblBorxhet = new System.Windows.Forms.Label();
            this.lblFatBank = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dsInkasimet = new System.Data.DataSet();
            this.label4 = new System.Windows.Forms.Label();
            this.lblBorxhetBank = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblFatKESH = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblSumKthimet = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dsInkasimet)).BeginInit();
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
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 20);
            this.label1.Text = "Emri:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 20);
            this.label2.Text = "Mbimeri:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 20);
            this.label3.Text = "Depo:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblEmri
            // 
            this.lblEmri.Location = new System.Drawing.Point(92, 47);
            this.lblEmri.Name = "lblEmri";
            this.lblEmri.Size = new System.Drawing.Size(145, 20);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(12, 205);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(150, 20);
            this.label6.Text = "SH.Inkas.Faturat:(Bank)";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblMbiemri
            // 
            this.lblMbiemri.Location = new System.Drawing.Point(92, 71);
            this.lblMbiemri.Name = "lblMbiemri";
            this.lblMbiemri.Size = new System.Drawing.Size(145, 20);
            // 
            // lblDepo
            // 
            this.lblDepo.Location = new System.Drawing.Point(93, 95);
            this.lblDepo.Name = "lblDepo";
            this.lblDepo.Size = new System.Drawing.Size(144, 20);
            // 
            // lblBorxhet
            // 
            this.lblBorxhet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblBorxhet.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblBorxhet.ForeColor = System.Drawing.Color.Black;
            this.lblBorxhet.Location = new System.Drawing.Point(162, 132);
            this.lblBorxhet.Name = "lblBorxhet";
            this.lblBorxhet.Size = new System.Drawing.Size(75, 20);
            this.lblBorxhet.Text = "0.00";
            this.lblBorxhet.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblFatBank
            // 
            this.lblFatBank.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblFatBank.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblFatBank.ForeColor = System.Drawing.Color.Black;
            this.lblFatBank.Location = new System.Drawing.Point(161, 205);
            this.lblFatBank.Name = "lblFatBank";
            this.lblFatBank.Size = new System.Drawing.Size(76, 20);
            this.lblFatBank.Text = "0.00";
            this.lblFatBank.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(4, 177);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(233, 2);
            this.label7.Text = "label7";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dsInkasimet
            // 
            this.dsInkasimet.DataSetName = "dsIkasimet";
            this.dsInkasimet.Namespace = "";
            this.dsInkasimet.Prefix = "";
            this.dsInkasimet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(-10, 132);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(172, 20);
            this.label4.Text = "SH.Inkas.borxheve:(KESH)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblBorxhetBank
            // 
            this.lblBorxhetBank.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblBorxhetBank.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblBorxhetBank.ForeColor = System.Drawing.Color.Black;
            this.lblBorxhetBank.Location = new System.Drawing.Point(162, 154);
            this.lblBorxhetBank.Name = "lblBorxhetBank";
            this.lblBorxhetBank.Size = new System.Drawing.Size(75, 20);
            this.lblBorxhetBank.Text = "0.00";
            this.lblBorxhetBank.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(0, 154);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(162, 20);
            this.label8.Text = "SH.Inkas.borxheve:(Bank)";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblFatKESH
            // 
            this.lblFatKESH.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblFatKESH.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblFatKESH.ForeColor = System.Drawing.Color.Black;
            this.lblFatKESH.Location = new System.Drawing.Point(162, 182);
            this.lblFatKESH.Name = "lblFatKESH";
            this.lblFatKESH.Size = new System.Drawing.Size(75, 20);
            this.lblFatKESH.Text = "0.00";
            this.lblFatKESH.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(9, 182);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(153, 20);
            this.label10.Text = "SH.Inkas.Faturat:(KESH)";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(5, 228);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(232, 2);
            this.label11.Text = "label11";
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(29, 234);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(133, 20);
            this.label12.Text = "Kthimet/Pagesat";
            this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblSumKthimet
            // 
            this.lblSumKthimet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblSumKthimet.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblSumKthimet.ForeColor = System.Drawing.Color.Black;
            this.lblSumKthimet.Location = new System.Drawing.Point(161, 234);
            this.lblSumKthimet.Name = "lblSumKthimet";
            this.lblSumKthimet.Size = new System.Drawing.Size(76, 20);
            this.lblSumKthimet.Text = "0.00";
            this.lblSumKthimet.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // frmAllInkasimet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(240, 295);
            this.ControlBox = false;
            this.Controls.Add(this.lblFatBank);
            this.Controls.Add(this.lblFatKESH);
            this.Controls.Add(this.lblBorxhetBank);
            this.Controls.Add(this.lblBorxhet);
            this.Controls.Add(this.lblSumKthimet);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblDepo);
            this.Controls.Add(this.lblMbiemri);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblEmri);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Location = new System.Drawing.Point(0, 0);
            this.MaximizeBox = false;
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "frmAllInkasimet";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmAllInkasimet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dsInkasimet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuDalja;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblEmri;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblMbiemri;
        private System.Windows.Forms.Label lblDepo;
        private System.Windows.Forms.Label lblBorxhet;
        private System.Windows.Forms.Label lblFatBank;
        private System.Windows.Forms.Label label7;
        private System.Data.DataSet dsInkasimet;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblBorxhetBank;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblFatKESH;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblSumKthimet;
    }
}