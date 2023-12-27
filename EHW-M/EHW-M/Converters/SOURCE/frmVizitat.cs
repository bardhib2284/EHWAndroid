using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using PnUtils;
using Microsoft.WindowsMobile.Forms;
using System.IO;
using System.Reflection;
using MobileSales.BL;
using PnSync;

namespace MobileSales
{
    public partial class frmVizitat : Form
    {
        private DateTime DataCal = System.DateTime.Now.Date, dtFisrtDate, dtLastDate;
        public static bool DataGridRefreshed = false, RegjistroViziten = false, KthimMall = false, ShitjeKorrigjim = false;
        public static DataTable AppDataTable = new DataTable();
        public const int COL_Por = 1, COL_Lif = 2, COL_LfrStatus = 3, COL_DtPerLiferim = 4, COL_TitPorosise = 5; //COL_ListNePritje = 3
        public static Guid NONE = new Guid("00000000-0000-0000-0000-000000000000");
        public static Guid NAV = new Guid("10000000-0000-0000-0000-000000000000");
        public static Guid IDPorosi = NAV, IDLiferimi = NAV, LfrStat = NAV, ListaNePritje = NAV, CurrIDV = NAV;
        public string SqlString = "";
        private bool Key, isSelected = false;
        private int currentDay;
        public static DateTime selectedDate;
        private string StatVizites = "6"; // 6 Vizita eshte e mbyllur 
        public static string Vizitat_IDKlienti, Emri, EmriLokacionit, StatusiVizites, DtPerLiferim = "", TitPorosise = "", NrFatures;//Emri->EmriKlientit, Lokacioni->Emriokacionit
        public static int numberOFVizits;
        public static float _TVHS;

        #region Initialize Instace

        frmMeny frmAksioni = null;
        frmListaShitjeve frmListaShitjeve = null;
        frmShitja frmShitja = null;
        RegjistrimiVizites frmRegjistrimiVizites = null;
        RegjistroVizite regjVizite = null;
        frmStoku frmStoku = null;
        frmPrintInkasimet frmPrintInkasimet = null;
        Lib libLog = new Lib();

        #endregion

        public frmVizitat()
        {
            InitializeComponent();

        }

        private void frmVizitat_Load(object sender, EventArgs e)
        {
            InitControls();
            InitConnection();
            CultureInfo myCI = new CultureInfo("fr-FR");
            System.Globalization.Calendar myCal = myCI.Calendar;
            currentDay = (int)myCal.GetDayOfWeek(DateTime.Now);
            dtFisrtDate = DateTime.Now.Date.AddDays(-(currentDay - 1));
            dtLastDate = dtFisrtDate.AddDays(6);
            FillTimeCombobox();
            Application.DoEvents();            
            FillVizitatDataGrid();
            txbKompani.Focus();
            frmHome.AppDBAdapter.SelectCommand = new SqlCeCommand(SqlString, frmHome.AppDBConnection); //FS. mund te shkaktoj MemoryLeak,se cdo here qe hapet forma inscon SqlConnection te re (si objekt i ri)
            Cursor.Current = Cursors.Default;
            if (frmHome.ApplicationMode == "AMB")
            {
                btnLiferim.Visible = false;
                btnPorosi.Text = "Shitja";
            }
        }

