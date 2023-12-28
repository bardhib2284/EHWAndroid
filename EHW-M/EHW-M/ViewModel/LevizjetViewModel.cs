using Acr.UserDialogs;
using EHW_M;
using EHWM.Models;
using EHWM.Services;
using EHWM.Views;
using Newtonsoft.Json;
using Plugin.BxlMpXamarinSDK;
using Plugin.BxlMpXamarinSDK.Abstractions;
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
    public class LevizjetNavigationParameters {
        public List<Depot> Klientet { get; set; }
        public List<Artikulli> Artikujt { get; set; }
        public Agjendet Agjendi { get; set; }
        public List<LevizjetHeader> LevizjetHeader { get; set;}
    }

    public class LevizjetViewModel : BaseViewModel{

        public ObservableCollection<Depot> Klientet { get; set; }
        private ObservableCollection<Artikulli> _artikujt;
        public ObservableCollection<Artikulli> Artikujt {
            get { return _artikujt; }
            set { SetProperty(ref _artikujt, value); }
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
            set { SetProperty(ref _nga, value); }
        }
        private bool _Ne;
        public bool Ne {
            get { return _Ne; }
            set { SetProperty(ref _Ne, value); }
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

            LevizjaEPerfunduar = false;
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
                await _printer.printText("\nF A T U R E    S H O Q E R U E S E \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_CENTER, Bold = false, });
                await _printer.printLine(0, 0, 1, 1, 1);
                await _printer.printText(
"---------------------------------------------------------------------\n");


                await _printer.printText("Emri i Nisesit: E. H. W.          J61804031V \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });
                await _printer.printText("Tel: 048 200 711           web: www.ehwgmbh.com \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });
                await _printer.printText("Adresa: AA951IN            9923 \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });
                await _printer.printText("Qyteti / Shteti: Tirana, Albania \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });
                await _printer.printText("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");

                await _printer.printText("\nNumri i fatures: " + CurrentlySelectedLevizjetHeader.NumriFisk + "/" + CurrentlySelectedLevizjetHeader.Data.Value.Year);
                await _printer.printText("      Numri i serise: " + CurrentlySelectedLevizjetHeader.NumriLevizjes);
                await _printer.printText("\nData dhe ora e leshimit te fatures: " + CurrentlySelectedLevizjetHeader.Data);
                await _printer.printText("\nKodi i vendit te ushtrimit te veprimtarise se biznesit: " + CurrentlySelectedLevizjetHeader.TCRBusinessUnitCode);
                await _printer.printText("\nKodi i operatorit : " + CurrentlySelectedLevizjetHeader.TCROperatorCode);

                await _printer.printText("\nNIVFSH:  " + CurrentlySelectedLevizjetHeader.TCRNIVFSH);
                await _printer.printText("\nNSLFSH:  " + CurrentlySelectedLevizjetHeader.TCRNSLFSH);
                await _printer.printText("\nQellimi i Levizjes se Mallit:  Shitje me shumice\n");
                await _printer.printText(
"---------------------------------------------------------------------");
                var depot = await App.Database.GetDepotAsync();
                var depo = depot.FirstOrDefault(x => x.Depo == CurrentlySelectedLevizjetHeader.LevizjeNe);
                await _printer.printText("\nEmri i Pritesit: E. H. W.   NIPT: " + depo.NIPT + " +   SN : " + depo.SN);
                await _printer.printText("\nAdresa: " + depo.ADRESA +"\n");


                await _printer.printText(
"---------------------------------------------------------------------");
                var currentDepo = depot.FirstOrDefault(x => x.Depo == CurrentlySelectedLevizjetHeader.LevizjeNga);
                var agjendet = await App.Database.GetAgjendetAsync();
                var currAgjendi = agjendet.FirstOrDefault(x => x.Depo == CurrentlySelectedLevizjetHeader.LevizjeNga);
                await _printer.printText("\nTransportues:  " + currAgjendi.Emri.ToUpper() + "  J61804031V");
                await _printer.printText("\nAdresa: " + currentDepo.ADRESA );
                await _printer.printText("\nMjeti: " + currentDepo.TARGE );
                await _printer.printText("\nData dhe ora e furnizimit:  " + CurrentlySelectedLevizjetHeader.Data + " \n");

                await _printer.printText(
"---------------------------------------------------------------------");                
                await _printer.printText(
"---------------------------------------------------------------------");

                await _printer.printText("\nKodi   Pershkrimi   Njesia   Sasia   Cmimi   V.PaTVSH   TVSH   VLERA \n");
                await _printer.printText("---------------------------------------------------------------------");
                ;
                float teGjithaSasit = 0f;
                float teGjithaCmimetNjesi = 0f;
                double teGjitheCmimetTotale = 0f;
                decimal vleraTVSH = 0m;
                var artikujt = await App.Database.GetArtikujtAsync();

                foreach (var ld in CurrentlySelectedLevizjetDetails) {
                    foreach(var art in Artikujt) {
                        if(art.IDArtikulli == ld.IDArtikulli) {
                            await _printer.printText("\n" + art.IDArtikulli + "   " + art.Emri + "   " +
     "\n                     " + art.BUM + "         " + ld.Sasia + "       " + ld.Cmimi + "    " + Math.Round((decimal)(ld.Cmimi / 1.2m),2) + "       " + "20%  " + "   " + art.CmimiNjesi + "\n");
                            teGjithaSasit += (float)ld.Sasia;
                            teGjithaCmimetNjesi += (float)ld.Cmimi;
                            teGjitheCmimetTotale += (double)ld.Totali;
                            vleraTVSH = decimal.Parse(ld.Cmimi.ToString()) - Math.Round((decimal)(ld.Cmimi / 1.2m), 2);
                        }
                    }
                }


                await _printer.printText("---------------------------------------------------------------------");
                ;
                await _printer.printText("\n                    Total    " + teGjithaSasit + "       " + teGjithaCmimetNjesi + "        " + vleraTVSH + "        " + CurrentlySelectedLevizjetHeader.Totali, new MPosFontAttribute { Bold = true });
                await _printer.printText("\n");

                //printText = "A. 1. عدد ۰۱۲۳۴۵۶۷۸۹" + "\nB. 2. عدد 0123456789" + "\nC. 3. به" + "\nD. 4. نه" + "\nE. 5. مراجعه" + "\n";// 
                //await _printer.printText(printText, new MPosFontAttribute() { CodePage = (int)MPosCodePage.MPOS_CODEPAGE_FARSI, Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });     // Persian 
                await _printer.printText("\nNisesi:" + "             Transportuesi:" + "               Pritesi:");
                await _printer.printText("\n" + currAgjendi.Emri  + " "+ currAgjendi.Mbiemri + "          " + currAgjendi.Emri + " " + currAgjendi.Mbiemri + "        " + depo.TAGNR + "      ");



                await _printer.printText("\n");
                await _printer.printText("\n");

                await _printer.printText("___________________      ___________________     ___________________");


                await _printer.printText("\n");
                await _printer.printText("(emri,mbiemri,nensh.)   (emri,mbiemri,nensh.)   (emri,mbiemri,nensh.)");
                if (string.IsNullOrEmpty(CurrentlySelectedLevizjetHeader.TCRQRCodeLink)) {
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
                }

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
            await App.Instance.PushAsyncNewPage(LevizjetPage);
        }
        public async Task AddArtikulliAsync() {
            if (SelectedArikujt == null) {
                SelectedArikujt = new ObservableCollection<Artikulli>();
            }
            if (CurrentlySelectedArtikulli == null) return;
            if(CurrentlySelectedArtikulli.Sasia < Sasia) {
                UserDialogs.Instance.Alert("Sasia e shenuar eshte me e madhe se sasia aktuale, ju lutemi ta permiresoni", "Verejtje", "Ok");
                return;
            }
            var tempArts = Artikujt;
            CurrentlySelectedArtikulli.Sasia = Sasia;
            SelectedArikujt.Add(CurrentlySelectedArtikulli);
            TotalPrice = (double)(TotalPrice + CurrentlySelectedArtikulli.CmimiTotal);
            Sasia = 0;
            //Update sasia
            decimal SasiaUpdate = Math.Round(decimal.Parse(CurrentlySelectedArtikulli.Sasia.ToString()), 3);
            var stoqet = await App.Database.GetAllStoqetAsync();
            var stoku = stoqet.FirstOrDefault(x => x.Depo == Agjendi.IDAgjenti && x.Shifra == CurrentlySelectedArtikulli.IDArtikulli);
            decimal sasiaAktuale = Math.Round(decimal.Parse(stoku.Sasia.ToString()), 3);
            stoku.Sasia = double.Parse(Math.Round(sasiaAktuale - SasiaUpdate, 3).ToString());
            await Task.Delay(20);
            //await App.Database.UpdateStoqetAsync(stoku);
            //update malli i mbetur
            var malliIMbetur = MalliMbetur.FirstOrDefault( x=> x.IDArtikulli == CurrentlySelectedArtikulli.IDArtikulli && x.Depo == Agjendi.IDAgjenti);
            decimal SasiaShiturUpdate = Math.Round(decimal.Parse(CurrentlySelectedArtikulli.Sasia.ToString()), 3);
            decimal SasiaShiturAktuale = Math.Round(decimal.Parse(malliIMbetur.SasiaShitur.ToString()), 3);

            malliIMbetur.SasiaShitur = float.Parse(Math.Round(SasiaShiturUpdate + SasiaShiturAktuale, 3).ToString());

            //(sasiaPranuar - (SasiaShitur+SasiaKthyer-LevizjeStoku))
            var sasiaMbeturString = malliIMbetur.SasiaPranuar - malliIMbetur.SasiaShitur + malliIMbetur.SasiaKthyer - malliIMbetur.LevizjeStoku;
            malliIMbetur.SasiaMbetur = float.Parse(Math.Round(double.Parse(sasiaMbeturString.ToString()), 3).ToString());
            malliIMbetur.SyncStatus = 0;
            //await App.Database.UpdateMalliMbeturAsync(malliIMbetur);
            CurrentlySelectedArtikulli = null;

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

        public async Task RegjistroLevizjenAsync() {
            try {
                var levizjet = await App.Database.GetLevizjetHeaderAsync();
                if (levizjet.Count < 1) {
                    var levizjetApiResult = await App.ApiClient.GetAsync("levizje-header/" + Agjendi.DeviceID);
                    if(levizjetApiResult.IsSuccessStatusCode) {
                        var levizjetApiResponse = await levizjetApiResult.Content.ReadAsStringAsync();
                        levizjet = JsonConvert.DeserializeObject<List<LevizjetHeader>>(levizjetApiResponse);
                    }
                    else {
                        UserDialogs.Instance.Alert("Nuk ka levizje header te regjistruara per te ruajtur numrin fiskal, edhe marrja nga rrjeti deshtoi, ju lutemi provoni perseri me vone");
                        return;
                    }
                }
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
                topLevizjeIDN.IDN = topLevizjeIDN.IDN + 1;
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
                var levizjaHeader = new LevizjetHeader
                {
                    Data = DateTime.Now,
                    IDAgjenti = Agjendi.IDAgjenti,
                    Depo = Agjendi.Depo,
                    LevizjeNga = Nga ? Agjendi.Depo : SelectedKlientet.Depo,
                    LevizjeNe = Ne ? Agjendi.Depo : SelectedKlientet.Depo,
                    ImpStatus = 0,
                    NumriLevizjes = levizjNumber.ToString(),
                    KodiDyqanit = Agjendi.Depo,
                    TransferID = transferId,
                    Latitude = geoLocation.Latitude.ToString(),
                    Longitude = geoLocation.Longitude.ToString(),
                    Message = "Levizja ka perfunduar",
                    NumriDaljes = "0",
                    NumriFisk = topLevizjeIDN.IDN,
                    SyncStatus = 0,
                    TCR = Agjendi.TCRCode,
                    TCRBusinessUnitCode = query.FirstOrDefault().BusinessUnitCode,
                    TCRIssueDateTime = Agjendi.TCRRegisteredDateTime,
                    TCROperatorCode = query.FirstOrDefault().OperatorCode,
                    Totali = decimal.Parse(TotalPrice.ToString()),
                    TCRSyncStatus = 0,
                };
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
                        Seri = artikulli.Shifra,
                        SyncStatus = 0,
                        TCRSyncStatus = 0,
                        Totali = decimal.Parse(artikulli.CmimiTotal.ToString())
                    };
                    levizjetDetails.Add(levizjaDetails);
                    await App.Database.SaveMalliMbeturAsync(MalliMbetur.FirstOrDefault(x => x.IDArtikulli == artikulli.IDArtikulli && x.Depo == Agjendi.Depo));
                }
                await App.Database.SaveAllLevizjeDetailsAsync(levizjetDetails);
                await App.Database.SaveLevizjeHeaderAsync(levizjaHeader);
                LevizjetHeader.Add(levizjaHeader);
                await App.Database.UpdateNumriFiskalAsync(topLevizjeIDN);
                UserDialogs.Instance.Alert("Levizja u regjistrua me sukses");
                await App.Instance.PopPageAsync();

            }
            catch (Exception e) {
                UserDialogs.Instance.Alert("Levizja nuk u regjistrua me sukses errori : " + e.Message + " stack trace : " + e.StackTrace);
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
                if(MalliMbetur == null) {
                    var malliMbetur = await App.Database.GetMalliMbeturAsync();
                    MalliMbetur = new ObservableCollection<Malli_Mbetur>(malliMbetur);
                }
                if(SalesPrices == null) {
                    SalesPrices = new ObservableCollection<SalesPrice>(await App.Database.GetSalesPriceAsync());
                }
                ZgjidhArtikullinModalPage zgjidhArtikullinModalPage = new ZgjidhArtikullinModalPage();
                zgjidhArtikullinModalPage.BindingContext = this;
                Artikujt = new ObservableCollection<Artikulli>(await App.Database.GetArtikujtAsync());

                //SalesPrices = new ObservableCollection<SalesPrice>(await App.Database.GetSalesPriceAsync());
                //MalliMbetur = new ObservableCollection<Malli_Mbetur>(await App.Database.GetMalliMbeturAsync());
                foreach (var artikulli in Artikujt) {

                    artikulli.CmimiNjesi = SalesPrices.FirstOrDefault(x => x.ItemNo == artikulli.IDArtikulli).UnitPrice;
                    var MalliiMbetur = MalliMbetur.FirstOrDefault(x => x.IDArtikulli == artikulli.IDArtikulli && x.Depo == Agjendi.Depo);
                    if (MalliiMbetur != null) {
                        artikulli.Sasia = MalliiMbetur.SasiaMbetur;
                        artikulli.Seri = MalliiMbetur.Seri;
                    }
                }
                Artikujt = new ObservableCollection<Artikulli>(Artikujt.Where(x => x.Sasia >= 1).OrderBy(x=> x.Emri));

                await App.Instance.PushAsyncNewModal(zgjidhArtikullinModalPage);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception e) {
                var g = e.Message;
            }

        }
    }
}
