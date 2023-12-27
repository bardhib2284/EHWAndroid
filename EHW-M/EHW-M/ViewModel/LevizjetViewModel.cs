using Acr.UserDialogs;
using EHW_M;
using EHWM.Models;
using EHWM.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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

            LevizjaEPerfunduar = false;
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
                                    .Select(v => v.LevizjeIDN)
                                    .FirstOrDefault();
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
                    NumriFisk = topLevizjeIDN,
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
