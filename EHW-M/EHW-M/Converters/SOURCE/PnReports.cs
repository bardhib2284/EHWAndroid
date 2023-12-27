using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Data;
//using PsionTeklogix.Serial;
using System.IO.Ports;
using System.Text.RegularExpressions;
using PnUtils;

namespace PnReports
{
    #region CommDevice
    public class CommDevice
    {
        // Comm events
        private enum EventFlags : uint
        {
            NONE = 0x0000, //
            RXCHAR = 0x0001, // Any Character received
            RXFLAG = 0x0002, // Received specified flag character
            TXEMPTY = 0x0004, // Tx buffer Empty
            CTS = 0x0008, // CTS changed
            DSR = 0x0010, // DSR changed
            RLSD = 0x0020, // RLSD changed
            BREAK = 0x0040, // BREAK received
            ERR = 0x0080, // Line status error
            RING = 0x0100, // ring detected
            PERR = 0x0200, // printer error
            RX80FULL = 0x0400, // rx buffer is at 80%
            EVENT1 = 0x0800, // provider event
            EVENT2 = 0x1000, // provider event
            POWER = 0x2000, // wince power notification
            ALL = 0x3FFF  // mask of all flags 
        }

        // Device control block structure
        private class DeviceControlBlock
        {
            public uint BaudRate;
            public byte Size;
            public byte Parity;
            public byte StopBits;
        }

        // Comm timeout
        private class CommTimeout
        {
            public UInt32 ReadIntervalTimeout;
            public UInt32 ReadTotalTimeoutMultiplier;
            public UInt32 ReadTotalTimeoutConstant;
            public UInt32 WriteTotalTimeoutMultiplier;
            public UInt32 WriteTotalTimeoutConstant;
        }

        // Flags
        private enum Flags
        {
            EVENT_PULSE = 1,
            EVENT_RESET = 2,
            EVENT_SET = 3
        }
        #region API declarations
        // CECreateFileW
        [DllImport("coredll.dll", EntryPoint = "CreateFileW", SetLastError = true)]
        private static extern IntPtr CECreateFileW(
            String lpFileName, UInt32 dwDesiredAccess, UInt32 dwShareMode,
            IntPtr lpSecurityAttributes, UInt32 dwCreationDisposition, UInt32 dwFlagsAndAttributes,
            IntPtr hTemplateFile);
        // CESetupComm
        [DllImport("coredll.dll", EntryPoint = "SetupComm", SetLastError = true)]
        private static extern int CESetupComm(IntPtr hFile, UInt32 dwInQueue, UInt32 dwOutQueue);
        // CEWriteFile
        [DllImport("coredll.dll", EntryPoint = "WriteFile", SetLastError = true)]
        private static extern int CEWriteFile(IntPtr hFile, byte[] lpBuffer, UInt32 nNumberOfBytesToRead, ref Int32 lpNumberOfBytesRead, IntPtr lpOverlapped);
        // CESetCommMask
        [DllImport("coredll.dll", EntryPoint = "SetCommMask", SetLastError = true)]
        private static extern int CESetCommMask(IntPtr handle, EventFlags dwEvtMask);
        // CESetCommState
        [DllImport("coredll.dll", EntryPoint = "SetCommState", SetLastError = true)]
        private static extern int CESetCommState(IntPtr hFile, DeviceControlBlock dcb);
        // CESetCommTimeouts
        [DllImport("coredll.dll", EntryPoint = "SetCommTimeouts", SetLastError = true)]
        private static extern int CESetCommTimeouts(IntPtr hFile, CommTimeout timeouts);
        // CEWaitCommEvent
        [DllImport("coredll.dll", EntryPoint = "WaitCommEvent", SetLastError = true)]
        private static extern int CEWaitCommEvent(IntPtr hFile, ref EventFlags lpEvtMask, IntPtr lpOverlapped);
        // CEGetCommModemStatus
        [DllImport("coredll.dll", EntryPoint = "GetCommModemStatus", SetLastError = true)]
        extern private static int CEGetCommModemStatus(IntPtr hFile, ref uint lpModemStat);
        // CECloseHandle
        [DllImport("coredll.dll", EntryPoint = "CloseHandle", SetLastError = true)]
        private static extern int CECloseHandle(IntPtr hObject);
        // CEWaitForSingleObject
        [DllImport("coredll.dll", EntryPoint = "WaitForSingleObject", SetLastError = true)]
        private static extern int CEWaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);
        // CEEventModify
        [DllImport("coredll.dll", EntryPoint = "EventModify", SetLastError = true)]
        private static extern int CEEventModify(IntPtr hEvent, uint function);
        //CECreateEvent
        [DllImport("coredll.dll", EntryPoint = "CreateEvent", SetLastError = true)]
        private static extern IntPtr CECreateEvent(IntPtr lpEventAttributes, int bManualReset, int bInitialState, string lpName);
        #endregion
        private string sPort; // COM port#
        private IntPtr hPort; // COM port handler
        private uint iTransmitBufferSize; // Transmit buffer size
        private uint Acctype; // Access type to COM port
        private byte[] bTransmitBuffer; // Transmit buffer
        private const UInt32 EXISTING = 3; // Open existing port
        private const UInt32 WRITE = 0x40000000; // write only
        private const Int32 INVALID_HANDLE = -1; // Thread exit value
        private const Int32 PACKAGE_SIZE = 1024; // Packet size
        private const string ClosePortEventName = "ClosePort"; // Event name
        private IntPtr ClosePortEvent; // Event handle		
        private ManualResetEvent ThreadEvent = new ManualResetEvent(false); // Thread event
        private CommTimeout commTimeout; // time out
        private DeviceControlBlock dcb;	// Device control block


