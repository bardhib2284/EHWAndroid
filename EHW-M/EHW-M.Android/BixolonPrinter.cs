//using Android.Content;
//using Com.Bxl.Config.Editor;
//using Java.Interop;
//using Jpos;
//using Jpos.Config;
//using Jpos.Events;
//using Org.Xml.Sax;
//using System;

//namespace EHW_M.Droid {
//    public class BixolonPrinter : Java.Lang.Object{
//        public static int ALIGNMENT_LEFT = 1;
//        public static int ALIGNMENT_CENTER = 2;
//        public static int ALIGNMENT_RIGHT = 4;

//        // ------------------- Text attribute ------------------- //
//        public static int ATTRIBUTE_NORMAL = 0;
//        public static int ATTRIBUTE_FONT_A = 1;
//        public static int ATTRIBUTE_FONT_B = 2;
//        public static int ATTRIBUTE_FONT_C = 4;
//        public static int ATTRIBUTE_BOLD = 8;
//        public static int ATTRIBUTE_UNDERLINE = 16;
//        public static int ATTRIBUTE_REVERSE = 32;
//        public static int ATTRIBUTE_FONT_D = 64;

//        // ------------------- Barcode Symbology ------------------- //
//        public static int BARCODE_TYPE_UPCA = POSPrinterConst.PtrBcsUpca;
//        public static int BARCODE_TYPE_UPCE = POSPrinterConst.PtrBcsUpce;
//        public static int BARCODE_TYPE_EAN8 = POSPrinterConst.PtrBcsEan8;
//        public static int BARCODE_TYPE_EAN13 = POSPrinterConst.PtrBcsEan13;
//        public static int BARCODE_TYPE_ITF = POSPrinterConst.PtrBcsItf;
//        public static int BARCODE_TYPE_Codabar = POSPrinterConst.PTRBCSCodabar;
//        public static int BARCODE_TYPE_Code39 = POSPrinterConst.PTRBCSCode39;
//        public static int BARCODE_TYPE_Code93 = POSPrinterConst.PTRBCSCode93;
//        public static int BARCODE_TYPE_Code128 = POSPrinterConst.PTRBCSCode128;
//        public static int BARCODE_TYPE_PDF417 = POSPrinterConst.PtrBcsPdf417;
//        public static int BARCODE_TYPE_MAXICODE = POSPrinterConst.PtrBcsMaxicode;
//        public static int BARCODE_TYPE_DATAMATRIX = POSPrinterConst.PtrBcsDatamatrix;
//        public static int BARCODE_TYPE_QRCODE = POSPrinterConst.PtrBcsQrcode;
//        public static int BARCODE_TYPE_EAN128 = POSPrinterConst.PtrBcsEan128;
//        // ------------------- Barcode HRI ------------------- //
//        public static int BARCODE_HRI_NONE = POSPrinterConst.PtrBcTextNone;
//        public static int BARCODE_HRI_ABOVE = POSPrinterConst.PtrBcTextAbove;
//        public static int BARCODE_HRI_BELOW = POSPrinterConst.PtrBcTextBelow;

//        // ------------------- Farsi Option ------------------- //
//        public static int OPT_REORDER_FARSI_RTL = 0;
//        public static int OPT_REORDER_FARSI_MIXED = 1;

//        // ------------------- CharacterSet ------------------- //
//        public static int CS_437_USA_STANDARD_EUROPE = 437;
//        public static int CS_737_GREEK = 737;
//        public static int CS_775_BALTIC = 775;
//        public static int CS_850_MULTILINGUAL = 850;
//        public static int CS_852_LATIN2 = 852;
//        public static int CS_855_CYRILLIC = 855;
//        public static int CS_857_TURKISH = 857;
//        public static int CS_858_EURO = 858;
//        public static int CS_860_PORTUGUESE = 860;
//        public static int CS_862_HEBREW_DOS_CODE = 862;
//        public static int CS_863_CANADIAN_FRENCH = 863;
//        public static int CS_864_ARABIC = 864;
//        public static int CS_865_NORDIC = 865;
//        public static int CS_866_CYRILLIC2 = 866;
//        public static int CS_928_GREEK = 928;
//        public static int CS_1250_CZECH = 1250;
//        public static int CS_1251_CYRILLIC = 1251;
//        public static int CS_1252_LATIN1 = 1252;
//        public static int CS_1253_GREEK = 1253;
//        public static int CS_1254_TURKISH = 1254;
//        public static int CS_1255_HEBREW_NEW_CODE = 1255;
//        public static int CS_1256_ARABIC = 1256;
//        public static int CS_1257_BALTIC = 1257;
//        public static int CS_1258_VIETNAM = 1258;
//        public static int CS_FARSI = 7065;
//        public static int CS_KATAKANA = 7565;
//        public static int CS_KHMER_CAMBODIA = 7572;
//        public static int CS_THAI11 = 8411;
//        public static int CS_THAI14 = 8414;
//        public static int CS_THAI16 = 8416;
//        public static int CS_THAI18 = 8418;
//        public static int CS_THAI42 = 8442;
//        public static int CS_KS5601 = 5601;
//        public static int CS_BIG5 = 6605;
//        public static int CS_GB2312 = 2312;
//        public static int CS_SHIFT_JIS = 8374;
//        public static int CS_TCVN_3_1 = 3031;
//        public static int CS_TCVN_3_2 = 3032;

