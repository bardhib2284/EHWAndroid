namespace MobileSales
{
    partial class frmMbetja
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
            System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn4;
            System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn6;
            System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn5;
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menPrinto = new System.Windows.Forms.MenuItem();
            this.menuDalja = new System.Windows.Forms.MenuItem();
            this.dsMbetja = new System.Data.DataSet();
            this.grdMbetja = new System.Windows.Forms.DataGrid();
            this.dsHeader = new System.Data.DataSet();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblDepo = new System.Windows.Forms.Label();
            this.lblShitesi = new System.Windows.Forms.Label();
            this.lblPranuar = new System.Windows.Forms.Label();
            this.lblShitur = new System.Windows.Forms.Label();
            this.lblKthyer = new System.Windows.Forms.Label();
            this.lblMbetur = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblLevizja = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridTextBoxColumn7 = new System.Windows.Forms.DataGridTextBoxColumn();
            dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
            dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
            dataGridTextBoxColumn2 = new System.Windows.Forms.DataGridTextBoxColumn();
            dataGridTextBoxColumn3 = new System.Windows.Forms.DataGridTextBoxColumn();
            dataGridTextBoxColumn4 = new System.Windows.Forms.DataGridTextBoxColumn();
            dataGridTextBoxColumn6 = new System.Windows.Forms.DataGridTextBoxColumn();
            dataGridTextBoxColumn5 = new System.Windows.Forms.DataGridTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dsMbetja)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsHeader)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridTableStyle1
            // 
            dataGridTableStyle1.GridColumnStyles.Add(dataGridTextBoxColumn1);
            dataGridTableStyle1.GridColumnStyles.Add(dataGridTextBoxColumn2);
            dataGridTableStyle1.GridColumnStyles.Add(dataGridTextBoxColumn3);
            dataGridTableStyle1.GridColumnStyles.Add(dataGridTextBoxColumn4);
            dataGridTableStyle1.GridColumnStyles.Add(dataGridTextBoxColumn6);
            dataGridTableStyle1.GridColumnStyles.Add(dataGridTextBoxColumn5);
            dataGridTableStyle1.GridColumnStyles.Add(this.dataGridTextBoxColumn7);
            dataGridTableStyle1.MappingName = "dsMbetja";
            // 
            // dataGridTextBoxColumn1
            // 
            dataGridTextBoxColumn1.Format = "";
            dataGridTextBoxColumn1.FormatInfo = null;
            dataGridTextBoxColumn1.HeaderText = "Artikulli";
            dataGridTextBoxColumn1.MappingName = "Emri";
            // 
            // dataGridTextBoxColumn2
            // 
            dataGridTextBoxColumn2.Format = "0.000";
            dataGridTextBoxColumn2.FormatInfo = null;
            dataGridTextBoxColumn2.HeaderText = "Pranuar";
            dataGridTextBoxColumn2.MappingName = "Pranuar";
            dataGridTextBoxColumn2.Width = 16;
            // 
            // dataGridTextBoxColumn3
            // 
            dataGridTextBoxColumn3.Format = "0.000";
            dataGridTextBoxColumn3.FormatInfo = null;
            dataGridTextBoxColumn3.HeaderText = "Shitur";
            dataGridTextBoxColumn3.MappingName = "Shitur";
            dataGridTextBoxColumn3.NullText = "0.000";
            dataGridTextBoxColumn3.Width = 16;
            // 
            // dataGridTextBoxColumn4
            // 
            dataGridTextBoxColumn4.Format = "0.000";
            dataGridTextBoxColumn4.FormatInfo = null;
            dataGridTextBoxColumn4.HeaderText = "Kthyer";
            dataGridTextBoxColumn4.MappingName = "Kthyer";
            dataGridTextBoxColumn4.NullText = "0.000";
            dataGridTextBoxColumn4.Width = 16;
            // 
            // dataGridTextBoxColumn6
            // 
            dataGridTextBoxColumn6.Format = "0.000";
            dataGridTextBoxColumn6.FormatInfo = null;
            dataGridTextBoxColumn6.HeaderText = "Lëvizje";
            dataGridTextBoxColumn6.MappingName = "Levizje";
            dataGridTextBoxColumn6.NullText = "0.000";
            // 
            // dataGridTextBoxColumn5
            // 
            dataGridTextBoxColumn5.Format = "0.000";
            dataGridTextBoxColumn5.FormatInfo = null;
            dataGridTextBoxColumn5.HeaderText = "Mbetur";
            dataGridTextBoxColumn5.MappingName = "Mbetur";
            dataGridTextBoxColumn5.NullText = "0.000";
            dataGridTextBoxColumn5.Width = 16;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menPrinto);
            this.mainMenu1.MenuItems.Add(this.menuDalja);
            // 
            // menPrinto
            // 
            this.menPrinto.Text = "Printo";
            this.menPrinto.Click += new System.EventHandler(this.menPrinto_Click);
            // 
            // menuDalja
            // 
            this.menuDalja.Text = "Dalja";
            this.menuDalja.Click += new System.EventHandler(this.menuDalja_Click);
            // 
            // dsMbetja
            // 
            this.dsMbetja.DataSetName = "dsMbetja";
            this.dsMbetja.Namespace = "";
            this.dsMbetja.Prefix = "";
            this.dsMbetja.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // grdMbetja
            // 
            this.grdMbetja.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.grdMbetja.DataSource = this.dsMbetja;
            this.grdMbetja.Location = new System.Drawing.Point(0, 23);
            this.grdMbetja.Name = "grdMbetja";
            this.grdMbetja.RowHeadersVisible = false;
            this.grdMbetja.Size = new System.Drawing.Size(240, 239);
            this.grdMbetja.TabIndex = 0;
            this.grdMbetja.TableStyles.Add(dataGridTableStyle1);
            this.grdMbetja.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdMbetja_MouseUp);
            // 
            // dsHeader
            // 
            this.dsHeader.DataSetName = "NewDataSet";
            this.dsHeader.Namespace = "";
            this.dsHeader.Prefix = "";
            this.dsHeader.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 20);
            this.label1.Text = "Shitësi:";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label2.Location = new System.Drawing.Point(133, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 20);
            this.label2.Text = "Depo:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblDepo
            // 
            this.lblDepo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Underline);
            this.lblDepo.Location = new System.Drawing.Point(183, 0);
            this.lblDepo.Name = "lblDepo";
            this.lblDepo.Size = new System.Drawing.Size(57, 20);
            // 
            // lblShitesi
            // 
            this.lblShitesi.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Underline);
            this.lblShitesi.Location = new System.Drawing.Point(48, 0);
            this.lblShitesi.Name = "lblShitesi";
            this.lblShitesi.Size = new System.Drawing.Size(61, 20);
            // 
            // lblPranuar
            // 
            this.lblPranuar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblPranuar.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblPranuar.ForeColor = System.Drawing.Color.Black;
            this.lblPranuar.Location = new System.Drawing.Point(0, 264);
            this.lblPranuar.Name = "lblPranuar";
            this.lblPranuar.Size = new System.Drawing.Size(48, 26);
            this.lblPranuar.Text = "0.00";
            // 
            // lblShitur
            // 
            this.lblShitur.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblShitur.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblShitur.ForeColor = System.Drawing.Color.Black;
            this.lblShitur.Location = new System.Drawing.Point(49, 264);
            this.lblShitur.Name = "lblShitur";
            this.lblShitur.Size = new System.Drawing.Size(48, 26);
            this.lblShitur.Text = "0.00";
            // 
            // lblKthyer
            // 
            this.lblKthyer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblKthyer.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblKthyer.ForeColor = System.Drawing.Color.Black;
            this.lblKthyer.Location = new System.Drawing.Point(98, 264);
            this.lblKthyer.Name = "lblKthyer";
            this.lblKthyer.Size = new System.Drawing.Size(44, 26);
            this.lblKthyer.Text = "0.00";
            // 
            // lblMbetur
            // 
            this.lblMbetur.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblMbetur.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblMbetur.ForeColor = System.Drawing.Color.Black;
            this.lblMbetur.Location = new System.Drawing.Point(189, 264);
            this.lblMbetur.Name = "lblMbetur";
            this.lblMbetur.Size = new System.Drawing.Size(48, 26);
            this.lblMbetur.Text = "0.00";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(97, 264);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(1, 20);
            this.label6.Text = "Shumat:";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(141, 264);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(1, 20);
            this.label7.Text = "Shumat:";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(48, 264);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(1, 20);
            this.label5.Text = "Shumat:";
            // 
            // lblLevizja
            // 
            this.lblLevizja.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblLevizja.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblLevizja.ForeColor = System.Drawing.Color.Black;
            this.lblLevizja.Location = new System.Drawing.Point(142, 264);
            this.lblLevizja.Name = "lblLevizja";
            this.lblLevizja.Size = new System.Drawing.Size(48, 26);
            this.lblLevizja.Text = "0.00";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(189, 264);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(1, 20);
            this.label3.Text = "Shumat:";
            // 
            // dataGridTextBoxColumn7
            // 
            this.dataGridTextBoxColumn7.Format = "";
            this.dataGridTextBoxColumn7.FormatInfo = null;
            this.dataGridTextBoxColumn7.HeaderText = "Seri";
            this.dataGridTextBoxColumn7.MappingName = "Seri";
            // 
            // frmMbetja
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(240, 293);
            this.ControlBox = false;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblLevizja);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblMbetur);
            this.Controls.Add(this.lblKthyer);
            this.Controls.Add(this.lblShitur);
            this.Controls.Add(this.lblPranuar);
            this.Controls.Add(this.lblShitesi);
            this.Controls.Add(this.lblDepo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grdMbetja);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(0, 0);
            this.MaximizeBox = false;
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "frmMbetja";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMbetja_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dsMbetja)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsHeader)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Data.DataSet dsMbetja;
        private System.Windows.Forms.MenuItem menPrinto;
        private System.Windows.Forms.MenuItem menuDalja;
        private System.Windows.Forms.DataGrid grdMbetja;
        private System.Data.DataSet dsHeader;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblDepo;
        private System.Windows.Forms.Label lblShitesi;
        private System.Windows.Forms.Label lblPranuar;
        private System.Windows.Forms.Label lblShitur;
        private System.Windows.Forms.Label lblKthyer;
        private System.Windows.Forms.Label lblMbetur;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblLevizja;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn7;
    }
}