        // Constructor
        public CommDevice(string Port)
        {
            // Set max transmit & receive buffer sizes
            iTransmitBufferSize = PACKAGE_SIZE;

            // Create instance for device control block and set values
            dcb = new DeviceControlBlock();
            dcb.BaudRate = 9600;
            dcb.Size = 8;
            dcb.Parity = 0;
            dcb.StopBits = 0;

            // Access type
            Acctype = WRITE;

            // Reset close event value
            ClosePortEvent = IntPtr.Zero;

            // Set buffer area for transmit & receive data
            bTransmitBuffer = new byte[iTransmitBufferSize];

            // Create event
            ClosePortEvent = CECreateEvent(IntPtr.Zero, Convert.ToInt32(true), Convert.ToInt32(false), ClosePortEventName);
            sPort = Port;
        }

        // Open COM port
        private IntPtr OpenPort()
        {
            if (sPort.Length != 0)
            {
                // Open COM port
                hPort = CECreateFileW(sPort,
                    Acctype,
                    0,
                    IntPtr.Zero,
                    EXISTING,
                    0,
                    IntPtr.Zero);
            }
            else
            {
                // Port number has not been specified
                hPort = IntPtr.Zero;
            }
            return (hPort);
        }

        // Setup comm
        private void SetupComm()
        {
            // Set buffer sizes
            CESetupComm(hPort,
                        0,
                        iTransmitBufferSize);
        }

        // Set timeout
        private void SetConnectionTimeOut()
        {
            // Create comm timeout instance
            commTimeout = new CommTimeout();

            commTimeout.ReadIntervalTimeout = uint.MaxValue; // Read interval
            commTimeout.ReadTotalTimeoutConstant = 0; // 0 Seconds for write
            commTimeout.ReadTotalTimeoutMultiplier = 0; // 0 Seconds for write
            commTimeout.WriteTotalTimeoutConstant = 5; // 5 Seconds for write
            commTimeout.WriteTotalTimeoutMultiplier = 0; // 0 Seconds for write
        }

        // Set DCB
        private void SetDCB()
        {
            // Set device control block
            CESetCommState(hPort,
                            dcb);
        }

        // Create thread
        private void CreateCommThread()
        {
            // Create thread
            Thread eventThread = new Thread(new ThreadStart(CommThread));
            // Set priority
            eventThread.Priority = ThreadPriority.AboveNormal;
            // Start now
            eventThread.Start();
            // Set wait event
            ThreadEvent.WaitOne();
        }
        // Public initialization method
        public void Init()
        {
            // Initialize COM port
            hPort = OpenPort(); // Open COM port
            SetupComm(); // Set buffer size
            SetDCB(); // Device control block
            SetConnectionTimeOut(); // Timeout settings
            CreateCommThread(); // Create & begin thread
        }
        // Send data to COM port (char array)
        public void Send(char[,] Data)
        {
            int iXPos = 0; // Row
            int iYPos = 0; // Column
            int iXBound = 0; // X
            int iYBound = 0; // Y
            int iCounter = 0; // Byte counter
            int iTotalCounter = 0; // Total count						
            int iIndex; // Clear Output
            int iNumberOfBytesRead = 0;
            uint iNumberOfBytesToRead = PACKAGE_SIZE;
            byte[] Output = new byte[PACKAGE_SIZE]; // Package

            // Get bounds
            iYBound = (Data.GetUpperBound(0)) + 1;
            iXBound = (Data.GetUpperBound(1)) + 1;

            // Parse & locate the data into byte array
            for (iYPos = 0; iYPos != iYBound; iYPos++)
            {
                // Row loop
                for (iXPos = 0; iXPos != iXBound; iXPos++)
                {
                    // Insert data into buffer
                    if (0 != (byte)(Data[iYPos, iXPos]))
                        Output[iCounter] = (byte)(Data[iYPos, iXPos]); // char
                    else
                        Output[iCounter] = 0x20; // Space character
                    iCounter++; // 1024 bytes
                    iTotalCounter++; // As a whole document
                    // Column loop
                    if (iCounter == PACKAGE_SIZE || iTotalCounter == (iXBound * iYBound)) // Check package size
                    {
                        // Send data to COM port
                        CEWriteFile(hPort, Output, iNumberOfBytesToRead, ref iNumberOfBytesRead, IntPtr.Zero);
                        // Clear package
                        iCounter = 0;
                        for (iIndex = 0; iIndex != PACKAGE_SIZE; iIndex++)
                            Output[iIndex] = 0x0;
                    }
                }
                Output[iCounter] = 0xD;
                iCounter++; // 1024 bytes
                Output[iCounter] = 0xA;
                iCounter++; // 1024 bytes
            }

            // Dispose value
            Output = null;

            // Call gc
            GC.Collect();
        }
        // Send data to COM port (byte)
        public void Send(byte Data)
        {
            // Send a byte
            byte[] Output = new byte[1];
            uint iNumberOfBytesToRead = 1;
            int iNumberOfBytesRead = 0;

            // Send data
            Output[0] = Data;
            CEWriteFile(hPort, Output, iNumberOfBytesToRead, ref iNumberOfBytesRead, IntPtr.Zero);

            // Dispose value
            Output = null;

            // Call gc
            GC.Collect();
        }
        // Close port
        public void ClosePort()
        {
            if (Convert.ToBoolean(CECloseHandle(hPort)))
            {
                hPort = (IntPtr)INVALID_HANDLE;
                CEEventModify(ClosePortEvent, (uint)Flags.EVENT_SET);
            }
        }
        // Dispose
        public void Dispose()
        {
            bTransmitBuffer = null;
            commTimeout = null;
            ThreadEvent = null;
        }
        // Properties

