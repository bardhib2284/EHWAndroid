using Acr.UserDialogs;
using EHW_M;
using EHWM.Models;
using EHWM.Views;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using System.IO;
using EHWM.Services;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http;
using Xamarin.Essentials;
using Newtonsoft.Json;
using Color = Xamarin.Forms.Color;
using EHWM.Views.Popups;
using Syncfusion.Compression;
using System.Globalization;
using EHWM.DependencyInjections.FiskalizationExtraModels;
using System.Data;
using Plugin.BxlMpXamarinSDK.Abstractions;
using Plugin.BxlMpXamarinSDK;
using System.Threading;
using Plugin.Connectivity;
using EHWM.Renderers;

namespace EHWM.ViewModel {
    public class MainViewModel : BaseViewModel {

        public ICommand OpenKlientet { get; set; }
        public ICommand GoToDeveloperModeCommand { get; set; }
        public ICommand OpenPorositeCommand { get; set; }
        public ICommand CreateNewKlientPageCommand { get; set; }
        public ICommand SaveNewClientCommand { get; set; }
        public ICommand SaveNewVizitaCommand { get; set; }
        public ICommand SaveSampleArtikujtCommand { get; set; }
        public ICommand SaveAuthenticatedUserCommand { get; set; }
        public ICommand HapVizitenCommand { get; set; }
        public ICommand GoToShitjaCommand { get; set; }
        public ICommand SaveSalesPricesCommand { get; set; }
        public ICommand SaveMalliMbeturCommand { get; set; }
        public ICommand SaveNumriFaturaveCommand { get; set; }
        public ICommand GeneratePDFCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand OpenSinkronizimiCommand { get; set; }
        public ICommand OpenArtikujtCommand { get; set; }
        public ICommand GoToShtoVizitenPageCommand { get; set; }
        public ICommand GoToConfigurimiCommand { get; set; }
        public ICommand SaveConfigurimiCommand { get; set; }
        public ICommand OpenLevizjetCommand { get; set; }
        public ICommand OpenInkasimiCommand { get; set; }
        public ICommand HapFaturatEShituraCommand { get; set; }
        public ICommand ShfaqPorosineCommand { get; set; }
        public ICommand KthePorosineAutomatikishtCommand { get; set; }
        public ICommand PrintoFaturenCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand GoToFiskalizimiCommand { get; set; }
        public ICommand ChangeVizitaStatusCommand { get; set; }
        public ICommand GoToMbetjaMallitCommand { get; set; }
        public ICommand HapInkasimetCommand { get; set; }
        public ICommand PrintTestCommand { get; set; }
        public ICommand GoToAddLinksCommand { get; set; }
        public ICommand CreateLinkunPerFiskalizimCommand { get; set; }
        public ICommand CreateLinkunPerAPICommand { get; set; }
        public ICommand EditLinkunPerAPICommand { get; set; }
        public ICommand EditLinkunPerFiskCommand { get; set; }


        public bool DissapearingFromShitjaPage { get; set; }

        private bool _AllClientsList;
        public bool AllClientsList {
            get { return _AllClientsList; }
            set {
                SetProperty(ref _AllClientsList,value);
            }
        }

        private bool _SearchedClientsList;
        public bool SearchedClientsList {
            get { return _SearchedClientsList; }
            set {
                SetProperty(ref _SearchedClientsList, value);
            }
        }

        public Klientet CreatedKlient { get; set; }
        private ObservableCollection<Klientet> _clients;
        public ObservableCollection<Klientet> Clients {
            get {
                return _clients;
            }
            set {
                SetProperty(ref _clients, value);
            }
        }
        private VizualizimiFatures _SelectedLiferimetEKryera;
        public VizualizimiFatures SelectedLiferimetEKryera {
            get {
                return _SelectedLiferimetEKryera;
            }
            set {
                SetProperty(ref _SelectedLiferimetEKryera, value);
            }
        }       
        private ObservableCollection<VizualizimiFatures> _LiferimetEKryera;
        public ObservableCollection<VizualizimiFatures> LiferimetEKryera {
            get {
                return _LiferimetEKryera;
            }
            set {
                SetProperty(ref _LiferimetEKryera, value);
            }
        }
        private ObservableCollection<Klientet> _searchedClients;
        public ObservableCollection<Klientet> SearchedClients {
            get { return _searchedClients; }
            set { SetProperty(ref _searchedClients, value); }
        }

        private ObservableCollection<Vizita> _searchedVizitat;
        public ObservableCollection<Vizita> SearchedVizitat {
            get { return _searchedVizitat; }
            set { SetProperty(ref _searchedVizitat, value); }
        }

        private Klientet _selectedClient;
        public Klientet SelectedClient {
            get {
                return _selectedClient;
            }
            set {
                SetProperty(ref _selectedClient, value);
            }
        }

        private LiferimiArt _SelectedLiferimiArtDevMode;
        public LiferimiArt SelectedLiferimiArtDevMode {
            get {
                return _SelectedLiferimiArtDevMode;
            }
            set {
                SetProperty(ref _SelectedLiferimiArtDevMode, value);
            }
        }

        private Liferimi _SelectedLiferimiDevMode;
        public Liferimi SelectedLiferimiDevMode {
            get {
                return _SelectedLiferimiDevMode;
            }
            set {
                SetProperty(ref _SelectedLiferimiDevMode, value);
            }
        }
        private NumriFaturave _SelectedNumriFatDevMode;
        public NumriFaturave SelectedNumriFatDevMode {
            get {
                return _SelectedNumriFatDevMode;
            }
            set {
                SetProperty(ref _SelectedNumriFatDevMode, value);
            }
        }
        private NumriFisk _SelectedNumriFiskDevMode;
        public NumriFisk SelectedNumriFiskDevMode {
            get {
                return _SelectedNumriFiskDevMode;
            }
            set {
                SetProperty(ref _SelectedNumriFiskDevMode, value);
            }
        }

        private AuthenticatedUser _SelectedAuthenticatedUser;
        public AuthenticatedUser SelectedAuthenticatedUser {
            get {
                return _SelectedAuthenticatedUser;
            }
            set {
                SetProperty(ref _SelectedAuthenticatedUser, value);
            }
        }

        private Vizita _selectedVizita;
        public Vizita SelectedVizita {
            get {
                return _selectedVizita;
            }
            set {
                SetProperty(ref _selectedVizita, value);
            }
        }

        private Vizita _LastSelectedVizita;
        public Vizita LastSelectedVizita {
            get {
                return _LastSelectedVizita;
            }
            set {
                SetProperty(ref _LastSelectedVizita, value);
            }
        }

        private Agjendet _CurrentAgent;
        public Agjendet CurrentAgent {
            get { return _CurrentAgent; }
            set { SetProperty(ref _CurrentAgent, value); }
        }

        private Depot _Depoja;
        public Depot Depoja{
            get { return _Depoja; }
            set { SetProperty(ref _Depoja, value); }
        }
        private DateTime _FilterDate;
        public DateTime FilterDate {
            get { return _FilterDate; }
            set { SetProperty(ref _FilterDate, value); }
        }

        public DateTime FilterMinDate { get; set; }
        public DateTime FilterMaxDate { get; set; }
        private int currentDay;
        private DateTime DataCal = System.DateTime.Now.Date, dtFisrtDate, dtLastDate;

        public bool EditLinks { get; set; }
        public MainViewModel() {
            OpenKlientet = new Command(async () => await OpenKlientetAsync());
            OpenSinkronizimiCommand = new Command(async () => await OpenSinkronizimiAsync());
            OpenArtikujtCommand = new Command(async () => await OpenArtikujtAsync());
            OpenInkasimiCommand = new Command(async () => await OpenInkasimiAsync());
            OpenPorositeCommand = new Command(async () => await OpenPorositeAsync());
            OpenLevizjetCommand = new Command(async () => await OpenLevizjetAsync());
            CreateNewKlientPageCommand = new Command(async () => await OpenCreateNewKlientAsync());
            SaveNewClientCommand = new Command(async () => await SaveNewClientAsync());
            SaveNewVizitaCommand = new Command(async () => await SaveNewVizitaAsync());
            SaveSampleArtikujtCommand = new Command(async () => await SaveSampleArtikujtAsync());
            SaveAuthenticatedUserCommand = new Command(async () => await SaveAuthenticatedUserAsync());
            HapVizitenCommand = new Command(async () => await HapVizitenAsync());
            SelectedAuthenticatedUser = new AuthenticatedUser();
            GoToShitjaCommand = new Command(async () => await GoToShitjaAsync());
            SaveSalesPricesCommand = new Command(async () => await SaveSalesPricesAsync());
            SaveMalliMbeturCommand = new Command(async () => await SaveMalliMbeturAsync());
            SaveNumriFaturaveCommand = new Command(async () => await SaveNumriFaturaveAsync());
            GeneratePDFCommand = new Command(async () => await GeneratePDFAsync());
            CurrentAgent = new Agjendet();
            LoginCommand = new Command(async () => await LoginAsync());
            GoToShtoVizitenPageCommand = new Command(async () => await GoToShtoVizitenPageAsync());
            HapFaturatEShituraCommand = new Command(async () => await HapFaturatEShituraAsync());
            ShfaqPorosineCommand = new Command(async () => await ShfaqPorosineAsync());
            GoToConfigurimiCommand = new Command(async () => await GoToConfigurimiAsync());
            SaveConfigurimiCommand = new Command(async () => await SaveConfigurimiAsync());
            KthePorosineAutomatikishtCommand = new Command(async () => await KthePorosineAutomatikishtAsync());
            PrintoFaturenCommand = new Command(async () => await PrintoFaturenAsync());
            LogoutCommand = new Command(async () => await LogoutAsync());
            GoToFiskalizimiCommand = new Command(async () => await GoToFiskalizimiAsync());
            ChangeVizitaStatusCommand = new Command(async () => await ChangeVizitaStatusAsync());
            GoToMbetjaMallitCommand = new Command(async () => await GoToMbetjaMallitAsync());
            HapInkasimetCommand = new Command(async () => await HapInkasimetAsync());
            PrintTestCommand = new Command(async () => await PrintoFaturenAsync());
            GoToAddLinksCommand = new Command(async () => await GoToAddLinksAsync());
            GoToDeveloperModeCommand = new Command(async () => await GoToDeveloperModeAsync());
            CreateLinkunPerFiskalizimCommand = new Command(async () => { EditLinks = false; await App.Instance.MainPage.Navigation.PushPopupAsync(new RegjistroLinkunPerFiskalizimiPopup() { BindingContext = this }); });

            CreateLinkunPerAPICommand = new Command(async () => { EditLinks = false; await App.Instance.MainPage.Navigation.PushPopupAsync(new RegjistroLinkunPerAPIPopup() { BindingContext = this }); });

            EditLinkunPerAPICommand = new Command(async () => { EditLinks = true; await App.Instance.MainPage.Navigation.PushPopupAsync(new RegjistroLinkunPerAPIPopup() { BindingContext = this }); });
            EditLinkunPerFiskCommand = new Command(async () => { EditLinks = true; await App.Instance.MainPage.Navigation.PushPopupAsync(new RegjistroLinkunPerFiskalizimiPopup() { BindingContext = this }); });
            FilterDate = DateTime.Now;
            FilterMinDate = DateTime.Now.AddDays(-7);
            FilterMaxDate = DateTime.Now.AddDays(7);
            dtFisrtDate = DateTime.Now.Date.AddDays(-(currentDay - 1));
            dtLastDate = dtFisrtDate.AddDays(7);
            CultureInfo myCI = new CultureInfo("fr-FR");
            System.Globalization.Calendar myCal = myCI.Calendar;
            currentDay = (int)myCal.GetDayOfWeek(DateTime.Now);
            FilterMinDate = DateTime.Now.Date.AddDays(-(currentDay - 1));
            FilterMaxDate = dtFisrtDate.AddDays(7);
            al = new AgjendiLogin();
            LinqetPerAPI = new ObservableCollection<Linqet>();
            LinqetPerFiskalizim = new ObservableCollection<Linqet>();
        }

        private async Task GoToDeveloperModeAsync() {
            await App.Instance.PushAsyncNewPage(new DeveloperMode() { BindingContext = this }); ;
        }
        
        private async Task GoToAddLinksAsync() {
            await App.Instance.PushAsyncNewPage(new AddLinksPage() { BindingContext = this }); ;
        }

        public ObservableCollection<Malli_Mbetur> MalliMbetur { get; set; }

        private double pranuarAll;
        public double PranuarAll {
            get { return pranuarAll; }
            set { SetProperty(ref pranuarAll, value); }
        }
        private double shiturAll;
        public double ShiturAll {
            get { return shiturAll; }
            set { SetProperty(ref shiturAll, value); }
        }
        private double kthyerAll;
        public double KthyerAll {
            get { return kthyerAll; }
            set { SetProperty(ref kthyerAll, value); }
        }
        private double _LevizjeAll;
        public double LevizjeAll {
            get { return _LevizjeAll; }
            set { SetProperty(ref _LevizjeAll, value); }
        }
        private double mbetjaAll;
        public double MbetjaAll {
            get { return mbetjaAll; }
            set { SetProperty(ref mbetjaAll, value); }
        }

        public ObservableCollection<EvidencaPagesave> InkasimetList { get; set; }
        public async Task HapInkasimetAsync() {

            UserDialogs.Instance.ShowLoading("Duke hapur inkasimet");
            InkasimetList = new ObservableCollection<EvidencaPagesave>( await App.Database.GetEvidencaPagesaveAsync());
            InkasimetList = new ObservableCollection<EvidencaPagesave>(InkasimetList.Where(x => x.PayType == "KESH"));
            var klientet = await App.Database.GetKlientetAsync();
            foreach(var inkasim in InkasimetList) {
                foreach(var klient in klientet) {
                    if(klient.IDKlienti == inkasim.IDKlienti) {
                        inkasim.Kontakti = klient.Emri;
                    }
                }
                if(string.IsNullOrEmpty(inkasim.KMON)) {
                    inkasim.KMON = "LEK";
                }
            }
            await App.Instance.MainPage.Navigation.PopPopupAsync();

            await App.Instance.PushAsyncNewPage(new VizualizoInkasimetPage() { BindingContext = this });
            UserDialogs.Instance.HideLoading();

        }
        public async Task GoToMbetjaMallitAsync() {
            PranuarAll = 0;
            shiturAll = 0;
            kthyerAll = 0;
            LevizjeAll = 0;
            MbetjaAll = 0;
            UserDialogs.Instance.ShowLoading("Duke mbledhur mallin e mbetur");
            MalliMbetur = new ObservableCollection<Malli_Mbetur>( await App.Database.GetMalliMbeturAsync());
            MalliMbetur = new ObservableCollection<Malli_Mbetur>(MalliMbetur.Where(x => x.SasiaMbetur != 0));
            foreach (var mall in MalliMbetur) {
                PranuarAll += mall.SasiaPranuar;
                ShiturAll += mall.SasiaShitur;
                KthyerAll += mall.SasiaKthyer;
                LevizjeAll += mall.LevizjeStoku;
                MbetjaAll += mall.SasiaMbetur;
            }
            await App.Instance.MainPage.Navigation.PopPopupAsync();

            await App.Instance.PushAsyncNewPage(new VizualizoMalliMbeturPage() { BindingContext = this });
            UserDialogs.Instance.HideLoading();

        }

        public async Task ChangeVizitaStatusAsync() {
            VizitatFilteredByDate.Remove(SelectedVizita);
            VizitatFilteredByDate = new ObservableCollection<Vizita>(VizitatFilteredByDate.OrderBy(x => x.Klienti));
            VizitatFilteredByDate.Insert(0,SelectedVizita);
            if(SearchedVizitat!= null) {
                SearchedVizitat.Remove(SelectedVizita);
                SearchedVizitat.Insert(0, SelectedVizita);
            }
            UserDialogs.Instance.Alert("Vizita eshte ndryshuar me sukses");
            await App.Database.UpdateVizitaAsync(SelectedVizita);
            await App.Instance.PopPageAsync();
            SelectedVizita = null;
        }

        private CashRegister _cashRegister;
        public CashRegister CashRegister {
            get { return _cashRegister; }
            set { SetProperty(ref _cashRegister, value); }
        }
        public bool EshteRuajtuarArka { get; set; }
        public Agjendet LoginData { get; set; }

        private DateTime _regjistroVizitenDate;
        public DateTime RegjistroVizitenDate {
            get { return _regjistroVizitenDate; }
            set { SetProperty(ref _regjistroVizitenDate, value); }
        }

        public Vizita RegjistroVizitenVizita { get; set; }
        public Configurimi Configurimi { get; set; }

        public string NrFatKthim { get; set; }
        public Guid IDPorosi { get; set; }
        public Guid IDLiferimi { get; set; }

        public async Task GoToFiskalizimiAsync() {

            FiskalizimiViewModelNavigationParams np = new FiskalizimiViewModelNavigationParams
            {
                CashRegisters = await App.Database.GetCashRegisterAsync(),
                Levizjet = await App.Database.GetLevizjetHeaderAsync(),
                Liferimet = await App.Database.GetLiferimetAsync(),
                InkasimetList = await App.Database.GetEvidencaPagesaveAsync()
            };
            FiskalizimiViewModel fvw = new FiskalizimiViewModel(np);

            await App.Instance.PushAsyncNewPage(new FiskalizimiPage() { BindingContext = fvw });

        }

        public async Task LogoutAsync() {
            LoginPage mp = new LoginPage();
            NavigationPage navigationPage = new NavigationPage(mp) { BarBackgroundColor = Color.LightBlue };
            mp.BindingContext = App.Instance.MainViewModel;
            App.Instance.MainPage = navigationPage;
            App.ApiClient = new HttpClient();
            App.ApiClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
        }
            
        public async Task KthePorosineAutomatikishtAsync() {
            UserDialogs.Instance.ShowLoading("Duke kthyer porosine automatikisht...");
            if (SelectedLiferimetEKryera == null) {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert("Fatura eshte e that, ju lutem rishikoni selektimin perseri");
                return;
            }
            var liferimi = await App.Database.GetLiferimetAsync();
            var checkForAlreadyKthimList = LiferimetEKryera.Where(x => x.IDVizita == SelectedLiferimetEKryera.IDVizita);
            if(checkForAlreadyKthimList.Count() > 1) {
                foreach(var lif in checkForAlreadyKthimList) {
                    var lifi = liferimi.FirstOrDefault(x => x.IDLiferimi == lif.IDLiferimi);
                    if (!string.IsNullOrEmpty(lifi.IDKthimi)) {
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert("Fatura vecse eshte kthyer njehere, ju lutem rishikoni selektimin perseri");
                        return;
                    }
                }
            }
            NrFatKthim = SelectedLiferimetEKryera.NrFisk.ToString();
            
            var vizitat = App.Instance.MainViewModel.VizitatFilteredByDate;
            var klientet = await App.Database.GetKlientetAsync();
            var nipt = (from v in vizitat
                        join k in klientet on v.IDKlientDheLokacion equals k.IDKlienti
                        where v.IDVizita == SelectedLiferimetEKryera.IDVizita
                        select k.NIPT).FirstOrDefault();
            var nrCount = (from l in liferimi
                            join k in klientet on l.IDKlienti equals k.IDKlienti
                            where l.NumriFisk.ToString() == NrFatKthim.ToString()
                                    && l.LLOJDOK == "SH"
                                    && k.NIPT == nipt
                            select l.NumriFisk).Count();
            var FiscalisationService = App.Instance.FiskalizationService;
            var agjendi = App.Instance.MainViewModel.LoginData;
            var agjendet = await App.Database.GetAgjendetAsync();
            var depot = await App.Database.GetDepotAsync();
            var FiskalizimiKonfigurimet = await App.Database.GetFiskalizimiKonfigurimetAsync();
            if (FiskalizimiKonfigurimet.Count <= 0) {
                var fiskalizimiKonfigurimetResult = await App.ApiClient.GetAsync("fiskalizimi-konfigurimet");
                if (fiskalizimiKonfigurimetResult.IsSuccessStatusCode) {
                    var fiskalizimiKonfigurimetResponse = await fiskalizimiKonfigurimetResult.Content.ReadAsStringAsync();
                    FiskalizimiKonfigurimet = JsonConvert.DeserializeObject<List<FiskalizimiKonfigurimet>>(fiskalizimiKonfigurimetResponse);
                    await App.Database.SaveFiskalizimiKonfigurimetAsync(FiskalizimiKonfigurimet);
                }
            }
            var numriFiskal = await App.Database.GetNumratFiskalAsync();
            NumriFisk NumriFisk = await App.Database.GetNumratFiskalIDAsync(LoginData.IDAgjenti);
            if (NumriFisk == null) {
                var numriFiskalAPIResult = await App.ApiClient.GetAsync("numri-fisk/" + LoginData.IDAgjenti);
                if (numriFiskalAPIResult.IsSuccessStatusCode) {
                    var numriFiskalResponse = await numriFiskalAPIResult.Content.ReadAsStringAsync();
                    NumriFisk = JsonConvert.DeserializeObject<NumriFisk>(numriFiskalResponse);
                    await App.Database.SaveNumriFiskalAsync(NumriFisk);
                    numriFiskal.Add(NumriFisk);
                }
                else {
                    UserDialogs.Instance.Alert("Problem ne numrin fiskal, ju lutem provoni perseri.");
                    UserDialogs.Instance.HideLoading();
                    return;
                }
            }
            var query = from a in agjendet
                        join d in depot on a.Depo equals d.Depo
                        join fk in FiskalizimiKonfigurimet on d.TAGNR equals fk.TAGNR
                        join nf in numriFiskal on fk.TCRCode equals nf.TCRCode into nfGroup
                        from nf in nfGroup.DefaultIfEmpty()
                        where a.IDAgjenti.ToLower() == agjendi.IDAgjenti.ToLower()
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
            if (nrCount == 0 && FiscalisationService.CheckCorrectiveInvoice(NrFatKthim.ToString().Trim(), agjendi.Depo, App.Instance.MainViewModel.Configurimi.KodiTCR, agjendi.OperatorCode, query.FirstOrDefault().BusinessUnitCode, nipt) <= 0) {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert(@"Fusha ""Nr. Fat. Kthim"" nuk është i sakt, ju lutemi rishikoni edhe njëherë!", "Verejtje");
                return;
            }
            else {
                SelectedLiferimetEKryera.Totali = SelectedLiferimetEKryera.Totali * -1;

                foreach (var art in SelectedLiferimetEKryera.ListaEArtikujve) {
                    art.Sasia = art.Sasia * -1;
                }
                DateTime MyTimeInWesternEurope = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "GMT Standard Time").AddHours(2);
                var FaturaArritjes = MyTimeInWesternEurope;

                var NumriFaturaveList = await App.Database.GetNumriFaturaveAsync();
                var NumriFaturave = NumriFaturaveList.FirstOrDefault(x => x.KOD == LoginData.IDAgjenti);
                if (NumriFaturave != null) {
                    var currentNumriFatures = NumriFaturave.CurrNrFat;
                    var currentNumriKUFIP = NumriFaturave.NRKUFIP;
                    var currentNumriKUFIS = NumriFaturave.NRKUFIS;
                    if (currentNumriFatures + currentNumriKUFIP > currentNumriKUFIS) {
                        UserDialogs.Instance.Alert("Gabim ne leximin e fatures", "Error", "Ok");
                        return;
                    }
                    else {
                        currentNumriFatures = currentNumriFatures + currentNumriKUFIP;
                        Porosite newPorosi = new Porosite
                        {
                            IDPorosia = Guid.NewGuid(),
                            IDVizita = SelectedLiferimetEKryera.IDVizita,
                        };
                        if (FaturaArritjes != null) {
                            DateTime dp = MyTimeInWesternEurope;
                            DateTime dl = MyTimeInWesternEurope;
                            DateTime KohaPorosise = MyTimeInWesternEurope.ToLocalTime();
                            //update porosite
                            IDPorosi = Guid.NewGuid();
                            var porosite = await App.Database.GetPorositeAsync();
                            var location = await Geolocation.GetLastKnownLocationAsync();
                            Porosite porosite1 = new Porosite
                            {
                                IDPorosia = IDPorosi,
                                IDVizita = SelectedLiferimetEKryera.IDVizita,
                                NrPorosise = currentNumriFatures.ToString(),
                                DataPerLiferim = MyTimeInWesternEurope,
                                DataPorosise = MyTimeInWesternEurope,
                                StatusiPorosise = 1,
                                DeviceID = LoginData.DeviceID,
                                NrDetalet = SelectedLiferimetEKryera.ListaEArtikujve.Count,
                                Longitude = location?.Longitude.ToString(),
                                Latitude = location?.Latitude.ToString(),
                                SyncStatus = 0,
                                OraPorosise = MyTimeInWesternEurope,
                                TitulliPorosise = null,
                            };
                            //SEND POROSIA TO API SO 
                            await App.Database.SavePorositeAsync(porosite1);
                            var userResult = await UserDialogs.Instance.ConfirmAsync("Jeni te sigurte per kthimin e fatures?", "Verejtje", "Po", "Jo");
                            if(userResult) {

                                await PerfundoLiferimin(SelectedLiferimetEKryera, IDPorosi);
                                FixDataVizualizimit();
                                UserDialogs.Instance.HideLoading();
                            }
                        }
                    }
                }

            }
            UserDialogs.Instance.HideLoading();
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
            if(_printSemaphore == null) {
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
            await App.Instance.PushAsyncNewPage(new PrinterSelectionPage() { BindingContext = this});
        }


