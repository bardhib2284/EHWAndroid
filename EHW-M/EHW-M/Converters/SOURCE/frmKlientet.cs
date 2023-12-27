using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Data.SqlServerCe;
using Microsoft.WindowsMobile.Forms;
using System.IO;
using System.Reflection;


namespace MobileSales
{
    public partial class frmKlientet : Form
    {
        public static DataTable AppDataTable = new DataTable();
        private DateTime dtFisrtDate,dtLastDate;
        private int currentDay, effectRows=0;
        private string IDLokacioni;
        public static Guid IDVizita;
        public static string strIDVizita;
        public static DateTime DataLiferimit;
        public static Guid IDLiferimi;
        public static string Position;
        private bool isSelected, AnyKeys;
        private DateTime DataPlanifikimit = DateTime.Now.Date;  //Data e Arritjes
        private DateTime OraPlanifikimit = DateTime.Now;        //Ora e arritjes
        private bool ExistOrder, NewOrder;

        #region Initialize Instance

        frmArtikujtShitur frmArtikujtShitur = null;
        frmShitja frmShitja = null;
        
        #endregion

        public frmKlientet()
        {
            InitializeComponent();
        }

        private void Klientet_Load(object sender, EventArgs e)
        {
            vizitatTableAdapter1.Connection.ConnectionString = frmHome.AppDBConnectionsStr;
            listaVizitaveTableAdapter.Connection.ConnectionString = frmHome.AppDBConnectionsStr;
            klientDheLokacionTableAdapter1.Connection.ConnectionString = frmHome.AppDBConnectionsStr;
            klientetTableAdapter.Connection.ConnectionString = frmHome.AppDBConnectionsStr;
           
            CultureInfo myCI = new CultureInfo("fr-FR"); //FS. cdo here te hapet forma instancim i objektit pa dispose().
            System.Globalization.Calendar myCal = myCI.Calendar;
            currentDay = (int)myCal.GetDayOfWeek(DateTime.Now);
            dtFisrtDate = DateTime.Now.Date.AddDays(-(currentDay - 1));
            dtLastDate = dtFisrtDate.AddDays(6);
            if (MyMobileDataSetUtil.DesignerUtil.IsRunTime())
            {
                this.vizitatTableAdapter1.GetLokacionet(this.myMobileDataset.Vizitat, frmHome.IDAgjenti, dtFisrtDate, dtLastDate);
                string IDKlientLokacon = this.myMobileDataset.Vizitat[0]["IDKlientDheLokacion"].ToString();
                this.klientDheLokacionTableAdapter1.FillByIDKlientDheLokacion(this.myMobileDataset.KlientDheLokacion, IDKlientLokacon);
                string IDKlienti = this.myMobileDataset.KlientDheLokacion[0]["IDKlienti"].ToString();
                this.klientetTableAdapter.FillByIDKlienti(this.myMobileDataset.Klientet, IDKlienti);
                this.listaVizitaveTableAdapter.FillByAgentAndDate(this.myMobileDataset.ListaVizitave, frmHome.IDAgjenti, dtFisrtDate, dtLastDate);
             
            }
           Cursor.Current= Cursors.Default;
        }

        private void menuShfaq_Click(object sender, EventArgs e)
        {
            if (isSelected)//isSelected=True
            {
                IDVizita = new Guid(lblIDVizita.Text);
                strIDVizita = IDVizita.ToString();
                Position = "Klientet";
                if (frmArtikujtShitur == null) frmArtikujtShitur = new frmArtikujtShitur();
                frmArtikujtShitur.pos = Position;
                frmArtikujtShitur.ShowDialog();
            }
        }
        
