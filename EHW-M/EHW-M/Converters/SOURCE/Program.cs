using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using PnUtils;
using System.Data.SqlServerCe;
using Microsoft.WindowsMobile.Samples.Location;
using System.Drawing;
using System.Collections;
using System.Data;
using System.Diagnostics;


namespace MobileSales
{
	static class Program
  {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

     

        [MTAThread]
		static void Main()
		{
            
			ConfigXpnParams c;
            ConfigXpn.ReadConfiguration(out c);
            string MainConnectionString = "Data Source=" + c.LocalDbPath;            
            
            if (c.LocalDbPassword != "")
            {
                MainConnectionString += "; Password=" + c.LocalDbPassword;
            }

            DbUtils.MainSqlConnection = new SqlCeConnection(MainConnectionString);

            try
            {
                Application.Run(new frmHome());
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message.ToString());
                DbUtils.WriteExeptionErrorLog(ex);
                Application.Exit();
            }
            finally
            {
                Application.Exit();
                try
                {
                    Process thisProcess = Process.GetCurrentProcess();
                    thisProcess.Kill();
                }
                catch { }
            }
		}

      
  }
}