        async Task OnPrintTextClickedInkasimet() {

            // Prepares to communicate with the printer 
            _printer = await OpenPrinterService(_connectionInfo) as MPosControllerPrinter;

            if (_printer == null)
                return;

            try {
                await _printSemaphore.WaitAsync();

                uint textCount = 0;
                string printText = string.Empty;


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
                            120/*(int)MPosImageWidth.MPOS_IMAGE_WIDTH_ASIS*/,   // Image Width
                            (int)MPosAlignment.MPOS_ALIGNMENT_CENTER,           // Alignment
                            60,                                                 // brightness
                            true,                                               // Image Dithering
                            true);
                await _printer.printLine(1, 1, 1, 1, 1);
                await _printer.printText("\nR A P O R T I   I   I N K A S I M E V E (Faturave) \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_CENTER, Bold = false, });
                await _printer.printLine(0, 0, 1, 1, 1);
                await _printer.printText(
"---------------------------------------------------------------------\n");
                await _printer.printText("", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });
                await _printer.printText(" \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });
                await _printer.printText("\n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });
                await _printer.printText("\n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });
                DateTime MyTimeInWesternEurope = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "GMT Standard Time").AddHours(2);

                var agjender = await App.Database.GetAgjendetAsync();
                var agjendi = agjender.FirstOrDefault(x => x.Depo == LoginData.Depo);
                await _printer.printText("          " +agjendi.Emri + " " + agjendi.Mbiemri + "         " + MyTimeInWesternEurope.ToString("dd.MM.yyyy HH:mm:ss") + "\n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });

                await _printer.printText("------------------------------------------------------------------------------------------------------------------------------------------");

                await _printer.printText("\nKlienti             Tipi    Monedhe    Fatura     Totali     Paguar\n");
                Debug.WriteLine("\nKlienti             Tipi    Monedhe    Fatura     Totali     Paguar\n");
                await _printer.printText("---------------------------------------------------------------------\n");
                float teGjithaSasit = 0f;
                float teGjithaCmimetNjesi = 0f;
                double teGjitheCmimetTotale = 0f;
                var tvsh = 0m;
                var nrBarkodi = 0;
                var klientet = await App.Database.GetKlientetAsync();
                var evidencatEPagesave = await App.Database.GetEvidencaPagesaveAsync();
                var InkasimetListToPrint = evidencatEPagesave.Where(x => x.PayType == "KESH").ToList();
                int reservedSpaceForEachElement = 10;
                string emptySpace;
                string sPranuar;
                string sShitur;
                string sKthyer;
                string slevizje;
                string smbetur;
                string scmimi;
                foreach (var art in InkasimetListToPrint) {
                    foreach(var klient in klientet) {
                        if (klient.IDKlienti != art.IDKlienti)
                            continue;
                        reservedSpaceForEachElement = 10;
                        
                        string prntBuilder = string.Empty; 
                        if (klient.Emri.Trim().Length > 18) {
                            klient.Emri = klient.Emri.Trim().Remove(18);
                        }
                        emptySpace = "\n";
                        sPranuar = klient.Emri.Trim();
                        sShitur = art.PayType ?? " ";
                        sKthyer = art.KMON ?? " ";
                        slevizje = art.NrFatures ?? " ";
                        smbetur = String.Format("{0:0.00}", art.ShumaTotale);
                        scmimi = String.Format("{0:0.00}", art.ShumaPaguar);
                        if (sPranuar.Length < (reservedSpaceForEachElement + 8)) {
                            var index = 1;
                            while (sPranuar.Length < (reservedSpaceForEachElement + 8)) {
                                sPranuar += " ";
                            }
                        }
                        if (sShitur.Length < (reservedSpaceForEachElement - 2)) {
                            var index = 1;
                            while (sShitur.Length < (reservedSpaceForEachElement - 2)) {
                                if (index > 0) {
                                    sShitur = sShitur.Insert(0, " ");
                                }
                                else
                                    sShitur += " ";
                                index *= -1;
                            }
                        }
                        if (sKthyer.Length < reservedSpaceForEachElement + 1) {
                            var index = 1;
                            while (sKthyer.Length < reservedSpaceForEachElement + 1) {
                                if (index > 0) {
                                    sKthyer = sKthyer.Insert(0, " ");
                                }
                                else
                                    sKthyer += " ";
                                index *= -1;
                            }
                        }

                        if (slevizje.Length < reservedSpaceForEachElement + 1) {
                            var index = 1;
                            while (slevizje.Length < reservedSpaceForEachElement + 1) {
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
                        teGjithaCmimetNjesi += (float)art.ShumaPaguar;
                    }

                }


                await _printer.printText("\n---------------------------------------------------------------------");



                //printText = "A. 1. عدد ۰۱۲۳۴۵۶۷۸۹" + "\nB. 2. عدد 0123456789" + "\nC. 3. به" + "\nD. 4. نه" + "\nE. 5. مراجعه" + "\n";// 
                //await _printer.printText(printText, new MPosFontAttribute() { CodePage = (int)MPosCodePage.MPOS_CODEPAGE_FARSI, Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });     // Persian 
                await _printer.printText("\n");
                await _printer.printText("\n");
                await _printer.printText("Llojet e pagesave \n");
                await _printer.printText("Shuma e paguar ne EURO :                               0.00\n");
                await _printer.printText("Shuma e paguar ne LEK  :                               " + String.Format("{0:0.00}", teGjithaCmimetNjesi) + "\n");
                await _printer.printText("Shuma e paguar ne USD  :                               0.00\n");


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
        async Task OnPrintTest() {

            // Prepares to communicate with the printer 
            //_printer = await OpenPrinterService(_connectionInfo) as MPosControllerPrinter;

            

            try {
               // await _printSemaphore.WaitAsync();

                uint textCount = 0;
                string printText = "_Hola Xamarin\n";


                //sd.AddToPreview();
                // sd.Preview();
                //lRet = await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosCodePage.MPOS_CODEPAGE_WPC1252 });

                // note : Page mode and transaction mode cannot be used together between IN and OUT.
                // When "setTransaction" function called with "MPOS_PRINTER_TRANSACTION_IN", print data are stored in the buffer.
                //await _printer.setTransaction((int)MPosTransactionMode.MPOS_PRINTER_TRANSACTION_IN);
                // Printer Setting Initialize
               // await _printer.directIO(new byte[] { 0x1b, 0x40 });

                // Code Pages for the contries in east Asia. Please note that the font data downloading is required to print characters for Korean, Japanese and Chinese.
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosEastAsiaCodePage.MPOS_CODEPAGE_KSC5601 });   // Korean
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosEastAsiaCodePage.MPOS_CODEPAGE_SHIFTJIS });  // Japanese
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosEastAsiaCodePage.MPOS_CODEPAGE_GB2312 });    // Simplifies Chinese
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosEastAsiaCodePage.MPOS_CODEPAGE_BIG5 });      // Traditional Chinese
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosCodePage.MPOS_CODEPAGE_FARSI });     // Persian 
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosCodePage.MPOS_CODEPAGE_FARSI_II });  // Persian 

                //await _printer.setTransaction((int)MPosTransactionMode.MPOS_PRINTER_TRANSACTION_IN);


                
                var liferimi = await App.Database.GetLiferimetAsync();
                var lif = liferimi.FirstOrDefault(x => x.IDLiferimi == SelectedLiferimetEKryera.IDLiferimi);
                
                var klientet = await App.Database.GetKlientetAsync();
                var klienti = klientet.FirstOrDefault(x => x.IDKlienti == lif.IDKlienti);
                var klientetDheLokacionet = await App.Database.GetKlientetDheLokacionetAsync();
                var klientiDheLokacioni = klientetDheLokacionet.FirstOrDefault(x => x.IDKlienti == lif.IDKlienti);
               
                float teGjithaSasit = 0f;
                float teGjithaCmimetNjesi = 0f;
                double teGjitheCmimetTotale = 0f;
                var liferimetArt = await App.Database.GetLiferimetArtAsync();
                var liferimiArt = liferimetArt.Where(x => x.IDLiferimi == lif.IDLiferimi);
                var tvsh = 0m;
                SelectedLiferimetEKryera.ListaEArtikujve = (from s in SelectedLiferimetEKryera.ListaEArtikujve
                                                            orderby s.IDArtikulli
                                                            select s).ToList();
                                                           
                foreach (var art in SelectedLiferimetEKryera.ListaEArtikujve) {
                    string prntBuilder = string.Empty;

                    prntBuilder += "\n" + art.IDArtikulli + "   " + art.Emri + "   " + art.Seri;


                    //BUM
                    if (art.BUM.Length >= 4) {
                        prntBuilder += "\n                    " + art.BUM + "    ";
                    }
                    else if (art.BUM.Length >= 3) {
                        prntBuilder += "\n                    " + art.BUM + "     ";
                    }
                    else if (art.BUM.Length >= 2) {
                        prntBuilder += "\n                    " + art.BUM + "      ";
                    }

                    //SASIA-

                    if (art.Sasia.ToString().Length >= 4) {
                        prntBuilder += +art.Sasia + "  ";
                    }
                    else if (art.Sasia.ToString().Length >= 3) {
                        prntBuilder += +art.Sasia + "   ";
                    }
                    else if (art.Sasia.ToString().Length >= 2) {
                        prntBuilder += +art.Sasia + "    ";
                    }
                    else if (art.Sasia.ToString().Length >= 1) {
                        prntBuilder += +art.Sasia + "     ";
                    }

                    //cmimiNjesi
                    var cmimiNjesi = String.Format("{0:0.00}", art.CmimiNjesi);
                    if (cmimiNjesi.Contains("-")) {
                        cmimiNjesi = cmimiNjesi.Remove(0, 1);
                    }
                    if (cmimiNjesi.Length >= 9) {
                        prntBuilder += art.CmimiNjesi + "  ";
                    }
                    else if (cmimiNjesi.Length >= 8) {
                        prntBuilder += art.CmimiNjesi + "   ";
                    }
                    else if (cmimiNjesi.Length >= 7) {
                        prntBuilder += art.CmimiNjesi + "    ";
                    }
                    else if (cmimiNjesi.Length >= 6) {
                        prntBuilder += art.CmimiNjesi + "     ";
                    }
                    else if (cmimiNjesi.Length >= 5) {
                        prntBuilder += art.CmimiNjesi + "      ";
                    }
                    else if (cmimiNjesi.Length >= 4) {
                        prntBuilder += art.CmimiNjesi + "       ";
                    }
                    else if (cmimiNjesi.Length >= 3) {
                        prntBuilder += art.CmimiNjesi + "        ";
                    }
                    //vlera pa tvsh
                    var vleraPaTVSH = String.Format("{0:0.00}", Math.Round(decimal.Parse((art.CmimiNjesi * art.Sasia).ToString()) / 1.2m, 2));
                    tvsh = decimal.Parse((art.CmimiNjesi * art.Sasia).ToString()) - decimal.Parse(vleraPaTVSH);
                    var tvshstring = String.Format("{0:0.00}", tvsh);

                    var total = art.CmimiNjesi * art.Sasia;

                    var vlera = String.Format("{0:0.00}", total);
                    if (vleraPaTVSH.Length >= 9) {
                        prntBuilder = prntBuilder.Remove(prntBuilder.Length - 1, 1);
                        prntBuilder += vleraPaTVSH + "  ";
                    }
                    else if (vleraPaTVSH.Length >= 8) {
                        if (tvshstring.Length == 7 && vlera.Length == 8) {
                            prntBuilder += vleraPaTVSH + "   ";
                        }
                        else
                            prntBuilder += vleraPaTVSH + "  ";
                    }
                    else if (vleraPaTVSH.Length >= 7) {
                        if (tvshstring.Length == 7 && vlera.Length == 8) {
                            prntBuilder += vleraPaTVSH + "   ";
                        }
                        else {
                            prntBuilder += vleraPaTVSH + "   ";
                        }
                    }
                    else if (vleraPaTVSH.Length >= 6) {
                        prntBuilder += vleraPaTVSH + "    ";
                    }
                    else if (vleraPaTVSH.Length >= 5) {
                        prntBuilder += vleraPaTVSH + "     ";
                    }
                    else if (vleraPaTVSH.Length >= 4) {
                        prntBuilder += vleraPaTVSH + "      ";
                    }


                    //TVSH
                    if (tvshstring.Length >= 8) {
                        if (vlera.Length > tvshstring.Length) {
                            prntBuilder += tvshstring + "";
                        }
                        else
                            prntBuilder += tvshstring + " ";
                    }
                    else if (tvshstring.Length >= 7) {
                        if (vleraPaTVSH.Length == 8) {
                            prntBuilder += "" + tvshstring + " ";
                        }
                        else if (vleraPaTVSH.Length == 7) {
                            if (vlera.Length == 7) {
                                prntBuilder += " " + tvshstring + "  ";
                            }
                            else
                                prntBuilder += " " + tvshstring + " ";
                        }
                        else {
                            prntBuilder += tvshstring + "  ";
                        }
                    }
                    else if (tvshstring.Length >= 6) {
                        if (vleraPaTVSH.Length == 7) {
                            prntBuilder += " " + tvshstring + "   ";
                        }
                        else
                            prntBuilder += tvshstring + "  ";
                    }
                    else if (tvshstring.Length >= 5) {
                        prntBuilder += "  " + tvshstring + "    ";
                    }
                    else if (tvshstring.Length >= 4) {
                        prntBuilder += tvshstring + "     ";
                    }
                    else if (tvshstring.Length >= 3) {
                        prntBuilder += tvshstring + "      ";
                    }
                    else if (tvshstring.Length >= 2) {
                        prntBuilder += tvshstring + "       ";
                    }
                    //vlera
                    if (art.Sasia < 0 && total > 0) {
                        total = total * -1;
                    }
                    prntBuilder += vlera + "\n";
                    //await _printer.printText(prntBuilder);
                    Debug.WriteLine(prntBuilder + "Length:" + prntBuilder.Length);
                    teGjithaSasit += (float)art.Sasia;
                }


                string totalBuilder = string.Empty;
                totalBuilder += "\n    Mbyllet me total     ";
                if (teGjithaSasit.ToString().Length >= 5) {
                    totalBuilder += "   " + teGjithaSasit + "    ";

                }
                else if (teGjithaSasit.ToString().Length >= 4) {
                    totalBuilder += "   " + teGjithaSasit + "     ";

                }
                else if (teGjithaSasit.ToString().Length >= 3) {
                    totalBuilder += "   " + teGjithaSasit + "         ";

                }
                else if (teGjithaSasit.ToString().Length >= 2) {
                    totalBuilder += "   " + teGjithaSasit + "          ";
                }
                else if (teGjithaSasit.ToString().Length >= 1) {
                    totalBuilder += "   " + teGjithaSasit + "           ";
                }


                var tvshAll = String.Format("{0:0.00}", Math.Round((lif.CmimiTotal - lif.TotaliPaTVSH), 2));
                var cmimTotal = String.Format("{0:0.00}", lif.CmimiTotal);
                //v.patvsh
                var vptvsh = String.Format("{0:0.00}", lif.TotaliPaTVSH);
                if (vptvsh.Length >= 10) {
                    totalBuilder += "" + String.Format("{0:0.00}", lif.TotaliPaTVSH);
                }
                else if (vptvsh.Length >= 9) {
                    totalBuilder += " " + String.Format("{0:0.00}", lif.TotaliPaTVSH);
                }
                else if (vptvsh.Length >= 8) {
                    if (tvshAll.Length == 7 && cmimTotal.Length == 8) {
                        totalBuilder += "     " + String.Format("{0:0.00}", lif.TotaliPaTVSH);
                    }
                    else
                        totalBuilder += "  " + String.Format("{0:0.00}", lif.TotaliPaTVSH);
                }
                else if (vptvsh.Length >= 7) {
                    totalBuilder += "  " + String.Format("{0:0.00}", lif.TotaliPaTVSH);
                }
                else if (vptvsh.Length >= 6) {
                    totalBuilder += "  " + String.Format("{0:0.00}", lif.TotaliPaTVSH);
                }
                else if (vptvsh.Length >= 5) {
                    totalBuilder += "  " + String.Format("{0:0.00}", lif.TotaliPaTVSH);
                }

                if (tvshAll.ToString().Length >= 9) {
                    totalBuilder += "" + String.Format("{0:0.00}", Math.Round((lif.CmimiTotal - lif.TotaliPaTVSH), 2)) + " ";
                }
                else if (tvshAll.ToString().Length >= 8) {
                    if (tvshAll.ToString().Length < cmimTotal.Length) {
                        if (cmimTotal.Length == 9 && vptvsh.Length == 9 && teGjithaSasit.ToString().Length > 3) {
                            totalBuilder += "  " + String.Format("{0:0.00}", Math.Round((lif.CmimiTotal - lif.TotaliPaTVSH), 2)) + "  ";
                        }
                        else
                            totalBuilder += "  " + String.Format("{0:0.00}", Math.Round((lif.CmimiTotal - lif.TotaliPaTVSH), 2));
                    }
                    else
                        totalBuilder += "  " + String.Format("{0:0.00}", Math.Round((lif.CmimiTotal - lif.TotaliPaTVSH), 2)) + " ";
                }
                else if (tvshAll.ToString().Length >= 7) {
                    if (vptvsh.Length == 8) {
                        if (cmimTotal.Length == 8) {
                            totalBuilder += "   " + String.Format("{0:0.00}", Math.Round((lif.CmimiTotal - lif.TotaliPaTVSH), 2)) + " ";
                        }
                        else
                            totalBuilder += "   " + String.Format("{0:0.00}", Math.Round((lif.CmimiTotal - lif.TotaliPaTVSH), 2)) + "   ";
                    }
                    else if (vptvsh.Length == 7) {
                        if (cmimTotal.Length == 8) {
                            totalBuilder += "    " + String.Format("{0:0.00}", Math.Round((lif.CmimiTotal - lif.TotaliPaTVSH), 2)) + " ";
                        }
                        else
                            totalBuilder += "    " + String.Format("{0:0.00}", Math.Round((lif.CmimiTotal - lif.TotaliPaTVSH), 2)) + "  ";
                    }
                    else
                        totalBuilder += "   " + String.Format("{0:0.00}", Math.Round((lif.CmimiTotal - lif.TotaliPaTVSH), 2)) + "  ";
                }
                else if (tvshAll.ToString().Length >= 6) {
                    if (cmimTotal.Length == 7) {
                        totalBuilder += "    " + String.Format("{0:0.00}", Math.Round((lif.CmimiTotal - lif.TotaliPaTVSH), 2)) + "  ";
                    }
                    else if (vptvsh.Length == 7) {
                        totalBuilder += "    " + String.Format("{0:0.00}", Math.Round((lif.CmimiTotal - lif.TotaliPaTVSH), 2)) + "   ";
                    }
                    else if (vptvsh.Length == 6) {
                        totalBuilder += "      " + String.Format("{0:0.00}", Math.Round((lif.CmimiTotal - lif.TotaliPaTVSH), 2)) + "   ";
                    }
                    else
                        totalBuilder += "   " + String.Format("{0:0.00}", Math.Round((lif.CmimiTotal - lif.TotaliPaTVSH), 2)) + "  ";
                }
                else if (tvshAll.ToString().Length >= 5) {
                    totalBuilder += "      " + String.Format("{0:0.00}", Math.Round((lif.CmimiTotal - lif.TotaliPaTVSH), 2)) + "    ";
                }

                if (tvshAll.Length == 8 && cmimTotal.Length == 8) {
                    totalBuilder += "" + cmimTotal;
                }
                else {
                    totalBuilder += cmimTotal;
                }
                if (totalBuilder.Length > 69) {
                    bool wasLastEmpty = false;
                    string totalTemp = totalBuilder;
                    for (int i = totalBuilder.Length-1; i >= 0; i--) {
                        var de = totalBuilder[i];
                        if (wasLastEmpty) {
                            if (totalBuilder[i] == ' ') {
                                wasLastEmpty = true;
                                totalTemp = totalTemp.Remove(totalTemp[i], 1);
                                if (totalTemp.Length <= 69) {
                                    totalBuilder = totalTemp;
                                    break;
                                }
                            }
                        }
                        if (totalBuilder[i] == ' ') {
                            wasLastEmpty = true;
                        }
                    }
                }
                Debug.WriteLine(totalBuilder + "Length:" + totalBuilder.Length);
                //await _printer.printText(totalBuilder);


                //printText = "A. 1. عدد ۰۱۲۳۴۵۶۷۸۹" + "\nB. 2. عدد 0123456789" + "\nC. 3. به" + "\nD. 4. نه" + "\nE. 5. مراجعه" + "\n";// 
                //await _printer.printText(printText, new MPosFontAttribute() { CodePage = (int)MPosCodePage.MPOS_CODEPAGE_FARSI, Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });     // Persian 


               
                var agjendet = await App.Database.GetAgjendetAsync();
                var agjendi = agjendet.FirstOrDefault(x => x.IDAgjenti == LoginData.IDAgjenti);
                var klientiLength = klienti.Emri.Trim();
                var agjendiLengh = agjendi.Emri + " " + agjendi.Mbiemri;
                var differenceBetweenTheTwo = 60 - agjendiLengh.Length - klientiLength.Length;
                var emptyString = String.Empty;
                for (int i = 0; i < differenceBetweenTheTwo; i++) {
                    emptyString += " ";
                }

                
                totalBuilder = String.Empty;
                var TotaliPaTVSH = String.Format("{0:0.00}", Math.Round(lif.TotaliPaTVSH, 2));
                if (TotaliPaTVSH.ToString().Length >= 10) {
                    totalBuilder += "\n   S-VAT             20        " + String.Format("{0:0.00}", Math.Round(lif.TotaliPaTVSH, 2)) + "      ";
                }
                else if (TotaliPaTVSH.ToString().Length >= 9) {
                    totalBuilder += "\n   S-VAT             20         " + String.Format("{0:0.00}", Math.Round(lif.TotaliPaTVSH, 2)) + "       ";
                }
                else if (TotaliPaTVSH.ToString().Length >= 8) {
                    totalBuilder += "\n   S-VAT             20          " + String.Format("{0:0.00}", Math.Round(lif.TotaliPaTVSH, 2)) + "        ";
                }
                else if (TotaliPaTVSH.ToString().Length >= 7) {
                    totalBuilder += "\n   S-VAT             20           " + String.Format("{0:0.00}", Math.Round(lif.TotaliPaTVSH, 2)) + "        ";
                }
                else if (TotaliPaTVSH.ToString().Length >= 6) {
                    totalBuilder += "\n   S-VAT             20             " + String.Format("{0:0.00}", Math.Round(lif.TotaliPaTVSH, 2)) + "           ";
                }
                else if (TotaliPaTVSH.ToString().Length >= 5) {
                    totalBuilder += "\n   S-VAT             20              " + String.Format("{0:0.00}", Math.Round(lif.TotaliPaTVSH, 2)) + "           ";
                }


                if (tvshAll.ToString().Length >= 9) {
                    totalBuilder += tvshAll + " ";
                }
                else if (tvshAll.ToString().Length >= 8) {
                    totalBuilder += tvshAll + "  ";
                }
                else if (tvshAll.ToString().Length >= 7) {
                    if (TotaliPaTVSH.Length == 7) {
                        if (cmimTotal.Length == 8) {
                            totalBuilder += "  " + tvshAll + "   ";
                        }
                        else
                            totalBuilder += "  " + tvshAll + "    ";
                    }
                    else if (TotaliPaTVSH.Length == 8 && cmimTotal.Length == 8) {
                        totalBuilder += tvshAll + "     ";
                    }
                    else
                        totalBuilder += tvshAll + "   ";
                }
                else if (tvshAll.ToString().Length >= 6) {
                    if (TotaliPaTVSH.Length == 7) {
                        totalBuilder += "  " + tvshAll + "    ";
                    }
                    else
                        totalBuilder += tvshAll + "   ";
                }
                else if (tvshAll.ToString().Length >= 5) {
                    totalBuilder += tvshAll + "     ";
                }

                if (tvshAll.Length == 8 && cmimTotal.Length == 9) {
                    totalBuilder += "  " + cmimTotal;
                }
                else if (tvshAll.Length == 8 && cmimTotal.Length == 8) {
                    totalBuilder += " " + cmimTotal;
                }
                else {
                    totalBuilder += cmimTotal;
                }
                //await _printer.printText(totalBuilder);


               
            }
            catch (Exception ex) {
                UserDialogs.Instance.Alert("Exception", ex.Message, "OK");
            }
            finally {
                
            }
        }


        public async Task OnDeviceOpenClicked() {
            if (_printer == null) {
                // Prepares to communicate with the printer 
                _printer = await OpenPrinterService(_connectionInfo) as MPosControllerPrinter;
                if(App.Instance.MainPage is NavigationPage np) {
                    if (np.Navigation.NavigationStack[np.Navigation.NavigationStack.Count - 2] is FaturatEShituraPage) {
                            await OnPrintTextClickedAllFaturat();
                        return;
                    }
                    if (np.Navigation.NavigationStack[np.Navigation.NavigationStack.Count -2] is VizualizoMalliMbeturPage) {
                            await OnPrintTextClickedMalliMbetur();
                            return;
                    }
                    if (np.Navigation.NavigationStack[np.Navigation.NavigationStack.Count -2] is VizualizoInkasimetPage) {
                            await OnPrintTextClickedInkasimet();
                            return;
                    }
                }
                if (SelectedLiferimetEKryera == null) {
                    UserDialogs.Instance.Alert("Ju lutem zgjedhni njeren prej faturave");
                    return;
                }
                await OnPrintTextClicked();
            }
            else {
                if (App.Instance.MainPage is FlyoutPage fp) {
                    if (fp.Detail is NavigationPage np) {
                        if (np.Navigation.NavigationStack[np.Navigation.NavigationStack.Count - 2] is VizualizoMalliMbeturPage) {
                            await OnPrintTextClickedMalliMbetur();
                            return;
                        }
                        if (np.Navigation.NavigationStack[np.Navigation.NavigationStack.Count - 2] is FaturatEShituraPage) {
                            await OnPrintTextClickedAllFaturat();
                            return;
                        }
                    }
                }
                if (SelectedLiferimetEKryera == null) {
                    UserDialogs.Instance.Alert("Ju lutem zgjedhni njeren prej faturave");
                    return;
                }
                await OnPrintTextClicked();
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
                await _printer.printText("\nF A T U R E    T A T I M O R E   SH I T J E \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_CENTER, Bold = false, });
                await _printer.printLine(0, 0, 1, 1, 1);
                await _printer.printText(
"---------------------------------------------------------------------\n");

                var depot = await App.Database.GetDepotAsync();
                var currDepo = depot.FirstOrDefault(x => x.Depo == LoginData.Depo);
                await _printer.printText("Shitesi: E. H. W.          J61804031V \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });
                await _printer.printText("Tel: 048 200 711           web: www.ehwgmbh.com \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });
                await _printer.printText("Adresa: "+ currDepo.TAGNR +"             \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });
                await _printer.printText("Qyteti / Shteti: Tirana, Albania \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });
                await _printer.printText("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");
                var liferimi = await App.Database.GetLiferimetAsync();
                
                var lif = liferimi.FirstOrDefault(x => x.IDLiferimi == SelectedLiferimetEKryera.IDLiferimi);
                if (lif.CmimiTotal < 0) {
                    await _printer.printText("\nKodi / Emri i llojit te fatures : " + 380 + " / Fature korigjuese");
                    await _printer.printText("\nLloji i procesit:");
                    await _printer.printText("\nP10 Faturimi korrigjues ( anulim / korigjim i nje fature )\n");
                }
                else {
                    await _printer.printText("\nKodi / Emri i llojit te fatures : " + 388 + " / Fature tatimore");
                    await _printer.printText("\nLloji i procesit:");
                    await _printer.printText("\nP3 Faturimi i dorezimit te porosise se blerjes se rastesishme \n");
                }
                await _printer.printText(
"---------------------------------------------------------------------");
                DateTime MyTimeInWesternEurope = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "GMT Standard Time").AddHours(2);

                await _printer.printText("\nNumri i fatures: " + lif.NumriFisk + "/" + lif.KohaLiferimit.Year);
                await _printer.printText("\nData dhe ora e leshimit te fatures: " + lif.KohaLiferimit.ToString("dd.MM.yyyy HH:mm:ss"));
                await _printer.printText("\nMenyra e pageses: " + lif.PayType);
                await _printer.printText("\nMonedha e fatures: ALL");
                await _printer.printText("\nKodi i vendit te ushtrimit te veprimtarise se biznesit: " + lif.TCRBusinessCode);
                await _printer.printText("\nKodi i operatorit : " + lif.TCROperatorCode);
                await _printer.printText("\nNIVF:  " + lif.TCRNIVF);
                await _printer.printText("\nNSLF:  " + lif.TCRNSLF + "\n");
                await _printer.printText(
"---------------------------------------------------------------------");
                var klientet = await App.Database.GetKlientetAsync();
                var klienti = klientet.FirstOrDefault(x => x.IDKlienti == lif.IDKlienti);
                var klientetDheLokacionet = await App.Database.GetKlientetDheLokacionetAsync();
                var klientiDheLokacioni = klientetDheLokacionet.FirstOrDefault(x => x.IDKlienti == lif.IDKlienti);
                await _printer.printText("\nBleresi: " + klienti.Emri + " " + klienti.NIPT);
                await _printer.printText("\nAdresa: " + klientiDheLokacioni.Adresa + "       \n");

                await _printer.printText(
"---------------------------------------------------------------------");
                var agjendet = await App.Database.GetAgjendetAsync();
                var currAgjendi = agjendet.FirstOrDefault(x => x.Depo == LoginData.Depo);
                await _printer.printText("\nTransportues: E. H. W. J61804031V");
               
                await _printer.printText("\nAdresa: " + currDepo.TAGNR + "  (" + currAgjendi.Emri + " " + currAgjendi.Mbiemri + ")");
                await _printer.printText("\nData dhe ora e furinizimit: " + lif.KohaLiferimit.ToString("dd.MM.yyyy HH:mm:ss") + "  \n");

                await _printer.printText("------------------------------------------------------------------------------------------------------------------------------------------");

                await _printer.printText("\nKodi   Pershkrimi  Njesia  Sasia  Cmimi   V.PaTVSH    TVSH    VLERA  \n");
                await _printer.printText("---------------------------------------------------------------------");
                float teGjithaSasit = 0f;
                float teGjithaCmimetNjesi = 0f;
                double teGjitheCmimetTotale = 0f;
                var liferimetArt = await App.Database.GetLiferimetArtAsync();
                var liferimiArt = liferimetArt.Where(x => x.IDLiferimi == lif.IDLiferimi);
                var tvsh = 0m;
                int reservedSpaceForEachElement = 10;
                string emptySpace;
                string sPranuar;
                string sShitur;
                string sKthyer;
                string slevizje;
                string smbetur;
                string scmimi;
                if (lif.CmimiTotal < 0) {
                    SelectedLiferimetEKryera.ListaEArtikujve = (from s in SelectedLiferimetEKryera.ListaEArtikujve
                                                                orderby s.IDArtikulli
                                                                select s).ToList();
                }
                
                foreach (var art in SelectedLiferimetEKryera.ListaEArtikujve) {
                    await _printer.printText("\n" + art.IDArtikulli + "   " + art.Emri + "   " + art.Seri);
                    reservedSpaceForEachElement = 10;
                    emptySpace = "\n                  ";
                    sPranuar = art.BUM;
                    sShitur = String.Format("{0:0.00}", art.Sasia);
                    sKthyer = String.Format("{0:0.00}", Math.Round(decimal.Parse((art.CmimiNjesi).ToString()) / 1.2m, 2));
                    slevizje = String.Format("{0:0.00}", Math.Round(decimal.Parse((art.CmimiNjesi * art.Sasia).ToString()) / 1.2m, 2));
                    smbetur = String.Format("{0:0.00}", decimal.Parse((art.CmimiNjesi * art.Sasia).ToString()) - decimal.Parse(slevizje));
                    scmimi = String.Format("{0:0.00}", art.CmimiNjesi * art.Sasia);
                    if (sPranuar.Length < (reservedSpaceForEachElement - 3)) {
                        var index = 1;
                        while (sPranuar.Length < (reservedSpaceForEachElement - 3)) {
                            if (index > 0) {
                                sPranuar = sPranuar.Insert(0, " ");
                            }
                            else
                                sPranuar += " ";
                            index *= -1;
                        }
                    }
                    if (sShitur.Length < (reservedSpaceForEachElement - 3)) {
                        var index = 1;
                        while (sShitur.Length < (reservedSpaceForEachElement - 3)) {
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
                
                    teGjithaSasit += (float)art.Sasia;
                }


                await _printer.printText("\n---------------------------------------------------------------------");
                string totalBuilder = string.Empty;
                reservedSpaceForEachElement = 15;
                emptySpace = "\n Mbyllet me total  ";
                if (teGjithaSasit < 0) {
                    if (lif.TotaliPaTVSH > 0) {
                        lif.TotaliPaTVSH = lif.TotaliPaTVSH * -1;
                    }
                    if (lif.CmimiTotal > 0) {
                        lif.CmimiTotal = lif.CmimiTotal * -1;
                    }
                }
                var tvshAll = String.Format("{0:0.00}", Math.Round((lif.CmimiTotal - lif.TotaliPaTVSH), 2));
                var cmimTotal = String.Format("{0:0.00}", lif.CmimiTotal);
                //v.patvsh
                var vptvsh = String.Format("{0:0.00}", lif.TotaliPaTVSH);                //v.patvsh
                sPranuar = String.Format("{0:0.00}", teGjithaSasit);
                sShitur = String.Format("{0:0.00}", lif.TotaliPaTVSH);
                slevizje = String.Format("{0:0.00}", Math.Round((lif.CmimiTotal - lif.TotaliPaTVSH), 2));
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


                //printText = "A. 1. عدد ۰۱۲۳۴۵۶۷۸۹" + "\nB. 2. عدد 0123456789" + "\nC. 3. به" + "\nD. 4. نه" + "\nE. 5. مراجعه" + "\n";// 
                //await _printer.printText(printText, new MPosFontAttribute() { CodePage = (int)MPosCodePage.MPOS_CODEPAGE_FARSI, Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });     // Persian 


                await _printer.printText("\n");
                await _printer.printText("\n");
                await _printer.printText("\n");
                await _printer.printText("\n");
                var agjendi = agjendet.FirstOrDefault(x => x.IDAgjenti == LoginData.IDAgjenti);
                await _printer.printText("Bleresi                                          Shitesi");
                var klientiLength = klienti.Emri.Trim();
                var agjendiLengh = agjendi.Emri + " " + agjendi.Mbiemri;
                var differenceBetweenTheTwo = 60 - agjendiLengh.Length - klientiLength.Length;
                var emptyString = String.Empty;
                for (int i = 0; i < differenceBetweenTheTwo; i++) {
                    emptyString += " ";
                }

                await _printer.printText("\n"+klienti.Emri.Trim()+ emptyString + agjendi.Emri +" " + agjendi.Mbiemri);
                await _printer.printText("\n");
                await _printer.printText("\n");
                await _printer.printText("\n___________________                              ___________________");


                await _printer.printText("\n");
                await _printer.printText("(emri,mbiemri,nensh.)                           (emri,mbiemri,nensh.)");

                await _printer.printText("\n");
                await _printer.printText("\n");
                await _printer.printText("\n");
                await _printer.printText("\n");
                await _printer.printText("\n");
                await _printer.printText("\nShtese ne nivel fature\n");
                await _printer.printText("------------------------------------------------------------------------------------------------------------------------------------------");

                await _printer.printText("\nKodi  TVSH-se    Shk. TVSH-se    Vl. tatushme      TVSH        VLERA \n");
                Debug.WriteLine("\nKodi  TVSH-se    Shk. TVSH-se    Vl. tatushme      TVSH        VLERA ");
                await _printer.printText("---------------------------------------------------------------------");
                reservedSpaceForEachElement = 15;
                emptySpace = "\n   S-VAT             20        ";
                sPranuar = String.Format("{0:0.00}", Math.Round(lif.TotaliPaTVSH, 2));
                sShitur = tvshAll;
                slevizje = cmimTotal;
                totalBuilder = String.Empty;


                if (sPranuar.Length < (reservedSpaceForEachElement - 3)) {
                    var index = 1;
                    while (sPranuar.Length <= (reservedSpaceForEachElement - 3)) {
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
                    }
                }

                emptySpace += sPranuar + sShitur + slevizje;

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



                if (string.IsNullOrEmpty(lif.TCRQRCodeLink) || string.IsNullOrEmpty(lif.TCRNSLF)) {
                    await _printer.printText("\n");

                    await _printer.printText("\n");

                    await _printer.printText("                       Dokument i pa fiskalizuar");
                }
                else {
                    await _printer.printText("\n");
                    await _printer.printText("\n");

                    await _printer.printLine(1, 1, 1, 1, 1);

                    if (lif.PayType == "BANK") {
                        await _printer.printText("\n");
                        await _printer.printText("\n");

                        await _printer.printText("                     Afati per pagese : " + MyTimeInWesternEurope.AddMonths(1).ToString("dd.MM.yyyy") + "\n");
                    }
                    await _printer.printBitmap(DependencyService.Get<IPlatformInfo>().GenerateQRCode(lif.TCRQRCodeLink),
                                300/*(int)MPosImageWidth.MPOS_IMAGE_WIDTH_ASIS*/,   // Image Width
                                (int)MPosAlignment.MPOS_ALIGNMENT_CENTER,           // Alignment
                                50,                                                 // brightness
                                true,                                               // Image Dithering
                                true);
                    await _printer.printText("\n");
                    await _printer.printText("\n");

                    var companyInfo = await App.Database.GetCompanyInfoAsync();
                    if (lif.PayType == "BANK") {
                        var smallBarcodeString = string.Empty;
                        if (!string.IsNullOrEmpty(lif.TCRNIVF)) {
                            smallBarcodeString = companyInfo.FirstOrDefault(x => x.Item == "NIPT").Value + ";" + companyInfo.FirstOrDefault(x => x.Item == "Shitesi").Value + ";" + lif.TCRNIVF + ";" + MyTimeInWesternEurope.ToString("dd.MM.yyyy HH:ss") + ";" + String.Format("{0:###0.00}", lif.CmimiTotal) + "ALL;;;";
                        }
                        else
                            smallBarcodeString = companyInfo.FirstOrDefault(x => x.Item == "NIPT").Value + ";" + companyInfo.FirstOrDefault(x => x.Item == "Shitesi").Value + ";" + lif.IDPorosia + ";" + MyTimeInWesternEurope.ToString("dd.MM.yyyy HH:ss") + ";" + String.Format("{0:###0.00}", lif.CmimiTotal) + "ALL;;;";
                        await _printer.printBitmap(DependencyService.Get<IPlatformInfo>().GenerateQRCode(smallBarcodeString),
                                    225/*(int)MPosImageWidth.MPOS_IMAGE_WIDTH_ASIS*/,   // Image Width
                                    (int)MPosAlignment.MPOS_ALIGNMENT_CENTER,           // Alignment
                                    50,                                                 // brightness
                                    true,                                               // Image Dithering
                                    true);
                        await _printer.printText("\n");

                        await _printer.printText("\n");
                        await _printer.printText("\n");
                        await _printer.printText(" ISP       AL 8820 81120 400000 3044 9935302 \n");
                        await _printer.printText("RZB        AL 2420 2111 7800 0000 0001 313616\n");
                        await _printer.printText("PCB        AL 4420 9111 0800 0010 5418 3900 01\n");
                        await _printer.printText(" BKT       AL 0820 51111 79063 36CL PRCLALLF\n");
                        await _printer.printText("\n");

                    }
                    if (!string.IsNullOrEmpty(lif.TCRQRCodeLink)) {
                        await _printer.printText("\n");
                        await _printer.printText("Te gjitha Informacionet ne lidhje me kete fature, mund te shihen ne \n kete Kod QR");
                    }
                }
                
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

        async Task OnPrintTextClickedAllFaturat() {

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

                DateTime MyTimeInWesternEurope = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "GMT Standard Time").AddHours(2);

                await _printer.printBitmap(DependencyService.Get<IPlatformInfo>().GetImgResource(),
                            100/*(int)MPosImageWidth.MPOS_IMAGE_WIDTH_ASIS*/,   // Image Width
                            (int)MPosAlignment.MPOS_ALIGNMENT_CENTER,           // Alignment
                            50,                                                 // brightness
                            true,                                               // Image Dithering
                            true);
                await _printer.printLine(1, 1, 1, 1, 1);
                await _printer.printText("\nR A P O R T I  I  S H I T J E V E \n");
                await _printer.printLine(0, 0, 1, 1, 1);
                await _printer.printText(
"---------------------------------------------------------------------\n");
                await _printer.printText("", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });
                await _printer.printText(" \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });
                await _printer.printText("\n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });
                await _printer.printText("\n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });
                var agjendet = await App.Database.GetAgjendetAsync();
                var agjendi = agjendet.FirstOrDefault(x => x.IDAgjenti == LoginData.IDAgjenti);
                await _printer.printText(agjendi.Emri + " " + agjendi.Mbiemri + "    " + MyTimeInWesternEurope.ToString("dd.MM.yyyy HH:mm:ss") + "    " + LoginData.Depo + "\n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });
                await _printer.printText("\n------------------------------------------------------------------------------------------------------------------------------------------");                


                await _printer.printText("\nNr Fatura       Klienti       Adresa          Faturim         Inkasim\n");
                Debug.WriteLine("\nNr Fatura       Klienti       Adresa          Faturim      Inkasim \n");
                await _printer.printText("---------------------------------------------------------------------");
                decimal faturimiTotal = 0m;
                decimal inkasimiTotal = 0m;
                var liferimet = await App.Database.GetLiferimetAsync();
                var indexi = 0;
                int reservedSpaceForEachElement = 10;
                string emptySpace;
                string sPranuar;
                string sShitur;
                string sKthyer;
                string slevizje;
                string smbetur;
                LiferimetEKryera = new ObservableCollection<VizualizimiFatures>(LiferimetEKryera.OrderBy(x => x.NrFisk));
                LiferimetEKryera = new ObservableCollection<VizualizimiFatures>(LiferimetEKryera.GroupBy(p => p.IDLiferimi)
  .Select(g => g.First())
  .ToList());
                foreach (var vizLif in LiferimetEKryera) {
                    indexi++;
                    var lif = liferimet.FirstOrDefault(x => x.IDLiferimi == vizLif.IDLiferimi);
                    faturimiTotal += vizLif.Totali;
                    inkasimiTotal += decimal.Parse(lif.ShumaPaguar.ToString());
                    string klienti = vizLif.Klienti;
                    if (vizLif.Klienti.Length > 11) {
                        klienti = vizLif.Klienti.Remove(11);
                    }
                    string Kontakt = vizLif.Kontakt;
                    if (Kontakt.Length > 14) {
                        Kontakt = vizLif.Kontakt.Remove(14);
                    }
                    if (klienti.Length < 11) {
                        while (klienti.Length < 11) {
                            klienti += " ";
                        }
                    }
                    reservedSpaceForEachElement = 15;
                    emptySpace = "\n";
                    sPranuar = vizLif.NrFat + "   " + vizLif.NrFisk;
                    sShitur = klienti.Trim();
                    sKthyer = Kontakt;
                    slevizje = String.Format("{0:0.00}", vizLif.Totali);
                    smbetur = String.Format("{0:0.00}", lif.ShumaPaguar);

                    if (sPranuar.Length < reservedSpaceForEachElement - 3) {
                        var index = 1;
                        while (sPranuar.Length < reservedSpaceForEachElement - 3) {
                            if (index > 0) {
                                sPranuar = sPranuar.Insert(0, " ");
                            }
                            else
                                sPranuar += " ";
                            index *= -1;
                        }
                    }
                    if (sShitur.Length < reservedSpaceForEachElement) {
                        var index = 1;
                        while (sShitur.Length < reservedSpaceForEachElement) {
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

                    if (slevizje.Length < (reservedSpaceForEachElement + 4)) {
                        var index = 1;
                        while (slevizje.Length < (reservedSpaceForEachElement)) {
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

                    emptySpace += sPranuar + sShitur + sKthyer + slevizje + smbetur;

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
                }
                


                await _printer.printText("\n---------------------------------------------------------------------");

                if(faturimiTotal.ToString().Length == 0) {
                    var printBuilder = "Nr. Total Fature: " + indexi + "                            " + String.Format("{0:0.00}", faturimiTotal) + "              " + String.Format("{0:0.00}", inkasimiTotal);
                    if (printBuilder.Length < 69) {
                        bool wasLastEmpty = false;
                        string totalTemp = printBuilder;
                        for (int i = printBuilder.Length - 1; i >= 0; i--) {
                            var de = printBuilder[i];
                            if (wasLastEmpty) {
                                if (printBuilder[i] == ' ') {
                                    wasLastEmpty = true;
                                    totalTemp = totalTemp.Remove(i, 1);
                                    if (totalTemp.Length == 70) {
                                        printBuilder += " ";
                                        break;
                                    }
                                }
                            }
                            if (printBuilder[i] == ' ') {
                                wasLastEmpty = true;
                            }
                            else {
                                wasLastEmpty = false;
                            }
                        }
                    }
                    if (printBuilder.Length > 69) {
                        bool wasLastEmpty = false;
                        string totalTemp = printBuilder;
                        for (int i = printBuilder.Length - 1; i >= 0; i--) {
                            var de = printBuilder[i];
                            if (wasLastEmpty) {
                                if (printBuilder[i] == ' ') {
                                    wasLastEmpty = true;
                                    totalTemp = totalTemp.Remove(i, 1);
                                    if (totalTemp.Length == 70) {
                                        printBuilder = totalTemp;
                                        break;
                                    }
                                }
                            }
                            if (printBuilder[i] == ' ') {
                                wasLastEmpty = true;
                            }
                            else {
                                wasLastEmpty = false;
                            }
                        }
                    }
                    await _printer.printText(printBuilder);
                    Debug.WriteLine("\n" +printBuilder + " Length : "+ printBuilder.Length);
                }
                else {
                    var printBuilder = "Nr. Total Fature: " + indexi + "                            " + String.Format("{0:0.00}", faturimiTotal) + "               " + String.Format("{0:0.00}", inkasimiTotal);
                    if (printBuilder.Length < 69) {
                        bool wasLastEmpty = false;
                        string totalTemp = printBuilder;
                        for (int i = printBuilder.Length - 1; i >= 0; i--) {
                            var de = printBuilder[i];
                            if (wasLastEmpty) {
                                if (printBuilder[i] == ' ') {
                                    wasLastEmpty = true;
                                    totalTemp = totalTemp.Remove(i, 1);
                                    if (totalTemp.Length == 69) {
                                        printBuilder += " ";
                                        break;
                                    }
                                }
                            }
                            if (printBuilder[i] == ' ') {
                                wasLastEmpty = true;
                            }
                            else {
                                wasLastEmpty = false;
                            }
                        }
                    }
                    if (printBuilder.Length > 69) {
                        bool wasLastEmpty = false;
                        string totalTemp = printBuilder;
                        for (int i = printBuilder.Length - 1; i >= 0; i--) {
                            var de = printBuilder[i];
                            if (wasLastEmpty) {
                                if (printBuilder[i] == ' ') {
                                    wasLastEmpty = true;
                                    totalTemp = totalTemp.Remove(i, 1);
                                    if (totalTemp.Length == 69) {
                                        printBuilder = totalTemp;
                                        break;
                                    }
                                }
                            }
                            if (printBuilder[i] == ' ') {
                                wasLastEmpty = true;
                            }
                            else {
                                wasLastEmpty = false;
                            }
                        }
                    }
                    await _printer.printText(printBuilder);
                    Debug.WriteLine("\n" + printBuilder + " Length : " + printBuilder.Length);
                }
                var printBuilderi = "                                              " + String.Format("{0:0.00}", faturimiTotal) + "    " + String.Format("{0:0.00}", inkasimiTotal);
                Debug.WriteLine("\n" + printBuilderi + " Length : " + printBuilderi.Length);

                await _printer.printText(" \n");
                await _printer.printText("\n");
                await _printer.printText("\n");
                await _printer.printText("Dorezoi:_______________________  Pranoi:_____________________________");
                //printText = "A. 1. عدد ۰۱۲۳۴۵۶۷۸۹" + "\nB. 2. عدد 0123456789" + "\nC. 3. به" + "\nD. 4. نه" + "\nE. 5. مراجعه" + "\n";// 
                //await _printer.printText(printText, new MPosFontAttribute() { CodePage = (int)MPosCodePage.MPOS_CODEPAGE_FARSI, Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });     // Persian 
                await _printer.printText(" \n");
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
        async Task OnPrintTextClickedMalliMbetur() {

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
                await _printer.printText("\nR A P O R T I  I  M A L L I T  T E  M B E T U R \n");
                await _printer.printLine(0, 0, 1, 1, 1);
                await _printer.printText(
"---------------------------------------------------------------------\n");

                DateTime MyTimeInWesternEurope = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "GMT Standard Time").AddHours(2);

                await _printer.printText("", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });
                await _printer.printText(" \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });
                await _printer.printText("\n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });
                await _printer.printText("\n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });
                var agjendet = await App.Database.GetAgjendetAsync();
                var agjendi = agjendet.FirstOrDefault(x => x.IDAgjenti == LoginData.IDAgjenti);
                await _printer.printText(agjendi.Emri + " " + agjendi.Mbiemri + "    " + MyTimeInWesternEurope.ToString("dd.MM.yyyy HH:mm:ss") + "    " + LoginData.Depo +"\n" ,new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });
                

                await _printer.printText("------------------------------------------------------------------------------------------------------------------------------------------");

                await _printer.printText("\nKodi  Artikulli  S.Pranuar   S.Shitur   S.Kthyer   Levizje   S.Mbetur\n");
                await _printer.printText("---------------------------------------------------------------------");
                ;
                int reservedSpaceForEachElement = 10;
                string emptySpace;
                string sPranuar;
                string sShitur;
                string sKthyer;
                string slevizje;
                string smbetur;
                MalliMbetur = new ObservableCollection<Malli_Mbetur>((from s in MalliMbetur
                                                                   orderby s.IDArtikulli
                                                                       select s).ToList());
                foreach (var art in MalliMbetur) {
                    await _printer.printText(art.IDArtikulli + "   " + art.Emri + "   " + art.Seri);
                    reservedSpaceForEachElement = 10;
                    emptySpace ="\n                     ";
                    sPranuar = String.Format("{0:0.00}", art.SasiaPranuar);
                    sShitur = String.Format("{0:0.00}", art.SasiaShitur);
                    sKthyer = String.Format("{0:0.00}", art.SasiaKthyer);
                    slevizje = String.Format("{0:0.00}", art.LevizjeStoku);
                    smbetur = String.Format("{0:0.00}", art.SasiaMbetur);

                    if (sPranuar.Length < reservedSpaceForEachElement) 
                    {
                        var index = 1;
                        while(sPranuar.Length < reservedSpaceForEachElement) {
                            if (index > 0) {
                                sPranuar = sPranuar.Insert(0, " ");
                            }
                            else
                                sPranuar += " ";
                            index *= -1;
                        }
                    }
                    if (sShitur.Length < reservedSpaceForEachElement) {
                        var index = 1;
                        while (sShitur.Length < reservedSpaceForEachElement) {
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


                    if (emptySpace.Length > 69) {
                        bool wasLastEmpty = false;
                        string totalTemp = emptySpace;
                        for (int i = emptySpace.Length - 1; i >= 0; i--) {
                            var de = emptySpace[i];
                            if (wasLastEmpty) {
                                if (emptySpace[i] == ' ') {
                                    wasLastEmpty = true;
                                    totalTemp = totalTemp.Remove(totalTemp[i], 1);
                                    if (totalTemp.Length <= 69) {
                                        emptySpace = totalTemp;
                                        break;
                                    }
                                }
                            }
                            if (emptySpace[i] == ' ') {
                                wasLastEmpty = true;
                            }
                        }
                    }
                    emptySpace += sPranuar + sShitur + sKthyer + slevizje + smbetur;
                    await _printer.printText(emptySpace);
                    Debug.WriteLine(emptySpace);
                }
                

                await _printer.printText("\n---------------------------------------------------------------------");
                reservedSpaceForEachElement = 10;
                emptySpace = "\n                     ";
                sPranuar = String.Format("{0:0.00}", PranuarAll);
                sShitur = String.Format("{0:0.00}", ShiturAll);
                sKthyer = String.Format("{0:0.00}", KthyerAll);
                slevizje = String.Format("{0:0.00}", LevizjeAll);
                smbetur = String.Format("{0:0.00}", MbetjaAll);
                if (sPranuar.Length < reservedSpaceForEachElement) {
                    var index = 1;
                    while (sPranuar.Length < reservedSpaceForEachElement) {
                        if (index > 0) {
                            sPranuar = sPranuar.Insert(0, " ");
                        }
                        else
                            sPranuar += " ";
                        index *= -1;
                    }
                }
                if (sShitur.Length < reservedSpaceForEachElement) {
                    var index = 1;
                    while (sShitur.Length < reservedSpaceForEachElement) {
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


                if (emptySpace.Length > 69) {
                    bool wasLastEmpty = false;
                    string totalTemp = emptySpace;
                    for (int i = emptySpace.Length - 1; i >= 0; i--) {
                        var de = emptySpace[i];
                        if (wasLastEmpty) {
                            if (emptySpace[i] == ' ') {
                                wasLastEmpty = true;
                                totalTemp = totalTemp.Remove(totalTemp[i], 1);
                                if (totalTemp.Length <= 69) {
                                    emptySpace = totalTemp;
                                    break;
                                }
                            }
                        }
                        if (emptySpace[i] == ' ') {
                            wasLastEmpty = true;
                        }
                    }
                }
                emptySpace += sPranuar + sShitur + sKthyer + slevizje + smbetur;
                await _printer.printText(emptySpace);

                await _printer.printText("", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });
                await _printer.printText(" \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });
                await _printer.printText("\n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });
                await _printer.printText("\n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });
                //printText = "A. 1. عدد ۰۱۲۳۴۵۶۷۸۹" + "\nB. 2. عدد 0123456789" + "\nC. 3. به" + "\nD. 4. نه" + "\nE. 5. مراجعه" + "\n";// 
                //await _printer.printText(printText, new MPosFontAttribute() { CodePage = (int)MPosCodePage.MPOS_CODEPAGE_FARSI, Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });     // Persian 


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

        public string PagesaType { get; set; }
        private async Task PerfundoLiferimin(VizualizimiFatures vizualizimiFatures,Guid idPorosia) {
            var liferimet = await App.Database.GetLiferimetAsync();
            var liferim = liferimet.FirstOrDefault(x => x.IDLiferimi == vizualizimiFatures.IDLiferimi);
            PagesaType = liferim.PayType;
            var porosite = await App.Database.GetPorositeAsync();
            var location = await Geolocation.GetLastKnownLocationAsync();

            foreach (var porosia in porosite) {
                if (porosia.IDPorosia == idPorosia) {
                    var numRows = vizualizimiFatures.ListaEArtikujve.Count;
                    List<Stoqet> StoqetPerUpdate = new List<Stoqet>();
                    var userResult = true;
                    if (userResult) {
                        var TotaliPaguar = 0;
                        if (numRows > 0) // nese ka artikuj t kthyer 
                        {
                            foreach (var artikull in vizualizimiFatures.ListaEArtikujve) {
                                decimal SasiaUpdate = Math.Round(decimal.Parse(artikull.Sasia.ToString()), 3);
                                var stoqet = await App.Database.GetAllStoqetAsync();
                                var stoku = stoqet.FirstOrDefault(x => x.Depo == LoginData.IDAgjenti && x.Seri == artikull.Seri);
                                if (stoku == null) {
                                    stoku = stoqet.FirstOrDefault(x => x.Shifra == artikull.IDArtikulli && x.Depo == LoginData.IDAgjenti);
                                    if (stoku == null) {
                                        UserDialogs.Instance.Alert("Mungon stoku, ju lutem sinkronizoni");
                                        return;
                                    }
                                    stoku.Seri = artikull.Seri;
                                }
                                decimal sasiaAktuale = Math.Round(decimal.Parse(stoku.Sasia.ToString()), 3);
                                stoku.Sasia = double.Parse(Math.Round((decimal)(sasiaAktuale - SasiaUpdate), 3).ToString());
                                await App.Database.UpdateStoqetAsync(stoku);

                                //TODO update malli i mbetur SASIA E KTHYER
                                var malliIMbetur = await App.Database.GetMalliMbeturIDAsync(artikull.Seri, LoginData.IDAgjenti,artikull.IDArtikulli);

                                decimal SasiaShiturUpdate = Math.Round(decimal.Parse(artikull.Sasia.ToString()), 3);
                                decimal SasiaKthyerAktuale = Math.Round(decimal.Parse(malliIMbetur.SasiaKthyer.ToString()), 3);

                                malliIMbetur.SasiaKthyer = float.Parse(Math.Round(SasiaShiturUpdate + SasiaKthyerAktuale, 3).ToString());

                                //(sasiaPranuar - (SasiaShitur+SasiaKthyer-LevizjeStoku))
                                var sasiaMbeturString = malliIMbetur.SasiaPranuar - (malliIMbetur.SasiaShitur + malliIMbetur.SasiaKthyer - malliIMbetur.LevizjeStoku);
                                malliIMbetur.SasiaMbetur = float.Parse(Math.Round(double.Parse(sasiaMbeturString.ToString()), 3).ToString());
                                malliIMbetur.SyncStatus = 0;
                                await App.Database.UpdateMalliMbeturAsync(malliIMbetur);

                                var companyInfo = await App.Database.GetCompanyInfoAsync();
                                var pArt = new PorosiaArt
                                {
                                    CmimiAktual = float.Parse(artikull.CmimiNjesi.ToString()),
                                    IDArtikulli = artikull.IDArtikulli,
                                    SasiaPorositur = (float)artikull.Sasia,
                                    Rabatet = 0,
                                    IDPorosia = idPorosia,
                                    SasiLiferuar = (float)artikull.Sasia,
                                    Emri = artikull.Emri,
                                    Gratis = 0,
                                    SasiaPako = artikull.SasiaPako == null ? (float)artikull.Sasia : (float)artikull.SasiaPako,
                                    DeviceID = LoginData.DeviceID,
                                    SyncStatus = 0,
                                    BUM = "KG",
                                    Seri = artikull.Seri,

                                };
                                pArt.CmimiPaTVSH = (float)(pArt.CmimiAktual / 1.2f);
                                await App.Database.SavePorosiaArtAsync(pArt);
                            }
                        }

                        NumriFisk NumriFisk = await App.Database.GetNumratFiskalIDAsync(LoginData.IDAgjenti);
                        if (NumriFisk == null) {
                            var numriFiskalAPIResult = await App.ApiClient.GetAsync("numri-fisk/" + LoginData.IDAgjenti);
                            if (numriFiskalAPIResult.IsSuccessStatusCode) {
                                var numriFiskalResponse = await numriFiskalAPIResult.Content.ReadAsStringAsync();
                                NumriFisk = JsonConvert.DeserializeObject<NumriFisk>(numriFiskalResponse);
                                await App.Database.SaveNumriFiskalAsync(NumriFisk);
                            }
                            else {
                                UserDialogs.Instance.Alert("Problem ne numrin fiskal, ju lutem provoni perseri.");
                                return;
                            }
                        }
                        int _NumriFisk = NumriFisk.IDN + 1;
                        NumriFisk.IDN = _NumriFisk;
                        await App.Database.UpdateNumriFiskalAsync(NumriFisk);
                        string _getVitiNumriFisk = NumriFisk.Viti.ToString();


                        var porosiaSelektuar = await App.Database.GetPorositeIDAsync(porosiaID: IDPorosi);
                        int _nrDetaleve = await App.Database.GetPorositeArtCountAsync(IDPorosi);
                        porosiaSelektuar.NrDetalet = numRows;
                        porosiaSelektuar.NrPorosise = porosia.NrPorosise.ToString();
                        porosia.Latitude = location?.Latitude.ToString();
                        porosia.Longitude = location?.Longitude.ToString();
                        await App.Database.UpdatePorositeAsync(porosiaSelektuar);
                        var porosiaArt = await App.Database.GetPorositaArtAsync(LoginData.IDAgjenti, -1);
                        if (porosiaArt == null) {
                            var listOfPorositeArt = await App.Database.GetPorositeArtAsync();
                            porosiaArt = listOfPorositeArt.FirstOrDefault(x => x.SasiaPorositur < 0 && x.IDArsyeja == 0);
                        }
                        await App.Database.UpdatePorositeArtAsync(porosiaArt);

                        if (!AprovimFaturash) {
                            var NumriFaturave = await App.Database.GetNumriFaturaveIDAsync(LoginData.IDAgjenti);
                            NumriFaturave.CurrNrFatJT = NumriFaturave.CurrNrFatJT + 1;
                            await App.Database.UpdateNumriFaturave(NumriFaturave);
                        }
                        else {
                            var NumriFaturave = await App.Database.GetNumriFaturaveIDAsync(LoginData.IDAgjenti);
                            NumriFaturave.CurrNrFat = NumriFaturave.CurrNrFat + 1;
                            await App.Database.UpdateNumriFaturave(NumriFaturave);
                        }
                        Guid IDLiferimi = Guid.NewGuid();
                        float totaliFaturesMeTVSH = System.Convert.ToSingle(vizualizimiFatures.Totali);
                        float TotaliFaturesPaTVH = totaliFaturesMeTVSH / 1.2f; //TODO DIVIDE BY TVSH
                        var agjendet = await App.Database.GetAgjendetAsync();
                        Agjendet agjendi = agjendet.FirstOrDefault(x => x.IDAgjenti == LoginData.IDAgjenti);
                        //TODO : FIX FISKALIZIMI KONFIGURIMET FROM API

                        var depot = await App.Database.GetDepotAsync();
                        var FiskalizimiKonfigurimet = await App.Database.GetFiskalizimiKonfigurimetAsync();
                        if (FiskalizimiKonfigurimet.Count <= 0) {
                            var fiskalizimiKonfigurimetResult = await App.ApiClient.GetAsync("fiskalizimi-konfigurimet");
                            if (fiskalizimiKonfigurimetResult.IsSuccessStatusCode) {
                                var fiskalizimiKonfigurimetResponse = await fiskalizimiKonfigurimetResult.Content.ReadAsStringAsync();
                                FiskalizimiKonfigurimet = JsonConvert.DeserializeObject<List<FiskalizimiKonfigurimet>>(fiskalizimiKonfigurimetResponse);
                                await App.Database.SaveFiskalizimiKonfigurimetAsync(FiskalizimiKonfigurimet);
                            }
                        }
                        var numriFiskal = await App.Database.GetNumratFiskalAsync();

                        if (numriFiskal.Count < 1) {
                            var numratFiskalResult = await App.ApiClient.GetAsync("numri-fisk");
                            if (numratFiskalResult.IsSuccessStatusCode) {
                                var numratFiskalResponse = await numratFiskalResult.Content.ReadAsStringAsync();
                                numriFiskal = JsonConvert.DeserializeObject<List<NumriFisk>>(numratFiskalResponse);
                                await App.Database.SaveNumratFiskalAsync(numriFiskal);
                            }
                        }
                        var query = from a in agjendet
                                    join d in depot on a.Depo equals d.Depo
                                    join fk in FiskalizimiKonfigurimet on d.TAGNR equals fk.TAGNR
                                    join nf in numriFiskal on fk.TCRCode equals nf.TCRCode into nfGroup
                                    from nf in nfGroup.DefaultIfEmpty()
                                    where a.IDAgjenti.ToLower() == agjendi.IDAgjenti.ToLower()
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

                        var nrPorosise = await App.Database.GetNumriPorosiveAsync();
                        var nrPor = nrPorosise.FirstOrDefault(x => x.TIPI == "SHITJE");
                        DateTime MyTimeInWesternEurope = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "GMT Standard Time").AddHours(2);

                        if (nrPor == null) {
                            nrPor = new NumriPorosive
                            {
                                TIPI = "SHITJE",
                                NrPorosise = 01,
                                Date = MyTimeInWesternEurope,
                            };
                            await App.Database.SaveNumriPorosiveAsync(nrPor);
                        }
                        else {
                            nrPor.NrPorosise += 1;
                        }
                        Liferimi liferimi = new Liferimi
                        {
                            IDLiferimi = IDLiferimi,
                            DataLiferuar = MyTimeInWesternEurope.Date,
                            KohaLiferuar = MyTimeInWesternEurope.ToLocalTime(),
                            TitulliLiferimit = "",
                            DataLiferimit = MyTimeInWesternEurope.Date,
                            KohaLiferimit = MyTimeInWesternEurope.ToLocalTime(),
                            IDPorosia = IDPorosi,
                            Liferuar = 1,
                            NrLiferimit = porosia.NrPorosise,
                            CmimiTotal = totaliFaturesMeTVSH,
                            DeviceID = LoginData.DeviceID,
                            SyncStatus = 0,
                            ShumaPaguar = PagesaType == "BANK" ? 0f : float.Parse(vizualizimiFatures.Totali.ToString()),
                            Aprovuar = true,
                            LLOJDOK = "KD",
                            PayType = PagesaType,
                            NrDetalet = numRows,
                            IDKlienti = vizualizimiFatures.IDKlientDheLokacion,
                            Depo = LoginData.IDAgjenti,
                            Longitude = location?.Longitude.ToString(),
                            Latitude = location?.Latitude.ToString(),
                            IDKthimi = NrFatKthim,
                            NumriFisk = _NumriFisk,
                            TCR = App.Instance.MainViewModel.Configurimi.KodiTCR,
                            TCROperatorCode = agjendi.OperatorCode,
                            TCRBusinessCode = query.FirstOrDefault().BusinessUnitCode, // TODO FIND BUSINESSUNITCODE
                            TCRIssueDateTime = MyTimeInWesternEurope.Date,
                            NrPorosis = nrPor.NrPorosise
                        };
                        liferimi.TotaliPaTVSH = float.Parse(Math.Round(liferimi.CmimiTotal / 1.2f, 2).ToString());
                        await App.Database.SaveLiferimiAsync(liferimi);
                        var porositeArtForPorosiaID = await App.Database.GetPorositeArtAsyncWithPorosiaID(IDPorosi);
                        foreach (var porosArt in porositeArtForPorosiaID) {
                            LiferimiArt liferimiArt = new LiferimiArt
                            {
                                ArtEmri = porosArt.Emri,
                                Cmimi = (float)porosArt.CmimiAktual,
                                CmimiPaTVSH = porosArt.CmimiPaTVSH,
                                DeviceID = porosArt.DeviceID,
                                Gratis = porosArt.Gratis,
                                IDArtikulli = porosArt?.IDArtikulli,
                                IDLiferimi = IDLiferimi,
                                SasiaLiferuar = porosArt.SasiLiferuar,
                                SasiaPorositur = porosArt.SasiaPorositur,
                                Seri = porosArt.Seri,
                                SyncStatus = porosArt.SyncStatus, // THIS SHOULD BE 0
                                TCRSyncStatus = 0,
                                Totali = (float)(porosArt.CmimiAktual * porosArt.SasiaPorositur),
                            };
                            liferimiArt.TotaliPaTVSH = float.Parse(Math.Round(liferimiArt.Totali / 1.2f, 2).ToString());
                            liferimiArt.VlefteTVSH = float.Parse(Math.Round(liferimiArt.Totali - liferimiArt.TotaliPaTVSH, 2).ToString());
                            var saveResult = await App.Database.SaveLiferimiArtAsync(liferimiArt);
                            if (saveResult == -1) {
                                UserDialogs.Instance.Alert("Problem ne ruajtjen e liferimit ART tek regjistro");
                            }
                        }

                        //SYNC MALLI MBETUR DIRECTLY SINCE IT DOESN'T EXIST IN SYNC ALL 
                        if (liferimi.PayType == "KESH") {
                            EvidencaPagesave evidencaPagesave = new EvidencaPagesave
                            {
                                Borxhi = 0,
                                DataPageses = MyTimeInWesternEurope,
                                DataPerPagese = MyTimeInWesternEurope,
                                DeviceID = agjendi.DeviceID,
                                ExportStatus = 0,
                                IDAgjenti = agjendi.IDAgjenti,
                                IDKlienti = SelectedLiferimetEKryera.IDKlienti,
                                NrFatures = liferimi.NumriFisk.ToString() + "/" + liferimi.KohaLiferuar.Year,
                                NrPageses = App.Instance.MainViewModel.LoginData.DeviceID + "-|-" + MyTimeInWesternEurope.ToString(),
                                PayType = liferimi.PayType,
                                ShumaPaguar = liferimi.ShumaPaguar,
                                ShumaTotale = liferimi.CmimiTotal,
                                SyncStatus = 1,
                                KMON = "LEK"
                            };
                            await App.Database.SaveEvidencaPagesaveAsync(evidencaPagesave);
                        }
                        else {
                            EvidencaPagesave evidencaPagesave = new EvidencaPagesave
                            {
                                Borxhi = liferimi.ShumaPaguar,
                                DataPageses = MyTimeInWesternEurope,
                                DataPerPagese = MyTimeInWesternEurope,
                                DeviceID = agjendi.DeviceID,
                                ExportStatus = 0,
                                IDAgjenti = agjendi.IDAgjenti,
                                IDKlienti = SelectedLiferimetEKryera.IDKlienti,
                                NrFatures = liferimi.NumriFisk.ToString() + "/" + liferimi.KohaLiferuar.Year,
                                NrPageses = App.Instance.MainViewModel.LoginData.DeviceID + "-|-" + MyTimeInWesternEurope.ToString(),
                                PayType = liferimi.PayType,
                                ShumaPaguar = 0,
                                ShumaTotale = liferimi.CmimiTotal,
                                SyncStatus = 1,
                                KMON = "LEK"
                            };
                            await App.Database.SaveEvidencaPagesaveAsync(evidencaPagesave);
                        }
                        await FiskalizoTCRInvoice(IDLiferimi.ToString());

                        var NumriFaturaveList = await App.Database.GetNumriFaturaveAsync();
                        var numriFatures = NumriFaturaveList.FirstOrDefault(x => x.KOD == agjendi.IDAgjenti);
                        numriFatures.CurrNrFat = numriFatures.CurrNrFat + 1;
                        if (numriFatures.CurrNrFat > 0) {
                            numriFatures.NRKUFIP += numriFatures.CurrNrFat;
                            numriFatures.CurrNrFat = 0;
                            await App.Database.SaveNumriFaturaveAsync(numriFatures);
                        }
                        //UPDATE EVERYTHING LOCALLY IN ORDER TO SYNC LATER
                        UserDialogs.Instance.Alert("Kthimi automatik perfundoi me sukses");
                        //SYNC MALLI MBETUR DIRECTLY SINCE IT DOESN'T EXIST IN SYNC ALL 
                        UserDialogs.Instance.HideLoading();
                        await App.Instance.PopPageAsync();
                    }
                }
            }
        }

        public async Task FiskalizoTCRInvoice(string idLiferimi) {
            var liferimet = await App.Database.GetLiferimetAsync();
            var liferimetArt = await App.Database.GetLiferimetArtAsync();
            var query = from l2 in liferimet
                        join la2 in liferimetArt on l2.IDLiferimi equals la2.IDLiferimi
                        where (l2.TCRSyncStatus == 0 || l2.TCRSyncStatus == null) &&
                              l2.DeviceID == LoginData.DeviceID &&
                              string.Equals(l2.IDLiferimi.ToString().Trim(), idLiferimi.Trim(), StringComparison.OrdinalIgnoreCase)
                        group la2 by la2.IDLiferimi into g
                        select new MapperHeader
                        {
                            IDLiferimi = g.Key.ToString(),
                            TotaliPaTVSH = g.Sum(x => Math.Round(x.TotaliPaTVSH, 2)),
                            TotaliMeTVSH = g.Sum(x => Math.Round(x.Totali, 2)),
                            TVSH = g.Sum(x => Math.Round(x.Totali - x.TotaliPaTVSH, 2))
                        };
            DateTime MyTimeInWesternEurope = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "GMT Standard Time").AddHours(2);

            List<MapperHeader> mapperHeaderList = query.ToList();
            if (mapperHeaderList.Count > 0) {
                var klientet = await App.Database.GetKlientetAsync();
                var Klientdhelokacion = await App.Database.GetKlientetDheLokacionetAsync();
                var artikujt = await App.Database.GetArtikujtAsync();
                var companyInfo = await App.Database.GetCompanyInfoAsync();
                var query2 = from l in liferimet
                             join la in liferimetArt on l.IDLiferimi equals la.IDLiferimi
                             join k in klientet on l.IDKlienti equals k.IDKlienti
                             join kl in KlientetDheLokacionet on k.IDKlienti equals kl.IDKlienti
                             join a in artikujt on la.IDArtikulli equals a.IDArtikulli
                             join ci in companyInfo on "TVSH" equals ci.Item
                             where (l.TCRSyncStatus == 0 || l.TCRSyncStatus == null) &&
                                   string.Equals(l.IDLiferimi.ToString().Trim(), idLiferimi.Trim(), StringComparison.OrdinalIgnoreCase) &&
                                   l.DeviceID == LoginData.DeviceID
                             select new MapperLines
                             {
                                 IDLiferimi = l.IDLiferimi.ToString(),
                                 NrLiferimit = l.NrLiferimit,
                                 DeviceID = l.Depo,
                                 LLOJDOK = l.LLOJDOK,
                                 IDKthimi = l.IDKthimi ?? "",
                                 OperatorCode = "OperatorCode",
                                 InvoiceSType = "Cash GJITHMON",
                                 InvNum = l.NumriFisk + "/" + DateTime.Now.Year,
                                 InvOrdNum = Convert.ToInt32(l.NumriFisk),
                                 SendDatetime = l.KohaLiferimit,
                                 IsIssuerInVAT = true,
                                 TaxFreeAmt = Math.Round(0d, 2),
                                 PaymentMethodTypesType = l.PayType,
                                 Buyer_IDNum = k.NIPT ?? "",
                                 Buyer_IDType = "idtypestype.id",
                                 Buyer_Name = k.Emri,
                                 Buyer_Address = kl.Adresa,
                                 Buyer_Country = 5,
                                 Buyer_Town = kl.EmriLokacionit,
                                 Item_N = a.Emri,
                                 Item_C = a.IDArtikulli,
                                 Item_U = a.BUM,
                                 Item_Q = la.SasiaLiferuar,
                                 Item_UPB = Math.Round(la.CmimiPaTVSH, 2),
                                 Item_UPA = Math.Round(la.Cmimi, 2),
                                 Item_PB = Math.Round(la.TotaliPaTVSH, 2),
                                 Item_PA = Math.Round(la.Totali, 2),
                                 Item_VA = Math.Round(la.Totali - la.TotaliPaTVSH, 2),
                                 Item_VR = (double)Math.Round(decimal.Parse(ci.Value.ToString()), 2),
                                 MobileRefId = "",
                                 IICRef = "",
                                 IssueDateTimeRef = MyTimeInWesternEurope,
                                 TypeRef = "CORRECTIVE|DEBIT|CREDIT",
                                 IsCorrectiveInv = false,
                                 CardNumber = "",
                                 IsReverseCharge = false
                             };

                if (mapperHeaderList.Count > 0) {
                    List<MapperLines> mapperLineList = query2.ToList();

                    List<TCRLiferimi> invoiceObject = ClassMapper.MapHeadersAndLines(mapperHeaderList, mapperLineList);
                    foreach (TCRLiferimi inv in invoiceObject) {
                        RegisterInvoiceInputRequestPCL req = new RegisterInvoiceInputRequestPCL();
                        req.Buyer = inv.Buyer;
                        req.InvoiceItems = inv.Items.ToList();

                        req.DeviceID = inv.DeviceID;
                        req.NrLiferimit = inv.NrLiferimit;
                        req.SubseqDelivTypeSType = -1;//NOINTERNET
                        req.IICRef = inv.IICRef;
                        req.InoiceType = InvoiceSTypePCL.CASH;
                        req.InoiceTypeSpecified = true;
                        req.InvOrdNum = inv.InvOrdNum.ToString();
                        req.IsCorrectiveInv = inv.IsCorrectiveInv;
                        req.IsIssuerInVAT = inv.IsIssuerInVAT;
                        req.IsReverseCharge = inv.IsReverseCharge;
                        req.IssueDateTimeRef = inv.IssueDateTimeRef;
                        req.MobileRefId = inv.InvOrdNum.ToString();
                        req.OperatorCode = LoginData.OperatorCode;
                        req.TCRCode = App.Instance.MainViewModel.Configurimi.KodiTCR;
                        req.BusinessUnitCode = Configurimi.KodiINjesiseSeBiznesit;
                        //if (inv.PaymentMethodTypesType.ToUpper() == "BANK")
                        //    req.PaymentMethodTypeSType = PaymentMethodTypeSType.CARD;
                        //else
                        //    req.PaymentMethodTypeSType = PaymentMethodTypeSType.BANKNOTE;
                        if (inv.PaymentMethodTypesType.ToUpper() == "BANK") {
                            req.PaymentMethodTypeSType = PaymentMethodTypeSTypePCL.ACCOUNT;
                            req.InoiceType = InvoiceSTypePCL.NONCASH;
                            req.InvNum = inv.InvNum;
                        }
                        else {
                            req.PaymentMethodTypeSType = PaymentMethodTypeSTypePCL.BANKNOTE;
                            req.InvNum = inv.InvNum + "/" + App.Instance.MainViewModel.Configurimi.KodiTCR;
                        }

                        req.PaymentMethodTypeSTypeSpecified = true;

                        req.PriceWithoutVAT = (decimal)inv.TotaliPaTVSH;
                        req.PriceWithoutVATSpecified = true;
                        req.PriceWithVAT = (decimal)inv.TotaliMeTVSH;
                        req.PriceWithVATSpecified = true;
                        req.SendDatetime = inv.SendDatetime;
                        req.TaxFreeAmt = (decimal)inv.TaxFreeAmt;
                        req.TaxFreeAmtSpecified = true;
                        req.TypeRef = CorrectiveInvTypeSTypePCL.CORRECTIVE;
                        req.VATAmount = (decimal)inv.TVSH;
                        req.VATAmountSpecified = true;

                        //Kthim Malli
                        if (!inv.IsShitje) {
                            req.IICRef = inv.IDKthimi;
                            req.TypeRef = CorrectiveInvTypeSTypePCL.CORRECTIVE;
                            req.TypeRefSpecified = true;
                            req.IsCorrectiveInv = true;
                        }
                        else if (!string.IsNullOrEmpty(inv.IDKthimi)) {
                            req.IICRef = inv.IDKthimi.Replace(" ", "");
                            req.TypeRef = CorrectiveInvTypeSTypePCL.CORRECTIVE;
                            req.TypeRefSpecified = true;
                            req.IsCorrectiveInv = true;
                        }
                        req.SubseqDelivTypeSType = -1; //ONLINE


                        ResultLogPCL log = App.Instance.FiskalizationService.RegisterInvoice(req);
                        if (log == null) {
                            liferimet = await App.Database.GetLiferimetAsync();
                            liferimetArt = await App.Database.GetLiferimetArtAsync();
                            var liferimiToUpdate = liferimet
                                                .FirstOrDefault(l => l.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                            if (liferimiToUpdate != null) {
                                liferimiToUpdate.TCRSyncStatus = -1;
                                liferimiToUpdate.TCRIssueDateTime = MyTimeInWesternEurope;
                                liferimiToUpdate.TCRQRCodeLink = null;
                                liferimiToUpdate.TCR = App.Instance.MainViewModel.Configurimi.KodiTCR;
                                liferimiToUpdate.TCROperatorCode = LoginData.OperatorCode;
                                liferimiToUpdate.TCRBusinessCode = liferimiToUpdate.TCRBusinessCode;
                                liferimiToUpdate.UUID = null;
                                liferimiToUpdate.EIC = null;
                                liferimiToUpdate.TCRNSLF = null;
                                liferimiToUpdate.TCRNIVF = null;
                                liferimiToUpdate.Message = "Komunikimi me service ka deshtuar shkaku pajisja nuk ka qen e konektuar me rrjet";

                                await App.Database.SaveLiferimiAsync(liferimiToUpdate);

                                var liferimiArtToUpdate = liferimetArt
                                        .Where(la => la.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                                foreach (var art in liferimiArtToUpdate) {
                                    art.TCRSyncStatus = -1;
                                    await App.Database.UpdateLiferimiArtAsync(art);
                                }
                            }
                            return;
                        }
                        if (log.Status == StatusPCL.Ok) {
                            liferimet = await App.Database.GetLiferimetAsync();
                            liferimetArt = await App.Database.GetLiferimetArtAsync();
                            var liferimiToUpdate = liferimet
                                                .FirstOrDefault(l => l.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                            if (liferimiToUpdate != null) {
                                liferimiToUpdate.TCRSyncStatus = 1;
                                liferimiToUpdate.TCRIssueDateTime = MyTimeInWesternEurope;
                                liferimiToUpdate.TCRQRCodeLink = log.QRCodeLink;
                                liferimiToUpdate.TCR = App.Instance.MainViewModel.Configurimi.KodiTCR;
                                liferimiToUpdate.TCROperatorCode = LoginData.OperatorCode;
                                liferimiToUpdate.TCRBusinessCode = liferimiToUpdate.TCRBusinessCode;
                                liferimiToUpdate.UUID = log.ResponseUUID;
                                liferimiToUpdate.EIC = log.EIC;
                                liferimiToUpdate.TCRNSLF = log.NSLF;
                                liferimiToUpdate.TCRNIVF = log.NIVF;
                                liferimiToUpdate.Message = log.Message.Replace("'", "");

                                await App.Database.SaveLiferimiAsync(liferimiToUpdate);

                                var liferimiArtToUpdate = liferimetArt
                                        .Where(la => la.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                                foreach (var art in liferimiArtToUpdate) {
                                    art.TCRSyncStatus = 1;
                                    await App.Database.UpdateLiferimiArtAsync(art);
                                }
                            }
                        }
                        else if (log.Status == StatusPCL.FaultCode) {
                            if (string.IsNullOrEmpty(log.Message)) {
                                liferimet = await App.Database.GetLiferimetAsync();
                                liferimetArt = await App.Database.GetLiferimetArtAsync();
                                var liferimiToUpdate = liferimet
                                                    .FirstOrDefault(l => l.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                                if (liferimiToUpdate != null) {
                                    liferimiToUpdate.TCRSyncStatus = -1;
                                    liferimiToUpdate.TCRIssueDateTime = MyTimeInWesternEurope;
                                    liferimiToUpdate.TCRQRCodeLink = log.QRCodeLink;
                                    liferimiToUpdate.TCR = App.Instance.MainViewModel.Configurimi.KodiTCR;
                                    liferimiToUpdate.TCROperatorCode = LoginData.OperatorCode;
                                    liferimiToUpdate.TCRBusinessCode = liferimiToUpdate.TCRBusinessCode;
                                    liferimiToUpdate.UUID = log.ResponseUUID;
                                    liferimiToUpdate.EIC = log.EIC;
                                    liferimiToUpdate.TCRNSLF = log.NSLF;
                                    liferimiToUpdate.TCRNIVF = log.NIVF;
                                    liferimiToUpdate.Message = "Fiskalizimi deshtoi, ju lutemi provoni me vone!";

                                    await App.Database.SaveLiferimiAsync(liferimiToUpdate);


                                    var liferimiArtToUpdate = liferimetArt
                                            .Where(la => la.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                                    foreach (var art in liferimiArtToUpdate) {
                                        art.TCRSyncStatus = -1;
                                        await App.Database.UpdateLiferimiArtAsync(art);
                                    }
                                }
                            }
                            else {
                                liferimet = await App.Database.GetLiferimetAsync();
                                liferimetArt = await App.Database.GetLiferimetArtAsync();
                                var liferimiToUpdate = liferimet
                                                    .FirstOrDefault(l => l.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                                if (liferimiToUpdate != null) {
                                    liferimiToUpdate.TCRSyncStatus = -1;
                                    liferimiToUpdate.TCRIssueDateTime = MyTimeInWesternEurope;
                                    liferimiToUpdate.TCRQRCodeLink = log.QRCodeLink;
                                    liferimiToUpdate.TCR = App.Instance.MainViewModel.Configurimi.KodiTCR;
                                    liferimiToUpdate.TCROperatorCode = LoginData.OperatorCode;
                                    liferimiToUpdate.TCRBusinessCode = liferimiToUpdate.TCRBusinessCode;
                                    liferimiToUpdate.UUID = log.ResponseUUID;
                                    liferimiToUpdate.EIC = log.EIC;
                                    liferimiToUpdate.TCRNSLF = log.NSLF;
                                    liferimiToUpdate.TCRNIVF = log.NIVF;
                                    liferimiToUpdate.Message = log.Message;

                                    await App.Database.SaveLiferimiAsync(liferimiToUpdate);

                                    var liferimiArtToUpdate = liferimetArt
                                            .Where(la => la.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                                    foreach (var art in liferimiArtToUpdate) {
                                        art.TCRSyncStatus = -1;
                                        await App.Database.UpdateLiferimiArtAsync(art);
                                    }
                                }
                            }
                        }
                        else if (log.Status == StatusPCL.TCRAlreadyRegistered) {
                            liferimet = await App.Database.GetLiferimetAsync();
                            liferimetArt = await App.Database.GetLiferimetArtAsync();
                            var liferimiToUpdate = liferimet
                                .Where(l => l.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                            foreach (var liferi in liferimiToUpdate) {
                                liferi.TCRSyncStatus = 4;
                                liferi.TCRIssueDateTime = MyTimeInWesternEurope;
                                liferi.TCRQRCodeLink = log.QRCodeLink;
                                liferi.TCR = App.Instance.MainViewModel.Configurimi.KodiTCR;
                                liferi.TCROperatorCode = LoginData.OperatorCode;
                                liferi.TCRBusinessCode = liferi.TCRBusinessCode;
                                liferi.UUID = log.ResponseUUID;
                                liferi.EIC = log.EIC;
                                liferi.TCRNSLF = log.NSLF;
                                liferi.TCRNIVF = log.NIVF;
                                liferi.Message = log.Message.Replace("'", "");
                                await App.Database.SaveLiferimiAsync(liferi);

                            }

                            var liferimiArtToUpdate = liferimetArt
                                .Where(la => la.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                            foreach (var art in liferimiArtToUpdate) {
                                art.TCRSyncStatus = 4;
                                await App.Database.UpdateLiferimiArtAsync(art);
                            }
                        }
                        else if (log.Status == StatusPCL.InProcess) {
                            liferimet = await App.Database.GetLiferimetAsync();
                            liferimetArt = await App.Database.GetLiferimetArtAsync();
                            var liferimiToUpdate = liferimet
                                .Where(l => l.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                            foreach (var liferi in liferimiToUpdate) {
                                liferi.TCRSyncStatus = -2;
                                liferi.TCRIssueDateTime = MyTimeInWesternEurope;
                                liferi.TCRQRCodeLink = log.QRCodeLink;
                                liferi.TCR = App.Instance.MainViewModel.Configurimi.KodiTCR;
                                liferi.TCROperatorCode = LoginData.OperatorCode;
                                liferi.TCRBusinessCode = liferi.TCRBusinessCode;
                                liferi.UUID = log.ResponseUUID;
                                liferi.EIC = log.EIC;
                                liferi.TCRNSLF = log.NSLF;
                                liferi.TCRNIVF = log.NIVF;
                                liferi.Message = log.Message.Replace("'", "");
                                await App.Database.SaveLiferimiAsync(liferi);
                            }

                            var liferimiArtToUpdate = liferimetArt
                                .Where(la => la.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                            foreach (var art in liferimiArtToUpdate) {
                                art.TCRSyncStatus = -2;
                                await App.Database.UpdateLiferimiArtAsync(art);
                            }
                        }
                        else {
                            try {
                                liferimet = await App.Database.GetLiferimetAsync();
                                liferimetArt = await App.Database.GetLiferimetArtAsync();
                                var liferimiToUpdate = liferimet
                                    .Where(l => l.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                                foreach (var lif in liferimiToUpdate) {
                                    lif.TCRSyncStatus = -3;
                                    lif.TCRIssueDateTime = MyTimeInWesternEurope;
                                    lif.TCRQRCodeLink = log.QRCodeLink;
                                    lif.TCR = App.Instance.MainViewModel.Configurimi.KodiTCR;
                                    lif.TCROperatorCode = LoginData.OperatorCode;
                                    lif.TCRBusinessCode = lif.TCRBusinessCode;
                                    lif.UUID = log.ResponseUUID;
                                    lif.EIC = log.EIC;
                                    lif.TCRNSLF = log.NSLF;
                                    lif.TCRNIVF = log.NIVF;
                                    lif.Message = log.Message.Replace("'", "");
                                    await App.Database.SaveLiferimiAsync(lif);

                                }

                                var liferimiArtToUpdate = liferimetArt
                                    .Where(la => la.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                                foreach (var art in liferimiArtToUpdate) {
                                    art.TCRSyncStatus = -1;
                                    await App.Database.UpdateLiferimiArtAsync(art);
                                }
                            }
                            catch (Exception ex) {
                                // Handle exception
                            }
                        }
                    }
                }
            }
        }
        public async Task SaveConfigurimiAsync() {
            var result = await App.Database.SaveConfigurimiAsync(Configurimi);
            if(result != -1) {
                UserDialogs.Instance.Alert("Configurimi u ruajt me sukses", "Sukses", "Ok");
            }
            else
                UserDialogs.Instance.Alert("Configurimi nuk u ruajt me sukses, ju lutem provoni perseri!", "Deshtim", "Ok");
        }

        public async Task GoToConfigurimiAsync() {
            Configurimi = await App.Database.GetConfigurimiAsync();
            if(Configurimi == null) {
                Configurimi = new Configurimi{ ID = 1};
            }
            var linqet = await App.Database.GetAllLinqetAsync();
            LinqetPerAPI = new ObservableCollection<Linqet>(linqet.Where(x => x.Tipi == "API"));
            LinqetPerFiskalizim = new ObservableCollection<Linqet>(linqet.Where(x => x.Tipi == "FISKALIZIMI"));
            foreach(var linku in LinqetPerAPI) {
                if(linku.Titulli == null) {
                    linku.Titulli = String.Empty;
                }
            }
            foreach(var linku in LinqetPerFiskalizim) {
                if(linku.Titulli == null) {
                    linku.Titulli = String.Empty;
                }
            }
            await App.Instance.PushAsyncNewPage(new ConfigurimiPage() { BindingContext = this});
        }
        public List<string> DitetEJaves = new List<string> { "E Hënë", "E Martë", "E Mërkurë", "E Enjte", "E Premte", "E Shtunë", "E Diel" };

        private string _dateEZgjedhur;
        public string DataEZgjedhur {
            get { return _dateEZgjedhur; }
            set { SetProperty(ref _dateEZgjedhur, value); }
        }
        public DateTime TodaysDate { get; set; }
        public async Task FixDataVizualizimit() {
            DataEZgjedhur = DitetEJaves.FirstOrDefault(x => x == GetDayName(TodaysDate));
            LiferimetEKryera = new ObservableCollection<VizualizimiFatures>();
            var liferimet = await App.Database.GetLiferimetAsync();
            liferimet = liferimet.Where(x => x.DataLiferimit.Day == TodaysDate.Day && x.DataLiferimit.Month == TodaysDate.Month && x.DataLiferimit.Year == TodaysDate.Year).ToList();
            var porosite = await App.Database.GetPorositeAsync();
            var porositeart = await App.Database.GetPorositeArtAsync();
            var artikujt = await App.Database.GetArtikujtAsync();
            AllLiferimetEKryeraKthimet = 0;
            AllLiferimetEKryeraCmimiTotal = 0;
            AllLiferimetEKryeraInkasimet = 0;
            foreach (var lif in liferimet) {
                var lfV = new VizualizimiFatures
                {
                    Data = lif.DataLiferimit,
                    IDKlientDheLokacion = lif.IDKlienti,
                    IDKlienti = lif.IDKlienti,
                    IDLiferimi = lif.IDLiferimi,
                    IDPorosia = lif.IDPorosia,
                    IDVizita = porosite.FirstOrDefault(x => x.IDPorosia == lif.IDPorosia).IDVizita,
                    Klienti = KlientetDheLokacionet.FirstOrDefault(x => x.IDKlienti == lif.IDKlienti).KontaktEmriMbiemri,
                    Kontakt = KlientetDheLokacionet.FirstOrDefault(x => x.IDKlienti == lif.IDKlienti).Adresa,
                    NrFat = lif.NrLiferimit,
                    Totali = decimal.Parse(lif.CmimiTotal.ToString()),
                    ListaEArtikujve = new List<Artikulli>(),
                    NrFisk = lif.NumriFisk
                };
                Debug.WriteLine("Vizita ID : " + lfV.IDVizita.ToString());
                foreach (var porArt in porositeart) {
                    if (porArt.IDPorosia.ToString() == lfV.IDPorosia.ToString()) {
                        var art = artikujt.FirstOrDefault(x => x.IDArtikulli == porArt.IDArtikulli);
                        var newArt = new Artikulli
                        {
                            IDArtikulli = art.IDArtikulli,
                            Sasia = porArt.SasiaPorositur,
                            CmimiNjesi = porArt.CmimiAktual,
                            Emri = art.Emri,
                            Shifra = art.Shifra,
                            Seri = porArt.Seri,
                            BUM = art.BUM,
                            ArsyejaEKthimit = porArt.IDArsyeja == 0 ? "KM" : "DM",
                            Barkod = art.Barkod,

                        }; lfV.ListaEArtikujve.Add(newArt);
                    }
                }
                lfV.ListaEArtikujve = lfV.ListaEArtikujve
                  .GroupBy(p => p.IDArtikulli)
                  .Select(g => g.First())
                  .ToList();
                LiferimetEKryera.Add(lfV);

                if (decimal.Parse(lif.CmimiTotal.ToString()) < 0) {
                    AllLiferimetEKryeraKthimet -= decimal.Parse(lif.CmimiTotal.ToString());
                    AllLiferimetEKryeraInkasimet -=  Math.Abs(decimal.Parse(lif.ShumaPaguar.ToString()));
                }
                else if (decimal.Parse(lif.CmimiTotal.ToString()) >= 0) {
                    AllLiferimetEKryeraCmimiTotal += decimal.Parse(lif.CmimiTotal.ToString());
                    AllLiferimetEKryeraInkasimet += decimal.Parse(lif.ShumaPaguar.ToString());
                }
                else if (decimal.Parse(lif.ShumaPaguar.ToString()) == 0) {
                    AllLiferimetEKryeraCmimiTotal += decimal.Parse(lif.ShumaPaguar.ToString());
                    AllLiferimetEKryeraInkasimet += decimal.Parse(lif.ShumaPaguar.ToString());
                }
                else {
                    AllLiferimetEKryeraCmimiTotal += decimal.Parse(lif.ShumaPaguar.ToString());
                    AllLiferimetEKryeraInkasimet += decimal.Parse(lif.ShumaPaguar.ToString());
                }
            }
            AllLiferimetEKryeraKthimet = AllLiferimetEKryeraKthimet * -1;
        }

        private decimal _allLiferimetEKryeraCmimiTotal;
        public decimal AllLiferimetEKryeraCmimiTotal {
            get {
                return _allLiferimetEKryeraCmimiTotal;
                
            }
            set {
                SetProperty(ref _allLiferimetEKryeraCmimiTotal,value);
            }
        }
        private decimal _AllLiferimetEKryeraInkasimet;
        public decimal AllLiferimetEKryeraInkasimet {
            get {
                return _AllLiferimetEKryeraInkasimet;
                
            }
            set {
                SetProperty(ref _AllLiferimetEKryeraInkasimet, value);
            }
        }
        private decimal _AllLiferimetEKryeraKthimet;
        public decimal AllLiferimetEKryeraKthimet {
            get {
                return _AllLiferimetEKryeraKthimet;
                
            }
            set {
                SetProperty(ref _AllLiferimetEKryeraKthimet, value);
            }
        }
        public async Task HapFaturatEShituraAsync() {
            AllLiferimetEKryeraKthimet = 0m;
            AllLiferimetEKryeraInkasimet = 0m;
            AllLiferimetEKryeraCmimiTotal = 0m;
            TodaysDate = DateTime.Now;
            LiferimetEKryera = new ObservableCollection<VizualizimiFatures>();
            var liferimet = await App.Database.GetLiferimetAsync();
            liferimet = liferimet.Where(x => x.DataLiferimit.Day == TodaysDate.Day && x.DataLiferimit.Month == TodaysDate.Month && x.DataLiferimit.Year == TodaysDate.Year).ToList();
            var porosite = await App.Database.GetPorositeAsync();
            var porositeart = await App.Database.GetPorositeArtAsync();
            var artikujt = await App.Database.GetArtikujtAsync();
            AllLiferimetEKryeraKthimet = 0;
            AllLiferimetEKryeraCmimiTotal = 0;
            AllLiferimetEKryeraInkasimet = 0;
            foreach (var lif in liferimet) {
                var lfV = new VizualizimiFatures
                {
                    Data = lif.DataLiferimit,
                    IDKlientDheLokacion = lif.IDKlienti,
                    IDKlienti = lif.IDKlienti,
                    IDLiferimi = lif.IDLiferimi,
                    IDPorosia = lif.IDPorosia,
                    IDVizita = porosite.FirstOrDefault(x => x.IDPorosia == lif.IDPorosia).IDVizita,
                    Klienti = KlientetDheLokacionet.FirstOrDefault(x => x.IDKlienti == lif.IDKlienti).KontaktEmriMbiemri,
                    Kontakt = KlientetDheLokacionet.FirstOrDefault(x => x.IDKlienti == lif.IDKlienti).EmriLokacionit,
                    NrFat = lif.NrLiferimit,
                    Totali = decimal.Parse(lif.CmimiTotal.ToString()),
                    ListaEArtikujve = new List<Artikulli>(),
                    NrFisk = lif.NumriFisk
                };
                foreach(var porArt in porositeart) {
                    if(porArt.IDPorosia == lfV.IDPorosia) {
                        var art = artikujt.FirstOrDefault(x => x.IDArtikulli == porArt.IDArtikulli);
                        var newArt = new Artikulli
                        {
                            IDArtikulli = art.IDArtikulli,
                            Sasia = porArt.SasiaPorositur,
                            CmimiNjesi = porArt.CmimiAktual,
                            Emri = art.Emri,
                            Shifra = art.Shifra,
                            Seri = art.Seri,
                            BUM = art.BUM,
                             ArsyejaEKthimit = porArt.IDArsyeja == 0 ? "KM" : "DM",
                              Barkod = art.Barkod,
                            
                        }; 
                        lfV.ListaEArtikujve.Add(newArt);
                    }
                }

                LiferimetEKryera.Add(lfV);

                if (decimal.Parse(lif.CmimiTotal.ToString()) < 0) {
                    AllLiferimetEKryeraKthimet -= decimal.Parse(lif.ShumaPaguar.ToString());
                    AllLiferimetEKryeraInkasimet += decimal.Parse(lif.ShumaPaguar.ToString());
                }
                else if(decimal.Parse(lif.CmimiTotal.ToString()) == 0) {
                    AllLiferimetEKryeraCmimiTotal += decimal.Parse(lif.ShumaPaguar.ToString());
                    AllLiferimetEKryeraInkasimet += decimal.Parse(lif.ShumaPaguar.ToString());
                }
                else {
                    AllLiferimetEKryeraCmimiTotal += decimal.Parse(lif.ShumaPaguar.ToString());
                    AllLiferimetEKryeraInkasimet += decimal.Parse(lif.ShumaPaguar.ToString());
                }
            }
            DataEZgjedhur = DitetEJaves.FirstOrDefault(x => x == GetDayName(TodaysDate));
            await App.Instance.MainPage.Navigation.PopPopupAsync();
            await App.Instance.PushAsyncNewPage(new FaturatEShituraPage() { BindingContext = this });
        }

        public async Task ShfaqPorosineAsync() {
            await App.Instance.PushAsyncNewPage(new ShfaqPorosinePage() { BindingContext = this });
        }
        public string GetDayName(DateTime date) {
            string _ret = string.Empty; 
            var culture = new System.Globalization.CultureInfo("sq-AL"); 
            _ret = culture.DateTimeFormat.GetDayName(date.DayOfWeek);    
            _ret = culture.TextInfo.ToTitleCase(_ret.ToLower()); 
            return _ret;
        }


        public async Task GoToShtoVizitenPageAsync() {
            foreach (var viz in VizitatFilteredByDate) {
                if (viz.IDStatusiVizites == "1") {
                    UserDialogs.Instance.Alert("Nuk mund te shtohet vizite e re pa perfunduar viziten e hapur");
                    return;
                }
            }
            var klientet = await App.Database.GetKlientetAsync();
            if (klientet != null) {
                if (klientet.Count > 0) {
                    await App.Instance.MainPage.Navigation.PopPopupAsync();
                    Clients = new ObservableCollection<Klientet>(klientet.OrderBy(x=> x.Emri));
                    await App.Instance.PushAsyncNewPage(new RegjistrimiIVizitesPage() { BindingContext = this });
                    DateTime MyTime = DateTime.UtcNow;

                    DateTime MyTimeInWesternEurope = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(MyTime, "GMT Standard Time").AddHours(2);
                    RegjistroVizitenDate = MyTimeInWesternEurope;
                }
                else {
                    UserDialogs.Instance.HideLoading();
                    await Task.Delay(20);
                    UserDialogs.Instance.Alert("Nuk ka klienta te sinkronizuar, ju lutemi sinkronizoni klientet ne fillim");
                }
            }
        }
        public AgjendiLogin al { get; set; }

        private float _CashAmountForFirstTimeOfDayRegister;

        public float CashAmountForFirstTimeOfDayRegister {
            get { return _CashAmountForFirstTimeOfDayRegister; }
            set { SetProperty(ref _CashAmountForFirstTimeOfDayRegister, value); }
        }
        public async Task LoginAsync() {
            try {
                Configurimi = await App.Database.GetConfigurimiAsync();
                DateTime MyTimeInWesternEurope = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "GMT Standard Time").AddHours(2);

                if (Configurimi != null) {
                    if (Configurimi.Serveri != null) {
                        if(!string.IsNullOrEmpty(Configurimi.Shfrytezuesi))
                        {
                            if (al.perdoruesi.ToLower() != Configurimi.Shfrytezuesi.ToLower()) {
                                UserDialogs.Instance.Alert("Paisja eshte konfiguruar per depon : " + Configurimi.Shfrytezuesi + " ju lutem kycuni me depon ne fjale");
                                return;
                            }
                        }
                        if(App.ApiClient.BaseAddress == null)
                            App.ApiClient.BaseAddress = new Uri(Configurimi.Serveri);
                       
                            App.Instance.WebServerFiskalizimiUrl = Configurimi.URLFiskalizim;
                        var depot = await App.Database.GetDepotAsync();
                        
                        if (depot.Count > 0) {
                            Depoja = depot.FirstOrDefault(x => x.Depo == Configurimi.Shfrytezuesi);
                        }
                        else {
                            if (Configurimi.Token != null) {
                                if(App.ApiClient.DefaultRequestHeaders != null && App.ApiClient.DefaultRequestHeaders.Authorization != null)
                                    App.ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Instance.MainViewModel.Configurimi.Token);
                                var depotResult = await App.ApiClient.GetAsync("depot");
                                if (depotResult.IsSuccessStatusCode) {
                                    var depotResponse = await depotResult.Content.ReadAsStringAsync();
                                    depot = JsonConvert.DeserializeObject<List<Depot>>(depotResponse);
                                    if (depot.Count > 0) {
                                        Depoja = depot.FirstOrDefault(x => x.Depo == Configurimi.Shfrytezuesi);
                                        await App.Database.ClearAllDepotAsync();
                                        await App.Database.SaveDepotAsync(depot);
                                    }
                                }
                            }
                            else {
                            }
                            
                        }
                    }
                    else {
                        UserDialogs.Instance.Alert("Ju lutem filloni me Configurimin fillimisht para se te qaseni ne aplikacion, perndryshe nuk do te mundeni te qaseni", "Verejtje", "Ok");
                        return;
                    }
                }
                else {
                    UserDialogs.Instance.Alert("Ju lutem filloni me Configurimin fillimisht para se te qaseni ne aplikacion, perndryshe nuk do te mundeni te qaseni", "Verejtje", "Ok");
                    return;
                }
                UserDialogs.Instance.ShowLoading("Duke u kycur...");
                var vizitat = await App.Database.GetVizitatAsync();
                if(vizitat != null) {
                    if (vizitat.Count > 0) 
                    {
                        foreach(var viz in vizitat) {
                            if(viz.DataPlanifikimit.Value.AddDays(8) <=  DateTime.Now) {
                                await App.Database.DeleteVizita(viz);
                            }
                        }
                    }
                }
                if(!App.Instance.DoIHaveInternetNoAlert()) { 
                    if(string.IsNullOrEmpty(Configurimi.Token)) {
                        App.Instance.DoIHaveInternet();
                        UserDialogs.Instance.HideLoading();
                        return;
                    }
                    else {
                        var agjendet = await App.Database.GetAgjendetAsync();
                        LoginData = agjendet.FirstOrDefault(x => x.DeviceID == Configurimi.Paisja);
                        var CashRegisters = await App.Database.GetCashRegisterAsync();
                        EshteRuajtuarArka = false;

                        if (CashRegisters.Count > 0) {
                            var cReg = CashRegisters.FirstOrDefault(x => x.DepositType == 0 && x.RegisterDate.Date == DateTime.Now.Date && x.DeviceID == LoginData.DeviceID);
                            if (cReg != null) {
                                if(cReg.TCRCode == Configurimi.KodiTCR) {
                                    EshteRuajtuarArka = true;
                                    CashRegister = cReg;
                                }
                                else {
                                    UserDialogs.Instance.HideLoading();
                                    CashRegister = new CashRegister
                                    {
                                        ID = Guid.NewGuid(),
                                        Cashamount = 0,
                                        DepositType = 0,
                                        DeviceID = LoginData.DeviceID,
                                        Message = "Modifikuar manualisht, pa fiskalizuar",
                                        RegisterDate = MyTimeInWesternEurope,
                                        SyncStatus = 0,
                                        TCRCode = Configurimi.KodiTCR,
                                        TCRSyncStatus = 0,
                                    };
                                    var liferimet = await App.Database.GetLiferimetAsync();
                                    var malliMbetur = await App.Database.GetMalliMbeturAsync();
                                    if (malliMbetur.Count > 0) {
                                        CashAmountForFirstTimeOfDayRegister = liferimet
                                            .Where(l => l.PayType == "KESH")
                                            .Sum(l => l.ShumaPaguar);
                                    }
                                    CashRegister.Cashamount = decimal.Parse(CashAmountForFirstTimeOfDayRegister.ToString());
                                    await App.Instance.MainPage.Navigation.PushPopupAsync(new RegjistroArkenPopup() { BindingContext = this }, true);
                                }
                            }
                            else {
                                UserDialogs.Instance.HideLoading();
                                CashRegister = new CashRegister
                                {
                                    ID = Guid.NewGuid(),
                                    Cashamount = 0,
                                    DepositType = 0,
                                    DeviceID = LoginData.DeviceID,
                                    Message = "Modifikuar manualisht, pa fiskalizuar",
                                    RegisterDate = MyTimeInWesternEurope,
                                    SyncStatus = 0,
                                    TCRCode = Configurimi.KodiTCR,
                                    TCRSyncStatus = 0,
                                };
                                var liferimet = await App.Database.GetLiferimetAsync();
                                var malliMbetur = await App.Database.GetMalliMbeturAsync();
                                if(malliMbetur.Count > 0) {
                                    CashAmountForFirstTimeOfDayRegister = liferimet
                                        .Where(l => l.PayType == "KESH")
                                        .Sum(l => l.ShumaPaguar);
                                }

                                CashRegister.Cashamount = decimal.Parse(CashAmountForFirstTimeOfDayRegister.ToString());
                                await App.Instance.MainPage.Navigation.PushPopupAsync(new RegjistroArkenPopup() { BindingContext = this }, true);
                            }
                        }
                        else {
                            UserDialogs.Instance.HideLoading();
                            CashRegister = new CashRegister
                            {
                                ID = Guid.NewGuid(),
                                Cashamount = 0,
                                DepositType = 0,
                                DeviceID = LoginData.DeviceID,
                                Message = "Modifikuar manualisht, pa fiskalizuar",
                                RegisterDate = MyTimeInWesternEurope,
                                SyncStatus = 0,
                                TCRCode = Configurimi.KodiTCR,
                                TCRSyncStatus = 0,
                            };
                            var liferimet = await App.Database.GetLiferimetAsync();
                            var malliMbetur = await App.Database.GetMalliMbeturAsync();
                            if (malliMbetur.Count > 0) {
                                CashAmountForFirstTimeOfDayRegister = liferimet
                                    .Where(l => l.PayType == "KESH")
                                    .Sum(l => l.ShumaPaguar);
                            }
                            CashRegister.Cashamount = decimal.Parse(CashAmountForFirstTimeOfDayRegister.ToString());
                            await App.Instance.MainPage.Navigation.PushPopupAsync(new RegjistroArkenPopup() { BindingContext = this }, true);
                        }
                        while (!EshteRuajtuarArka) {
                            await Task.Delay(2000);
                        }
                        if (EshteRuajtuarArka) {

                        }

                        await App.Database.SaveCashRegisterAsync(CashRegister);



                        
                        if(CashRegister.TCRSyncStatus <= 0) {
                            var result = App.Instance.FiskalizationService.RegisterCashDeposit(new DependencyInjections.FiskalizationExtraModels.RegisterCashDepositInputRequestPCL
                            {
                                CashAmount = CashRegister.Cashamount,
                                TCRCode = CashRegister.TCRCode,
                                DepositType = DependencyInjections.FiskalizationExtraModels.CashDepositOperationSTypePCL.INITIAL,
                                OperatorCode = LoginData.OperatorCode,
                                SendDateTime = CashRegister.RegisterDate,
                                SubseqDelivTypeSType = -1
                            });
                            if (result.Status == StatusPCL.Ok) {
                                CashRegister.TCRSyncStatus = 1;
                                if (result.Message != null)
                                    CashRegister.Message = result.Message.Replace("'", "");
                            }
                            else if (result.Status == StatusPCL.TCRAlreadyRegistered) {
                                CashRegister.TCRSyncStatus = 4;
                                CashRegister.Message = result.Message.Replace("'", "");
                            }
                            else {
                                CashRegister.TCRSyncStatus = 999;
                                CashRegister.Message = result.Message.Replace("'", "");
                            }
                            await App.Database.SaveCashRegisterAsync(CashRegister);
                        }
                        MainPage mainPage = new MainPage();
                        mainPage.BindingContext = this;
                        NavigationPage navigationPage = new NavigationPage(mainPage) { BarBackgroundColor = Color.LightBlue };
                        UserDialogs.Instance.HideLoading();

                        App.Instance.MainPage = navigationPage;
                    }
                }

                if(al.idagjenti == string.Empty) {
                    al.idagjenti = " ";
                }
                if (!string.IsNullOrEmpty(al.perdoruesi) && !string.IsNullOrEmpty(al.idagjenti)) {
                    al.idagjenti = string.Empty;
                    var jsonRequest = JsonConvert.SerializeObject(al);

                    var stringContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                    var response = await App.ApiClient.PostAsync(App.ApiClient.BaseAddress + "agjendi/login", stringContent);
                    if (response.IsSuccessStatusCode) {

                    }
                    else {
                        UserDialogs.Instance.Alert("Probleme me server, ju lutem provoni me vone !", "Error", "Ok");
                        UserDialogs.Instance.HideLoading();
                        return;

                    }
                    var responseString = await response.Content.ReadAsStringAsync();
                    LoginData = JsonConvert.DeserializeObject<Agjendet>(responseString);
                    Debug.WriteLine("Username " + LoginData.Emri);
                    Debug.WriteLine("Token  " + LoginData.token);

                    if (LoginData.IDAgjenti != null && LoginData.token != null) {
                        App.ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LoginData.token);
                        App.ApiClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                        if(string.IsNullOrEmpty(Configurimi.KodiTCR)) {
                            var sync = await SinkronizoFiskalizimin();
                            if (!sync) {
                                UserDialogs.Instance.Alert("Problem me sinkronizimin e fiskalizimit, ju lutemi provoni me vone!");
                                UserDialogs.Instance.HideLoading();
                                return;
                            }
                        }

                        Configurimi.Token = LoginData.token;
                        Configurimi.Shfrytezuesi = LoginData.IDAgjenti;
                        Configurimi.Paisja = LoginData.DeviceID;
                        //REGISTER ARKEN
                        var depot = await App.Database.GetDepotAsync();
                        if (depot.Count > 0) {
                            var depotResult = await App.ApiClient.GetAsync("depot");
                            if (depotResult.IsSuccessStatusCode) {
                                var depotResponse = await depotResult.Content.ReadAsStringAsync();
                                depot = JsonConvert.DeserializeObject<List<Depot>>(depotResponse);
                                if (depot.Count > 0) {
                                    Depoja = depot.FirstOrDefault(x => x.Depo == Configurimi.Shfrytezuesi);
                                    await App.Database.ClearAllDepotAsync();
                                    await App.Database.SaveDepotAsync(depot);
                                }
                            }
                            Depoja = depot.FirstOrDefault(x => x.Depo == Configurimi.Shfrytezuesi);
                            if (string.IsNullOrEmpty(Configurimi.TAGNR))
                                Configurimi.TAGNR = Depoja.TAGNR;
                            if (Configurimi.TAGNR != Depoja.TAGNR) {
                                var sync = await SinkronizoFiskalizimin();
                                if (!sync) {
                                    UserDialogs.Instance.Alert("Problem me sinkronizimin e fiskalizimit, ju lutemi provoni me vone!");
                                    UserDialogs.Instance.HideLoading();
                                    return;
                                }
                            }
                            else
                                Configurimi.TAGNR = Depoja.TAGNR;
                        }
                        else {
                            if (Configurimi.Token != null) {
                                App.ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Instance.MainViewModel.Configurimi.Token);
                                var depotResult = await App.ApiClient.GetAsync("depot");
                                if (depotResult.IsSuccessStatusCode) {
                                    var depotResponse = await depotResult.Content.ReadAsStringAsync();
                                    depot = JsonConvert.DeserializeObject<List<Depot>>(depotResponse);
                                    if (depot.Count > 0) {
                                        Depoja = depot.FirstOrDefault(x => x.Depo == Configurimi.Shfrytezuesi);
                                        await App.Database.ClearAllDepotAsync();
                                        await App.Database.SaveDepotAsync(depot);
                                    }
                                }
                            }
                            else {

                            }

                        }
                        await App.Database.SaveConfigurimiAsync(Configurimi);
                        UserDialogs.Instance.HideLoading();
                        var CashRegisters = await App.Database.GetCashRegisterAsync();
                        EshteRuajtuarArka = false;
                        if (CashRegisters.Count > 0) {
                            var cReg = CashRegisters.FirstOrDefault(x => x.DepositType == 0 && x.RegisterDate.Date == DateTime.Now.Date && x.DeviceID == LoginData.DeviceID && x.TCRCode == Configurimi.KodiTCR);
                            if (cReg != null) {
                                if (cReg.TCRCode == Configurimi.KodiTCR) {
                                    EshteRuajtuarArka = true;
                                    CashRegister = cReg;
                                    CashRegister.TCRCode = Configurimi.KodiTCR;
                                }
                                else {
                                    UserDialogs.Instance.HideLoading();
                                    CashRegister = new CashRegister
                                    {
                                        ID = Guid.NewGuid(),
                                        Cashamount = 0,
                                        DepositType = 0,
                                        DeviceID = LoginData.DeviceID,
                                        Message = "Modifikuar manualisht, pa fiskalizuar",
                                        RegisterDate = MyTimeInWesternEurope,
                                        SyncStatus = 0,
                                        TCRCode = Configurimi.KodiTCR,
                                        TCRSyncStatus = 0,
                                    };
                                    var liferimet = await App.Database.GetLiferimetAsync();
                                    var malliMbetur = await App.Database.GetMalliMbeturAsync();
                                    if (malliMbetur.Count > 0) {
                                        CashAmountForFirstTimeOfDayRegister = liferimet
                                            .Where(l => l.PayType == "KESH")
                                            .Sum(l => l.ShumaPaguar);
                                    }
                                    CashRegister.Cashamount = decimal.Parse(CashAmountForFirstTimeOfDayRegister.ToString());
                                    await App.Instance.MainPage.Navigation.PushPopupAsync(new RegjistroArkenPopup() { BindingContext = this }, true);
                                }
                            }
                            else {
                                UserDialogs.Instance.HideLoading();
                                CashRegister = new CashRegister
                                {
                                    ID = Guid.NewGuid(),
                                    Cashamount = 0,
                                    DepositType = 0,
                                    DeviceID = LoginData.DeviceID,
                                    Message = "Modifikuar manualisht, pa fiskalizuar",
                                    RegisterDate = MyTimeInWesternEurope,
                                    SyncStatus = 0,
                                    TCRCode = Configurimi.KodiTCR,
                                    TCRSyncStatus = 0,
                                };
                                var liferimet = await App.Database.GetLiferimetAsync();
                                var malliMbetur = await App.Database.GetMalliMbeturAsync();
                                if (malliMbetur.Count > 0) {
                                    CashAmountForFirstTimeOfDayRegister = liferimet
                                        .Where(l => l.PayType == "KESH")
                                        .Sum(l => l.ShumaPaguar);
                                }
                                CashRegister.Cashamount = decimal.Parse(CashAmountForFirstTimeOfDayRegister.ToString());

                                await App.Instance.MainPage.Navigation.PushPopupAsync(new RegjistroArkenPopup() { BindingContext = this }, true);
                            }
                        }
                        else {
                            UserDialogs.Instance.HideLoading();
                            CashRegister = new CashRegister
                            {
                                ID = Guid.NewGuid(),
                                Cashamount = 0,
                                DepositType = 0,
                                DeviceID = LoginData.DeviceID,
                                Message = "Modifikuar manualisht, pa fiskalizuar",
                                RegisterDate = MyTimeInWesternEurope,
                                SyncStatus = 0,
                                TCRCode = Configurimi.KodiTCR,
                                TCRSyncStatus = 0,
                            };
                            var liferimet = await App.Database.GetLiferimetAsync();
                            var malliMbetur = await App.Database.GetMalliMbeturAsync();
                            if (malliMbetur.Count > 0) {
                                CashAmountForFirstTimeOfDayRegister = liferimet
                                    .Where(l => l.PayType == "KESH")
                                    .Sum(l => l.ShumaPaguar);
                            }
                            CashRegister.Cashamount = decimal.Parse(CashAmountForFirstTimeOfDayRegister.ToString());
                            await App.Instance.MainPage.Navigation.PushPopupAsync(new RegjistroArkenPopup() { BindingContext = this }, true);
                        }
                        while (!EshteRuajtuarArka) {
                            await Task.Delay(2000);
                        }
                        if (EshteRuajtuarArka) {

                        }

                        await App.Database.SaveCashRegisterAsync(CashRegister);

                        UserDialogs.Instance.ShowLoading("Duke u kycur...");

                        
                        if(CashRegister.TCRSyncStatus <= 0) {
                            var result = App.Instance.FiskalizationService.RegisterCashDeposit(new DependencyInjections.FiskalizationExtraModels.RegisterCashDepositInputRequestPCL
                            {
                                CashAmount = CashRegister.Cashamount,
                                TCRCode = CashRegister.TCRCode,
                                DepositType = DependencyInjections.FiskalizationExtraModels.CashDepositOperationSTypePCL.INITIAL,
                                OperatorCode = LoginData.OperatorCode,
                                SendDateTime = CashRegister.RegisterDate,
                                SubseqDelivTypeSType = -1
                            });

                            if (result.Status == StatusPCL.Ok) {
                                CashRegister.TCRSyncStatus = 1;
                                if (result.Message != null)
                                    CashRegister.Message = result.Message.Replace("'", "");
                            }
                            else if (result.Status == StatusPCL.TCRAlreadyRegistered) {
                                CashRegister.TCRSyncStatus = 4;
                                CashRegister.Message = result.Message.Replace("'", "");
                            }
                            else {
                                CashRegister.TCRSyncStatus = -1;
                                CashRegister.Message = result.Message.Replace("'", "");
                            }

                            await App.Database.SaveCashRegisterAsync(CashRegister);
                        }
                        MainPage mainPage = new MainPage();
                        mainPage.BindingContext = this;
                        NavigationPage navigationPage = new NavigationPage(mainPage) { BarBackgroundColor = Color.LightBlue };

                        App.Instance.MainPage = navigationPage;
                        await Task.Delay(100);
                        UserDialogs.Instance.HideLoading();

                    }
                    else {
                        UserDialogs.Instance.HideLoading();
                        await Task.Delay(200);
                        UserDialogs.Instance.Alert("Username ose password gabim", "Error", "Provo Perseri");
                    }
                }
                else {
                    UserDialogs.Instance.Alert("Ju lutemi mbushni fushat para se te kyceni");
                    UserDialogs.Instance.HideLoading();
                }
            }catch(Exception e) {
                if (e is UriFormatException ufe) {
                    UserDialogs.Instance.Alert("Linku I API't eshte gabim, ju lutemi rregullojeni linkun tek konfigurimi");
                    UserDialogs.Instance.HideLoading();
                }
                else if (e.Message== "This instance has already started one or more requests. Properties can only be modified before sending the first request.") {
                    App.ApiClient = new HttpClient();
                    LoginAsync();
                }
            }
            
        }

        public async Task<bool> LoginAsyncWithoutPageChange(string username) {
            try {
                Configurimi = await App.Database.GetConfigurimiAsync();
                DateTime MyTimeInWesternEurope = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "GMT Standard Time").AddHours(2);

                var vizitat = await App.Database.GetVizitatAsync();
                if (vizitat != null) {
                    if (vizitat.Count > 0) {
                        foreach (var viz in vizitat) {
                            if (viz.DataPlanifikimit.Value.AddDays(8) <= DateTime.Now) {
                                await App.Database.DeleteVizita(viz);
                            }
                        }
                    }
                }
                if (al.idagjenti == string.Empty) {
                    al.idagjenti = " ";
                }
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(al.idagjenti)) {
                    al.idagjenti = string.Empty;
                    var jsonRequest = JsonConvert.SerializeObject(al);

                    var stringContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                    var response = await App.ApiClient.PostAsync(App.ApiClient.BaseAddress + "agjendi/login", stringContent);
                    if (response.IsSuccessStatusCode) {

                    }
                    else {
                        UserDialogs.Instance.Alert("Probleme me server, ju lutem provoni me vone !", "Error", "Ok");
                        UserDialogs.Instance.HideLoading();
                        return false;

                    }
                    var responseString = await response.Content.ReadAsStringAsync();
                    LoginData = JsonConvert.DeserializeObject<Agjendet>(responseString);
                    Debug.WriteLine("Username " + LoginData.Emri);
                    Debug.WriteLine("Token  " + LoginData.token);

                    if (LoginData.IDAgjenti != null && LoginData.token != null) {
                        App.ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LoginData.token);
                        App.ApiClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                        if (string.IsNullOrEmpty(Configurimi.KodiTCR)) {
                            var sync = await SinkronizoFiskaliziminWithoutLogin();
                            if (!sync) {
                                UserDialogs.Instance.Alert("Problem me sinkronizimin e fiskalizimit, ju lutemi provoni me vone!");
                                UserDialogs.Instance.HideLoading();
                                return false;
                            }
                        }

                        Configurimi.Token = LoginData.token;
                        Configurimi.Shfrytezuesi = LoginData.IDAgjenti;
                        Configurimi.Paisja = LoginData.DeviceID;
                        //REGISTER ARKEN
                        var depot = await App.Database.GetDepotAsync();
                        if (depot.Count > 0) {
                            var depotResult = await App.ApiClient.GetAsync("depot");
                            if (depotResult.IsSuccessStatusCode) {
                                var depotResponse = await depotResult.Content.ReadAsStringAsync();
                                depot = JsonConvert.DeserializeObject<List<Depot>>(depotResponse);
                                if (depot.Count > 0) {
                                    Depoja = depot.FirstOrDefault(x => x.Depo == Configurimi.Shfrytezuesi);
                                    await App.Database.ClearAllDepotAsync();
                                    await App.Database.SaveDepotAsync(depot);
                                }
                            }
                            Depoja = depot.FirstOrDefault(x => x.Depo == Configurimi.Shfrytezuesi);
                            if (string.IsNullOrEmpty(Configurimi.TAGNR))
                                Configurimi.TAGNR = Depoja.TAGNR;
                            if (Configurimi.TAGNR != Depoja.TAGNR) {
                                var sync = await SinkronizoFiskalizimin();
                                if (!sync) {
                                    UserDialogs.Instance.Alert("Problem me sinkronizimin e fiskalizimit, ju lutemi provoni me vone!");
                                    UserDialogs.Instance.HideLoading();
                                    return false;
                                }
                            }
                            else {
                                var sync = await SinkronizoFiskaliziminWithoutLogin();
                                if (!sync) {
                                    UserDialogs.Instance.Alert("Problem me sinkronizimin e fiskalizimit, ju lutemi provoni me vone!");
                                    UserDialogs.Instance.HideLoading();
                                    return false;
                                }
                            }

                        }
                        else {
                            if (Configurimi.Token != null) {
                                App.ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Instance.MainViewModel.Configurimi.Token);
                                var depotResult = await App.ApiClient.GetAsync("depot");
                                if (depotResult.IsSuccessStatusCode) {
                                    var depotResponse = await depotResult.Content.ReadAsStringAsync();
                                    depot = JsonConvert.DeserializeObject<List<Depot>>(depotResponse);
                                    if (depot.Count > 0) {
                                        Depoja = depot.FirstOrDefault(x => x.Depo == Configurimi.Shfrytezuesi);
                                        await App.Database.ClearAllDepotAsync();
                                        await App.Database.SaveDepotAsync(depot);
                                    }
                                }
                            }
                            else {

                            }

                        }
                        await App.Database.SaveConfigurimiAsync(Configurimi);
                        UserDialogs.Instance.HideLoading();
                        var CashRegisters = await App.Database.GetCashRegisterAsync();
                        EshteRuajtuarArka = false;
                        if (CashRegisters.Count > 0) {
                            var cReg = CashRegisters.FirstOrDefault(x => x.DepositType == 0 && x.RegisterDate.Date == DateTime.Now.Date && x.DeviceID == LoginData.DeviceID && x.TCRCode == Configurimi.KodiTCR);
                            if (cReg != null) {
                                if (cReg.TCRCode == Configurimi.KodiTCR) {
                                    EshteRuajtuarArka = true;
                                    CashRegister = cReg;
                                    CashRegister.TCRCode = Configurimi.KodiTCR;
                                }
                                else {
                                    UserDialogs.Instance.HideLoading();
                                    CashRegister = new CashRegister
                                    {
                                        ID = Guid.NewGuid(),
                                        Cashamount = 0,
                                        DepositType = 0,
                                        DeviceID = LoginData.DeviceID,
                                        Message = "Modifikuar manualisht, pa fiskalizuar",
                                        RegisterDate = MyTimeInWesternEurope,
                                        SyncStatus = 0,
                                        TCRCode = Configurimi.KodiTCR,
                                        TCRSyncStatus = 0,
                                    };
                                    var liferimet = await App.Database.GetLiferimetAsync();
                                    var malliMbetur = await App.Database.GetMalliMbeturAsync();
                                    if (malliMbetur.Count > 0) {
                                        CashAmountForFirstTimeOfDayRegister = liferimet
                                            .Where(l => l.PayType == "KESH")
                                            .Sum(l => l.ShumaPaguar);
                                    }
                                    CashRegister.Cashamount = decimal.Parse(CashAmountForFirstTimeOfDayRegister.ToString());
                                    await App.Instance.MainPage.Navigation.PushPopupAsync(new RegjistroArkenPopup() { BindingContext = this }, true);
                                }
                            }
                            else {
                                UserDialogs.Instance.HideLoading();
                                CashRegister = new CashRegister
                                {
                                    ID = Guid.NewGuid(),
                                    Cashamount = 0,
                                    DepositType = 0,
                                    DeviceID = LoginData.DeviceID,
                                    Message = "Modifikuar manualisht, pa fiskalizuar",
                                    RegisterDate = MyTimeInWesternEurope,
                                    SyncStatus = 0,
                                    TCRCode = Configurimi.KodiTCR,
                                    TCRSyncStatus = 0,
                                };
                                var liferimet = await App.Database.GetLiferimetAsync();
                                var malliMbetur = await App.Database.GetMalliMbeturAsync();
                                if (malliMbetur.Count > 0) {
                                    CashAmountForFirstTimeOfDayRegister = liferimet
                                        .Where(l => l.PayType == "KESH")
                                        .Sum(l => l.ShumaPaguar);
                                }
                                CashRegister.Cashamount = decimal.Parse(CashAmountForFirstTimeOfDayRegister.ToString());

                                await App.Instance.MainPage.Navigation.PushPopupAsync(new RegjistroArkenPopup() { BindingContext = this }, true);
                            }
                        }
                        else {
                            UserDialogs.Instance.HideLoading();
                            CashRegister = new CashRegister
                            {
                                ID = Guid.NewGuid(),
                                Cashamount = 0,
                                DepositType = 0,
                                DeviceID = LoginData.DeviceID,
                                Message = "Modifikuar manualisht, pa fiskalizuar",
                                RegisterDate = MyTimeInWesternEurope,
                                SyncStatus = 0,
                                TCRCode = Configurimi.KodiTCR,
                                TCRSyncStatus = 0,
                            };
                            var liferimet = await App.Database.GetLiferimetAsync();
                            var malliMbetur = await App.Database.GetMalliMbeturAsync();
                            if (malliMbetur.Count > 0) {
                                CashAmountForFirstTimeOfDayRegister = liferimet
                                    .Where(l => l.PayType == "KESH")
                                    .Sum(l => l.ShumaPaguar);
                            }
                            CashRegister.Cashamount = decimal.Parse(CashAmountForFirstTimeOfDayRegister.ToString());
                            await App.Instance.MainPage.Navigation.PushPopupAsync(new RegjistroArkenPopup() { BindingContext = this }, true);
                        }
                        while (!EshteRuajtuarArka) {
                            await Task.Delay(2000);
                        }
                        if (EshteRuajtuarArka) {

                        }

                        await App.Database.SaveCashRegisterAsync(CashRegister);

                        UserDialogs.Instance.ShowLoading("Duke u kycur...");


                        if (CashRegister.TCRSyncStatus <= 0) {
                            var result = App.Instance.FiskalizationService.RegisterCashDeposit(new DependencyInjections.FiskalizationExtraModels.RegisterCashDepositInputRequestPCL
                            {
                                CashAmount = CashRegister.Cashamount,
                                TCRCode = CashRegister.TCRCode,
                                DepositType = DependencyInjections.FiskalizationExtraModels.CashDepositOperationSTypePCL.INITIAL,
                                OperatorCode = LoginData.OperatorCode,
                                SendDateTime = CashRegister.RegisterDate,
                                SubseqDelivTypeSType = -1
                            });

                            if (result.Status == StatusPCL.Ok) {
                                CashRegister.TCRSyncStatus = 1;
                                if (result.Message != null)
                                    CashRegister.Message = result.Message.Replace("'", "");
                            }
                            else if (result.Status == StatusPCL.TCRAlreadyRegistered) {
                                CashRegister.TCRSyncStatus = 4;
                                CashRegister.Message = result.Message.Replace("'", "");
                            }
                            else {
                                CashRegister.TCRSyncStatus = -1;
                                CashRegister.Message = result.Message.Replace("'", "");
                            }

                            await App.Database.SaveCashRegisterAsync(CashRegister);
                            return true;
                        }
                    }
                    else {
                        UserDialogs.Instance.HideLoading();
                        await Task.Delay(200);
                        UserDialogs.Instance.Alert("Username ose password gabim", "Error", "Provo Perseri");
                        return false;

                    }
                }
                else {
                    UserDialogs.Instance.Alert("Ju lutemi mbushni fushat para se te kyceni");
                    UserDialogs.Instance.HideLoading();
                    return false;
                }
            }
            catch (Exception e) {
                if (e is UriFormatException ufe) {
                    UserDialogs.Instance.Alert("Linku I API't eshte gabim, ju lutemi rregullojeni linkun tek konfigurimi");
                    UserDialogs.Instance.HideLoading();
                    return false;
                }
            }
            return true;
        }
        public List<Depot> Depot { get; set; }
        public List<Agjendet> Agjendet { get; set; }
        public List<NumriFisk> NumratFiskal { get; set; }


        private ObservableCollection<NumriFaturave> _NumratFaturaveDevMode;

        public ObservableCollection<NumriFaturave> NumratFaturaveDevMode { get { return _NumratFaturaveDevMode; } set { SetProperty(ref _NumratFaturaveDevMode, value); } }

        private ObservableCollection<NumriFisk> _numratFiskalDevMode;

        public ObservableCollection<NumriFisk> NumratFiskalDevMode { get { return _numratFiskalDevMode; } set { SetProperty(ref _numratFiskalDevMode, value); } }

        private ObservableCollection<Liferimi> _LiferimetDevMode;

        public ObservableCollection<Liferimi> LiferimetDevMode { get { return _LiferimetDevMode; } set { SetProperty(ref _LiferimetDevMode, value); } }
         private ObservableCollection<LiferimiArt> _LiferimetArtDevMode;

        public ObservableCollection<LiferimiArt> LiferimetArtDevMode { get { return _LiferimetArtDevMode; } set { SetProperty(ref _LiferimetArtDevMode, value); } }

        public List<FiskalizimiKonfigurimet> FiskalizimiKonfigurimet { get; set; }
        public async Task<string[]> GetConfigurationByIDAgjentiAsync() {
            string[] result = new string[7];
            var depotResult = await App.ApiClient.GetAsync("depot");
            if (depotResult.IsSuccessStatusCode) {
                var depotResponse = await depotResult.Content.ReadAsStringAsync();
                Depot = JsonConvert.DeserializeObject<List<Depot>>(depotResponse);
            }
            if (Agjendet == null) {
                Agjendet = new List<Agjendet> { LoginData };
            }
            
            var numratFiskalResult = await App.ApiClient.GetAsync("numri-fisk");
            var numratFiskalResponse = await numratFiskalResult.Content.ReadAsStringAsync();
            if (numratFiskalResult.IsSuccessStatusCode) {
                var numratFiskal = JsonConvert.DeserializeObject<List<NumriFisk>>(numratFiskalResponse);
                NumratFiskal = numratFiskal;
                await App.Database.ClearAllNumratFiskalAsync();
                await App.Database.SaveNumratFiskalAsync(NumratFiskal);
            }

            var FiskalizimiKonfigurimetresult = await App.ApiClient.GetAsync("fiskalizimi-konfigurimet");
            if (FiskalizimiKonfigurimetresult.IsSuccessStatusCode) {
                var FiskalizimiKonfigurimetResponse = await FiskalizimiKonfigurimetresult.Content.ReadAsStringAsync();
                FiskalizimiKonfigurimet = JsonConvert.DeserializeObject<List<FiskalizimiKonfigurimet>>(FiskalizimiKonfigurimetResponse);
                await App.Database.ClearAllFiskalizimiKonfigurimi();
                await App.Database.SaveFiskalizimiKonfigurimetAsync(FiskalizimiKonfigurimet);
            }

            
            try {


                //TODO : FIX FISKALIZIMI KONFIGURIMET FROM API
                var query = from a in Agjendet
                            join d in Depot on a.Depo equals d.Depo
                            join fk in FiskalizimiKonfigurimet on d.TAGNR equals fk.TAGNR
                            join nf in NumratFiskal on fk.TCRCode equals nf.TCRCode into nfGroup
                            from nf in nfGroup.DefaultIfEmpty()
                            where a.IDAgjenti.ToLower() == LoginData.IDAgjenti.ToLower() && nf.Depo == d.Depo
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
                foreach (var quer in query) {
                    result[0] = quer.TAGNR;
                    result[1] = quer.TCRCode;
                    result[2] = quer.OperatorCode;
                    result[3] = quer.BusinessUnitCode;
                    result[4] = quer.IDN.ToString();
                    result[5] = quer.LevizjeIDN.ToString();
                    result[6] = quer.Viti.ToString();
                    Configurimi.KodiINjesiseSeBiznesit = quer.BusinessUnitCode;
                    Configurimi.KodiTCR = quer.TCRCode;
                    Configurimi.KodiIOperatorit = quer.OperatorCode;
                    Configurimi.TAGNR = quer.TAGNR;
                    await App.Database.SaveConfigurimiAsync(App.Instance.MainViewModel.Configurimi);
                }
                return result;
            }
            catch (Exception ex) {
                //PnUtils.DbUtils.WriteExeptionErrorLog(ex);
            }
            return null;
        }

        public async Task<string[]> GetConfigurationByIDAgjentiAsyncWithoutLogin() {
            string[] result = new string[7];
            var depotResult = await App.ApiClient.GetAsync("depot");
            if (depotResult.IsSuccessStatusCode) {
                var depotResponse = await depotResult.Content.ReadAsStringAsync();
                Depot = JsonConvert.DeserializeObject<List<Depot>>(depotResponse);
            }
            if (Agjendet == null) {
                Agjendet = new List<Agjendet> { LoginData };
            }
            var numratFiskLokal = await App.Database.GetNumratFiskalAsync();
            if (numratFiskLokal.Count > 0) {
                NumratFiskal = numratFiskLokal;
            }
            else {
                var numratFiskalResult = await App.ApiClient.GetAsync("numri-fisk");
                if (numratFiskalResult.IsSuccessStatusCode) {
                    var numratFiskalResponse = await numratFiskalResult.Content.ReadAsStringAsync();
                    var numratFiskal = JsonConvert.DeserializeObject<List<NumriFisk>>(numratFiskalResponse);
                    NumratFiskal = numratFiskal;
                    await App.Database.ClearAllNumratFiskalAsync();
                    await App.Database.SaveNumratFiskalAsync(NumratFiskal);
                }
            }

            var FiskalizimiKonfigurimetresult = await App.ApiClient.GetAsync("fiskalizimi-konfigurimet");
            if (FiskalizimiKonfigurimetresult.IsSuccessStatusCode) {
                var FiskalizimiKonfigurimetResponse = await FiskalizimiKonfigurimetresult.Content.ReadAsStringAsync();
                FiskalizimiKonfigurimet = JsonConvert.DeserializeObject<List<FiskalizimiKonfigurimet>>(FiskalizimiKonfigurimetResponse);
                await App.Database.ClearAllFiskalizimiKonfigurimi();
                await App.Database.SaveFiskalizimiKonfigurimetAsync(FiskalizimiKonfigurimet);
            }
            try {


                //TODO : FIX FISKALIZIMI KONFIGURIMET FROM API
                var query = from a in Agjendet
                            join d in Depot on a.Depo equals d.Depo
                            join fk in FiskalizimiKonfigurimet on d.TAGNR equals fk.TAGNR
                            join nf in NumratFiskal on fk.TCRCode equals nf.TCRCode into nfGroup
                            from nf in nfGroup.DefaultIfEmpty()
                            where a.IDAgjenti.ToLower() == LoginData.IDAgjenti.ToLower() && nf.Depo == d.Depo
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
                foreach (var quer in query) {
                    result[0] = quer.TAGNR;
                    result[1] = quer.TCRCode;
                    result[2] = quer.OperatorCode;
                    result[3] = quer.BusinessUnitCode;
                    result[4] = quer.IDN.ToString();
                    result[5] = quer.LevizjeIDN.ToString();
                    result[6] = quer.Viti.ToString();
                    App.Instance.MainViewModel.Configurimi.KodiINjesiseSeBiznesit = quer.BusinessUnitCode;
                    App.Instance.MainViewModel.Configurimi.KodiTCR = quer.TCRCode;
                    App.Instance.MainViewModel.Configurimi.KodiIOperatorit = quer.OperatorCode;
                    App.Instance.MainViewModel.Configurimi.TAGNR = quer.TAGNR;

                    await App.Database.SaveConfigurimiAsync(App.Instance.MainViewModel.Configurimi);
                }
                return result;
            }
            catch (Exception ex) {
                //PnUtils.DbUtils.WriteExeptionErrorLog(ex);
                UserDialogs.Instance.Alert("Problem me GetConfigurationByIDAgjentiAsync : " + ex.Message);
            }
            return null;
        }

        public async Task<bool> SinkronizoFiskalizimin() {
            //SyncLloji = 1 -- update all
            //SyncLloji = 2 -- mos bej update numrinFisk
            string[] config = await GetConfigurationByIDAgjentiAsync();

            string TAGNR, TCRCode, OperatorCode, BusinessUnitCode, IDN, LevijeIDN, Viti;

            var result = "";

            if (config != null) {
                TAGNR = config[0];
                TCRCode = config[1];
                OperatorCode = config[2];
                BusinessUnitCode = config[3];
                IDN = config[4];
                LevijeIDN = config[5];
                Viti = config[6];

                DataSet dsConfig = null;
                try {
                    var numriFiskal = await App.Database.GetNumratFiskalAsync();
                    var numriFisk = numriFiskal.FirstOrDefault(x => x.TCRCode == Configurimi.KodiTCR);
                    if (numriFisk == null) {
                        var numratFiskalResult = await App.ApiClient.GetAsync("numri-fisk/" + LoginData.Depo);
                        var numratFiskalResponse = await numratFiskalResult.Content.ReadAsStringAsync();
                        if (numratFiskalResult.IsSuccessStatusCode) {
                            var numratFiskal = JsonConvert.DeserializeObject<NumriFisk>(numratFiskalResponse);
                            numriFisk = numratFiskal;
                            var resultInsert = await App.Database.SaveNumriFiskalAsync(numratFiskal);
                        }
                    }
                    if (numriFisk == null) {
                        // Insert new record
                        var newNumriFisk = new NumriFisk
                        {
                            IDN = int.Parse(config[4]),
                            LevizjeIDN = int.Parse(config[5]),
                            Viti = int.Parse(config[6]),
                            Depo = LoginData.IDAgjenti,
                            TCRCode = config[1]
                        };

                        var resultInsert = await App.Database.SaveNumriFiskalAsync(newNumriFisk);
                        if (resultInsert != -1) {
                            return true;
                        }
                        else {
                            UserDialogs.Instance.Alert("Shenimet nuk u shtuan me sukses");
                            return false;
                        }
                    }
                    else {
                        // Update existing record

                        var jsonRequest = JsonConvert.SerializeObject(numriFisk);
                        var stringContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                        var putResult = await App.ApiClient.PutAsync("numri-fisk", stringContent);
                        if (putResult.IsSuccessStatusCode) {
                            return true;

                        }
                        //else {
                        //    UserDialogs.Instance.Alert("Shenimet nuk u shtuan me sukses");
                        //    return false;

                        //}
                        return false;
                    }
                }
                catch (Exception ex) {

                    //MessageBox.Show("Gabim, Nuk mund te ruhen konfigurimet e fiskalizimit!");
                    UserDialogs.Instance.Alert("Gabim, Nuk mund te ruhen konfigurimet e fiskalizimit!");
                    return false;
                }
            }
            else {
                //MessageBox.Show("Lidhja me MobSales ka deshtura ose nuk jane regjistruar konfigurimet per fiskalizim ne MobSales");
                UserDialogs.Instance.Alert("Lidhja me MobSales ka deshtuar ose nuk jane regjistruar konfigurimet per fiskalizim ne MobSales");
                return false;
            }
        }

        public async Task<bool> SinkronizoFiskaliziminWithoutLogin() {
            //SyncLloji = 1 -- update all
            //SyncLloji = 2 -- mos bej update numrinFisk
            string[] config = await GetConfigurationByIDAgjentiAsyncWithoutLogin();

            string TAGNR, TCRCode, OperatorCode, BusinessUnitCode, IDN, LevijeIDN, Viti;

            var result = "";

            if (config != null) {
                TAGNR = config[0];
                TCRCode = config[1];
                OperatorCode = config[2];
                BusinessUnitCode = config[3];
                IDN = config[4];
                LevijeIDN = config[5];
                Viti = config[6];

                DataSet dsConfig = null;
                try {
                    var numriFiskal = await App.Database.GetNumratFiskalAsync();
                    var numriFisk = numriFiskal.FirstOrDefault(x => x.TCRCode == Configurimi.KodiTCR);
                    if (numriFisk == null) {
                        var numratFiskalResult = await App.ApiClient.GetAsync("numri-fisk/" + LoginData.Depo);
                        var numratFiskalResponse = await numratFiskalResult.Content.ReadAsStringAsync();
                        if (numratFiskalResult.IsSuccessStatusCode) {
                            var numratFiskal = JsonConvert.DeserializeObject<NumriFisk>(numratFiskalResponse);
                            numriFisk = numratFiskal;
                            var resultInsert = await App.Database.SaveNumriFiskalAsync(numratFiskal);
                        }
                    }
                    if (numriFisk == null) {
                        // Insert new record
                        var newNumriFisk = new NumriFisk
                        {
                            IDN = int.Parse(config[4]),
                            LevizjeIDN = int.Parse(config[5]),
                            Viti = int.Parse(config[6]),
                            Depo = LoginData.IDAgjenti,
                            TCRCode = config[1]
                        };

                        var resultInsert = await App.Database.SaveNumriFiskalAsync(newNumriFisk);
                        if (resultInsert != -1) {
                            return true;
                        }
                        else {
                            UserDialogs.Instance.Alert("Shenimet nuk u shtuan me sukses");
                            return false;
                        }
                    }
                    else {
                        // Update existing record

                        var jsonRequest = JsonConvert.SerializeObject(numriFisk);
                        var stringContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                        var putResult = await App.ApiClient.PutAsync("numri-fisk", stringContent);
                        if (putResult.IsSuccessStatusCode) {
                            return true;

                        }
                        //else {
                        //    UserDialogs.Instance.Alert("Shenimet nuk u shtuan me sukses");
                        //    return false;

                        //}
                        return false;
                    }
                }
                catch (Exception ex) {

                    //MessageBox.Show("Gabim, Nuk mund te ruhen konfigurimet e fiskalizimit!");
                    UserDialogs.Instance.Alert("Gabim, Nuk mund te ruhen konfigurimet e fiskalizimit!");
                    return false;
                }
            }
            else {
                //MessageBox.Show("Lidhja me MobSales ka deshtura ose nuk jane regjistruar konfigurimet per fiskalizim ne MobSales");
                UserDialogs.Instance.Alert("Lidhja me MobSales ka deshtuar ose nuk jane regjistruar konfigurimet per fiskalizim ne MobSales");
                return false;
            }
        }
        public async Task GeneratePDFAsync() {
            // Create a new PDF document
            PdfDocument document = new PdfDocument();

            //Add a page to the document
            PdfPage page = document.Pages.Add();

            //Create PDF graphics for the page
            PdfGraphics graphics = page.Graphics;

            //Set the standard font
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);

            //Draw the text
            graphics.DrawString("Hello World!!!", font, PdfBrushes.Black, new PointF(0, 0));

            //Save the document to the stream
            MemoryStream stream = new MemoryStream();
            document.Save(stream);

            //Close the document
            document.Close(true);

            //Save the stream as a file in the device and invoke it for viewing
            await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("testBIXOLON.pdf", "application/pdf", stream);
        }
        public static Guid NONE = new Guid("00000000-0000-0000-0000-000000000000");

        public async Task ShtijeKorrigjim() {
            try {
                if(SelectedVizita != null) {
                    if(SelectedVizita.IDStatusiVizites != "0" || SelectedVizita.IDStatusiVizites != "1") {
                        if(IDPorosi != NONE) {
                            IDPorosi = NONE;
                            IDLiferimi = NONE;
                        }
                        var NumriFaturaveList = await App.Database.GetNumriFaturaveAsync();
                        var NumriFaturave = NumriFaturaveList.FirstOrDefault(x => x.KOD == LoginData.IDAgjenti);
                        if (NumriFaturave != null) {
                            var currentNumriFatures = NumriFaturave.CurrNrFat;
                            var currentNumriKUFIP = NumriFaturave.NRKUFIP;
                            var currentNumriKUFIS = NumriFaturave.NRKUFIS;
                            if (currentNumriFatures + currentNumriKUFIP > currentNumriKUFIS) {
                                UserDialogs.Instance.Alert("Nuk është i caktuar numri i kufive të faturave \n shitja nuk mund të vazhdojë", "Error", "Ok");
                                return;
                            }
                            else {
                                currentNumriFatures = currentNumriFatures + currentNumriKUFIP;
                                var companyInfo = await App.Database.GetCompanyInfoAsync();
                                if(companyInfo == null) {
                                    UserDialogs.Instance.Alert("Vlera e TVSH-se nuk është e përcaktuar \n Tabela:CompanyInfo");
                                    return;
                                }
                                Porosite newPorosi = new Porosite
                                {
                                    IDPorosia = Guid.NewGuid(),
                                    IDVizita = SelectedVizita.IDVizita,

                                }; 
                                ShitjaPage page = new ShitjaPage();
                                await App.Database.SavePorositeAsync(newPorosi);
                                ShitjaNavigationParameters np = new ShitjaNavigationParameters { Agjendi = LoginData, VizitaEHapur = SelectedVizita, NrFatures = (int)currentNumriFatures, PorosiaID = newPorosi.IDPorosia, ShitjeKorigjim = true };
                                ShitjaViewModel shitjaViewModel = new ShitjaViewModel(np);
                                page.BindingContext = shitjaViewModel;
                                App.Instance.ShitjaViewModel = shitjaViewModel;
                                await App.Instance.PushAsyncNewPage(page);
                            }
                        }
                        else {
                            UserDialogs.Instance.Alert("Nuk është i caktuar numri i kufive të faturave \n shitja nuk mund të vazhdojë", "Error", "Ok");
                            return;
                        }
                    }
                    else {
                        UserDialogs.Instance.Alert("Vizita nuk eshte e hapur!");
                        return;
                    }
                }
                else {
                    UserDialogs.Instance.Alert("Ju lutem selektoni njeren nga vizitat");
                    return;
                }
            }catch(Exception e) {

            }
        }
        public async Task KthimMalli() {
            if (SelectedVizita == null) {
                UserDialogs.Instance.Alert("Ju lutemi selektoni viziten para se te vazhdoni me shitje", "Error", "Ok");
                return;
            }
            if (SelectedVizita.IDStatusiVizites == "6") {
                UserDialogs.Instance.Alert("Vizita veqse ka perfunduar, ju lutem selektoni nje vizite tjeter", "Verejtje", "Ok");
                return;
            }
            //if (SelectedVizita.IDStatusiVizites != "1") {
            //    UserDialogs.Instance.Alert("Ju lutemi hapeni viziten para se te vazhdoni me shitje", "Error", "Ok");
            //    return;
            //}
            //foreach (var g in VizitatFilteredByDate) {
            //    if (g != null) {
            //        if (g.IDVizita.ToString() != SelectedVizita.IDVizita.ToString()) {
            //            if (g.IDStatusiVizites == "1") {
            //                UserDialogs.Instance.Alert("Ka vizite te hapur, ju lutem perfundoni viziten e hapur fillimisht", "Error", "Ok");
            //                return;
            //            }
            //        }
            //    }
            //}
            ShitjaPage page = new ShitjaPage();
            //          SELECT nf.CurrNrFat from NumriFaturave nf where nf.KOD = 'M03'
            //SELECT nf.NRKUFIP from NumriFaturave nf where nf.KOD = 'M03'
            //  SELECT nf.NRKUFIS from NumriFaturave nf where nf.KOD = 'M03'
            //Select count(*) from NumriFaturave nf where nf.KOD = 'M03'
            Arsyejet = await App.Database.GetArsyejetAsync();
            var NumriFaturaveList = await App.Database.GetNumriFaturaveAsync();
            var NumriFaturave = NumriFaturaveList.FirstOrDefault(x => x.KOD == LoginData.IDAgjenti);
            if (NumriFaturave != null) {
                var currentNumriFatures = NumriFaturave.CurrNrFat;
                var currentNumriKUFIP = NumriFaturave.NRKUFIP;
                var currentNumriKUFIS = NumriFaturave.NRKUFIS;
                if (currentNumriFatures + currentNumriKUFIP > currentNumriKUFIS) {
                    UserDialogs.Instance.Alert("Gabim ne leximin e fatures", "Error", "Ok");
                    return;
                }
                else {
                    currentNumriFatures = currentNumriFatures + currentNumriKUFIP;
                    Porosite newPorosi = new Porosite
                    {
                        IDPorosia = Guid.NewGuid(),
                        IDVizita = SelectedVizita.IDVizita,
                        
                    };
                    await App.Database.SavePorositeAsync(newPorosi);
                    ShitjaNavigationParameters np = new ShitjaNavigationParameters { Agjendi = LoginData, VizitaEHapur = SelectedVizita, NrFatures = (int)currentNumriFatures, PorosiaID = newPorosi.IDPorosia, KthimMalli = true };
                    ShitjaViewModel shitjaViewModel = new ShitjaViewModel(np);
                    page.BindingContext = shitjaViewModel;
                    App.Instance.ShitjaViewModel = shitjaViewModel;
                    await App.Instance.PushAsyncNewPage(page);
                }
            }
            else {
                Debug.Write("Error ne numrin e fatures");
            }
        }

        public async Task GoToShitjaAsync() {
            if (SelectedVizita == null) {
                UserDialogs.Instance.Alert("Ju lutemi selektoni viziten para se te vazhdoni me shitje", "Error", "Ok");
                return;
            }
            if (SelectedVizita.IDStatusiVizites == "6") {
                UserDialogs.Instance.Alert("Vizitje veqse ka perfunduar, ju lutem selektoni nje vizite tjeter", "Verejtje", "Ok");
                return;
            }
            //if (SelectedVizita.IDStatusiVizites != "1") {
            //    UserDialogs.Instance.Alert("Ju lutemi hapeni viziten para se te vazhdoni me shitje", "Error", "Ok");
            //    return;
            //}
            //foreach (var g in VizitatFilteredByDate) {
            //    if (g != null) {
            //        if (g.IDVizita.ToString() != SelectedVizita.IDVizita.ToString()) {
            //            if (g.IDStatusiVizites == "1") {
            //                UserDialogs.Instance.Alert("Ka vizite te hapur, ju lutem perfundoni viziten e hapur fillimisht", "Error", "Ok");
            //                return;
            //            }
            //        }
            //    }
            //}
            ShitjaPage page = new ShitjaPage();
            //          SELECT nf.CurrNrFat from NumriFaturave nf where nf.KOD = 'M03'
            //SELECT nf.NRKUFIP from NumriFaturave nf where nf.KOD = 'M03'
            //  SELECT nf.NRKUFIS from NumriFaturave nf where nf.KOD = 'M03'
            //Select count(*) from NumriFaturave nf where nf.KOD = 'M03'

            Arsyejet = await App.Database.GetArsyejetAsync();
            var NumriFaturaveList = await App.Database.GetNumriFaturaveAsync();
            var NumriFaturave = NumriFaturaveList.FirstOrDefault(x => x.KOD == LoginData.IDAgjenti);
            if (NumriFaturave != null) {
                var currentNumriFatures = NumriFaturave.CurrNrFat;
                var currentNumriKUFIP = NumriFaturave.NRKUFIP;
                var currentNumriKUFIS = NumriFaturave.NRKUFIS;
                if (currentNumriFatures + currentNumriKUFIP > currentNumriKUFIS) {
                    UserDialogs.Instance.Alert("Gabim ne leximin e fatures", "Error", "Ok");
                    return;
                }
                else {
                    currentNumriFatures = currentNumriFatures + currentNumriKUFIP;
                    Porosite newPorosi = new Porosite
                    {
                        IDPorosia = Guid.NewGuid(),
                        IDVizita = SelectedVizita.IDVizita
                    };
                    await App.Database.SavePorositeAsync(newPorosi);
                    ShitjaNavigationParameters np = new ShitjaNavigationParameters { Agjendi = LoginData, VizitaEHapur = SelectedVizita, NrFatures = (int)currentNumriFatures, PorosiaID = newPorosi.IDPorosia, KthimMalli = false,ShitjeKorigjim = false};
                    ShitjaViewModel shitjaViewModel = new ShitjaViewModel(np);
                    page.BindingContext = shitjaViewModel;
                    App.Instance.ShitjaViewModel = shitjaViewModel;
                    await App.Instance.PushAsyncNewPage(page);
                }
            }
            else {
                Debug.Write("Error ne numrin e fatures");
            }
        }

        public Vizita VizitaAktuale { get; set; }
        public async Task HapVizitenAsync() {
            try {
                if(SelectedVizita == null) {
                    UserDialogs.Instance.Alert("Ju lutem selektoni nje vizite");
                    return;
                }
                if (!checkOraRealizimit(SelectedVizita.IDVizita)) {
                    UserDialogs.Instance.Alert("Duhet te shtohet vizite e re per kete klient!", "Error", "Ok");
                    return;             
                }
                foreach (var g in VizitatFilteredByDate) {
                    if (g != null) {
                        if (g.IDVizita.ToString() != SelectedVizita.IDVizita.ToString()) {
                            if (g.IDStatusiVizites == "1") {
                                UserDialogs.Instance.Alert("Ka vizite te hapur, ju lutem perfundoni viziten e hapur fillimisht", "Error", "Ok");
                                return;
                            }
                        }
                    }
                }
                var res = VizitatFilteredByDate.FirstOrDefault(x => x.IDVizita == SelectedVizita.IDVizita);
                if (res != null) {
                    switch (res.IDStatusiVizites) {
                        case "0":
                            break;
                        case "1":
                            UserDialogs.Instance.Alert("Vizita veqse eshte e hapur", "Error", "Ok");
                            return;
                        case "2":
                        case "3":
                        case "4":
                        case "5":
                        case "6":
                            UserDialogs.Instance.Alert("Vizita veqse ka perfunduar, ju lutemi selektoni nje vizite tjeter", "Verejtje", "Ok");
                            return;
                        default:
                            break;
                    }
                    await App.Current.MainPage.Navigation.PopPopupAsync();
                    await App.Instance.PushAsyncNewPage(new HapVizitenPage() { BindingContext = this });
                    //var updateResult = await App.Database.UpdateVizitaAsync(res);
                    //if (updateResult == 1) {
                    //    
                    //}
                    //for (int i = 0; i < Vizitat.Count; i++) {
                    //    if (Vizitat[i].IDVizita == SelectedVizita.IDVizita) {
                    //        Vizitat[i] = res;
                    //    }
                    //}
                }
            } catch (Exception e) {
                UserDialogs.Instance.Alert("Error ne HapVizitenAsync  : " + e.Message);
            }


        }

        private bool checkOraRealizimit(System.Guid VizitaID) {
            if (SelectedVizita == null) {
                UserDialogs.Instance.Alert("Ju lutem selektoni viziten", "ERROR", "Ok");
                return false;
            }
            DateTime MyTimeInWesternEurope = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "GMT Standard Time").AddHours(2);

            DateTime FromDate = MyTimeInWesternEurope.AddDays(-(int)MyTimeInWesternEurope.DayOfWeek);
            FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day);

            DateTime ToDate = FromDate.AddDays(7);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 59, 998);

            int vizitat0 = 0;
            bool _result = false;
            try {
                //_selectQuery1 = @"select MAX(OraRealizimit) from Vizitat where DataPlanifikimit between '" + FromDate + "' and '" + ToDate + "'";
                
                var maxOraRealizimit = Vizitat
                    .Where(v => v.DataPlanifikimit >= FromDate && v.DataPlanifikimit <= ToDate)
                    .Max(v => v.OraRealizimit);
                if (maxOraRealizimit == null) {
                    maxOraRealizimit = MyTimeInWesternEurope;
                    if (SelectedVizita.OraRealizimit == null)
                        SelectedVizita.OraRealizimit = MyTimeInWesternEurope;
                }
                DateTime OraRealizimitAll = (DateTime)maxOraRealizimit;
                if(SelectedVizita.OraRealizimit == null) {
                    SelectedVizita.OraRealizimit = MyTimeInWesternEurope;
                }
                DateTime OraRealizimitVizites = (DateTime)SelectedVizita.OraRealizimit  == null ? MyTimeInWesternEurope : (DateTime)SelectedVizita.OraRealizimit;

                var DataArritjesVizites = SelectedVizita.DataAritjes;
                if(DataArritjesVizites == null) {
                    DataArritjesVizites = MyTimeInWesternEurope;
                }
                if (OraRealizimitVizites >= OraRealizimitAll) {
                    if (((DateTime)DataArritjesVizites).Date == MyTimeInWesternEurope.Date) {
                        _result = true;
                    }
                    else {
                        _result = false;
                    }
                }
                else {
                    _result = false;
                }
            }
            catch (Exception ex) {

            }
            return _result;
        }
        public bool DatesAreInTheSameWeek(DateTime date1, DateTime date2) {
            DateTime MyTimeInWesternEurope = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "GMT Standard Time").AddHours(2);

            if (date1.Date == DateTime.MinValue) {
                date1 = MyTimeInWesternEurope;
            }
            var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
            var d1 = date1.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date1));
            var d2 = date2.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date2));

            return d1 == d2;
        }
        public ObservableCollection<Vizita> Vizitat { get; set; }
        public ObservableCollection<Linqet> LinqetPerAPI { get; set; }
        public Linqet LinkuPerApiPerEdit { get; set; }
        public Linqet LinkuPerFiskPerEdit { get; set; }
        public ObservableCollection<Linqet> LinqetPerFiskalizim { get; set; }

        private ObservableCollection<Vizita> vizitatFilteredByDate;
        private string _llojDok;

        public List<Arsyejet> Arsyejet { get; set; }
        public List<StatusiVizites> StatusetEVizites { get; set; }

        public ObservableCollection<Vizita> VizitatFilteredByDate 
        { 
            get { return vizitatFilteredByDate; }
            
            set { SetProperty(ref vizitatFilteredByDate, value); } 
        }
        public ObservableCollection<KlientDheLokacion> KlientetDheLokacionet { get; set; }
        public bool AprovimFaturash { get; private set; }

        public async Task OpenKlientetAsync() {
            UserDialogs.Instance.ShowLoading("Loading..");
            var vizitatLokale = await App.Database.GetVizitatAsync();
            var KlientetDheLokacionetLokale = await App.Database.GetKlientetDheLokacionetAsync();
            Vizitat = new ObservableCollection<Vizita>(vizitatLokale);
            KlientetDheLokacionet = new ObservableCollection<KlientDheLokacion>(KlientetDheLokacionetLokale);
            VizitatFilteredByDate = new ObservableCollection<Vizita>();
            SearchedVizitat = new ObservableCollection<Vizita>();
            StatusetEVizites = await App.Database.GetStatusiVizitesAsync();
            foreach (var v in Vizitat) {
                if(KlientetDheLokacionet != null) {
                    if (v.DataPlanifikimit != null) {
                        v.Klienti = KlientetDheLokacionet.FirstOrDefault(x => x.IDKlienti == v.IDKlientDheLokacion)?.KontaktEmriMbiemri ?? string.Empty;
                        v.Vendi = KlientetDheLokacionet.FirstOrDefault(x => x.IDKlienti == v.IDKlientDheLokacion)?.EmriLokacionit ?? string.Empty;
                        v.Adresa = KlientetDheLokacionet.FirstOrDefault(x => x.IDKlienti == v.IDKlientDheLokacion)?.Adresa ?? string.Empty;
                        if (v.IDStatusiVizites == "1") {

                        }
                        if(v.Klienti != string.Empty && v.Vendi != string.Empty) {
                            if (DatesAreInTheSameWeek((DateTime)v.DataPlanifikimit.Value.Date, DateTime.Today)) {
                                VizitatFilteredByDate.Add(v);
                            }
                        }
                    }
                    //v.Klienti = KlientetDheLokacionet.FirstOrDefault(x => x.IDKlienti == v.IDKlientDheLokacion)?.Adresa ?? "PA IDENTIFIKUAR";
                }
            }
            VizitatFilteredByDate = new ObservableCollection<Vizita>(VizitatFilteredByDate.OrderBy(x => x.Klienti));
            VizitatFilteredByDate = new ObservableCollection<Vizita>(VizitatFilteredByDate.OrderByDescending(x => x.IDStatusiVizites));
            ClientsPage ClientsPage = new ClientsPage();
            ClientsPage.BindingContext = this;
            await Task.Delay(400);
            await App.Instance.PushAsyncNewPage(ClientsPage);
            UserDialogs.Instance.HideLoading();
        }
        public ADatePicker ADatePicker;
        public async Task OpenPorositeAsync() {
            UserDialogs.Instance.ShowLoading("Loading..");
            PorositePage PorositePage = new PorositePage();
            var klientet = await App.Database.GetKlientetAsync();
            var krijimiPorosive = await App.Database.GetKrijimiPorosiveAsync();
            var orders = await App.Database.GetOrdersAsync();
            var nrRendor = orders.Where(x => x.Data.Year == DateTime.Now.Year && x.Data.Month == DateTime.Now.Month && x.Data.Date == DateTime.Now.Date);
            PorositeViewModelNavigationParameters porositeViewModelNavigationParameters = new PorositeViewModelNavigationParameters { Agjendi = LoginData, Klientet = klientet, KrijimiPorosives = krijimiPorosive, NrRendor = nrRendor.Count() + 1, Orders = orders};
            PorositePage.BindingContext = new PorositeViewModel(porositeViewModelNavigationParameters);
            await App.Instance.PushAsyncNewPage(PorositePage);
            UserDialogs.Instance.HideLoading();
        }        
        
        public async Task OpenInkasimiAsync() {
            UserDialogs.Instance.ShowLoading("Loading..");
            InkasimiPage inkasimiPage = new InkasimiPage();
            var klientet = await App.Database.GetKlientetAsync();
            klientet = klientet.Where(x => x.Depo.Trim() == LoginData.Depo).ToList();
            var detyrimet = await App.Database.GetDetyrimetAsync();
            if(detyrimet.Count < 1) {
                var detyrimetResult = await App.ApiClient.GetAsync("detyrimet");
                if (detyrimetResult.IsSuccessStatusCode) {
                    var detyrimetResponse = await detyrimetResult.Content.ReadAsStringAsync();
                    detyrimet = JsonConvert.DeserializeObject<List<Detyrimet>>(detyrimetResponse);
                    if (detyrimet.Count > 0)
                        await App.Database.SaveAllDetyrimetAsync(detyrimet);
                }
                else {
                    UserDialogs.Instance.Alert("Problem ne marrjen e detyrimeve, provoni me vone");
                    return;
                }

            }
            var evidencaPagesave = await App.Database.GetEvidencaPagesaveAsync();
            inkasimiPage.BindingContext = new InkasimiViewModel(new InkasimiViewModelNavigationParameters { Agjendi = LoginData , Detyrimet = detyrimet, EvidencaPagesave = evidencaPagesave, Klientet = klientet });
            await App.Instance.PushAsyncNewPage(inkasimiPage);
            UserDialogs.Instance.HideLoading();
        }
        public async Task OpenLevizjetAsync() {
            UserDialogs.Instance.ShowLoading("Loading..");
            LevizjetListPage levizjetListPage = new LevizjetListPage();


            //LevizjetPage LevizjetPage = new LevizjetPage();
            var artikujt = await App.Database.GetArtikujtAsync();
            var klientet = await App.Database.GetDepotAsync();
            var levizjetHeader = await App.Database.GetLevizjetHeaderAsync();
            var agjendi = LoginData;
            levizjetListPage.BindingContext = new LevizjetViewModel(new LevizjetNavigationParameters { LevizjetHeader = levizjetHeader,  Artikujt = artikujt, Klientet = klientet, Agjendi = agjendi });
            await App.Instance.PushAsyncNewPage(levizjetListPage);
            UserDialogs.Instance.HideLoading();
        }

        public async Task OpenArtikujtAsync() {
            UserDialogs.Instance.ShowLoading("Loading..");
            ArtikujtPage ArtikujtPage = new ArtikujtPage();
            var artikujt = await App.Database.GetArtikujtAsync();

            var malliMbetur = await App.Database.GetMalliMbeturAsync();
            var MalliMbetur = new ObservableCollection<Malli_Mbetur>(malliMbetur);

            var SalesPrices = new ObservableCollection<SalesPrice>(await App.Database.GetSalesPriceAsync());
            //SalesPrices = new ObservableCollection<SalesPrice>(await App.Database.GetSalesPriceAsync());
            //MalliMbetur = new ObservableCollection<Malli_Mbetur>(await App.Database.GetMalliMbeturAsync());
            var artikujtPerShfaqje = new List<Artikulli>();
            foreach (var artikulli in artikujt) {
                artikulli.CmimiNjesi = SalesPrices.FirstOrDefault(x => x.ItemNo == artikulli.IDArtikulli && x.SalesCode == "STANDARD")?.UnitPrice;
                var hasTwoMalliMbeturs = malliMbetur.Where(x => x.IDArtikulli == artikulli.IDArtikulli);
                if(hasTwoMalliMbeturs.Count() > 1) {
                    foreach(var mm in hasTwoMalliMbeturs) {
                        if (mm != null) {
                            if (mm.SasiaMbetur > 0 || mm.SasiaMbetur < 0) {
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
                }
                else {
                    var MalliiMbetur = MalliMbetur.FirstOrDefault(x => x.IDArtikulli == artikulli.IDArtikulli && x.Depo == LoginData.IDAgjenti);
                    if (MalliiMbetur != null) {
                        if(MalliiMbetur.SasiaMbetur > 0 || MalliiMbetur.SasiaMbetur < 0) {
                            artikulli.Sasia = MalliiMbetur.SasiaMbetur;
                            artikulli.Seri = MalliiMbetur.Seri;
                            artikujtPerShfaqje.Add(artikulli);
                        }
                    }
                }

            }
            var artikujtPerShfaqjeFinal = new ObservableCollection<Artikulli>(artikujtPerShfaqje.Where(x=> x.Sasia < 0 || x.Sasia > 0));
            ArtikujtPage.BindingContext = new ArtikujtViewModel(new ArikujtNavigationParameters { Artikujt = artikujtPerShfaqjeFinal.ToList() }) ;
            await App.Instance.PushAsyncNewPage(ArtikujtPage);
            UserDialogs.Instance.HideLoading();
        }
        
        public async Task OpenSinkronizimiAsync() {
            UserDialogs.Instance.ShowLoading("Loading..");
            SinkronizimiPage SinkroPage = new SinkronizimiPage();
            SinkroPage.BindingContext = new SinkronizimiViewModel(new SinkronizimiNavigationParameters { Agjendi = LoginData });
            await App.Instance.PushAsyncNewPage(SinkroPage);
            UserDialogs.Instance.HideLoading();

        }

        public async Task OpenCreateNewKlientAsync() {
            CreatedKlient = new Klientet();
            SelectedVizita = new Vizita();
            SelectedAuthenticatedUser = new AuthenticatedUser();
            UserDialogs.Instance.ShowLoading("Loading..");
            CreateClientPage ClientsPage = new CreateClientPage();
            ClientsPage.BindingContext = this;
            await App.Instance.PushAsyncNewPage(ClientsPage);
            UserDialogs.Instance.HideLoading();
        }

        public async Task SaveNewClientAsync() {
            var result = await App.Database.SaveKlientAsync(CreatedKlient);
            if (result == 0) {
                await UserDialogs.Instance.AlertAsync("Deshtim, klienti nuk u shtua", "Deshtim");
            }
            else
                await UserDialogs.Instance.AlertAsync("Sukses, klienti u shtua", "Sukses");
            await App.Instance.MainPage.Navigation.PopAsync();
        }

        public async Task SaveNumriFaturaveAsync() {
            try {
                //                IDNumri KOD NRKUFIP NRKUFIS NRKUFIPJT NRKUFISJT   DataBrezit CurrNrFat   CurrNrFatJT SyncStatus  NRKUFIP_D NRKUFIS_D   CurrNrFat_D
                //1646737 M03 145065  160000  50020002    50020001    2023 - 11 - 01 11:49:28.227 1   0   NULL    509630  511000  0

                NumriFaturave numriFaturave = new NumriFaturave
                {
                    IDNumri = 1646737,
                    KOD = "M03",
                    NRKUFIP = 145065,
                    NRKUFIS = 160000,
                    NRKUFIPJT = 50020002,
                    NRKUFISJT = 50020001,
                    DataBrezit = DateTime.Now,
                    CurrNrFat = 1,
                    CurrNrFatJT = 0,
                    SyncStatus = 0,
                    NRKUFIP_D = 509630,
                    NRKUFIS_D = 511000,
                    CurrNrFat_D = 0
                };
                await App.Database.SaveNumriFaturaveAsync(numriFaturave);
                await UserDialogs.Instance.AlertAsync("Sukses, Numri faturave u shtuan", "Sukses");

            }
            catch (Exception e) {

            }
        }

        public async Task SaveSalesPricesAsync() {
            try {
                SalesPrice salesPrice = new SalesPrice
                {
                    ItemNo = "A0737",
                    SalesCode = "K47075",
                    UnitPrice = 130
                };


                SalesPrice salesPrice2 = new SalesPrice
                {
                    ItemNo = "A3007",
                    SalesCode = "K47075",
                    UnitPrice = 829
                };


                SalesPrice salesPrice3 = new SalesPrice
                {
                    ItemNo = "A0733",
                    SalesCode = "K47075",
                    UnitPrice = 207.2
                };
                await App.Database.SaveSalesPriceAsync(salesPrice);
                await App.Database.SaveSalesPriceAsync(salesPrice2);
                await App.Database.SaveSalesPriceAsync(salesPrice3);
                await UserDialogs.Instance.AlertAsync("Sukses, Sales Prices u shtuan", "Sukses");

            }
            catch (Exception e) {

            }
        }

        public async Task SaveMalliMbeturAsync() {
            try {
                Malli_Mbetur malli = new Malli_Mbetur
                {
                    IDArtikulli = "A0733",
                    Emri = "LASAGNE UOVO 250 GR",
                    SasiaPranuar = 0,
                    SasiaKthyer = 0,
                    SasiaShitur = 0,
                    SasiaMbetur = 4,
                    Data = DateTime.Now,
                    LLOJDOK = "KM",
                    Depo = "M03",
                    PKeyIDArtDepo = "P601M47",
                    ID = 27401823,
                    LevizjeStoku = 0

                };
                Malli_Mbetur malli2 = new Malli_Mbetur
                {
                    IDArtikulli = "A0733",
                    Emri = "TORTELLINI RICO/SPINA 250",
                    SasiaPranuar = 0,
                    SasiaKthyer = 0,
                    SasiaShitur = 0,
                    SasiaMbetur = 22,
                    Data = DateTime.Now,
                    LLOJDOK = "KM",
                    Depo = "M03",
                    PKeyIDArtDepo = "P601M47",
                    ID = 27401823,
                    LevizjeStoku = 0

                };
                Malli_Mbetur malli3 = new Malli_Mbetur
                {
                    IDArtikulli = "A3007",
                    Emri = "DJATH LOPE ERZENI",
                    SasiaPranuar = 0,
                    SasiaKthyer = 0,
                    SasiaShitur = 0,
                    SasiaMbetur = 2,
                    Data = DateTime.Now,
                    LLOJDOK = "KM",
                    Depo = "M03",
                    PKeyIDArtDepo = "P601M47",
                    ID = 27401823,
                    LevizjeStoku = 0

                };

                await App.Database.SaveMalliMbeturAsync(malli);
                await App.Database.SaveMalliMbeturAsync(malli2);
                await App.Database.SaveMalliMbeturAsync(malli3);

            }
            catch (Exception e) {

            }

        }

        public async Task SaveSampleArtikujtAsync() {
            try {
                Artikulli artikulli = new Artikulli
                {
                    IDArtikulli = "A0733",
                    Emri = "LASAGNE UOVO 250 GR",
                    BUM = "COP"
                };

                Artikulli artikulli2 = new Artikulli
                {
                    IDArtikulli = "A0737",
                    Emri = "TORTELLINI RICO/SPINA 250",
                    BUM = "COP"
                };

                Artikulli artikulli3 = new Artikulli
                {
                    IDArtikulli = "A3007",
                    Emri = "DJATH LOPE ERZENI",
                    BUM = "KG"
                };

                await App.Database.SaveArtikulliAsync(artikulli);
                await App.Database.SaveArtikulliAsync(artikulli2);
                await App.Database.SaveArtikulliAsync(artikulli3);
            } catch (Exception e) {

            }

        }
        public async Task SaveNewVizitaAsync() {
            SelectedVizita = new Vizita
            {
                IDVizita = new Guid(),
                DataPlanifikimit = DateTime.Now.AddDays(1),
                IDAgjenti = "M03",
                NrRendor = 4,
                IDStatusiVizites = "0",
                IDKlientDheLokacion = "K47075",
                DeviceID = "M-55",
                SyncStatus = 0,
            };
            KlientDheLokacion Klientdhelokacion = new KlientDheLokacion
            {
                IDKlienti = "K47075",
                Adresa = "Himare Vlore",
                EmriLokacionit = "HIMARE",
                KontaktEmriMbiemri = "ARDIAN ZHUPAJ",
                IDVendi = "3115",
                IDKlientDheLokacion = "K47075"
            };
            var result = await App.Database.SaveVizitaAsync(SelectedVizita);
            var kdlresult = await App.Database.SaveKlientiDheLokacioni(Klientdhelokacion);
            if (result == 0) {
                await UserDialogs.Instance.AlertAsync("Deshtim, klienti nuk u shtua", "Deshtim");
            }
            else
                await UserDialogs.Instance.AlertAsync("Sukses, vizita u shtua", "Sukses");
            await App.Instance.MainPage.Navigation.PopAsync();
        }

        public async Task SaveAuthenticatedUserAsync() {
            var result = await App.Database.SaveAuthenticatedUserAsync(SelectedAuthenticatedUser);
            if (result == 0) {
                await UserDialogs.Instance.AlertAsync("Deshtim, klienti nuk u shtua", "Deshtim");
            }
            else
                await UserDialogs.Instance.AlertAsync("Sukses, perosni i autorizuar u shtua", "Sukses");
            await App.Instance.MainPage.Navigation.PopAsync();
        }
    }

    public static class IsBetweenClass{
        public static bool IsBetween<T>(this T item, T start, T end) {
            return Comparer<T>.Default.Compare(item, start) >= 0
                && Comparer<T>.Default.Compare(item, end) <= 0;
        }
    }
}
