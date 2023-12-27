using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Data.SqlServerCe;
using System.Windows.Forms;
using System.Globalization;
using PnUtils;
namespace MobileSales
{
    public partial class frmPorosite : Form
    {
        private bool isSelected = false, Key;
        private DateTime dtFisrtDate;
        private int currentDay, SelectedRow;
        private string _IDPorosia, _IDKlientDheLokacion;
        public static OrderType Order_Type;

        #region Initialize Instance

        frmRpt_Porosit rpt_Porosia = null;
        public frmPorosia_Detalet frmPorosia_Detalet = null;


        #endregion

        public frmPorosite()
        {
            InitializeComponent();
        }

        private void frmPorosite_Load(object sender, EventArgs e)
        {
            InitDate();
            FillPorosite();
        }

        private void menuDalja_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuPorosite_Click(object sender, EventArgs e)
        {
            
        }
    
        private void orderDataGrid_MouseUp(object sender, MouseEventArgs e)
        {

            System.Windows.Forms.DataGrid.HitTestInfo myHitTest = this.orderDataGrid.HitTest(e.X, e.Y);
            if (myHitTest.Type == DataGrid.HitTestType.Cell)
            {
                isSelected = true;
                this.orderDataGrid.Select(myHitTest.Row);
                SelectedRow = myHitTest.Row;

            }
            if (orderDataGrid.CurrentRowIndex != -1)
            {
                try
                {
                    if (orderDataGrid[SelectedRow, 2].ToString() == "0")
                    {
                        menu_Edit.Enabled = true;
                    }
                    else
                    {
                        menu_Edit.Enabled = false;
                    }

                    _IDPorosia = orderDataGrid[orderDataGrid.CurrentRowIndex, 0].ToString();
                    _IDKlientDheLokacion = (orderBindingSource.Current as DataRowView)["IDKlientDheLokacion"].ToString();
                    isSelected = true;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private void orderDataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            Key = (e.KeyData == Keys.Up || e.KeyData == Keys.Down || e.KeyData == Keys.Left || e.KeyData == Keys.Right);
            if (isSelected == true && Key)
            {
                this.orderDataGrid.Select(this.orderDataGrid.CurrentRowIndex);
                SelectedRow = this.orderDataGrid.CurrentRowIndex;
                _IDKlientDheLokacion = (orderBindingSource.Current as DataRowView)["IDKlientDheLokacion"].ToString();
                _IDPorosia = orderDataGrid[orderDataGrid.CurrentRowIndex, 0].ToString();
                if (orderDataGrid[this.orderDataGrid.CurrentRowIndex, 2].ToString() == "0")
                {
                    menu_Edit.Enabled = true;
                }
                else
                {
                    menu_Edit.Enabled = false;
                }
            }

        }

        #region Methods

        private void InitDate()
        {
            CultureInfo myCI = new CultureInfo("fr-FR");//FS. cdo here qe hapet forma instancohet ky objekt, shkakton memoryLeak
            System.Globalization.Calendar myCal = myCI.Calendar;
            currentDay = (int)myCal.GetDayOfWeek(DateTime.Now);
            dtFisrtDate = DateTime.Now.Date.AddDays(-7);
            
            lblData.Text = "Lista e porosive " + dtFisrtDate.ToString("dd/MM/yyy") + "-" + DateTime.Now.Date.ToString("dd/MM/yyy");

        }

        private void FillPorosite()
        {
            orderTableAdapter.Connection = frmHome.AppDBConnection;
            this.orderTableAdapter.FillBy_Date(this.myMobileDataSet.Orders, dtFisrtDate);
            menu_Edit.Enabled = false;
            if (orderBindingSource.Count >= 1)
            {
                orderDataGrid.UnSelect(0);

            }
        }

        private bool Delete_History()
        {
            bool result = false;
            DateTime date = DateTime.Now.Date;
            SqlCeConnection con = null;
            SqlCeCommand cmd = null;
            try
            {
                con = new SqlCeConnection(frmHome.AppDBConnectionsStr);
                cmd = new SqlCeCommand("", con);
                cmd.CommandType = CommandType.Text;
                if (con.State == ConnectionState.Closed)
                    con.Open();
                cmd.Transaction = con.BeginTransaction();

                cmd.CommandText = @"DELETE 
                                FROM   Orders
                                WHERE  [Data] <'" + date.AddDays(-7) + "'";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"
                                DELETE 
                                FROM   Order_Details
                                WHERE  IDOrder NOT IN (SELECT o.IDOrder
                                                       FROM   Orders o)";
                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();
                result = true;
            }
            catch (SqlCeException)
            {
                cmd.Transaction.Rollback();
                result = false;
            }
            catch (Exception)
            {
                cmd.Transaction.Rollback();
                result = false;
            }
            finally
            {
                con.Dispose();
                cmd.Dispose();
            }
            return result;
        }

        #endregion

        private void menuKonvert_Click(object sender, EventArgs e)
        {
            if (orderDataGrid.VisibleRowCount != 0)
            {
                if (orderDataGrid[orderDataGrid.CurrentRowIndex, 2].ToString() == "1")
                {
                    if (MessageBox.Show("Jeni të sigurtë për konvertim në fatur?", "Konvertimi", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        generateBill();

                    }

                }
            }
        }

        private void generateBill()
        {
            SqlCeConnection con = new SqlCeConnection(DbUtils.MainSqlConnection.ConnectionString);
            SqlCeCommand cmd = new SqlCeCommand("", con);
            SqlCeDataAdapter da = new SqlCeDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            cmd.Transaction = con.BeginTransaction();


            Guid IDPorosia = Guid.NewGuid();
            // string idPorosia = IDPorosia.ToString();


            try
            {
                string IDKlienti = orderDataGrid[orderDataGrid.CurrentRowIndex, 3].ToString();

                Guid IDVizita = new Guid(
                    DbUtils.ExecSqlScalar("Select IDVizita from Vizitat where IDKlientDheLokacion='" + IDKlienti + "'"));

                string _sqlCE = @"INSERT INTO Porosite
                            (
	                            IDVizita,
	                            DataPerLiferim,
	                            IDPorosia,
	                            DataPorosise,
	                            OraPorosise,
	                            StatusiPorosise,
	                            NrPorosise,
	                            DeviceID,
	                            SyncStatus,
	                            NrDetalet
                            )
                            VALUES
                            (
	                            '" + IDVizita + @"',
	                            '" + DateTime.Now.Date + @"',
	                            '" + IDPorosia + @"',
	                            '" + DateTime.Now.Date + @"',
	                            GETDATE(),
	                            1,
	                            'NrPorosise',
	                            '" + frmHome.DevID + @"',
	                            0,
	                            0
                            )";
                cmd.CommandText = _sqlCE;
                cmd.ExecuteNonQuery();

                _sqlCE = @"Select * from Order_Details where IDOrder='" + _IDPorosia + "'";
                da.SelectCommand.CommandText = _sqlCE;
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string _IDArtikulli = dt.Rows[i]["IDArtikulli"].ToString();
                    string _Artikulli = dt.Rows[i]["Emri"].ToString();
                    decimal _Sasia = decimal.Parse(dt.Rows[i]["Sasia_Porositur"].ToString());

                    cmd.CommandText = "Select UnitPrice from SalesPrice where SalesCode='" + _IDKlientDheLokacion + "' and ItemNo_='" + _IDArtikulli + "'";
                    decimal _Cmimi = Math.Round(decimal.Parse(cmd.ExecuteScalar().ToString()), 2);


                    cmd.CommandText = "Select Sasia from Stoqet where Shifra='" + _IDArtikulli + "'";
                    decimal _Max_Sasia = decimal.Parse(cmd.ExecuteScalar().ToString());

                    cmd.CommandText = "Select BUM from Artikujt where IDArtikulli='" + _IDArtikulli + "'";
                    string _BUM = cmd.ExecuteScalar().ToString();

                    if (_Max_Sasia != 0 && _Sasia > _Max_Sasia)
                    { _Sasia = _Max_Sasia; }
                    if (_Max_Sasia != 0 && _Sasia > 0)
                    {
                        _sqlCE = @"INSERT INTO PorosiaArt
                        (
	                        IDArtikulli,
	                        SasiaPorositur,
	                        CmimiAktual,
	                        Rabatet,
	                        IDPorosia,
	                        SasiLiferuar,
	                        Emri,
	                        Gratis,
	                        SasiaPako,
	                        DeviceID,
	                        SyncStatus,
	                        IDArsyeja,
	                        CmimiPaTVSH,
	                        BUM
                        )
                        VALUES
                        (
	                        '" + _IDArtikulli + @"',
	                        " + _Sasia + @",
	                        " + _Cmimi + @",
	                        0,
	                        '" + IDPorosia + @"',
	                        " + _Sasia + @",
	                        '" + _Artikulli + @"',
	                        0,
	                        0,
	                        '" + frmHome.DevID + @"',
	                        0,
	                        NULL,
	                        0,
	                       '" + _BUM + @"'
                        )";
                        cmd.CommandText = _sqlCE;
                        cmd.ExecuteNonQuery();
                    }
                }
                cmd.Transaction.Commit();
                con.Close();
                cmd.Dispose();
                new frmShitja(IDVizita, IDPorosia, IDKlienti, _IDPorosia).ShowDialog();
                FillPorosite();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gabim gjatë gjenerimit të faturës");
                DbUtils.WriteExeptionErrorLog(ex);
            }
           
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            new frm_Gjenerimi_Porosive().ShowDialog();
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            if (isSelected && orderDataGrid.VisibleRowCount!=0)
            {
                new rptOrder_Details(orderDataGrid[orderDataGrid.CurrentRowIndex, 0].ToString()).ShowDialog();
            }
            else
            {
                MessageBox.Show("Selektoni një porosi");
            }
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            if (isSelected && orderDataGrid.VisibleRowCount!=0)
            {

                if (rpt_Porosia == null) rpt_Porosia = new frmRpt_Porosit(_IDPorosia); ;
                rpt_Porosia.ShowDialog();
                rpt_Porosia = null;

            }
            else
            {
                MessageBox.Show("Selektoni një porosi");
            }
        }

        private void frmPorosite_KeyDown(object sender, KeyEventArgs e)
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

        private void menu_New_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (frmPorosia_Detalet == null) frmPorosia_Detalet = new frmPorosia_Detalet("NONE", OrderType.New, "NONE");
            frmPorosia_Detalet.ShowDialog();
            frmPorosia_Detalet = null;
            FillPorosite();
        }

        private void menu_Edit_Click(object sender, EventArgs e)
        {
            if (isSelected)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (frmPorosia_Detalet == null) frmPorosia_Detalet = new frmPorosia_Detalet(_IDPorosia, OrderType.Edit, _IDKlientDheLokacion);
                frmPorosia_Detalet.ShowDialog();
                frmPorosia_Detalet = null;
                FillPorosite();
            }
        }
    }

    public enum OrderType
    {
        New,
        Edit

    }


}