using System.Data.SqlServerCe;
using PnUtils;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using PnReports;
using System.Reflection;

namespace MobileSales
{
    public partial class frmPorosia_Detalet : Form
    {
        private bool isSelected = false, Key;
        private string _IDKlienti, _IDPorosia, _IDArtikulli, BUM;//, _IDPorosiaTCR;
        private int SelectedRow;
        private OrderType _Type;
        public string _artName, _sasi, _idartikulli;
        string _nrPorosise = "";
        public float Porosia_value;
        private frmMessage msg = null;

        #region Initialize Instance

        frmArtikujtStoqet fArt = null;

        #endregion

        public frmPorosia_Detalet()
        {
            InitializeComponent();
        }

        public frmPorosia_Detalet(string IDPorosia, OrderType ORDERTYPE, string IDKlienti)
        {
            InitializeComponent();
            _IDPorosia = IDPorosia;
            //if (_IDPorosia.Length > frmHome.DevID.Length)
            //{
            //    _IDPorosiaTCR = _IDPorosia.Substring(_IDPorosia.IndexOf(frmHome.DevID) + frmHome.DevID.Length).Replace("-", "");
            //}
            _Type = ORDERTYPE;
            switch (ORDERTYPE)
            {
                case OrderType.New:
                    cmbKlientet.Enabled = true;
                    _IDKlienti = IDKlienti;
                    break;
                case OrderType.Edit:
                    cmbKlientet.Enabled = false;
                    _IDKlienti = IDKlienti;
                    break;
                default:
                    break;
            }

        }

        private void frmPorosia_Detalet_Load(object sender, EventArgs e)
        {
            this.order_DetailsTableAdapter.Connection.ConnectionString = frmHome.AppDBConnectionsStr;
            if (_Type == OrderType.New)
            {
                if (New_Order())
                {
                    this.order_DetailsTableAdapter.FillFor_Orders(this.myMobileDataSet.Order_Details, _IDPorosia);
                }
                else
                {
                    MessageBox.Show("Test");
                    return;
                }
            }
            else
            {
                this.order_DetailsTableAdapter.FillFor_Orders(this.myMobileDataSet.Order_Details, _IDPorosia);
                _nrPorosise = _IDPorosia;
            }
            FillKientet();
            //_IDPorosiaTCR = _IDPorosia.Substring(_IDPorosia.IndexOf(frmHome.DevID) + frmHome.DevID.Length).Replace("-", "");
            lblNrPorosise.Text = _IDPorosia;
            Cursor.Current = Cursors.Default;
        }

