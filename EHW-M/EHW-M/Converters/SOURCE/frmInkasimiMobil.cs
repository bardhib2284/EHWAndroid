using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Data.SqlServerCe;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using PnReports;
using Microsoft.VisualBasic;
using MobileSales.BL;

namespace MobileSales
{
    public partial class frmInkasimiMobil : Form
    {
        public static string _pType;
        public frmInkasimiMobil()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(string IDKlienti)
        {
            Inkasimi_Mobile_IDKlienti = IDKlienti;
            return this.ShowDialog();
        }

        DataSet dsInkasimi = null; //internal use
        DataSet dsKlientet = null;//internal use
        SqlCeDataAdapter da = null;//internal use
        SqlCeConnection conn = null;//internal use       

        private string Inkasimi_Mobile_IDKlienti = ""; //gets value from other form, or when selecting cmbKlienti       
        private string strPrintPort = "COM5:";

        #region HelperFunctions
        private void Init()
        {
            //instantiate objects if needed   
            if (conn == null) conn = new SqlCeConnection(frmHome.AppDBConnectionsStr);
            if (dsInkasimi == null) dsInkasimi = new DataSet();
            if (da == null)
            {
                da = new SqlCeDataAdapter("select * from EvidencaPagesave", conn);
            }

            DataSet dsConfig = new DataSet();
            try
            {
                string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
                dsConfig.ReadXml(CurrentDir + "\\Config.xpn");
                strPrintPort = dsConfig.Tables[0].Rows[0]["PrintPort"].ToString();
            }
            finally
            {
                dsConfig.Dispose();
            }



            //fill dataSet "dsKlientet"
            if (dsKlientet == null) dsKlientet = new DataSet();
            dsKlientet.Clear();
            da.SelectCommand.CommandText = "select IDKlienti,Emri from Klientet Order By Emri ASC";
            da.Fill(dsKlientet);

            // da.SelectCommand.CommandText = "select DataPerPagese,Borxhi from EvidencaPagesave";
            //  da.Fill(dsInkasimi);
            FillGrid(Inkasimi_Mobile_IDKlienti);

            //bind controls to corresponding dataSets
            //  grdFaturatPagesat.DataSource = dsInkasimi.Tables[0];

            cmbKlienti.DataSource = dsKlientet.Tables[0];
            cmbKlienti.DisplayMember = "Emri";
            cmbKlienti.ValueMember = "IDKlienti";

        }

        private void Inkaso(float Vlera, DateTime DataPageses, string PayType)
        {
            SqlCeCommand kom = new SqlCeCommand();
            SqlCeConnection conn = new SqlCeConnection(frmHome.AppDBConnectionsStr);
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

            try
            {
                kom.Connection = conn;
                //kom.CommandText =
                //"INSERT INTO EvidencaPagesave (NrPageses,ShumaPaguar,DataPageses,IDAgjenti,IDKlienti,DeviceID,SyncStatus)" +
                //" VALUES('"+PagesatID+"',"+Vlera.ToString() + ",'" + DataPageses.ToString("yyyy.MM.dd") + "'," +
                //" '" + Home.IDAgjenti + "','" + strIDKlienti + "','" + Home.DevID + "',0)";

                kom.CommandText =
                "INSERT INTO EvidencaPagesave (NrPageses,ShumaPaguar,DataPageses,IDAgjenti,IDKlienti,DeviceID,SyncStatus,KMON,Export_Status,PayType)" +
                " VALUES('" + PagesatID + "'," + Vlera.ToString() + ",'" + DataPageses.ToString("yyyy.MM.dd HH:mm:ss") + "'," +
                " '" + frmHome.IDAgjenti + "','" + Inkasimi_Mobile_IDKlienti + "','" + frmHome.DevID + "',0,'" + cmbKMON.Text + "',0,'" + PayType + "')";
                conn.Open();
                kom.ExecuteNonQuery();
                conn.Close();

            }
            catch (SqlCeException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                kom.Dispose();
                conn.Dispose();

            }


        }

