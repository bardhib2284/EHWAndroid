namespace MobileSales
{
    partial class frmVerejtjet
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
            this.menDalja = new System.Windows.Forms.MenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbArsyeja = new System.Windows.Forms.ComboBox();
            this.btnGet = new System.Windows.Forms.Button();
            this.dsVerejtjet = new System.Data.DataSet();
            ((System.ComponentModel.ISupportInitialize)(this.dsVerejtjet)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menDalja);
            // 
            // menDalja
            // 
            this.menDalja.Text = "Dalja";
            this.menDalja.Click += new System.EventHandler(this.menDalja_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(211, 20);
            this.label1.Text = "Selktoni arsyjen e kthimit të mallit";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 20);
            this.label2.Text = "Arsyeja:";
            // 
            // cmbArsyeja
            // 
            this.cmbArsyeja.Location = new System.Drawing.Point(76, 97);
            this.cmbArsyeja.Name = "cmbArsyeja";
            this.cmbArsyeja.Size = new System.Drawing.Size(138, 22);
            this.cmbArsyeja.TabIndex = 2;
            // 
            // btnGet
            // 
            this.btnGet.Location = new System.Drawing.Point(93, 138);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(72, 20);
            this.btnGet.TabIndex = 3;
            this.btnGet.Text = "Shto";
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // dsVerejtjet
            // 
            this.dsVerejtjet.DataSetName = "NewDataSet";
            this.dsVerejtjet.Namespace = "";
            this.dsVerejtjet.Prefix = "";
            this.dsVerejtjet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // frmVerejtjet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(240, 295);
            this.ControlBox = false;
            this.Controls.Add(this.btnGet);
            this.Controls.Add(this.cmbArsyeja);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Location = new System.Drawing.Point(0, 0);
            this.MaximizeBox = false;
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "frmVerejtjet";
            this.Text = "Lista e arsyjeve";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmVerejtjet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dsVerejtjet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbArsyeja;
        private System.Windows.Forms.Button btnGet;
        private System.Windows.Forms.MenuItem menDalja;
        private System.Data.DataSet dsVerejtjet;
    }
}