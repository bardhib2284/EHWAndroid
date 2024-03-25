using Acr.UserDialogs;
using EHW_M;
using EHWM.DependencyInjections.FiskalizationExtraModels;
using EHWM.Models;
using EHWM.Services;
using EHWM.Views;
using Newtonsoft.Json;
using Plugin.BxlMpXamarinSDK;
using Plugin.BxlMpXamarinSDK.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EHWM.ViewModel {
    public class LevizjetNavigationParameters {
        public List<Depot> Klientet { get; set; }
        public List<Artikulli> Artikujt { get; set; }
        public Agjendet Agjendi { get; set; }
        public List<LevizjetHeader> LevizjetHeader { get; set;}
    }

    public class LevizjetViewModel : BaseViewModel{
        private ObservableCollection<Artikulli> _SearchedArtikujt;
        public ObservableCollection<Artikulli> SearchedArtikujt {
            get { return _SearchedArtikujt; }
            set { SetProperty(ref _SearchedArtikujt, value); }
        }
        public ObservableCollection<Depot> Klientet { get; set; }
        private ObservableCollection<Artikulli> _artikujt;
        public ObservableCollection<Artikulli> Artikujt {
            get { return _artikujt; }
            set { SetProperty(ref _artikujt, value); }
        }

        private ObservableCollection<LevizjetDetails> _LevizjetDetails;
        public ObservableCollection<LevizjetDetails> LevizjetDetails {
            get { return _LevizjetDetails; }
            set { SetProperty(ref _LevizjetDetails, value); }
        }
        private ObservableCollection<LevizjetHeader> _LevizjetHeader;
        public ObservableCollection<LevizjetHeader> LevizjetHeader {
            get { return _LevizjetHeader; }
            set { SetProperty(ref _LevizjetHeader, value); }
        }
        public Agjendet Agjendi { get; set; }

        private Artikulli _selectedArtikullu;
        public Artikulli SelectedArtikulli {
            get { return _selectedArtikullu; }
            set { SetProperty(ref _selectedArtikullu, value); }
        }
        private Depot _SelectedKlientet;
        public Depot SelectedKlientet {
            get { return _SelectedKlientet; }
            set { SetProperty(ref _SelectedKlientet, value); }
        }
        private string _LevizjeText;
        public string LevizjeText {
            get { return _LevizjeText; }
            set { SetProperty(ref _LevizjeText, value); }
        }
        public ICommand ZgjedhArtikullinCommand { get; set; }
        public ICommand AddArtikulliCommand { get; set; }
        public ICommand RegjistroCommand { get; set; }
        public ICommand ShtoLevizjenCommand { get; set; }
        public ICommand PrintoLevizjenCommand { get; set; }

        public ICommand FshijArtikullinCommand { get; set; }
        private bool _hasAnArticleBeenSelected;
        public bool HasAnArticleBeenSelected {
            get { return _hasAnArticleBeenSelected; }
            set { SetProperty(ref _hasAnArticleBeenSelected, value); }
        }
        
        private bool _nga;
        public bool Nga {
            get { return _nga; }
            set 
                {
                if(value) {
                    if (NumriFraturave != null)
                        NrLevizjes = (NumriFraturave.NRKUFIP_D + NumriFraturave.CurrNrFat_D).ToString();
                }

                LevizjeText = "Transfer nga"; 
                SetProperty(ref _nga, value); 
            }
        }
        private bool _Ne;
        public bool Ne {
            get { return _Ne; }
            set 
                {
                if(value) {
                    if (NumriPorosive != null)
                        NrLevizjes = "M03-" + NumriPorosive.NrPorosise;
                }

                LevizjeText = "Transfer ne";
                SetProperty(ref _Ne, value); 
            }
        }

        private double _totalPrice;
        public double TotalPrice {
            get { return _totalPrice; }
            set { SetProperty(ref _totalPrice, value); }
        }
        private bool _LevizjaEPerfunduar;
        public bool LevizjaEPerfunduar {
            get { return _LevizjaEPerfunduar; }
            set { SetProperty(ref _LevizjaEPerfunduar, value); }
        }
        public LevizjetViewModel(LevizjetNavigationParameters levizjetNavigationParameters) {
            Klientet = new ObservableCollection<Depot>(levizjetNavigationParameters.Klientet);
            Artikujt = new ObservableCollection<Artikulli>(levizjetNavigationParameters.Artikujt);
            Agjendi = levizjetNavigationParameters.Agjendi;
            LevizjetHeader = new ObservableCollection<LevizjetHeader>(levizjetNavigationParameters.LevizjetHeader);
            ZgjedhArtikullinCommand = new Command(async () => await ZgjedhArtikullinAsync());
            HasAnArticleBeenSelected = false;
            AddArtikulliCommand = new Command(async () => await AddArtikulliAsync());
            RegjistroCommand = new Command(async () => await RegjistroLevizjenAsync());
            ShtoLevizjenCommand = new Command(async () => await ShtoLevizjenAsync());
            FshijArtikullinCommand = new Command(FshijArtikullinAsync);
            PrintoLevizjenCommand = new Command(async () => await PrintoFaturenAsync());
            SearchedArtikujt = new ObservableCollection<Artikulli>();
            Nga = true;
            LevizjaEPerfunduar = false;
            LevizjeText = "Levizje nga";
        }
        async Task<MPosControllerDevices> OpenPrinterService(MposConnectionInformation connectionInfo) {
            if (connectionInfo == null)
                return null;

            if (_printer != null)
                return _printer;

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
            if (_printSemaphore == null) {
                _printSemaphore = new SemaphoreSlim(1, 1);
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
        public MPosControllerPrinter _printer;
        public MposConnectionInformation _connectionInfo;
        public static SemaphoreSlim _printSemaphore = new SemaphoreSlim(1, 1);

        public async Task PrintoFaturenAsync() {
            await App.Instance.PushAsyncNewPage(new PrinterSelectionPage() { BindingContext = this });
        }
        public async Task OnDeviceOpenClicked() {
            if (_printer == null) {
                // Prepares to communicate with the printer 
                _printer = await OpenPrinterService(_connectionInfo) as MPosControllerPrinter;
                if (CurrentlySelectedLevizjetHeader == null) {
                    UserDialogs.Instance.Alert("Ju lutem zgjedhni njeren prej faturave");
                    return;
                }
                await OnPrintTextClicked(CurrentlySelectedLevizjetHeader);
            }
            else {

            }
        }
        async Task OnPrintTextClicked(LevizjetHeader CurrentlySelectedLevizjetHeader) {

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
                await _printer.setTransaction((int)MPosTransactionMode.MPOS_PRINTER_TRANSACTION_IN);


                await _printer.printBitmap(DependencyService.Get<IPlatformInfo>().GetImgResource(),
                            100/*(int)MPosImageWidth.MPOS_IMAGE_WIDTH_ASIS*/,   // Image Width
                            (int)MPosAlignment.MPOS_ALIGNMENT_CENTER,           // Alignment
                            50,                                                 // brightness
                            true,                                               // Image Dithering
                            true);
                // Code Pages for the contries in east Asia. Please note that the font data downloading is required to print characters for Korean, Japanese and Chinese.
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosEastAsiaCodePage.MPOS_CODEPAGE_KSC5601 });   // Korean
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosEastAsiaCodePage.MPOS_CODEPAGE_SHIFTJIS });  // Japanese
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosEastAsiaCodePage.MPOS_CODEPAGE_GB2312 });    // Simplifies Chinese
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosEastAsiaCodePage.MPOS_CODEPAGE_BIG5 });      // Traditional Chinese
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosCodePage.MPOS_CODEPAGE_FARSI });     // Persian 
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosCodePage.MPOS_CODEPAGE_FARSI_II });  // Persian 
                var depot = await App.Database.GetDepotAsync();
                var depo = depot.FirstOrDefault(x => x.Depo == CurrentlySelectedLevizjetHeader.LevizjeNe);

                var currentDepo = depot.FirstOrDefault(x => x.Depo == Agjendi.Depo);
                var agjendet = await App.Database.GetAgjendetAsync();
                var currAgjendi = agjendet.FirstOrDefault(x => x.Depo == Agjendi.Depo);
                if ((CurrentlySelectedLevizjetHeader.Latitude == "8" && CurrentlySelectedLevizjetHeader.Longitude == "8") || CurrentlySelectedLevizjetHeader.LevizjeNga == Agjendi.Depo) {
                    await _printer.printLine(1, 1, 1, 1, 1);
                    await _printer.printText("\nF A T U R E    S H O Q E R U E S E \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_CENTER, Bold = false, });
                    await _printer.printLine(0, 0, 1, 1, 1);
                    await _printer.printText(
    "---------------------------------------------------------------------\n");


                    await _printer.printText("Emri i Nisesit: E. H. W.          J61804031V \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });
                    await _printer.printText("Tel: 048 200 711           web: www.ehwgmbh.com \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });
                    await _printer.printText("Adresa: AA951IN             \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });
                    await _printer.printText("Qyteti / Shteti: Tirana, Albania \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });
                    await _printer.printText("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");

                    await _printer.printText("\nNumri i fatures: " + CurrentlySelectedLevizjetHeader.NumriFisk + "/" + CurrentlySelectedLevizjetHeader.Data.Value.Year);
                    if (CurrentlySelectedLevizjetHeader.Latitude == "8" && CurrentlySelectedLevizjetHeader.Longitude == "8")
                        await _printer.printText("      Numri i serise: FISK-" + CurrentlySelectedLevizjetHeader.NumriFisk + "-" + Agjendi.Depo);
                    else
                        await _printer.printText("      Numri i serise: " + Agjendi.Depo + "-" + CurrentlySelectedLevizjetHeader.NumriFisk);
                    await _printer.printText("\nData dhe ora e leshimit te fatures: " + CurrentlySelectedLevizjetHeader.Data.Value.ToString("dd.MM.yyyy HH:mm:ss"));

                    await _printer.printText("\nKodi i vendit te ushtrimit te veprimtarise se biznesit: " + CurrentlySelectedLevizjetHeader.TCRBusinessUnitCode);
                    await _printer.printText("\nKodi i operatorit : " + CurrentlySelectedLevizjetHeader.TCROperatorCode);

                    await _printer.printText("\nNIVFSH:  " + CurrentlySelectedLevizjetHeader.TCRNIVFSH);
                    await _printer.printText("\nNSLFSH:  " + CurrentlySelectedLevizjetHeader.TCRNSLFSH);
                    await _printer.printText("\nQellimi i Levizjes se Mallit:  Shitje me shumice\n");
                    await _printer.printText(
    "---------------------------------------------------------------------");
                    await _printer.printText("\nEmri i Pritesit: E. H. W.   NIPT: " + depo.NIPT);
                    await _printer.printText("\nAdresa: " + depo.ADRESA + "\n");


                    await _printer.printText(
    "---------------------------------------------------------------------");
                    await _printer.printText("\nTransportues:  " + currAgjendi.Emri.ToUpper() + " " + currAgjendi.Mbiemri.ToUpper() + "  J61804031V");
                    await _printer.printText("\nAdresa: Autostrada Tirane Durres");
                    await _printer.printText("\nMjeti: " + currentDepo.TARGE);
                    await _printer.printText("\nData dhe ora e furnizimit:  " + CurrentlySelectedLevizjetHeader.Data.Value.ToString("dd.MM.yyyy HH:mm:ss") + " \n");

                }
                else if (CurrentlySelectedLevizjetHeader.LevizjeNe == Agjendi.Depo) {
                    //NGA
                    await _printer.printLine(1, 1, 1, 1, 1);
                    await _printer.printText("\nH Y R J E\n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_CENTER, Bold = false, });
                    await _printer.printText(
    "---------------------------------------------------------------------\n");
                    await _printer.printText("\n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });
                    await _printer.printText("", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });
                    await _printer.printText("\n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });
                    await _printer.printText("", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });
                    await _printer.printText("\nHyrje nga :  " + CurrentlySelectedLevizjetHeader.LevizjeNga);
                    await _printer.printText("\nMjeti: " + currentDepo.TARGE);
                    await _printer.printText("\nData dhe ora e furnizimit:  " + CurrentlySelectedLevizjetHeader.Data.Value.ToString("dd.MM.yyyy HH:mm:ss") + " \n");
                    if (CurrentlySelectedLevizjetHeader.NumriLevizjes.Contains("-")) {
                        var nrPorosiseDepo = CurrentlySelectedLevizjetHeader.NumriLevizjes.Split('-');
                        if (CurrentlySelectedLevizjetHeader.NumriFisk == null) {
                            await _printer.printText("Nr.Dok : " + Agjendi.Depo + "-" + nrPorosiseDepo[1] + "\n");
                        }
                        else
                            await _printer.printText("Nr.Dok : " + CurrentlySelectedLevizjetHeader.NumriFisk + "-" + nrPorosiseDepo[1] + "-" + Agjendi.Depo + "\n");
                    }
                    else
                        await _printer.printText("Nr.Dok : " + CurrentlySelectedLevizjetHeader.NumriLevizjes + "\n");
                }





                await _printer.printText(
"---------------------------------------------------------------------");
                await _printer.printText(
"---------------------------------------------------------------------");
                Debug.WriteLine("\nKodi   Pershkrimi  Njesia  Sasia  Cmimi   V.PaTVSH    TVSH    VLERA  \n");
                await _printer.printText("\nKodi   Pershkrimi  Njesia  Sasia  Cmimi   V.PaTVSH    TVSH    VLERA  \n");
                await _printer.printText("---------------------------------------------------------------------");
                ;
                float teGjithaSasit = 0f;
                float teGjithaCmimetNjesi = 0f;
                double teGjitheCmimetTotale = 0f;
                decimal tegjithaCmimetVPaTVSH = 0m;
                decimal vleraTVSH = 0m;
                var artikujt = await App.Database.GetArtikujtAsync();
                int maxLength = 69;
                string builderToPrint = string.Empty;
                int reservedSpaceForEachElement = 10;
                string emptySpace;
                string sPranuar;
                string sShitur;
                string sKthyer;
                string slevizje;
                string smbetur;
                string scmimi;
                CurrentlySelectedLevizjetDetails = new ObservableCollection<LevizjetDetails>((from s in CurrentlySelectedLevizjetDetails
                                                    orderby s.IDArtikulli
                                                    select s).ToList());

                foreach (var ld in CurrentlySelectedLevizjetDetails) {
                    foreach (var art in Artikujt) {
                        if (art.IDArtikulli != ld.IDArtikulli) continue;
                        await _printer.printText("\n" + art.IDArtikulli + "   " + art.Emri + "   " + ld.Seri);
                        reservedSpaceForEachElement = 10;
                        emptySpace = "\n                  ";
                        sPranuar = art.BUM;
                        sShitur = String.Format("{0:0.00}", ld.Sasia);
                        sKthyer = String.Format("{0:0.00}", (ld.Cmimi / 1.2m)) ;
                        slevizje = String.Format("{0:0.00}", Math.Round((decimal)((ld.Cmimi * ld.Sasia) / 1.2m), 2));
                        smbetur = String.Format("{0:0.00}", (decimal.Parse((ld.Cmimi * ld.Sasia).ToString()) - Math.Round((decimal)((ld.Cmimi * ld.Sasia) / 1.2m), 2)));
                        scmimi = String.Format("{0:0.00}", ld.Cmimi * ld.Sasia);
                        if (sPranuar.Length < (reservedSpaceForEachElement - 2)) {
                            var index = 1;
                            while (sPranuar.Length < (reservedSpaceForEachElement - 2)) {
                                if (index > 0) {
                                    sPranuar = sPranuar.Insert(0, " ");
                                }
                                else
                                    sPranuar += " ";
                                index *= -1;
                            }
                        }
                        if (sShitur.Length < (reservedSpaceForEachElement - 4)) {
                            var index = 1;
                            while (sShitur.Length < (reservedSpaceForEachElement - 4)) {
                                if (index > 0) {
                                    sShitur = sShitur.Insert(0, " ");
                                }
                                else
                                    sShitur += " ";
                                index *= -1;
                            }
                        }
                        if (sKthyer.Length < reservedSpaceForEachElement) {
                            var index = 1;
                            while (sKthyer.Length < reservedSpaceForEachElement) {
                                if (index > 0) {
                                    sKthyer = sKthyer.Insert(0, " ");
                                }
                                else
                                    sKthyer += " ";
                                index *= -1;
                            }
                        }

                        if (slevizje.Length < reservedSpaceForEachElement) {
                            var index = 1;
                            while (slevizje.Length < reservedSpaceForEachElement) {
                                if (index > 0) {
                                    slevizje = slevizje.Insert(0, " ");
                                }
                                else
                                    slevizje += " ";
                                index *= -1;
                            }
                        }
                        if (smbetur.Length < reservedSpaceForEachElement) {
                            var index = 1;
                            while (smbetur.Length < reservedSpaceForEachElement) {
                                if (index > 0) {
                                    smbetur = smbetur.Insert(0, " ");
                                }
                                else
                                    smbetur += " ";
                                index *= -1;
                            }
                        }
                        if (scmimi.Length < reservedSpaceForEachElement) {
                            var index = 1;
                            while (scmimi.Length < reservedSpaceForEachElement) {
                                if (index > 0) {
                                    scmimi = scmimi.Insert(0, " ");
                                }
                            }
                        }

                        emptySpace += sPranuar + sShitur + sKthyer + slevizje + smbetur + scmimi;

                        if (emptySpace.Length < 70) {
                            bool wasLastEmpty = false;
                            string totalTemp = emptySpace;
                            for (int i = emptySpace.Length - 1; i >= 0; i--) {
                                var de = emptySpace[i];
                                if (wasLastEmpty) {
                                    if (emptySpace[i] == ' ') {
                                        wasLastEmpty = true;
                                        totalTemp = totalTemp.Remove(i, 1);
                                        if (totalTemp.Length == 70) {
                                            emptySpace += " ";
                                            break;
                                        }
                                    }
                                }
                                if (emptySpace[i] == ' ') {
                                    wasLastEmpty = true;
                                }
                                else {
                                    wasLastEmpty = false;
                                }
                            }
                        }
                        if (emptySpace.Length > 70) {
                            bool wasLastEmpty = false;
                            string totalTemp = emptySpace;
                            for (int i = emptySpace.Length - 1; i >= 0; i--) {
                                var de = emptySpace[i];
                                if (wasLastEmpty) {
                                    if (emptySpace[i] == ' ') {
                                        wasLastEmpty = true;
                                        totalTemp = totalTemp.Remove(i, 1);
                                        if (totalTemp.Length == 70) {
                                            emptySpace = totalTemp;
                                            break;
                                        }
                                    }
                                }
                                if (emptySpace[i] == ' ') {
                                    wasLastEmpty = true;
                                }
                                else {
                                    wasLastEmpty = false;
                                }
                            }
                        }
                        await _printer.printText(emptySpace);
                        Debug.WriteLine(emptySpace);

                        teGjithaSasit += (float)ld.Sasia;
                        teGjithaCmimetNjesi += float.Parse((ld.Cmimi * ld.Sasia).ToString()) - (float)((ld.Cmimi * ld.Sasia) / 1.2m);
                        teGjitheCmimetTotale += (double)ld.Totali;
                        vleraTVSH += (decimal.Parse((ld.Cmimi * ld.Sasia).ToString()) - Math.Round((decimal)((ld.Cmimi * ld.Sasia) / 1.2m), 2));
                        tegjithaCmimetVPaTVSH += Math.Round((decimal)((ld.Cmimi * ld.Sasia) / 1.2m), 2);
                    }
                }

                await _printer.printText("\n---------------------------------------------------------------------");
                string totalBuilder = string.Empty;
                reservedSpaceForEachElement = 15;
                emptySpace = "\n Mbyllet me total  ";
                var tvshAll = String.Format("{0:0.00}", vleraTVSH);
                var cmimTotal = String.Format("{0:0.00}", teGjitheCmimetTotale);
                //v.patvsh
                var vptvsh = String.Format("{0:0.00}", tegjithaCmimetVPaTVSH);                //v.patvsh
                sPranuar = String.Format("{0:0.00}", teGjithaSasit);
                sShitur = String.Format("{0:0.00}", tegjithaCmimetVPaTVSH);
                slevizje = String.Format("{0:0.00}", Math.Round(((decimal)teGjitheCmimetTotale - tegjithaCmimetVPaTVSH), 2));
                smbetur = cmimTotal;
                if (sPranuar.Length < (reservedSpaceForEachElement + 3)) {
                    var index = 1;
                    while (sPranuar.Length <= (reservedSpaceForEachElement + 3)) {
                        if (index > 0) {
                            sPranuar = sPranuar.Insert(0, " ");
                        }
                        else
                            sPranuar += " ";
                        index *= -1;
                    }
                }
                if (sShitur.Length < reservedSpaceForEachElement + 3) {
                    var index = 1;
                    while (sShitur.Length <= reservedSpaceForEachElement + 3) {
                        if (index > 0) {
                            sShitur = sShitur.Insert(0, " ");
                        }
                        else
                            sShitur += " ";
                        index *= -1;
                    }
                }

                if (slevizje.Length < reservedSpaceForEachElement) {
                    var index = 1;
                    while (slevizje.Length < reservedSpaceForEachElement) {
                        if (index > 0) {
                            slevizje = slevizje.Insert(0, " ");
                        }
                        else
                            slevizje += " ";
                        index *= -1;
                    }
                }
                if (smbetur.Length < reservedSpaceForEachElement) {
                    var index = 1;
                    while (smbetur.Length < reservedSpaceForEachElement) {
                        if (index > 0) {
                            smbetur = smbetur.Insert(0, " ");
                        }
                    }
                }

                emptySpace += sPranuar + sShitur + slevizje + smbetur;

                if (emptySpace.Length < 70) {
                    bool wasLastEmpty = false;
                    string totalTemp = emptySpace;
                    for (int i = emptySpace.Length - 1; i >= 0; i--) {
                        var de = emptySpace[i];
                        if (wasLastEmpty) {
                            if (emptySpace[i] == ' ') {
                                wasLastEmpty = true;
                                totalTemp = totalTemp.Remove(i, 1);
                                if (totalTemp.Length == 69) {
                                    emptySpace += " ";
                                    break;
                                }
                            }
                        }
                        if (emptySpace[i] == ' ') {
                            wasLastEmpty = true;
                        }
                        else {
                            wasLastEmpty = false;
                        }
                    }
                }
                if (emptySpace.Length > 70) {
                    bool wasLastEmpty = false;
                    string totalTemp = emptySpace;
                    for (int i = emptySpace.Length - 1; i >= 0; i--) {
                        var de = emptySpace[i];
                        if (wasLastEmpty) {
                            if (emptySpace[i] == ' ') {
                                wasLastEmpty = true;
                                totalTemp = totalTemp.Remove(i, 1);
                                if (totalTemp.Length == 70) {
                                    emptySpace = totalTemp;
                                    break;
                                }
                            }
                        }
                        if (emptySpace[i] == ' ') {
                            wasLastEmpty = true;
                        }
                        else {
                            wasLastEmpty = false;
                        }
                    }
                }
                await _printer.printText(emptySpace);
                Debug.WriteLine(emptySpace);

                await _printer.printText("\n");
                
                //printText = "A. 1. عدد ۰۱۲۳۴۵۶۷۸۹" + "\nB. 2. عدد 0123456789" + "\nC. 3. به" + "\nD. 4. نه" + "\nE. 5. مراجعه" + "\n";// 
                //await _printer.printText(printText, new MPosFontAttribute() { CodePage = (int)MPosCodePage.MPOS_CODEPAGE_FARSI, Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });     // Persian 
                await _printer.printText("\nNisesi:" + "                  Transportuesi:" + "          Pritesi:");
                Debug.WriteLine("\nNisesi:" + "                Transportuesi:" + "            Pritesi:");
                reservedSpaceForEachElement = 20;
                emptySpace = "\n";
                sPranuar = currAgjendi.Emri + " " + currAgjendi.Mbiemri;
                sShitur = currAgjendi.Emri + " " + currAgjendi.Mbiemri;
                sKthyer = depo.TAGNR;
                if (sPranuar.Length < (reservedSpaceForEachElement + 3)) {
                    var index = 1;
                    while (sPranuar.Length < (reservedSpaceForEachElement + 3)) {
                        sPranuar += " ";
                    }
                }
                if (sShitur.Length < (reservedSpaceForEachElement * 1.9f)) {
                    var index = 1;
                    while (sShitur.Length < (reservedSpaceForEachElement * 1.9f)) {
                        sShitur += " ";
                    }
                }
                if (sKthyer.Length < reservedSpaceForEachElement) {
                    var index = 1;
                    while (sKthyer.Length < reservedSpaceForEachElement) {
                        if (index > 0) {
                            sKthyer = sKthyer.Insert(0, " ");
                        }
                        else
                            sKthyer += " ";
                        index *= -1;
                    }
                }
                emptySpace += sPranuar + sShitur + sKthyer;

                if (emptySpace.Length < 70) {
                    bool wasLastEmpty = false;
                    string totalTemp = emptySpace;
                    for (int i = emptySpace.Length - 1; i >= 0; i--) {
                        var de = emptySpace[i];
                        if (wasLastEmpty) {
                            if (emptySpace[i] == ' ') {
                                wasLastEmpty = true;
                                totalTemp = totalTemp.Remove(i, 1);
                                if (totalTemp.Length == 70) {
                                    emptySpace += " ";
                                    break;
                                }
                            }
                        }
                        if (emptySpace[i] == ' ') {
                            wasLastEmpty = true;
                        }
                        else {
                            wasLastEmpty = false;
                        }
                    }
                }
                if (emptySpace.Length > 70) {
                    bool wasLastEmpty = false;
                    string totalTemp = emptySpace;
                    for (int i = emptySpace.Length - 1; i >= 0; i--) {
                        var de = emptySpace[i];
                        if (wasLastEmpty) {
                            if (emptySpace[i] == ' ') {
                                wasLastEmpty = true;
                                totalTemp = totalTemp.Remove(i, 1);
                                if (totalTemp.Length == 70) {
                                    emptySpace = totalTemp;
                                    break;
                                }
                            }
                        }
                        if (emptySpace[i] == ' ') {
                            wasLastEmpty = true;
                        }
                        else {
                            wasLastEmpty = false;
                        }
                    }
                }
                Debug.WriteLine(emptySpace);

                if (CurrentlySelectedLevizjetHeader.LevizjeNe == Agjendi.Depo) {
                    await _printer.printText("\n" + "                         " + depo.TAGNR );
                }
                else {
                    if (depo.TAGNR.Length >= 21) {
                        var wholeDepo = depo.TAGNR;
                        var only20Depo = depo.TAGNR.Remove(20);
                        await _printer.printText("\n" + currAgjendi.Emri + " " + currAgjendi.Mbiemri + "              " + currAgjendi.Emri + " " + currAgjendi.Mbiemri + "             " + only20Depo + "           " + "                  " + "           " + "         " + wholeDepo.Remove(0, 20));
                    }
                }
                
                await _printer.printText("\n");
                await _printer.printText("\n");

                await _printer.printText("___________________      ___________________     ___________________");


                await _printer.printText("\n");
                await _printer.printText("(emri,mbiemri,nensh.)   (emri,mbiemri,nensh.)   (emri,mbiemri,nensh.)");
                if (string.IsNullOrEmpty(CurrentlySelectedLevizjetHeader.TCRQRCodeLink) || string.IsNullOrEmpty(CurrentlySelectedLevizjetHeader.TCRNSLFSH)) {
                    await _printer.printText("                       Dokument i pa fiskalizuar");
                }
                else {
                    await _printer.printText("\n");
                    await _printer.printLine(1, 1, 1, 1, 1);
                    await _printer.printBitmap(DependencyService.Get<IPlatformInfo>().GenerateQRCode(CurrentlySelectedLevizjetHeader.TCRQRCodeLink),
                                300/*(int)MPosImageWidth.MPOS_IMAGE_WIDTH_ASIS*/,   // Image Width
                                (int)MPosAlignment.MPOS_ALIGNMENT_CENTER,           // Alignment
                                50,                                                 // brightness
                                true,                                               // Image Dithering
                                true);
                    await _printer.printText("\n");
                    await _printer.printText("\n");

                    await _printer.printText("\n");
                    await _printer.printText("Te gjitha Informacionet ne lidhje me kete fature, mund te shihen ne  kete Kod QR");
                    await _printer.printText("\n");
                }

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


        public async Task ShtoLevizjenAsync() {
            LevizjetPage LevizjetPage = new LevizjetPage();
            LevizjetPage.BindingContext = this;
            TotalPrice = 0;
            SelectedKlientet = null;
            CurrentlySelectedArtikulli = null;
            NumriFraturave = await App.Database.GetNumriFaturaveIDAsync(Agjendi.IDAgjenti);
            var gNumriPorosive = await App.Database.GetNumriPorosiveAsync();
            NumriPorosive = gNumriPorosive.FirstOrDefault(x=> x.TIPI == "LEVIZJE");
            DateTime MyTimeInWesternEurope = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "GMT Standard Time").AddHours(1);
            if (NumriPorosive == null) {
                NumriPorosive = new NumriPorosive
                {
                    TIPI = "LEVIZJE",
                    NrPorosise = 01,
                    Date = MyTimeInWesternEurope,
                };
                await App.Database.SaveNumriPorosiveAsync(NumriPorosive);
            }
            else {
                NumriPorosive.NrPorosise += 1;
            }
            if(SelectedArikujt != null)
                SelectedArikujt.Clear();
            var ld = await App.Database.GetLevizjeDetailsAsync();
            LevizjetDetails = new ObservableCollection<LevizjetDetails>(ld.ToList());
            await App.Instance.PushAsyncNewPage(LevizjetPage);
        }
        public async Task AddArtikulliAsync() {

            
            if (SelectedArikujt == null) {
                SelectedArikujt = new ObservableCollection<Artikulli>();
            }
            if (CurrentlySelectedArtikulli == null) return;
            if (string.IsNullOrEmpty(CurrentlySelectedArtikulli.Seri)) {
                UserDialogs.Instance.Alert("Arikulli qe keni zgjedhur nuk ka seri, ju lutemi mbusheni fushen SERI", "Verejtje", "Ok");
                return;
            }
            if (CurrentlySelectedArtikulli.Seri.Trim().Length == 0) {
                UserDialogs.Instance.Alert("Arikulli qe keni zgjedhur nuk ka seri, ju lutemi mbusheni fushen SERI", "Verejtje", "Ok");
                return;
            }
            if (Sasia == 0)
                return;
            if(Nga) {
                if (CurrentlySelectedArtikulli.Sasia < Sasia) {
                    UserDialogs.Instance.Alert("Sasia e shenuar eshte me e madhe se sasia aktuale, ju lutemi ta permiresoni", "Verejtje", "Ok");
                    return;
                }
            }
            var artikulli = Artikujt.FirstOrDefault(x => x.IDArtikulli == CurrentlySelectedArtikulli.IDArtikulli);
            var malliMbetur = MalliMbetur.FirstOrDefault(x => x.IDArtikulli == CurrentlySelectedArtikulli.IDArtikulli);
            var stoqet = await App.Database.GetAllStoqetAsync();

            if(malliMbetur == null) {
                UserDialogs.Instance.Alert("Artikulli " + artikulli.Emri + " nuk gjindet ne mall te mbetur per kete depo, ju lutemi kontaktoni bazen ose provoni artikull tjeter");
                CurrentlySelectedArtikulli = null;
                return;
            }
            
            if (artikulli != null) {
                artikulli.Seri = CurrentlySelectedArtikulli.Seri;
                malliMbetur.Seri = CurrentlySelectedArtikulli.Seri;
                await App.Database.UpdateArtikulliAsync(artikulli);
                await App.Database.UpdateMalliMbeturAsync(malliMbetur);
            }
            var tempArts = Artikujt;
            CurrentlySelectedArtikulli.Sasia = Sasia;
            SelectedArikujt.Add(CurrentlySelectedArtikulli);
            TotalPrice = (double)(TotalPrice + CurrentlySelectedArtikulli.CmimiTotal);
            Sasia = 0;
            //Update sasia
            decimal SasiaUpdate = Math.Round(decimal.Parse(CurrentlySelectedArtikulli.Sasia.ToString()), 3);
            
            var stoku = stoqet.FirstOrDefault(x => x.Depo == Agjendi.IDAgjenti && x.Seri == CurrentlySelectedArtikulli.Seri);
            decimal sasiaAktuale = Math.Round(decimal.Parse(stoku.Sasia.ToString()), 3);
            stoku.Sasia = double.Parse(Math.Round(sasiaAktuale - SasiaUpdate, 3).ToString());
            await Task.Delay(20);
            //await App.Database.UpdateStoqetAsync(stoku);
            //update malli i mbetur
            //await App.Database.UpdateMalliMbeturAsync(malliIMbetur);
            CurrentlySelectedArtikulli = null;
            KrijoPorosine = false;

        }        
        private ObservableCollection<SalesPrice> _salesPrices;
        public ObservableCollection<SalesPrice> SalesPrices {
            get { return _salesPrices; }
            set { SetProperty(ref _salesPrices, value); }
        }
        private ObservableCollection<Malli_Mbetur> _MalliMbetur;
        public ObservableCollection<Malli_Mbetur> MalliMbetur {
            get { return _MalliMbetur; }
            set { SetProperty(ref _MalliMbetur, value); }
        }
        private ObservableCollection<Artikulli> _SelectedArikujt;
        public ObservableCollection<Artikulli> SelectedArikujt {
            get { return _SelectedArikujt; }
            set { SetProperty(ref _SelectedArikujt, value); }
        }
        private Artikulli _currentlySelectedArtikulli;
        public Artikulli CurrentlySelectedArtikulli {
            get {
                return _currentlySelectedArtikulli;
            }
            set { SetProperty(ref _currentlySelectedArtikulli, value); }
        }
        private LevizjetHeader _currentlySelectedLevizjetHeader;
        public LevizjetHeader CurrentlySelectedLevizjetHeader {
            get {
                return _currentlySelectedLevizjetHeader;
            }
            set { SetProperty(ref _currentlySelectedLevizjetHeader, value); }
        }

        public ObservableCollection<LevizjetDetails> CurrentlySelectedLevizjetDetails { get; set; }
        private float _sasia;
        public float Sasia {
            get { return _sasia; }
            set { SetProperty(ref _sasia, value); }
        }

        private bool _krijoPorosine;
        public bool KrijoPorosine {
            get { return _krijoPorosine; }
            set { SetProperty(ref _krijoPorosine, value); }
        }

        private string _nrLevizjes;
        public string NrLevizjes {
            get { return _nrLevizjes; }
            set { SetProperty(ref _nrLevizjes, value); }
        }

        public NumriFaturave NumriFraturave { get; set; }
        public NumriPorosive NumriPorosive { get; set; }
        public async Task RegjistroLevizjenAsync() {
            try {
                UserDialogs.Instance.ShowLoading("Duke perfunduar levizjen");
                if(SelectedKlientet == null) {
                    UserDialogs.Instance.Alert("Zgjedhni Depon para se te perfundoni levizjen", "Error", "Ok");
                    UserDialogs.Instance.HideLoading();
                    return;
                }
                var levizjet = await App.Database.GetLevizjetHeaderAsync();

                var numriFaturave = await App.Database.GetNumriFaturaveAsync();
                var result = numriFaturave
                    .Where(n => n.KOD == Agjendi.IDAgjenti)
                    .Select(n => new
                    {
                        Sum = n.NRKUFIP_D + n.CurrNrFat_D
                    })
                    .FirstOrDefault();

                
                var levizjNumber = 0;
                if (result != null) {
                    levizjNumber = (int)result.Sum;
                }
                var geoLocation = await Geolocation.GetLastKnownLocationAsync();
                var depot = await App.Database.GetDepotAsync();
                var agjendet = await App.Database.GetAgjendetAsync();
                var FiskalizimiKonfigurimet = await App.Database.GetFiskalizimiKonfigurimetAsync();
                var numriFiskal = await App.Database.GetNumratFiskalAsync();
                if(SelectedArikujt == null) {
                    UserDialogs.Instance.HideLoading();
                    UserDialogs.Instance.Alert("Ju lutem zgjedhni artikujt para se te regjistroni levizjen");
                    return;
                }
                foreach (var artikull in SelectedArikujt) {

                    if(Ne) {
                        //update malli i mbetur
                        var malliIMbetur = await App.Database.GetMalliMbeturIDAsync(artikull.Seri, Agjendi.IDAgjenti,artikull.IDArtikulli);

                        malliIMbetur.LevizjeStoku += (float)artikull.Sasia;

                        var sasiaMbeturString = malliIMbetur.SasiaPranuar - (malliIMbetur.SasiaShitur + malliIMbetur.SasiaKthyer - malliIMbetur.LevizjeStoku);
                        malliIMbetur.SasiaMbetur = float.Parse(Math.Round(double.Parse(sasiaMbeturString.ToString()), 3).ToString());
                        malliIMbetur.SyncStatus = 0;
                        var res = await App.Database.UpdateMalliMbeturAsync(malliIMbetur);
                        await App.Database.UpdateNumriPorosiveAsync(NumriPorosive);
                    }
                        
                    if(Nga) {
                        var malliIMbetur = await App.Database.GetMalliMbeturIDAsync(artikull.Seri, Agjendi.IDAgjenti,artikull.IDArtikulli);
                        malliIMbetur.LevizjeStoku -= (float)artikull.Sasia;

                        //(sasiaPranuar - (SasiaShitur+SasiaKthyer-LevizjeStoku))
                        var sasiaMbeturString = malliIMbetur.SasiaPranuar - (malliIMbetur.SasiaShitur + malliIMbetur.SasiaKthyer - malliIMbetur.LevizjeStoku);
                        malliIMbetur.SasiaMbetur = float.Parse(Math.Round(double.Parse(sasiaMbeturString.ToString()), 3).ToString());
                        malliIMbetur.SyncStatus = 0;
                        var res = await App.Database.UpdateMalliMbeturAsync(malliIMbetur);
                        var currNumriFaturave = numriFaturave.FirstOrDefault(x => x.KOD == Agjendi.IDAgjenti);
                        if (currNumriFaturave != null) {
                            currNumriFaturave.CurrNrFat_D = currNumriFaturave.CurrNrFat_D + 1;
                            NumriFraturave = currNumriFaturave;
                            await App.Database.UpdateNumriFaturave(currNumriFaturave);
                        }
                    }
                        
                }


                if (numriFiskal.Count < 1) {
                    var numratFiskalResult = await App.ApiClient.GetAsync("numri-fisk" );
                    if(numratFiskalResult.IsSuccessStatusCode) {
                        var numratFiskalResponse = await numratFiskalResult.Content.ReadAsStringAsync();
                        numriFiskal = JsonConvert.DeserializeObject<List<NumriFisk>>(numratFiskalResponse);
                    }
                }
                var topLevizjeIDN = numriFiskal
                                    .Where(v => v.Depo == App.Instance.MainViewModel.LoginData.Depo)
                                    .OrderBy(v => v.LevizjeIDN)
                                    .FirstOrDefault();
                topLevizjeIDN.LevizjeIDN = topLevizjeIDN.LevizjeIDN + 1;
                //TODO : FIX FISKALIZIMI KONFIGURIMET FROM API
                var query = from a in agjendet
                            join d in depot on a.Depo equals d.Depo
                            join fk in FiskalizimiKonfigurimet on d.TAGNR equals fk.TAGNR
                            join nf in numriFiskal on fk.TCRCode equals nf.TCRCode into nfGroup
                            from nf in nfGroup.DefaultIfEmpty()
                            where a.IDAgjenti.ToLower() == Agjendi.IDAgjenti.ToLower()
                            select new
                            {
                                a.IDAgjenti,
                                d.TAGNR,
                                fk.TCRCode,
                                a.OperatorCode,
                                fk.BusinessUnitCode,
                                nf.IDN,
                                LevizjeIDN = nf != null ? nf.LevizjeIDN : (int?)null,
                                nf.Viti
                            };
                var transferId = Guid.NewGuid();
                DateTime MyTimeInWesternEurope = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "GMT Standard Time").AddHours(1);
                var levizjaHeader = new LevizjetHeader
                {
                    Data = MyTimeInWesternEurope,
                    IDAgjenti = Agjendi.IDAgjenti,
                    Depo = Agjendi.Depo,
                    LevizjeNga = Nga ? Agjendi.Depo : SelectedKlientet.Depo,
                    LevizjeNe = Ne ? Agjendi.Depo : SelectedKlientet.Depo,
                    ImpStatus = 0,
                    NumriLevizjes = NrLevizjes,
                    KodiDyqanit = Agjendi.Depo,
                    TransferID = transferId,
                    Latitude = geoLocation?.Latitude.ToString(),
                    Longitude = geoLocation?.Longitude.ToString(),
                    Message = "Levizja ka perfunduar",
                    NumriDaljes = "0",
                    NumriFisk = topLevizjeIDN.LevizjeIDN,
                    SyncStatus = 0,
                    TCR = App.Instance.MainViewModel.Configurimi.KodiTCR,
                    TCRBusinessUnitCode = query?.FirstOrDefault().BusinessUnitCode,
                    TCRIssueDateTime = Agjendi.TCRRegisteredDateTime,
                    TCROperatorCode = query?.FirstOrDefault().OperatorCode,
                    Totali = decimal.Parse(TotalPrice.ToString()),
                    TCRSyncStatus = 0,
                };
                if (Ne)
                    levizjaHeader.NumriFisk = null;
                var levizjetDetails = new List<LevizjetDetails>();
                foreach (var artikulli in SelectedArikujt) {
                    var levizjaDetails = new LevizjetDetails
                    {
                        Artikulli = artikulli.Emri,
                        Cmimi = decimal.Parse(artikulli.CmimiNjesi.ToString()),
                        Depo = Agjendi.Depo,
                        Sasia = decimal.Parse(artikulli.Sasia.ToString()),
                        IDArtikulli = artikulli.IDArtikulli,
                        Njesia_matese = artikulli.BUM,
                        NumriLevizjes = levizjaHeader.NumriLevizjes,
                        Seri = artikulli.Seri,
                        SyncStatus = 0,
                        TCRSyncStatus = 0,
                        Totali = decimal.Parse(artikulli.CmimiTotal.ToString())
                    };
                    levizjetDetails.Add(levizjaDetails);
                }
                await App.Database.SaveAllLevizjeDetailsAsync(levizjetDetails);
                await App.Database.SaveLevizjeHeaderAsync(levizjaHeader);

                SelectedArikujt = null;
                TotalPrice = 0;
                if(Nga)
                    await App.Database.UpdateNumriFiskalAsync(topLevizjeIDN);
                if(Nga) {
                    await RegisterTCRWTN(levizjaHeader.NumriLevizjes);
                }
                await App.Database.SaveNumriPorosiveAsync(NumriPorosive);
                UserDialogs.Instance.Alert("Levizja u regjistrua me sukses");
                await App.Instance.PopPageAsync();
                UserDialogs.Instance.HideLoading();
                Nga = false;
                Ne = false;
                NrLevizjes = String.Empty;
                LevizjetHeader = new ObservableCollection<LevizjetHeader>( await App.Database.GetLevizjetHeaderAsync());
            }
            catch (Exception e) {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert("Levizja nuk u regjistrua me sukses errori : " + e.StackTrace + " stack trace : " + e.StackTrace);
            }
            UserDialogs.Instance.HideLoading();

        }


        public async Task RegisterTCRWTN(string nrLevizjes) {
            var levizjetHeader = await App.Database.GetLevizjetHeaderAsync();
            var depot = await App.Database.GetDepotAsync();
            var result = from lh in levizjetHeader
                         join d_ne in depot on lh.LevizjeNe equals d_ne.Depo
                         join d_nga in depot on lh.LevizjeNga equals d_nga.Depo
                         where (lh.TCRSyncStatus == null || lh.TCRSyncStatus == 0)
                               && lh.NumriLevizjes == nrLevizjes
                               && lh.LevizjeNga.Trim() == Agjendi.IDAgjenti
                         orderby lh.Data descending
                         select new EHWM.Models.WTNModels.MapperHeader
                         {
                             Numri_Levizjes = lh.NumriLevizjes,
                             InvOrdNum = (int)lh.NumriFisk,
                             InvNum = lh.NumriFisk + "/" + DateTime.Now.Year,
                             ValueOfGoods = Math.Round((decimal)lh.Totali, 2),
                             FromDevice = d_nga.TAGNR,
                             ToDevice = d_ne.TAGNR,
                             ImpStatus = (int)lh.ImpStatus
                         };


            List<EHWM.Models.WTNModels.MapperHeader> mapperHeaderList = result.ToList();
            var levizjetDetails = await App.Database.GetLevizjeDetailsAsync();
            var query = from h in levizjetHeader
                        join l in levizjetDetails on h.NumriLevizjes equals l.NumriLevizjes
                        where (h.TCRSyncStatus == null || h.TCRSyncStatus == 0)
                              && h.NumriLevizjes == nrLevizjes
                              && h.LevizjeNga.Trim() == Agjendi.IDAgjenti
                              && Math.Round((decimal)l.Sasia, 2) >= 0.01m
                        select new EHWM.Models.WTNModels.MapperLines
                        {
                            Numri_Levizjes = h.NumriLevizjes,
                            DeviceID = h.Depo,
                            OperatorCode = Agjendi.OperatorCode,
                            InvOrdNum = (int)h.NumriFisk,
                            InvNum = h.NumriFisk + "/" + DateTime.Now.Year,
                            SendDatetime = (DateTime)h.Data,
                            ValueOfGoods = Math.Round((decimal)h.Totali, 2),
                            StartPointSType = "SALE",
                            DestinPointSType = "SALE",
                            Item_N = l.Artikulli,
                            Item_C = l.IDArtikulli,
                            Item_U = l.Njesia_matese,
                            Item_Q = Math.Round((decimal)l.Sasia, 2),
                            MobileRefId = Agjendi.DeviceID
                        };

            List<Models.WTNModels.MapperLines> mapperLinesList = query.ToList();

            if (mapperHeaderList.Count > 0) {


                List<TCRLevizjetPCL> invoiceObject = Models.WTNModels.ClassMapper.MapHeadersAndLines(mapperHeaderList, mapperLinesList);
                foreach (TCRLevizjetPCL inv in invoiceObject) {
                    RegisterWTNInputRequestPCL req = new RegisterWTNInputRequestPCL();

                    req.TransferItems = inv.Items.ToList();
                    req.InvOrdNum = inv.InvOrdNum.ToString();
                    req.InvNum = inv.InvNum.ToString();
                    req.DeviceID = inv.DeviceID;
                    req.MobileRefId = inv.InvOrdNum.ToString();
                    req.OperatorCode = Agjendi.OperatorCode;
                    req.TCRCode = App.Instance.MainViewModel.Configurimi.KodiTCR;
                    req.BusinessUnitCode = App.Instance.MainViewModel.Configurimi.KodiINjesiseSeBiznesit;
                    req.SendDatetime = inv.SendDatetime;
                    req.SubseqDelivTypeSType = -1; //ONLINE
                    req.ValueOfGoods = (decimal)inv.ValueOfGoods;

                    req.DestinPointSType = inv.DestinPointSType;
                    req.DestinPointSTypeSpecified = true;
                    req.StartPointSType = inv.StartPointSType;
                    req.StartPointSTypeSpecified = true;
                    req.FromDeviceId = inv.FromDevice;
                    req.ToDeviceId = inv.ToDevice;
                    DateTime MyTimeInWesternEurope = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "GMT Standard Time").AddHours(1);

                    ResultLogPCL log = App.Instance.FiskalizationService.RegisterWTN(req);
                    if (log == null) {
                        var levizjaHeader = levizjetHeader
                                        .FirstOrDefault(h => h.NumriLevizjes == inv.Numri_Levizjes);

                        if (levizjaHeader != null) {
                            levizjaHeader.TCRSyncStatus = -1;
                            levizjaHeader.TCRIssueDateTime = MyTimeInWesternEurope;
                            levizjaHeader.TCRQRCodeLink = null;
                            levizjaHeader.TCR = App.Instance.MainViewModel.Configurimi.KodiTCR;
                            levizjaHeader.TCROperatorCode = Agjendi.OperatorCode;
                            levizjaHeader.TCRBusinessUnitCode = App.Instance.MainViewModel.Configurimi.KodiINjesiseSeBiznesit;
                            levizjaHeader.UUID = null;
                            levizjaHeader.TCRNSLFSH = null;
                            levizjaHeader.TCRNIVFSH = null;
                            levizjaHeader.Message = "Fiskalizimi deshtoi, ju lutemi provoni me vone!";

                            await App.Database.UpdateLevizjeHeaderAsync(levizjaHeader);
                        }

                        var levizjetDetailsToUpdate = levizjetDetails
                                    .Where(d => d.NumriLevizjes == inv.Numri_Levizjes)
                                    .ToList();

                        foreach (var detail in levizjetDetailsToUpdate) {
                            detail.TCRSyncStatus = -1;
                            await App.Database.UpdateLevizjeDetailsAsync(detail);
                        }
                        return;
                    }
                    if (log.Status == StatusPCL.Ok) {
                        var levizjaHeader = levizjetHeader
                                .FirstOrDefault(h => h.NumriLevizjes == inv.Numri_Levizjes);

                        if (levizjaHeader != null) {
                            levizjaHeader.TCRSyncStatus = 1;
                            levizjaHeader.TCRIssueDateTime = MyTimeInWesternEurope;
                            levizjaHeader.TCRQRCodeLink = log.QRCodeLink;
                            levizjaHeader.TCR = App.Instance.MainViewModel.Configurimi.KodiTCR;
                            levizjaHeader.TCROperatorCode = Agjendi.OperatorCode;
                            levizjaHeader.TCRBusinessUnitCode = App.Instance.MainViewModel.Configurimi.KodiINjesiseSeBiznesit;
                            levizjaHeader.UUID = log.ResponseUUIDSH;
                            levizjaHeader.TCRNSLFSH = log.NSLFSH;
                            levizjaHeader.TCRNIVFSH = log.NIVFSH;
                            levizjaHeader.Message = log.Message.Replace("'", "");

                            await App.Database.UpdateLevizjeHeaderAsync(levizjaHeader);
                        }

                        var levizjetDetailsToUpdate = levizjetDetails
                                    .Where(d => d.NumriLevizjes == inv.Numri_Levizjes)
                                    .ToList();

                        foreach (var detail in levizjetDetailsToUpdate) {
                            detail.TCRSyncStatus = 1;
                            await App.Database.UpdateLevizjeDetailsAsync(detail);
                        }
                    }
                    else if (log.Status == StatusPCL.FaultCode) {
                        try {
                            if (String.IsNullOrEmpty(log.Message)) {
                                var levizjaHeader = levizjetHeader
                                        .FirstOrDefault(h => h.NumriLevizjes == inv.Numri_Levizjes);

                                if (levizjaHeader != null) {
                                    levizjaHeader.TCRSyncStatus = -1;
                                    levizjaHeader.TCRIssueDateTime = MyTimeInWesternEurope;
                                    levizjaHeader.TCRQRCodeLink = log.QRCodeLink;
                                    levizjaHeader.TCR = App.Instance.MainViewModel.Configurimi.KodiTCR;
                                    levizjaHeader.TCROperatorCode = Agjendi.OperatorCode;
                                    levizjaHeader.TCRBusinessUnitCode = App.Instance.MainViewModel.Configurimi.KodiINjesiseSeBiznesit;
                                    levizjaHeader.UUID = log.ResponseUUIDSH;
                                    levizjaHeader.TCRNSLFSH = log.NSLFSH;
                                    levizjaHeader.TCRNIVFSH = log.NIVFSH;
                                    levizjaHeader.Message = "Fiskalizimi deshtoi, ju lutemi provoni me vone!";

                                    await App.Database.UpdateLevizjeHeaderAsync(levizjaHeader);
                                }

                                var levizjetDetailsToUpdate = levizjetDetails
                                            .Where(d => d.NumriLevizjes == inv.Numri_Levizjes)
                                            .ToList();

                                foreach (var detail in levizjetDetailsToUpdate) {
                                    detail.TCRSyncStatus = -1;
                                    await App.Database.UpdateLevizjeDetailsAsync(detail);
                                }
                            }
                            else {
                                var levizjaHeader = levizjetHeader
                                        .FirstOrDefault(h => h.NumriLevizjes == inv.Numri_Levizjes);

                                if (levizjaHeader != null) {
                                    levizjaHeader.TCRSyncStatus = -1;
                                    levizjaHeader.TCRIssueDateTime = MyTimeInWesternEurope;
                                    levizjaHeader.TCRQRCodeLink = log.QRCodeLink;
                                    levizjaHeader.TCR = App.Instance.MainViewModel.Configurimi.KodiTCR;
                                    levizjaHeader.TCROperatorCode = Agjendi.OperatorCode;
                                    levizjaHeader.TCRBusinessUnitCode = App.Instance.MainViewModel.Configurimi.KodiINjesiseSeBiznesit;
                                    levizjaHeader.UUID = log.ResponseUUIDSH;
                                    levizjaHeader.TCRNSLFSH = log.NSLFSH;
                                    levizjaHeader.TCRNIVFSH = log.NIVFSH;
                                    levizjaHeader.Message = log.Message.Replace("'", "");

                                    await App.Database.UpdateLevizjeHeaderAsync(levizjaHeader);
                                }

                                var levizjetDetailsToUpdate = levizjetDetails
                                            .Where(d => d.NumriLevizjes == inv.Numri_Levizjes)
                                            .ToList();

                                foreach (var detail in levizjetDetailsToUpdate) {
                                    detail.TCRSyncStatus = -1;
                                    await App.Database.UpdateLevizjeDetailsAsync(detail);
                                }
                            }
                        }
                        catch (Exception ex) {
                        }
                    }
                    else if (log.Status == StatusPCL.TCRAlreadyRegistered) {
                        var levizjaHeader = levizjetHeader
                                        .FirstOrDefault(h => h.NumriLevizjes == inv.Numri_Levizjes);

                        if (levizjaHeader != null) {
                            levizjaHeader.TCRSyncStatus = 4;
                            levizjaHeader.TCRIssueDateTime = MyTimeInWesternEurope;
                            levizjaHeader.TCRQRCodeLink = log.QRCodeLink;
                            levizjaHeader.TCR = App.Instance.MainViewModel.Configurimi.KodiTCR;
                            levizjaHeader.TCROperatorCode = Agjendi.OperatorCode;
                            levizjaHeader.TCRBusinessUnitCode = App.Instance.MainViewModel.Configurimi.KodiINjesiseSeBiznesit;
                            levizjaHeader.UUID = log.ResponseUUIDSH;
                            levizjaHeader.TCRNSLFSH = log.NSLFSH;
                            levizjaHeader.TCRNIVFSH = log.NIVFSH;
                            levizjaHeader.Message = log.Message.Replace("'", "");

                            await App.Database.UpdateLevizjeHeaderAsync(levizjaHeader);
                        }

                        var levizjetDetailsToUpdate = levizjetDetails
                                    .Where(d => d.NumriLevizjes == inv.Numri_Levizjes)
                                    .ToList();

                        foreach (var detail in levizjetDetailsToUpdate) {
                            detail.TCRSyncStatus = 4;
                            await App.Database.UpdateLevizjeDetailsAsync(detail);
                        }
                    }
                }

            }

        }


        public void FshijArtikullinAsync() {
            SelectedArikujt.Remove(CurrentlySelectedArtikulli);
            TotalPrice -= (double)CurrentlySelectedArtikulli.CmimiTotal;
            CurrentlySelectedArtikulli = null;
        }
        public async Task ZgjedhArtikullinAsync() {
            try {
                UserDialogs.Instance.ShowLoading("Duka shfaqur artikujt");
                var malliMbetur = await App.Database.GetMalliMbeturAsync();
                MalliMbetur = new ObservableCollection<Malli_Mbetur>(malliMbetur);
                if(SalesPrices == null) {
                    SalesPrices = new ObservableCollection<SalesPrice>(await App.Database.GetSalesPriceAsync());
                }
                ZgjidhArtikullinModalPage zgjidhArtikullinModalPage = new ZgjidhArtikullinModalPage();
                zgjidhArtikullinModalPage.BindingContext = this;
                Artikujt = new ObservableCollection<Artikulli>(await App.Database.GetArtikujtAsync());

                //SalesPrices = new ObservableCollection<SalesPrice>(await App.Database.GetSalesPriceAsync());
                //MalliMbetur = new ObservableCollection<Malli_Mbetur>(await App.Database.GetMalliMbeturAsync());
                var artikujtPerShfaqje = new ObservableCollection<Artikulli>();

                foreach (var artikulli in Artikujt) {

                    artikulli.CmimiNjesi = SalesPrices.FirstOrDefault(x => x.ItemNo == artikulli.IDArtikulli)?.UnitPrice;
                    var hasTwoMalliMbeturs = malliMbetur.Where(x => x.IDArtikulli == artikulli.IDArtikulli);
                    if (hasTwoMalliMbeturs.Count() > 1) {
                        foreach (var mm in hasTwoMalliMbeturs) {
                            if (mm != null) {
                                var cloneArtiull = new Artikulli
                                {
                                    Sasia = mm.SasiaMbetur,
                                    CmimiNjesi = artikulli.CmimiNjesi,
                                    ArsyejaEKthimit = artikulli.ArsyejaEKthimit,
                                    Barkod = artikulli.Barkod,
                                    BUM = artikulli.BUM,
                                    CmimiPako = artikulli.CmimiPako,
                                    Seri = mm.Seri,
                                    Emri = artikulli.Emri,
                                    IDArtikulli = artikulli.IDArtikulli,
                                    SasiaPako = artikulli.SasiaPako,
                                    Shifra = artikulli.Shifra,
                                    Standard = artikulli.Standard,
                                    StokuAktual = artikulli.StokuAktual,
                                    SyncStatus = artikulli.SyncStatus,
                                    TePorositur = artikulli.TePorositur,
                                    UnitPrice = artikulli.UnitPrice,
                                    UPP = artikulli.UPP
                                };
                                artikujtPerShfaqje.Add(cloneArtiull);
                            }
                        }
                    }
                    else {
                        var MalliiMbetur = MalliMbetur.FirstOrDefault(x => x.IDArtikulli == artikulli.IDArtikulli && x.Depo == Agjendi.IDAgjenti);
                        if (MalliiMbetur != null) {
                            artikulli.Sasia = MalliiMbetur.SasiaMbetur;
                            artikulli.Seri = MalliiMbetur.Seri;
                            artikujtPerShfaqje.Add(artikulli);
                        }
                    }
                }
                if(Nga) {
                    Artikujt = new ObservableCollection<Artikulli>(artikujtPerShfaqje.Where(x => x.Sasia >= 0.01f).OrderBy(x => x.Emri));
                }
                else {
                    Artikujt = new ObservableCollection<Artikulli>(artikujtPerShfaqje);
                }

                await App.Instance.PushAsyncNewModal(zgjidhArtikullinModalPage);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception e) {
                var g = e.Message;
            }

        }
    }
}