        public float GetDebtSum(string IDKlienti)
        {
            float res = 0;


            if (IDKlienti.Trim() == "")
                return 0;

            string strKom =
            "SELECT SUM(Borxhi) FROM EvidencaPagesave WHERE IDKlienti='" + Inkasimi_Mobile_IDKlienti + "'";

            SqlCeConnection conn = new SqlCeConnection(frmHome.AppDBConnectionsStr);
            SqlCeCommand kom = new SqlCeCommand(strKom, conn);
            try
            {
                conn.Open();
                string a = kom.ExecuteScalar().ToString();
                if (a != "")
                {
                    res = float.Parse(a);

                }
            }
            finally
            {
                conn.Dispose();
                kom.Dispose();
            }

            return res;
        }

        public float GetPayedSumToDate(string IDKlienti, DateTime ToDate)
        {
            float res = 0;


            if (IDKlienti.Trim() == "")
                return 0;

            string strKom =
            "SELECT sum(ShumaPaguar) FROM EvidencaPagesave WHERE IDKlienti='" + IDKlienti + "' AND NrFatures IS NULL";
            SqlCeConnection conn = new SqlCeConnection(frmHome.AppDBConnectionsStr);
            SqlCeCommand kom = new SqlCeCommand(strKom, conn);
            try
            {
                conn.Open();
                string a = kom.ExecuteScalar().ToString();
                if (a != "")
                {
                    res = float.Parse(a);

                }
            }
            finally
            {
                conn.Dispose();
                kom.Dispose();
            }

            return res;
        }

        private bool FillGrid(string IDKlienti)
        {
            bool res = false;

            if (IDKlienti.Trim() == "")
                return false;


            string command = "SELECT d.IDDetyrimi, \n"
                           + "       d.KOD, \n"
                           + "       d.Emri, \n"
                           + "       d.KMON, \n"
                           + "       CONVERT(NVARCHAR(20), CONVERT(DECIMAL(15, 2), d.Detyrimi)) AS Detyrimi \n"
                           + "FROM   Detyrimet d \n"
                           + "WHERE d.KOD='" + IDKlienti + "'";

            PnUtils.DbUtils.FillDataSet(dsInkasimi, command);
            grdFaturatPagesat.DataSource = null;
            grdFaturatPagesat.DataSource = dsInkasimi.Tables[0];

            //StringBuilder kom = new StringBuilder(
            //" SELECT NrFatures,ShumaTotale,NrPageses,ShumaPaguar,DataPageses,Borxhi,DataPerPagese " +
            //" FROM EvidencaPagesave WHERE IDKlienti='" + strIDKlienti + "'");

            //if ((rbtnPagesat.Checked) || (rbtnFaturat.Checked))
            //{
            //    kom.Append(rbtnFaturat.Checked ? " AND NrFatures IS NOT NULL" : " AND NrFatures IS NULL");
            //}

            //string DtFieldName = "";
            //if (chkData.Checked)
            //{

            //    DtFieldName = cmbSort.SelectedItem.ToString();
            //    kom.Append(" AND " + DtFieldName + " BETWEEN '" + dtFrom.Value.ToString("yyyy-MM-dd") + "' AND '" + dtTo.Value.ToString("yyyy-MM-dd") + "'");           
            //}            

            //kom.Append(" ORDER BY " + cmbSort.SelectedItem.ToString() + " DESC");

            //try
            //{
            //    da.SelectCommand.CommandText = kom.ToString();
            //    dsInkasimi.Clear();
            //    da.Fill(dsInkasimi);
            //   // if (!HasDebt(strIDKlienti))
            //    //{
            //    //    dsInkasimi.Clear();
            //   // }
            //    grdFaturatPagesat.DataSource = null;
            //    grdFaturatPagesat.DataSource = dsInkasimi.Tables[0];
            //    lblBorxhiTotal.Text = "Borxhi total=€"+GetDebtSum(IDKlienti).ToString();

            //    res = true;
            //}
            //catch (SqlCeException ex)
            //{
            //    res = false;
            //    MessageBox.Show(ex.Message);
            //}

            return res;
        }

