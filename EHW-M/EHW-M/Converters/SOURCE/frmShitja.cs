using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using Microsoft.VisualBasic;
using PnUtils;
using MobileSales.BL;


namespace MobileSales
{

    public partial class frmShitja : Form
    {
        private DateTime DataAktuale = System.DateTime.Now;
        private DateTime KohaAktuale = System.DateTime.Now;
        public static float PriceBoxWithoutRabat, Rabati, CopGratis, CopPako = 1, sasia, CmimiMeTVSH, CmimiPaTVSH, MaxCop;
        public static string prod_name, IDArtikulli, KMON, Shifra, BUM, PayType, Seri;
        public static decimal TotaliFature, TotaliPaguar;
        public static int StatVizits, IDArsyeja, _tempNrFatures;
        public static bool ExcistInstanc = false, ISBarcode = false, ISFromaList = false, isSelected = false;
        public const int POROSIA_DBCOL = 3;
        private double Sasia;
        private bool Key, isnum, ISAnulo; /*ISFromOtherControl tregone nese barkodi shtypet
                                                        * kur nuk jemi te fokusuar txtArtikullit
                                                       */
        private decimal TotPorosia;
        private float value;
        public static bool isClosed;
        private int SelectedRow;
        private int _aprovuar = 0;
        private string _llojDok;
        string _tempIDPorosia = "-1";
        #region Initialize Instance

        frmArtikujtStoqet fArt;
        frmLiferimi frmLiferimi = null;
        dlgMbylljaFatures MbylljaFatures = null;

        #endregion

        public frmShitja()
        {
            InitializeComponent();
        }

        public frmShitja(Guid temp_IDVizita, Guid temp_IDPorosia, string temp_IDKlienti, string temp_ID)
        {
            InitializeComponent();
            frmVizitat.IDPorosi = temp_IDPorosia;
            frmVizitat.CurrIDV = temp_IDVizita;
            frmVizitat.Vizitat_IDKlienti = temp_IDKlienti;
            excistTVSH();
            _tempIDPorosia = temp_ID;
        }

        private void frmShitja_Load(object sender, EventArgs e)
        {
            InitControls();
            InitConnection();

            if (frmVizitat.IDPorosi == frmVizitat.NONE) //kemi hyre pa porosi, duhet krijuar
            {
                Guid GuidPorosi;
                GuidPorosi = Guid.NewGuid();
                frmVizitat.IDPorosi = GuidPorosi;
                this.porositeBindingSource.AllowNew = true;
                this.porositeBindingSource.AddNew();
                this.dtpdataPorosise.Value = DateTime.Now.Date.ToLocalTime();
                this.dtpDateLiferim.Value = DateTime.Now.Date.ToLocalTime();
                DataRow porArt = ((DataRowView)(this.porositeBindingSource.Current)).Row;
                porArt["IDPorosia"] = GuidPorosi;
                porArt["IDVizita"] = frmVizitat.CurrIDV;
                porArt["DeviceID"] = frmHome.DevID;
                this.porositeBindingSource.EndEdit();
                this.porositeTableAdapter.Update(this.myMobileDataSet.Porosite);
            }
            else
            {
                frmVizitat.NrFatures = PnFunctions.getNurFatures(frmHome.IDAgjenti);
            }


            this.porositeTableAdapter.Update(this.myMobileDataSet);
            _tempNrFatures = int.Parse(frmVizitat.NrFatures);
            lblNrPorosis.Text = (_tempNrFatures).ToString();
            lblData.Text = dtpdataPorosise.Value.ToString("dd-MM-yyyy");
            this.porosiaArtTableAdapter.GetPorosiaByIDPorosia(this.myMobileDataSet.PorosiaArt, frmVizitat.IDPorosi);
            this.porositeTableAdapter.Fill(this.myMobileDataSet.Porosite, frmVizitat.CurrIDV);
            this.dtpdataPorosise.Enabled = false;

            if (porosiaArtDataGrid.VisibleRowCount != 0)
            {
                TotPorosia = Convert.ToDecimal((this.porosiaArtTableAdapter.Totali(frmVizitat.IDPorosi)).ToString());
                lblTotali.Text = "Totali: " + String.Format("{0:#,0.00}", TotPorosia);
                //lblTotali.Text = String.Format("{0:#,0.00}", TotPorosia);
            }
            else
            {
                lblTotali.Text = "Totali: 0.00 ";
                txtSeri.Text = "";
            }
            Cursor.Current = Cursors.Default;
            isSelected = false;
        }

        private void btnArtikujt_Click(object sender, EventArgs e)
        {
            porosiaArtDataGrid.Enabled = false;
            if (isSelected)
            {
                try
                {
                    porosiaArtDataGrid.UnSelect(SelectedRow);
                }
                catch (Exception ex)
                {
                    //  MessageBox.Show("Error:"+ex.Message);
                    DbUtils.WriteExeptionErrorLog(ex);
                }

            }
            int numRows = porosiaArtBindingSource.Count;
            if (numRows >= 1)
            {
                string IDART = ((porosiaArtBindingSource.Current as DataRowView)["IDArtikulli"].ToString());
                if (IDART == "")
                {
                    porosiaArtBindingSource.RemoveCurrent();
                }

            }

            isSelected = true;
            porosiaArtBindingSource.EndEdit();
            txtPako.TextChanged -= txtPako_TextChanged;
            porosiaArtBindingSource.AddNew();
            if (frmVizitat.KthimMall)
            {
                txtPako.Text = "-1";
            }
            porosiaArtDataGrid.Select(porosiaArtDataGrid.VisibleRowCount - 1);
            if (fArt == null)
                fArt = new frmArtikujtStoqet();
            fArt.position = "Shitja";
            Cursor.Current = Cursors.WaitCursor;
            fArt.ShitjaCrossForm = this;//to use as cross reference
            fArt.Show();
            fArt.txtArtFilter.Focus();
            //txtPako.Focus();
            Cursor.Current = Cursors.Default;

        }