        private void menuDalja_Click(object sender, EventArgs e)
        {
            #region Commented Code
            //if (MessageBox.Show("Jeni të sigurtë për anulimi i porosisë \n Të gjitha shënimet në lidhje me këtë porosi do të fshihen  ", "Verejtje", MessageBoxButtons.YesNo, MessageBoxIcon.Hand,
            //      MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            //{
            //    if (Perform_Delete())
            //    {
            //        MessageBox.Show("Anulimi përfundoi me sukses");
            //    }
            //    else
            //    {
            //        MessageBox.Show("Anulimi deshtoi");
            //    }
            //    fArt = null;
            //    this.Close();
            //}

            #endregion

            if (orderDetailsBindingSource.Count >= 1 && !btnShto.Enabled)
            {
                order_DetailsTableAdapter.Update(this.myMobileDataSet.Order_Details);
                this.Close();
            }
            else
            {
                if (MessageBox.Show("Porosia e filluar nuk do të regjistrohet \n Jeni të sigurt për mbylljen e formës?",
                    "Mbyllja", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {

                    if (_Type == OrderType.New)
                    {
                        if (!DeleteOrder(true))
                        {
                            MessageBox.Show("Anulimi dështoi");
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        if (orderDetailsBindingSource.Count == 0)
                        {
                            if (!DeleteOrder(false))
                            {
                                MessageBox.Show("Anulimi dështoi");
                            }
                            else
                            {
                                this.Close();
                            }
                            //MessageBox.Show("Nuk lejohet anulimi i komplet porosisë");
                            //return;
                        }
                    }

                }
                else
                {
                    return;
                }

            }
        }

        private void grdPorosia_MouseUp(object sender, MouseEventArgs e)
        {

            if (order_DetailsDataGrid.CurrentRowIndex >= 0)
            {
                order_DetailsDataGrid.Select(order_DetailsDataGrid.CurrentRowIndex);
                _IDArtikulli = dsPorosia.Tables[0].Rows[order_DetailsDataGrid.CurrentRowIndex]["IDArtikulli"].ToString();

            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            isSelected = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                orderDetailsBindingSource.RemoveCurrent();
                FillGid(_IDPorosia);
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                double.Parse(txtSasia.Text);

            }
            catch (FormatException)
            {
                MessageBox.Show("Gabim ne sasi");
                txtSasia.Text = "";
                txtSasia.Focus();
                return;
            }
            try
            {
                DataRow por = ((DataRowView)(this.orderDetailsBindingSource.Current)).Row;
                //int nr = int.Parse(por["NrRendor"].ToString());
                int nr = orderDetailsBindingSource.Count;
                string idart = por["IDArtikulli"].ToString();
                string emri = por["Emri"].ToString();
                //float sasia = float.Parse(txtSasia.Text);
                float sasia = System.Single.Parse(Math.Round(double.Parse(txtSasia.Text), 2).ToString());
                this.order_DetailsTableAdapter.InsertQuery(_IDPorosia, nr, idart, txtArtikulli.Text, sasia, 0, 0);
                this.order_DetailsTableAdapter.Update(this.myMobileDataSet.Order_Details);
                FillGid(_IDPorosia);
                PerformCount();
                order_DetailsDataGrid.Enabled = true;

                btnShto.Enabled = false;
            }
            catch (SqlCeException sce)
            {
                if (sce.NativeError == 25016)
                {
                    MessageBox.Show("Gabim:\nRegjistrim i dyfishtë i artikullit: " + txtArtikulli.Text);
                    orderDetailsBindingSource.RemoveCurrent();
                    orderDetailsBindingSource.EndEdit();
                    order_DetailsDataGrid.Enabled = true;
                    btnShto.Enabled = false;

                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            order_DetailsDataGrid.Enabled = false;
            if (isSelected)
            {
                try
                {
                    order_DetailsDataGrid.UnSelect(SelectedRow);
                }
                catch (Exception ex)
                {
                    DbUtils.WriteExeptionErrorLog(ex);
                }
            }
            if (btnShto.Enabled)
            {
                orderDetailsBindingSource.RemoveCurrent();
                orderDetailsBindingSource.EndEdit();
            }
            orderDetailsBindingSource.AddNew();
            isSelected = true;
            if (fArt == null) fArt = new frmArtikujtStoqet("Porosi", false);
            fArt.PorosiaCrossForm = this;
            Cursor.Current = Cursors.WaitCursor;
            fArt.Show();
            fArt.txtArtFilter.Focus();
            Cursor.Current = Cursors.Default;
        }

        private void menuAnulo_Click(object sender, EventArgs e)
        {
            if (isSelected && order_DetailsDataGrid.VisibleRowCount != 0)
            {

                if (MessageBox.Show("Jeni të sigurtë për fshirjen e artikulli: " + txtArtikulli.Text, "Vërejtje", MessageBoxButtons.YesNo, MessageBoxIcon.Hand,
                     MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {

                    try
                    {
                        orderDetailsBindingSource.RemoveAt(SelectedRow);
                        order_DetailsTableAdapter.Update(this.myMobileDataSet.Order_Details);
                        FillGid(_IDPorosia);
                        PerformCount();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }

            }

        }

        private void menuRegjistro_Click(object sender, EventArgs e)
        {
            if (btnShto.Enabled)
            {
                orderDetailsBindingSource.RemoveCurrent();
                btnShto.Enabled = false;
                order_DetailsDataGrid.Enabled = true;
            }
            if (orderDetailsBindingSource.Count >= 1 && !btnShto.Enabled)
            {
                if (MessageBox.Show("Jeni të sigurtë për regjistrimin e Porosise?", "Vërejtje", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    try
                    {
                        orderDetailsBindingSource.EndEdit();
                        this.order_DetailsTableAdapter.Update(this.myMobileDataSet.Order_Details);
                        if (cmbKlientet.SelectedIndex > 0)
                        {
                            string _UPDATE = "Update [Orders] set IDKlientDheLokacion='" + cmbKlientet.SelectedValue + "', SyncStatus = 0 where [Orders].IDOrder ='" + _IDPorosia + "'";
                            DbUtils.ExecSql(_UPDATE);

                            _UPDATE = "Update [Order_Details] set SyncStatus = 0 where [Order_Details].IDOrder ='" + _IDPorosia + "'";
                            DbUtils.ExecSql(_UPDATE);

                        }
                        else
                        {
                            string _UPDATE = "Update [Orders] set IDKlientDheLokacion='" + frmHome.Depo + "', SyncStatus = 0 where [Orders].IDOrder ='" + _IDPorosia + "'";
                            DbUtils.ExecSql(_UPDATE);

                            _UPDATE = "Update [Order_Details] set SyncStatus = 0 where [Order_Details].IDOrder ='" + _IDPorosia + "'";
                            DbUtils.ExecSql(_UPDATE);
                        }
                        //BL.SyncBL.SyncTablesWithGPS();
                        this.Close();

                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }
            }
            else
            {
                MessageBox.Show("Nuk keni artikull për porosi");
            }
        }


        private void menuPrint_Click(object sender, EventArgs e)
        {
            if (myMobileDataSet.Order_Details.Rows.Count != 0)
            {
                if (MessageBox.Show("Jeni të sigurte për printimin e porosisë", "Printimi", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    Print(_IDPorosia);
                }
            }
            else
            {
                MessageBox.Show("Nuk keni artikull për printim");
            }

        }

        private void order_DetailsDataGrid_MouseUp(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.DataGrid.HitTestInfo myHitTest = this.order_DetailsDataGrid.HitTest(e.X, e.Y);
            if (myHitTest.Type == DataGrid.HitTestType.Cell)
            {
                isSelected = true;
                this.order_DetailsDataGrid.Select(myHitTest.Row);
                SelectedRow = myHitTest.Row;
                try
                {
                    Porosia_value = System.Single.Parse(Math.Round(double.Parse(order_DetailsDataGrid[SelectedRow, 2].ToString()), 2).ToString());
                }
                catch (Exception)
                {


                }

            }
        }

        private void order_DetailsDataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            Key = (e.KeyData == Keys.Up || e.KeyData == Keys.Down || e.KeyData == Keys.Left || e.KeyData == Keys.Right);
            if (isSelected == true && Key)
            {
                this.order_DetailsDataGrid.Select(this.order_DetailsDataGrid.CurrentRowIndex);
                SelectedRow = this.order_DetailsDataGrid.CurrentRowIndex;
                try
                {
                    Porosia_value = System.Single.Parse(Math.Round(double.Parse(order_DetailsDataGrid[SelectedRow, 2].ToString()), 2).ToString());
                }
                catch (Exception)
                {

                }
            }
        }

        private void txtSasia_TextChanged(object sender, EventArgs e)
        {
            try
            {
                System.Single.Parse(Math.Round(double.Parse(txtSasia.Text), 2).ToString());;
                txtSasia.Text = txtSasia.Text.Trim();
            }
            catch (Exception)
            {
                txtSasia.Text = "";
            }
        }

        private void txtSasia_KeyDown(object sender, KeyEventArgs e)
        {
            if (btnShto.Enabled && e.KeyCode == Keys.Enter)
            {
                this.btnSave_Click(null, null);

            }
            #region Keys.UP && Keys.Down

            if (e.KeyCode == Keys.Up)
            {

                Porosia_value = Porosia_value + 1;
                txtSasia.Text = Porosia_value.ToString();
            }

            if (e.KeyCode == Keys.Down)
            {
                if (Porosia_value > 1)
                {
                    Porosia_value = Porosia_value - 1;

                }

                txtSasia.Text = Porosia_value.ToString();
            }


            #endregion
        }

        #region Methods

        private void FillKientet()
        {
            string _fillQuery = "Select kl.IDKlientDheLokacion,kl.KontaktEmriMbiemri from KlientDheLokacion kl";
            DbUtils.FillDataSet(dsKlientet, _fillQuery);
            DataRow myRow = dsKlientet.Tables[0].NewRow();
            myRow[dsKlientet.Tables[0].Columns[0]] = "00";
            myRow[dsKlientet.Tables[0].Columns[1]] = "Zgjedh Klientin";
            dsKlientet.Tables[0].Rows.InsertAt(myRow, 0);

            cmbKlientet.DataSource = dsKlientet.Tables[0];
            cmbKlientet.DisplayMember = "KontaktEmriMbiemri";
            cmbKlientet.ValueMember = "IDKlientDheLokacion";
            cmbKlientet.Enabled = true;
            if (_Type == OrderType.New)
            {
                cmbKlientet.SelectedIndex = 0;
            }
            else
            {
                _IDKlienti = DbUtils.ExecSqlScalar(@"SELECT     [IDKlientDheLokacion ]
                                        FROM         [Orders]
                                        WHERE     (IDOrder = '" + _IDPorosia + "')");

                if (_IDKlienti.Trim() != "NONE")
                {
                    cmbKlientet.SelectedValue = _IDKlienti;
                }
                else
                {
                    cmbKlientet.SelectedIndex = 0;
                }
            }


        }

        private void FillGid(string _IDKlienti)
        {
            this.order_DetailsTableAdapter.FillFor_Orders(this.myMobileDataSet.Order_Details, _IDPorosia);

        }

        private bool New_Order()
        {
            //PRS-[SHITESI]-[DATA]-##
            //              [yy-mm-dd]
            //PRS-AMB-01-100120-01

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
            if (int.Parse(DbUtils.ExecSqlScalar("Select Count(*) From NumriPorosive where Data='" + DateTime.Now.Date + "'").ToString()) == 0)
            {
                //Insert new and set _nrPorosise=1;
                if (DbUtils.ExecSql(@"Insert Into NumriPorosive (Data,NrPorosise) Values ('" + DateTime.Now.Date + "','" + 01 + "')"))
                {
                    _nrPorosise = "01";
                }
            }
            else
            {
                _nrPorosise = (int.Parse(DbUtils.ExecSqlScalar("Select NrPorosise From NumriPorosive where Data='" + DateTime.Now.Date + "'")) + 1).ToString().PadLeft(2, '0');
                if (!DbUtils.ExecSql("Update NumriPorosive set NrPorosise='" + _nrPorosise + "' where Data='" + DateTime.Now.Date + "'"))
                {
                    MessageBox.Show("error");
                }
            }

            _IDPorosia = "PRS-" + frmHome.DevID + "-" + n.Year.ToString().Substring(2, 2) + n.Month.ToString().PadLeft(2, '0') + n.Day.ToString().PadLeft(2, '0') + "-" + _nrPorosise;
            string _SQL = @"INSERT INTO [Orders]
                          (
                            -- ID -- this column value is auto-generated,
                            IDOrder,
                            Depo,
                            [Data],
                            IDAgjenti,
                            IDKlientDheLokacion,
                            DeviceID,
                            SyncStatus,
                            Longitude,
                            Latitude
                          )
                        VALUES
                          (
                            @IDOrder,
                            @Depo,
                            @Data,
                            @IDAgjenti,
                            @IDKlientDheLokacion,
                            @DeviceID,
                            @SyncStatus,
                            @Longitude,
                            @Latitude
                          )";
            SqlCeConnection con = new SqlCeConnection(frmHome.AppDBConnectionsStr);
            try
            {

                SqlCeCommand cmd = new SqlCeCommand("", con);
                if (con.State == ConnectionState.Closed) con.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = _SQL;
                cmd.Parameters.AddWithValue("@IDOrder", _IDPorosia);
                cmd.Parameters.AddWithValue("@Depo", frmHome.Depo);
                cmd.Parameters.AddWithValue("@Data", DateTime.Now.ToString());
                cmd.Parameters.AddWithValue("@IDAgjenti", frmHome.IDAgjenti);
                cmd.Parameters.AddWithValue("@IDKlientDheLokacion", "None");
                cmd.Parameters.AddWithValue("@DeviceID", frmHome.DevID);
                cmd.Parameters.AddWithValue("@SyncStatus", "0");
                cmd.Parameters.AddWithValue("@Longitude", frmHome.LongitudeCur);
                cmd.Parameters.AddWithValue("@Latitude", frmHome.LatitudeCur);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (SqlCeException sqex)
            {
                MessageBox.Show(sqex.Message);
                DbUtils.WriteSQLCeErrorLog(sqex, _SQL);
                return false;
            }
            catch (Exception ex)
            {
                DbUtils.WriteExeptionErrorLog(ex);
                return false;
            }
            finally
            {
                con.Dispose();

            }
        }

        private bool Perform_Delete()
        {
            SqlCeConnection con = null;
            SqlCeCommand cmd = null;
            try
            {
                con = new SqlCeConnection(frmHome.AppDBConnectionsStr);
                cmd = new SqlCeCommand("UPDATE TEST", con);
                if (con.State == ConnectionState.Closed)
                { con.Open(); }
                cmd.Transaction = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM Order_Details where IDOrder='" + _IDPorosia + "'";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM [Orders] WHERE    (IDOrder = '" + _IDPorosia + "')";
                cmd.ExecuteNonQuery();

                cmd.Transaction.Commit();
                return true;

            }
            catch (SqlCeException SQLEX)
            {
                cmd.Transaction.Rollback();
                DbUtils.WriteSQLCeErrorLog(SQLEX, cmd.CommandText);
                return false;
            }
            catch (Exception EX)
            {
                cmd.Transaction.Rollback();
                DbUtils.WriteExeptionErrorLog(EX);
                return false;
            }
            finally
            {
                con.Dispose();
                cmd.Dispose();
            }

        }

        private bool IsCountUnit()
        {
            bool a = false;

            if ((!btnShto.Enabled) && (orderDetailsBindingSource.Current != null))
            {
                a = (orderDetailsBindingSource.Current as DataRowView)["BUM"].ToString() != "KG";
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

        private bool DeleteOrder(bool update)
        {
            bool result = false;
            bool result2 = false;
            result = DbUtils.ExecSql("Delete from Orders where IDOrder='" + _IDPorosia + "'");
            result2 = DbUtils.ExecSql("Delete from Order_Details where IDOrder='" + _IDPorosia + "'");
            if (result && result2)
            {
                if (update)
                { DbUtils.ExecSql("Update NumriPorosive set NrPorosise='" + (int.Parse(_nrPorosise) - 1).ToString() + "' where Data = '" + DateTime.Now.Date.ToString() + "'"); }
            }

            return result && result2;
        }

        private void PerformCount()
        {
            for (int i = 1; i <= orderDetailsBindingSource.Count; i++)
            {
                myMobileDataSet.Order_Details.Rows[i - 1]["NrRendor"] = i;
            }
        }

        private void Print(string nrPorosise)
        {

            this.order_DetailsTableAdapter.Update(this.myMobileDataSet.Order_Details);
            if (cmbKlientet.SelectedIndex > 0)
            {
                string _UPDATE = "Update [Orders] set IDKlientDheLokacion='" + cmbKlientet.SelectedValue + "' where [Orders].IDOrder ='" + nrPorosise + "'";
                DbUtils.ExecSql(_UPDATE);
            }
            string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            PnPrint Rpt = null;
            PnType PT = PnType.Print;
            string _IDPorosia = frmVizitat.IDPorosi.ToString();
            DataTable details = new DataTable();
            DataTable header = new DataTable();
            DataTable footer = new DataTable();
            string strPortName = "";
            PnDevice devPort = PnDevice.BluetoothPort;
            #region PrintParams

            if (msg == null) msg = new frmMessage(true, new string[]{null});
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

            try
            {
                DataTable[] Header_Details = null;
                if (_Type == OrderType.Edit)
                {
                    Header_Details = PnFunctions.CreateOrdersBill(lblNrPorosise.Text);
                }
                else
                {
                    Header_Details = PnFunctions.CreateOrdersBill(lblNrPorosise.Text);
                }

                header = Header_Details[0];
                details = Header_Details[1];
                footer = Header_Details[2];

                if (details.Rows.Count > 0 && header.Rows.Count > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Rpt = new PnPrint(devPort, strPortName, CurrentDir + "\\rptPorosia.pnrep", details, header, footer);

                    Rpt.PrintPnReport(PT, true, " tre ", false);
                }
                else
                {
                    MessageBox.Show("Gabim gjatë gjenerimit te Fatures \n Printimin nuk munde të vazhdoj");
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

        #endregion

    }
}