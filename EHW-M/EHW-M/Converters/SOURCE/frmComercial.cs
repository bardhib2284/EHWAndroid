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

namespace MobileSales
{
    public partial class frmComercial : Form
    {
        private bool isEdit = false;
        private string _IDArtikulli;
        private Guid IDPorosia;
        SqlCeTransaction salecTran = null;
        SqlCeCommand cmd = null;
        SqlCeConnection con = null;
        string _IDKlienti;

        public frmComercial()
        {
            InitializeComponent();
        }

        public frmComercial(string IDKlienti)
        {
            InitializeComponent();
            _IDKlienti = IDKlienti;

        }

        private bool Begin_Transaction()
        {
            bool result = false;
            try
            {
                con = new SqlCeConnection(frmHome.AppDBConnectionsStr);
                cmd = new SqlCeCommand("", con);
                if (con.State == ConnectionState.Closed) con.Open();
                cmd.Transaction = salecTran;
                result = true;

            }
            catch (SqlCeException sqlex)
            {
                MessageBox.Show(sqlex.Message);
            }
            catch (Exception)
            {
                MessageBox.Show("Test");
                throw;
            }
            return result;
        }

        private void FillGid(string ID)
        {
            string fillQuery =@"SELECT pa.IDArtikulli,pa.Emri AS Artikulli,
                                       pa.SasiaPorositur AS Sasia,
                                       pa.CmimiAktual AS Cmimi,
                                       pa.Rabatet AS Rabat,
                                       (pa.SasiaPorositur * pa.CmimiAktual) AS Totali, 
                                       P.IDPorosia
                                FROM   Porosite p
                                       INNER JOIN PorosiaArt pa
                                            ON  pa.IDPorosia = p.IDPorosia
                                       LEFT JOIN Vizitat v
                                            ON  v.IDVizita = p.IDVizita
                                WHERE v.IDKlientDheLokacion='"+ID+"'";
            DbUtils.FillGrid(dsPorosia, grdPorosia, fillQuery);
            Begin_Transaction();
            if (dsPorosia.Tables[0].Rows.Count > 0)
            {
                IDPorosia = new Guid(dsPorosia.Tables[0].Rows[0]["IDPorosia"].ToString());
            }
            
                

        }

        private bool Insert_Porosia()
        {
            bool result = true;
            IDPorosia = new Guid();
            try
            {
                string insert_Porosite = @"INSERT INTO Porosite
                                      (
                                        IDVizita,
                                        IDPorosia,
                                        DataPorosise,
                                        OraPorosise,
                                        NrPorosise,
                                        DeviceID,
                                        SyncStatus,
                                        Aprovuar
                                      )
                                    VALUES
                                      (
                                        @IDVizita,
                                        @IDPorosia,
                                        @DataPorosise,
                                        @OraPorosise,
                                        @NrPorosise,
                                        @DeviceID,
                                        @SyncStatus,
                                        @Aprovuar
                                      )";
                cmd.CommandText = insert_Porosite;
                cmd.Parameters.AddWithValue("@IDVizita","'"+ frmVizitat.CurrIDV.ToString()+"'");
                cmd.Parameters.AddWithValue("@IDPorosia", "'" + (IDPorosia).ToString() + "'");
                cmd.Parameters.AddWithValue("@DataPorosise",System.DateTime.Now.ToString("dd/MM/yyyy"));
                cmd.Parameters.AddWithValue("@OraPorosise", System.DateTime.Now.ToString("hh:mm:ss"));
                cmd.Parameters.AddWithValue("@NrPorosise", 10);
                cmd.Parameters.AddWithValue("@DeviceID", frmHome.DevID);
                cmd.Parameters.AddWithValue("@SyncStatus", 0);
                cmd.Parameters.AddWithValue("@Aprovuar", 0);
                cmd.ExecuteNonQuery();
                result = true;
            }
            catch (SqlCeException)
            {
                result = false;
                salecTran.Rollback();
                salecTran.Dispose();
                con.Dispose();
                con.Close();
            }

            return result;
        }

        private bool Commit_Transaction()
        {
            bool result = false;
            try
            {
                cmd.ExecuteNonQuery();
                salecTran.Commit();
                result = true;
            }
            catch (SqlCeException sqlEX)
            {
                salecTran.Rollback();
                MessageBox.Show(sqlEX.Message);
                throw;
            }
            catch (Exception)
            {
                salecTran.Rollback();
                throw;
            }
            return result;
        }

