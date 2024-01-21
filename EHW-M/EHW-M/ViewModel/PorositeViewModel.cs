using Acr.UserDialogs;
using EHW_M;
using EHWM.Models;
using EHWM.Services;
using EHWM.Views;
using EHWM.Views.Popups;
using Newtonsoft.Json;
using Plugin.BxlMpXamarinSDK;
using Plugin.BxlMpXamarinSDK.Abstractions;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EHWM.ViewModel {

    public class PorositeViewModelNavigationParameters {
        public List<Klientet> Klientet { get; set; }
        public List<KrijimiPorosive> KrijimiPorosives { get; set; }
        public Agjendet Agjendi { get; set; }
        public int NrRendor { get; set; }
        public List<Orders> Orders { get; set; }
    }

    public class PorositeViewModel : BaseViewModel{

        public List<KrijimiPorosive> KrijimiPorosives { get; set; }

        public List<Klientet> Klientet { get; set; }
        public Agjendet Agjendi { get; set; }

        private int _nrRendor;
        public int NrRendor {
            get { return _nrRendor; } set { SetProperty(ref _nrRendor, value); }
        }
        private Porosite _selectedPorosia;
        public Porosite SelectedPorosia {
            get { return _selectedPorosia; }
            set { SetProperty(ref _selectedPorosia, value);}
        }

        public ICommand ShtoKlientinCommand { get; set; }
        public ICommand ZgjedhArtikullinCommand { get; set; }
        public ICommand FshijArtikullinCommand { get; set; }
        public ICommand HapPorositeCommand { get; set; }

        private Klientet _selectedKlient;
        public Klientet SelectedKlient {
            get { return _selectedKlient; }
            set { SetProperty(ref _selectedKlient, value); }
        }
        public ObservableCollection<KrijimiPorosive> KrijimiPorositeKlientet { get; set; }
        private string _subTitle;
        public string SubTitle {
            get { return _subTitle; }
            set { SetProperty(ref _subTitle, value); }
        }
        public DateTime Data { get; set; }
        public ICommand AddArtikulliCommand { get; set; }
        public ICommand RegjistroCommand { get; set; }
        public ICommand PrintoFaturenCommand { get; set; }
        private ObservableCollection<Klientet> _SearchedKlientet;
        public ObservableCollection<Klientet> SearchedKlientet {
            get { return _SearchedKlientet; }
            set { SetProperty(ref _SearchedKlientet, value); }
        }
        public PorositeViewModel(PorositeViewModelNavigationParameters PorositeViewModelNavigationParameters) {
            Klientet = PorositeViewModelNavigationParameters.Klientet;
            Agjendi = PorositeViewModelNavigationParameters.Agjendi;
            NrRendor = PorositeViewModelNavigationParameters.NrRendor;
            Data = DateTime.Now;
            SubTitle = "Lista e porosive "  + Data.AddDays(-7).ToString("dd/MM/yyyy") + " - " + Data.ToString("dd/MM/yyyy");
            KrijimiPorosives = PorositeViewModelNavigationParameters.KrijimiPorosives;
            ShtoKlientinCommand = new Command(ShtoKlientinNeListe);
            ZgjedhArtikullinCommand = new Command(async () => await ZgjedhArtikullinAsync());
            KrijimiPorositeKlientet = new ObservableCollection<KrijimiPorosive>(KrijimiPorosives);
            KrijoPorosine = true;
            SelectedKlientIndex = 0;
            IncreaseSasiaCommand = new Command(IncreaseSasia);
            DecreaseSasiaCommand = new Command(DecreaseSasia);
            AddArtikulliCommand = new Command(async () => await AddArtikulliAsync());
            FshijArtikullinCommand = new Command(FshijArtikullinAsync);
            RegjistroCommand = new Command(async () => await RegjistroAsync());
            HapPorositeCommand = new Command(async () => await HapPorositeAsync());
            OrdersList = new ObservableCollection<Orders>(PorositeViewModelNavigationParameters.Orders);
            PrintoFaturenCommand = new Command(async () => await PrintoFaturenAsync());

        }

        public async Task PrintoFaturenAsync() {
            await App.Instance.PushAsyncNewPage(new PrinterSelectionPage() { BindingContext = this });
        }
        public MPosControllerPrinter _printer;
        public MposConnectionInformation _connectionInfo;
        public static SemaphoreSlim _printSemaphore = new SemaphoreSlim(1, 1);
        async Task<MPosControllerDevices> OpenPrinterService(MposConnectionInformation connectionInfo) {
            if (connectionInfo == null)
                return null;

            if (_printer != null)
                return _printer;

            if (_printSemaphore == null) {
                _printSemaphore = new SemaphoreSlim(1, 1);
            }
            _printer = MPosDeviceFactory.Current.createDevice(MPosDeviceType.MPOS_DEVICE_PRINTER) as MPosControllerPrinter;

            switch (connectionInfo.IntefaceType) {
                case MPosInterfaceType.MPOS_INTERFACE_BLUETOOTH:
                case MPosInterfaceType.MPOS_INTERFACE_WIFI:
                case MPosInterfaceType.MPOS_INTERFACE_ETHERNET:
                    _printer.selectInterface((int)connectionInfo.IntefaceType, connectionInfo.Address);
                    _printer.selectCommandMode((int)(MPosCommandMode.MPOS_COMMAND_MODE_BYPASS));
                    break;
                default:
                    UserDialogs.Instance.Alert("Connection Fail", "Not Supported Interface", "OK");
                    return null;
            }

            await _printSemaphore.WaitAsync();

            try {
                var result = await _printer.openService();
                if (result != (int)MPosResult.MPOS_SUCCESS) {
                    _printer = null;
                    UserDialogs.Instance.Alert("Connection Fail", "openService failed. (" + result.ToString() + ")", "OK");
                }
            }
            finally {
                _printSemaphore.Release();
            }

            return _printer;
        }
        public async Task OnDeviceOpenClicked() {
            try {
                if (_printer == null) {
                    // Prepares to communicate with the printer 
                    _printer = await OpenPrinterService(_connectionInfo) as MPosControllerPrinter;
                    if(App.Instance.MainPage is NavigationPage np) {
                        if(np.Navigation.NavigationStack[np.Navigation.NavigationStack.Count - 2] is VizualizoPorosinePage vp) {
                            await OnPrintTextClickedSelectedPorosia();
                        }
                        else
                            await OnPrintTextClicked();
                    }
                }
            }
            catch (Exception ex) {
                UserDialogs.Instance.Alert("Problem me servis te printerit, ju lutem provoni perseri ose me vone !", "Verejtje", "Ok");
                await App.Instance.PopPageAsync();
            }

        }

        async Task OnPrintTextClicked() {

            // Prepares to communicate with the printer 
            _printer = await OpenPrinterService(_connectionInfo) as MPosControllerPrinter;

            if (_printer == null)
                return;

            try {
                await _printSemaphore.WaitAsync();

                uint textCount = 0;
                string printText = "_Hola Xamarin\n";


                //sd.AddToPreview();
                // sd.Preview();
                //lRet = await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosCodePage.MPOS_CODEPAGE_WPC1252 });

                // note : Page mode and transaction mode cannot be used together between IN and OUT.
                // When "setTransaction" function called with "MPOS_PRINTER_TRANSACTION_IN", print data are stored in the buffer.
                //await _printer.setTransaction((int)MPosTransactionMode.MPOS_PRINTER_TRANSACTION_IN);
                // Printer Setting Initialize
                await _printer.directIO(new byte[] { 0x1b, 0x40 });

                // Code Pages for the contries in east Asia. Please note that the font data downloading is required to print characters for Korean, Japanese and Chinese.
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosEastAsiaCodePage.MPOS_CODEPAGE_KSC5601 });   // Korean
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosEastAsiaCodePage.MPOS_CODEPAGE_SHIFTJIS });  // Japanese
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosEastAsiaCodePage.MPOS_CODEPAGE_GB2312 });    // Simplifies Chinese
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosEastAsiaCodePage.MPOS_CODEPAGE_BIG5 });      // Traditional Chinese
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosCodePage.MPOS_CODEPAGE_FARSI });     // Persian 
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosCodePage.MPOS_CODEPAGE_FARSI_II });  // Persian 

                await _printer.setTransaction((int)MPosTransactionMode.MPOS_PRINTER_TRANSACTION_IN);


                await _printer.printBitmap(DependencyService.Get<IPlatformInfo>().GetImgResource(),
                            100/*(int)MPosImageWidth.MPOS_IMAGE_WIDTH_ASIS*/,   // Image Width
                            (int)MPosAlignment.MPOS_ALIGNMENT_CENTER,           // Alignment
                            50,                                                 // brightness
                            true,                                               // Image Dithering
                            true);
                await _printer.printLine(1, 1, 1, 1, 1);
                await _printer.printText("\nR A P O R T I   I   P O R O S I S \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_CENTER, Bold = false, });
                await _printer.printLine(0, 0, 1, 1, 1);
                await _printer.printText(
"---------------------------------------------------------------------\n");


                await _printer.printText("Shitesi: E. H. W.          J61804031V \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });
                await _printer.printText("Adresa: Autostrada Tirane Durres \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });
                await _printer.printText("Tel: 048 200 711        04 356 085 \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });
                await _printer.printText("Depo: " + Agjendi.Depo + "\n");
                await _printer.printText("Data: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "\n");
                await _printer.printText("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");

                await _printer.printText("\nBleresi: ");
                await _printer.printText("\nAdresa: ");
                await _printer.printText("\nNRB:   NRF: \n");

                await _printer.printText("------------------------------------------------------------------------------------------------------------------------------------------");

                await _printer.printText("\nNr Barkodi     Shifra     Pershkrimi                          Sasia\n");
                await _printer.printText("---------------------------------------------------------------------");
                float teGjithaSasit = 0f;
                float teGjithaCmimetNjesi = 0f;
                double teGjitheCmimetTotale = 0f;
                var tvsh = 0m;
                var nrBarkodi = 0;
                SelectedArikujt = new ObservableCollection<Artikulli>((from s in SelectedArikujt
                                                                       orderby s.IDArtikulli
                                                                       select s).ToList());
                foreach (var art in SelectedArikujt) {
                    string prntBuilder = string.Empty;
                    var emriLength = art.Emri.Length;
                    if (art.Emri.Length > 26) {
                        art.Emri = art.Emri.Remove(26, art.Emri.Length - 26);
                    }
                    if (nrBarkodi >= 10) {
                        prntBuilder += "\n" + nrBarkodi + "            ";
                    }
                    else if (nrBarkodi < 10) {
                        prntBuilder += "\n" + nrBarkodi + "             ";
                    }
                    if (art.IDArtikulli.Length == 7) {
                        prntBuilder += art.IDArtikulli + "    ";
                    }
                    else if (art.IDArtikulli.Length == 6) {
                        prntBuilder += art.IDArtikulli + "     ";
                    }
                    else if (art.IDArtikulli.Length == 5) {
                        prntBuilder += art.IDArtikulli + "      ";
                    }
                    else if (art.IDArtikulli.Length == 4) {
                        prntBuilder += art.IDArtikulli + "       ";
                    }
                    else if (art.IDArtikulli.Length == 3) {
                        prntBuilder += art.IDArtikulli + "        ";
                    }
                    else if (art.IDArtikulli.Length == 2) {
                        prntBuilder += art.IDArtikulli + "         ";
                    }
                    else
                        prntBuilder += art.IDArtikulli + "          ";

                    if (art.Emri.Length == 26) {
                        prntBuilder += art.Emri + "          ";
                    }
                    else if (art.Emri.Length == 25) {
                        prntBuilder += art.Emri + "             ";
                    }
                    else if (art.Emri.Length == 24) {
                        prntBuilder += art.Emri + "              ";
                    }
                    else if (art.Emri.Length == 23) {
                        prntBuilder += art.Emri + "               ";
                    }
                    else if (art.Emri.Length == 22) {
                        prntBuilder += art.Emri + "                ";
                    }
                    else if (art.Emri.Length == 21) {
                        prntBuilder += art.Emri + "                 ";
                    }
                    else if (art.Emri.Length == 20) {
                        prntBuilder += art.Emri + "                  ";
                    }
                    else if (art.Emri.Length == 19) {
                        prntBuilder += art.Emri + "                   ";
                    }
                    else if (art.Emri.Length == 18) {
                        prntBuilder += art.Emri + "                    ";
                    }
                    else if (art.Emri.Length == 17) {
                        prntBuilder += art.Emri + "                     ";
                    }
                    else if (art.Emri.Length == 16) {
                        prntBuilder += art.Emri + "                      ";
                    }
                    else if (art.Emri.Length == 15) {
                        prntBuilder += art.Emri + "                       ";
                    }
                    else if (art.Emri.Length == 14) {
                        prntBuilder += art.Emri + "                        ";
                    }
                    else if (art.Emri.Length == 13) {
                        prntBuilder += art.Emri + "                         ";
                    }
                    else if (art.Emri.Length == 12) {
                        prntBuilder += art.Emri + "                          ";
                    }
                    else if (art.Emri.Length == 11) {
                        prntBuilder += art.Emri + "                           ";
                    }
                    else if (art.Emri.Length == 10) {
                        prntBuilder += art.Emri + "                            ";
                    }
                    else if (art.Emri.Length == 9) {
                        prntBuilder += art.Emri + "                             ";
                    }
                    else if (art.Emri.Length == 8) {
                        prntBuilder += art.Emri + "                              ";
                    }
                    else if (art.Emri.Length == 7) {
                        prntBuilder += art.Emri + "                               ";
                    }
                    else if (art.Emri.Length == 6) {
                        prntBuilder += art.Emri + "                                ";
                    }
                    else
                        prntBuilder += art.Emri + "                                 ";
                    var sasia = String.Format("{0:0.00}", art.Sasia);
                    if (sasia.Length >= 7)
                        prntBuilder = prntBuilder.Remove(prntBuilder.Length - 1, 1);

                    prntBuilder += sasia;

                    teGjithaSasit += (float)art.Sasia;
                    await _printer.printText(prntBuilder);
                    nrBarkodi++;
                }


                await _printer.printText("\n---------------------------------------------------------------------");

                var tegjithaSasisString = String.Format("{0:0.00}", teGjithaSasit);
                if (tegjithaSasisString.Length >= 8) {
                    await _printer.printText("\n                                                             " + tegjithaSasisString);
                }
                else if (tegjithaSasisString.Length >= 7) {
                    await _printer.printText("\n                                                              " + tegjithaSasisString);
                }
                else
                    await _printer.printText("\n                                                               " + tegjithaSasisString);


                //printText = "A. 1. عدد ۰۱۲۳۴۵۶۷۸۹" + "\nB. 2. عدد 0123456789" + "\nC. 3. به" + "\nD. 4. نه" + "\nE. 5. مراجعه" + "\n";// 
                //await _printer.printText(printText, new MPosFontAttribute() { CodePage = (int)MPosCodePage.MPOS_CODEPAGE_FARSI, Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });     // Persian 
                await _printer.printText("\n");
                await _printer.printText("\n");
                await _printer.printText("  Faturoi          Kontrolloi          Vozitesi         Pranoi");
                await _printer.printText("\n");
                await _printer.printText("___________       _____________       ___________      ___________");
                await _printer.printText("\n");
                await _printer.printText("\n");

                // Feed to tear-off position (Manual Cutter Position)
                await _printer.directIO(new byte[] { 0x1b, 0x4a, 0xaf });
            }
            catch (Exception ex) {
                UserDialogs.Instance.Alert("Exception", ex.Message, "OK");
            }
            finally {
                // Printer starts printing by calling "setTransaction" function with "MPOS_PRINTER_TRANSACTION_OUT"
                await _printer.setTransaction((int)MPosTransactionMode.MPOS_PRINTER_TRANSACTION_OUT);
                // If there's nothing to do with the printer, call "closeService" method to disconnect the communication between Host and Printer.
                await _printer.closeService();
                _printSemaphore.Release();
                _printer = null;
                _printSemaphore = null;
            }
        }
        async Task OnPrintTextClickedSelectedPorosia() {

            // Prepares to communicate with the printer 
            _printer = await OpenPrinterService(_connectionInfo) as MPosControllerPrinter;

            if (_printer == null)
                return;

            try {
                await _printSemaphore.WaitAsync();

                uint textCount = 0;
                string printText = "_Hola Xamarin\n";


                //sd.AddToPreview();
                // sd.Preview();
                //lRet = await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosCodePage.MPOS_CODEPAGE_WPC1252 });

                // note : Page mode and transaction mode cannot be used together between IN and OUT.
                // When "setTransaction" function called with "MPOS_PRINTER_TRANSACTION_IN", print data are stored in the buffer.
                //await _printer.setTransaction((int)MPosTransactionMode.MPOS_PRINTER_TRANSACTION_IN);
                // Printer Setting Initialize
                await _printer.directIO(new byte[] { 0x1b, 0x40 });

                // Code Pages for the contries in east Asia. Please note that the font data downloading is required to print characters for Korean, Japanese and Chinese.
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosEastAsiaCodePage.MPOS_CODEPAGE_KSC5601 });   // Korean
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosEastAsiaCodePage.MPOS_CODEPAGE_SHIFTJIS });  // Japanese
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosEastAsiaCodePage.MPOS_CODEPAGE_GB2312 });    // Simplifies Chinese
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosEastAsiaCodePage.MPOS_CODEPAGE_BIG5 });      // Traditional Chinese
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosCodePage.MPOS_CODEPAGE_FARSI });     // Persian 
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosCodePage.MPOS_CODEPAGE_FARSI_II });  // Persian 

                await _printer.setTransaction((int)MPosTransactionMode.MPOS_PRINTER_TRANSACTION_IN);


                await _printer.printBitmap(DependencyService.Get<IPlatformInfo>().GetImgResource(),
                            100/*(int)MPosImageWidth.MPOS_IMAGE_WIDTH_ASIS*/,   // Image Width
                            (int)MPosAlignment.MPOS_ALIGNMENT_CENTER,           // Alignment
                            50,                                                 // brightness
                            true,                                               // Image Dithering
                            true);
                await _printer.printLine(1, 1, 1, 1, 1);
                await _printer.printText("\nR A P O R T I   I   P O R O S I S \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_CENTER, Bold = false, });
                await _printer.printLine(0, 0, 1, 1, 1);
                await _printer.printText(
"---------------------------------------------------------------------\n");


                await _printer.printText("Shitesi: E. H. W.          J61804031V \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });
                await _printer.printText("Adresa: Autostrada Tirane Durres \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });
                await _printer.printText("Tel: 048 200 711        04 356 085 \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });
                await _printer.printText("Numri i Porosise: " + NrPorosise +"\n");
                await _printer.printText("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");

                await _printer.printText("\nBleresi: " + SelectedKlient.Emri);
                await _printer.printText("\nAdresa: ");
                await _printer.printText("\nNRB:   NRF: \n");
                 
                await _printer.printText("------------------------------------------------------------------------------------------------------------------------------------------");

                await _printer.printText("\nNr Barkodi     Shifra     Pershkrimi                          Sasia\n");
                await _printer.printText("---------------------------------------------------------------------");
                float teGjithaSasit = 0f;
                float teGjithaCmimetNjesi = 0f;
                double teGjitheCmimetTotale = 0f;
                var tvsh = 0m;
                var nrBarkodi = 0;
                Artikujt = new ObservableCollection<Artikulli>((from s in Artikujt
                                                                orderby s.IDArtikulli
                                                                       select s).ToList());
                foreach (var art in Artikujt) {
                    string prntBuilder = string.Empty;
                    var emriLength = art.Emri.Length;
                    if (art.Emri.Length > 26) {
                        art.Emri = art.Emri.Remove(26, art.Emri.Length - 26);
                    }
                    if (nrBarkodi >= 10) {
                        prntBuilder += "\n" + nrBarkodi + "            ";
                    }
                    else if (nrBarkodi < 10) {
                        prntBuilder += "\n" + nrBarkodi + "             ";
                    }
                    if (art.IDArtikulli.Length == 7) {
                        prntBuilder += art.IDArtikulli + "    ";
                    }
                    else if (art.IDArtikulli.Length == 6) {
                        prntBuilder += art.IDArtikulli + "     ";
                    }
                    else if (art.IDArtikulli.Length == 5) {
                        prntBuilder += art.IDArtikulli + "      ";
                    }
                    else if (art.IDArtikulli.Length == 4) {
                        prntBuilder += art.IDArtikulli + "       ";
                    }
                    else if (art.IDArtikulli.Length == 3) {
                        prntBuilder += art.IDArtikulli + "        ";
                    }
                    else if (art.IDArtikulli.Length == 2) {
                        prntBuilder += art.IDArtikulli + "         ";
                    }
                    else
                        prntBuilder += art.IDArtikulli + "          ";

                    if (art.Emri.Length == 26) {
                        prntBuilder += art.Emri + "          ";
                    }
                    else if (art.Emri.Length == 25) {
                        prntBuilder += art.Emri + "             ";
                    }
                    else if (art.Emri.Length == 24) {
                        prntBuilder += art.Emri + "              ";
                    }
                    else if (art.Emri.Length == 23) {
                        prntBuilder += art.Emri + "               ";
                    }
                    else if (art.Emri.Length == 22) {
                        prntBuilder += art.Emri + "                ";
                    }
                    else if (art.Emri.Length == 21) {
                        prntBuilder += art.Emri + "                 ";
                    }
                    else if (art.Emri.Length == 20) {
                        prntBuilder += art.Emri + "                  ";
                    }
                    else if (art.Emri.Length == 19) {
                        prntBuilder += art.Emri + "                   ";
                    }
                    else if (art.Emri.Length == 18) {
                        prntBuilder += art.Emri + "                    ";
                    }
                    else if (art.Emri.Length == 17) {
                        prntBuilder += art.Emri + "                     ";
                    }
                    else if (art.Emri.Length == 16) {
                        prntBuilder += art.Emri + "                      ";
                    }
                    else if (art.Emri.Length == 15) {
                        prntBuilder += art.Emri + "                       ";
                    }
                    else if (art.Emri.Length == 14) {
                        prntBuilder += art.Emri + "                        ";
                    }
                    else if (art.Emri.Length == 13) {
                        prntBuilder += art.Emri + "                         ";
                    }
                    else if (art.Emri.Length == 12) {
                        prntBuilder += art.Emri + "                          ";
                    }
                    else if (art.Emri.Length == 11) {
                        prntBuilder += art.Emri + "                           ";
                    }
                    else if (art.Emri.Length == 10) {
                        prntBuilder += art.Emri + "                            ";
                    }
                    else if (art.Emri.Length == 9) {
                        prntBuilder += art.Emri + "                             ";
                    }
                    else if (art.Emri.Length == 8) {
                        prntBuilder += art.Emri + "                              ";
                    }
                    else if (art.Emri.Length == 7) {
                        prntBuilder += art.Emri + "                               ";
                    }
                    else if (art.Emri.Length == 6) {
                        prntBuilder += art.Emri + "                                ";
                    }
                    else
                        prntBuilder += art.Emri + "                                 " ;
                    var sasia = String.Format("{0:0.00}", art.Sasia);
                    if (sasia.Length >= 7) 
                        prntBuilder = prntBuilder.Remove(prntBuilder.Length - 1, 1);

                    prntBuilder += sasia;

                    teGjithaSasit += (float)art.Sasia;
                    await _printer.printText(prntBuilder);
                    nrBarkodi++;
                }


                await _printer.printText("\n---------------------------------------------------------------------");
                var tegjithaSasisString = String.Format("{0:0.00}", teGjithaSasit);
                if (tegjithaSasisString.Length >= 8) {
                    await _printer.printText("\n                                                             " + tegjithaSasisString);
                }
                else if (tegjithaSasisString.Length >= 7) {
                    await _printer.printText("\n                                                              " + tegjithaSasisString);
                }
                else
                    await _printer.printText("\n                                                               " + tegjithaSasisString);


                //printText = "A. 1. عدد ۰۱۲۳۴۵۶۷۸۹" + "\nB. 2. عدد 0123456789" + "\nC. 3. به" + "\nD. 4. نه" + "\nE. 5. مراجعه" + "\n";// 
                //await _printer.printText(printText, new MPosFontAttribute() { CodePage = (int)MPosCodePage.MPOS_CODEPAGE_FARSI, Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });     // Persian 
                await _printer.printText("\n");
                await _printer.printText("\n");
                await _printer.printText("  Faturoi          Kontrolloi          Vozitesi         Pranoi");
                await _printer.printText("\n");
                await _printer.printText("___________       _____________       ___________      ___________");
                await _printer.printText("\n");
                await _printer.printText("\n");

                // Feed to tear-off position (Manual Cutter Position)
                await _printer.directIO(new byte[] { 0x1b, 0x4a, 0xaf });
            }
            catch (Exception ex) {
                UserDialogs.Instance.Alert("Exception", ex.Message, "OK");
            }
            finally {
                // Printer starts printing by calling "setTransaction" function with "MPOS_PRINTER_TRANSACTION_OUT"
                await _printer.setTransaction((int)MPosTransactionMode.MPOS_PRINTER_TRANSACTION_OUT);
                // If there's nothing to do with the printer, call "closeService" method to disconnect the communication between Host and Printer.
                await _printer.closeService();
                _printSemaphore.Release();
                _printer = null;
                _printSemaphore = null;
            }
        }

        public async Task HapPorositeAsync() {
            if(CurrentlySelectedOrder == null) {
                UserDialogs.Instance.Alert("Ju lutem selektoni njeren nga porosite");
                return;
            }
            var orderDetails = await App.Database.GetOrderDetailsAsync();
            orderDetails = orderDetails.Where(x => x.IDOrder == CurrentlySelectedOrder.IDOrder).ToList();
            await App.Instance.MainPage.Navigation.PopPopupAsync();
            var artikujt = await App.Database.GetArtikujtAsync();
            Artikujt = new ObservableCollection<Artikulli>();
            foreach (var orderd in orderDetails) {
                foreach(var art in artikujt) {
                    if(art.IDArtikulli == orderd.IDArtikulli) {
                        Artikulli arti = new Artikulli
                        {
                            BUM = art.BUM,
                            Sasia = orderd.SasiaPorositur,
                            Emri = art.Emri,
                            IDArtikulli = art.IDArtikulli
                        };
                        Artikujt.Add(arti);
                    }
                }
            }
            CurrentlySelectedOrderDetails = orderDetails;
            await App.Instance.PushAsyncNewPage(new VizualizoPorosinePage { BindingContext = this });
        }

        public async Task RegjistroAsync() 
        {
            Location loc = await Xamarin.Essentials.Geolocation.GetLastKnownLocationAsync();

            Orders order = new Orders
            {
                ID = Guid.NewGuid(),
                DeviceID = Agjendi.DeviceID,
                IDAgjenti = Agjendi.IDAgjenti,
                Data = DateTime.Now,
                Depo = Agjendi.Depo,
                IDKlientDheLokacion = SelectedKlient?.IDKlienti,
                IDOrder = NrPorosise,
                ImpStatus = 0,
                SyncStatus = 0,
                Latitude = loc?.Latitude.ToString(),
                Longitude = loc?.Longitude.ToString()
            };

            var res = await App.Database.SaveOrderAsync(order);
            if(res == 1) {
                foreach (var art in SelectedArikujt) {
                    OrderDetails orderDetails = new OrderDetails
                    {
                        Emri = art.Emri,
                        IDArtikulli = art.IDArtikulli,
                        IDOrder = order.IDOrder,
                        ImpStatus = order.ImpStatus,
                        NrRendor = NrRendor,
                        SasiaPorositur = art.Sasia,
                        SyncStatus = 0
                    };
                    await App.Database.SaveOrderDetailsAsync(orderDetails);
                }

                UserDialogs.Instance.Alert("Porosia u shtua me sukses");
                await App.Instance.PopPageAsync();
                NrRendor = NrRendor + 1;
                OrdersList.Add(order);
            }

        }

        private ObservableCollection<Orders> _OrdersList;
        public ObservableCollection<Orders> OrdersList {
            get { return _OrdersList; }
            set { SetProperty(ref _OrdersList, value); }
        }
        private ObservableCollection<Artikulli> _SelectedArikujt;
        public ObservableCollection<Artikulli> SelectedArikujt {
            get { return _SelectedArikujt; }
            set { SetProperty(ref _SelectedArikujt, value); }
        }
        public async Task AddArtikulliAsync() {
            if (SelectedArikujt == null) {
                SelectedArikujt = new ObservableCollection<Artikulli>();
            }
            if (CurrentlySelectedArtikulli == null) return;
            if(Sasia < 0) {
                UserDialogs.Instance.Alert("Nuk lejohet sasi negative");
                Sasia = Sasia * -1;
                return;
            }
            if (CurrentlySelectedArtikulli.BUM != "KG") {
                if (Sasia % 1 != 0) {
                    UserDialogs.Instance.Alert("Ju lutemi permiresoni sasin, arikulli lejon vetem numra te plote pasi qe eshte " + CurrentlySelectedArtikulli.BUM);
                    Sasia = (float)(int)(Sasia);
                    return;
                }
            }
            if (Sasia > CurrentlySelectedArtikulli.Sasia) {
                UserDialogs.Instance.Alert("Ju lutemi permiresoni sasin, nuk duhet te jete me shume se sasia aktualle e mallit " + CurrentlySelectedArtikulli.Sasia, "Verejtje", "Ok");
                return;
            }
            if (SelectedArikujt.FirstOrDefault(x => x.IDArtikulli == CurrentlySelectedArtikulli.IDArtikulli) != null) {
                UserDialogs.Instance.Alert("Artikulkli " + CurrentlySelectedArtikulli.Emri + " eshte shtuar njehere, nese eshte bere gabim ne sasi, ju lutem fshini artikullin dhe shtoni perseri.", "Verejtje", "Ok");
                CurrentlySelectedArtikulli = null;
                Sasia = 0;
                return;
            }
            CurrentlySelectedArtikulli.Sasia = Sasia;
            SelectedArikujt.Add(CurrentlySelectedArtikulli);
            CurrentlySelectedArtikulli = null;
            Sasia = 0;
        }

        public void FshijArtikullinAsync() {
            SelectedArikujt.Remove(CurrentlySelectedArtikulli);
            CurrentlySelectedArtikulli = null;
        }

        public void IncreaseSasia() {
            Sasia = Sasia + 1;
        }
        public void DecreaseSasia() {
            if (Sasia == 0) {
                return;
            }
            Sasia = Sasia - 1;

        }
        public ICommand IncreaseSasiaCommand { get; set; }
        public ICommand DecreaseSasiaCommand { get; set; }
        private int _SelectedKlientIndex;
        public int SelectedKlientIndex {
            get { return _SelectedKlientIndex; }
            set { SetProperty(ref _SelectedKlientIndex, value);}
        }
        private bool _krijoPorosine;
        public bool KrijoPorosine {
            get { return _krijoPorosine;}
            set { SetProperty(ref _krijoPorosine, value);}
        }

        private List<OrderDetails> _CurrentlySelectedOrderDetails;
        public List<OrderDetails> CurrentlySelectedOrderDetails {
            get {
                return _CurrentlySelectedOrderDetails;
            }
            set { SetProperty(ref _CurrentlySelectedOrderDetails, value); }
        }

        private Orders _CurrentlySelectedOrder;
        public Orders CurrentlySelectedOrder {
            get {
                return _CurrentlySelectedOrder;
            }
            set { SetProperty(ref _CurrentlySelectedOrder, value); }
        }

        private Artikulli _currentlySelectedArtikulli;
        public Artikulli CurrentlySelectedArtikulli {
            get {
                return _currentlySelectedArtikulli;
            }
            set { SetProperty(ref _currentlySelectedArtikulli, value); }
        }
        private float _sasia;
        public float Sasia {
            get { return _sasia; }
            set { SetProperty(ref _sasia, value); }
        }
        public async void ShtoKlientinNeListe() {
            if(SelectedKlient == null) {
                UserDialogs.Instance.Alert("Zgjedhni klientin fillimisht", "Verejtje", "Ok");
                return;
            }
            KrijimiPorosive krijimiPorosive = new KrijimiPorosive
            {
                KPID = Guid.NewGuid(),
                Data_Regjistrimit = DateTime.Now,
                IDKlienti = SelectedKlient.IDKlienti,
                Emri = SelectedKlient?.Emri,
                Depo = Agjendi.Depo,
                DeviceID = Agjendi.DeviceID,
                IDAgjenti = Agjendi.IDAgjenti,
                Imp_Status = 0,
                SyncStatus = 0
            };
            KrijimiPorositeKlientet.Add(krijimiPorosive);
            await App.Database.SaveKrijimiPorosiveAsync(krijimiPorosive);
        }

        private ObservableCollection<Artikulli> _artikujt;
        public ObservableCollection<Artikulli> Artikujt {
            get { return _artikujt; }
            set { SetProperty(ref _artikujt, value); }
        }
        private ObservableCollection<Artikulli> _SearchedArtikujt;
        public ObservableCollection<Artikulli> SearchedArtikujt {
            get { return _SearchedArtikujt; }
            set { SetProperty(ref _SearchedArtikujt, value); }
        }

        public async Task ZgjedhKlientet() {
            try {
                UserDialogs.Instance.ShowLoading("Duke hapur artikujt");
                ZgjidhKlientetModalPage ZgjidhKlientetModalPage = new ZgjidhKlientetModalPage();
                ZgjidhKlientetModalPage.BindingContext = this;

                await App.Instance.PushAsyncNewModal(ZgjidhKlientetModalPage);
                UserDialogs.Instance.HideLoading();

            }
            catch (Exception e) {
                var g = e.Message;
            }
        }

        public async Task ZgjedhArtikullinAsync() {
            try {
                UserDialogs.Instance.ShowLoading("Duke hapur artikujt");
                ZgjidhArtikullinModalPage zgjidhArtikullinModalPage = new ZgjidhArtikullinModalPage();
                zgjidhArtikullinModalPage.BindingContext = this;
                Artikujt = new ObservableCollection<Artikulli>(await App.Database.GetArtikujtAsync());


                await App.Instance.PushAsyncNewModal(zgjidhArtikullinModalPage);
                UserDialogs.Instance.HideLoading();

            }
            catch (Exception e) {
                var g = e.Message;
            }

        }

        public async Task GoToKrijoListenAsync() {
            KrijoPorositePage krijoPorositePage = new KrijoPorositePage();
            krijoPorositePage.BindingContext = this;
            await App.Instance.PushAsyncNewPage(krijoPorositePage);
        }

        private string _NrPorosise;
        public string NrPorosise {
            get { return _NrPorosise; }
            set { SetProperty(ref _NrPorosise, value);}
        }

        public async Task GoToKrijoPorosineAsync() {
            NrPorosise = "PRS-" + App.Instance.MainViewModel.LoginData.DeviceID + "-" + DateTime.Now.ToString("yyMMdd")+ "-" + NrRendor.ToString("00");
            KrijoPorosinePage kpPorosia = new KrijoPorosinePage();
            kpPorosia.BindingContext = this;
            SelectedKlient = Klientet.FirstOrDefault(x => x.Depo.Trim() == Agjendi.Depo && x.Emri.Contains(Agjendi.Depo));
            SelectedKlientIndex = Klientet.FindIndex(x=> x.Depo.Trim() == Agjendi.Depo && x.Emri.Contains(Agjendi.Depo));
            await App.Instance.PushAsyncNewPage(kpPorosia);
            SelectedArikujt = new ObservableCollection<Artikulli>();
        }

        public async Task KonvertoPorosine() {
            
        }
    }
}