//        private Context context = null;

//        private BXLConfigLoader bxlConfigLoader = null;
//        private POSPrinter posPrinter = null;
//        private MSR msr = null;
//        private CashDrawer cashDrawer = null;
//        private SmartCardRW smartCardRW = null;
//        private int mPortType;
//        private String mAddress;

//        public BixolonPrinter(Context context) {
//            this.context = context;
//            posPrinter = new POSPrinter(this.context);


//            smartCardRW = new SmartCardRW();
//            cashDrawer = new CashDrawer();

//            bxlConfigLoader = new BXLConfigLoader(this.context);
//            try {
//                bxlConfigLoader.OpenFile();
//            }
//            catch (Exception e) {
//                bxlConfigLoader.NewFile();
//            }

//        }

//        public bool PrinterOpen(int portType, String logicalName, String address, bool isAsyncMode) {
//            if (SetTargetDevice(portType, logicalName, BXLConfigLoader.DeviceCategoryPosPrinter, address)) {
//                int retry = 1;
//                if (portType == BXLConfigLoader.DeviceBusBluetoothLe) {
//                    retry = 5;
//                }
//                for (int i = 0; i < retry; i++) {
//                    try {
//                        posPrinter.Open(logicalName);
//                        posPrinter.Claim(5000 * 2);
//                        posPrinter.DeviceEnabled = true;
//                        posPrinter.AsyncMode = isAsyncMode;

//                        mPortType = portType;
//                        mAddress = address;
//                        return true;
//                    }
//                    catch (JposException e) {
//                        e.PrintStackTrace();
//                        try {
//                            posPrinter.Close();

//                        }
//                        catch (JposException e1) {
//                            e1.PrintStackTrace();
//                        }
//                    }
//                }
//            }
//            return false;
//        }

//        public bool PrinterClose() {
//            try {
//                if (posPrinter.Claimed) {
//                    posPrinter.DeviceEnabled = false;
//                    posPrinter.Release();
//                    posPrinter.Close();
//                }
//            }
//            catch (JposException e) {
//                e.PrintStackTrace();
//            }
//            return true;
//        }

//        private bool SetTargetDevice(int portType, string logicalName, int deviceCategory, string address) {
//            try {
//                foreach (object entry in bxlConfigLoader.Entries) {
//                    JposEntry jposEntry = (JposEntry)entry;
//                    if (jposEntry.ToString() == logicalName) {
//                        bxlConfigLoader.RemoveEntry(logicalName);
//                        break;
//                    }
//                }
//                bxlConfigLoader.AddEntry(logicalName, deviceCategory, GetProductName(logicalName), portType, address);
//                bxlConfigLoader.SaveFile();
//            }
//            catch (JposException e) {
//                e.PrintStackTrace();
//                return false;
//            }
//            return true;
//        }


//        private string GetProductName(string name) {
//            String productName = BXLConfigLoader.ProductNameSppR200ii;
//            if (name == "SPP-R410")
//                productName = BXLConfigLoader.ProductNameSppR410;
//            return productName;
//        }


//        public bool PrintText(string data, int alignment, int attribute, int textSize) {
//            bool ret = true;
//            try {
//                if (!posPrinter.DeviceEnabled) {
//                    return false;
//                }

//                string strOption = EscapeSequence.getString(0);
//                if ((alignment & ALIGNMENT_LEFT) == ALIGNMENT_LEFT) {
//                    strOption += EscapeSequence.getString(4);
//                }

//                if ((alignment & ALIGNMENT_CENTER) == ALIGNMENT_CENTER) {
//                    strOption += EscapeSequence.getString(5);
//                }

//                if ((alignment & ALIGNMENT_RIGHT) == ALIGNMENT_RIGHT) {
//                    strOption += EscapeSequence.getString(6);
//                }

