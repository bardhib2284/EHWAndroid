using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PnUtils;
using System.Data.SqlServerCe;
using System.IO;
using System.Reflection;
using PnReports;
using MobileSales.BL;

namespace MobileSales
{
    public partial class frmLevizjet : Form
    {
        private string _Numri_Levizjes;
        public string artikulli, idArtikulli, njesia_matese, seri, _sqlCeQuery;
        public string _number_to = "", _number_from = "";
        public decimal MaxSasia, Cmimi;
        private SqlCeConnection _con;
        private SqlCeCommand _cmd;
        private SqlCeDataAdapter da;
        private decimal _SASIA, _TOTALI;
        private bool isSelected, Key, HYRJE;
        private bool isNew;
        string _nrPorosise = "";
        private int index_Sasia = 3, index_idArtikulli = 0, index_Artikulli = 1, index_Njesia_Matese = 2, index_seri = 6;
        frmArtikujtStoqet fArt = null;
        private DataTable _dt;
        frmMessage msg = null;
        private string _reasonOfMovement;

        public frmLevizjet()
        {
            InitializeComponent();
        }

        private void menu_EXIT_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Jeni të sigurt për anulim të lëvizjes së filluar", "Dalja", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                _cmd.Transaction.Rollback();
                _cmd.Dispose();
                _con.Dispose();
                this.Close();
            }
        }

        private void frmLevizjet_Load(object sender, EventArgs e)
        {
            fill_cmb();
            _con = new SqlCeConnection(DbUtils.MainSqlConnection.ConnectionString);
            if (_con.State == ConnectionState.Closed) _con.Open();
            _cmd = new SqlCeCommand("", _con);
            _cmd.Transaction = _con.BeginTransaction();
            da = new SqlCeDataAdapter(_cmd);
            _dt = new DataTable();
            //getNumri();
            check_NE.Text = "Transfer në [" + frmHome.Depo + "]";
            check_NGA.Text = "Transfer nga [" + frmHome.Depo + "]";
            Perform_Select();

        }

        private void txtSasia_TextChanged(object sender, EventArgs e)
        {
            if (txtSasia.Text.Trim() != "" && txtArtikulli.Text != "")	//no empty string
            {
                CheckUnit();
            }
        }

        private void txtSasia_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter) { btnShto_Click(null, null); }
            if (e.KeyData == Keys.Up && (isSelected || isNew))
            {
                try
                {
                    _SASIA = decimal.Parse(txtSasia.Text);
                    _SASIA += 1;
                    txtSasia.Text = _SASIA.ToString();
                }
                catch (Exception)
                {
                    MessageBox.Show("Gabim në sasi");
                }
            }

            if (e.KeyData == Keys.Down && (isSelected || isNew))
            {
                try
                {
                    _SASIA = decimal.Parse(txtSasia.Text);
                    if (_SASIA >= 1) { _SASIA -= 1; txtSasia.Text = _SASIA.ToString(); }
                }
                catch (Exception)
                {
                    MessageBox.Show("Gabim në sasi");
                }
            }
        }

        private void menu_REGJISTRO_Click(object sender, EventArgs e)
        {
            if (grdDetails.VisibleRowCount == 0)
            { MessageBox.Show("Nuk keni artikull për regjistrim"); return; }

            if (MessageBox.Show("Jeni të sigurt për përfundim të lëvizje së filluar", "Përfundimi", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                string levizje_nga, levizje_ne;
                if (check_NE.Checked)
                {
                    levizje_ne = frmHome.Depo;
                    levizje_nga = cmb_TRANSFER.Text;
                    txt_Numri.Enabled = false;
                    New_LEVIZJ();
                    if (_Numri_Levizjes != "01")
                    {
                        _cmd.CommandText = "Update NumriPorosive set NrPorosise=NrPorosise+1 where TIPI='LEVIZJE'";
                        _cmd.ExecuteNonQuery();
                    }
                    //if (cmb_TRANSFER.Text.Contains("D"))
                    //{
                    _cmd.CommandText = "Update NumriFaturave set CurrNrFat_D=CurrNrFat_D+1 where KOD='" + cmb_TRANSFER.Text + "'";
                    _cmd.ExecuteNonQuery();
                    // }
                }
                else if (check_NGA.Checked)
                {
                    levizje_nga = frmHome.Depo;
                    levizje_ne = cmb_TRANSFER.Text;
                    txt_Numri.Enabled = true;
                    _cmd.CommandText = "Update NumriFaturave set CurrNrFat_D=CurrNrFat_D+1 where KOD='" + frmHome.IDAgjenti + "'";
                    _cmd.ExecuteNonQuery();
                }

                else
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Duhet të selektohet \"Transfer Nga\" ose \"Transfer Në\" për ta përfunduar lëvizjen!\nJu lutem selektoni njëren.");
                    return;
                }

                if (check_NGA.Checked)
                {
                    string _getNumriFisk = "SELECT top(1) v.LevizjeIDN FROM NumriFisk v WHERE v.Depo = '" + frmHome.IDAgjenti + "'";
                    int _NumriFisk = int.Parse(DbUtils.ExecSqlScalar(_getNumriFisk).ToString());
                    _NumriFisk = _NumriFisk + 1;

                    _cmd.CommandText = @"Insert into [LEVIZJET_HEADER] ([Numri_Levizjes],[Levizje_Nga],[Levizje_Ne],[Data],[Totali],[IDAgjenti],[SyncStatus],[Kodi_Dyqanit],[Numri_Daljes],[Depo], [Longitude], [Latitude], [NumriFisk], TCR, TCROperatorCode, TCRBusinessUnitCode)
                                                      Values('" + lbl_NUMRI.Text + "','" + levizje_nga + "','" + levizje_ne + "',getDate()," + _TOTALI + ",'" + frmHome.IDAgjenti + "',0,'" + cmb_TRANSFER.Text + "','" + txt_Numri.Text + "','" + frmHome.Depo + "', '" + frmHome.LongitudeCur + "', '" + frmHome.LatitudeCur + "', " + _NumriFisk + ", '" + frmHome.TCRCode + "', '" + frmHome.OperatorCode + "', '" + frmHome.BusinessUnitCode + "')";
                    _cmd.ExecuteNonQuery();

                    _cmd.CommandText = @"UPDATE NumriFisk SET LevizjeIDN = " + (_NumriFisk) + " where Depo = '" + frmHome.IDAgjenti + "'";
                    _cmd.ExecuteNonQuery();
                }
                else
                {
                    _cmd.CommandText = @"Insert into [LEVIZJET_HEADER] ([Numri_Levizjes],[Levizje_Nga],[Levizje_Ne],[Data],[Totali],[IDAgjenti],[SyncStatus],[Kodi_Dyqanit],[Numri_Daljes],[Depo], [Longitude], [Latitude], [NumriFisk], TCR, TCROperatorCode, TCRBusinessUnitCode)
                                                      Values('" + lbl_NUMRI.Text + "','" + levizje_nga + "','" + levizje_ne + "',getDate()," + _TOTALI + ",'" + frmHome.IDAgjenti + "',0,'" + cmb_TRANSFER.Text + "','" + txt_Numri.Text + "','" + frmHome.Depo + "', '" + frmHome.LongitudeCur + "', '" + frmHome.LatitudeCur + "', null, '" + frmHome.TCRCode + "', '" + frmHome.OperatorCode + "', '" + frmHome.BusinessUnitCode + "')";
                    _cmd.ExecuteNonQuery();
                }



                Perform_Close();
                _cmd.Transaction.Commit();
                _cmd.Dispose();
                _con.Dispose();

                //BL.SyncBL.SyncTablesWithGPS();
                if (check_NGA.Checked)
                {
                    FiskalizimiBL.RegisterTCRWTN(lbl_NUMRI.Text.Trim());
                }
                Cursor.Current = Cursors.Default;
                if (MessageBox.Show("Dëshironi ta printoni raportin e lëvizjes ?", "Printimi", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    Print(lbl_NUMRI.Text);


                }
                this.Close();
            }
        }

        private void menu_Anulo_Click(object sender, EventArgs e)
        {
            if (grdDetails.VisibleRowCount != 0)
            {
                if (!isSelected)
                {
                    if (isNew)
                    {
                        if (MessageBox.Show("Jeni të sigurt për anulim të artikullit: [" + txtArtikulli.Text + "]", "Anulimi", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            txtArtikulli.Text = ""; txtSasia.Text = ""; isNew = false; grdDetails.Enabled = true; btnShto.Enabled = false;
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Selektoni një artikull për fshirje");
                    }
                }
                else
                {
                    if (MessageBox.Show("Jeni të sigurt për anulim të artikullit: [" + txtArtikulli.Text + "]", "Anulimi", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        idArtikulli = grdDetails[grdDetails.CurrentRowIndex, index_idArtikulli].ToString();
                        seri = grdDetails[grdDetails.CurrentRowIndex, index_seri].ToString();
                        _cmd.CommandText = "Delete from [LEVIZJET_DETAILS] where [Numri_Levizjes]='" + lbl_NUMRI.Text + "' and [IDArtikulli]='" + idArtikulli + "' and Seri = '" + seri + "'";
                        _cmd.ExecuteNonQuery();
                        Perform_Select();
                    }
                }
            }
            else { MessageBox.Show("Nuk keni artikull për anulim"); }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            txtSeri.Enabled = true;
            grdDetails.Enabled = false;
            isNew = true;
            if (fArt == null)
            {
                if (check_NE.Checked)
                { fArt = new frmArtikujtStoqet("Lëvizje", false); }
                else
                { fArt = new frmArtikujtStoqet("Lëvizje", true); }
            }

            fArt.LevizjetCrossForm = this;
            Cursor.Current = Cursors.WaitCursor;
            fArt.Show();
            fArt.txtArtFilter.Focus();
            //txtSasia.Focus();
            Cursor.Current = Cursors.Default;
            for (int i = 0; i < grdDetails.VisibleRowCount; i++)
            {
                grdDetails.UnSelect(i);
            }
            isSelected = false;
        }

        private void check_NGA_CheckStateChanged(object sender, EventArgs e)
        {
            fArt = null;
            check_NE.Checked = false;
            check_NE.Enabled = false;
            if (check_NGA.Checked)
            { lbl_Transfer.Text = "Transfer në"; }
            pnlAll.Enabled = true;
            txt_Numri.Enabled = false;
            HYRJE = false;
            cmb_TRANSFER.SelectAll();
            cmb_TRANSFER.Focus();
            getNumri();
            lbl_NUMRI.Text = _Numri_Levizjes;
            _number_from = lbl_NUMRI.Text;
            CheckChange();
        }

        private void check_NE_CheckStateChanged(object sender, EventArgs e)
        {
            fArt = null;
            check_NGA.Checked = false;
            check_NGA.Enabled = false;
            if (check_NE.Checked)
            { lbl_Transfer.Text = "Transfer nga"; }
            pnlAll.Enabled = true;
            txt_Numri.Enabled = true;
            HYRJE = true;
            cmb_TRANSFER.SelectAll();
            cmb_TRANSFER.Focus();
            New_LEVIZJ();
            lbl_NUMRI.Text = frmHome.Depo + "-" + _Numri_Levizjes;
            _number_to = lbl_NUMRI.Text;
            CheckChange();
        }

        private void btnShto_Click(object sender, EventArgs e)
        {
            if (txtSeri.Text != "")
            {
                try
                {
                    TryToInsert();
                    if (!isNew) { idArtikulli = grdDetails[grdDetails.CurrentRowIndex, index_idArtikulli].ToString(); }
                    _SASIA = Math.Round(decimal.Parse(txtSasia.Text), 2);
                    MaxSasia = decimal.Parse(DbUtils.ExecSqlScalar("Select [Sasia] from [Stoqet] where [Shifra]='" + idArtikulli + "' and Seri = '" + seri + "'").ToString());
                    if (!HYRJE)
                    {
                        if (_SASIA > MaxSasia || _SASIA < 0) { MessageBox.Show("Sasia maksimale për lëvizje është " + MaxSasia); return; }
                    }
                    if (isNew)
                    {
                        if (Perform_Insert())
                        {
                            isNew = false;
                            grdDetails.Enabled = true;
                            btnAdd.Enabled = true;
                            txtArtikulli.Text = "";
                            txtSasia.Text = "";
                            txtSeri.Text = "";
                            btnShto.Enabled = true;
                            txtSeri.Enabled = false;
                        }
                    }
                    else
                    {
                        if (Perform_Update())
                        {

                        }
                    }
                    Perform_Select();
                }
                catch (SqlCeException sce)
                {
                    if (sce.NativeError == 25016)
                    {
                        MessageBox.Show("Gabim:\nRegjistrim i dyfishtë i artikullit: " + txtArtikulli.Text);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Gabim në sasi të artikullit");
                    txtSasia.Focus();
                    return;
                }
            }
            else
            {
                MessageBox.Show("Nuk mund të shtohet Artikull i cili ka Serinë të paplotësuar!");
                txtSeri.Focus();
            }
        }

        #region Data Grid
        private void grdDetails_MouseUp(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.DataGrid.HitTestInfo myHitTest = this.grdDetails.HitTest(e.X, e.Y);
            if (myHitTest.Type == DataGrid.HitTestType.Cell)
            {
                isSelected = true;
                this.grdDetails.Select(myHitTest.Row);
                txtArtikulli.Text = grdDetails[grdDetails.CurrentRowIndex, index_Artikulli].ToString();
                txtSasia.Text = grdDetails[grdDetails.CurrentRowIndex, index_Sasia].ToString();
                txtSeri.Text = grdDetails[grdDetails.CurrentRowIndex, index_seri].ToString();
                njesia_matese = grdDetails[grdDetails.CurrentRowIndex, index_Njesia_Matese].ToString();
            }
        }

        private void grdDetails_KeyDown(object sender, KeyEventArgs e)
        {
            Key = (e.KeyData == Keys.Up || e.KeyData == Keys.Down || e.KeyData == Keys.Left || e.KeyData == Keys.Right);
            if (isSelected == true && Key)
            {
                this.grdDetails.Select(this.grdDetails.CurrentRowIndex);
                txtArtikulli.Text = grdDetails[grdDetails.CurrentRowIndex, index_Artikulli].ToString();
                txtSasia.Text = grdDetails[grdDetails.CurrentRowIndex, index_Sasia].ToString();
                txtSeri.Text = grdDetails[grdDetails.CurrentRowIndex, index_seri].ToString();
                njesia_matese = grdDetails[grdDetails.CurrentRowIndex, index_Njesia_Matese].ToString();
            }
        }
        #endregion

        #region Methods

        private bool Perform_Insert()
        {
            bool a = false;

            _cmd.CommandText = @"Insert into [LEVIZJET_DETAILS] 
                                           ([Numri_Levizjes],[Artikulli],[IDArtikulli],[Sasia],[Cmimi],[Njesia_matese],[Totali],[SyncStatus],[Seri], [Depo])
                                    Values ('" + lbl_NUMRI.Text + "','" + txtArtikulli.Text + "','" + idArtikulli + "'," + _SASIA + "," + Cmimi + ",'" + njesia_matese + "'," + Math.Round((_SASIA * Cmimi), 2) + ",0,'" + seri + "','" + frmHome.Depo + "')";
            _cmd.ExecuteNonQuery();

            a = true;
            return a;
        }

        private void CheckChange()
        {
            int rows = grdDetails.VisibleRowCount;
            if (rows > 0)
            {
                if (lbl_NUMRI.Text == _number_to && check_NE.Checked && _number_from != "")
                {

                    _cmd.CommandText = @"Update [LEVIZJET_DETAILS] 
                                            Set
                                           [Numri_Levizjes]='" + _number_to + "' Where [Numri_Levizjes]='" + _number_from + "'";
                    _cmd.ExecuteNonQuery();

                }

                else if (lbl_NUMRI.Text == _number_from && check_NGA.Checked && _number_to != "")
                {
                    _cmd.CommandText = @"Update [LEVIZJET_DETAILS] 
                                            Set
                                           [Numri_Levizjes]='" + _number_from + "' Where [Numri_Levizjes]='" + _number_to + "'";
                    _cmd.ExecuteNonQuery();

                }
            }

        }
        private void Perform_Select()
        {
            _dt.Clear();
            _cmd.CommandText = "Select * from [LEVIZJET_DETAILS] Where [Numri_Levizjes]='" + lbl_NUMRI.Text + "'";
            da.Fill(_dt);
            grdDetails.DataSource = null;
            grdDetails.DataSource = _dt;
            PnFunctions.CalcSumDataColumn("Totali", lblTotali, _dt, 2, "");
            _TOTALI = decimal.Parse(lblTotali.Text);
            txtArtikulli.Text = "";
            txtSasia.Text = "";
        }

        private void fill_cmb()
        {
            DataTable dt = new DataTable();
            DbUtils.FillDataTable(dt, "Select [IDDepo],[Depo] from [Depot] order by Depo asc");
            cmb_TRANSFER.DataSource = dt;
            cmb_TRANSFER.DisplayMember = "Depo";
            cmb_TRANSFER.ValueMember = "IDDepo";
        }

        private void Perform_Close()
        {

            for (int i = 0; i < _dt.Rows.Count; i++)
            {
                _SASIA = Math.Round(decimal.Parse(grdDetails[i, index_Sasia].ToString()), 2);
                if (!HYRJE) { _SASIA = -_SASIA; }
                idArtikulli = grdDetails[i, index_idArtikulli].ToString();
                seri = grdDetails[i, index_seri].ToString();
                _cmd.CommandText = "Update Stoqet set [Sasia]=[Sasia]+" + _SASIA + " where [Shifra]='" + idArtikulli + "' and Seri = '" + seri + "'";
                _cmd.ExecuteNonQuery();
                _cmd.CommandText = "Update [Malli_Mbetur] set [LevizjeStoku]=[LevizjeStoku]+" + _SASIA + ", SyncStatus=0 where IDArtikulli='" + idArtikulli + "' and Seri = '" + seri + "'";
                _cmd.ExecuteNonQuery();
            }
            _cmd.CommandText = @"Update Malli_Mbetur set SasiaMbetur=Round(SasiaPranuar-(SasiaShitur+SasiaKthyer-LevizjeStoku),3), SyncStatus=0";
            _cmd.ExecuteNonQuery();
        }

        private bool Perform_Update()
        {

            _cmd.CommandText = "Update [LEVIZJET_DETAILS] SET [Sasia]=" + _SASIA + " where [Numri_Levizjes]='" + lbl_NUMRI.Text + "' and [IDArtikulli]='" + idArtikulli + "' and Seri = '" + seri + "'";
            _cmd.ExecuteNonQuery();

            _cmd.CommandText = "Update [LEVIZJET_DETAILS] SET [Totali]=[Sasia]*[Cmimi] where [Numri_Levizjes]='" + lbl_NUMRI.Text + "' ";
            _cmd.ExecuteNonQuery();
            return true;
        }

        private bool IsCountUnit()
        {
            bool a = false;

            if ((!btnShto.Enabled))
            {
                a = njesia_matese != "KG";
            }
            else
            {
                a = (njesia_matese != "KG");
            }
            return a;
        }

        private void CheckUnit()
        {
            if (IsCountUnit())
            {
                txtSasia.Text = txtSasia.Text.Replace(".", "");
                if (txtSasia.Text.Contains('.'))
                {
                    int _indexOfPoint = txtSasia.Text.IndexOf('.');
                    int _numAfterPoint = txtSasia.Text.Substring(_indexOfPoint, txtSasia.Text.Length).Length;
                    if (_numAfterPoint > 3)
                    {

                    }
                }

            }
        }

        private void New_LEVIZJ()
        {
            //LËV-[SHITESI]-[DATA]-##
            //              [yy-mm-dd]
            //LËV-AMB-01-100120-01

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

            _cmd.CommandText = "Select Count(*) From NumriPorosive where TIPI='LEVIZJE'";
            if (int.Parse(_cmd.ExecuteScalar().ToString()) == 0)
            {
                //Insert new and set _nrLevizjes=01;
                _cmd.CommandText = @"Insert Into NumriPorosive (Data,NrPorosise,TIPI) 
                                                      Values ('" + DateTime.Now.Date + "','" + 01 + "','LEVIZJE')";
                _cmd.ExecuteNonQuery();
                _nrPorosise = "01";
            }
            else
            {
                _cmd.CommandText = "Select NrPorosise From NumriPorosive where TIPI='LEVIZJE' ";
                _nrPorosise = (int.Parse(_cmd.ExecuteScalar().ToString()) + 1).ToString().PadLeft(2, '0');


            }

            _Numri_Levizjes = _nrPorosise;
        }

        private void getNumri()
        {
            _cmd.CommandText = "Select NRKUFIP_D+CurrNrFat_D from NumriFaturave where KOD='" + frmHome.IDAgjenti + "'";
            _Numri_Levizjes = _nrPorosise = _cmd.ExecuteScalar().ToString();
        }

        #endregion

        private void Print(string nrLevizjes)
        {
            string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            PnPrint Rpt = null;
            PnType PT = PnType.Print;
            string _IDPorosia = frmVizitat.IDPorosi.ToString();
            DataTable details = new DataTable();
            DataTable header = new DataTable();
            DataTable footer = new DataTable();
            string strPortName = "";
            PnDevice devPort = PnDevice.BluetoothPort;
            DataTable TCRQRCodeTbl = new DataTable();

            #region PrintParams

            //@BE QR Code
            PnUtils.DbUtils.FillDataTable(TCRQRCodeTbl, @"
                                                            Select Numri_Levizjes, Totali, TCRQRCodeLink from Levizjet_Header
                                                            where Numri_Levizjes = '" + nrLevizjes + @"'
                                                            and TCRNSLFSH <> ''
                                                        ");
            if (TCRQRCodeTbl.Rows.Count > 0)
            {
                if (msg == null) msg = new frmMessage(true, new string[] { TCRQRCodeTbl.Rows[0]["Totali"].ToString(),
                                                        TCRQRCodeTbl.Rows[0]["Numri_Levizjes"].ToString(), 
                                                        TCRQRCodeTbl.Rows[0]["TCRQRCodeLink"].ToString() });
            }
            else
            {
                if (msg == null) msg = new frmMessage(true, new string[3]);
            }
            DialogResult a = msg.ShowDialog();

            if (a == DialogResult.Abort)
            {
                return;//cancel printing
            }

            if (frmMessage.isBlutooth)
            {
                PT = PnType.Print;
                devPort = PnDevice.BluetoothPort;
                strPortName = frmHome.PnPrintPort + ":";
            }
            if (frmMessage.isPreview)
            {
                PT = PnType.Preview;
                devPort = PnDevice.PreviewOnly;
            }
            //if (frmMessage.isMultiplicatior)
            //{
            //    devPort = PnDevice.MultiplicatorPort;
            //    strPortName = frmHome.PnPrintPort + ":";
            //    PT = PnType.Print;
            //}
            if (!frmMessage.isPreview)
            {
                if (!PnFunctions.AvailablePort(strPortName))
                {
                    MessageBox.Show("Porti: " + strPortName + " nuk munde te hapet");
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }
            #endregion

            string _adresa_SHITESI = "";
            string _adresa_BLERESIT = "";
            string _nipt_BLERESIT = "";
            string _sn_BLERESIT = "";
            string _Drejteues = "";
            string _NISESI = "";
            string _PRITESI = "";
            string _targ_mjetit = "";
            string _numriFatures = "";
            string _numriHyres = "";
            string _magazina_PRITESIT = "";
            string numri = nrLevizjes;
            if (check_NE.Checked) //Hyrje
            {
                _adresa_BLERESIT = DbUtils.ExecSqlScalar("SELECT d.TAGNR FROM Depot d WHERE d.Depo='" + frmHome.Depo + "'");
                _magazina_PRITESIT = frmHome.Depo;
                if (cmb_TRANSFER.Text.Contains("D"))
                {
                    _adresa_SHITESI = DbUtils.ExecSqlScalar("SELECT d.Adresa FROM Depot d WHERE d.Depo='" + cmb_TRANSFER.Text + "'");
                }
                else
                {
                    _adresa_SHITESI = DbUtils.ExecSqlScalar("SELECT d.TAGNR FROM Depot d WHERE d.Depo='" + cmb_TRANSFER.Text + "'");
                }
                _nipt_BLERESIT = DbUtils.ExecSqlScalar("SELECT d.NIPT FROM Depot d WHERE d.Depo='" + frmHome.Depo + "'");
                _sn_BLERESIT = DbUtils.ExecSqlScalar("SELECT d.SN FROM Depot d WHERE d.Depo='" + frmHome.Depo + "'");
                if (cmb_TRANSFER.Text.StartsWith("M"))
                {
                    _Drejteues = DbUtils.ExecSqlScalar("SELECT a.Emri+' '+a.Mbiemri FROM Agjendet a WHERE a.IDAgjenti='" + cmb_TRANSFER.Text + "'");
                    _targ_mjetit = DbUtils.ExecSqlScalar("SELECT d.TAGNR FROM Depot d WHERE d.Depo='" + cmb_TRANSFER.Text + "'");
                    _NISESI = _Drejteues;
                    _PRITESI = frmHome.EmriAgjendit + " " + frmHome.MbiemriAgjendit;
                    numri = _numriFatures = lbl_NUMRI.Text;
                }
                else
                {
                    _targ_mjetit = DbUtils.ExecSqlScalar("SELECT d.TAGNR FROM Depot d WHERE d.Depo='" + frmHome.Depo + "'");
                    _PRITESI = frmHome.EmriAgjendit + " " + frmHome.MbiemriAgjendit;
                    _Drejteues = _PRITESI;
                    _NISESI = DbUtils.ExecSqlScalar("SELECT d.Adresa FROM Depot d WHERE d.Depo='" + cmb_TRANSFER.Text + "'");
                    _numriFatures = lbl_NUMRI.Text;
                    numri = txt_Numri.Text;
                }
                _numriHyres = "        " + cmb_TRANSFER.Text;// +" [" + txt_Numri.Text + "]";
            }
            else //Dalje
            {
                _adresa_SHITESI = DbUtils.ExecSqlScalar("SELECT d.TAGNR FROM Depot d WHERE d.Depo='" + frmHome.Depo + "'");
                _nipt_BLERESIT = DbUtils.ExecSqlScalar("SELECT d.NIPT FROM Depot d WHERE d.Depo='" + cmb_TRANSFER.Text + "'");
                _sn_BLERESIT = DbUtils.ExecSqlScalar("SELECT d.SN FROM Depot d WHERE d.Depo='" + cmb_TRANSFER.Text + "'");
                _Drejteues = frmHome.EmriAgjendit + " " + frmHome.MbiemriAgjendit;
                _NISESI = _Drejteues;
                _targ_mjetit = DbUtils.ExecSqlScalar("SELECT d.TAGNR FROM Depot d WHERE d.Depo='" + frmHome.Depo + "'");

                if (cmb_TRANSFER.Text.Contains("M"))
                {
                    _adresa_BLERESIT = DbUtils.ExecSqlScalar("SELECT d.TAGNR FROM Depot d WHERE d.Depo='" + cmb_TRANSFER.Text + "'");
                    _magazina_PRITESIT = cmb_TRANSFER.Text;
                    _PRITESI = DbUtils.ExecSqlScalar("SELECT a.Emri+' '+a.Mbiemri FROM Agjendet a WHERE a.IDAgjenti='" + cmb_TRANSFER.Text + "'");
                    _Drejteues = _PRITESI;
                }
                else
                {
                    _adresa_BLERESIT = DbUtils.ExecSqlScalar("SELECT d.Adresa FROM Depot d WHERE d.Depo='" + cmb_TRANSFER.Text + "'");
                    _magazina_PRITESIT = cmb_TRANSFER.Text;
                    _PRITESI = DbUtils.ExecSqlScalar("SELECT d.Adresa FROM Depot d WHERE d.Depo='" + cmb_TRANSFER.Text + "'");
                }
                numri = _numriFatures = _nrPorosise;
                _numriHyres = "";
            }
            try
            {
                string stm = "";
                if (frmHome.Depo == "AT")
                {
                    _reasonOfMovement = "Shitje me pakice";
                    stm = "('emer,mbiemer,nenshkrim') AS Agjenti,";
                }

                else
                {
                    _reasonOfMovement = "Shitje me shumice";
                    stm = "('') AS Agjenti,";
                }

                header = new DataTable();
                _sqlCeQuery = @"
                                SELECT ('Emri i Nisesit: ' + c.[Value] + '   ' + cl.[Value]) AS [FirstHeader]
                                      FROM   CompanyInfo c, CompanyInfo cl
                                      WHERE  c.Item = 'Shitesi' and cl.Item = 'NIPT' 

                                        UNION ALL       


                                SELECT ('Tel: 048 200 711      web: www.ehwgmbh.com') AS [FirstHeader] 

                                        UNION ALL  

                                SELECT ('Adresa: ' +'" + _adresa_SHITESI + @"') + ('   " + _numriHyres + @"') AS [FirstHeader]
                                      

                                      UNION ALL             
                                      

                                select 'Qyteti / Shteti: Tirana, Albania' as FirstHeader
                                      

                                      UNION ALL  

                                select '----------------------------------------------------------------------' as [FirstHeader]

                                        UNION ALL 
 
                                SELECT ('Numri i fatures: ' + CASE 
                                                            WHEN l.NumriFisk IS NOT NULL THEN CONVERT(NCHAR(20),l.NumriFisk) + '/' + '" + frmHome.Viti + @"'
                                                            ELSE l.Numri_Levizjes
                                                       END) + ('          Nr. Serise: ' + '" + numri + @"')  AS [FirstHeader] 
                                      from Levizjet_Header l 
                                      where l.Numri_Levizjes = '" + nrLevizjes + @"'


                                      UNION ALL  
                                      
                                SELECT  'Data dhe ora e leshimit te fatures: ' + CONVERT(NCHAR(10), l.Data, 104) + 
                                                 ' '+CONVERT(NCHAR(10), l.Data, 8) AS [FirstHeader]  
                                      from Levizjet_Header l where l.Numri_Levizjes = '" + nrLevizjes + @"'  


                                      UNION ALL   

                                      
                                SELECT ('Kodi i vendit te ushtrimit te veprimtarise se biznesit: ' + CASE 
                                                            WHEN l.TCRBusinessUnitCode IS NOT NULL THEN l.TCRBusinessUnitCode
                                                            ELSE ''
                                                       END) AS [FirstHeader] 
                                      from Levizjet_Header l where l.Numri_Levizjes = '" + nrLevizjes + @"'  


                                      UNION ALL   

                                      
                                      SELECT ('Kodi i operatori: ' + CASE 
                                                            WHEN l.TCROperatorCode IS NOT NULL THEN l.TCROperatorCode
                                                            ELSE ''
                                                       END) AS [FirstHeader] 
                                      from Levizjet_Header l where l.Numri_Levizjes = '" + nrLevizjes + @"' 


                                      UNION ALL   

                                      select ('NIVFSH: ' + CASE 
                                                              WHEN l.TCRNIVFSH IS NOT NULL THEN l.TCRNIVFSH
                                                              ELSE ''
                                                         END) AS [FirstHeader]
                                      from Levizjet_Header l where l.Numri_Levizjes = '" + nrLevizjes + @"' 


                                      UNION ALL   

                                      select ('NSLFSH: ' + CASE 
                                                              WHEN l.TCRNSLFSH IS NOT NULL THEN l.TCRNSLFSH
                                                              ELSE ''
                                                         END) AS [FirstHeader]
                                      from Levizjet_Header l where l.Numri_Levizjes = '" + nrLevizjes + @"' 

                                        UNION ALL

                                      SELECT ('Qellimi i Levizjes se Mallit:'+'" + _reasonOfMovement + @"') AS [FirstHeader]

                                        UNION ALL 

                                      select '----------------------------------------------------------------------' as [FirstHeader]  

                                        UNION ALL 


                                      SELECT ('Emri i Pritesit: ' + c.[Value] + '  NIPT: '+'" + _nipt_BLERESIT + "  ' +'SN: '+'" + _sn_BLERESIT + @"') AS [FirstHeader]
                                        FROM   CompanyInfo c 
                                        WHERE  c.Item = 'Shitesi'  

                                        UNION ALL 


                                      SELECT ('Adresa: ' + '" + _adresa_BLERESIT + @" '+ ' " + _magazina_PRITESIT + @"') AS [FirstHeader]
                                      

                                      UNION ALL  

                                      select '----------------------------------------------------------------------' as [FirstHeader] 
                                      

                                      UNION ALL  


                                      SELECT  ('Transportues: '+'" + _Drejteues + @"' + '   ' + cl.[Value]) AS [FirstHeader]
                                      FROM   CompanyInfo cl
                                      WHERE  cl.Item = 'NIPT' 

                                      UNION ALL   


                                      SELECT  ('Adresa: ' + c1.[Value]) AS FirstHeader
                                      FROM   CompanyInfo C1 
                                      WHERE  C1.Item = 'Adresa' 

                                      UNION ALL  

                                      SELECT ('Mjeti:' + '" + _targ_mjetit + @"') AS [FirstHeader]


                                      UNION ALL 
                                      
                                      SELECT  'Data dhe ora e furnizimit: ' + CONVERT(NCHAR(10), l.Data, 104) + 
                                                 ' '+CONVERT(NCHAR(10), l.Data, 8) AS [FirstHeader]  
                                      from Levizjet_Header l where l.Numri_Levizjes = '" + nrLevizjes + @"'  
                                       
                                ";
                DbUtils.FillDataTable(header, _sqlCeQuery);


                


                footer = new DataTable();

                string commFooter = @"
                                       SELECT  
                                             ('') AS Blersi, 
                                             ('') AS Transportuesi, 
                                             ('') AS Agjenti  

                                         UNION ALL  

                                        SELECT  
                                         ('Nisesi') AS [Blersi], 
                                         ('Transportuesi') AS [Transportuesi] , 
                                         ('Pritesi') AS [Agjenti] 

                                   
                                      UNION ALL  

                                      SELECT  
                                        ('" + _NISESI + @"') AS Blersi,
                                        ('" + _Drejteues + @"') AS [Transportuesi], 
                                        ('" + _PRITESI + @"') AS Agjenti 

                                         UNION ALL  


                                       SELECT  
                                             ('') AS Blersi, 
                                             ('') AS Transportuesi, 
                                             ('') AS Agjenti 

                                      UNION ALL  
                                      

                                      SELECT  
                                                   ('_____________________') AS Blersi, 
                                                   ('_____________________') AS Transportuesi, 
                                                   ('_____________________') AS Agjenti

                                   
                                      UNION ALL  

                                      SELECT  
                                        ('(emri,mbiemri,nensh.)') AS Blersi,
                                        ('(emri,mbiemri,nensh.)') AS [Transportuesi], 
                                        ('(emri,mbiemri,nensh.)') AS Agjenti ";

                DbUtils.FillDataTable(footer, commFooter);

                details = new DataTable();
                _sqlCeQuery = @"SELECT ld.IDArtikulli AS Shifra,
                                       REPLACE(REPLACE(REPLACE(REPLACE(ld.Artikulli, 'ç', 'c'), 'Ç', 'C'),'ë','e'),'Ë','E')
                                       + '   ' + (CASE WHEN ld.Seri IS NOT NULL THEN ld.Seri ELSE '' END) AS Pershkrimi,
                                       ld.Njesia_matese AS Njesia,
                                       CONVERT(DECIMAL(10, 3), ROUND(ld.Sasia, 3)) AS Sasia,
                                       CONVERT(DECIMAL(15, 2), ROUND(100 * (ld.Cmimi) / (100 + 20), 2)) AS 
                                       [CmimiPaTVSh],
                                       CONVERT(DECIMAL(10, 2), ROUND(100 * (ld.Totali) / (100 + 20), 2)) AS 
                                       [VleftaPaTVSh],
                                       CONVERT(DECIMAL(10, 2),ROUND(ld.Totali -100 * (ld.Totali) / (100 + 20), 2)) AS [VlefteTVSh],
                                       CONVERT(DECIMAL(10, 2), ROUND(ld.Totali, 2)) AS [VleftaMeTVSh],
                                       CONVERT(DECIMAL(10, 2), ROUND(ld.Cmimi, 3)) AS [CmimiMeTVSh]
                                FROM   LEVIZJET_DETAILS ld
                                WHERE  ld.Numri_Levizjes = '" + nrLevizjes + "'";
                DbUtils.FillDataTable(details, _sqlCeQuery);


                if (details.Rows.Count > 0 && header.Rows.Count > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    //@BE DUHET NDRYSHOHET
                    BxlPrint.PrinterOpen("COM1:115200", 1000);
                    BxlPrint.PrintBitmap(CurrentDir + "\\Image\\EHW-logo.png", 100, BxlPrint.BXL_ALIGNMENT_CENTER, 30);
                    BxlPrint.LineFeed(1);

                    Rpt = new PnPrint(devPort, strPortName, CurrentDir + "\\rptLevizja.pnrep", details, header, footer);
                    Rpt.PrintPnReport(PT, true, " tre ", false);

                    if (TCRQRCodeTbl.Rows.Count > 0)
                    {
                            byte[] bytes = Encoding.UTF8.GetBytes(TCRQRCodeTbl.Rows[0]["TCRQRCodeLink"].ToString());
                            //byte[] secondBarcodeBytes = Encoding.UTF8.GetBytes(barcodeString);
                            BxlPrint.PrintBarcode(bytes, BxlPrint.BXL_BCS_QRCODE_MODEL2, 0, 4, BxlPrint.BXL_ALIGNMENT_CENTER, 0);
                            BxlPrint.PrintText("Te gjitha informacionet ne lidhje me kete fature mund te shihen ne \n kete Kod QR", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                            BxlPrint.LineFeed(5);
                    }
                    else
                    {
                        //byte[] QRbytes = Encoding.UTF8.GetBytes("DOKUMENT I PAFISKALIZUAR!");
                        //BxlPrint.PrintBarcode(QRbytes, BxlPrint.BXL_BCS_QRCODE_MODEL2, 0, 6, BxlPrint.BXL_ALIGNMENT_CENTER, 0);
                        BxlPrint.PrintText("DOKUMENT I PAFISKALIZUAR! \n", BxlPrint.BXL_ALIGNMENT_CENTER, BxlPrint.BXL_FT_DEFAULT, BxlPrint.BXL_TS_0WIDTH | BxlPrint.BXL_TS_0HEIGHT);
                        BxlPrint.LineFeed(4); 
                    }
                }
                else
                {
                    MessageBox.Show("Gabim gjatë gjenerimit te raportit \n Printimin nuk munde të vazhdoj");
                    return;
                }
            }
            catch (FileNotFoundException fex)
            {
                MessageBox.Show("Mungon Fajlli: \n rptLevizja.pnrep");
                PnUtils.DbUtils.WriteExeptionErrorLog(fex);
            }
            catch (TypeLoadException)
            { }
            catch (MissingMethodException)
            { }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                PnUtils.DbUtils.WriteExeptionErrorLog(ex);
            }
            finally
            {
                details.Dispose();
                header.Dispose();
                if (Rpt != null) Rpt.Dispose();
                if (msg != null) msg.Dispose();
                frmMessage.isPreview = false;
                frmMessage.isBlutooth = false;
                frmMessage.isMultiplicatior = false;
                msg = null;
                Cursor.Current = Cursors.Default;
            }
        }

        private void cmb_TRANSFER_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void cmb_TRANSFER_LostFocus(object sender, EventArgs e)
        {
            if (check_NE.Checked)
            {
                if (cmb_TRANSFER.Text == frmHome.Depo)
                {
                    MessageBox.Show("Nuk lejohet lëvizja nga [" + frmHome.Depo + "]");
                    cmb_TRANSFER.SelectAll();
                    cmb_TRANSFER.Focus();
                }
                else
                {
                    if (cmb_TRANSFER.Text.Contains("'") || cmb_TRANSFER.Text.Contains("--"))
                    {
                        cmb_TRANSFER.Text = "";
                    }

                    //if (cmb_TRANSFER.Text.Contains("D"))
                    //{
                    try
                    {

                        txt_Numri.Text = DbUtils.ExecSqlScalar("SELECT nf.NRKUFIP_D+nf.CurrNrFat_D FROM NumriFaturave nf WHERE nf.KOD='" +
                            cmb_TRANSFER.Text + "'");
                    }
                    catch (Exception)
                    {
                        // MessageBox.Show("Nuk është caktuar numri i faturave për këtë dyqan");
                        //cmb_TRANSFER.Focus();
                    }
                    //}
                    //else
                    //{
                    //    txt_Numri.Text = "";
                    //}
                }
            }
            if (check_NGA.Checked)
            {
                if (cmb_TRANSFER.Text == frmHome.Depo)
                {
                    MessageBox.Show("Nuk lejohet lëvizja në [" + frmHome.Depo + "]");
                    cmb_TRANSFER.SelectAll();
                    cmb_TRANSFER.Focus();
                }
            }
        }
        private void TryToInsert()
        {
            if (seri == null || seri == "")
            {
                string sql = "SELECT Count(*) FROM Stoqet s WHERE s.Shifra = '" + idArtikulli + "' and Seri = '" + txtSeri.Text + "'";
                int _numRows = int.Parse(DbUtils.ExecSqlScalar(sql));
                if (_numRows == 0)
                {
                    string strUpdateStoqet = "Update Stoqet set Seri = '" + txtSeri.Text + "' where Shifra = '" + idArtikulli + "' and (Seri = '' or Seri is null)";
                    DbUtils.ExecSql(strUpdateStoqet);

                    string strUpdateMalli_Mbetur = "Update Malli_Mbetur set Seri = '" + txtSeri.Text + "' where IDArtikulli = '" + idArtikulli + "' and (Seri = '' or Seri is null)";
                    DbUtils.ExecSql(strUpdateMalli_Mbetur);


                    if (check_NE.Checked)
                    { fArt = new frmArtikujtStoqet("Lëvizje", false); }
                    else
                    { fArt = new frmArtikujtStoqet("Lëvizje", true); }
                }

                seri = txtSeri.Text;
            }
            else if (txtSeri.Text != seri)
            {
                string sql = "SELECT Count(*) FROM Stoqet s WHERE s.Shifra = '" + idArtikulli + "' and Seri = '" + txtSeri.Text + "'";
                int _numRows = int.Parse(DbUtils.ExecSqlScalar(sql));
                if (_numRows == 0)
                {
                    string strTryToInsertStoqet = @" Insert into Stoqet (Shifra, Depo, NjesiaMatse, Paketimi, Sasia, Paketa, SyncStatus, Dhurate, NRDOK, LLOJDOK, Seri)
                                        Select Shifra, Depo, NjesiaMatse, Paketimi, 0, Paketa, SyncStatus, Dhurate, NRDOK, LLOJDOK, '" + txtSeri.Text + "' from Stoqet where Shifra = '" + idArtikulli + "' and Seri = '" + seri + @"'";
                    DbUtils.ExecSql(strTryToInsertStoqet);

                    string strTryToInsertMalli_Mbetur = @" Insert into Malli_Mbetur (IDArtikulli, Emri, SasiaPranuar, SasiaShitur, SasiaKthyer, SasiaMbetur, NrDoc, SyncStatus, Data, LLOJDOK, Depo, Export_Status, PKeyIDArtDepo, LevizjeStoku, Seri)
                                        Select IDArtikulli, Emri, 0, 0, 0, 0, NrDoc, SyncStatus, Data, LLOJDOK, Depo, Export_Status, IDArtikulli+Depo+'" + txtSeri.Text + "', 0, '" + txtSeri.Text + "' from Malli_Mbetur where IDArtikulli = '" + idArtikulli + "' and Seri = '" + seri + @"'";
                    DbUtils.ExecSql(strTryToInsertMalli_Mbetur);

                    if (check_NE.Checked)
                    { fArt = new frmArtikujtStoqet("Lëvizje", false); }
                    else
                    { fArt = new frmArtikujtStoqet("Lëvizje", true); }
                }

                seri = txtSeri.Text;
            }
        }

    }
}