        private bool Update_Record(string IDArt)
        {
            bool result = false;

            try
            {
                string update_PorosiaArt = @"UPDATE PorosiaArt
                                        SET    SasiaPorositur = @SasiaPorositur,
                                               SasiLiferuar = @SasiaLiferuar
                                        WHERE  IDArtikulli = @IDArtikulli
                                        ";
                cmd.CommandText = update_PorosiaArt;
                cmd.Parameters.AddWithValue("@SasiaPorositur", txtSasia.Text);
                cmd.Parameters.AddWithValue("@SasiaLiferuar", txtSasia.Text);
                cmd.Parameters.AddWithValue("@IDArtikulli",IDArt);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                string update_Porosite = @"UPDATE Porosite
                                        SET    DataPorosise = @DataPorosise,
                                               OraPorosise = @OraPorosise,
                                               DataPerLiferim = @DataPerLiferim
                                        WHERE  IDPorosia = @IDPorosia";
                cmd.CommandText = update_Porosite;
                cmd.Parameters.AddWithValue("@DataPorosise", System.DateTime.Now.ToShortDateString());
                cmd.Parameters.AddWithValue("@OraPorosise", System.DateTime.Now.ToString("hh:mm:ss"));
                cmd.Parameters.AddWithValue("@DataPerLiferim", dtpData.Value.ToString("dd/MM/yyyy"));
                cmd.Parameters.AddWithValue("@IDPorosia",IDPorosia.ToString());
                cmd.ExecuteNonQuery();

                result = true;
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

        private bool Insert_Record()
        {
            bool result = false;
            try
            {
                string insertCommand = @"INSERT INTO PorosiaArt
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
                                        IDArsyeja
                                      )
                                    VALUES
                                      (
                                        @IDArtikulli,
                                        @SasiaPorositur,
                                        @CmimiAktual,
                                        @Rabatet,
                                        @IDPorosia,
                                        @SasiLiferuar,
                                        @Emri,
                                        @Gratis,
                                        @SasiaPako,
                                        @DeviceID,
                                        @SyncStatus,
                                        @IDArsyeja
                                      )";
                cmd.CommandText = insertCommand;
                cmd.Parameters.AddWithValue("@IDArtikulli", 100);
                cmd.Parameters.AddWithValue("@SasiaPorositur", 10);
                cmd.Parameters.AddWithValue("@CmimiAktual", 10);
                cmd.Parameters.AddWithValue("@Rabatet", 10);
                cmd.Parameters.AddWithValue("@IDPorosia", 10);
                cmd.Parameters.AddWithValue("@SasiLiferuar", 10);
                cmd.Parameters.AddWithValue("@Emri", 10);
                cmd.Parameters.AddWithValue("@Gratis", 10);
                cmd.Parameters.AddWithValue("@SasiaPako", 10);
                cmd.Parameters.AddWithValue("@DeviceID", 10);
                cmd.Parameters.AddWithValue("@SyncStatus", 0);
                cmd.Parameters.AddWithValue("@IDArsyeja", 10);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                MessageBox.Show("Regjistrimi deshtoi");
                
            }

            return result;
        }

        private void fillDetails()
        {
            //dtpData.Value = (DateTime)dsPorosia.Tables[0].Rows[grdPorosia.CurrentRowIndex]["DataPerLiferim"];
            txtSasia.Text = dsPorosia.Tables[0].Rows[grdPorosia.CurrentRowIndex]["Sasia"].ToString();
            txtArtikulli.Text = dsPorosia.Tables[0].Rows[grdPorosia.CurrentRowIndex]["Artikulli"].ToString();

        }

        #region Helper functions
        
        private void DoButtonActivation(object sender)
        {
            if (sender is Button)
            {
                switch ((sender as Button).Name)
                {
                    case "btnSave":
                        {
                            btnNew.Enabled = true;
                            btnNew.Font = new Font("New", 8, FontStyle.Bold);
                            btnNew.BackColor = Color.WhiteSmoke;

                            btnSave.Enabled = false;
                            btnSave.Font = new Font("Save", 8, FontStyle.Italic);
                            btnSave.BackColor = Color.Silver;

                            btnEdit.Enabled = false;
                            btnEdit.Font = new Font("Edit", 8, FontStyle.Italic);
                            btnEdit.BackColor = Color.Silver;

                            btnCancel.Enabled = false;
                            btnCancel.Font = new Font("Cancel", 8, FontStyle.Italic);
                            btnCancel.BackColor = Color.Silver;

                            grdPorosia.Enabled = true;
                            pnlDetails.Enabled = false;
                            break;
                        }
                    case "btnEdit":
                        {
                            btnNew.Enabled = false;
                            btnNew.Font = new Font("New", 8, FontStyle.Italic);
                            btnNew.BackColor = Color.Silver;

                            btnSave.Enabled = true;
                            btnSave.Font = new Font("Save", 8, FontStyle.Bold);
                            btnSave.BackColor = Color.WhiteSmoke;

                            btnEdit.Enabled = false;
                            btnEdit.Font = new Font("Edit", 8, FontStyle.Italic);
                            btnEdit.BackColor = Color.Silver;

                            btnCancel.Enabled = true;
                            btnCancel.Font = new Font("Cancel", 8, FontStyle.Bold);
                            btnCancel.BackColor = Color.WhiteSmoke;
                            grdPorosia.Enabled = false;
                            pnlDetails.Enabled = true;
                            break;
                        }
                    case "btnCancel":
                        {
                            btnNew.Enabled = true;
                            btnNew.Font = new Font("New", 8, FontStyle.Bold);
                            btnNew.BackColor = Color.WhiteSmoke;

                            btnSave.Enabled = false;
                            btnSave.Font = new Font("Save", 8, FontStyle.Italic);
                            btnSave.BackColor = Color.Silver;

                            btnEdit.Enabled = false;
                            btnEdit.Font = new Font("Edit", 8, FontStyle.Italic);
                            btnEdit.BackColor = Color.Silver;

                            btnCancel.Enabled = false;
                            btnCancel.Font = new Font("Cancel", 8, FontStyle.Italic);
                            btnCancel.BackColor = Color.Silver;

                            grdPorosia.Enabled = true;
                            pnlDetails.Enabled = false;
                            //int pos = grdPorosia.CurrentRowIndex;
                            //FillVendet();
                            //Ctrl.SetRowPosition(grdVendet, pos);
                            break;
                        }
                }
            }
            else
            {
                if ((sender as Form).Name == "frmComercial")
                {
                    pnlDetails.Enabled = false;
                    btnCancel.Enabled = false;
                    btnSave.Enabled = false;
                    btnEdit.Enabled = false;
                    btnNew.Enabled = true;
                    btnNew.Font = new Font("New", 8, FontStyle.Bold);
                    btnNew.BackColor = Color.WhiteSmoke;


                }
            }
            
        }

        #endregion

        private void frmComercial_Load(object sender, EventArgs e)
        {
            FillGid(_IDKlienti);
            DoButtonActivation(sender);
        }

        private void menuDalja_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grdPorosia_MouseUp(object sender, MouseEventArgs e)
        {
            
            if (grdPorosia.CurrentRowIndex >= 0)
            {
                grdPorosia.Select(grdPorosia.CurrentRowIndex);
                _IDArtikulli = dsPorosia.Tables[0].Rows[grdPorosia.CurrentRowIndex]["IDArtikulli"].ToString();
                fillDetails();
                btnEdit.Enabled = true;
                btnEdit.BackColor = Color.WhiteSmoke;
                btnEdit.Font = new Font("Edit", 8, FontStyle.Bold);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            DoButtonActivation(sender);
            isEdit = true;
        }

        frmArtikujtStoqet fArt = null;
        private void btnNew_Click(object sender, EventArgs e)
        {
           

            //if (fArt == null) fArt = new frmArtikujtStoqet();
            //fArt.position = "Shitja";
            //Cursor.Current = Cursors.WaitCursor;
            //fArt.Show();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            FillGid(_IDKlienti);
            txtArtikulli.Text = "";
            txtSasia.Text = "";
            DoButtonActivation(sender);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool successful = false;
            if (isEdit)
            {
                //Update record
                successful = Update_Record(_IDArtikulli);
            }
            else
            {
                //Insert new Record
            }
            if (successful)
            {
                FillGid(_IDKlienti);
                DoButtonActivation(sender);
            }
            else
            {
                MessageBox.Show("Freshkimi i shenimeve deshtoi");
                return;
            }
        }


    }
}