//                if ((attribute & ATTRIBUTE_FONT_A) == ATTRIBUTE_FONT_A) {
//                    strOption += EscapeSequence.getString(1);
//                }

//                if ((attribute & ATTRIBUTE_FONT_B) == ATTRIBUTE_FONT_B) {
//                    strOption += EscapeSequence.getString(2);
//                }

//                if ((attribute & ATTRIBUTE_FONT_C) == ATTRIBUTE_FONT_C) {
//                    strOption += EscapeSequence.getString(3);
//                }

//                if ((attribute & ATTRIBUTE_FONT_D) == ATTRIBUTE_FONT_D) {
//                    strOption += EscapeSequence.getString(33);
//                }

//                if ((attribute & ATTRIBUTE_BOLD) == ATTRIBUTE_BOLD) {
//                    strOption += EscapeSequence.getString(7);
//                }

//                if ((attribute & ATTRIBUTE_UNDERLINE) == ATTRIBUTE_UNDERLINE) {
//                    strOption += EscapeSequence.getString(9);
//                }

//                if ((attribute & ATTRIBUTE_REVERSE) == ATTRIBUTE_REVERSE) {
//                    strOption += EscapeSequence.getString(11);
//                }

//                switch (textSize) {
//                    case 1:
//                        strOption += EscapeSequence.getString(17);
//                        strOption += EscapeSequence.getString(25);
//                        break;
//                    case 2:
//                        strOption += EscapeSequence.getString(18);
//                        strOption += EscapeSequence.getString(26);
//                        break;
//                    case 3:
//                        strOption += EscapeSequence.getString(19);
//                        strOption += EscapeSequence.getString(27);
//                        break;
//                    case 4:
//                        strOption += EscapeSequence.getString(20);
//                        strOption += EscapeSequence.getString(28);
//                        break;
//                    case 5:
//                        strOption += EscapeSequence.getString(21);
//                        strOption += EscapeSequence.getString(29);
//                        break;
//                    case 6:
//                        strOption += EscapeSequence.getString(22);
//                        strOption += EscapeSequence.getString(30);
//                        break;
//                    case 7:
//                        strOption += EscapeSequence.getString(23);
//                        strOption += EscapeSequence.getString(31);
//                        break;
//                    case 8:
//                        strOption += EscapeSequence.getString(24);
//                        strOption += EscapeSequence.getString(32);
//                        break;
//                    default:
//                        strOption += EscapeSequence.getString(17);
//                        strOption += EscapeSequence.getString(25);
//                        break;
//                }

//                posPrinter.PrintNormal(POSPrinterConst.PtrSReceipt, strOption + data);
//            }
//            catch (JposException e) {
//                // TODO Auto-generated catch block
//                e.PrintStackTrace();

//                ret = false;
//            }

//            return ret;
//        }

//        public IntPtr Handle => throw new NotImplementedException();

//        public int JniIdentityHashCode => throw new NotImplementedException();

//        public JniObjectReference PeerReference => throw new NotImplementedException();

//        public JniPeerMembers JniPeerMembers => throw new NotImplementedException();

//        public JniManagedPeerStates JniManagedPeerState => throw new NotImplementedException();

//        public void DataOccurred(DataEvent p0) {
//            throw new NotImplementedException();
//        }

//        public void DirectIOOccurred(DirectIOEvent p0) {
//            throw new NotImplementedException();
//        }

//        public void Dispose() {
//            throw new NotImplementedException();
//        }

//        public void Disposed() {
//            throw new NotImplementedException();
//        }

//        public void DisposeUnlessReferenced() {
//            throw new NotImplementedException();
//        }

//        public void ErrorOccurred(ErrorEvent p0) {
//            throw new NotImplementedException();
//        }

//        public void Finalized() {
//            throw new NotImplementedException();
//        }

//        public void OutputCompleteOccurred(OutputCompleteEvent p0) {
//            throw new NotImplementedException();
//        }

//        public void SetJniIdentityHashCode(int value) {
//            throw new NotImplementedException();
//        }

//        public void SetJniManagedPeerState(JniManagedPeerStates value) {
//            throw new NotImplementedException();
//        }

//        public void SetPeerReference(JniObjectReference reference) {
//            throw new NotImplementedException();
//        }

//        public void StatusUpdateOccurred(StatusUpdateEvent p0) {
//            throw new NotImplementedException();
//        }

//        public void UnregisterFromRuntime() {
//            throw new NotImplementedException();
//        }
//    }
//}