        SmartDevicePrintEngine sd = null;
        private void PirntoInkasimin(string strNrFatures, string strNrPageses, DateTime dtDataPageses, string strKompania, string strBusinessNo,
           string strEmriAgjentit, string strIDAgjenti, string strIDKlienti, string strLokacioni, string strKontaktEmri,
            DateTime dtPagesatArritura, string strShuma, string strInkasuar, string KMON, string payTyp)
        {
            string buffer = "";
            int PrintHeight = 55; int PrintWidth = 130;
            int x = 40; int y = 1;
            int prevX = 0;
            string titleSep = "";

            //SmartDevicePrintEngine sd = new SmartDevicePrintEngine(PnDevice.MultiplicatorPort, strPrintPort+":");
            try
            {
                string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);

                //@BE DUHET NDRYSHOHET
                BxlPrint.PrinterOpen("COM1:115200", 1000);
                BxlPrint.PrintBitmap(CurrentDir + "\\Image\\EHW-logo.png", 100, BxlPrint.BXL_ALIGNMENT_CENTER, 30);
                BxlPrint.LineFeed(1);
                if (msg == null) msg = new frmMessage(false);
                DialogResult a = msg.ShowDialog();
                if (a == DialogResult.Abort)
                {
                    return;//cancel printing
                }

                PnDevice devPort = PnDevice.BluetoothPort;

                if (frmMessage.isBlutooth == true)
                {
                    devPort = PnDevice.BluetoothPort;
                    strPrintPort = frmHome.PnPrintPort + ":";
                }
                //else
                //{
                //    devPort = PnDevice.MultiplicatorPort;
                //    strPrintPort = frmHome.PnPrintPort + ":";
                //}

                if (!frmMessage.isPreview)
                {
                    if (!PnFunctions.AvailablePort(strPrintPort))
                    {
                        MessageBox.Show("Porti: " + strPrintPort + " nuk munde te hapet");
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                sd = new SmartDevicePrintEngine(devPort, strPrintPort);

                sd.CreatePage(PrintWidth, PrintHeight);

                x = 20;

                buffer = "F L E T E  -  P A G E S E";
                //prevX = (PrintWidth / 2) - (buffer.Length / 2);
                sd.WriteToLogicalPage(x, y, buffer);
                y++;

                WriteSeparator(ref x, ref y, ref sd, "-"); //E shtuar

                //------------------TITLE SEPERATOR-------------------------
                sd.WriteToLogicalPage(prevX, y, titleSep);


                y += 1;
                buffer = "     Numri fletepageses: " + strNrPageses;
                sd.WriteToLogicalPage(x, y, buffer);
                y += 1;

                buffer = "     Nr.: " + strNrFatures;
                sd.WriteToLogicalPage(x, y, buffer);

                y += 1;
                buffer = "     Data: " + dtDataPageses.ToString("dd/MM/yyyy HH:mm:ss");
                sd.WriteToLogicalPage(x, y, buffer);
                //-------------------------------------------------------------
                WriteSeparator(ref x, ref y, ref sd, "-");

                y += 1;
                buffer = "     Kompania: " + strKompania;
                sd.WriteToLogicalPage(x, y, buffer);

                y += 1;
                buffer = "     Nr.Biznesit: " + strBusinessNo;
                sd.WriteToLogicalPage(x, y, buffer);

                y += 1;
                buffer = "     Emri agjentit: " + strEmriAgjentit;
                sd.WriteToLogicalPage(x, y, buffer);


                y += 1;
                buffer = "     IDAgjenti: " + strIDAgjenti;
                sd.WriteToLogicalPage(x, y, buffer);

                //-------------------------------------------------------------
                WriteSeparator(ref x, ref y, ref sd, "-");

                y += 1;
                buffer = "     Klienti: " + strIDKlienti;
                sd.WriteToLogicalPage(x, y, buffer);

                y += 1;
                buffer = "     Lokacioni: " + strLokacioni;
                sd.WriteToLogicalPage(x, y, buffer);

                y += 1;
                buffer = "     Kontakt Emri: " + strKontaktEmri;
                sd.WriteToLogicalPage(x, y, buffer);

                //-------------------------------------------------------------
                WriteSeparator(ref x, ref y, ref sd, "-");

                y += 1;
                buffer = "     Data e pageses:" + dtPagesatArritura.ToString("dd/MM/yyyy HH:mm:ss");
                sd.WriteToLogicalPage(x, y, buffer);

                y += 1;
                buffer = "     Shuma e fatures: " + Math.Round(decimal.Parse(strShuma), 2) + " " + KMON;
                sd.WriteToLogicalPage(x, y, buffer);

                y += 1;
                buffer = "     Pagesa e inkasuar: " + Math.Round(decimal.Parse(strInkasuar), 2) + " " + KMON + " " + cmbPayType.Text;
                sd.WriteToLogicalPage(x, y, buffer);

                //-------------------------------------------------------------
                WriteSeparator(ref x, ref y, ref sd, "-");

                x = 50;
                y += 3;
                buffer = "     Per Kompanine: ______________________"; //18
                sd.WriteToLogicalPage(x, y, buffer);

                y += 1;
                buffer = "     " + strEmriAgjentit;
                sd.WriteToLogicalPage(x, y, buffer);

                //prevX = buffer.Length + x + 50;

                y += 2;
                buffer = "     Per Klientin: ______________________";
                prevX = sd.PageWidth - buffer.Length;
                sd.WriteToLogicalPage(prevX, y, buffer);

                y += 3;

                WriteSeparator(ref x, ref y, ref sd, "="); y = y + 1;

                buffer = "     Fletepagesa eshte e hartuar ne tre kopje";
                sd.WriteToLogicalPage(x, y, buffer);

                y += 1;
                buffer = "     Printuar nga Sistemi MobSell Pronet - Kosove +381 38 557799";
                sd.WriteToLogicalPage(x, y, buffer);

                y += 5;


                //if (c==1) goto a;

                //sd.AddToPreview();
                // sd.Preview();
                sd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (sd != null) sd.Dispose();
                if (msg != null) msg.Dispose();
                msg = null;
                Cursor.Current = Cursors.Default;

            }

        }
        private void WriteSeparator(ref int x, ref int y, ref SmartDevicePrintEngine PrintEngine, string CharSeperator)
        {
            int prevX = x;
            string Seperator = "";
            x = 1;//reset x-coordinate            
            y++;
            //for (int i = 0; i < PrintEngine.PageWidth; i++) Seperator += CharSeperator;
            for (int i = 0; i < 69; i++) Seperator += CharSeperator;
            PrintEngine.WriteToLogicalPage(x, y, Seperator);
            x = prevX;
        }

        #endregion

        private void frmInkasimiMobil_Load(object sender, EventArgs e)
        {
            //cmbSort.SelectedIndex = 0;
            Init();
            if (Inkasimi_Mobile_IDKlienti != "") cmbKlienti.SelectedValue = Inkasimi_Mobile_IDKlienti;
            cmbKMON.SelectedIndex = 0;
            cmbPayType.SelectedIndex = 0;
            Cursor.Current = Cursors.Default;
            grdFaturatPagesat.TabIndex = 1;
            if (!frmHome.AprovimFaturash)
            {
                btnPrinto.Visible = false;
            }
            dtDataPageses.Enabled = false;


        }

        private void btnShfaq_Click(object sender, EventArgs e)
        {
            FillGrid(cmbKlienti.SelectedValue.ToString());
        }

        private void cmbKlienti_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbKlienti.Enabled)
            {
                if (cmbKlienti.SelectedItem != null)
                    Inkasimi_Mobile_IDKlienti = cmbKlienti.SelectedValue.ToString();
                FillGrid(Inkasimi_Mobile_IDKlienti);
                btnInkaso.Enabled = false;
                txtShumaPaguar.Enabled = false;
                cmbKMON.Enabled = false;
            }
        }