        // Port#
        public string Port
        {
            get
            {
                return (sPort);
            }
            set
            {
                sPort = value;
            }
        }
        // Baud
        public uint BaudRate
        {
            get
            {
                return (dcb.BaudRate);
            }
        }
        // Parity
        public int Parity
        {
            get
            {
                return (dcb.Parity);
            }
        }

        // Threading method
        private void CommThread()
        {
            EventFlags commEvents = new EventFlags();

            // Monitor events
            CESetCommMask(hPort, EventFlags.ALL);

            try
            {
                // Thread has been started
                ThreadEvent.Set();

                // Wait for new event
                while (hPort != (IntPtr)INVALID_HANDLE)
                {
                    if (!Convert.ToBoolean(CEWaitCommEvent(hPort, ref commEvents, IntPtr.Zero)))
                    {
                        CEWaitForSingleObject(ClosePortEvent, 1000);
                        ThreadEvent.Reset();
                        return;
                    }
                    CESetCommMask(hPort, EventFlags.ALL);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
    #endregion

    #region PrintEngine
    public class SmartDevicePrintEngine
    {
        // Local variables
        private int iPageWidth; // Page width
        private int iPageHeight; // Page Height
        private int iBottomLine; // Bottom line
        private int iPreviewPageIndex; // Page number
        private string Row; // Single row on logical page
        private char[,] PageBuffer; // Logical page

       // private PsionTeklogix.Serial.SerialPort commDevice; // Device class
        private System.IO.Ports.SerialPort commDeviceMS;
        private System.Collections.ArrayList PageArray; // Array for print preview buffer
        private System.Text.StringBuilder PreviewBuffer; // Preview buffer
        private System.Windows.Forms.Form frmPrintPreview; // Print preview window
        private System.Windows.Forms.TextBox txtPreview; // Print preview textbox
        private System.Windows.Forms.Label lblPageNumber; // Print preview page nuber
        private System.Windows.Forms.Button btnBack; // Create back button
        private System.Windows.Forms.Button btnPrev; // Create previous button
        private System.Windows.Forms.Button btnNext; // Create next button

        // Constructor
        public SmartDevicePrintEngine(PnDevice aPrintPort, string PrintPort)
        {
            // Create COM device
            //if (aPrintPort == PnDevice.MultiplicatorPort)
            //{
            //    commDevice = new PsionTeklogix.Serial.SerialPort(PrintPort);
            //}

            if (aPrintPort == PnDevice.BluetoothPort)
            {
                for (int i = 1; i <= 10; i++)
                {
                    try
                    {
                        commDeviceMS = new System.IO.Ports.SerialPort(PrintPort.Substring(0, PrintPort.Length - 1));
                        commDeviceMS.WriteTimeout = 30000;
                        commDeviceMS.Open();
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (i == 10)
                        {
                            throw ex;
                        }
                        else
                        {
                            Thread.Sleep(100);
                            GC.Collect();
                        }
                    }
                }
            }

            // Print preview arraylist
            PageArray = new System.Collections.ArrayList();
        }
        // Creates new page logical type
        public bool CreatePage(int Width, int Height)
        {
            try
            {
                // Allocate page size on memory
                PageBuffer = new char[Height, Width];

                // Set local variables
                iPageWidth = Width;
                iPageHeight = Height;

                // Set bottom line to 0
                iBottomLine = 0;

                return (true);
            }
            catch (Exception ex)
            {
                // An exception occured
                throw new MobilePrintEngineException("CreatePage() failed: " + ex.ToString());
            }
        }

        // Write to logical page
        public void WriteToLogicalPage(int x, int y, string Value)
        {
            int iLetterPos; // Position of word

            // Set index base 1
            x--;
            y--;

            // Page overflow check
            if (((x + Value.Length) > iPageWidth || (x < 0)) ||
                ((y > iPageHeight) || (y < 0)))
                throw new MobilePrintEngineException("Page overflow");

            // Iterate letters in word
            for (iLetterPos = 0; iLetterPos != Value.Length; iLetterPos++)
            {
                // Get 1 byte(letter) and put into array (logical page)
                PageBuffer[y, (x + iLetterPos)] = System.Convert.ToChar(Value.Substring(iLetterPos, 1));
            }

            // Set bottom line
            if (iBottomLine < y)
                iBottomLine = y;
        }

        // Write to logical page
        public void WriteToLogicalPage(int x, int y, int Length, int Alignment, string Value)
        {
            int iLetterPos; // Position of word

            // Set index base 1
            x--;
            y--;

            // Page overflow check
            if (((x + Value.Length) > iPageWidth || (x < 0)) ||
                ((y > iPageHeight) || (y < 0)))
                throw new MobilePrintEngineException("Page overflow");

            // Restricted text overflow
            if (Value.Length > Length)
                throw new MobilePrintEngineException("Field overflow");

            switch (Alignment)
            {
                case 0:
                    // LEFT
                    // Iterate letters in word
                    for (iLetterPos = 0; iLetterPos != Length; iLetterPos++)
                    {
                        // Get 1 letter and put into array (logical page)
                        PageBuffer[y, (x + iLetterPos)] = Convert.ToChar(Value.Substring(iLetterPos, 1));
                    }
                    break;
                case 1:
                    // RIGHT
                    int iStartPos; // Start position

                    // Get start position
                    iStartPos = Length - Value.Length;

                    // Iterate letters in word
                    for (iLetterPos = 0; iLetterPos != iStartPos; iLetterPos++)
                    {
                        // Put empty char
                        PageBuffer[y, (x + iLetterPos)] = ' ';
                    }

                    for (iLetterPos = iStartPos; iLetterPos != Length; iLetterPos++)
                    {
                        // Get 1 letter and put into array (logical page)
                        PageBuffer[y, (x + iLetterPos)] = Convert.ToChar(Value.Substring(iLetterPos - iStartPos, 1));
                    }
                    break;
                default:
                    throw new MobilePrintEngineException("Unexpected alignment");
            }
            // Set bottom line
            if (iBottomLine < y)
                iBottomLine = y;
        }
        // Form feed
        public void FormFeed()
        {
            // Send form feed character
            //if (commDevice != null)
            //{
            //    commDevice.Write(0x0C);
            //}
            //else 
                if (commDeviceMS != null)
            {
                Byte[] buff = new Byte[1];
                buff[0] = 0x0C;

                commDeviceMS.Write(buff, 0, 1);
            }
        }

        public void SendCommand(byte Command)
        {
            // Send form feed character
            //if (commDevice != null)
            //{
            //    commDevice.Write(Command);
            //}
            //else 
                if (commDeviceMS != null)
            {
                Byte[] buff = new Byte[1];
                buff[0] = Command;

                commDeviceMS.Write(buff, 0, 1);
            }
        }
        // Get logical row
        private string ReadLogicalPageByRow(int RowNumber)
        {
            int iCurrent; // Current row

            // Read into line for each char in array (array ~= a row in logical page)
            for (iCurrent = 0; iCurrent != iPageWidth; iCurrent++)
            {
                if ('\0' == PageBuffer[RowNumber, iCurrent])
                {
                    Row += " ";
                }
                else
                {
                    Row += Convert.ToString(PageBuffer[RowNumber, iCurrent]);
                }
            }
            Row.Trim();
            return (Row.ToString());
        }
        // Dispose the page
        public void Dispose()
        {

            PreviewBuffer = null; // Preview buffer area
            PageBuffer = null; // Print buffer area
            PageArray = null; // Page container for preview
            Row = null; // Logical row string value            
            //if (commDevice != null)
            //{
            //    commDevice.Close();
            //    commDevice.Dispose();
            //}
            //else 
                if (commDeviceMS != null)
            {
                commDeviceMS.Close();
                commDeviceMS.Dispose();
            }
            GC.Collect();

        }
        // New Page
        public void NewPage()
        {
            // Clear current page
            PageBuffer = null;
            GC.Collect();

            // Create new page
            CreatePage(iPageWidth, iPageHeight);
        }
        // Print the page
        public void Print()
        {

            // Init device
            string Line = "";
            //if (commDevice != null)
            //{
            //    commDevice.Write(0x1B);//Escape char
            //    commDevice.Write(0x0F);//condensed characters                
            //    a = true;
            //}
            //else 
                if (commDeviceMS != null)
            {
                Byte[] buff = new Byte[2];
                buff[0] = 0x1B;
                buff[1] = 0x0F;
                commDeviceMS.Write(buff, 0, 2);

            }
            int current = 1;
            int couple = PageHeight / 10;
            if (PageHeight % 10 > 0)
            {
                couple += 1;
            }
            for (int i = 0; i < PageHeight; i++)
            {
                for (int j = 0; j < PageWidth; j++)
                {
                    Line += PageBuffer[i, j];
                }
                Line = Line + "\n";
                if (current * (i + 1) % 10 == 0)
                {
                    //if (commDevice != null)
                    //{
                    //    //commDevice.Write(Line);
                    //}
                    //else 
                        if(commDeviceMS !=null)
                    {
                        commDeviceMS.Write(Line);
                    }
                    Thread.Sleep(1500);//Ja fut xhum printeri heheh
                    Line = "";
                }
                if (i + 1 == PageHeight)
                {
                    //if (commDevice != null)
                    //{
                    //    //commDevice.Write(Line);
                    //}
                    //else
                        if(commDeviceMS !=null)
                    {
                        commDeviceMS.Write(Line);
                    }
                    Thread.Sleep(1500);//Ja fut xhum printeri heheh
                    Line = "";
                }
            }

            // End of page
            FormFeed();

            // Close COM port
            //if (commDevice != null)
            //{
            //    commDevice.Close();
            //}
            //else 
                if (commDeviceMS != null)
            {
                commDeviceMS.DiscardInBuffer();
                commDeviceMS.DiscardOutBuffer();
                commDeviceMS.Close();
                commDeviceMS.Dispose();
            }

        }
        // Write to buffer for preview
        public void AddToPreview()
        {
            int iRow = 0; // Row number
            PreviewBuffer = new System.Text.StringBuilder(); // Buffer area

            for (iRow = 0; iRow != iBottomLine + 1; iRow++)
            {
                PreviewBuffer.Append(ReadLogicalPageByRow(iRow));
                PreviewBuffer.Append("\r\n");
                Row = "";
            }
            // Add to array
            PageArray.Add(PreviewBuffer);
        }
        // Preview the page
        public void Preview()
        {
            // Create preview window dynamicaly

            // Create form instance
            frmPrintPreview = new Form();
            // Create Textbox
            txtPreview = new TextBox();
            // Create page number indicator
            lblPageNumber = new Label();
            // Create back button
            btnBack = new Button();
            // Create previous button
            btnPrev = new Button();
            // Create next button
            btnNext = new Button();

            // Add controls to form object
            frmPrintPreview.Controls.Add(txtPreview);
            frmPrintPreview.Controls.Add(btnBack);
            frmPrintPreview.Controls.Add(btnPrev);
            frmPrintPreview.Controls.Add(btnNext);
            frmPrintPreview.Controls.Add(lblPageNumber);

            // Set a preview window controlbox
            frmPrintPreview.ControlBox = false;

            // txtPreview settings
            txtPreview.Top = 0;
            txtPreview.Left = 0;
            txtPreview.Width = Screen.PrimaryScreen.Bounds.Width;
            txtPreview.Height = Screen.PrimaryScreen.Bounds.Height - 70;
            txtPreview.Multiline = true;
            txtPreview.WordWrap = false;
            txtPreview.ReadOnly = true;
            txtPreview.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            txtPreview.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular);

            // Back button
            btnBack.Top = Screen.PrimaryScreen.Bounds.Height - 60;
            btnBack.Width = 50;
            btnBack.Height = 25;
            btnBack.Left = 5;
            btnBack.Text = "Back";
            btnBack.Click += new System.EventHandler(this.btnBack_Click);

            // Previous button
            btnPrev.Top = Screen.PrimaryScreen.Bounds.Height - 60;
            btnPrev.Width = 50;
            btnPrev.Height = 25;
            btnPrev.Left = Screen.PrimaryScreen.Bounds.Width - 115;
            btnPrev.Text = "Prev";
            btnPrev.Click += new System.EventHandler(this.btnPrev_Click);

            // Next button
            btnNext.Top = Screen.PrimaryScreen.Bounds.Height - 60;
            btnNext.Width = 50;
            btnNext.Height = 25;
            btnNext.Left = Screen.PrimaryScreen.Bounds.Width - 55;
            btnNext.Text = "Next";
            btnNext.Click += new System.EventHandler(this.btnNext_Click);

            // Indicator label
            lblPageNumber.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            lblPageNumber.Top = Screen.PrimaryScreen.Bounds.Height - 55;
            lblPageNumber.Width = 90;
            lblPageNumber.Height = 20;
            lblPageNumber.Left = 70;

            // Set start page
            iPreviewPageIndex = 0;
            lblPageNumber.Text = "Page #" + (iPreviewPageIndex + 1).ToString();
            txtPreview.Text = ((System.Text.StringBuilder)PageArray[iPreviewPageIndex]).ToString();

            // Show preview window
            frmPrintPreview.Show();
        }
        // Button event handlers
        private void btnBack_Click(object sender, System.EventArgs e)
        {
            // Dispose buffer area
            if (PageArray != null) PageArray.Clear();
            PreviewBuffer = null;
            GC.Collect();

            // Dispose print preview window
            lblPageNumber.Dispose();
            frmPrintPreview.Dispose();
            txtPreview.Dispose();
            btnPrev.Dispose();
            btnNext.Dispose();
        }
        private void btnNext_Click(object sender, System.EventArgs e)
        {
            // Set page preview into text box			
            if (iPreviewPageIndex != PageArray.Count - 1)
                iPreviewPageIndex++; // Go to next page
            lblPageNumber.Text = "Page #" + (iPreviewPageIndex + 1).ToString();
            txtPreview.Text = ((System.Text.StringBuilder)PageArray[iPreviewPageIndex]).ToString();
        }
        private void btnPrev_Click(object sender, System.EventArgs e)
        {
            // Set page preview into text box			
            if (iPreviewPageIndex != 0)
                iPreviewPageIndex--; // Go to previous page
            lblPageNumber.Text = "Page #" + (iPreviewPageIndex + 1).ToString();
            txtPreview.Text = ((System.Text.StringBuilder)PageArray[iPreviewPageIndex]).ToString();
        }
        // Properties
        public int PageWidth
        {
            get
            {
                return (iPageWidth);
            }
        }
        public int PageHeight
        {
            get
            {
                return (iPageHeight);
            }
        }
        //public string Port
        //{
        //    set
        //    {
        //        commDevice.PortName = value;
        //    }
        //    get
        //    {
        //        return (commDevice.PortName);
        //    }
        //}
    }
    public class MobilePrintEngineException : Exception
    {
        public MobilePrintEngineException(string sException)
        {
            MessageBox.Show("An error occured:" + sException, "MPE Exception");
        }
    }
    #endregion //PrintEngine uses CommDevice

    #region PnPrint
    class PnPrint
    {
        DataSet dsRep = null;
        SmartDevicePrintEngine sdPrint;
        //DataTable PrintTable; //@BE Komentuar me 10.21.2018 dhe zevendesuar me rreshtin poshte
        public DataTable PrintTable;
        DataTable HeaderTable;
        DataTable FooterTable;
        int PageWidth = 0;
        int PageHeight = 0;
        int iCopies = 1;
        PnType aPrintOption;
        List<string> CalcValues = null;

        //construcor that will take 3 arguments:Port given as string i.e. "COM1:",repor path and DataTable
        //from which to take Data for printing; =and instantiates a SmartDeviceEngine object
        /// <summary>
        /// Print Engine for WindowsMobile devices through Com ports
        /// </summary>
        /// <param name="DevicePort">The device to be printed through</param>
        /// <param name="PrintPort">Specify portname (i.e. COM5)</param>
        /// <param name="PnReportFileName">PnReports File</param>
        /// <param name="PrintData">Table with Details information</param>
        /// <param name="HeaderData">Table with Header information</param>
        public PnPrint(PnDevice DevicePort, string PrintPort, string PnReportFileName, DataTable PrintData, DataTable HeaderData)
        {
            dsRep = new DataSet();
            PrintTable = PrintData;
            HeaderTable = HeaderData;
            dsRep.ReadXml(PnReportFileName);
            //start reading propertis of report from PNREP (xml bases);
            PageWidth = Convert.ToInt16(dsRep.Tables[0].Rows[0]["Gjeresia"]);
            PageHeight = PrintData.Rows.Count + 20;
            sdPrint = new SmartDevicePrintEngine(DevicePort, PrintPort);
            sdPrint.CreatePage(PageWidth, PageHeight);
            CalcValues = new List<string>();
        }

        public PnPrint(PnDevice DevicePort, string PrintPort, string PnReportFileName, DataTable PrintData, DataTable HeaderData, DataTable FooterData)
        {
            dsRep = new DataSet();
            PrintTable = PrintData;
            HeaderTable = HeaderData;
            FooterTable = FooterData;
            dsRep.ReadXml(PnReportFileName);
            //start reading propertis of report from PNREP (xml bases);
            PageWidth = Convert.ToInt16(dsRep.Tables[0].Rows[0]["Gjeresia"]);
            PageHeight = PrintData.Rows.Count + HeaderData.Rows.Count + FooterData.Rows.Count + 20;//Convert.ToInt16(dsRep.Tables[0].Rows[0]["Lartesia"]);
            sdPrint = new SmartDevicePrintEngine(DevicePort, PrintPort);
            sdPrint.CreatePage(PageWidth, PageHeight);
            CalcValues = new List<string>();
        }

        public void PrintPnReport(PnType PrintOption, bool PrintColumnTitles, string Copies, bool printCopyRight)
        {
            bool FooterPrinting = false;
            DataTable dt = new DataTable();
            int x = 1;
            int y = 1;
            int ColumnCount = dsRep.Tables[1].Rows.Count;
            int RowCount = PrintTable.Rows.Count;
            aPrintOption = PrintOption;

            string Seperator = "";
            if (PrintTable.Rows.Count < 1)
            {
                MessageBox.Show("Tabela e dhënë është e zbrazët");
                return;
            }
            if (HeaderTable.Rows.Count < 1)
            {
                MessageBox.Show("Tabela e dhënë është e zbrazët");
                return;
            }
            CopyData(dsRep.Tables["label"], ref dt, "Tipi='Detal'");
            InitCalcValues(dt);//for each column will add 0.00f,

            #region Title
            //***********************TITLE//****************************************************
            string Titulli = dsRep.Tables[0].Rows[0]["Titulli"].ToString();
            x = (sdPrint.PageWidth / 2) - (Titulli.Length / 2);
            sdPrint.WriteToLogicalPage(x, y, Titulli); //write title
            y++;
            Seperator = "---------------------------------------------------------------------";
            x = 1;
            //for (int i = 0; i < PageWidth; i++) Seperator += "-";
            sdPrint.WriteToLogicalPage(x, y, Seperator); //write seperator (title.length) 
            #endregion

            #region Header
            //***********************Headers//********************************************                              
            if (HeaderTable != null) //if given paramter is NULL, don't print HeaderData
            {
                CopyData(dsRep.Tables["label"], ref dt, "Tipi='Header1'");
                //WriteColumnTitles(ref x, ref y, ref sdPrint, dt, HeaderTable);
                if (dt.Rows.Count > 0)
                    WriteData(ref x, ref y, ref sdPrint, dt, HeaderTable);

                CopyData(dsRep.Tables["label"], ref dt, "Tipi='Header2'");
                // WriteColumnTitles(ref x, ref y, ref sdPrint, dt, HeaderTable);
                if (dt.Rows.Count > 0)
                    WriteData(ref x, ref y, ref sdPrint, dt, HeaderTable);
            }
            WriteSeparator(ref x, ref y, ref sdPrint, "=");
            #endregion

            #region Lines
            //***********************Column Titles//********************************************           
            if (PrintColumnTitles)
            {
                CopyData(dsRep.Tables["label"], ref dt, "Tipi='Detal'");
                WriteColumnTitles(ref x, ref y, ref sdPrint, dt, PrintTable);
                WriteSeparator(ref x, ref y, ref sdPrint, "-");
            }

            //***********************//DataValue//**********************************************         
            CopyData(dsRep.Tables["label"], ref dt, "Tipi='Detal'");
            WriteData(ref x, ref y, ref sdPrint, dt, PrintTable);
            //***********************Total//***********************//***********************
            WriteSeparator(ref x, ref y, ref sdPrint, "-");
            CopyData(dsRep.Tables["label"], ref dt, "Tipi='Detal'");
            WriteTotal(ref x, ref y, ref sdPrint, dt);
            #endregion

            #region Footer
            if (FooterTable != null) //if given paramter is NULL, don't print HeaderData
            {
                FooterPrinting = true;
                y++;
                while (y < sdPrint.PageHeight - FooterTable.Rows.Count - 6)
                {
                    y++;
                    FixPageHeight(ref y, ref sdPrint);
                    sdPrint.WriteToLogicalPage(x, y, " ");
                }

                CopyData(dsRep.Tables["label"], ref dt, "Tipi='Footer'");
                WriteColumnTitles(ref x, ref y, ref sdPrint, dt, FooterTable);
                if (dt.Rows.Count > 0)
                    WriteData(ref x, ref y, ref sdPrint, dt, FooterTable);


            }
            y++;
            if (!FooterPrinting)
            {
                while (y < sdPrint.PageHeight - 10)
                {
                    y++;
                    FixPageHeight(ref y, ref sdPrint);

                    sdPrint.WriteToLogicalPage(x, y, " ");
                }
            }
            #endregion

            #region Summary
            if (printCopyRight)
            {
                FixPageHeight(ref y, ref sdPrint);
                sdPrint.WriteToLogicalPage(x, y, "Fatura eshte e hartuar ne " + Copies.ToString() + " kopje: a)faqe 1: Origjinali - Bleresi; b) faqe 2: Transportuesi; c) faqe 3: Shitesi"); // TEMP SOLUTION
                y++;
            }
            else
            { y += 4; }
            FixPageHeight(ref y, ref sdPrint);
            sdPrint.WriteToLogicalPage(x, y, dsRep.Tables[0].Rows[0]["Summary"].ToString());
            #endregion

            if (PrintOption == PnType.Print)
            {
                sdPrint.Print();
            }
            else
            {
                sdPrint.AddToPreview();
                sdPrint.Preview();
            }
        }

        #region HelperFunctions

        //Checks if a given string is a number integer,float or whatever number
        private bool IsNumber(string Sample)
        {
            return Regex.IsMatch(Sample, @"^\d*$|^[0-9]*\.[0-9]*$");
        }

        //This functions checks if y coordinate exceeds PageHeight, if so we need either to PRINT or to AddToPreview
        private void FixPageHeight(ref int y, ref SmartDevicePrintEngine PrintEngine)
        {
            //if (y > PrintEngine.PageHeight)
            //{
            //    if (iCopies == -55)
            //    {
            //        Pages += 1;
            //        return;
            //    }

            //    //before creating new page we need to get rid of current buffer, either print or addtoPreview
            //    if (aPrintOption == PnType.Print)
            //        sdPrint.Print();
            //    else
            //    {
            //        sdPrint.AddToPreview();
            //        sdPrint.Preview();
            //    }
            //    y = 1;//reset
            //    PrintEngine.NewPage();

            //}
        }

        //this function gets specific rows (fieldnames and their properties according to filter givem i.e. 
        //Tipi='Header1', gets all labels belonging to Header1
        private void CopyData(DataTable SrcTable, ref DataTable DestTable, string FilterExpression)
        {
            DestTable.Clear();
            DestTable.Columns.Clear();
            //get columns so we can add rows,
            for (int i = 0; i < SrcTable.Columns.Count; i++)
                DestTable.Columns.Add(SrcTable.Columns[i].Caption);

            DestTable.Clear();//clear before copying new rows
            DataRow[] dr = null;
            dr = SrcTable.Select(FilterExpression);
            for (int i = 0; i < dr.Length; i++)
                DestTable.Rows.Add(dr[i].ItemArray);
        }

        //writes given characted CharSeperator, which is equal to PageWidth (how many times)
        private void WriteSeparator(ref int x, ref int y, ref SmartDevicePrintEngine PrintEngine, string CharSeperator)
        {


            string Seperator = "";
            x = 1;//reset x-coordinate            
            y++;
            FixPageHeight(ref y, ref PrintEngine);

            //for (int i = 0; i < PrintEngine.PageWidth; i++) Seperator += CharSeperator;
            for (int i = 0; i < 69; i++) Seperator += CharSeperator;
            PrintEngine.WriteToLogicalPage(x, y, Seperator);

        }

        //Reads PnReport xml thorugh a given dataTable, and reads attrib 'titulli', and Writes them to logical page (char buffer)
        private void WriteColumnTitles(ref int x, ref int y, ref SmartDevicePrintEngine PrintEngine, DataTable PnReportData, DataTable DataValues)
        {

            string ColumnName = "";
            int width = 0;
            x = 1;
            y++;//proceed to next line
            FixPageHeight(ref y, ref PrintEngine);
            for (int j = 0; j < PnReportData.Rows.Count; j++)
            {
                ColumnName = "";
                width = Convert.ToInt16(PnReportData.Rows[j]["Gjeresia"].ToString());
                ColumnName = PnReportData.Rows[j]["Teksti"].ToString() + " ";

                if (Ctrl.IsDecimal(DataValues.Rows[0][PnReportData.Rows[j]["dbFusha"].ToString()].ToString()))
                {
                    ColumnName = ColumnName.Trim().PadLeft(width);
                }


                if (sdPrint.PageWidth > x)
                    PrintEngine.WriteToLogicalPage(x, y, ColumnName);
                x += width + 1;

            }
            x = 1;//reset x for next usage

        }

        //writes Data through a given dataTable, for fields mapped in PnReprot.pnrep created using PnReportDesigner
        private void WriteData(ref int x, ref int y, ref SmartDevicePrintEngine PrintEngine, DataTable PnReportData, DataTable DataValues)
        {
            int RowCount = DataValues.Rows.Count;
            int width = 0;
            int ColumnLength = 0;
            string ColumnName = "";
            string DataValue = "";
            string CalcFunction = "";
            decimal tempCalc = 0;
            int ColIndex = 0;

            for (int i = 0; i < RowCount; i++) //for each row in DataTable
            {
                // DataValue = "";
                y++;
                FixPageHeight(ref y, ref PrintEngine);
                x = 1;

                for (int j = 0; j < PnReportData.Rows.Count; j++)
                {
                    DataValue = "";
                    width = Convert.ToInt16(PnReportData.Rows[j]["Gjeresia"].ToString());

                    ColumnName = PnReportData.Rows[j]["dbFusha"].ToString();
                    DataValue = DataValues.Rows[i][ColumnName].ToString();
                    CalcFunction = PnReportData.Rows[j]["Kalkulim"].ToString();

                    ColIndex = DataValues.Columns.IndexOf(ColumnName);

                    if (CalcFunction == "SUM")
                    {
                        if (CalcValues[ColIndex] == "") CalcValues[ColIndex] = "0.00";
                        try
                        {
                            tempCalc = decimal.Parse(CalcValues[ColIndex]);
                            tempCalc += decimal.Parse(DataValue);
                            CalcValues[ColIndex] = tempCalc.ToString();
                        }
                        catch (Exception )
                        {

                        };
                    }
                    else if (CalcFunction == "COUNT")
                    {
                        CalcValues[ColIndex] = "Mbyllet me: " + RowCount.ToString() + " artikuj";
                    }
                    else
                    {
                        if (CalcFunction == "COUNTF")
                        {
                            CalcValues[ColIndex] = "Mbyllet me: " + RowCount.ToString() + " fatura";
                        }
                        else
                        {
                            CalcValues[ColIndex] = CalcFunction;


                        }
                    }






                    if ((DataValue.Length > width) || (DataValue.Length == width)) //truncate DataValue if longer or equal
                    {
                        DataValue = DataValue.Substring(0, width) + " ";
                    }
                    else if (DataValue.Length < width) //fill with spaces remaining Length
                    {
                        if (Ctrl.IsDecimal(DataValue))
                        {
                            DataValue = String.Format("{0," + width + "}", DataValue);
                        }
                        else
                        {
                            ColumnLength = width - DataValue.Length;
                            for (int a = 0; a < ColumnLength; a++)
                                DataValue += " ";
                            DataValue += "  ";
                        }

                    }



                    if (sdPrint.PageWidth > x)
                        PrintEngine.WriteToLogicalPage(x, y, DataValue);
                    x += width + 1;


                }

            }
            x = 1;//reset x, after writing,
            y++;

        }

        //initiate CalcValues set Default values to 0 for how Many fields are in DataTable
        private void InitCalcValues(DataTable PnReportData)
        {
            CalcValues.Clear();

            for (int i = 0; i < PrintTable.Columns.Count; i++)
            {
                CalcValues.Add("");//"0.00");
            }
        }

        //writes Calculated Data (Count or sum), using  public list CalcValues[] and PrReport.pnrep (to aknowledge fields to be calced).
        private void WriteTotal(ref int x, ref int y, ref SmartDevicePrintEngine PrintEngine, DataTable PnReportData)
        {
            y++;
            x = 1;
            FixPageHeight(ref y, ref sdPrint);
            int width = 0;
            int ColumnLength = 0;
            string DataValue = "";

            for (int i = 0; i < PnReportData.Rows.Count; i++)
            {
                width = Convert.ToInt16(PnReportData.Rows[i]["Gjeresia"]);
                if (width > 70)
                    width = 10;
                DataValue = "";

                DataValue = CalcValues[i];

                if (DataValue.Length >= width)//truncate DataValue if longer or equal
                {
                    DataValue = DataValue.Substring(0, width) + " ";
                }
                else if (DataValue.Length < width) //fill with spaces remaining Length
                {
                    ColumnLength = width - DataValue.Length;
                    for (int a = 0; a < ColumnLength; a++)
                        DataValue += " ";
                    DataValue += "  ";
                }

                decimal formatedValue = 0;

                if (Ctrl.IsDecimal(DataValue))
                {
                    formatedValue = decimal.Parse(DataValue);
                    DataValue = String.Format("{0:0.00#}", formatedValue);
                    DataValue = DataValue.PadLeft(width);
                }

                if (sdPrint.PageWidth > x)
                    PrintEngine.WriteToLogicalPage(x, y, DataValue);
                x += width + 1;

            }
            x = 1;
        }

        private int GetNumberOfPages()
        {
            //PrintPnReport(PnType.Preview, true, -55);
            return iCopies;
        }


        #endregion

        public void Dispose()
        {
            dsRep.Dispose();
            sdPrint.Dispose();
            GC.Collect();
        }
    }
    #endregion

    public enum PnType
    {
        Print,
        Preview
    }

    public enum PnDevice
    {
        //MultiplicatorPort,
        BluetoothPort,
        PreviewOnly
    }
}
