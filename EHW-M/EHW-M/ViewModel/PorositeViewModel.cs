using Acr.UserDialogs;
using EHW_M;
using EHWM.Models;
using EHWM.Views;
using EHWM.Views.Popups;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
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
        }

        public async Task KonvertoPorosine() {
            
        }
    }
}
