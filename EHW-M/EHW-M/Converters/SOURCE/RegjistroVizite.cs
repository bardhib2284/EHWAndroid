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

namespace MobileSales
{
    public partial class RegjistroVizite : Form
    {
        public RegjistroVizite()
        {
            InitializeComponent();
        }




        private void RegjistroVizite_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string _selectQuery = "";
            this.mobileDatabaseDataSet.EnforceConstraints = false;
            InitConnection();
            try
            {
                _selectQuery = "SELECT * FROM KlientDheLokacion kdl where kdl.KontaktEmriMbiemri <> '' order by kdl.KontaktEmriMbiemri";
                DbUtils.FillCombo(cmbKlienti, dsKlient, _selectQuery, "KontaktEmriMbiemri", "IDKlientDheLokacion");
                dTPickerData.Value = DateTime.Now;
                if (frmVizitat.Vizitat_IDKlienti != null)
                {
                    cmbKlienti.SelectedValue = frmVizitat.Vizitat_IDKlienti;
                }
            }
            catch (SqlCeException sce)
            {
                MessageBox.Show(sce.Message + ": Gabim gjate leximit te klienteve \n Tabela Klientet");
                DbUtils.WriteSQLCeErrorLog(sce, "Select * from Klientet");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            Cursor.Current = Cursors.Default;
        }

        private void cmbKlienti_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtAdresa.Text = dsKlient.Tables[0].Rows[cmbKlienti.SelectedIndex]["Adresa"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                DbUtils.WriteExeptionErrorLog(ex);
                return;
            }
        }

        private void InitConnection()
        {
            klientDheLokacionTableAdapter.Connection.ConnectionString = frmHome.AppDBConnectionsStr;
            vizitatTableAdapter1.Connection.ConnectionString = frmHome.AppDBConnectionsStr;
        }

        private void menu_Save_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime DataPlanifikimit = dTPickerData.Value;
                DateTime OraPlanifikimit = dTPickerKoha.Value;
                DateTime DataAritjes = DateTime.Now;
                string Data = dTPickerData.Value.ToString("dd/MM/yy");
                int effectRows = this.vizitatTableAdapter1.SearchVizitat(this.mobileDatabaseDataSet.Vizitat, Data, cmbKlienti.SelectedValue.ToString());
                if (effectRows >= 1)
                {
                    MessageBox.Show("Klienti ekziston në agjendën ditore!");
                    return;
                }
                else
                {
                    this.vizitatTableAdapter1.KrijoVizite(frmHome.IDAgjenti, frmVizitat.numberOFVizits + 1, "0", frmHome.DevID, 0, DataPlanifikimit, DataAritjes, cmbKlienti.SelectedValue.ToString(), frmHome.LongitudeCur, frmHome.LatitudeCur);
                    MessageBox.Show("Regjistrimi perfundoi me sukses");
                }
                this.Close();
            }
            catch (SqlCeException sce)
            {
                MessageBox.Show("Error: " + sce.Message);
                DbUtils.WriteSQLCeErrorLog(sce, "Insert into Vizitat");
            }
        }

        private void menu_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}