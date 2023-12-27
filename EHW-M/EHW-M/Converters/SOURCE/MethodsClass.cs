using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
//using PsionTeklogix.Serial;
using PnReports;

// using System.Windows.Forms;


namespace MobileSales
{      
        

    class Methods
    {   //thirrja e fajllave dll
        [System.Runtime.InteropServices.DllImport("coredll.dll")]
        private static extern bool EnableWindow(IntPtr hwnd, bool bEnable);

        [System.Runtime.InteropServices.DllImport("coredll.dll")]
        private static extern IntPtr FindWindow(string className, string windowName); 


        /* Metode e cila kthen: True  nese DataGrid eshte boshe 
                                False nese DataGrid eshte jo boshe
           duke u bazuar ne DataSet si dhe tablen perkates         */
        public static Boolean isNontNull(DataSet myDataSetName, string myTableName)
        {
            Boolean result =false ;
            int rows = myDataSetName.Tables[myTableName].Rows.Count;
            for (int i = 0; i < rows; i++)
            {
                if (myDataSetName.Tables[myTableName].Rows[i][0].ToString() == "")
                {
                    result = true;
                }

                else
                {
                    result = false;
                }
            }

            return result;
        }

        /* Metoda e cila kthene vleren e nje qelize(permbajtaje) 
         * bazuar ne selektimin e rreshtit dhe( Numrin e kolones ) 
         *                                    (    ColumNumber   )*/ 
        public static int IDVizita(DataSet myDataSetName ,DataGrid myDataGridName, string myTableName,int ColumNumber)
        {
            int row = myDataGridName.CurrentRowIndex;
            int ID =0;
            int id = Convert.ToInt32(myDataSetName.Tables[myTableName].Rows[row][ColumNumber]);
            if (id != 0)
            {
                ID=id;
            }
            return ID;
         }



        /* Metoda kontrollone nese ne nje Tabel ekziston artikulli me emrin item
                                     True nese ekziston
                                     False  nese nuk ekziston
         */
        public static Boolean isInTable( DataSet myDataSetName, string myTambleName, string item , string columnName)
        {
            Boolean result = false;
          //  Boolean state = isNontNull(myDataSetName, myTambleName);
            int nrRows = myDataSetName.Tables[myTambleName].Rows.Count;

           
                for (int i = 0; i < nrRows; i++)
                {
                    if (item == myDataSetName.Tables[myTambleName].Rows[i][1].ToString())
                    { result = true; }
                }

            

            return result;


        }


        public static Boolean isInTable2( DataSet myDataSetName, string myTambleName, string item, string columnName)
        {
            Boolean result = false;
            //  Boolean state = isNontNull(myDataSetName, myTambleName);
            int nrRows = myDataSetName.Tables[myTambleName].Rows.Count;
            for (int i = 0; i < nrRows; i++)
            {
                if (item == myDataSetName.Tables[myTambleName].Rows[i][columnName].ToString())
                { result = true; }
            }
             return result;


        }

        /* TotBill llogarite toatlin momentale te fatures, bazuar ne: initialize - vlera momentale
         *                                                            Cname      - emrin e kolones ne te cilen gjendet cmimi i artikullit ne datagrid
         *                                                            Row        - numri i rreshti te artikullit te fundit te shtuar
         *                                                            myDataGrid - emri i datagridit me te cilin punohet
         *                                                            Piece      - Nr. copeve te artikullit
         *                                                            Price      - Cmimi per cope(pako)
         */         
        public static double TotBill(double initialize, String CName, int Row,  DataGrid myDataGrid, int Piece , double Price)
        {
            double Total = 0.0;
            Total = initialize + Piece * Price;
            return Total;
        }


        //public static double Price(int Piece , int Pack , String ItemName ,  )
        //{
        //    double result = 0.0; 
            
        //}

        public static bool SetTaskBarEnabled(bool bEnabled)
        {
            IntPtr taskWindow = FindWindow("HHTaskBar", null);
            if (taskWindow == IntPtr.Zero)
            {
                taskWindow = FindWindow("Shell_TrayWnd", null);
            }
            if (taskWindow != IntPtr.Zero)
            {
                if (bEnabled)
                    return EnableWindow(taskWindow, true);
                else
                    return EnableWindow(taskWindow, false);

            }
            return false;



        }

    }
    
}
