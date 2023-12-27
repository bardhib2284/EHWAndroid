using System;
using PnUtils;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;

namespace MobileSales
{
    public partial class RegjistrimiVizites : Form
    {
        public static bool isClosed;
        private DateTime DateToday = DateTime.Now.Date;

        public RegjistrimiVizites()
        {
            InitializeComponent();
        }

        private void RegjistrimiVizites_Load(object sender, EventArgs e)
        {
            vizitatTableAdapter1.Connection.ConnectionString = frmHome.AppDBConnectionsStr;
            statusiVizitesTableAdapter.Connection.ConnectionString = frmHome.AppDBConnectionsStr;
            if (frmVizitat.RegjistroViziten == true) 
            {
                this.statusiVizitesTableAdapter.FillByIDstat(this.myMobileDataset.StatusiVizites, frmVizitat.StatusiVizites);
                txtKlienti.Text = frmVizitat.Emri;
                txtLokacioni.Text = frmVizitat.EmriLokacionit;
                txtKlienti.Enabled = false;
                txtLokacioni.Enabled = false;
                cmbStatusi.Enabled = false;
                cmbNewStatus.Enabled = true;
                cmbNewStatus.SelectedIndex = 0;
                menu_Save.Enabled = true;
                if (frmVizitat.selectedDate != DateToday && frmHome.DayLock==true)
                {
                    cmbNewStatus.Enabled = false;
                    menu_Save.Enabled = false;

                }

            }
            init();
            cmbStatusiVjeter.SelectedValue = frmVizitat.StatusiVizites;
           
        }

        #region Methods

        private void init()
        {
            string _selectQuery = "";
            string _selectQuerynew = "";
            try
            {
                _selectQuery = "SELECT * FROM StatusiVizites sv";

                _selectQuerynew = "SELECT * FROM StatusiVizites sv where sv.IDStatusiVizites <> 1";
                DbUtils.FillCombo(cmbStatusiVjeter, dsStatuset, _selectQuery, "Gjendja", "IDStatusiVizites");
                DbUtils.FillCombo(cmbNewStatus, dsNewStatus, _selectQuerynew, "Gjendja", "IDStatusiVizites");
            }
            catch (SqlCeException ex)
            {
                MessageBox.Show(ex.Message + ": Gabim gajte leximit ne Statuset e Vizitave");
                DbUtils.WriteSQLCeErrorLog(ex, _selectQuery);
                return;
            }

        }

        #endregion

        private void menu_Save_Click(object sender, EventArgs e)
        {
            string _updateCommand = "";
            try
            {
                _updateCommand = @"UPDATE Vizitat
                                     SET
                                     SyncStatus = 0
                                    ,IDStatusiVizites = " + cmbNewStatus.SelectedValue +
                                 @" ,DataRealizimit = GETDATE()
                                    ,OraRealizimit = GETDATE()  where IDVizita = '" + frmVizitat.CurrIDV + "'";
                if (DbUtils.ExecSql(_updateCommand))
                {
                    this.Close();
                    isClosed = false;
                }
                else
                {
                    MessageBox.Show("Ndryshimi i statusit te Vizites deshtoi");

                }
            }
            catch (SqlCeException sce)
            {
                MessageBox.Show(sce.Message + ": Gabim ne freskimin e statusit te Vizites");
                DbUtils.WriteSQLCeErrorLog(sce, _updateCommand);
                return;
            }
        }

        private void menu_Close_Click(object sender, EventArgs e)
        {
            isClosed = true;
            this.Close();


            string _updateCommand = "";
            try
            {
                _updateCommand = @"UPDATE Vizitat
                                     SET
                                     IDStatusiVizites = " + 0 +
                                 @"  where IDVizita = '" + frmVizitat.CurrIDV + "'";
                if (DbUtils.ExecSql(_updateCommand))
                {
                    this.Close();
                    isClosed = false;
                }
                else
                {
                    MessageBox.Show("Ndryshimi i statusit te Vizites deshtoi");

                }
            }
            catch (SqlCeException sce)
            {
                MessageBox.Show(sce.Message + ": Gabim ne freskimin e statusit te Vizites");
                DbUtils.WriteSQLCeErrorLog(sce, _updateCommand);
                return;
            }
        }


    }
}