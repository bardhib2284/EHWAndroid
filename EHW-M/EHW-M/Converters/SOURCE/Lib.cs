using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Data.SqlServerCe;
using System.Threading;
using System.IO;

/******************************************************************************/
/* Library functions for the pr sample programs.                              */
/******************************************************************************/
namespace MobileSales
{
    /******************************************************************************/
    class Lib
    {
        [System.Runtime.InteropServices.DllImport("msvcrt.dll")]
        private static extern int _kbhit();

        private int _loop_count = 0;
        private int _err_count = 0;
        private long _process_avg = 0;

        private long _start_, _stop_;
        private long _process_start_, _process_stop_;

        /**************************************************************************/
        /* Detects keyboard events.                                               */
        /**************************************************************************/
        public bool KbHit()
        {
            return _kbhit() != 0;
        }

        /**************************************************************************/
        /* Displays the string and flushes the output stream.                     */
        /**************************************************************************/
        public void Write(string str)
        {
            string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            StreamWriter LogText = new StreamWriter(CurrentDir + "\\ActivityLog.txt", true);
            
            LogText.Write(str);
            LogText.Close();
            LogText.Dispose();

        }

        /**************************************************************************/
        /* Displays the string.                                                   */
        /**************************************************************************/
        public void WriteLine(string str)
        {
            string CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            StreamWriter LogText = new StreamWriter(CurrentDir + "\\ActivityLog.txt", true);

            LogText.WriteLine(str);
            LogText.Close();
            LogText.Dispose();
        }

        /**************************************************************************/
        /* Reads a string from the standard input.                                */
        /**************************************************************************/
        public string ReadLine(string prompt)
        {
            if (prompt != "") Write(prompt);
            return System.Console.In.ReadLine();
        }


        /**************************************************************************/
        /* Displays the error message and exits the program.                      */
        /**************************************************************************/
        public void Error(string str)
        {
            _err_count += 1;
            WriteLine(str);
        }

        /**************************************************************************/
        /* Displays the start message and starts timer.                           */
        /**************************************************************************/
        public void FunctionStart(string str)
        {
            Write(str + "... "+DateTime.Now.Date.ToLocalTime().ToString()+"\n");
            _start_ = System.DateTime.Now.Ticks;
        }

        /**************************************************************************/
        /* Displays the end message and returns the time elapsed in ms.           */
        /**************************************************************************/
        public long FunctionEnd()
        {
            long _function_time;
            _stop_ = System.DateTime.Now.Ticks;
            _function_time = (_stop_ - _start_) / 10000;
            WriteLine("Done. (" + _function_time + " ms)");
            return _function_time;
        }

        /**************************************************************************/
        /* Displays the process start message and starts the process timer.       */
        /**************************************************************************/
        public void ProcessStart(string str)
        {
            _loop_count += 1;
            WriteLine("*** " + str + " [" + _loop_count + "]");
            System.GC.Collect();
            _process_start_ = System.DateTime.Now.Ticks;
        }

        /**************************************************************************/
        /* Displays the end message and returns the time elapsed in ms.           */
        /**************************************************************************/
        public long ProcessEnd()
        {
            _process_stop_ = System.DateTime.Now.Ticks;
            long _process_time = (_process_stop_ - _process_start_) / 10000;
            _process_avg = ((_loop_count - 1) * _process_avg + _process_time) / _loop_count;
            WriteLine("*** Process time: " + _process_time + " ms, avg: " + _process_avg);
            WriteLine("");
            return _process_time;
        }

        /**************************************************************************/
        /* Displays overall statistics.                                           */
        /**************************************************************************/
        public int PrintStat()
        {
            WriteLine("\n*** Statistics ***\n");
            WriteLine("Number of loops : " + _loop_count);
            WriteLine("Number of errors: " + _err_count);
            WriteLine("Process average : " + _process_avg + " ms");
            WriteLine("");
            return _loop_count;
        }

        /**************************************************************************/
        /* Saves binary data in file.                                             */
        /**************************************************************************/
        public bool Save(string fn, byte[] data)
        {
            System.IO.FileStream fw = System.IO.File.Create(fn);
            fw.Write(data, 0, data.Length);
            fw.Close();
            return true;
        }

        /**************************************************************************/
        /* Waits for a time specified in miliseconds.                             */
        /**************************************************************************/
        public void Wait(int ms)
        {
            System.Threading.Thread.Sleep(ms);
        }

        /**************************************************************************/
        /* Waits for n seconds.                                                   */
        /**************************************************************************/
        public void WaitForSec(int sec)
        {
            for (int i = sec; i > 0; i--)
            {
                if (KbHit()) break;
                Write("\rWaiting for " + i + " sec...   ");
                Wait(1000);
            }
            Write("\r                                                             \r");
        }

        /**************************************************************************/
        /* Creates a list of filenames recursively browsing the subdirs too.      */
        /**************************************************************************/
        public System.Collections.ArrayList FileList(string dirname, string mask)
        {
            System.Collections.ArrayList list = new System.Collections.ArrayList();
            try
            {
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(dirname);
                foreach (System.IO.DirectoryInfo d in dir.GetDirectories())
                    list.AddRange(FileList(dir.FullName + "/" + d.Name, mask));
                foreach (System.IO.FileInfo f in dir.GetFiles(mask))
                    list.Add(f.Name);
            }
            catch (System.Exception) { }
            return list;
        }
    };
    /******************************************************************************/
}