        private void frmInkasimiMobil_Closing(object sender, CancelEventArgs e)
        {
            cmbKlienti.Enabled = true;
        }

        private void menuDalja_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnInkaso_Click(object sender, EventArgs e)
        {
            if (cmbKlienti.SelectedItem != null)
            {
                bool isnum = Information.IsNumeric(txtShumaPaguar.Text);
                if (isnum == false)		//not num
                {
                    MessageBox.Show("Gabim në vlerë!");
                    string temp = txtShumaPaguar.Text.Trim();
                    txtShumaPaguar.Focus();
                    return;
                }
                float tot = float.Parse(txtShumaPaguar.Text);
                if (tot > 0)
                {
                    decimal _inkaso = ((Math.Round(decimal.Parse(txtShumaPaguar.Text), 2)));
                    //Inkaso(float.Parse(txtShumaPaguar.Text), dtDataPageses.Value);
                    Inkaso(float.Parse(_inkaso.ToString()), dtDataPageses.Value, cmbPayType.Text);
                    //rbtnPagesat.Checked = true;
                    //chkData.Checked = false;
                    FillGrid(Inkasimi_Mobile_IDKlienti);

                    if (MessageBox.Show(" Inkasimi përfundoi me sukses \n Dëshironi të shtypni fletëpagesën ?", "Shtypja", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                  MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        btnPrinto_Click(null, null);
                    }
                    else
                    {

                    }

                    //Release controls
                    txtShumaPaguar.Text = "";
                    btnInkaso.Enabled = false;
                    cmbKMON.Enabled = false;
                    txtShumaPaguar.Enabled = false;
                    grdFaturatPagesat.UnSelect(0);
                    cmbPayType.Enabled = false;
                    cmbPayType.SelectedIndex = 0;
                    cmbKMON.SelectedIndex = 0;


                }
                else
                {
                    MessageBox.Show("Pagesa duhet të ketë \n shumën më të madhe se 0(zero)");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Duhet te zgjedhni nje klient");
            }


        }

        private void tbtnShfaq_Click(object sender, EventArgs e)
        {
            if (cmbKlienti.SelectedItem != null)
            {
                Inkasimi_Mobile_IDKlienti = cmbKlienti.SelectedValue.ToString();
                FillGrid(Inkasimi_Mobile_IDKlienti);
            }
            else
            {
                MessageBox.Show("Duhet te zgjedhni nje klient");
            }

        }

        private void chkData_CheckStateChanged(object sender, EventArgs e)
        {
            //dtFrom.Enabled = chkData.Checked;
            //dtTo.Enabled = chkData.Checked;
        }

        frmMessage msg = null;
        private void btnPrinto_Click(object sender, EventArgs e)
        {

            string IDKlientiZgjedhur = cmbKlienti.SelectedValue.ToString();

            DataTable dtTemp = new DataTable();
            int SelectedRow = grdFaturatPagesat.CurrentCell.RowNumber;
            string NrFatures = grdFaturatPagesat[SelectedRow, 2].ToString();
            string Shuma = GetPayedSumToDate(IDKlientiZgjedhur, DateTime.Now).ToString();
            string Inkasuar = grdFaturatPagesat[SelectedRow, 3].ToString();
            string NrPageses = "";

            string EmriAgjentit = "Filan Fisteki";
            string Lokacioni = "Prishtine";
            string KontaktEmri = "Filan Kontakti";
            string KlientiEmri = cmbKlienti.Text;
            string PayType = cmbPayType.Text;


            da.SelectCommand.CommandText = "select * from Agjendet where IDAgjenti='" + frmHome.IDAgjenti + "'";
            da.Fill(dtTemp);
            if (dtTemp.Rows.Count > 0)
                EmriAgjentit = dtTemp.Rows[0]["Emri"].ToString() + " " + dtTemp.Rows[0]["Mbiemri"].ToString();


            da.SelectCommand.CommandText = "Select * from KlientDheLokacion where IDKlientDheLokacion='" + IDKlientiZgjedhur + "'";
            dtTemp.Clear();
            da.Fill(dtTemp);
            if (dtTemp.Rows.Count > 0)
                Lokacioni = dtTemp.Rows[0]["EmriLokacionit"].ToString();
            KontaktEmri = dtTemp.Rows[0]["KontaktEmriMbiemri"].ToString();

            dtTemp.Clear();
            da.SelectCommand.CommandText = "SELECT TOP(1) CASE  \n"
                                           + "                   WHEN ep.NrFatures IS NOT NULL THEN ep.NrFatures \n"
                                           + "                   ELSE '' \n"
                                           + "              END AS NrFatures, \n"
                                           + "       ep.NrPageses \n"
                                           + "FROM   EvidencaPagesave ep \n"
                                           + "ORDER BY \n"
                                           + "       ep.DataPageses DESC";

            da.Fill(dtTemp);
            if (dtTemp.Rows.Count > 0)
            {
                NrPageses = dtTemp.Rows[0]["NrPageses"].ToString();
                NrFatures = dtTemp.Rows[0]["NrFatures"].ToString();
            }

            da.SelectCommand.CommandText = "SELECT * FROM Klientet where IDKlienti='" + IDKlientiZgjedhur + "'";
            dtTemp.Clear();
            da.Fill(dtTemp);
            if (dtTemp.Rows.Count > 0)
            {
                KlientiEmri = dtTemp.Rows[0]["Emri"].ToString();

            }
            else
            {
                KlientiEmri = "";
            }


            DataSet temp = new DataSet();
            string companyName = "";
            string Nipt = "";
            try
            {
                string sql = "SELECT ci.[Value] as Value FROM CompanyInfo ci WHERE ci.Item='Shitesi' OR ci.Item='Nipt'";

                PnUtils.DbUtils.FillDataSet(temp, sql);
                Nipt = temp.Tables[0].Rows[0]["Value"].ToString();
                companyName = temp.Tables[0].Rows[1]["Value"].ToString();
            }
            catch
            {
                MessageBox.Show("Nuk mund te merren informatata rreth kompnis EHW!");
            }
            finally
            {
                temp.Dispose();
            }
            string kmon = cmbKMON.Text;

            PirntoInkasimin
                (NrFatures, NrPageses, DateTime.Now, companyName, Nipt, EmriAgjentit, frmHome.IDAgjenti,
                KlientiEmri, Lokacioni, KontaktEmri, DateTime.Now, Shuma, Shuma, kmon, PayType);
            Cursor.Current = Cursors.Default;
        }

        private void grdFaturatPagesat_MouseUp(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.DataGrid.HitTestInfo myHitTest = this.grdFaturatPagesat.HitTest(e.X, e.Y);
            if (myHitTest.Type == DataGrid.HitTestType.Cell)
            {
                this.grdFaturatPagesat.Select(myHitTest.Row);
                btnInkaso.Enabled = true;


            }
        }

        private void grdFaturatPagesat_MouseUp_1(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.DataGrid.HitTestInfo myHitTest = this.grdFaturatPagesat.HitTest(e.X, e.Y);
            if (myHitTest.Type == DataGrid.HitTestType.Cell)
            {
                this.grdFaturatPagesat.Select(myHitTest.Row);
                btnInkaso.Enabled = true;
                txtShumaPaguar.Enabled = true;
                cmbKMON.Enabled = true;
                cmbPayType.Enabled = true;

            }
        }

        private void txtShumaPaguar_TextChanged(object sender, EventArgs e)
        {
            if (txtShumaPaguar.Text.Contains('.') || txtShumaPaguar.Text.Contains(','))
            {
                txtShumaPaguar.Text.Replace(',', '.'); // Ensure about RegionalSettings 

            }

        }


        frmAllInkasimet allInkasimet = null;
        private void menuRaporti_Click(object sender, EventArgs e)
        {
            if (allInkasimet == null) allInkasimet = new frmAllInkasimet();
            allInkasimet.ShowDialog();
        }

        frmInkasimet inkasimet = null;
        private void menuInkasimet_Click(object sender, EventArgs e)
        {
            if (inkasimet == null) inkasimet = new frmInkasimet();
            inkasimet.ShowDialog();
        }

        frmListDetyrime listDetyrimet = null;
        private void menuListaDetyrimeve_Click(object sender, EventArgs e)
        {
            if (listDetyrimet == null) listDetyrimet = new frmListDetyrime();
            listDetyrimet.ShowDialog();
        }



    }
}