        private void frmVizitat_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == System.Windows.Forms.Keys.Up))
            {
                // Up
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Down))
            {
                // Down
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Left))
            {
                // Left
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Right))
            {
                // Right
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Enter))
            {
                // Enter
            }
        }

        private void menuDalja_Click(object sender, EventArgs e)
        {
            DataGridRefreshed = false;
            if (frmAksioni == null) frmAksioni = new frmMeny();
            frmAksioni.Show();
            this.Close();
            this.Dispose();

        }

        private void menuFaturat_Click(object sender, EventArgs e)
        {
            // new ListaShitjeve().ShowDialog(); //FS. Instancim direkt i objektit, pa dispose(). Ngarkim i panevojshem i memorjes
            if (frmListaShitjeve == null) frmListaShitjeve = new frmListaShitjeve();
            frmListaShitjeve.ShowDialog();
            Cursor.Current = Cursors.Default;
        }

        private void btnPorosi_Click(object sender, EventArgs e)
        {
            if (isSelected == true && listaVizitaveDataGrid.VisibleRowCount != 0)
            {

                if (exsistStatus0(frmVizitat.CurrIDV))
                {

                    if (!excistTVSH())//Kontrollojme se a ekziston Tvsh ne Tabeln CompanyInfo
                    {
                        MessageBox.Show("Vlera e TVSH-se nuk është e përcaktuar \n Tabela:CompanyInfo \n Shitja nuk mund të vazhdoj");
                        return;
                    }
                    NrFatures = getNurFatures(frmHome.Depo);
                    if (NrFatures == "Tejkalim" || NrFatures == "Error:Konfigurim")
                    {
                        MessageBox.Show("Kufiri i numrit të faturave nuk është përcaktuar \n Shitja nuk mund të vazhdoj");
                        return;
                    }
                    IDPorosi = NONE;
                    IDLiferimi = NONE;
                    Cursor.Current = Cursors.WaitCursor;
                    if (frmShitja == null) frmShitja = new frmShitja();
                    frmShitja.ShowDialog();
                    if (!frmShitja.isClosed)
                    {
                        //Need to refresh DataGrid
                        this.vizitatTableAdapter.UpdateViziten(StatVizites, DataCal, DateTime.Now.ToLocalTime(), CurrIDV);
                        DataGridRefreshed = false;
                        FillVizitatDataGrid();
                        UpdaterRows();
                        this.listaVizitaveDataGrid.Update();
                        textBox1.Text = string.Empty;
                    }

                    GC.Collect();
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    MessageBox.Show("Vizita nuk eshte e hapur!");
                }
            }


            //Shtuar nga Edin Verbiqi me date 19.12.2016
            isSelected = false;
            //
        }

        private void menuRegjistroViziten_Click(object sender, EventArgs e)
        {

            //if (isSelected == true)
            //{
            //    MessageBox.Show("Vizita është e myllur \n Nuk mund ti ndryshohet statusi");
            //    return;
            //}
            if (isSelected == true)
            {
                if (exsistStatus0onGridwithID(CurrIDV))
                {
                    if (checkOraRealizimit(CurrIDV))
                    {

                        string _updateCommand = "";

                        _updateCommand = @"UPDATE Vizitat
                                     SET DataAritjes = GETDATE(), Longitude = '"+frmHome.LongitudeCur+"', Latitude = '"+frmHome.LatitudeCur+"' where IDVizita = '" + frmVizitat.CurrIDV + "' and (DataAritjes is null or (DATEADD(dd, 0, DATEDIFF(dd, 0, DataAritjes))) <> (DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE()))))";
                        DbUtils.ExecSql(_updateCommand);


                        RegjistroViziten = true;
                        Emri = lblEmri.Text;
                        EmriLokacionit = lblLokacioni.Text;
                        if (frmRegjistrimiVizites == null) frmRegjistrimiVizites = new RegjistrimiVizites();
                        frmRegjistrimiVizites.ShowDialog();
                        if (!RegjistrimiVizites.isClosed)
                        {
                            //Need to refresh DataGrid
                            DataGridRefreshed = false;
                            FillVizitatDataGrid();
                            UpdaterRows();
                            this.listaVizitaveDataGrid.Update();
                        }
                        Cursor.Current = Cursors.Default;
                        RegjistroViziten = false;
                    }
                    else
                    {
                        MessageBox.Show("Duhet të shtohet Vizitë e re për këtë klient!");
                        return;
                    }
                }
                else
                {
                    string EmriKlientit = "";
                    string _selectQuery = "";
                    try
                    {
                        _selectQuery = @"SELECT top(1) k.Emri
                                       FROM   Klientet k inner join Vizitat v on v.IDKlientDheLokacion = k.IDKlienti 
                                      WHERE  v.IDStatusiVizites = 0 and v.IDVizita <> '" + frmVizitat.CurrIDV + "'";

                        EmriKlientit = PnUtils.DbUtils.ExecSqlScalar(_selectQuery).ToString();

                    }
                    catch (Exception ex)
                    {
                        DbUtils.WriteExeptionErrorLog(ex);

                    }
                    MessageBox.Show("Ekziston Vizitë e hapur për klientin: " + EmriKlientit + ", \n Ju lutem mbylleni Vizitë para se të shtoni një të re!");
                    return;
                }

            }
            else
            {
                MessageBox.Show("Selekto Vizitën!");
                return;
            }
        }

        private void menuKthimi_Click(object sender, EventArgs e)
        {
            if (isSelected)
            {

                if (exsistStatus0(CurrIDV))
                {
                    if (MessageBox.Show("Jeni të sigurt për kthim të artikujve?", "Verejtje", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        if (IDPorosi != NONE)
                        {
                            IDPorosi = NONE;
                            IDLiferimi = NONE;
                        }
                        NrFatures = getNurFatures(frmHome.IDAgjenti);
                        if (NrFatures == "Error" || NrFatures == "Error:Konfigurim")
                        {
                            MessageBox.Show("Nuk është i caktuar numri i kufive të faturave \n shitja nuk mund të vazhdojë");
                            return;
                        }
                        if (!excistTVSH())
                        {
                            MessageBox.Show("Vlera e TVSH-se nuk është e përcaktuar \n Tabela:CompanyInfo");
                            return;
                        }
                        else
                        {

                            KthimMall = true;
                            Cursor.Current = Cursors.WaitCursor;
                            //new Shitja().ShowDialog();//FS. Instancim direkt i objektit, pa dispose(). Ngarkim i panevojshem i memorjes
                            if (frmShitja == null) frmShitja = new frmShitja();
                            frmShitja.ShowDialog();
                            this.vizitatTableAdapter.UpdateViziten(StatVizites, DataCal, DateTime.Now.ToLocalTime(), CurrIDV);


                            KthimMall = false;
                            // Need to refresh DataGrid
                            DataGridRefreshed = false;
                            FillVizitatDataGrid();
                            UpdaterRows();
                            this.listaVizitaveDataGrid.Update();
                            txbKompani.Focus();
                            GC.Collect();
                            Cursor.Current = Cursors.Default;

                        }

                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Vizita nuk eshte e hapur!");
                }
            }


            //Shtuar nga Edin Verbiqi me date 20.12.2016
            isSelected = false;
            //
        }

        private void menuStoku_Click(object sender, EventArgs e)
        {
            if (frmStoku == null) frmStoku = new frmStoku();
            frmStoku.ShowDialog();
        }

        private void menuShtoVizite_Click(object sender, EventArgs e)
        {
            if (exsistStatus0onGrid())
            {
                numberOFVizits = listaVizitaveBindingSource.Count;
                if (regjVizite == null) regjVizite = new RegjistroVizite();
                DialogResult a = regjVizite.ShowDialog();
                if (a == DialogResult.Abort)
                {
                    regjVizite = null;
                    return;
                }
                else
                {
                    DataGridRefreshed = false;
                    FillVizitatDataGrid();
                    regjVizite = null;
                }
            }
            else
            {
                string EmriKlientit = "";
                string _selectQuery = "";
                try
                {
                    _selectQuery = @"SELECT top(1) k.Emri
                                       FROM   Klientet k inner join Vizitat v on v.IDKlientDheLokacion = k.IDKlienti 
                                      WHERE  v.IDStatusiVizites = 0";

                    EmriKlientit = PnUtils.DbUtils.ExecSqlScalar(_selectQuery).ToString();
                    
                }
                catch (Exception ex)
                {
                    DbUtils.WriteExeptionErrorLog(ex);

                }
                MessageBox.Show("Ekziston Vizitë e hapur për klientin: " + EmriKlientit + ", \n Ju lutem mbylleni Vizitë para se të shtoni një të re!");
                return;
            }
        }

        private void menuMbetja_Click(object sender, EventArgs e)
        {
            new frmMbetja().ShowDialog();
        }

        private void menuInkasimet_Click(object sender, EventArgs e)
        {
            if (frmPrintInkasimet == null) frmPrintInkasimet = new frmPrintInkasimet();
            frmPrintInkasimet.ShowDialog();
        }

        #region DataGrid && BindingSource
        private void listaVizitaveDataGrid_GotFocus(object sender, EventArgs e)
        {
            if (listaVizitaveDataGrid.VisibleRowCount != 0)
            {
                this.listaVizitaveDataGrid.Select(listaVizitaveDataGrid.CurrentRowIndex);
                CurrIDV = new Guid(lblIDVizita.Text);
            }
        }
        private void listaVizitaveDataGrid_MouseUp(object sender, MouseEventArgs e)
        {
            //Shtuar nga Edin Verbiqi me date 19.12.2016
            isSelected = false;
            //
            System.Windows.Forms.DataGrid.HitTestInfo myHitTest = this.listaVizitaveDataGrid.HitTest(e.X, e.Y);
            if (myHitTest.Type == DataGrid.HitTestType.Cell)
            {
                this.listaVizitaveDataGrid.Select(myHitTest.Row);
                isSelected = true;
                CurrIDV = new Guid(lblIDVizita.Text);
                Vizitat_IDKlienti = lblIDKlienti.Text;
                StatusiVizites = lblStatusiVizites.Text;
                if (prsTextBox.Text.CompareTo(NAV.ToString()) == 0 || lfrTextBox.Text.CompareTo(NAV.ToString()) == 0)
                {
                    UpdaterRows();
                }
                else
                {
                    UpdaterRows();
                    UpdateBtnsCache();

                }

            }
        }

        private void listaVizitaveDataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            Key = (e.KeyData == Keys.Enter || e.KeyData == Keys.Up || e.KeyData == Keys.Down || e.KeyData == Keys.Left || e.KeyData == Keys.Right);
            if (isSelected == true)
            {
                if (Key)
                {
                    this.listaVizitaveDataGrid.Select(this.listaVizitaveDataGrid.CurrentRowIndex);
                    Vizitat_IDKlienti = lblIDKlienti.Text;
                    CurrIDV = new Guid(lblIDVizita.Text);
                    UpdaterRows();
                }

            }

        }
        #endregion

        #region Methods

        private void FillTimeCombobox() 
        {

            comboBoxTimePeriod.Items.Insert(0,"E Diele");
            comboBoxTimePeriod.Items.Insert(1,"E Hënë");
            comboBoxTimePeriod.Items.Insert(2,"E Martë");
            comboBoxTimePeriod.Items.Insert(3,"E Mërkurë");
            comboBoxTimePeriod.Items.Insert(4,"E Enjtë");
            comboBoxTimePeriod.Items.Insert(5,"E Premte");
            comboBoxTimePeriod.Items.Insert(6,"E Shtune");
            comboBoxTimePeriod.Items.Insert(7,"Java");

            comboBoxTimePeriod.SelectedIndex = (int)DateTime.Now.DayOfWeek;
        }

        private void FillVizitatDataGrid()
        {
            //@BE 03.11.2018 Shtuar per aplikim te Filtrit per Periudhe te Vizitave
            DateTime FromDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
            DateTime ToDate = FromDate;


            if (comboBoxTimePeriod.SelectedIndex == 7)
            {
                FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day);

                ToDate = FromDate.AddDays(7);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 59, 998);
            }
            else
            {
                FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day).AddDays(comboBoxTimePeriod.SelectedIndex);
                ToDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 23, 59, 59, 998);
            }
            //@BE 03.11.2018 Perfundim te shtimit

            if (DataGridRefreshed == true) return;	//if refresh is needed, must put DataGridRefreshed TRUE
            try
            {
                this.myMobileDataSet.EnforceConstraints = false;
                //this.listaVizitaveTableAdapter.FillByDeviceIDandNumber(this.myMobileDataSet.ListaVizitave, frmHome.DevID);
                //@BE 03.11.2018 kodi i komentuar siper eshte zevendesuar me kete poshte per te perfshire Combobox te periudhes ne filter.
                this.listaVizitaveTableAdapter.FillByDevIdAndNumberAndPeriodTillEndOfWeek(this.myMobileDataSet.ListaVizitave, frmHome.DevID, FromDate, ToDate);
                DataGridRefreshed = true;

            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                DbUtils.WriteExeptionErrorLog(ex);
            }
        }

        private void InitControls()
        {
            if (frmHome.AprovimFaturash)
            {
                this.BackColor = Color.WhiteSmoke;
                kontaktEmriMbiemriLabel1.BackColor = Color.Gainsboro;
                adresaLabel1.BackColor = Color.Gainsboro;
                tel_MobilLabel1.BackColor = Color.Gainsboro;

            }
            else
            {
                this.BackColor = Color.Silver;
            }
        }

        private void InitConnection()
        {
            porosiaArtTableAdapter.Connection.ConnectionString = frmHome.AppDBConnectionsStr;
            listaVizitaveTableAdapter.Connection.ConnectionString = frmHome.AppDBConnectionsStr;
            vizitatTableAdapter.Connection.ConnectionString = frmHome.AppDBConnectionsStr;
            klientDheLokacionTableAdapter.Connection.ConnectionString = frmHome.AppDBConnectionsStr;
        }

        public static string getNurFatures(string KMAG)
        {
            string result = "";
            int NrFat = 0;
            bool aprovim = frmHome.AprovimFaturash;
            string sql = "SELECT Count(*) FROM NumriFaturave nf WHERE nf.KOD='" + KMAG + "'";
            int _numRows = int.Parse(DbUtils.ExecSqlScalar(sql));
            if (_numRows == 0)
            {
                result = "Error:Konfigurim";

            }
            else
            {

                if (aprovim)//aprovim=1
                {

                    /*(NumriFaturave = nf)
                     *Lexo nf.CurrNrFat, nf.NRKUFIP, nf.NRKUFIS where nf.KOD='" + KMAG + "'"
                     *NrFatures = nf.CurrNrFat + nf.NRKUFIP
                     *nese NrFatures > nf.NRKUFIS, ndalet shitja.
                     *nf.CurrNrFat = nf.CurrNrFat + 1
                     **/

                    ///*Marrim sa fatura deri me tani i kemi realizuarme kete brez
                    string _getCurrNrFat = "SELECT nf.CurrNrFat FROM NumriFaturave nf WHERE nf.KOD='" + KMAG + "'";
                    int _CurrNrFat = int.Parse(DbUtils.ExecSqlScalar(_getCurrNrFat).ToString());

                    ///*Marrim NRKUFIP per shitesin konkret */
                    string _getNRKUFIP = "SELECT nf.NRKUFIP FROM NumriFaturave nf WHERE nf.KOD='" + KMAG + "'";
                    int tempNRKUFIP = int.Parse(DbUtils.ExecSqlScalar(_getNRKUFIP));

                    ///*Marrim NRKUFIS per shitesin konkret */
                    string _getNRKUFIS = "SELECT nf.NRKUFIS FROM NumriFaturave nf WHERE nf.KOD='" + KMAG + "'";
                    int tempNRKUFIS = int.Parse(DbUtils.ExecSqlScalar(_getNRKUFIS));

                    NrFat = _CurrNrFat + tempNRKUFIP;

                    if (NrFat > tempNRKUFIS)
                    {
                        MessageBox.Show("Tejkalimi i numrit te faturave");
                        result = "Tejkalim";
                    }
                    else
                    {
                        result = NrFat.ToString();

                    }

                }
                else //aprovim=0
                {

                    ///*Marrim sa fatura deri me tani i kemi realizuarme kete brez
                    string _getCurrNrFat = "SELECT nf.CurrNrFatJT FROM NumriFaturave nf WHERE nf.KOD='" + KMAG + "'";
                    int _CurrNrFatJT = int.Parse(DbUtils.ExecSqlScalar(_getCurrNrFat).ToString());

                    /*Marrim NRKUFIPJT per shitesin konkret */
                    string _getNRKUFIPJT = "SELECT nf.NRKUFIPJT FROM NumriFaturave nf WHERE nf.KOD='" + KMAG + "'";
                    int tempNRKUFIPJT = int.Parse(DbUtils.ExecSqlScalar(_getNRKUFIPJT));

                    /*Marrim NRKUFISJT per shitesin konkret */
                    string _getNRKUFISJT = "SELECT nf.NRKUFISJT FROM NumriFaturave nf WHERE nf.KOD='" + KMAG + "'";
                    int temoNRKUFISJT = int.Parse(DbUtils.ExecSqlScalar(_getNRKUFISJT));

                    /*Kontrollojme tejkalimin eventual ne NRKUFIP dhe NRKUFIS */
                    NrFat = _CurrNrFatJT + tempNRKUFIPJT;

                    if (NrFat > temoNRKUFISJT)
                    {
                        MessageBox.Show("Tejkalimi i numrit te faturave");
                        result = "Tejkalim";
                    }
                    else
                    {
                        result = NrFat.ToString();

                    }
                }
            }
            return result;
        }

        private bool excistTVSH()
        {
            bool _result = false;
            string _selectQuery = "";
            try
            {
                _selectQuery = @"SELECT CASE 
                                             WHEN ci.[Value] IS NOT NULL THEN ci.[Value] 
                                          ELSE 0 
                                       END AS VA 
                                       FROM   CompanyInfo ci 
                                       WHERE  ci.Item = 'TVSH'";

                _TVHS = int.Parse(PnUtils.DbUtils.ExecSqlScalar(_selectQuery));
                if (_TVHS == 0)
                {
                    _result = false;
                }
                else
                {
                    _result = true;
                }
            }
            catch (Exception ex)
            {
                DbUtils.WriteExeptionErrorLog(ex);

            }
            return _result;
        }

        private void UpdateBtnsCache()
        {
            IDPorosi = new Guid(prsTextBox.Text);
            IDLiferimi = new Guid(lfrTextBox.Text);

        }

        private void UpdaterRows()
        {
            //FS. UpdatarRows() thirret ne MouseUp te Gridit, Kjo metode thret nje metode tjeter UpdateBtns ku kemi istancime
            //FS. cdo here ne MousUp. Mund te shkaktoje memoryLeak

            //Prepare qry to get nr of Porosi and Liferim when a vizita-row is selected
            string _selectQuery = "";
            try
            {
                _selectQuery = "SELECT Vizitat.IDVizita, Porosite.IDPorosia, Liferimi.IDLiferimi, Liferimi.Liferuar, Porosite.DataPerLiferim, Porosite.TitulliPorosise FROM Vizitat LEFT OUTER JOIN Porosite ON Vizitat.IDVizita = Porosite.IDVizita LEFT OUTER JOIN Liferimi ON Porosite.IDPorosia = Liferimi.IDPorosia WHERE (Vizitat.IDVizita = '" + CurrIDV.ToString() + "')";
                frmHome.AppDBAdapter.SelectCommand.CommandText = _selectQuery;
                AppDataTable.Clear();
                frmHome.AppDBAdapter.Fill(AppDataTable);
                if (AppDataTable.Rows.Count != 0) // we can have more than anu row 
                {
                    DataRow[] rows = AppDataTable.Select();
                    UpdateBtns(rows);
                }
            }
            catch (SqlCeException sce)
            {
                MessageBox.Show(sce.Message);
                DbUtils.WriteSQLCeErrorLog(sce, _selectQuery);

            }
        }

        private void UpdateBtns(DataRow[] rows)
        {
            int aCol; bool isColValNotNull;

            aCol = COL_Por;
            isColValNotNull = (rows[0][aCol] != null) && (Convert.ToString(rows[0][aCol])) != "" ? true : false;
            IDPorosi = isColValNotNull ? new Guid((rows[0][aCol]).ToString()) : NONE;
            prsTextBox.Text = IDPorosi.ToString();


            aCol = COL_Lif;
            isColValNotNull = (rows[0][aCol] != null) && (Convert.ToString(rows[0][aCol])) != "" ? true : false;
            IDLiferimi = isColValNotNull ? new Guid((rows[0][aCol]).ToString()) : NONE;
            lfrTextBox.Text = IDLiferimi.ToString();

            aCol = COL_DtPerLiferim;
            DtPerLiferim = isColValNotNull ? Convert.ToString(rows[0][aCol]) : "";
            aCol = COL_TitPorosise;
            TitPorosise = isColValNotNull ? Convert.ToString(rows[0][aCol]) : "";


            if (IDLiferimi == frmVizitat.NONE)
            {

                aCol = COL_LfrStatus;
                isColValNotNull = (rows[0][aCol] != null) && (Convert.ToString(rows[0][aCol])) != "" ? true : false;
                LfrStat = isColValNotNull ? new Guid((rows[0][aCol]).ToString()) : NAV;
                lfrStatusTextBox.Text = LfrStat.ToString();
            }
            else
            {
                LfrStat = NAV;
            }
            UpdateBtnsCache();
        }

        public bool HasDebt(string IDKlienti)
        {
            bool res = false;

            if (IDKlienti.Trim() == "")
                return false;

            string strKom =
            "SELECT SUM(Borxhi) FROM EvidencaPagesave WHERE IDKlienti='" + IDKlienti + "'";

            SqlCeConnection conn = new SqlCeConnection(frmHome.AppDBConnectionsStr);
            SqlCeCommand kom = new SqlCeCommand(strKom, conn);
            try
            {
                conn.Open();
                string a = kom.ExecuteScalar().ToString();
                if (a != "")
                {
                    float DebtSum = float.Parse(a);
                    conn.Close();
                    if (DebtSum > 0)
                        res = true;
                    else
                        res = false;
                }
                else
                    res = false;
            }
            finally
            {
                conn.Dispose();
                kom.Dispose();
            }


            return res;
        }

        #endregion

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text.Length != 0)
                {
                    listaVizitaveBindingSource.Filter = "KontaktEmriMbiemri LIKE  '%" + textBox1.Text + "%'";
                }
                else
                {
                    listaVizitaveBindingSource.RemoveFilter();
                }
            }
            catch (Exception ex)
            {
                DbUtils.WriteExeptionErrorLog(ex);
            }
        }

        private void menuItemFoto_Click(object sender, EventArgs e)
        {
            CameraCaptureDialog cameraCapture = new CameraCaptureDialog();

            cameraCapture.Owner = null;
            cameraCapture.InitialDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);//@"\My Documents";
            cameraCapture.DefaultFileName = kontaktEmriMbiemriLabel1.Text + " " + DateTime.Now.ToString("dd_MM_yyyy HH_mm") + ".jpg";
            cameraCapture.Title = "Camera Demo";
            cameraCapture.VideoTypes = CameraCaptureVideoTypes.Messaging;
            cameraCapture.StillQuality = CameraCaptureStillQuality.Low;
            cameraCapture.Resolution = new Size(176, 144);
            //cameraCapture.VideoTimeLimit = new TimeSpan(0, 0, 15);  // Limited to 15 seconds of video.
            cameraCapture.Mode = CameraCaptureMode.Still;

            if (DialogResult.OK == cameraCapture.ShowDialog())
            {
                if (FotoBL.InsertImageToSQLCe(cameraCapture.FileName, cameraCapture.DefaultFileName, lblIDVizita.Text))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Application.DoEvents();
                    ConfigXpnParams SyncParams;
                    ConfigXpn.ReadConfiguration(out SyncParams);
                    //Sync Snc = new Sync(SyncParams.LocalDbPath,
                    //                    SyncParams.LocalDbPassword,
                    //                    SyncParams.ServerName,
                    //                    SyncParams.MobileServerUrl,
                    //                    SyncParams.DbName,
                    //                    SyncParams.UserName,
                    //                    SyncParams.Password);

                    //if (
                    //    //Snc.SyncTable("Vizitat_Foto", new string[] { "ID" }, PnSyncDirection.UploadWithInsert, "1=1", "SyncStatus", "")
                    //    Snc.SyncTable("Vizitat_Foto", new string[] { "ID" }, PnSyncDirection.Upload, "1=1", "SyncStatus", "")
                    //    )


                    BL.FotoBL.UploadFotos(SyncParams.WebServiceURL);
                    Cursor.Current = Cursors.Default;
                    Application.DoEvents();

                }
            }
            if (cameraCapture.FileName!="")
            File.Delete(cameraCapture.FileName);


        }

        private void comboBoxTimePeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataGridRefreshed = false;
            FillVizitatDataGrid();
        }

        #region KushtetShtuarMB

        private bool exsistStatus0onGrid()
        {
            DateTime FromDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
            FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day);

            DateTime ToDate = FromDate.AddDays(7);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 59, 998);

            int vizitat0 = 0;
            bool _result = false;
            string _selectQuery = "";
            try
            {
                _selectQuery = @"SELECT count(*)
                                       FROM   Vizitat v 
                                       WHERE  v.IDStatusiVizites = 0 and 
                                        v.DataPlanifikimit between '" + FromDate + "' and '" + ToDate + "'";

                vizitat0 = int.Parse(PnUtils.DbUtils.ExecSqlScalar(_selectQuery));
                if (vizitat0 > 0)
                {
                    _result = false;
                }
                else
                {
                    _result = true;
                }
            }
            catch (Exception ex)
            {
                DbUtils.WriteExeptionErrorLog(ex);

            }
            return _result;
        }

        private bool exsistStatus0onGridwithID(System.Guid VizitaID)
        {
            DateTime FromDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
            FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day);

            DateTime ToDate = FromDate.AddDays(7);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 59, 998);

            int vizitat0 = 0;
            bool _result = false;
            string _selectQuery = "";
            try
            {
                _selectQuery = @"SELECT count(*)
                                       FROM   Vizitat v 
                                       WHERE  v.IDStatusiVizites = 0 and 
                                        v.IDVizita <> '" + VizitaID + "' and v.DataPlanifikimit between '" + FromDate + "' and '" + ToDate + "'";

                vizitat0 = int.Parse(PnUtils.DbUtils.ExecSqlScalar(_selectQuery));
                if (vizitat0 > 0)
                {
                    _result = false;
                }
                else
                {
                    _result = true;
                }
            }
            catch (Exception ex)
            {
                DbUtils.WriteExeptionErrorLog(ex);

            }
            return _result;
        }

        private bool checkOraRealizimit(System.Guid VizitaID)
        {
            DateTime FromDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
            FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day);

            DateTime ToDate = FromDate.AddDays(7);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 59, 998);

            int vizitat0 = 0;
            bool _result = false;
            string _selectQuery = "";
            string _selectQuery1 = "";
            string _selectQuery2 = "";
            try
            {
                _selectQuery1 = @"select MAX(OraRealizimit) from Vizitat where DataPlanifikimit between '" + FromDate + "' and '" + ToDate + "'";
                DateTime OraRealizimitAll = DateTime.Now; 

                _selectQuery = @"select MAX(OraRealizimit) from Vizitat where IDVizita = '" + VizitaID + "'";
                DateTime OraRealizimitVizites = DateTime.Now;

                _selectQuery2 = @"select DataAritjes from Vizitat where IDVizita = '" + VizitaID + "'";
                DateTime DataArritjesVizites = DateTime.Now;

                try
                {
                    OraRealizimitAll = Convert.ToDateTime(PnUtils.DbUtils.ExecSqlScalar(_selectQuery1));
                    OraRealizimitVizites = Convert.ToDateTime(PnUtils.DbUtils.ExecSqlScalar(_selectQuery));
                }
                catch
                {
                    OraRealizimitVizites = OraRealizimitAll;
                }

                try
                {
                    DataArritjesVizites = Convert.ToDateTime(PnUtils.DbUtils.ExecSqlScalar(_selectQuery2));
                }
                catch
                {

                }

                if (OraRealizimitVizites >= OraRealizimitAll)
                {
                    if (DataArritjesVizites.Date == DateTime.Now.Date)
                    {
                        _result = true;
                    }
                    else
                    {
                        _result = false;
                    }
                }
                else
                {
                    _result = false;
                }
            }
            catch (Exception ex)
            {
                DbUtils.WriteExeptionErrorLog(ex);

            }
            return _result;
        }

        private bool exsistStatus0(System.Guid VizitaID)
        {
            int IDStatusiVizites = 0;
            bool _result = false;
            string _selectQuery = "";
            try
            {
                _selectQuery = @"SELECT v.IDStatusiVizites
                                       FROM   Vizitat v 
                                       WHERE  v.IDVizita = '" + VizitaID + "'";

                IDStatusiVizites = int.Parse(PnUtils.DbUtils.ExecSqlScalar(_selectQuery));
                if (IDStatusiVizites == 0)
                {
                    _result = true;
                }
                else
                {
                    _result = false;
                }
            }
            catch (Exception ex)
            {
                DbUtils.WriteExeptionErrorLog(ex);

            }
            return _result;
        }
        #endregion

        private void menuShtijeKorrigjim_Click(object sender, EventArgs e)
        {
            if (isSelected)
            {

                if (exsistStatus0(CurrIDV))
                {

                    if (IDPorosi != NONE)
                    {
                        IDPorosi = NONE;
                        IDLiferimi = NONE;
                    }
                    NrFatures = getNurFatures(frmHome.IDAgjenti);
                    if (NrFatures == "Error" || NrFatures == "Error:Konfigurim")
                    {
                        MessageBox.Show("Nuk është i caktuar numri i kufive të faturave \n shitja nuk mund të vazhdojë");
                        return;
                    }
                    if (!excistTVSH())
                    {
                        MessageBox.Show("Vlera e TVSH-se nuk është e përcaktuar \n Tabela:CompanyInfo");
                        return;
                    }
                    else
                    {

                        ShitjeKorrigjim = true;
                        Cursor.Current = Cursors.WaitCursor;
                        //new Shitja().ShowDialog();//FS. Instancim direkt i objektit, pa dispose(). Ngarkim i panevojshem i memorjes
                        if (frmShitja == null) frmShitja = new frmShitja();
                        frmShitja.ShowDialog();
                        this.vizitatTableAdapter.UpdateViziten(StatVizites, DataCal, DateTime.Now.ToLocalTime(), CurrIDV);


                        ShitjeKorrigjim = false;
                        // Need to refresh DataGrid
                        DataGridRefreshed = false;
                        FillVizitatDataGrid();
                        UpdaterRows();
                        this.listaVizitaveDataGrid.Update();
                        txbKompani.Focus();
                        GC.Collect();
                        Cursor.Current = Cursors.Default;

                    }
                }
                else
                {
                    MessageBox.Show("Vizita nuk eshte e hapur!");
                }
            }


            //Shtuar nga Edin Verbiqi me date 20.12.2016
            isSelected = false;
            //
        }

        //private void newMenuItemMenuItem1_Click(object sender, EventArgs e)
        //{
        //    listaVizitaveBindingSource.AddNew();
        //    MobileSales.ListaVizitaveEditViewDialog listavizitaveEditViewDialog = MobileSales.ListaVizitaveEditViewDialog.Instance(this.listaVizitaveBindingSource);
        //    listavizitaveEditViewDialog.ShowDialog();

        //}
    }
}
