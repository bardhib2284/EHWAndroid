using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Data;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Threading;
using System.Security.Cryptography;
using System.IO;
using Microsoft.WindowsCE.Forms;
using System.Drawing.Imaging;
using MobileSales;
using System.Xml.Serialization;



namespace PnUtils
{

    public class CryptorEngine
    {
        /// <summary>
        /// Encrypt a string using dual encryption method. Return a encrypted cipher Text
        /// </summary>
        /// <param name="toEncrypt">string to be encrypted</param>
        /// <param name="useHashing">use hashing? send to for extra secirity</param>
        /// <returns></returns>
        public static string Encrypt(string toEncrypt, string SecretKey, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);


            //using this key the text will be encrypted using md5 algorithm
            string key = SecretKey;
            //System.Windows.Forms.MessageBox.Show(key);
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        /// <summary>
        /// DeCrypt a string using dual encryption method. Return a DeCrypted clear string
        /// </summary>
        /// <param name="cipherString">encrypted string</param>
        /// <param name="useHashing">Did you use hashing to encrypt this data? pass true is yes</param>
        /// <returns></returns>
        public static string Decrypt(string cipherString, string SecretKey, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            //using this key the text will be decrypted
            string key = SecretKey;

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray, 0, resultArray.Length);
        }
    }

    public class DbUtils
    {
        //public static string ConnString = "";
        public static SqlCeConnection MainSqlConnection = null;
        private static bool CheckConnString()
        {
            bool a = false;
            if (MainSqlConnection != null)
                a = true;

            return a;
        }


        //DATA FILLERS
        /// <summary>
        /// //this fills comboBox and binds it
        /// </summary>
        /// <param name="cmb">ref ComboBox</param>
        /// <param name="DataSource">ref DataSet</param>
        /// <param name="CommandText">Query</param>
        /// <param name="DisplayMember"></param>
        /// <param name="ValueMember"></param>
        /// <returns></returns>
        public static bool FillCombo(ComboBox cmb, DataSet DataSource, string CommandText, string DisplayMember, string ValueMember)
        {
            bool result = false;

            if (!CheckConnString())
            {
                MessageBox.Show("DbUtils: ConnString cannot be null");
                goto a;
            }

            try
            {

                DataSource.Clear();
                SqlCeDataAdapter da = new SqlCeDataAdapter(CommandText, MainSqlConnection);
                try
                {

                    cmb.DisplayMember = DisplayMember;
                    cmb.ValueMember = ValueMember;
                    da.Fill(DataSource);
                    cmb.DataSource = DataSource.Tables[0];

                }
                finally
                {
                    da.Dispose();
                }
                result = true;
            }
            catch (SqlCeException ex)
            {
                DbUtils.WriteSQLCeErrorLog(ex);
                result = false;
            }

        a:
            return result;
        }

        public static bool FillCombo(ComboBox cmb, BindingSource bindingSource, DataSet DataSource, string CommandText, string DisplayMember, string ValueMember)
        {
            bool result = false;

            if (!CheckConnString())
            {
                MessageBox.Show("DbUtils: ConnString cannot be null");
                goto a;
            }

            try
            {

                DataSource.Clear();
                SqlCeDataAdapter da = new SqlCeDataAdapter(CommandText, MainSqlConnection);
                try
                {

                    cmb.DisplayMember = DisplayMember;
                    cmb.ValueMember = ValueMember;
                    da.Fill(DataSource);
                    bindingSource.DataSource = DataSource.Tables[0];
                    cmb.DataSource = bindingSource;

                }
                finally
                {
                    da.Dispose();
                }
                result = true;
            }
            catch (SqlCeException ex)
            {
                DbUtils.WriteSQLCeErrorLog(ex);
                result = false;
            }
        a:
            return result;
        }

        public static bool FillListBox(ListBox lb, DataSet DataSource, string CommandText, string DisplayMember, string ValueMember)
        {
            bool result = false;

            if (!CheckConnString())
            {
                MessageBox.Show("DbUtils: ConnString cannot be null");
                goto a;
            }

            try
            {

                DataSource.Clear();
                SqlCeDataAdapter da = new SqlCeDataAdapter(CommandText, MainSqlConnection);
                try
                {

                    lb.DisplayMember = DisplayMember;
                    lb.ValueMember = ValueMember;
                    da.Fill(DataSource);
                    lb.DataSource = DataSource.Tables[0];

                }
                finally
                {
                    da.Dispose();
                }
                result = true;
            }
            catch (SqlCeException ex)
            {
                DbUtils.WriteSQLCeErrorLog(ex);
                result = false;
            }
        a:
            return result;
        }

        public static bool FillListBox(ListBox lb, BindingSource bindingSource, DataSet DataSource, string CommandText, string DisplayMember, string ValueMember)
        {
            bool result = false;

            if (!CheckConnString())
            {
                MessageBox.Show("DbUtils: ConnString cannot be null");
                goto a;
            }

            try
            {

                DataSource.Clear();
                SqlCeDataAdapter da = new SqlCeDataAdapter(CommandText, MainSqlConnection);
                try
                {

                    lb.DisplayMember = DisplayMember;
                    lb.ValueMember = ValueMember;
                    da.Fill(DataSource);
                    bindingSource.DataSource = DataSource.Tables[0];
                    lb.DataSource = bindingSource;

                }
                finally
                {
                    da.Dispose();
                }
                result = true;
            }
            catch (SqlCeException ex)
            {
                DbUtils.WriteSQLCeErrorLog(ex);
                result = false;
            }
        a:
            return result;
        }

        /// <summary>
        /// This function fills given dataSet, so it must be referenced existing untyped dataSet ie. ref dsClients
        /// </summary>
        /// <param name="ds">ref DataSet</param>
        /// <param name="CommandText">Query</param>
        /// <returns></returns>

        public static bool FillDataSet(DataSet ds, string CommandText)
        {
            bool result = false;

            if (!CheckConnString())
            {
                MessageBox.Show("DbUtils: ConnString cannot be null");
                goto a;
            }

            try
            {
                ds.Clear();
                SqlCeDataAdapter da = new SqlCeDataAdapter(CommandText, MainSqlConnection);
                try
                {
                    da.Fill(ds);
                }
                finally
                {
                    da.Dispose();
                }
                result = true;
            }
            catch (SqlCeException ex)
            {
                result = false;
                MessageBox.Show(ex.Message);
            }
        a:
            return result;
        }

        /// <summary>
        /// This function fills given dataSet and binds it with a bindingSource
        /// </summary>
        /// <param name="ds">ref DataSet</param>
        /// <param name="CommandText">Query</param>
        /// <returns></returns>

        public static bool FillDataSet(BindingSource bindingSource, DataSet ds, string CommandText)
        {
            bool result = false;

            if (!CheckConnString())
            {
                MessageBox.Show("DbUtils: ConnString cannot be null");
                goto a;
            }

            try
            {
                ds.Clear();
                SqlCeDataAdapter da = new SqlCeDataAdapter(CommandText, MainSqlConnection);
                try
                {
                    da.Fill(ds);
                    bindingSource.DataSource = ds.Tables[0];
                }
                finally
                {
                    da.Dispose();
                }
                result = true;
            }
            catch (SqlCeException ex)
            {
                DbUtils.WriteSQLCeErrorLog(ex);
                result = false;
            }
        a:
            return result;
        }

        /// <summary>
        /// Fills DataSet and Binds it with bindingSource, and return type is SlqDataAdapter that filled sources
        /// </summary>
        /// <param name="bindingSource"></param>
        /// <param name="ds"></param>
        /// <param name="CommandText"></param>
        /// <returns></returns>
        public static SqlCeDataAdapter FillDataSetOnce(BindingSource bindingSource, DataSet ds, string CommandText)
        {
            SqlCeDataAdapter da = null;

            if (!CheckConnString())
            {
                MessageBox.Show("DbUtils: ConnString cannot be null");
                goto a;
            }

            try
            {
                ds.Clear();
                da = new SqlCeDataAdapter(CommandText, MainSqlConnection);

                da.Fill(ds);
                bindingSource.DataSource = ds.Tables[0];

            }
            catch (SqlCeException ex)
            {
                DbUtils.WriteSQLCeErrorLog(ex);
                da = null;
            }
        a:

            return da;
        }

        public static bool FillDataTable(DataTable dt, string CommandText)
        {
            bool result = false;

            if (!CheckConnString())
            {
                MessageBox.Show("DbUtils: ConnString cannot be null");
                goto a;
            }

            try
            {
                dt.Clear();
                SqlCeDataAdapter da = new SqlCeDataAdapter(CommandText, MainSqlConnection);
                try
                {
                    da.Fill(dt);
                }
                finally
                {
                    da.Dispose();
                }
                result = true;
            }
            catch (SqlCeException ex)
            {
                DbUtils.WriteSQLCeErrorLog(ex);
                result = false;
            }
        a:
            return result;
        }

        public static void FillGrid(DataSet ds, DataGrid GridView, string CommandText)
        {


            if (!CheckConnString())
            {
                MessageBox.Show("DbUtils: ConnString cannot be null");
                return;
            }

            try
            {
                ds.Clear();
                SqlCeDataAdapter da = new SqlCeDataAdapter(CommandText, MainSqlConnection);
                try
                {
                    da.Fill(ds);
                    //GridView. = AutoGenerateColumns;
                    GridView.DataSource = ds.Tables[0];
                }
                finally
                {
                    da.Dispose();
                }
            }
            catch (SqlCeException ex)
            {
                MessageBox.Show("Gabim në ekzekutim: " + ex.Message);
            }


        }

        public static void FillGrid(BindingSource bindingSource, DataSet ds, DataGrid GridView, string CommandText)
        {


            if (!CheckConnString())
            {
                MessageBox.Show("DbUtils: ConnString cannot be null");
                return;
            }

            try
            {
                ds.Clear();
                SqlCeDataAdapter da = new SqlCeDataAdapter(CommandText, MainSqlConnection);
                try
                {
                    da.Fill(ds);
                    bindingSource.DataSource = ds.Tables[0];
                    //GridView.AutoGenerateColumns = AutoGenerateColumns;
                    GridView.DataSource = bindingSource;

                }
                finally
                {
                    da.Dispose();
                }
            }
            catch (SqlCeException ex)
            {
                MessageBox.Show("Gabim në ekzekutim: " + ex.Message);
            }


        }

        //CONTROL BINDERS
        public static bool BindControl(BindingSource srcBindinSource, Control ControlName, string FieldName)
        {
            bool a = false;
            try
            {
                ControlName.DataBindings.Clear();
                ControlName.DataBindings.Add("Text", srcBindinSource, FieldName);

                a = true;
            }
            catch (Exception ex)
            {
                DbUtils.WriteExeptionErrorLog(ex);
                a = false;
            }

            return a;
        }

        /// <summary>
        /// This functions Binds Controls(ie text property of a TextBox) to a BindingSource,
        /// assuming that FieldName is Name of control without prefix IE. txtName : Fieldname = Name
        /// </summary>
        /// <param name="srcBindinSource">BindingSource that contains data</param>
        /// <param name="Controls">Controls as parameters ie. (srcData,txtName,txtLastName)</param>
        /// <returns></returns>
        public static void BindControls(BindingSource srcBindinSource, params Control[] Controls)
        {

            for (int i = 0; i < Controls.Length; i++)
            {

                try
                {
                    Controls[i].DataBindings.Clear();
                    Controls[i].DataBindings.Add("Text", srcBindinSource, Controls[i].Name.Substring(3));
                }
                catch (Exception)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// This functions Binds Controls(ie text property of a TextBox) to a BindingSource,
        /// assuming that FieldName is Name of control without prefix IE. txtName : Fieldname = Name
        /// </summary>
        /// <param name="srcBindinSource">BindingSource that contains data</param>
        /// <param name="Controls">ParentContainer, ie. (srcData,grbxCustomer)</param>
        /// <returns></returns>
        public static void BindControls(BindingSource srcBindinSource, Control ParentContainer)
        {
            for (int i = 0; i < ParentContainer.Controls.Count; i++)
            {

                try
                {
                    ParentContainer.Controls[i].DataBindings.Clear();
                    ParentContainer.Controls[i].DataBindings.Add("Text", srcBindinSource, ParentContainer.Controls[i].Name.Substring(3));
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }



        public static List<string> GetFieldNames(DataSet ds)
        {

            List<string> a = new List<string>();

            try
            {
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    a.Add(ds.Tables[0].Columns[i].ColumnName);
                }
            }
            catch (Exception ex)
            {
                DbUtils.WriteExeptionErrorLog(ex);

            }
            return a;
        }

        public static bool ExecSqlSeperator(string CommandText, char seperator)
        {
            bool a = false;

            if (!CheckConnString())
            {
                //MessageBox.Show("DbUtils: ConnString cannot be null");
                goto s;
            }


            string cmdErrorText = string.Empty;
            SqlCeCommand cmd = new SqlCeCommand(CommandText, MainSqlConnection);
            try
            {
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                foreach (string s in CommandText.Split(seperator))
                {
                    cmdErrorText = s;
                    cmd.CommandText = s;
                    if (s.Trim().Length > 7)
                        cmd.ExecuteNonQuery();
                }
                cmd.Connection.Close();
                a = true;
            }
            catch (SqlCeException ex)
            {
                a = false;
                if (ex.NativeError == 2601 || ex.NativeError == 2627)
                {
                    //MessageBox.Show("Keni futur duplikat të ID Numrit në bazën e shënemieve." + (char)13 + "Modifikoni ID Numrin");
                }
                else
                {
                    //MessageBox.Show("Gabim ne ekzekutim:" + ex.Message);
                }
                WriteSQLCeErrorLog(ex, cmdErrorText);
            }
            finally
            {
                cmd.Dispose();
            }
        s:
            return a;
        }

        public static bool ExecSql(string CommandText)
        {
            bool a = false;

            if (!CheckConnString())
            {
                MessageBox.Show("DbUtils: ConnString cannot be null");
                goto s;
            }


            SqlCeCommand cmd = new SqlCeCommand(CommandText, MainSqlConnection);
            try
            {
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                a = true;
            }
            catch (SqlCeException ex)
            {
                a = false;
                if (ex.NativeError == 2601 || ex.NativeError == 2627)
                {
                    MessageBox.Show("Keni futur duplikat të ID Numrit në bazën e shënemieve." + (char)13 + "Modifikoni ID Numrin");
                }
                else
                    MessageBox.Show("Gabim ne ekzekutim:" + ex.Message);

                WriteSQLCeErrorLog(ex, CommandText);
            }
            finally
            {
                cmd.Dispose();
            }
        s:
            return a;
        }



        public static void WriteSQLCeErrorLog(SqlCeException ex, string sqlcmdtext)
        {
            string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            string fullyPath = CurrentDir + @"\Logs";
            if (!Directory.Exists(fullyPath)) Directory.CreateDirectory(fullyPath);
            StreamWriter LogText = new StreamWriter(fullyPath + "\\SQLCeErrorLog.txt", true);


            LogText.WriteLine("DT:" + DateTime.Now.ToString());
            LogText.WriteLine("Message:" + ex.Message);
            LogText.WriteLine("InnerException" + ex.InnerException);
            LogText.WriteLine("Source" + ex.Source);
            LogText.WriteLine("StackTrace" + ex.StackTrace);
            LogText.WriteLine("sqlcmdtext=" + sqlcmdtext);
            LogText.WriteLine("");
            LogText.Close();
            LogText.Dispose();

        }

        public static void WriteSQLCeErrorLog(SqlCeException ex)
        {

            string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            string fullyPath = CurrentDir + @"\Logs";
            if (!Directory.Exists(fullyPath)) Directory.CreateDirectory(fullyPath);
            StreamWriter LogText = new StreamWriter(fullyPath + "\\SQLCeErrorLog.txt", true);


            LogText.WriteLine("DT:" + DateTime.Now.ToString());
            LogText.WriteLine("Message:" + ex.Message);
            LogText.WriteLine("InnerException" + ex.InnerException);
            LogText.WriteLine("Source" + ex.Source);
            LogText.WriteLine("StackTrace" + ex.StackTrace);
            LogText.WriteLine("");
            LogText.Close();
            LogText.Dispose();

        }
        public static void WriteErrorLog(string ex)
        {

            string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            string fullyPath = CurrentDir + @"\Logs";
            if (!Directory.Exists(fullyPath)) Directory.CreateDirectory(fullyPath);
            StreamWriter LogText = new StreamWriter(fullyPath + "\\ErrorLog.txt", true);


            LogText.WriteLine("DT:" + DateTime.Now.ToString());
            LogText.WriteLine("Message:" + ex);

            LogText.WriteLine("");
            LogText.Close();
            LogText.Dispose();

        }

        public static void WriteQueryLog(string queryString)
        {

            string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            string fullyPath = CurrentDir + @"\Logs";
            if (!Directory.Exists(fullyPath)) Directory.CreateDirectory(fullyPath);
            StreamWriter LogText = new StreamWriter(fullyPath + "\\QueryLog.txt", true);


            LogText.WriteLine("DT:" + DateTime.Now.ToString());
            LogText.WriteLine("Message:" + queryString);

            LogText.WriteLine("");
            LogText.Close();
            LogText.Dispose();

        }

        public static void SqlCeExceptionHandling(SqlCeException e)
        {
            string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            StreamWriter LogText = new StreamWriter(CurrentDir + "\\SQLCeErrorLog.txt", true);

            SqlCeErrorCollection errorCollection = e.Errors;
            StringBuilder bld = new StringBuilder();
            Exception inner = e.InnerException;

            foreach (SqlCeError err in errorCollection)
            {
                bld.Append("\n Error Code: " + err.HResult.ToString("X"));
                bld.Append("\n Message : " + err.Message);
                bld.Append("\n Minor Err.: " + err.NativeError);
                bld.Append("\n Source : " + err.Source);
                foreach (int numPar in err.NumericErrorParameters)
                {
                    if (0 != numPar) bld.Append("\n Num. Par. : " + numPar);
                }
                foreach (string errPar in err.ErrorParameters)
                {
                    if (String.Empty != errPar) bld.Append("\n Err. Par. : " +
                    errPar);
                }

            }

            LogText.WriteLine(bld.ToString());
            LogText.Close();
            LogText.Dispose();
        }

        public static void WriteExeptionErrorLog(Exception ex)
        {
            string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            string fullyPath = CurrentDir + @"\Logs";
            if (!Directory.Exists(fullyPath)) Directory.CreateDirectory(fullyPath);
            StreamWriter LogText = new StreamWriter(fullyPath + "\\ExceptionErrorLog.txt", true);


            LogText.WriteLine("DT:" + DateTime.Now.ToString());
            LogText.WriteLine("Message:" + ex.Message);
            LogText.WriteLine("InnerException" + ex.InnerException);
            LogText.WriteLine("StackTrace" + ex.StackTrace);
            LogText.WriteLine("");
            LogText.Close();
            LogText.Dispose();

        }

        public static bool CheckSQLCeErrorLog()
        {
            bool result = true;
            string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            StreamReader LogReader = null;
            string strbuf = "";
            try
            {
                LogReader = new StreamReader(CurrentDir + @"\Logs" + "\\SQLCeErrorLog.txt");
                strbuf = LogReader.ReadToEnd();

                if (strbuf != "")
                    if (strbuf.Length < 2)
                        result = false;


            }
            catch (Exception)
            {
                if (LogReader == null)
                    result = true;
                else
                    result = false;
            }
            finally
            {
                if (LogReader != null)
                {
                    LogReader.Close();
                    LogReader.Dispose();
                }
            }

            return result;

        }

        public static void UpateMalli_Mbetur_SHITUR()
        {
            DataSet _dsLiferimiART = null;
            string _selectQuery = "";


            SqlCeConnection con = null;
            SqlCeCommand cmd = null;
            try
            {
                con = new SqlCeConnection(frmHome.AppDBConnectionsStr);
                cmd = new SqlCeCommand("UPDATE TEST", con);
                con.Open();
                cmd.Transaction = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;

                _dsLiferimiART = new DataSet();
                _selectQuery = @"SELECT ROUND(SUM(la.SasiaLiferuar),3) as SumShitur,
                                           la.IDArtikulli
                                    FROM   LiferimiArt la WHERE la.SasiaLiferuar>0
                                    GROUP BY
                                           la.IDArtikulli";

                PnUtils.DbUtils.FillDataSet(_dsLiferimiART, _selectQuery);

                if (_dsLiferimiART.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < _dsLiferimiART.Tables[0].Rows.Count; i++)
                    {
                        string _idArtikulli = _dsLiferimiART.Tables[0].Rows[i]["IDArtikulli"].ToString();
                        double _SasiaShitur = double.Parse(_dsLiferimiART.Tables[0].Rows[i]["SumShitur"].ToString());

                        cmd.CommandText = @"Update Malli_Mbetur
                                            SET 
                                            SasiaShitur =@SasiaShitur, SyncStatus=0 where IDArtikulli=@IDArtikulli";
                        cmd.Parameters.AddWithValue("@SasiaShitur", _SasiaShitur);
                        cmd.Parameters.AddWithValue("IDArtikulli", _idArtikulli);

                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();//Release parameters

                    }

                    //Finally Applies pending changes to the underlying data

                    cmd.Transaction.Commit();
                }
            }
            catch (SqlCeException ex)
            {
                cmd.Transaction.Rollback();
                cmd.Connection.Close();
                WriteSQLCeErrorLog(ex, cmd.CommandText);
            }

            finally
            {
                _dsLiferimiART.Dispose();
                cmd.Connection.Close();
                con.Dispose();
                cmd.Dispose();
            }
        }

        public static void UpdateMalli_Mbetur_KTHYER()
        {
            DataSet _dsLiferimiArt = null;
            string _selectQuery = "";
            SqlCeConnection con = null;
            SqlCeCommand cmd = null;
            try
            {
                con = new SqlCeConnection(frmHome.AppDBConnectionsStr);
                cmd = new SqlCeCommand("UPDATE TEST", con);
                con.Open();
                cmd.Transaction = con.BeginTransaction();
                cmd.CommandType = CommandType.Text;

                _dsLiferimiArt = new DataSet();



                _selectQuery = @"SELECT ROUND(SUM(la.SasiaLiferuar),3) as SumKthyer,
                                           la.IDArtikulli
                                    FROM   LiferimiArt la WHERE la.SasiaLiferuar<0
                                    GROUP BY
                                           la.IDArtikulli";
                PnUtils.DbUtils.FillDataSet(_dsLiferimiArt, _selectQuery);

                if (_dsLiferimiArt.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < _dsLiferimiArt.Tables[0].Rows.Count; i++)
                    {
                        string _idArtikulli = _dsLiferimiArt.Tables[0].Rows[i]["IDArtikulli"].ToString();
                        double _SasiaKthyer = double.Parse(_dsLiferimiArt.Tables[0].Rows[i]["SumKthyer"].ToString());
                        cmd.CommandText = @"Update Malli_Mbetur
                                            SET SasiaKthyer =@SasiaKthyer, SyncStatus=0
                                            where IDArtikulli=@IDArtikulli";
                        cmd.Parameters.AddWithValue("@SasiaKthyer", _SasiaKthyer);
                        cmd.Parameters.AddWithValue("@IDArtikulli", _idArtikulli);

                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();//Release parameters
                    }

                    //Finally Applies pending changes to the underlying data
                    cmd.Transaction.Commit();
                }



            }
            catch (SqlCeException ex)
            {
                cmd.Transaction.Rollback();
                WriteSQLCeErrorLog(ex, cmd.CommandText);
            }
            finally
            {
                _dsLiferimiArt.Dispose();
                cmd.Connection.Close();
                con.Dispose();
                cmd.Dispose();
            }

        }

        public static void UpdateMalli_Mbetur_Mbetur()
        {
            string _UpdateQuery = "";
            try
            {

                _UpdateQuery = @"UPDATE Malli_Mbetur
                 SET
                 SasiaMbetur = Round((SasiaPranuar - SasiaShitur -SasiaKthyer),3), SyncStatus=0 ";
                PnUtils.DbUtils.ExecSql(_UpdateQuery);

            }
            catch
                 (SqlCeException ex)
            {

                MessageBox.Show(ex.Message);
                WriteSQLCeErrorLog(ex, _UpdateQuery);
            }




        }


        public static string ExecSqlScalar(string CommandText)
        {
            string a = "";

            if (!CheckConnString())
            {
                MessageBox.Show("DbUtils: ConnString cannot be null");
                goto s;
            }


            SqlCeCommand cmd = new SqlCeCommand(CommandText, MainSqlConnection);
            try
            {
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
                a = cmd.ExecuteScalar().ToString();
                cmd.Connection.Close();

            }
            catch (SqlCeException ex)
            {
                a = "error";
                if (ex.NativeError == 2601 || ex.NativeError == 2627)
                {
                    MessageBox.Show("Keni futur duplikat të ID Numrit në bazën e shënemieve." + (char)13 + "Modifikoni ID Numrin");
                }
                else
                    MessageBox.Show("Gabim ne ekzekutim:" + ex.Message);
                WriteSQLCeErrorLog(ex, CommandText);
            }
            finally
            {
                cmd.Dispose();
            }
        s:
            return a;
        }

    }

    public struct DbMessages
    {
        public static string UpdateSuccess = "Shënimi është azhuruar me sukses";
        public static string SaveSuccsess = "Shënimi është ruajtur me sukses";
        public static string DeleteSuccess = "Shënimi është fshirë me sukses";
        public static string ConfirmDelete = "A jeni të sigurtë që doni ta fshini shënimin e zgjedhur?";
        public static string ConfirmSave = "A jeni të sigurtë që doni ta ruani shënimin e zgjedhur?";
        public static string ConfirmUpdate = "A jeni të sigurtë që doni ta azhuroni shënimin e zgjedhur?";
        public static string RequiredFields = "Ju lutem, plotësoni fushat e kërkuara";

    }

    public class Ctrl
    {

        /// <summary>
        /// Validates controls TextBoxes,MaskedTextBoxes,RadioButtons,ComboBoxes (RedLight color if nothing selcted or entered)
        /// </summary>
        /// <param name="Controls"></param>
        /// <returns></returns>
        /// 
        public static bool ValidateControls(params Control[] Controls)
        {
            bool result = true;


            for (int i = 0; i < Controls.Length; i++)
            {
                if (Controls[i] is TextBox)
                {
                    if ((Controls[i] as TextBox).Text == "")
                    {
                        Controls[i].BackColor = Color.Salmon;
                        result = false;
                    }
                    else
                    {
                        Controls[i].BackColor = Color.Empty;
                    }
                }


                if (Controls[i] is RadioButton)
                {
                    if (!(Controls[i] as RadioButton).Checked)
                    {
                        Controls[i].BackColor = Color.Salmon;
                        result = false;
                    }
                    else
                    {
                        (Controls[i] as RadioButton).BackColor = Color.Empty;
                    }
                }

                if (Controls[i] is ComboBox)
                {
                    if ((Controls[i] as ComboBox).SelectedItem == null)
                    {
                        Controls[i].BackColor = Color.Salmon;
                        result = false;
                    }
                    else
                    {
                        Controls[i].BackColor = Color.Empty;
                    }
                }

                if (Controls[i] is CheckBox)
                {
                    if (!(Controls[i] as CheckBox).Checked)
                    {
                        Controls[i].BackColor = Color.Salmon;
                        result = false;
                    }
                    else
                    {
                        (Controls[i] as CheckBox).BackColor = Color.Empty;
                    }
                }


                //if (Controls[i] is DateTimePicker)
                //{
                //    if ((Controls[i] as DateTimePicker).Value.ToString("dd/MM/yyyy") == "01/01/1989")
                //    {
                //        result = false;
                //    }
                //}
            }

            return result;
        }

        public static bool EnableControls(bool Enable, params Control[] Controls)
        {
            bool result = true;


            for (int i = 0; i < Controls.Length; i++)
            {
                Controls[i].Enabled = Enable;
            }

            return result;
        }

        public static bool ValidateAllControls(Control Container)
        {
            bool result = true;




            for (int i = 0; i < Container.Controls.Count; i++)
            {


                if (Container.Controls[i].Controls.Count > 0)
                {
                    ValidateAllControls(Container.Controls[i]);
                }

                if (Container.Controls[i] is TextBox)
                {
                    if ((Container.Controls[i] as TextBox).Text == "")
                    {
                        Container.Controls[i].BackColor = Color.Salmon;
                        result = false;
                    }
                    else
                    {
                        Container.Controls[i].BackColor = Color.Empty;
                    }
                }



                if (Container.Controls[i] is RadioButton)
                {
                    if (!(Container.Controls[i] as RadioButton).Checked)
                    {
                        Container.Controls[i].BackColor = Color.Salmon;
                        result = false;
                    }
                    else
                    {
                        (Container.Controls[i] as RadioButton).BackColor = Color.Empty;
                    }
                }

                if (Container.Controls[i] is ComboBox)
                {
                    if ((Container.Controls[i] as ComboBox).SelectedItem == null)
                    {
                        Container.Controls[i].BackColor = Color.Salmon;
                        result = false;
                    }
                    else
                    {
                        Container.Controls[i].BackColor = Color.Empty;
                    }
                }

                //if (Controls[i] is DateTimePicker)
                //{
                //    if ((Controls[i] as DateTimePicker).Value.ToString("dd/MM/yyyy") == "01/01/1989")
                //    {
                //        result = false;
                //    }
                //}
            }

            return result;
        }

        /// <summary>
        /// Return BckColor of control to Default
        /// </summary>
        /// <param name="Controls"></param>
        public static void InvalidateControls(params Control[] Controls)
        {
            for (int i = 0; i < Controls.Length; i++)
            {
                Controls[i].BackColor = Color.Empty;

                if (Controls[i] is TextBox)
                    (Controls[i] as TextBox).Text = "";

                if (Controls[i] is ComboBox)
                    (Controls[i] as ComboBox).SelectedIndex = -1;

                if (Controls[i] is DateTimePicker)
                    (Controls[i] as DateTimePicker).Value = DateTime.Now;
            }
        }

        public static void CleanUpControls(Control Container)
        {
            for (int i = 0; i < Container.Controls.Count; i++)
            {
                if (Container.Controls[i].Controls.Count > 0)
                    CleanUpControls(Container.Controls[i]);

                if ((Container.Controls[i] is TextBox) || (Container.Controls[i] is ComboBox))
                {
                    Container.Controls[i].Text = "";
                }
                else
                    if (Container.Controls[i] is DateTimePicker)
                    {
                        (Container.Controls[i] as DateTimePicker).Value = DateTime.Now;
                    }
                    else
                        if (Container.Controls[i] is ListBox)
                        {
                            (Container.Controls[i] as ListBox).Items.Clear();
                        }
                        else if (Container.Controls[i] is ListView)
                        {
                            (Container.Controls[i] as ListView).Items.Clear();
                        }
            }
        }

        public static string replaceUnSupportedChar(string _input)
        {
            string _output;
            _output = _input.Replace('ç', 'c');
            _output = _input.Replace('Ç', 'C');
            _output = _input.Replace('ë', 'e');
            _output = _input.Replace('Ë', 'E');

            return _output;
        }
        /// <summary>
        /// Llogarite shumen e vlerave ne nje kolene, dhe rrezultatin e shfaq ne label
        /// </summary>
        /// <param name="labName">labela ku do te shfaqet rrezultati</param>
        /// <param name="ColIndex">indeksi i kolones ne DataSet</param>
        /// <param name="_dataSet">emri i DataSet-it prej nga do te mbushte Gridi</param>
        public static void CalcSUM(Label labName, int ColIndex, DataSet _dataSet)
        {
            double SUM = 0;
            int numRows = int.Parse(_dataSet.Tables[0].Rows.Count.ToString());
            if (numRows > 0)
            {
                for (int j = 0; j < numRows; j++)
                {
                    double _tmpSum = Convert.ToDouble(_dataSet.Tables[0].Rows[j][ColIndex].ToString());

                    SUM = SUM + Convert.ToDouble(_dataSet.Tables[0].Rows[j][ColIndex].ToString());


                }
                labName.Text = String.Format("{0:0.00 }", SUM);

            }
            else
            {
                labName.Text = "0.00";
            }
        }

        public static bool IsInteger(TextBox InputTextBox)
        {
            bool res = false;

            try
            {
                Convert.ToInt32(InputTextBox.Text);
                InputTextBox.BackColor = Color.Empty;
                res = true;
            }
            catch (Exception ex)
            {
                DbUtils.WriteExeptionErrorLog(ex);
                InputTextBox.BackColor = Color.Salmon;
                res = false;
            }

            return res;
        }

        public static bool IsInteger(string InputTextBox)
        {
            bool res = false;

            try
            {
                Convert.ToInt32(InputTextBox);
                res = true;
            }
            catch (Exception)
            {
                res = false;
            }

            return res;
        }

        public static bool IsDecimal(TextBox InputTextBox)
        {
            bool res = false;

            try
            {
                InputTextBox.Text = InputTextBox.Text.Replace(",", ".");
                Convert.ToDecimal(InputTextBox.Text);
                InputTextBox.BackColor = Color.Empty;
                res = true;
            }
            catch (Exception)
            {
                InputTextBox.BackColor = Color.Salmon;
                res = false;
            }

            return res;
        }

        public static bool IsDecimal(string InputTextBox)
        {
            bool res = false;

            try
            {
                InputTextBox = InputTextBox.Replace(",", ".");
                Convert.ToDecimal(InputTextBox);
                res = true;
            }
            catch (Exception)
            {
                res = false;
            }

            return res;
        }
    }

    public static class Serlialization
    {
        public static string SerializeObjectToString<T>(T toSerialize)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());
                using (StringWriter textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, toSerialize);
                    return textWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                DbUtils.WriteExeptionErrorLog(ex);
            }
            return "";
        }

        public static T DeserializeObject<T>(string xmlString) where T : class
        {
            try
            {
                using (TextReader reader = new StringReader(xmlString))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                DbUtils.WriteExeptionErrorLog(ex);
            }
            return null;
        }

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    try
                    {

                        if (pro.Name == column.ColumnName)
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        else
                            continue;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            }
            return obj;
        }
    }
}
