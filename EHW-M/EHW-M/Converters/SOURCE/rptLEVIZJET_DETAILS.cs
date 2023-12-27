using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PnReports;
using System.IO;
using System.Reflection;
using PnUtils;
using MobileSales.BL;

namespace MobileSales
{
    public partial class rptLEVIZJET_DETAILS : Form
    {
        private int SelectedRow;
        private bool Key, isSelected;
        private string _sqlCeQuery, _Numri_Levizjes;
        frmMessage msg;
        private string _reasonOfMovement;

        public rptLEVIZJET_DETAILS()
        {
            InitializeComponent();
        }

        public rptLEVIZJET_DETAILS(string nrLevizjes)
        {
            InitializeComponent();
            _Numri_Levizjes = nrLevizjes;
        }

        private void menuDalja_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rptLEVIZJET_DETAILS_Load(object sender, EventArgs e)
        {
            Fill_Levizjet();
        }

        private void Fill_Levizjet()
        {
            DataTable dt = new DataTable();
            PnUtils.DbUtils.FillDataTable(dt, "Select * from LEVIZJET_DETAILS where Numri_Levizjes='" + _Numri_Levizjes + "'");
            grdDetalet.DataSource = dt;
        }

        private void grdDetalet_KeyDown(object sender, KeyEventArgs e)
        {
            Key = (e.KeyData == Keys.Up || e.KeyData == Keys.Down || e.KeyData == Keys.Left || e.KeyData == Keys.Right);
            if (isSelected == true && Key)
            {
                this.grdDetalet.Select(this.grdDetalet.CurrentRowIndex);
                SelectedRow = this.grdDetalet.CurrentRowIndex;
            }
        }

        private void grdDetalet_MouseUp(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.DataGrid.HitTestInfo myHitTest = this.grdDetalet.HitTest(e.X, e.Y);
            if (myHitTest.Type == DataGrid.HitTestType.Cell)
            {
                isSelected = true;
                this.grdDetalet.Select(myHitTest.Row);
                SelectedRow = myHitTest.Row;
            }
        }

        private void menuPrinto_Click(object sender, EventArgs e)
        {
            if (grdDetalet.VisibleRowCount != 0)
            {
                Print_New(_Numri_Levizjes);
            }
        }

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