        private void btnShto_Click(object sender, EventArgs e)
        {
            float Porosia;
            if (txtArtikulli.Text != "")
            {
                if (txtSeri.Text != "")
                {
                    try
                    {
                        Porosia = System.Single.Parse(Math.Round(double.Parse(txtPako.Text), 2).ToString());// Convert.ToDouble(txtPako.Text);
                        if (Porosia == 0)
                        {
                            MessageBox.Show("Gabim në sasi");
                            txtPako.Select(0, txtPako.Text.Length);
                            return;
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        DbUtils.WriteExeptionErrorLog(ex);
                        return;
                    }

                    TryToUpdateSeriShitje();

                    if (!frmVizitat.KthimMall)
                    {
                        //MaxCop = System.Single.Parse((Math.Round(MaxCop, 3)).ToString());

                        string _getMax = "SELECT S.Sasia FROM Stoqet s WHERE s.Depo = '" + frmHome.Depo + "' And S.Shifra='" + IDArtikulli + "' and s.Seri = '" + txtSeri.Text + "'";
                        MaxCop = System.Single.Parse(PnUtils.DbUtils.ExecSqlScalar(_getMax));
                        MaxCop = System.Single.Parse((Math.Round(MaxCop, 3)).ToString());

                        if (Porosia > MaxCop)// nese tejkalohet maksimumi i stokut per artikullin perkates
                        {
                            MessageBox.Show("Tejkalim i stokut \n   " + Porosia + ">" + MaxCop);
                            txtPako.Select(0, txtPako.Text.Length);
                            txtPako.Text = MaxCop.ToString();
                            return;
                        }
                    }
                    else
                    {
                        new frmVerejtjet().ShowDialog();
                        if (IDArsyeja == 0)
                        {
                            MessageBox.Show("Ju lutemi zgjedhni arsyejen \n ose kaloni ne artikull tjeter \n ");
                            return;
                        }
                    }
                    try
                    {
                        CmimiPaTVSH = CmimiMeTVSH - 100 / frmVizitat._TVHS;
                        if (frmVizitat.KthimMall)
                        {
                            TryToUpdateSeriKthime();
                            this.porosiaArtTableAdapter.InsertQueryWithArsyeje(IDArtikulli, Porosia, CmimiMeTVSH, Rabati, frmVizitat.IDPorosi, prod_name, Porosia, CopGratis, CopPako, frmHome.DevID, IDArsyeja, CmimiPaTVSH, BUM, txtSeri.Text);
                        }
                        else
                        {
                            if (frmVizitat.ShitjeKorrigjim)
                            {
                                IDArsyeja = 1;
                                this.porosiaArtTableAdapter.InsertQueryWithArsyeje(IDArtikulli, Porosia, CmimiMeTVSH, Rabati, frmVizitat.IDPorosi, prod_name, Porosia, CopGratis, CopPako, frmHome.DevID, IDArsyeja, CmimiPaTVSH, BUM, txtSeri.Text);
                            }
                            else
                            {
                                this.porosiaArtTableAdapter.InsertItemPA
                                    (IDArtikulli, Porosia, CmimiMeTVSH, Rabati, frmVizitat.IDPorosi, prod_name, Porosia, CopGratis, CopPako, frmHome.DevID, CmimiPaTVSH, BUM, txtSeri.Text);
                            }
                        }
                        this.porosiaArtTableAdapter.Update(this.myMobileDataSet.PorosiaArt);
                        TotPorosia = Convert.ToDecimal((this.porosiaArtTableAdapter.Totali2(frmVizitat.IDPorosi)).ToString());
                        lblTotali.Text = "Totali: " + String.Format("{0:#,0.00}", TotPorosia);
                        //lblTotali.Text = String.Format("{0:0.00 }", TotPorosia);
                        btnShto.Enabled = false;
                        txtSeri.Enabled = false;
                        porosiaArtDataGrid.Enabled = true;
                        porosiaArtDataGrid.Focus();
                        this.porosiaArtTableAdapter.GetPorosiaByIDPorosia(this.myMobileDataSet.PorosiaArt, frmVizitat.IDPorosi);
                        FormatQuantity();
                        isSelected = false;

                    }
                    catch (SqlCeException sce)
                    {
                        if (sce.NativeError == 25016)
                        {
                            MessageBox.Show("Gabim:\nRegjistrim i dyfishtë i artikullit: " + txtArtikulli.Text);
                            porosiaArtBindingSource.RemoveCurrent();
                            porosiaArtBindingSource.EndEdit();
                            porosiaArtDataGrid.Enabled = true;
                            btnShto.Enabled = false;
                            txtSeri.Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show(sce.Message);
                        }
                        DbUtils.WriteSQLCeErrorLog(sce);
                    }
                    catch (Exception ex)
                    {
                        DbUtils.WriteExeptionErrorLog(ex);
                    }
                }
                else
                {
                    MessageBox.Show("Nuk mund të shtohet Artikull i cili ka Serinë të paplotësuar!");
                    txtSeri.Focus();
                }
            }
            else
            {
                MessageBox.Show("Jepni emrin e artikullit sakt!");
            }

        }

        private void menuAnulo_Click(object sender, EventArgs e)
        {
            if (isSelected == true && porosiaArtDataGrid.VisibleRowCount != 0)
            {
                if (MessageBox.Show("Jeni të sigurtë për fshirjen e artikulli: " + txtArtikulli.Text, "Verejtje", MessageBoxButtons.YesNo, MessageBoxIcon.Hand,
                   MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    if (this.porositeBindingSource.AllowRemove == true)
                    {
                        ISAnulo = true;
                        this.porosiaArtBindingSource.RemoveCurrent();
                        this.porosiaArtTableAdapter.Update(this.myMobileDataSet.PorosiaArt);
                        btnShto.Enabled = false;
                        porosiaArtDataGrid.Enabled = true;

                        if (porosiaArtDataGrid.VisibleRowCount != 0)
                        {
                            TotPorosia = Convert.ToDecimal(this.porosiaArtTableAdapter.Totali(frmVizitat.IDPorosi).ToString());
                            lblTotali.Text = "Totali: " + String.Format("{0:#,0.00}", TotPorosia);
                            //lblTotali.Text = String.Format("{0:0.00 }", TotPorosia);
                            txtArtikulli.Focus();
                            isSelected = false;
                        }
                        else
                        {
                            TotPorosia = 0;
                            lblTotali.Text = "Totali: 0.00 ";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Largimi i Artikullit pa sukses");
                    }
                }
                else
                {
                    txtArtikulli.Focus();
                }
            }
            else
            {
                MessageBox.Show("Nuk keni artikull për fshirje", "Gabim", MessageBoxButtons.OK, MessageBoxIcon.Hand,
                                MessageBoxDefaultButton.Button1);
            }
        }

        private void menuRegjistro_Click(object sender, EventArgs e)
        {
            if (frmVizitat.KthimMall)
            {
                if (textBoxKthimi.Text.Trim().Length == 0)
                {
                    MessageBox.Show(@"Duhet të plotësohet fusha ""Nr. Fat. Kthim"" Kjo është fushë obligative për fiskalizim!",
                            "Vërejtje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    textBoxKthimi.Focus();
                    return;
                }
                else
                {                    
                    string _getKlinetiNIPT = @"SELECT top(1) k.NIPT FROM Vizitat v 
                                                        inner join Klientet k on k.IDKlienti = v.IDKlientDheLokacion
                                                        WHERE v.IDVizita = '" + frmVizitat.CurrIDV + "'";
                    string _KlinetiNIPT = DbUtils.ExecSqlScalar(_getKlinetiNIPT).ToString();

                    string _sqlQuery = @"SELECT count(l.NumriFisk) as NrCount
                                         FROM Liferimi l
                                         inner join Klientet k on k.IDKlienti = l.IDKlienti
                                         WHERE convert(nchar,l.NumriFisk) = '" + textBoxKthimi.Text + @"' and l.LLOJDOK = 'SH' and 
                                               k.NIPT = '" + _KlinetiNIPT + "'";
                    int NrFaturave = int.Parse(PnUtils.DbUtils.ExecSqlScalar(_sqlQuery).ToString());

                    if (NrFaturave == 0 && SyncFiskalizimi.CheckCorrectiveInvoice(textBoxKthimi.Text.Trim(), frmHome.Depo, frmHome.TCRCode, frmHome.OperatorCode, frmHome.BusinessUnitCode, _KlinetiNIPT) <= 0)
                    {
                        MessageBox.Show(@"Fusha ""Nr. Fat. Kthim"" nuk është i sakt, ju lutemi rishikoni edhe njëherë!",
                                "Vërejtje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        textBoxKthimi.Focus();
                        return;
                    }
                }
            }
            if (frmVizitat.ShitjeKorrigjim)
            {
                if (textBoxKthimi.Text.Trim().Length == 0)
                {
                    MessageBox.Show(@"Duhet të plotësohet fusha ""Nr. Fat. Kthim"" Kjo është fushë obligative për fiskalizim!",
                            "Vërejtje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    textBoxKthimi.Focus();
                    return;
                }
                else
                {
                    string _getKlinetiNIPT = @"SELECT top(1) k.NIPT FROM Vizitat v 
                                                        inner join Klientet k on k.IDKlienti = v.IDKlientDheLokacion
                                                        WHERE v.IDVizita = '" + frmVizitat.CurrIDV + "'";
                    string _KlinetiNIPT = DbUtils.ExecSqlScalar(_getKlinetiNIPT).ToString();

                    string _sqlQuery = @"SELECT count(l.NumriFisk) as NrCount
                                         FROM Liferimi l
                                         inner join Klientet k on k.IDKlienti = l.IDKlienti
                                         WHERE convert(nchar,l.NumriFisk) = '" + textBoxKthimi.Text + @"' and l.LLOJDOK = 'SH' and 
                                               k.NIPT = '" + _KlinetiNIPT + "'";
                    int NrFaturave = int.Parse(PnUtils.DbUtils.ExecSqlScalar(_sqlQuery).ToString());

                    if (NrFaturave == 0 && SyncFiskalizimi.CheckCorrectiveInvoice(textBoxKthimi.Text.Trim(), frmHome.Depo, frmHome.TCRCode, frmHome.OperatorCode, frmHome.BusinessUnitCode, _KlinetiNIPT) <= 0)
                    {
                        MessageBox.Show(@"Fusha ""Nr. Fat. Kthim"" nuk është i sakt, ju lutemi rishikoni edhe njëherë!",
                                "Vërejtje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        textBoxKthimi.Focus();
                        return;
                    }
                }
            }
            if (btnShto.Enabled)
            {
                porosiaArtBindingSource.RemoveCurrent();
                btnShto.Enabled = false;
                porosiaArtDataGrid.Enabled = true;
            }
            string _selectQuery = @"SELECT max(v.DataAritjes)
                                       FROM   Vizitat v WHERE  v.IDVizita = '" + frmVizitat.CurrIDV + "'";
            string FataArritjes = PnUtils.DbUtils.ExecSqlScalar(_selectQuery).ToString();


            if (FataArritjes != "" || FataArritjes != null)
            {
                if (porosiaArtDataGrid.VisibleRowCount > 0)
                {

                    SqlCeConnection con = null;
                    SqlCeCommand cmd = null;
                    try
                    {
                        con = new SqlCeConnection(frmHome.AppDBConnectionsStr + " ; Default Lock Timeout = 50000");
                        cmd = new SqlCeCommand("UPDATE TEST", con);

                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Transaction = con.BeginTransaction();
                        cmd.CommandType = CommandType.Text;

                        DateTime dp = dtpdataPorosise.Value;
                        DateTime dl = dtpDateLiferim.Value;
                        DateTime KohaPorosise = DateTime.Now.ToLocalTime();
                        cmd.CommandText = @"UPDATE Porosite
                                        SET    DataPerLiferim = @DataPerLiferim,
                                               DataPorosise = @DataPorosise,
                                               StatusiPorosise = @StatusiPorosise,
                                               IDVizita = @IDVizita,
                                               NrPorosise = @NrPorosise,
                                               SyncStatus = 0,
                                               OraPorosise = @OraPorosise,
                                               Longitude = @Longitude,
                                               Latitude = @Latitude
                                        WHERE  (IDPorosia = @IDPorosia)";
                        cmd.Parameters.AddWithValue("@DataPerLiferim", dl);
                        cmd.Parameters.AddWithValue("@DataPorosise", dp);
                        cmd.Parameters.AddWithValue("@StatusiPorosise", 1);
                        cmd.Parameters.AddWithValue("@IDVizita", frmVizitat.CurrIDV.ToString());
                        cmd.Parameters.AddWithValue("@NrPorosise", _tempNrFatures);
                        cmd.Parameters.AddWithValue("@OraPorosise", KohaPorosise.ToString());
                        cmd.Parameters.AddWithValue("@IDPorosia", frmVizitat.IDPorosi.ToString());
                        cmd.Parameters.AddWithValue("@Longitude", frmHome.LongitudeCur);
                        cmd.Parameters.AddWithValue("@Latitude", frmHome.LatitudeCur);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        Cursor.Current = Cursors.Default;
                        this.porosiaArtBindingSource.EndEdit();
                        this.porosiaArtTableAdapter.Update(this.myMobileDataSet.PorosiaArt);
                        TotPorosia = Convert.ToDecimal(this.porosiaArtTableAdapter.Totali(frmVizitat.IDPorosi));
                        TotaliFature = Convert.ToDecimal(String.Format("{0:0.00}", TotPorosia));

                        lblTotali.Text = "Totali: " + String.Format("{0:#,0.00}", TotaliFature);
                        //lblTotali.Text = TotaliFature.ToString();
                        Application.DoEvents();


                        if (MessageBox.Show("Jeni të sigurtë për mbylljen e faturës?", "Vërejtje", MessageBoxButtons.YesNo, MessageBoxIcon.Hand,
                             MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            int NumRows = this.porosiaArtBindingSource.Count;
                            if (!frmVizitat.KthimMall)//Freskimi i stokut behte vetem nese kemi shitje e jo kthime te artikujve,si dhe inkasimi
                            {

                                if (NumRows > 0) // Nese kemi artikuj te porositur per klientin
                                {
                                    for (int i = 0; i < NumRows; i++)
                                    {
                                        if (frmVizitat.ShitjeKorrigjim)
                                        {
                                            _llojDok = "KM";
                                        }
                                        else
                                        {
                                            _llojDok = "SH";
                                        }
                                        decimal SasiaUpdate = Math.Round(decimal.Parse((this.porosiaArtDataGrid[i, 1].ToString())), 3);
                                        Shifra = this.porosiaArtDataGrid[i, 4].ToString();
                                        string GridSeri = this.porosiaArtDataGrid[i, 5].ToString();

                                        cmd.CommandText = @"UPDATE Stoqet
                                                        SET    Sasia = Round(Sasia - @Sasia,3),
                                                               SyncStatus = 0
                                                        WHERE  Shifra = @Shifra and Depo = @Depo and Seri = @Seri";
                                        cmd.Parameters.AddWithValue("@Sasia", SasiaUpdate);
                                        cmd.Parameters.AddWithValue("@Shifra", Shifra);
                                        cmd.Parameters.AddWithValue("@Depo", frmHome.Depo);
                                        cmd.Parameters.AddWithValue("@Seri", GridSeri);
                                        cmd.ExecuteNonQuery();
                                        cmd.Parameters.Clear();

                                        cmd.CommandText = @"UPDATE Malli_Mbetur
                                                             SET    SasiaShitur = ROUND(SasiaShitur + @Sasia, 3), SyncStatus=0
                                                             WHERE  IDArtikulli = @IDArtikulli and Seri = @Seri";
                                        cmd.Parameters.AddWithValue("@Sasia", SasiaUpdate);
                                        cmd.Parameters.AddWithValue("@IDArtikulli", Shifra);
                                        cmd.Parameters.AddWithValue("@Seri", GridSeri);
                                        cmd.ExecuteNonQuery();
                                        cmd.Parameters.Clear();

                                        cmd.CommandText = @"Update Malli_Mbetur 
                                                               Set SasiaMbetur=Round(SasiaPranuar-(SasiaShitur+SasiaKthyer-LevizjeStoku),3), SyncStatus=0
                                                                   WHERE  IDArtikulli = @IDArtikulli and Seri = @Seri";
                                        cmd.Parameters.AddWithValue("@IDArtikulli", Shifra);
                                        cmd.Parameters.AddWithValue("@Seri", GridSeri);
                                        cmd.ExecuteNonQuery();
                                        cmd.Parameters.Clear();
                                    }
                                }
                            }
                            else
                            {
                                _llojDok = "KD";
                                TotaliPaguar = 0;
                                if (NumRows > 0) // Nese kemi artikuj te kthyer per klientin
                                {
                                    for (int i = 0; i < NumRows; i++)
                                    {
                                        decimal _SasiaKthyer = Math.Round(decimal.Parse((this.porosiaArtDataGrid[i, 1].ToString())), 3);
                                        string _IDArtikulli = this.porosiaArtDataGrid[i, 4].ToString();
                                        string _Seri = this.porosiaArtDataGrid[i, 5].ToString();

                                        cmd.CommandText = @"UPDATE Stoqet
                                                                       SET    Sasia = Round(Sasia - @Sasia,3)
                                                                       WHERE  (Shifra = @IDArtikuli)
                                                                       AND (Depo = @Depo)
                                                                       AND (Seri = @Seri)";
                                        cmd.Parameters.AddWithValue("@Sasia", _SasiaKthyer);
                                        cmd.Parameters.AddWithValue("@IDArtikuli", _IDArtikulli);
                                        cmd.Parameters.AddWithValue("@Depo", frmHome.Depo);
                                        cmd.Parameters.AddWithValue("@Seri", _Seri);
                                        cmd.ExecuteNonQuery();
                                        cmd.Parameters.Clear();

                                        cmd.CommandText = @"UPDATE Malli_Mbetur
                                                                SET    SasiaKthyer = Round(SasiaKthyer + @Sasia_Kthyer,3), SyncStatus=0
                                                                WHERE  (IDArtikulli = @IDArtikulli)
                                                                       AND (Seri = @Seri)";
                                        cmd.Parameters.AddWithValue("@Sasia_Kthyer", _SasiaKthyer);
                                        cmd.Parameters.AddWithValue("@IDArtikulli", _IDArtikulli);
                                        cmd.Parameters.AddWithValue("@Seri", _Seri);
                                        cmd.ExecuteNonQuery();
                                        cmd.Parameters.Clear();

                                        cmd.CommandText = @"Update Malli_Mbetur 
                                                               Set SasiaMbetur=Round(SasiaPranuar-(SasiaShitur+SasiaKthyer-LevizjeStoku),3), SyncStatus=0
                                                                   WHERE  (IDArtikulli = @IDArtikulli)
                                                                       AND (Seri = @Seri)";
                                        cmd.Parameters.AddWithValue("@IDArtikulli", _IDArtikulli);
                                        cmd.Parameters.AddWithValue("@Seri", _Seri);
                                        cmd.ExecuteNonQuery();
                                        cmd.Parameters.Clear();

                                    }
                                }
                            }

                            string _getNumriFisk = "SELECT top(1) v.IDN FROM NumriFisk v WHERE v.Depo = '" + frmHome.IDAgjenti + "'";
                            int _NumriFisk = int.Parse(DbUtils.ExecSqlScalar(_getNumriFisk).ToString());
                            _NumriFisk = _NumriFisk + 1;

                            string _getVitiNumriFisk = "SELECT top(1) v.Viti FROM NumriFisk v WHERE v.Depo = '" + frmHome.IDAgjenti + "'";
                            int _VitiNumriFisk = int.Parse(DbUtils.ExecSqlScalar(_getVitiNumriFisk).ToString());

                            Cursor.Current = Cursors.Default;
                            if (MbylljaFatures == null) MbylljaFatures = new dlgMbylljaFatures();
                            //if (frmVizitat.KthimMall)
                            //{
                            //    MbylljaFatures.cmbMenyraPageses.Enabled = false;
                            //}
                            if (MbylljaFatures.ShowDialog(frmVizitat.Vizitat_IDKlienti, Math.Round(TotaliFature, 2), 0) == DialogResult.OK)
                            {
                                TotaliPaguar = decimal.Parse(MbylljaFatures.txtShumaPaguar.Text);
                                PayType = MbylljaFatures.cmbMenyraPageses.Text;
                                InsertEvidencaPagesave
                                       (
                                        _NumriFisk.ToString() + "/" + _VitiNumriFisk.ToString(),//_tempNrFatures.ToString(),
                                        DateTime.Now,
                                        MbylljaFatures.dtDataPerPagese.Value,
                                        TotaliFature,
                                        TotaliPaguar,
                                        dlgMbylljaFatures.KMON,
                                        PayType, cmd
                                        );

                            }
                            Cursor.Current = Cursors.WaitCursor;

                            fArt = null;
                            cmd.CommandText = @"UPDATE Vizitat
                                                SET    IDStatusiVizites = 6,
                                                       DataRealizimit = @Data_Realizimit,
                                                       OraRealizimit = @Ora_Realizimit, 
                                                       SyncStatus = 0
                                                WHERE  IDVizita = @IDVizita";
                            cmd.Parameters.AddWithValue("Data_Realizimit", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmd.Parameters.AddWithValue("Ora_Realizimit", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmd.Parameters.AddWithValue("IDVizita", frmVizitat.CurrIDV);
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();


                            cmd.CommandText = @"Select Count(*) from PorosiaArt where IDPorosia='" + frmVizitat.IDPorosi + "'";
                            int _nrDetaleve = int.Parse(cmd.ExecuteScalar().ToString());


                            cmd.CommandText = @"UPDATE Porosite 
                                               SET    NrPorosise = @Nr_Porosise,
                                                      NrDetalet = @Nr_Detaleve,
                                                      Longitude = @Longitude,
                                                      Latitude = @Latitude 
                                               WHERE  IDPorosia = @IDPorosia";
                            cmd.Parameters.AddWithValue("Nr_Porosise", _tempNrFatures.ToString());
                            cmd.Parameters.AddWithValue("@Nr_Detaleve", _nrDetaleve);
                            cmd.Parameters.AddWithValue("@IDPorosia", frmVizitat.IDPorosi.ToString());
                            cmd.Parameters.AddWithValue("@Longitude", frmHome.LongitudeCur);
                            cmd.Parameters.AddWithValue("@Latitude", frmHome.LatitudeCur);
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();

                            cmd.CommandText = @"UPDATE PorosiaArt
                                                SET    DeviceID = '" + frmHome.DevID + "'," +
                                                       "IDArsyeja = " + 1 + "" +
                                                "WHERE  SasiaPorositur < 0 AND IDArsyeja IS NULL";
                            cmd.ExecuteNonQuery();


                            if (frmHome.AprovimFaturash)
                            {
                                cmd.CommandText = @"UPDATE NumriFaturave
                                                   SET    CurrNrFat = CurrNrFat + 1
                                                     WHERE  KOD = @IDAgjenti";
                                cmd.Parameters.AddWithValue("@IDAgjenti", frmHome.IDAgjenti);
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                            }
                            else
                            {
                                cmd.CommandText = @"UPDATE NumriFaturave
                                                   SET    CurrNrFatJT = CurrNrFatJT + 1
                                                  WHERE  KOD = @IDAgjenti";

                                cmd.Parameters.AddWithValue("@IDAgjenti", frmHome.IDAgjenti);
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();

                            }

                            /*Krojojme Hederin e Liferimi*/

                            string _getIDKlientDheLokacion = "SELECT top(1) v.IDKlientDheLokacion FROM Vizitat v WHERE v.IDVizita = '" + frmVizitat.CurrIDV + "'";
                            string _IDKlientDheLokacion = DbUtils.ExecSqlScalar(_getIDKlientDheLokacion).ToString();



                            Guid IDLiferimi; //Generate new Guid();
                            IDLiferimi = Guid.NewGuid();
                            frmVizitat.IDLiferimi = IDLiferimi;
                            float TotaliFautresMeTVSH = System.Convert.ToSingle(frmShitja.TotaliFature);
                            float TotaliFaturesPaTVSH = TotaliFautresMeTVSH - 100 / frmVizitat._TVHS;
                            cmd.CommandText = @"INSERT INTO Liferimi
                                                            (
	                                                            IDLiferimi,
	                                                            DataLiferuar,
	                                                            KohaLiferuar,
	                                                            TitulliLiferimit,
	                                                            DataLiferimit,
	                                                            KohaLiferimit,
	                                                            IDPorosia,
	                                                            Liferuar,
	                                                            NrLiferimit,
	                                                            CmimiTotal,
	                                                            DeviceID,
	                                                            SyncStatus,
	                                                            ShumaPaguar,
	                                                            Aprovuar,
	                                                            LLOJDOK,
	                                                            PayType,
	                                                            TotaliPaTVSH,
                                                                NrDetalet,
                                                                IDKlienti,
                                                                Depo,
                                                                Longitude ,
                                                                Latitude,
                                                                IDKthimi,
                                                                NumriFisk,
                                                                TCR,
                                                                TCROperatorCode,
                                                                TCRBusinessCode
                                                            )
                                                            VALUES
                                                            (
	                                                            '" + IDLiferimi.ToString() + "','" + "','" +
                                                                DateTime.Now.Date + "','" +
                                                                DateTime.Now.ToLocalTime() + "'," +
                                                                ' ' + "'" +
                                                                DateTime.Now.Date + "','" +
                                                                DateTime.Now.ToLocalTime() + "','" +
                                                                frmVizitat.IDPorosi.ToString() + "'," +
                                                                "1," +
                                                                _tempNrFatures.ToString() + "," +
                                                                TotaliFautresMeTVSH + ",'" +
                                                                frmHome.DevID + "'," +
                                                                "0," +
                                                                TotaliPaguar + "," +
                                                                _aprovuar + ",'" +
                                                                _llojDok + "','" +
                                                                PayType + "'," +
                                                                TotaliFaturesPaTVSH + "," +
                                                                _nrDetaleve + ",'" +
                                                                _IDKlientDheLokacion + "','" +
                                                                frmHome.Depo + "','" +
                                                                frmHome.LongitudeCur + "','" +
                                                                frmHome.LatitudeCur + "'," +
                                                                (!string.IsNullOrEmpty(textBoxKthimi.Text)
                                                                        ? "'" + textBoxKthimi.Text.Replace(" ", "") + "'"
                                                                        : "null") + ","
                                                                + _NumriFisk.ToString() + ",'" +
                                                                frmHome.TCRCode + "','" +
                                                                frmHome.OperatorCode + "','" +
                                                                frmHome.BusinessUnitCode + "'" +
                                                                ")";


                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();


                            /*Copy PorosiaArt to LiferimiArt*/
                            cmd.CommandText = @"INSERT INTO LiferimiArt  
                                                      (  
                                                        IDLiferimi,  
                                                        IDArtikulli,  
                                                        Cmimi,  
                                                        SasiaLiferuar,  
                                                        SasiaPorositur,  
                                                        ArtEmri,  
                                                        Totali, 
                                                        DeviceID,  
                                                        Gratis, 
                                                        SyncStatus, 
                                                        IDArsyeja,  
                                                        CmimiPaTVSH,  
                                                        TotaliPaTVSH,  
                                                        VlefteTVSH,
                                                        Seri  
                                                      )  
                                                    SELECT '" + IDLiferimi.ToString() + "'," + @" 
                                                           pa.IDArtikulli,  
                                                           pa.CmimiAktual,  
                                                           Round(pa.SasiaPorositur,3),  
                                                           Round(pa.SasiaPorositur,3),  
                                                           pa.Emri,  
                                                           Round(pa.SasiaPorositur * pa.CmimiAktual,2),  
                                                           '" + frmHome.DevID + "'," + @" 
                                                           pa.Gratis,  
                                                           0,  
                                                           pa.IDArsyeja,  
                                                           Round(pa.CmimiAktual / (1+" + frmVizitat._TVHS / 100 + "),2)," +
                                                           "Round((pa.SasiaPorositur * pa.CmimiAktual) / (1+" + frmVizitat._TVHS / 100 + "),2)," +
                                                           "Round(pa.SasiaPorositur * pa.CmimiAktual,2)- Round(((pa.SasiaPorositur * pa.CmimiAktual) / (1+" + frmVizitat._TVHS / 100 + ")),2)," +
                                                           "pa.Seri " +
                                                    "FROM   PorosiaArt pa  " +
                                                    "WHERE  pa.IDPorosia = '" + frmVizitat.IDPorosi.ToString() + "'";
                            //testimi i fiskalizimit mb22062021
//                            cmd.CommandText = @"INSERT INTO LiferimiArt  
//                                                      (  
//                                                        IDLiferimi,  
//                                                        IDArtikulli,  
//                                                        Cmimi,  
//                                                        SasiaLiferuar,  
//                                                        SasiaPorositur,  
//                                                        ArtEmri,  
//                                                        Totali, 
//                                                        DeviceID,  
//                                                        Gratis, 
//                                                        SyncStatus, 
//                                                        IDArsyeja,  
//                                                        CmimiPaTVSH,  
//                                                        TotaliPaTVSH,  
//                                                        VlefteTVSH,
//                                                        Seri  
//                                                      )  
//                                                    SELECT '" + IDLiferimi.ToString() + "'," + @" 
//                                                           pa.IDArtikulli,  
//                                                           pa.CmimiAktual,  
//                                                           Round(pa.SasiaPorositur,3),  
//                                                           Round(pa.SasiaPorositur,3),  
//                                                           pa.Emri,  
//                                                           Round(pa.SasiaPorositur * pa.CmimiAktual,2),  
//                                                           '" + frmHome.DevID + "'," + @" 
//                                                           pa.Gratis,  
//                                                           0,  
//                                                           pa.IDArsyeja,  
//                                                           case when pa.IDArtikulli = 'A3053' then Round(pa.CmimiAktual,2)
//                                                                when pa.IDArtikulli = 'A3230' then Round(pa.CmimiAktual / (1.06),2)
//                                                                when pa.IDArtikulli = 'P111' then Round(pa.CmimiAktual / (1.10),2)
//                                                                else Round(pa.CmimiAktual / (1+" + frmVizitat._TVHS / 100 + @"),2) end," +
//                                                           @"case when pa.IDArtikulli = 'A3053' then Round((pa.SasiaPorositur * pa.CmimiAktual),2)
//                                                                when pa.IDArtikulli = 'A3230' then Round((pa.SasiaPorositur * pa.CmimiAktual) / (1.06),2)
//                                                                when pa.IDArtikulli = 'P111' then Round((pa.SasiaPorositur * pa.CmimiAktual) / (1.10),2)
//                                                                else Round((pa.SasiaPorositur * pa.CmimiAktual) / (1+" + frmVizitat._TVHS / 100 + "),2) end," +
//                                                           @"case when pa.IDArtikulli = 'A3053' then Round(pa.SasiaPorositur * pa.CmimiAktual,2)- Round((pa.SasiaPorositur * pa.CmimiAktual),2)
//                                                                when pa.IDArtikulli = 'A3230' then Round(pa.SasiaPorositur * pa.CmimiAktual,2)- Round((pa.SasiaPorositur * pa.CmimiAktual) / (1.06),2)
//                                                                when pa.IDArtikulli = 'P111' then Round(pa.SasiaPorositur * pa.CmimiAktual,2)- Round((pa.SasiaPorositur * pa.CmimiAktual) / (1.10),2)
//                                                                else Round(pa.SasiaPorositur * pa.CmimiAktual,2)- Round((pa.SasiaPorositur * pa.CmimiAktual) / (1+" + frmVizitat._TVHS / 100 + "),2) end," +
//                                                           "pa.Seri " +
//                                                    "FROM   PorosiaArt pa  " +
//                                                    "WHERE  pa.IDPorosia = '" + frmVizitat.IDPorosi.ToString() + "'";
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();

                            //// Testimi i fiskalizimit mb /////////


                            // RegisterInvoice

                            // 
                            if (_tempIDPorosia != "-1")
                            {
                                cmd.CommandText = "Delete from Orders where IDOrder='" + _tempIDPorosia + "'";
                                cmd.ExecuteNonQuery();

                                cmd.CommandText = "Delete from Order_Details where IDOrder='" + _tempIDPorosia + "'";
                                cmd.ExecuteNonQuery();
                            }

                            cmd.Transaction.Commit();

                            cmd.CommandText = @"UPDATE NumriFisk
                                                                 SET IDN = " + (_NumriFisk) + " where Depo = '" + frmHome.IDAgjenti + "'";
                            cmd.ExecuteNonQuery();

                            //@BE FISKALIZIMI PER SHITJET DHE KTHIMET
                            FiskalizimiBL.RegisterTCRInvoice(IDLiferimi.ToString());

                            Cursor.Current = Cursors.Default;
                            if (frmLiferimi == null) frmLiferimi = new frmLiferimi();
                            frmLiferimi.ShowDialog();
                            this.Close();
                        }
                        else
                        {
                            txtPako.Focus();
                        }

                    }
                    catch (SqlCeException ex)
                    {
                        cmd.Transaction.Rollback();
                        DeletePorosi();
                        PnUtils.DbUtils.WriteSQLCeErrorLog(ex, cmd.CommandText);
                        MessageBox.Show(" Realizimi i shitjes dështoi\n Shitja duhet të përsëritet");
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        cmd.Transaction.Rollback();
                        DeletePorosi();
                        PnUtils.DbUtils.WriteExeptionErrorLog(ex);
                        MessageBox.Show(" Realizimi i shitjes dështoi\n Shitja duhet të përsëritet");
                        this.Close();
                    }
                    finally
                    {
                        con.Dispose();
                        cmd.Dispose();
                        //BL.SyncBL.SyncTablesWithGPS();
                        Cursor.Current = Cursors.Default;
                    }
                }
                else
                {
                    MessageBox.Show("Nuk lejohet regjistrimi i faturës boshe");
                    isSelected = false;
                    btnShto.Enabled = false;
                    porosiaArtDataGrid.Enabled = false;
                    return;
                }
            }
            else
            {
                MessageBox.Show("Regjistrimi i fatures deshtoi, ju lutem provoni prap!");
                isSelected = false;
                btnShto.Enabled = false;
                porosiaArtDataGrid.Enabled = false;
                return;
            }
        }

        private void menuDalja_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Jeni të sigurtë për anulimi i porosisë: ", "Verejtje", MessageBoxButtons.YesNo, MessageBoxIcon.Hand,
                  MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DeletePorosi();
                isClosed = true;//Ketu ruajme faktin se porosia e filluar eshte anuluar
                // Qe do te sherbeje per te mos bere Update per viziten aktuale
                fArt = null;
                this.DialogResult = DialogResult.Cancel;
                //this.Close();
            }
            else
            {
                return;
            }


        }

        public void txtPako_TextChanged(object sender, EventArgs e)
        {
            if (txtPako.Text.Trim() != "" && txtArtikulli.Text != "")	//no empty string
            {
                CheckUnit();
                if (frmVizitat.KthimMall)
                {
                    try
                    {

                        if (txtPako.Text.Substring(0, 1) == "-" && txtPako.Text.Length > 1)
                        {
                            isnum = Information.IsNumeric(txtPako.Text.Substring(1, txtPako.Text.Length - 1));
                        }
                        else
                        {
                            if (txtPako.Text.Substring(0, 1) == "-" && txtPako.Text.Length == 1)
                            {
                                isnum = true;
                            }
                            else
                            {
                                isnum = false;
                            }
                        }
                    }
                    catch (ArgumentException)
                    {

                        isnum = false;
                    }
                }
                else
                {
                    if (isnum = Information.IsNumeric(txtPako.Text))
                    {
                        if (btnShto.Enabled == false)//We are in EDIT mode
                        {
                            if (checkPiece(porosiaArtDataGrid.CurrentCell.RowNumber))
                            {
                                MessageBox.Show("Tejkalimi i stokut");
                                txtPako.Select(0, txtPako.Text.Length);
                                txtPako.Text = MaxCop.ToString();
                                return;
                            }
                        }
                    }
                }

                if (!isnum)		//not num
                {
                    MessageBox.Show("Gabim në sasi 0!");
                    if (frmVizitat.KthimMall)
                    {
                        if (txtPako.Text.Length > 1)
                        {
                            string temp = txtPako.Text.Trim();
                            if (txtPako.Text.Trim().Length > 1)
                            {
                                txtPako.Text = temp.Substring(0, temp.Length - 1);
                            }
                            else
                            {
                                txtPako.Text = "";
                            }
                        }
                        else
                        {
                            txtPako.Text = "-1";
                        }
                    }
                    else
                    {
                        if (txtPako.Text.Length > 1)
                        {
                            string temp = txtPako.Text.Trim();
                            if (txtPako.Text.Trim().Length > 1)
                            { txtPako.Text = temp.Substring(0, temp.Length - 1); }
                            else
                            {
                                txtPako.Text = "";
                            }
                        }
                        else
                        {
                            txtPako.Text = "1";
                        }
                    }

                    txtPako.Focus();
                    return;
                }
                else	//is num
                {
                    if (!frmVizitat.KthimMall)
                    {
                        Sasia = Convert.ToDouble(txtPako.Text);
                    }

                    if (frmVizitat.KthimMall)
                    {
                        if ((txtPako.Text.Substring(0, 1) == "-" && txtPako.Text.Length == 1))
                        {
                            Sasia = 0;
                        }
                        else
                        {
                            Sasia = Convert.ToDouble(txtPako.Text.ToString());
                            if (Sasia > 0)
                            {
                                MessageBox.Show("Gabim në sasi 1!");
                                string temp = txtPako.Text.Trim();
                                if (txtPako.Text.Trim().Length > 1)
                                { temp.Substring(0, temp.Length - 1); }
                                else
                                {
                                    txtPako.Text = "";
                                }
                                txtPako.Focus();
                                return;
                            }
                            else
                            {
                                txtCop.Text = sasia.ToString();
                            }
                        }
                    }
                }

            }


        }

        private void txtPako_GotFocus(object sender, EventArgs e)
        {
            txtPako.BackColor = Color.LightCyan;

        }

        private void txtPako_LostFocus(object sender, EventArgs e)
        {
            txtPako.BackColor = Color.White;
        }

        private void txtPako_KeyDown(object sender, KeyEventArgs e)
        {
            bool isnum = Information.IsNumeric(txtPako.Text);
            if (txtPako.Text != "")
            {
                if (frmVizitat.KthimMall)
                {
                    if (txtPako.Text.Substring(0, 1) == "-" && txtPako.Text.Length > 1)
                    {
                        isnum = Information.IsNumeric(txtPako.Text.Substring(1, txtPako.Text.Length - 1));
                    }
                    else
                    {
                        if (txtPako.Text.Substring(0, 1) == "-" && txtPako.Text.Length == 1)
                        {
                            isnum = true;
                        }
                        else
                        {
                            isnum = false;
                        }
                    }
                }

                if (isnum == false)		//not num
                {
                    MessageBox.Show("Gabim në sasi 2!");
                    string temp = txtPako.Text.Trim();
                    if (txtPako.Text.Trim().Length > 1)
                    { txtPako.Text = temp.Substring(0, temp.Length - 1); }
                    else
                    {
                        txtPako.Text = "";
                    }
                    txtPako.Focus();
                    return;
                }
                else
                {
                    if (frmVizitat.KthimMall && txtPako.Text.Length == 1)
                    {
                        //value = 0;
                    }
                    else
                    {
                        value = float.Parse(txtPako.Text);
                    }
                }

            }   // Nese shtypet tasti Enter per Insert

            if (btnShto.Enabled && e.KeyCode == Keys.Enter)
            {
                this.btnShto_Click(null, null);

            }

            #region Keys.UP && Keys.Down

            if (e.KeyCode == Keys.Up)
            {
                if (frmVizitat.KthimMall)
                {
                    value = value - 1;
                    txtPako.Text = value.ToString();
                }
                else
                {
                    value = value + 1;
                    txtPako.Text = value.ToString();
                    if (btnShto.Enabled == false)//We are in EDIT mode
                    {
                        if (checkPiece(porosiaArtDataGrid.CurrentCell.RowNumber))
                        {
                            MessageBox.Show("Tejkalimi i stokut");
                            txtPako.Select(0, txtPako.Text.Length);
                            txtPako.Text = MaxCop.ToString();
                            return;

                        }

                    }
                }

            }

            if (e.KeyCode == Keys.Down)
            {
                if (value > 1)
                {
                    value = value - 1;

                }
                if (value < -1)
                {
                    if (frmVizitat.KthimMall)
                    {
                        value = value + 1;
                    }
                }
                txtPako.Text = value.ToString();
            }


            #endregion
        }

        private void Shitja_KeyDown(object sender, KeyEventArgs e)
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
            if ((e.KeyCode == System.Windows.Forms.Keys.ControlKey))
            {
                btnArtikujt_Click(null, null);
            }
            if (e.KeyCode == Keys.Space)
            {
                txtPako.Text = txtPako.Text.Trim();

            }



        }

        #region DataGrid && BindingSource
        private void porosiaArtBindingSource_PositionChanged(object sender, EventArgs e)
        {
            FormatQuantity();

        }
        private void porosiaArtDataGrid_MouseUp(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.DataGrid.HitTestInfo myHitTest = this.porosiaArtDataGrid.HitTest(e.X, e.Y);
            if (myHitTest.Type == DataGrid.HitTestType.Cell)
            {
                this.porosiaArtDataGrid.Select(myHitTest.Row);
                lblPako.Visible = false;
                lblCPako.Visible = true;
                isSelected = true;
                DataRow rw = ((DataRowView)(this.porosiaArtBindingSource.Current)).Row;
                BUM = rw["BUM"].ToString();
                SelectedRow = myHitTest.Row;
                txtSeri.Text = rw["Seri"].ToString();

            }
        }
        private void porosiaArtDataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            Key = (e.KeyData == Keys.Up || e.KeyData == Keys.Down || e.KeyData == Keys.Left || e.KeyData == Keys.Right);
            if (isSelected == true && Key)
            {
                this.porosiaArtDataGrid.Select(this.porosiaArtDataGrid.CurrentRowIndex);
                SelectedRow = this.porosiaArtDataGrid.CurrentRowIndex;
            }
        }
        private void porosiaArtBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (ISAnulo == false && porosiaArtDataGrid.VisibleRowCount != 0)
            {
                this.porosiaArtTableAdapter.Update(this.myMobileDataSet.PorosiaArt);
                TotPorosia = Convert.ToDecimal(this.porosiaArtTableAdapter.Totali(frmVizitat.IDPorosi).ToString());
                lblTotali.Text = "Totali: " + String.Format("{0:#,0.00}", TotPorosia);
            }
            this.porosiaArtBindingSource.Sort = "NrRendor DESC";

        }
        private void porosiaArtBindingSource_CurrentItemChanged(object sender, EventArgs e)
        {
            ISFromaList = false;
        }
        # endregion

        #region Methods && Functions

        private void InitConnection()
        {
            numerimInternTableAdapter.Connection.ConnectionString = frmHome.AppDBConnectionsStr;
            porosiaArtTableAdapter.Connection.ConnectionString = frmHome.AppDBConnectionsStr;
            porositeTableAdapter.Connection.ConnectionString = frmHome.AppDBConnectionsStr;
            stoqetTableAdapter1.Connection.ConnectionString = frmHome.AppDBConnectionsStr;
        }


        private void InitControls()
        {
            if (frmHome.AprovimFaturash)
            {
                _aprovuar = 1;
                this.BackColor = Color.WhiteSmoke;
                lblData.BackColor = Color.Gainsboro;
                lblNrPorosis.BackColor = Color.Gainsboro;
                lblTitle.BackColor = Color.Gainsboro;
            }
            else
            {
                this.BackColor = Color.Silver;
            }

            if (frmVizitat.KthimMall)
            {
                lblTitle.Text = "Kthimi";
                textBoxKthimi.Text = "";
                txtSeri.Enabled = true;
                textBoxKthimi.Visible = true;
                lblFaturaKthim.Visible = true;
                lblFaturaKthim.Text = "Nr. Fat. Kthim";
            }
            else if (frmVizitat.ShitjeKorrigjim)
            {
                lblTitle.Text = "Shitja";
                textBoxKthimi.Text = "";
                txtSeri.Enabled = true;
                textBoxKthimi.Visible = true;
                lblFaturaKthim.Visible = true;
                lblFaturaKthim.Text = "Nr. Fat. Reference";
                if (Seri == null || Seri == "")
                {
                    txtSeri.Enabled = true;
                }
                else
                {
                    txtSeri.Enabled = false;
                }
            }
            else
            {
                lblTitle.Text = "Shitja";

                textBoxKthimi.Visible = false;
                textBoxKthimi.Text = "";
                lblFaturaKthim.Visible = false;
                if (Seri == null || Seri == "")
                {
                    txtSeri.Enabled = true;
                }
                else
                {
                    txtSeri.Enabled = false;
                }
            }
        }
        private bool isbumKG(string bum)
        {
            bool result = false;
            if (bum.Trim().ToUpper() == "KG")
            {
                result = true;
            }
            return result;
        }

        private void DeletePorosi()
        {
            bool _delPorosia = false;
            bool _delPorosiaArt = false;
            string LastID = DbUtils.ExecSqlScalar("SELECT TOP (1) IDPorosia FROM Porosite p ORDER BY p.OraPorosise ASC");
            string del_Porosia = "Delete from Porosite where IDPorosia='" + LastID + "'";
            _delPorosia = DbUtils.ExecSql(del_Porosia);

            string del_PorosiaArt = "Delete from PorosiaArt where IDPorosia='" + LastID + "'";
            _delPorosiaArt = DbUtils.ExecSql(del_PorosiaArt);
            if (_delPorosia)
            {
                if (_delPorosiaArt)
                {
                    MessageBox.Show("Anulimi i faturës përfundoi me sukses");
                }
            }
            else
            {
                MessageBox.Show("Anulimi dështoi!");
                return;
            }
        }

        private bool IsCountUnit()
        {
            bool a = false;

            if ((!btnShto.Enabled) && (porosiaArtBindingSource.Current != null))
            {
                a = (porosiaArtBindingSource.Current as DataRowView)["BUM"].ToString() != "KG";
            }
            else
            {
                a = (BUM != "KG");
            }
            return a;

        }

        private void CheckUnit()
        {
            if (IsCountUnit())
            {
                txtPako.Text = txtPako.Text.Replace(".", "");
                if (txtPako.Text.Contains('.'))
                {
                    int _indexOfPoint = txtPako.Text.IndexOf('.');
                    int _numAfterPoint = txtPako.Text.Substring(_indexOfPoint, txtPako.Text.Length).Length;
                    if (_numAfterPoint > 3)
                    {

                    }
                }

            }
        }

        private void FormatQuantity()
        {
            if (!IsCountUnit())
            {
                txtPako.Text = Math.Round(Decimal.Parse(txtPako.Text), 3).ToString();
            }
        }

        private bool checkPiece(int _numrRows)
        {
            bool result = false;
            try
            {
                string _IDArtikulli = this.porosiaArtDataGrid[_numrRows, 4].ToString();//4=Kolona:IDArtikulli
                string _Seri = this.porosiaArtDataGrid[_numrRows, 5].ToString();
                float _tempPorosia = System.Single.Parse(Math.Round(double.Parse(txtPako.Text), 2).ToString());
                string _getMax = "SELECT S.Sasia FROM Stoqet s WHERE s.Depo = '" + frmHome.Depo + "' And S.Shifra='" + _IDArtikulli + "' and s.Seri = '" + _Seri + "'";
                MaxCop = System.Single.Parse(PnUtils.DbUtils.ExecSqlScalar(_getMax));
                MaxCop = System.Single.Parse((Math.Round(MaxCop, 3)).ToString());
                if (_tempPorosia > MaxCop)// nese tejkalohet maksimumi i stokut per artikullin perkates
                {
                    result = true;

                }
            }
            catch (Exception ex)
            {
                DbUtils.WriteExeptionErrorLog(ex);
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

                frmVizitat._TVHS = int.Parse(PnUtils.DbUtils.ExecSqlScalar(_selectQuery));
                if (frmVizitat._TVHS == 0)
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

        private void InsertEvidencaPagesave(string NrFatures, DateTime DataPageses, DateTime DataPerPages, decimal ShumaFatures, decimal ShumaPaguar, string KMON, string PayType, SqlCeCommand con)
        {

            DateTime n = DateTime.Now;
            string hr = n.Hour.ToString();
            string min = n.Minute.ToString();
            string sec = n.Second.ToString();

            //ie. if Hour is 4 then 04; if minute is 3 then 03             
            if (hr.Length == 1)
                hr = "0" + hr;
            if (min.Length == 1)
                min = "0" + min;
            if (sec.Length == 1)
                sec = "0" + sec;

            string PagesatID = frmHome.DevID + "-|-" + n.Year.ToString() + n.Month.ToString() + n.Day.ToString() +
                hr + min + sec;


            string strInsertCommand = "";
            try
            {
                strInsertCommand = "INSERT INTO EvidencaPagesave(NrPageses,IDKlienti,IDAgjenti,NrFatures,DataPageses,DataPerPagese,ShumaTotale,ShumaPaguar,Borxhi,DeviceID,SyncStatus,KMON,Export_Status,PayType) " +
                "VALUES ('" + PagesatID + "','" + frmVizitat.Vizitat_IDKlienti + "','" + frmHome.IDAgjenti + "','" + NrFatures + "','" + DataPageses.ToString("yyyy-MM-dd HH:mm:ss") + "','" + DataPerPages.ToString("yyyy-MM-dd") + "',"
                + ShumaFatures.ToString() + "," + ShumaPaguar.ToString() + "," + (ShumaFatures - ShumaPaguar).ToString() + ",'" + frmHome.DevID + "',0,'" + KMON + "',0,'" + PayType + "')";

                con.CommandText = strInsertCommand;
                con.ExecuteNonQuery();
            }
            catch (SqlCeException sce)
            {
                MessageBox.Show(sce.Message + ": Gabim gjate regjistrimit te Pageses");
                DbUtils.WriteSQLCeErrorLog(sce, strInsertCommand);
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message);
                DbUtils.WriteExeptionErrorLog(EX);
            }


        }

        #endregion

        private void txtPako_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                txtPako.Text = txtPako.Text.Trim();
            }


        }

        private string getNurFatures(string KMAG)
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
                        MessageBox.Show("Tejkalimi i 111te faturave");
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

        private void TryToUpdateSeriShitje()
        {
            if (Seri == null || Seri == "")
            {
                string sql = "SELECT Count(*) FROM Stoqet s WHERE s.Shifra = '" + IDArtikulli + "' and Seri = '" + txtSeri.Text + "'";
                int _numRows = int.Parse(DbUtils.ExecSqlScalar(sql));
                if (_numRows == 0)
                {
                    string strUpdateStoqet = "Update Stoqet set Seri = '" + txtSeri.Text + "' where Shifra = '" + IDArtikulli + "' and (Seri = '' or Seri is null)";
                    DbUtils.ExecSql(strUpdateStoqet);

                    string strUpdateMalli_Mbetur = "Update Malli_Mbetur set Seri = '" + txtSeri.Text + "' where IDArtikulli = '" + IDArtikulli + "' and (Seri = '' or Seri is null)";
                    DbUtils.ExecSql(strUpdateMalli_Mbetur);
                    fArt = new frmArtikujtStoqet();
                }

                Seri = txtSeri.Text;
                if (!frmVizitat.KthimMall)
                {
                    txtSeri.Enabled = false;
                }
            }
        }

        private void TryToUpdateSeriKthime()
        {
            if (txtSeri.Text != Seri)
            {
                string sql = "SELECT Count(*) FROM Stoqet s WHERE s.Shifra = '" + IDArtikulli + "' and Seri = '" + txtSeri.Text + "'";
                int _numRows = int.Parse(DbUtils.ExecSqlScalar(sql));
                if (_numRows == 0)
                {
                    string strTryToInsertStoqet = @" Insert into Stoqet (Shifra, Depo, NjesiaMatse, Paketimi, Sasia, Paketa, SyncStatus, Dhurate, NRDOK, LLOJDOK, Seri)
                                        Select Shifra, Depo, NjesiaMatse, Paketimi, 0, Paketa, SyncStatus, Dhurate, NRDOK, LLOJDOK, '" + txtSeri.Text + "' from Stoqet where Shifra = '" + IDArtikulli + "' and Seri = '" + Seri + @"'";
                    DbUtils.ExecSql(strTryToInsertStoqet);

                    string strTryToInsertMalli_Mbetur = @" Insert into Malli_Mbetur (IDArtikulli, Emri, SasiaPranuar, SasiaShitur, SasiaKthyer, SasiaMbetur, NrDoc, SyncStatus, Data, LLOJDOK, Depo, Export_Status, PKeyIDArtDepo, LevizjeStoku, Seri)
                                        Select IDArtikulli, Emri, 0, 0, 0, 0, NrDoc, SyncStatus, Data, LLOJDOK, Depo, Export_Status, IDArtikulli+Depo+'" + txtSeri.Text + "', 0, '" + txtSeri.Text + "' from Malli_Mbetur where IDArtikulli = '" + IDArtikulli + "' and Seri = '" + Seri + @"'";
                    DbUtils.ExecSql(strTryToInsertMalli_Mbetur);

                    fArt = new frmArtikujtStoqet();
                }

                Seri = txtSeri.Text;
            }
        }
    }
}