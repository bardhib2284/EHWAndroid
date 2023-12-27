namespace MobileSales
{
    partial class frmDetArtikujve
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label emriLabel;
            System.Windows.Forms.Label cmimiNjesiLabel;
            System.Windows.Forms.Label sasiaPakoLabel;
            System.Windows.Forms.Label sasiaLabel;
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menDlaja = new System.Windows.Forms.MenuItem();
            this.emriTextBox = new System.Windows.Forms.TextBox();
            this.artikujDepoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.myMobileDataset = new MobileSales.MyMobileDataSet();
            this.cmimiNjesiTextBox = new System.Windows.Forms.TextBox();
            this.sasiaPakoTextBox = new System.Windows.Forms.TextBox();
            this.lblLine = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.sasiaTextBox = new System.Windows.Forms.TextBox();
            this.artikujDepoTableAdapter = new MobileSales.MyMobileDataSetTableAdapters.ArtikujDepoTableAdapter();
            emriLabel = new System.Windows.Forms.Label();
            cmimiNjesiLabel = new System.Windows.Forms.Label();
            sasiaPakoLabel = new System.Windows.Forms.Label();
            sasiaLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.artikujDepoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDataset)).BeginInit();
            this.SuspendLayout();
            // 
            // emriLabel
            // 
            emriLabel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            emriLabel.Location = new System.Drawing.Point(59, 69);
            emriLabel.Name = "emriLabel";
            emriLabel.Size = new System.Drawing.Size(30, 14);
            emriLabel.Text = "Emri:";
            // 
            // cmimiNjesiLabel
            // 
            cmimiNjesiLabel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            cmimiNjesiLabel.Location = new System.Drawing.Point(55, 103);
            cmimiNjesiLabel.Name = "cmimiNjesiLabel";
            cmimiNjesiLabel.Size = new System.Drawing.Size(38, 14);
            cmimiNjesiLabel.Text = "Cmimi:";
            // 
            // sasiaPakoLabel
            // 
            sasiaPakoLabel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            sasiaPakoLabel.Location = new System.Drawing.Point(29, 132);
            sasiaPakoLabel.Name = "sasiaPakoLabel";
            sasiaPakoLabel.Size = new System.Drawing.Size(60, 14);
            sasiaPakoLabel.Text = "Sasia pako:";
            // 
            // sasiaLabel
            // 
            sasiaLabel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            sasiaLabel.Location = new System.Drawing.Point(49, 157);
            sasiaLabel.Name = "sasiaLabel";
            sasiaLabel.Size = new System.Drawing.Size(40, 14);
            sasiaLabel.Text = "Shifra:";
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menDlaja);
            // 
            // menDlaja
            // 
            this.menDlaja.Text = "Dalja";
            this.menDlaja.Click += new System.EventHandler(this.menDlaja_Click);
            // 
            // emriTextBox
            // 
            this.emriTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.artikujDepoBindingSource, "Emri", true));
            this.emriTextBox.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.emriTextBox.Location = new System.Drawing.Point(99, 69);
            this.emriTextBox.Name = "emriTextBox";
            this.emriTextBox.ReadOnly = true;
            this.emriTextBox.Size = new System.Drawing.Size(132, 19);
            this.emriTextBox.TabIndex = 2;
            // 
            // artikujDepoBindingSource
            // 
            this.artikujDepoBindingSource.DataMember = "ArtikujDepo";
            this.artikujDepoBindingSource.DataSource = this.myMobileDataset;
            // 
            // myMobileDataset
            // 
            this.myMobileDataset.DataSetName = "myMobileDataSet";
            this.myMobileDataset.Prefix = "";
            this.myMobileDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // cmimiNjesiTextBox
            // 
            this.cmimiNjesiTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.artikujDepoBindingSource, "Cmimi", true));
            this.cmimiNjesiTextBox.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.cmimiNjesiTextBox.Location = new System.Drawing.Point(99, 98);
            this.cmimiNjesiTextBox.Name = "cmimiNjesiTextBox";
            this.cmimiNjesiTextBox.ReadOnly = true;
            this.cmimiNjesiTextBox.Size = new System.Drawing.Size(71, 19);
            this.cmimiNjesiTextBox.TabIndex = 3;
            // 
            // sasiaPakoTextBox
            // 
            this.sasiaPakoTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.artikujDepoBindingSource, "Sasia", true));
            this.sasiaPakoTextBox.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.sasiaPakoTextBox.Location = new System.Drawing.Point(99, 125);
            this.sasiaPakoTextBox.Name = "sasiaPakoTextBox";
            this.sasiaPakoTextBox.ReadOnly = true;
            this.sasiaPakoTextBox.Size = new System.Drawing.Size(71, 19);
            this.sasiaPakoTextBox.TabIndex = 5;
            // 
            // lblLine
            // 
            this.lblLine.BackColor = System.Drawing.Color.Black;
            this.lblLine.Location = new System.Drawing.Point(9, 58);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(222, 2);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(63, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 14);
            this.label1.Text = "Detalet e artikullit";
            // 
            // sasiaTextBox
            // 
            this.sasiaTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.artikujDepoBindingSource, "IDArtikulli", true));
            this.sasiaTextBox.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.sasiaTextBox.Location = new System.Drawing.Point(99, 152);
            this.sasiaTextBox.Name = "sasiaTextBox";
            this.sasiaTextBox.ReadOnly = true;
            this.sasiaTextBox.Size = new System.Drawing.Size(71, 19);
            this.sasiaTextBox.TabIndex = 9;
            // 
            // artikujDepoTableAdapter
            // 
            this.artikujDepoTableAdapter.ClearBeforeFill = true;
            // 
            // DetArtikujve
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.ControlBox = false;
            this.Controls.Add(sasiaLabel);
            this.Controls.Add(this.sasiaTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblLine);
            this.Controls.Add(sasiaPakoLabel);
            this.Controls.Add(this.sasiaPakoTextBox);
            this.Controls.Add(cmimiNjesiLabel);
            this.Controls.Add(this.cmimiNjesiTextBox);
            this.Controls.Add(emriLabel);
            this.Controls.Add(this.emriTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Menu = this.mainMenu1;
            this.Name = "DetArtikujve";
            this.Text = "Artikujt";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmDetArtikujve_Load);
            ((System.ComponentModel.ISupportInitialize)(this.artikujDepoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myMobileDataset)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private MyMobileDataSet myMobileDataset;
        private System.Windows.Forms.TextBox emriTextBox;
        private System.Windows.Forms.MenuItem menDlaja;
        private System.Windows.Forms.TextBox cmimiNjesiTextBox;
        private System.Windows.Forms.TextBox sasiaPakoTextBox;
        private System.Windows.Forms.Label lblLine;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox sasiaTextBox;
        private System.Windows.Forms.BindingSource artikujDepoBindingSource;
        private MobileSales.MyMobileDataSetTableAdapters.ArtikujDepoTableAdapter artikujDepoTableAdapter;
    }
}