        private void menuShitja_Click(object sender, EventArgs e)
        {
            Guid NewIDVizita = Guid.NewGuid();
            if (IDLokacioni != "" && isSelected) // Nese selektojme nje kliente ne liste
            {
                try
                {
                   // effectRows =
                         //this.vizitatTableAdapter1.SearchVizitat(this.myMobileDataset.Vizitat, DataPlanifikimit, IDLokacioni);
                    if (effectRows == 1) // Nese klienti i zgjedhur  eshte ne agjenden ditore
                    {

                        if (MessageBox.Show("Dëshironi të krijoni një porosi të re?", "Porosi",
                       MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            ExistOrder = true;

                        }

                    }
                    else
                    {
                        try
                        {

                            this.vizitatTableAdapter1.InsertQuery(DataPlanifikimit, OraPlanifikimit, frmHome.IDAgjenti, IDLokacioni.ToString(), frmHome.DevID, NewIDVizita);
                            NewOrder = true;
                        }
                        catch (SqlCeException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                    }

                }
                catch (SqlCeException ex)
                {
                    if (ex.NativeError == 25016)
                    {
                        MessageBox.Show("Gabim:\nKlienti:" + emriComboBox.SelectedText.ToString() + " egziston në agjndën ditore");
                    }
                        
                    else
                    {
                        MessageBox.Show("Gabim: " + ex.Message);
                    }
                }

                if (ExistOrder)// Nese kemi krijuar nje vizite sipas kushteve te kerkuara , kthehemi te Forma Vizitat
                {
                    int IdvColPos = 0;
                    if (isSelected)
                    {
                        frmVizitat.CurrIDV = new Guid(lblIDVizita.Text);
                    }
                    else
                    {
                        this.vizitatTableAdapter1.FillByIDLokacioni(this.myMobileDataset.Vizitat, IDLokacioni, DataPlanifikimit);
                        frmVizitat.CurrIDV = new Guid(myMobileDataset.Vizitat[0][IdvColPos].ToString());
                    }
                    ExistOrder = false;

                }
                if (NewOrder)
                {

                    this.vizitatTableAdapter1.Update(this.myMobileDataset.Vizitat);
                    frmVizitat.CurrIDV = NewIDVizita;

                }

                DataRow test = ((DataRowView)(listaVizitaveBindingSource.Current)).Row;
                frmVizitat.IDPorosi = frmVizitat.NONE;
                frmVizitat.IDLiferimi = frmVizitat.NONE;
                Cursor.Current = Cursors.WaitCursor;

                if (frmShitja == null) frmShitja = new frmShitja();
                frmShitja.ShowDialog();
                this.listaVizitaveTableAdapter.FillByAgentAndDate(this.myMobileDataset.ListaVizitave, frmHome.IDAgjenti, dtFisrtDate, dtLastDate);
                NewOrder = false;
            }
            else
            {
                MessageBox.Show("Selektoni nje klient!");
            }
        }

        private void listaVizitaveDataGrid_MouseUp(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.DataGrid.HitTestInfo myHitTest = this.listaVizitaveDataGrid.HitTest(e.X, e.Y);
            if (myHitTest.Type == DataGrid.HitTestType.Cell)
            {
                this.listaVizitaveDataGrid.Select(myHitTest.Row);
                IDLokacioni = txtIDKlientDheLokacion.Text;
                isSelected = true;
                if (testStatLabel1.Text != "")
                {
                    menShfaq.Enabled = true;
                }
                else
                {
                    menShfaq.Enabled=false;
                }
            }
        }

        private void listaVizitaveDataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            AnyKeys =
          (e.KeyData == Keys.Enter || e.KeyData == Keys.Up || e.KeyData == Keys.Down || e.KeyData == Keys.Left || e.KeyData == Keys.Right);
            if (isSelected == true)
            {
                if (AnyKeys)
                {
                    this.listaVizitaveDataGrid.Select(this.listaVizitaveDataGrid.CurrentRowIndex);
                }

            }
        }

        private void menuDalja_Click(object sender, EventArgs e)
        {
            this.Close();//Mbyllja e formes
        }

       
    }
}