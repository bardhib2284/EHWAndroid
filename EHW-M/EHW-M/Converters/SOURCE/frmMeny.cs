using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//using PnSyncTestForm;
using System.Data.SqlServerCe;
using System.IO;
using System.Reflection;
using PnUtils;
using System.Diagnostics;

namespace MobileSales
{
    public partial class frmMeny : Form
    {
        #region Initialize Instance

        frmVizitat frmVizitat = null;
        frmStoku frmStoku = null;
        frmInkasimiMobil Inkasimi = null;
        frmPorosite frmPorosit = null;

        #endregion

        public frmMeny()
        {
            InitializeComponent();
        }

        private void frmMeny_Load(object sender, EventArgs e)
        {
            btnVizitat.Focus();
            Cursor.Current = Cursors.Default;

            lblEmriAgjentit.Text = "Agjenti: " + frmHome.EmriAgjendit.ToString();
            lblDevID.Text = frmHome.TAGNR_text; //"Psion: " + frmHome.DevIDConfig;
            lblDepo.Text = "Depo: " + frmHome.Depo;

            Assembly asm = Assembly.GetExecutingAssembly();
            //lblVersion.Text = asm.GetName().Version.ToString();
            //if (PnUtils.DbUtils.CheckSQLCeErrorLog() == true)
            //{
            //    if (lblVersion.Text.Substring(0, 1) != "!")
            //        lblVersion.Text = "!" + lblVersion.Text;
            //}
            //else
            //    if (lblVersion.Text.Substring(0, 1) == "!")
            //        lblVersion.Text.Remove(0, 1);


        }

        private void btnVizitat_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (frmVizitat == null)
                frmVizitat = new frmVizitat();
            frmVizitat.ShowDialog();
            this.Close(); //FS. Ky kod nuk ekzekutohet pa u mbyll [Vizitat], kemi ShowDialog() qe pritet DialogResult(modal) deri sa te mbyllet forma.
            GC.Collect();
        }

        private void btnCkycu_Click(object sender, EventArgs e)
        {
            //GeG: Dalja nga Aksioni = dalja mga aplikacioni
            if (MessageBox.Show("Confirmoni daljen nga aplikacioni", "Dalja", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                Application.Exit();
                Application.DoEvents();
                try
                {
                    Process.GetCurrentProcess().Kill();
                }
                catch { }
            }
        }

        private void btnArtikujt_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (frmStoku == null)
                frmStoku = new frmStoku();
            frmStoku.ShowDialog();

            GC.Collect();
        }

        //private void picBack_Paint(object sender, PaintEventArgs e)
        //{
        //    foreach (Control C in this.Controls)
        //    {
        //        if (C is Label)
        //        {
        //            Label L = (Label)C;
        //            L.Visible = false;
        //            e.Graphics.DrawString(L.Text, L.Font, new
        //            SolidBrush(L.ForeColor), L.Left - picBack.Left, L.Top - picBack.Top);

        //        }
        //    }
        //}

        private void btnSinkornizimi_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            frmSync Sync = null;//Instancim objekti
            try
            {
                Sync = new frmSync();
                Sync.btnSinkronizo.Enabled = true;
                Sync.btnShkarko.Enabled = true;
                Sync.ShowDialog();
                GC.Collect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Sync.Dispose();
            }
        }

        private void btnInkasimiBorxhet_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (Inkasimi == null) Inkasimi = new frmInkasimiMobil();
            Inkasimi.ShowDialog();
            GC.Collect();

        }

        #region Methods

        /// <summary>
        /// Metode e cila Freskon shenime e kolonave Shitur|Kthyer|Mbetur para gjenerimit te raportit
        /// </summary>
        private void UpdateAllValues()
        {
            PnUtils.DbUtils.UpateMalli_Mbetur_SHITUR();
            PnUtils.DbUtils.UpdateMalli_Mbetur_KTHYER();
            PnUtils.DbUtils.UpdateMalli_Mbetur_Mbetur();
        }

        private int counLiferimi()
        {
            SqlCeCommand cm = null;
            SqlCeConnection conn = new SqlCeConnection(frmHome.AppDBConnectionsStr);
            int result = 0;
            try
            {
                string _CountLiferimi = "Select Count(L.IDLiferimi) from Liferimi L";
                cm = new SqlCeCommand(_CountLiferimi, conn);
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                result = int.Parse(cm.ExecuteScalar().ToString());
            }
            catch (SqlCeException ex)
            {
                result = -1;
                DbUtils.WriteSQLCeErrorLog(ex, cm.CommandText);

            }
            finally
            {
                conn.Dispose();
                cm.Dispose();
            }
            return result;
        }

        #endregion

        private void btnPorosite_Click(object sender, EventArgs e)
        {
            if (frmPorosit == null)
            { frmPorosit = new frmPorosite(); }
            frmPorosit.ShowDialog();

            GC.Collect();
        }

        private void btn_LEVIZJE_Click(object sender, EventArgs e)
        {
            new rptLEVIZJET_HEADER().ShowDialog();
            GC.Collect();
        }

        private void picPronet_Click(object sender, EventArgs e)
        {

        }

        private void btn_Krijimi_Porosive_Click(object sender, EventArgs e)
        {
            new frm_Gjenerimi_Porosive().ShowDialog();
        }

        private void frmMeny_KeyDown(object sender, KeyEventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            frmFiskalizimi fiskalizimi = new frmFiskalizimi();
            fiskalizimi.Show();
            Cursor.Current = Cursors.Default;
        }

    }
}