            try
            {
                #region PrintParams
                //@BE QR Code
                PnUtils.DbUtils.FillDataTable(TCRQRCodeTbl, @"
                                                            Select Numri_Levizjes, Totali, TCRQRCodeLink from Levizjet_Header
                                                            where Numri_Levizjes = '" + nrLevizjes + @"'
                                                            and TCRSyncStatus >= 1
                                                        ");
                if (TCRQRCodeTbl.Rows.Count > 0)
                {
                    if (msg == null) msg = new frmMessage(true, new string[] { TCRQRCodeTbl.Rows[0]["CmimiTotal"].ToString(),
                                                        TCRQRCodeTbl.Rows[0]["NrLiferimit"].ToString(), 
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

                header = new DataTable();
                _sqlCeQuery = @"SELECT 'Data e levizjes ' + CONVERT(NCHAR(8), lh.[Data], 3) AS Kolona1,
                                       'Dalje nga ' + lh.Levizje_Nga AS Kolona2
                                FROM   LEVIZJET_HEADER lh
                                WHERE  lh.Numri_Levizjes = '" + nrLevizjes + @"'
                                UNION ALL
                                SELECT 'Numri i levizjes ' + lh.Numri_Levizjes AS Kolona1,
                                       'Dalje ne '+lh.Levizje_Ne AS Kolona2
                                FROM   LEVIZJET_HEADER lh
                                WHERE  lh.Numri_Levizjes = '" + nrLevizjes + "'";
                DbUtils.FillDataTable(header, _sqlCeQuery);
                details = new DataTable();
                _sqlCeQuery = @"Select
                                Numri_Levizjes,
                                IDArtikulli,
                                CONVERT (Decimal(10,3),Round(Sasia,3)) as Sasia,
                                CONVERT (Decimal(10,3),Round(Cmimi,3)) as Cmimi,
                                Njesia_matese,
                                CONVERT (Decimal(10,3),Round(Totali,2)) as Totali,
                                REPLACE(REPLACE(REPLACE(REPLACE(Artikulli,'ç', 'c'), 'Ç', 'C'), 'ë', 'e'),'Ë','E')  AS Artikulli 
                                from LEVIZJET_DETAILS where Numri_Levizjes='" + nrLevizjes + "'";
                DbUtils.FillDataTable(details, _sqlCeQuery);

                if (details.Rows.Count > 0 && header.Rows.Count > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Rpt = new PnPrint(devPort, strPortName, CurrentDir + "\\rptLevizja.pnrep", details, header, header);
                    Rpt.PrintPnReport(PT, true, " tre ", true);
                }
                else
                {
                    MessageBox.Show("Gabim gjatë gjenerimit te raportit \n Printimin nuk munde të vazhdoj");
                    return;
                }
            }
            catch (FileNotFoundException fex)
            {
                MessageBox.Show("Mungon Fajlli: \n Porosia.pnrep");
                PnUtils.DbUtils.WriteExeptionErrorLog(fex);

            }
            catch (TypeLoadException)
            {
            }
            catch (MissingMethodException)
            {

            }
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

        private void btnShto_Click(object sender, EventArgs e)
        {
            new frmLevizjet().ShowDialog();
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            new frmLevizjet().ShowDialog();
            Fill_Levizjet();
        }

        private void Print_New(string nrLevizjes)
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

            try
            {
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


                DataTable tempDt = new DataTable();

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
                tempDt.Clear();
                //DbUtils.FillDataTable(tempDt, "SELECT * FROM LEVIZJET_HEADER lh WHERE lh.Numri_Levizjes='" + _Numri_Levizjes + "' AND lh.Levizje_Nga=lh.Depo");
                DbUtils.FillDataTable(tempDt, "SELECT * FROM LEVIZJET_HEADER lh WHERE lh.Numri_Levizjes='" + nrLevizjes + "' AND lh.Levizje_Nga=lh.Depo");

                if (tempDt.Rows.Count == 1) //Dalje
                {
                    _adresa_SHITESI = DbUtils.ExecSqlScalar("SELECT d.TAGNR FROM Depot d WHERE d.Depo='" + tempDt.Rows[0]["Levizje_Nga"].ToString() + "'");
                    _nipt_BLERESIT = DbUtils.ExecSqlScalar("SELECT d.NIPT FROM Depot d WHERE d.Depo='" + tempDt.Rows[0]["Levizje_Ne"].ToString() + "'");
                    _sn_BLERESIT = DbUtils.ExecSqlScalar("SELECT d.SN FROM Depot d WHERE d.Depo='" + tempDt.Rows[0]["Levizje_Ne"].ToString() + "'");
                    if (tempDt.Rows[0]["Levizje_Ne"].ToString().StartsWith("M"))
                    {
                        _adresa_BLERESIT = DbUtils.ExecSqlScalar("SELECT d.TAGNR FROM Depot d WHERE d.Depo='" + tempDt.Rows[0]["Levizje_Ne"].ToString() + "'");
                        _magazina_PRITESIT = tempDt.Rows[0]["Levizje_Ne"].ToString();
                        _Drejteues = DbUtils.ExecSqlScalar("SELECT a.Emri+' '+a.Mbiemri FROM Agjendet a WHERE a.IDAgjenti='" + tempDt.Rows[0]["Levizje_Ne"].ToString() + "'");
                        _targ_mjetit = DbUtils.ExecSqlScalar("SELECT d.TAGNR FROM Depot d WHERE d.Depo='" + tempDt.Rows[0]["Levizje_Ne"].ToString() + "'");
                        _NISESI = frmHome.EmriAgjendit + " " + frmHome.MbiemriAgjendit;
                        _PRITESI = _Drejteues;
                    }
                    else
                    {
                        _adresa_BLERESIT = DbUtils.ExecSqlScalar("SELECT d.Adresa FROM Depot d WHERE d.Depo='" + tempDt.Rows[0]["Levizje_Ne"].ToString() + "'");
                        _magazina_PRITESIT = tempDt.Rows[0]["Levizje_Ne"].ToString();
                        _Drejteues = frmHome.EmriAgjendit + " " + frmHome.MbiemriAgjendit;
                        _targ_mjetit = DbUtils.ExecSqlScalar("SELECT d.TAGNR FROM Depot d WHERE d.Depo='" + frmHome.Depo + "'");
                        _PRITESI = DbUtils.ExecSqlScalar("SELECT d.Adresa FROM Depot d WHERE d.Depo='" + tempDt.Rows[0]["Levizje_Ne"].ToString() + "'");
                        _NISESI = _Drejteues;
                    }
                    if (tempDt.Rows[0]["Levizje_Ne"].ToString().Equals("PG1"))
                    {
                        numri = "Fisk-" + tempDt.Rows[0]["NumriFisk"].ToString() + "-" + frmHome.Depo;
                        _numriFatures = nrLevizjes;
                    }
                    else
                    {
                        numri = _numriFatures = nrLevizjes;
                    }
                    _numriHyres = "";
                }
                tempDt.Clear();
                //DbUtils.FillDataTable(tempDt, "SELECT * FROM LEVIZJET_HEADER lh WHERE lh.Numri_Levizjes='" + _Numri_Levizjes + "' AND lh.Levizje_Ne=lh.Depo");
                DbUtils.FillDataTable(tempDt, "SELECT * FROM LEVIZJET_HEADER lh WHERE lh.Numri_Levizjes='" + nrLevizjes + "' AND lh.Levizje_Ne=lh.Depo");
                if (tempDt.Rows.Count == 1)//Hyrje
                {
                    _adresa_BLERESIT = DbUtils.ExecSqlScalar("SELECT d.TAGNR FROM Depot d WHERE d.Depo='" + tempDt.Rows[0]["Levizje_Ne"].ToString() + "'"); ;
                    _magazina_PRITESIT = tempDt.Rows[0]["Levizje_Ne"].ToString();
                    if (tempDt.Rows[0]["Levizje_Nga"].ToString().Contains("D"))
                    {
                        _adresa_SHITESI = DbUtils.ExecSqlScalar("SELECT d.Adresa FROM Depot d WHERE d.Depo='" + tempDt.Rows[0]["Levizje_Nga"].ToString() + "'");
                    }
                    else
                    {
                        _adresa_SHITESI = DbUtils.ExecSqlScalar("SELECT d.TAGNR FROM Depot d WHERE d.Depo='" + tempDt.Rows[0]["Levizje_Nga"].ToString() + "'");
                    }
                    _nipt_BLERESIT = DbUtils.ExecSqlScalar("SELECT d.NIPT FROM Depot d WHERE d.Depo='" + frmHome.Depo + "'");
                    _sn_BLERESIT = DbUtils.ExecSqlScalar("SELECT d.SN FROM Depot d WHERE d.Depo='" + frmHome.Depo + "'");
                    _targ_mjetit = DbUtils.ExecSqlScalar("SELECT d.TAGNR FROM Depot d WHERE d.Depo='" + frmHome.Depo + "'");
                    if (tempDt.Rows[0]["Levizje_Nga"].ToString().Contains("M"))
                    {
                        _Drejteues = DbUtils.ExecSqlScalar("SELECT a.Emri+' '+a.Mbiemri FROM Agjendet a WHERE a.IDAgjenti='" + tempDt.Rows[0]["Levizje_Nga"].ToString() + "'");
                        _targ_mjetit = DbUtils.ExecSqlScalar("SELECT d.TAGNR FROM Depot d WHERE d.Depo='" + tempDt.Rows[0]["Levizje_Nga"].ToString() + "'");
                        _NISESI = _Drejteues;
                        _PRITESI = frmHome.EmriAgjendit + " " + frmHome.MbiemriAgjendit;
                        numri = _numriFatures = nrLevizjes;
                    }
                    else
                    {
                        _targ_mjetit = DbUtils.ExecSqlScalar("SELECT d.TAGNR FROM Depot d WHERE d.Depo='" + frmHome.Depo + "'");
                        _Drejteues = _PRITESI = frmHome.EmriAgjendit + " " + frmHome.MbiemriAgjendit;
                        _NISESI = DbUtils.ExecSqlScalar("SELECT d.Adresa FROM Depot d WHERE d.Depo='" + tempDt.Rows[0]["Levizje_Nga"].ToString() + "'");
                        _numriFatures = nrLevizjes;
                        numri = tempDt.Rows[0]["Numri_Daljes"].ToString();
                    }
                    _numriHyres = "        " + tempDt.Rows[0]["Levizje_Nga"].ToString(); // +" [" + tempDt.Rows[0]["Numri_Daljes"].ToString() + "]";
                }
                if (frmHome.Depo == "AT")
                {
                    _reasonOfMovement = "Shitje me pakice";
                }

                else
                {
                    _reasonOfMovement = "Shitje me shumice";
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
            {
            }
            catch (MissingMethodException)
            {

            }
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

    }
}