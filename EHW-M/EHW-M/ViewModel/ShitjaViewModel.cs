﻿using Acr.UserDialogs;
using EHW_M;
using EHWM.DependencyInjections.FiskalizationExtraModels;
using EHWM.Models;
using EHWM.Services;
using EHWM.Views;
using EHWM.Views.Popups;
using Newtonsoft.Json;
using Plugin.BxlMpXamarinSDK;
using Plugin.BxlMpXamarinSDK.Abstractions;
using Rg.Plugins.Popup.Extensions;
using SQLite;
using Syncfusion.Compression;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using static EHWM.ViewModel.SinkronizimiViewModel;
using static SQLite.SQLite3;

namespace EHWM.ViewModel {
    public class ShitjaNavigationParameters {
        public Vizita VizitaEHapur { get; set; }
        public int NrFatures { get; set; }
        public Guid PorosiaID { get; set; }
        public bool KthimMalli { get; set; }
        public Agjendet Agjendi { get; set; }
        public bool ShitjeKorigjim { get; set; }
    }
    public class ShitjaViewModel : BaseViewModel {
        private bool _krijoPorosine;
        public bool KrijoPorosine {
            get { return _krijoPorosine; }
            set { SetProperty(ref _krijoPorosine, value); }
        }
        public Vizita VizitaESelektuar { get; set; }
        public static Guid NAV = new Guid("10000000-0000-0000-0000-000000000000");
        public static Guid IDPorosi = NAV, IDLiferimi = NAV, LfrStat = NAV, ListaNePritje = NAV, CurrIDV = NAV;
        private Artikulli _currentlySelectedArtikulli;
        public Artikulli CurrentlySelectedArtikulli {
            get {
                return _currentlySelectedArtikulli;
            }
            set { SetProperty(ref _currentlySelectedArtikulli, value); }
        }

        public Agjendet LoginData { get; set; }

        private float _sasia;
        public float Sasia {
            get { return _sasia; }
            set { SetProperty(ref _sasia, value); }
        }

        public ICommand ZgjedhArtikullinCommand { get; set; }
        public ICommand IncreaseSasiaCommand { get; set; }
        public ICommand DecreaseSasiaCommand { get; set; }
        public ICommand AddArtikulliCommand { get; set; }
        public ICommand RegjistroCommand { get; set; }
        public ICommand VazhdoTeKonfirmoPageCommand { get; set; }
        public ICommand DaljeCommand { get; set; }
        public ICommand FshijArtikullinCommand { get; set; }
        public ICommand EditArtikullinCommand { get; set; }



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

        private int _NrFatures;
        public int NrFatures {
            get {
                return _NrFatures;
            }
            set { SetProperty(ref _NrFatures, value); }
        }
                
        private int _NrFatKthim;
        public int NrFatKthim {
            get {
                return _NrFatKthim;
            }
            set { SetProperty(ref _NrFatKthim, value); }
        }

        public string Title { get; set; }
        public DateTime TodayDate => TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "GMT Standard Time").AddHours(1);
        public ShitjaViewModel(ShitjaNavigationParameters navigationParameters) {
            Sasia = 0;
            if(navigationParameters.VizitaEHapur != null) {
                VizitaESelektuar = navigationParameters.VizitaEHapur;
                NrFatures = navigationParameters.NrFatures;
                IDPorosi = navigationParameters.PorosiaID;
                KthimMalli = navigationParameters.KthimMalli;
                LoginData = navigationParameters.Agjendi;
                if (KthimMalli) {
                    Title = "Kthimi";
                    Sasia = -1;
                    KthimMalli = true;
                }
                else {
                    KthimMalli = false;
                    Title = "Shitje";
                }
                ShitjeKorrigjim = navigationParameters.ShitjeKorigjim;
            }
            ZgjedhArtikullinCommand = new Command(async () => await ZgjedhArtikullinAsync());
            AddArtikulliCommand = new Command(async () => await AddArtikulliAsync());
            RegjistroCommand = new Command(async () => await RegjistroAsync());
            VazhdoTeKonfirmoPageCommand = new Command(async () => await VazhdoTeKonfirmoPageAsync());
            DaljeCommand = new Command(async () => await DaljeAsync());
            IncreaseSasiaCommand = new Command(IncreaseSasia);
            DecreaseSasiaCommand = new Command(DecreaseSasia);
            FshijArtikullinCommand = new Command(FshijArtikullinAsync);
            EditArtikullinCommand = new Command(EditArtikullinAsync);
            DateTime MyTimeInWesternEurope = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "GMT Standard Time").AddHours(1);
            DataEPageses = MyTimeInWesternEurope;
            SearchedArtikujt = new ObservableCollection<Artikulli>();
            KrijoPorosine = false;
        }
        private bool _kthimMalli;

        public bool KthimMalli {
            get { return _kthimMalli; }
            set { SetProperty(ref _kthimMalli, value); }
        }

        private bool _EnableSeri;
        public bool EnableSeri {
            get { return _EnableSeri; }
            set { SetProperty(ref _EnableSeri, value); }
        }
        private bool _shitjeKorrigjim;
        public bool ShitjeKorrigjim {
            get { return _shitjeKorrigjim; }
            set { SetProperty(ref _shitjeKorrigjim, value); }
        }
        private bool AprovimFaturash { get; set; }
        private string _llojDok { get; set; }
        public static decimal TotaliFature, TotaliPaguar;
        public DateTime DataEPageses { get; set; }
        public string PagesaType { get; set; }
        public bool ClosedEditPopup { get; set; }
        public async void EditArtikullinAsync() {
            ClosedEditPopup = false;
            EditActive = true;
            var registerArk = new RegjistroArkenPopup() { BindingContext = this};
            registerArk.Shitja("dd");
            await App.Instance.MainPage.Navigation.PushPopupAsync(registerArk, true);
            while (EditActive) {
                if(KthimMalli) {
                    if(NewSasia > 0) {
                        EditActive = true;
                        UserDialogs.Instance.Alert("Ju lutemi permiresoni sasin, duhet te jete me vlere negative", "Verejtje", "Ok");
                        NewSasia *= -1;
                    }
                }
                if(CurrentlySelectedArtikulli != null)
                {
                    if (NewSasia > CurrentlySelectedArtikulli.SasiaMax)
                    {
                        EditActive = true;
                        UserDialogs.Instance.Alert("Sasia tejkalon sasine maksimale per kete artikull qe eshte : " + CurrentlySelectedArtikulli.SasiaMax);
                        NewSasia = CurrentlySelectedArtikulli.SasiaMax;
                    }
                    await Task.Delay(50);
                }
            }
            if (!ClosedEditPopup)
            {
                CurrentlySelectedArtikulli = null;
                return;
            }
            var CurrentInxed = SelectedArikujt.IndexOf(CurrentlySelectedArtikulli);
            SelectedArikujt.Remove(CurrentlySelectedArtikulli);
            TotalPrice -= (double)CurrentlySelectedArtikulli.CmimiTotal;
            CurrentlySelectedArtikulli.Sasia = NewSasia;
            SelectedArikujt.Insert(CurrentInxed,CurrentlySelectedArtikulli);
            TotalPrice += (double)CurrentlySelectedArtikulli.CmimiTotal;
            TotalBillPrice = TotalPrice;
            CurrentlySelectedArtikulli = null;
            NewSasia = 0;
        }
        public bool EditActive { get; set; }
        private float _newSasia;
        public float NewSasia {
            get { return _newSasia; }
            set { SetProperty(ref _newSasia, value); }
        }
        public void  FshijArtikullinAsync() {
            SelectedArikujt.Remove(CurrentlySelectedArtikulli);
            TotalPrice -= (double)CurrentlySelectedArtikulli.CmimiTotal;
            TotalBillPrice = TotalPrice;
            CurrentlySelectedArtikulli = null;
        }
        public async Task VazhdoTeKonfirmoPageAsync() {
            UserDialogs.Instance.ShowLoading("Duke perfunduar faturen");
            var liferimet = await App.Database.GetLiferimetAsync();
            bool checkServer = true;
            if (KthimMalli)
            {
                decimal lastPriceAdded = 0;
                decimal PriceToNotPass = 0;
                foreach (var lif in liferimet)
                {
                    if (lif.NumriFisk == NrFatKthim && lif.DeviceID == App.Instance.MainViewModel.Configurimi.Paisja)
                    {
                        if(lif.DataLiferuar.Day == DateTime.Now.Day)
                        {
                            checkServer = false;
                        }
                        PriceToNotPass = (decimal)lif.CmimiTotal;
                        break;
                    }

                }
                if(PriceToNotPass > 0)
                {
                    if ((decimal)(TotalPrice) < (PriceToNotPass * -1))
                    {
                        UserDialogs.Instance.Alert("Kthimi me kete shume kalon/afron shumen totale te fatures : " + PriceToNotPass);
                        UserDialogs.Instance.HideLoading();
                        return;
                    }
                }
                lastPriceAdded += (decimal)TotalPrice;
                foreach (var lif in liferimet)
                {

                    if (lif.IDKthimi != NrFatKthim.ToString())
                        continue;
                    if ((decimal)lif.CmimiTotal - (decimal)TotalPrice < (PriceToNotPass * -1))
                    {
                        UserDialogs.Instance.Alert("Kthimi me kete shume kalon shumen totale te fatures : " + PriceToNotPass + " \nPer kete fature ka kthim ne vlere : " + lif.CmimiTotal);
                        UserDialogs.Instance.HideLoading();

                        return;
                    }
                    lastPriceAdded = Math.Abs(lastPriceAdded) + Math.Abs((decimal)lif.CmimiTotal);
                    
                    lastPriceAdded *= -1;
                    if (lastPriceAdded < (PriceToNotPass * -1))
                    {
                        UserDialogs.Instance.Alert("Kthimi me kete shume kalon/afron shumen totale te fatures : " + PriceToNotPass + " \nPer kete fature ka kthim ne vlere : " + lif.CmimiTotal);
                        UserDialogs.Instance.HideLoading();

                        return;
                    }
                    if ((decimal)(TotalPrice) < (PriceToNotPass * -1))
                    {
                        UserDialogs.Instance.Alert("Kthimi me kete shume kalon/afron shumen totale te fatures : " + PriceToNotPass);
                        UserDialogs.Instance.HideLoading();
                        return;
                    }
                }
                if (App.Instance.DoIHaveInternetNoAlert())
                {
                    if(checkServer)
                    {
                        var response = await App.ApiClient.GetAsync("fatura-e-fiskalizuara/");
                        if (response.IsSuccessStatusCode)
                        {
                            var responseString = await response.Content.ReadAsStringAsync();
                            var FaturatEFiskalizuara = JsonConvert.DeserializeObject<List<FaturatEFiskalizuara>>(responseString);
                            var klients = await App.Database.GetKlientetAsync();
                            var currKlient = klients.FirstOrDefault(x => x.IDKlienti == VizitaESelektuar.IDKlientDheLokacion);
                            foreach (var fature in FaturatEFiskalizuara)
                            {
                                if (fature.IDFature == NrFatKthim.ToString() && fature.DeviceID == App.Instance.MainViewModel.Configurimi.Shfrytezuesi && fature.NIPT == currKlient.NIPT)
                                {
                                    PriceToNotPass = fature.TotalAmount;
                                    break;
                                }

                            }
                            if (PriceToNotPass > 0)
                            {
                                if ((decimal)(TotalPrice) < (PriceToNotPass * -1))
                                {
                                    UserDialogs.Instance.Alert("Kthimi me kete shume kalon/afron shumen totale te fatures : " + PriceToNotPass);
                                    UserDialogs.Instance.HideLoading();
                                    return;
                                }
                            }
                            foreach (var fature in FaturatEFiskalizuara)
                            {
                                if (fature.IDFatureCorrected == NrFatKthim.ToString() && fature.DeviceID == App.Instance.MainViewModel.Configurimi.Shfrytezuesi)
                                {
                                    if (fature.Type != "KTHIM")
                                    {
                                        continue;
                                    }
                                    if (fature.TotalAmount + (decimal)TotalPrice < (PriceToNotPass * -1))
                                    {
                                        UserDialogs.Instance.Alert("Kthimi me kete shume kalon/afron shumen totale te fatures : " + PriceToNotPass + " \nPer kete fature ka kthim ne vlere : " + fature.TotalAmount);
                                        UserDialogs.Instance.HideLoading();

                                        return;
                                    }
                                    lastPriceAdded = Math.Abs(lastPriceAdded) + Math.Abs(fature.TotalAmount);
                                    lastPriceAdded *= -1;
                                    if (lastPriceAdded < (PriceToNotPass * -1))
                                    {
                                        UserDialogs.Instance.Alert("Kthimi me kete shume kalon/afron shumen totale te fatures : " + PriceToNotPass + " \nPer kete fature ka kthim ne vlere : " + lastPriceAdded);
                                        UserDialogs.Instance.HideLoading();

                                        return;
                                    }
                                    if (fature.TotalAmount - (decimal)TotalPrice < (PriceToNotPass * -1))
                                    {
                                        UserDialogs.Instance.Alert("Kthimi me kete shume kalon shumen totale te fatures : " + PriceToNotPass + " \nPer kete fature ka kthim ne vlere : " + fature.TotalAmount);
                                        UserDialogs.Instance.HideLoading();

                                        return;
                                    }
                                }
                            }
                        }
                    }
                    
                }

                
 
            }
            var liferimetToday = liferimet.Where(x=> x.KohaLiferuar.Date.Day == DateTime.Now.Day && x.KohaLiferuar.Date.Month == DateTime.Now.Month && x.KohaLiferuar.Year == DateTime.Now.Year);
            var klietns = await App.Database.GetKlientetAsync();
            var currClient = klietns.FirstOrDefault(x => x.IDKlienti == VizitaESelektuar.IDKlientDheLokacion);
            klietns = klietns.Where(x => x.NIPT == currClient.NIPT).ToList();
            var lifForNipt = new List<Liferimi>();
            foreach(var lif in liferimetToday) {
                if (klietns.FirstOrDefault(x => x.IDKlienti == lif.IDKlienti) != null)
                    lifForNipt.Add(lif);
            }
            var niptTotal = lifForNipt.Sum(x=> x.ShumaPaguar);
            if (niptTotal >= 150000) {
                UserDialogs.Instance.Alert("Ju keni arritur faturimin maximal per NIPT per sot");
                UserDialogs.Instance.HideLoading();
                return;
            }
            else {
                if(App.Instance.DoIHaveInternetNoAlert()) {
                    try
                    {
                        var response = await App.ApiClient.GetAsync("liferimi");
                        if (response.IsSuccessStatusCode)
                        {
                            var responseLif = await response.Content.ReadAsStringAsync();
                            var liferimetServer = JsonConvert.DeserializeObject<List<Liferimi>>(responseLif);
                            var liferimetTodayServer = liferimetServer.Where(x => x.KohaLiferuar.Date.Day == DateTime.Now.Day && x.KohaLiferuar.Date.Month == DateTime.Now.Month && x.KohaLiferuar.Year == DateTime.Now.Year);
                            var lifForNiptServer = new List<Liferimi>();
                            foreach (var lif in liferimetTodayServer)
                            {
                                if (klietns.FirstOrDefault(x => x.IDKlienti == lif.IDKlienti) != null)
                                {
                                    if (liferimetToday.FirstOrDefault(x => x.IDLiferimi == lif.IDLiferimi) == null)
                                    {
                                        lifForNipt.Add(lif);
                                    }
                                }
                            }
                            var niptTotalServer = lifForNipt.Sum(x => x.ShumaPaguar);
                            if (niptTotalServer >= 150000)
                            {
                                UserDialogs.Instance.Alert("Ju keni arritur faturimin maximal per NIPT per sot");
                                UserDialogs.Instance.HideLoading();
                                return;
                            }
                            niptTotalServer += (float)TotalPrice;
                            if (niptTotalServer >= 150000)
                            {
                                UserDialogs.Instance.Alert("Fatura kalon faturimin maximal per NIPT : " + niptTotalServer);
                                UserDialogs.Instance.HideLoading();
                                return;
                            }
                        }
                    }catch(Exception ex)
                    {

                    }

                }
                niptTotal += (float)TotalPrice;
                if (niptTotal >= 150000) {
                    UserDialogs.Instance.Alert("Fatura kalon faturimin maximal per NIPT : " + niptTotal);
                    UserDialogs.Instance.HideLoading();
                    return;
                }
            }
            await PerfundoLiferimin();
            await App.Instance.PushAsyncNewPage(new KonfirmoPagesenPage { BindingContext = this });
            UserDialogs.Instance.HideLoading();
        }
        public Artikulli LastKnownArtikulliForScroll { get; set; }

        private async Task PerfundoLiferimin() {
            var porosite = await App.Database.GetPorositeAsync();
            var location = await Geolocation.GetLastKnownLocationAsync();
            if(PagesaType == "ZGJEDH") {
                UserDialogs.Instance.Alert("Ju lutem zgjedhni menyren e pageses para se te vazhdoni");
                return;
            }
            foreach (var porosia in porosite) {
                if (porosia.IDPorosia == IDPorosi) {

                    var numRows = SelectedArikujt.Count;
                    List<Stoqet> StoqetPerUpdate = new List<Stoqet>();
                    var userResult = true;
                    if (userResult) {
                        if (!KthimMalli) {
                            if (numRows > 0) {
                                //Nese keni artikuj te porositur per klientin
                                foreach (var artikull in SelectedArikujt) {
                                    if (ShitjeKorrigjim) {
                                        _llojDok = "KM";
                                    }
                                    else
                                        _llojDok = "SH";
                                    //Update sasia
                                    decimal SasiaUpdate = Math.Round(decimal.Parse(artikull.Sasia.ToString()), 3);
                                    var stoqet = await App.Database.GetAllStoqetAsync();
                                    var stoku = stoqet.FirstOrDefault(x => x.Depo == VizitaESelektuar.IDAgjenti && x.Seri == artikull.Seri);
                                    if (stoku == null) {
                                        stoku = stoqet.FirstOrDefault(x => x.Shifra == artikull.IDArtikulli && x.Depo == VizitaESelektuar.IDAgjenti);
                                        if (stoku == null) {
                                            UserDialogs.Instance.Alert("Mungon stoku, ju lutem sinkronizoni");
                                            return;
                                        }
                                        stoku.Seri = artikull.Seri;
                                    }
                                    decimal sasiaAktuale = Math.Round(decimal.Parse(stoku.Sasia.ToString()), 3);
                                    stoku.Sasia = double.Parse(Math.Round(sasiaAktuale - SasiaUpdate, 3).ToString());
                                    await Task.Delay(20);
                                    await App.Database.UpdateStoqetAsync(stoku);
                                    StoqetPerUpdate.Add(stoku);
                                    //update malli i mbetur
                                    var malliIMbetur = await App.Database.GetMalliMbeturIDAsync(artikull.Seri, VizitaESelektuar.IDAgjenti,artikull.IDArtikulli);
                                    decimal SasiaShiturUpdate = Math.Round(decimal.Parse(artikull.Sasia.ToString()), 3);
                                    decimal SasiaShiturAktuale = Math.Round(decimal.Parse(malliIMbetur.SasiaShitur.ToString()), 3);

                                    malliIMbetur.SasiaShitur = float.Parse(Math.Round(SasiaShiturUpdate + SasiaShiturAktuale, 3).ToString());

                                    //(sasiaPranuar - (SasiaShitur+SasiaKthyer-LevizjeStoku))
                                    var sasiaMbeturString = malliIMbetur.SasiaPranuar - (malliIMbetur.SasiaShitur + malliIMbetur.SasiaKthyer - malliIMbetur.LevizjeStoku);
                                    malliIMbetur.SasiaMbetur = float.Parse(Math.Round(double.Parse(sasiaMbeturString.ToString()), 3).ToString());
                                    malliIMbetur.SyncStatus = 0;
                                    await App.Database.UpdateMalliMbeturAsync(malliIMbetur);
                                    //TODO ID ARSYEJA ID KTHIMI NULL PER SHITJE 
                                    var companyInfo = await App.Database.GetCompanyInfoAsync();
                                    var pArt = new PorosiaArt
                                    {
                                        CmimiAktual = float.Parse(artikull.CmimiNjesi.ToString()),
                                        IDArtikulli = artikull.IDArtikulli,
                                        SasiaPorositur = (float)artikull.Sasia,
                                        Rabatet = 0,
                                        IDPorosia = IDPorosi,
                                        SasiLiferuar = (float)artikull.Sasia,
                                        Emri = artikull.Emri,
                                        Gratis = 0,
                                        SasiaPako = artikull.SasiaPako == null ? (float)artikull.Sasia : (float)artikull.SasiaPako,
                                        DeviceID = VizitaESelektuar.DeviceID,
                                        SyncStatus = 0,
                                        BUM = artikull.BUM,
                                        Seri = artikull.Seri,
                                        IDArsyeja = artikull.ArsyejaEKthimit == App.Instance.MainViewModel.Arsyejet[0].Pershkrimi ? 0 : 1,
                                    };
                                    pArt.CmimiPaTVSH = (float)(pArt.CmimiAktual / 1.2f);
                                    await App.Database.SavePorosiaArtAsync(pArt);
                                }
                            }
                        }
                        else {
                            _llojDok = "KD";
                            TotaliPaguar = 0;
                            if (numRows > 0) // nese ka artikuj t kthyer 
                            {

                                foreach (var artikull in SelectedArikujt) {
                                    decimal SasiaUpdate = Math.Round(decimal.Parse(artikull.Sasia.ToString()), 3);
                                    var stoqet = await App.Database.GetAllStoqetAsync();
                                    var stoku = stoqet.FirstOrDefault(x => x.Depo == VizitaESelektuar.IDAgjenti && x.Seri == artikull.Seri);
                                    if (stoku == null) {
                                        stoku = stoqet.FirstOrDefault(x => x.Shifra == artikull.IDArtikulli && x.Depo == VizitaESelektuar.IDAgjenti);
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
                                        IDPorosia = IDPorosi,
                                        SasiLiferuar = (float)artikull.Sasia,
                                        Emri = artikull.Emri,
                                        Gratis = 0,
                                        SasiaPako = artikull.SasiaPako == null ? (float)artikull.Sasia : (float)artikull.SasiaPako,
                                        DeviceID = VizitaESelektuar.DeviceID,
                                        SyncStatus = 0,
                                        BUM = artikull.BUM,
                                        Seri = artikull.Seri,

                                    };
                                    pArt.CmimiPaTVSH = (float)(pArt.CmimiAktual / 1.2f);
                                    await App.Database.SavePorosiaArtAsync(pArt);
                                }
                            }
                        }

                        var NumratFisk = await App.Database.GetNumratFiskalAsync();
                        NumriFisk NumriFisk = NumratFisk.FirstOrDefault(x=> x.TCRCode == App.Instance.MainViewModel.Configurimi.KodiTCR);
                        if (NumriFisk == null) {
                            var numriFiskalAPIResult = await App.ApiClient.GetAsync("numri-fisk/" + VizitaESelektuar.IDAgjenti);
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

                        TotaliPaguar = decimal.Parse(TotalPrice.ToString());

                        var porosiaSelektuar = await App.Database.GetPorositeIDAsync(porosiaID: IDPorosi);
                        int _nrDetaleve = await App.Database.GetPorositeArtCountAsync(IDPorosi);
                        porosiaSelektuar.NrDetalet = numRows;
                        porosiaSelektuar.NrPorosise = NrFatures.ToString();
                        porosia.Latitude = location?.Latitude.ToString();
                        porosia.Longitude = location?.Longitude.ToString();
                        await App.Database.UpdatePorositeAsync(porosiaSelektuar);
                        var porosiaArt = await App.Database.GetPorositaArtAsync(VizitaESelektuar.IDAgjenti, -1);
                        if (porosiaArt == null) {
                            var listOfPorositeArt = await App.Database.GetPorositeArtAsync();
                            porosiaArt = listOfPorositeArt.FirstOrDefault(x => x.SasiaPorositur < 0 && x.IDArsyeja == 0);
                        }
                        await App.Database.UpdatePorositeArtAsync(porosiaArt);

                        if (!AprovimFaturash) {
                            var NumriFaturave = await App.Database.GetNumriFaturaveIDAsync(VizitaESelektuar.IDAgjenti);
                            NumriFaturave.CurrNrFatJT = NumriFaturave.CurrNrFatJT + 1;
                            await App.Database.UpdateNumriFaturave(NumriFaturave);
                        }
                        else {
                            var NumriFaturave = await App.Database.GetNumriFaturaveIDAsync(VizitaESelektuar.IDAgjenti);
                            NumriFaturave.CurrNrFat = NumriFaturave.CurrNrFat + 1;
                            await App.Database.UpdateNumriFaturave(NumriFaturave);
                        }
                        IDLiferimi = Guid.NewGuid();
                        float totaliFaturesMeTVSH = System.Convert.ToSingle(TotalBillPrice);
                        float TotaliFaturesPaTVH = totaliFaturesMeTVSH / 1.2f; //TODO DIVIDE BY TVSH
                        var agjendet = await App.Database.GetAgjendetAsync();
                        Agjendet agjendi = agjendet.FirstOrDefault(x => x.IDAgjenti == VizitaESelektuar.IDAgjenti);
                        //TODO : FIX FISKALIZIMI KONFIGURIMET FROM API

                        var depot = await App.Database.GetDepotAsync();
                        var FiskalizimiKonfigurimet = await App.Database.GetFiskalizimiKonfigurimetAsync();
                        if(FiskalizimiKonfigurimet.Count <= 0) {
                            var fiskalizimiKonfigurimetResult = await App.ApiClient.GetAsync("fiskalizimi-konfigurimet");
                            if(fiskalizimiKonfigurimetResult.IsSuccessStatusCode) {
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
                        DateTime MyTimeInWesternEurope = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "GMT Standard Time").AddHours(1);

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
                            KohaLiferuar = MyTimeInWesternEurope,
                            TitulliLiferimit = "",
                            DataLiferimit = MyTimeInWesternEurope,
                            KohaLiferimit = MyTimeInWesternEurope,
                            IDPorosia = IDPorosi,
                            Liferuar = 1,
                            NrLiferimit = NrFatures.ToString(),
                            CmimiTotal = totaliFaturesMeTVSH,
                            DeviceID = VizitaESelektuar.DeviceID,
                            SyncStatus = 0,
                            ShumaPaguar = PagesaType == "BANK" ? 0f :  float.Parse(TotalPrice.ToString()),
                            Aprovuar = true,
                            LLOJDOK = _llojDok,
                            PayType = PagesaType,
                            NrDetalet = numRows,
                            IDKlienti = VizitaESelektuar.IDKlientDheLokacion,
                            Depo = VizitaESelektuar.IDAgjenti,
                            Longitude = location?.Longitude.ToString(),
                            Latitude = location?.Latitude.ToString(),
                            IDKthimi = NrFatKthim > 0 ? NrFatKthim.ToString() : null, // TODO KTHIMI FIX IF KTHIM
                            NumriFisk = _NumriFisk,
                            TCR = App.Instance.MainViewModel.Configurimi.KodiTCR,
                            TCROperatorCode = App.Instance.MainViewModel.Configurimi.KodiIOperatorit,
                            TCRBusinessCode = App.Instance.MainViewModel.Configurimi.KodiINjesiseSeBiznesit, // TODO FIND BUSINESSUNITCODE
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
                                CmimiPaTVSH = (float)Math.Round(porosArt.CmimiPaTVSH,2),
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
                            liferimiArt.TotaliPaTVSH = float.Parse(Math.Round(liferimiArt.Totali / 1.2f,2).ToString());
                            liferimiArt.VlefteTVSH = float.Parse(Math.Round(liferimiArt.Totali - liferimiArt.TotaliPaTVSH, 2).ToString());
                            var saveResult = await App.Database.SaveLiferimiArtAsync(liferimiArt);
                            if (saveResult == -1) {
                                UserDialogs.Instance.Alert("Problem ne ruajtjen e liferimit ART tek regjistro");
                            }
                        }

                        VizitaESelektuar.IDStatusiVizites = "6";
                        VizitaESelektuar.DataRealizimit = MyTimeInWesternEurope;
                        VizitaESelektuar.SyncStatus = 0;
                        await App.Database.UpdateVizitaAsync(VizitaESelektuar);
                        App.Instance.MainViewModel.SelectedVizita = null;
                        var NumriFaturaveList = await App.Database.GetNumriFaturaveAsync();
                        var numriFatures = NumriFaturaveList.FirstOrDefault(x => x.KOD == agjendi.IDAgjenti);
                        numriFatures.CurrNrFat = numriFatures.CurrNrFat + 1;
                        if (numriFatures.CurrNrFat > 0)
                        {
                            numriFatures.NRKUFIP += numriFatures.CurrNrFat;
                            numriFatures.CurrNrFat = 0;
                            await App.Database.SaveNumriFaturaveAsync(numriFatures);
                        }

                        try
                        {
                            if (App.Instance.DoIHaveInternetNoAlert())
                            {
                                if (string.IsNullOrEmpty(liferimi.TCRNSLF))
                                {
                                    var response = await App.ApiClient.GetAsync("fatura-e-fiskalizuara/" + NrFatures.ToString());
                                    if (response.IsSuccessStatusCode)
                                    {
                                        var responseString = await response.Content.ReadAsStringAsync();
                                        var FaturatEFiskalizuara = JsonConvert.DeserializeObject<List<FaturatEFiskalizuara>>(responseString);
                                        foreach (var fature in FaturatEFiskalizuara)
                                        {
                                            if (fature.NrLiferimit == NrFatures.ToString() && fature.DeviceID == App.Instance.MainViewModel.Configurimi.Shfrytezuesi)
                                            {
                                                liferimi.TCRNSLF = fature.TCRNSLF;
                                                liferimi.TCRNIVF = fature.TCRNIVF;
                                                await App.Database.UpdateLiferimiAsync(liferimi);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }catch(Exception e)
                        {

                        }
                        

                        
                            
                       
                        //UPDATE EVERYTHING LOCALLY IN ORDER TO SYNC LATER

                        //SYNC MALLI MBETUR DIRECTLY SINCE IT DOESN'T EXIST IN SYNC ALL 
                        if(liferimi.PayType == "KESH") {
                            EvidencaPagesave evidencaPagesave = new EvidencaPagesave
                            {
                                Borxhi = 0,
                                DataPageses = MyTimeInWesternEurope,
                                DataPerPagese = MyTimeInWesternEurope,
                                DeviceID = agjendi.DeviceID,
                                ExportStatus = 0,
                                IDAgjenti = agjendi.IDAgjenti,
                                IDKlienti = VizitaESelektuar.IDKlientDheLokacion,
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
                                IDKlienti = VizitaESelektuar.IDKlientDheLokacion,
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

                        ////FISKALIZIMIF
                        //FiskalizimiService bc = new FiskalizimiService();
                        try {
                            await FiskalizoTCRInvoice(IDLiferimi.ToString());
                        }
                        catch (Exception ex) {

                        }

                        if (liferimi.TCRSyncStatus == null)
                            liferimi.TCRSyncStatus = -1;
                    }
                }
            }
        }

        public async Task DaljeAsync() {
            UserDialogs.Instance.ShowLoading("Duke u kthyer te vizitat");
            App.Instance.MainPage = new NavigationPage( new MainPage { BindingContext = App.Instance.MainViewModel });
            await App.Instance.PushAsyncNewPage(new ClientsPage() { BindingContext = App.Instance.MainViewModel });
            App.Instance.MainViewModel.VizitatFilteredByDate.Remove(VizitaESelektuar);
            App.Instance.MainViewModel.VizitatFilteredByDate.Insert(0, VizitaESelektuar);
            App.Instance.MainViewModel.SearchedVizitat.Remove(VizitaESelektuar);
            App.Instance.MainViewModel.SearchedVizitat.Insert(0, VizitaESelektuar);
            UserDialogs.Instance.HideLoading();
        }


        public async Task FiskalizoTCRInvoice(string idLiferimi) {
            var liferimet = await App.Database.GetLiferimetAsync();
            var liferimi = liferimet.FirstOrDefault(x => x.IDLiferimi == IDLiferimi);
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

            List<MapperHeader> mapperHeaderList = query.ToList();
            if (liferimi != null)
            {
                if (liferimi.TotaliPaTVSH != mapperHeaderList[0].TotaliPaTVSH)
                {
                    liferimi.TotaliPaTVSH = float.Parse(mapperHeaderList[0].TotaliPaTVSH.ToString());
                }
                if(liferimi.CmimiTotal != mapperHeaderList[0].TotaliMeTVSH)
                {
                    liferimi.CmimiTotal = float.Parse(mapperHeaderList[0].TotaliMeTVSH.ToString());
                }
                if(liferimi.PayType != "BANK")
                {
                    if (liferimi.ShumaPaguar != mapperHeaderList[0].TotaliMeTVSH)
                    {
                        liferimi.ShumaPaguar = float.Parse(mapperHeaderList[0].TotaliMeTVSH.ToString());
                    }
                }
                await App.Database.SaveLiferimiAsync(liferimi);
            }
            if (mapperHeaderList.Count > 0) {
                var klientet = await App.Database.GetKlientetAsync();
                var Klientdhelokacion = await App.Database.GetKlientetDheLokacionetAsync();
                var artikujt = await App.Database.GetArtikujtAsync();
                var companyInfo = await App.Database.GetCompanyInfoAsync();
                var query2 = from l in liferimet
                             join la in liferimetArt on l.IDLiferimi equals la.IDLiferimi
                             join k in klientet on l.IDKlienti equals k.IDKlienti
                             join kl in Klientdhelokacion on k.IDKlienti equals kl.IDKlienti
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
                                 IssueDateTimeRef = DateTime.Now,
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
                        req.BusinessUnitCode = App.Instance.MainViewModel.Configurimi.KodiINjesiseSeBiznesit;
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


                        try {
                            if (!App.Instance.MainViewModel.Configurimi.VetemPerPorosi)
                            {
                                ResultLogPCL log = App.Instance.FiskalizationService.RegisterInvoice(req);
                                if (log == null)
                                {
                                    liferimet = await App.Database.GetLiferimetAsync();
                                    liferimetArt = await App.Database.GetLiferimetArtAsync();
                                    var liferimiToUpdate = liferimet
                                                        .FirstOrDefault(l => l.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                                    if (liferimiToUpdate != null)
                                    {
                                        liferimiToUpdate.TCRSyncStatus = -1;
                                        liferimiToUpdate.TCRIssueDateTime = DateTime.Now;
                                        liferimiToUpdate.TCRQRCodeLink = null;
                                        liferimiToUpdate.TCR = App.Instance.MainViewModel.Configurimi.KodiTCR;
                                        liferimiToUpdate.TCROperatorCode = LoginData.OperatorCode;
                                        liferimiToUpdate.TCRBusinessCode = liferimiToUpdate.TCRBusinessCode;
                                        liferimiToUpdate.UUID = null;
                                        liferimiToUpdate.EIC = null;
                                        try
                                        {
                                            if (App.Instance.DoIHaveInternetNoAlert())
                                            {

                                                var response = await App.ApiClient.GetAsync("fatura-e-fiskalizuara/" + NrFatures.ToString());
                                                if (response.IsSuccessStatusCode)
                                                {
                                                    var responseString = await response.Content.ReadAsStringAsync();
                                                    var FaturatEFiskalizuara = JsonConvert.DeserializeObject<List<FaturatEFiskalizuara>>(responseString);
                                                    foreach (var fature in FaturatEFiskalizuara)
                                                    {
                                                        if (fature.NrLiferimit == NrFatures.ToString() && fature.DeviceID == App.Instance.MainViewModel.Configurimi.Shfrytezuesi)
                                                        {
                                                            liferimiToUpdate.TCRNSLF = fature.TCRNSLF;
                                                            liferimiToUpdate.TCRNIVF = fature.TCRNIVF;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception e)
                                        {

                                        }

                                        liferimiToUpdate.Message = "Komunikimi me service ka deshtuar shkaku pajisja nuk ka qen e konektuar me rrjet";

                                        await App.Database.SaveLiferimiAsync(liferimiToUpdate);

                                        var liferimiArtToUpdate = liferimetArt
                                                .Where(la => la.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                                        foreach (var art in liferimiArtToUpdate)
                                        {
                                            art.TCRSyncStatus = -1;
                                            await App.Database.UpdateLiferimiArtAsync(art);
                                        }
                                    }
                                    return;
                                }
                                if (log.Status == StatusPCL.Ok)
                                {
                                    liferimet = await App.Database.GetLiferimetAsync();
                                    liferimetArt = await App.Database.GetLiferimetArtAsync();
                                    var liferimiToUpdate = liferimet
                                                        .FirstOrDefault(l => l.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                                    if (liferimiToUpdate != null)
                                    {
                                        liferimiToUpdate.TCRSyncStatus = 1;
                                        liferimiToUpdate.TCRIssueDateTime = DateTime.Now;
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

                                        foreach (var art in liferimiArtToUpdate)
                                        {
                                            art.TCRSyncStatus = 1;
                                            await App.Database.UpdateLiferimiArtAsync(art);
                                        }
                                    }
                                }
                                else if (log.Status == StatusPCL.FaultCode)
                                {
                                    if (string.IsNullOrEmpty(log.Message))
                                    {
                                        liferimet = await App.Database.GetLiferimetAsync();
                                        liferimetArt = await App.Database.GetLiferimetArtAsync();
                                        var liferimiToUpdate = liferimet
                                                            .FirstOrDefault(l => l.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                                        if (liferimiToUpdate != null)
                                        {
                                            liferimiToUpdate.TCRSyncStatus = -1;
                                            liferimiToUpdate.TCRIssueDateTime = DateTime.Now;
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

                                            foreach (var art in liferimiArtToUpdate)
                                            {
                                                art.TCRSyncStatus = -1;
                                                await App.Database.UpdateLiferimiArtAsync(art);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        liferimet = await App.Database.GetLiferimetAsync();
                                        liferimetArt = await App.Database.GetLiferimetArtAsync();
                                        var liferimiToUpdate = liferimet
                                                            .FirstOrDefault(l => l.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                                        if (liferimiToUpdate != null)
                                        {
                                            liferimiToUpdate.TCRSyncStatus = -1;
                                            liferimiToUpdate.TCRIssueDateTime = DateTime.Now;
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

                                            foreach (var art in liferimiArtToUpdate)
                                            {
                                                art.TCRSyncStatus = -1;
                                                await App.Database.UpdateLiferimiArtAsync(art);
                                            }
                                        }
                                    }
                                }
                                else if (log.Status == StatusPCL.TCRAlreadyRegistered)
                                {
                                    liferimet = await App.Database.GetLiferimetAsync();
                                    liferimetArt = await App.Database.GetLiferimetArtAsync();
                                    var liferimiToUpdate = liferimet
                                        .Where(l => l.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                                    foreach (var liferi in liferimiToUpdate)
                                    {
                                        liferi.TCRSyncStatus = 4;
                                        liferi.TCRIssueDateTime = DateTime.Now;
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

                                    foreach (var art in liferimiArtToUpdate)
                                    {
                                        art.TCRSyncStatus = 4;
                                        await App.Database.UpdateLiferimiArtAsync(art);
                                    }
                                }
                                else if (log.Status == StatusPCL.InProcess)
                                {
                                    liferimet = await App.Database.GetLiferimetAsync();
                                    liferimetArt = await App.Database.GetLiferimetArtAsync();
                                    var liferimiToUpdate = liferimet
                                        .Where(l => l.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                                    foreach (var liferi in liferimiToUpdate)
                                    {
                                        liferi.TCRSyncStatus = -2;
                                        liferi.TCRIssueDateTime = DateTime.Now;
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

                                    foreach (var art in liferimiArtToUpdate)
                                    {
                                        art.TCRSyncStatus = -2;
                                        await App.Database.UpdateLiferimiArtAsync(art);
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        liferimet = await App.Database.GetLiferimetAsync();
                                        liferimetArt = await App.Database.GetLiferimetArtAsync();
                                        var liferimiToUpdate = liferimet
                                            .Where(l => l.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                                        foreach (var lif in liferimiToUpdate)
                                        {
                                            lif.TCRSyncStatus = -3;
                                            lif.TCRIssueDateTime = DateTime.Now;
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

                                        foreach (var art in liferimiArtToUpdate)
                                        {
                                            art.TCRSyncStatus = -1;
                                            await App.Database.UpdateLiferimiArtAsync(art);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        // Handle exception
                                    }
                                }
                            }
                        }
                        catch (Exception e) {

                        }
                        
                    }
                }
            }
        }


        public async Task RegjistroAsync() {
            DateTime MyTimeInWesternEurope = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "GMT Standard Time").AddHours(1);

            App.Instance.MainViewModel.TodaysDate = MyTimeInWesternEurope;
            await App.Instance.MainViewModel.FixDataVizualizimit();
            //await App.Instance.MainPage.Navigation.PushAsync(new PrinterSelectionPage() { BindingContext = this });


            if (SelectedArikujt == null || SelectedArikujt.Count <= 0) {
                UserDialogs.Instance.Alert("Nuk mund te vazhdohet me faturim pa artikuj te zgjedhur");
                return;
            }
            if (KthimMalli) {
                if(NrFatKthim <= 0) {
                    UserDialogs.Instance.Alert(@"Duhet të plotësohet fusha ""Nr. Fat. Kthim"" Kjo është fushë obligative për fiskalizim!", "Vërejtje");
                    return;
                }
                var vizitat = await App.Database.GetVizitatAsync();
                var klientet = await App.Database.GetKlientetAsync();
                var liferimi = await App.Database.GetLiferimetAsync();
                var nipt = (from v in vizitat
                            join k in klientet on v.IDKlientDheLokacion equals k.IDKlienti
                            where v.IDVizita == VizitaESelektuar.IDVizita
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
                NumriFisk NumriFisk = await App.Database.GetNumratFiskalIDAsync(VizitaESelektuar.IDAgjenti);
                if (NumriFisk == null) {
                    var numriFiskalAPIResult = await App.ApiClient.GetAsync("numri-fisk/" + VizitaESelektuar.IDAgjenti);
                    if (numriFiskalAPIResult.IsSuccessStatusCode) {
                        var numriFiskalResponse = await numriFiskalAPIResult.Content.ReadAsStringAsync();
                        NumriFisk = JsonConvert.DeserializeObject<NumriFisk>(numriFiskalResponse);
                        await App.Database.SaveNumriFiskalAsync(NumriFisk);
                        numriFiskal.Add(NumriFisk);
                    }
                    else {
                        UserDialogs.Instance.Alert("Problem ne numrin fiskal, ju lutem provoni perseri.");
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
                //LOKALE nrCount
                //fiskServ serv
                var fiskServ = FiscalisationService.CheckCorrectiveInvoice(NrFatKthim.ToString().Trim(), agjendi.Depo, App.Instance.MainViewModel.Configurimi.KodiTCR, App.Instance.MainViewModel.Configurimi.KodiIOperatorit, App.Instance.MainViewModel.Configurimi.KodiINjesiseSeBiznesit, nipt);
                if (nrCount == 0 && FiscalisationService.CheckCorrectiveInvoice(NrFatKthim.ToString().Trim(), agjendi.Depo, App.Instance.MainViewModel.Configurimi.KodiTCR, App.Instance.MainViewModel.Configurimi.KodiIOperatorit, App.Instance.MainViewModel.Configurimi.KodiINjesiseSeBiznesit, nipt) <= 0) {
                    UserDialogs.Instance.Alert(@"Fusha ""Nr. Fat. Kthim"" nuk është i sakt, ju lutemi rishikoni edhe njëherë!", "Verejtje");
                    return;
                }
            }
            else if (ShitjeKorrigjim){
                if (NrFatKthim <= 0) {
                    UserDialogs.Instance.Alert(@"Duhet të plotësohet fusha ""Nr. Fat. Kthim"" Kjo është fushë obligative për fiskalizim!", "Vërejtje");
                    return;
                }
                var vizitat =await App.Database.GetVizitatAsync();
                var klientet = await App.Database.GetKlientetAsync();
                var liferimi = await App.Database.GetLiferimetAsync();
                var nipt = (from v in vizitat
                            join k in klientet on v.IDKlientDheLokacion equals k.IDKlienti
                            where v.IDVizita == VizitaESelektuar.IDVizita
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
                NumriFisk NumriFisk = await App.Database.GetNumratFiskalIDAsync(VizitaESelektuar.IDAgjenti);
                if (NumriFisk == null) {
                    var numriFiskalAPIResult = await App.ApiClient.GetAsync("numri-fisk/" + VizitaESelektuar.IDAgjenti);
                    if (numriFiskalAPIResult.IsSuccessStatusCode) {
                        var numriFiskalResponse = await numriFiskalAPIResult.Content.ReadAsStringAsync();
                        NumriFisk = JsonConvert.DeserializeObject<NumriFisk>(numriFiskalResponse);
                        await App.Database.SaveNumriFiskalAsync(NumriFisk);
                        numriFiskal.Add(NumriFisk);
                    }
                    else {
                        UserDialogs.Instance.Alert("Problem ne numrin fiskal, ju lutem provoni perseri.");
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

                if (nrCount == 0 && FiscalisationService.CheckCorrectiveInvoice(NrFatKthim.ToString().Trim(), agjendi.Depo, App.Instance.MainViewModel.Configurimi.KodiTCR, App.Instance.MainViewModel.Configurimi.KodiIOperatorit, App.Instance.MainViewModel.Configurimi.KodiINjesiseSeBiznesit, nipt) <= 0) {
                    UserDialogs.Instance.Alert(@"Fusha ""Nr. Fat. Kthim"" nuk është i sakt, ju lutemi rishikoni edhe njëherë!", "Verejtje");
                    return;
                }
            }

            if (VizitaESelektuar.DataAritjes == null)
                VizitaESelektuar.DataAritjes = MyTimeInWesternEurope;
            var FaturaArritjes = VizitaESelektuar.DataAritjes;
            
            if(FaturaArritjes != null) {
                DateTime dp = MyTimeInWesternEurope;
                DateTime dl = MyTimeInWesternEurope;
                DateTime KohaPorosise = MyTimeInWesternEurope.ToLocalTime();
                //update porosite
                var porosite = await App.Database.GetPorositeAsync();
                var location = await Geolocation.GetLastKnownLocationAsync();
                Porosite porosite1 = new Porosite
                {
                    IDPorosia = IDPorosi,
                    IDVizita = VizitaESelektuar.IDVizita,
                    NrPorosise = NrFatures.ToString(),
                    DataPerLiferim = MyTimeInWesternEurope,
                    DataPorosise = MyTimeInWesternEurope,
                    StatusiPorosise = 1,
                    DeviceID = VizitaESelektuar.DeviceID,
                    NrDetalet = SelectedArikujt.Count(),
                    Longitude = location?.Longitude.ToString(),
                    Latitude = location?.Latitude.ToString(),
                    SyncStatus = 0,
                    OraPorosise = MyTimeInWesternEurope,
                    TitulliPorosise = null,
                };
                //SEND POROSIA TO API SO 
                await App.Database.SavePorositeAsync(porosite1);
                var userResult = await UserDialogs.Instance.ConfirmAsync("Jeni te sigurte per mbylljen e fatures?", "Verejtje", "Po", "Jo");
                if (userResult) {
                    await App.Instance.PushAsyncNewPage(new PagesaPage() { BindingContext = this });
                }
            }
        }
        

        async Task<MPosControllerDevices> OpenPrinterService(MposConnectionInformation connectionInfo) {
            if (connectionInfo == null)
                return null;

            if (_printer != null)
                return _printer;

            if(_printSemaphore == null) {
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
        public MPosControllerPrinter _printer;
        public MposConnectionInformation _connectionInfo;
        public static SemaphoreSlim _printSemaphore = new SemaphoreSlim(1, 1);

        public async Task OnDeviceOpenClicked() {
            try {
                if (_printer == null) {
                    // Prepares to communicate with the printer 
                    _printer = await OpenPrinterService(_connectionInfo) as MPosControllerPrinter;
                    await OnPrintTextClicked();
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
                await _printer.printText("\nF A T U R E    T A T I M O R E   SH I T J E \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_CENTER, Bold = false, });
                await _printer.printLine(0, 0, 1, 1, 1);
                await _printer.printText(
"---------------------------------------------------------------------\n");

                var depot = await App.Database.GetDepotAsync();
                var currDepo = depot.FirstOrDefault(x => x.Depo == LoginData.Depo);
                await _printer.printText("Shitesi: E. H. W.          J61804031V \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });
                await _printer.printText("Tel: 048 200 711           web: www.ehwgmbh.com \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });
                await _printer.printText("Adresa: " + App.Instance.MainViewModel.Configurimi.TAGNR + "             \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });
                await _printer.printText("Qyteti / Shteti: Tirana, Albania \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });
                await _printer.printText("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");
                var liferimi = await App.Database.GetLiferimetAsync();
                var lif = liferimi.FirstOrDefault(x => x.IDLiferimi == IDLiferimi);
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
                DateTime MyTimeInWesternEurope = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "GMT Standard Time").AddHours(1);

                await _printer.printText("\nNumri i fatures: " + lif.NumriFisk + "/" + lif.DataLiferimit.Year);
                await _printer.printText("\nData dhe ora e leshimit te fatures: " + lif.DataLiferimit.ToString("dd.MM.yyyy HH:mm:ss"));
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
                await _printer.printText("\nBleresi: " + klienti.Emri + " " +  klienti.NIPT);
                await _printer.printText("\nAdresa: " + klientiDheLokacioni.Adresa + "       \n");

                await _printer.printText(
"---------------------------------------------------------------------");
                var agjendet = await App.Database.GetAgjendetAsync();
                var currAgjendi = agjendet.FirstOrDefault(x => x.Depo == LoginData.Depo);
                await _printer.printText("\nTransportues: E. H. W. J61804031V");
                await _printer.printText("\nAdresa: " + currDepo.TAGNR + "  (" + currAgjendi.Emri + " " + currAgjendi.Mbiemri + ")");
                await _printer.printText("\nData dhe ora e furinizimit: " + lif.DataLiferimit.ToString("dd.MM.yyyy HH:mm:ss") + "  \n");

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

                foreach (var art in SelectedArikujt) {
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
                await _printer.printText("Bleresi                                          Shitesi");
                var klientiLength = klienti.Emri.Trim();
                var agjendiLengh = currAgjendi.Emri + " " + currAgjendi.Mbiemri;
                var differenceBetweenTheTwo = 60 - agjendiLengh.Length - klientiLength.Length;
                var emptyString = String.Empty;
                for (int i = 0; i < differenceBetweenTheTwo; i++) {
                    emptyString += " ";
                }

                await _printer.printText("\n" + klienti.Emri.Trim() + emptyString + currAgjendi.Emri + " " + currAgjendi.Mbiemri);
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


        private double _TotalBillPrice;
        public double TotalBillPrice {
            get { return _TotalBillPrice; }
            set { SetProperty(ref _TotalBillPrice, value); }
        }
        
        private double _totalPrice;
        public double TotalPrice {
            get { return _totalPrice; }
            set { SetProperty(ref _totalPrice, value); }
        }

        public bool WaitForArsyejenEKthimit { get; set; }
        public async Task AddArtikulliAsync() {
            if(SelectedArikujt == null) {
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

            if (!KthimMalli) {
                if (Sasia <= 0) {
                    UserDialogs.Instance.Alert("Ju lutemi permiresoni sasin, duhet te jete me shume se 0.00", "Verejtje", "Ok");
                    return;
                }
            }
            else {
                if(Sasia >= 0) {
                    UserDialogs.Instance.Alert("Ju lutemi permiresoni sasin, duhet te jete me e vogel se 0.00", "Verejtje", "Ok");
                    return;
                }
            }
            if(CurrentlySelectedArtikulli.BUM != "KG") {
                if(Sasia % 1 != 0) {
                    UserDialogs.Instance.Alert("Ju lutemi permiresoni sasin, arikulli lejon vetem numra te plote pasi qe eshte " + CurrentlySelectedArtikulli.BUM);
                    Sasia = (float)(int)(Sasia);
                    return;
                }
            }
            if(Sasia > CurrentlySelectedArtikulli.Sasia) {
                UserDialogs.Instance.Alert("Ju lutemi permiresoni sasin, nuk duhet te jete me shume se sasia aktualle e mallit " + CurrentlySelectedArtikulli.Sasia, "Verejtje", "Ok");
                Sasia = (float)CurrentlySelectedArtikulli.Sasia;
                return;
            }
            if(KthimMalli) {
                if(string.IsNullOrEmpty(CurrentlySelectedArtikulli.ArsyejaEKthimit)) {
                    await App.Instance.MainPage.Navigation.PushPopupAsync(new ArsyejaKthimitPopup() { BindingContext = this }, true);
                    WaitForArsyejenEKthimit = false;
                    while (!WaitForArsyejenEKthimit) {
                        await Task.Delay(250);
                    }
                }
            }
            var artikulli = Artikujt.FirstOrDefault(x => x.IDArtikulli == CurrentlySelectedArtikulli.IDArtikulli);
            var malliMbeturi = MalliMbetur.Where(x => x.IDArtikulli == CurrentlySelectedArtikulli.IDArtikulli).ToList();
            Malli_Mbetur malliMbetur = null;
            if(malliMbeturi.Count > 1) {
                foreach(var mall in malliMbeturi) {
                    if(!string.IsNullOrEmpty(mall.Seri)) {
                        if (mall.Seri == CurrentlySelectedArtikulli.Seri)
                            malliMbetur = mall;
                    }
                }
                if(malliMbetur == null && malliMbeturi.Count > 0) {
                    malliMbetur = malliMbeturi.FirstOrDefault();
                }
            }
            if (malliMbetur == null && malliMbeturi.Count > 0) {
                malliMbetur = malliMbeturi.FirstOrDefault();
            }
            if (malliMbetur == null) {
                UserDialogs.Instance.Alert("Nuk gjindet mall i mbetur per kete artikull, ju lutemi beni sinkronizimin perseri");
                return;
            }
            if (artikulli != null) {
                artikulli.Seri = CurrentlySelectedArtikulli.Seri;
                malliMbetur.Seri = CurrentlySelectedArtikulli.Seri;
                await App.Database.UpdateArtikulliAsync(artikulli);
                await App.Database.UpdateMalliMbeturAsync(malliMbetur);
            }
            if(SelectedArikujt.FirstOrDefault(x=> x.IDArtikulli == CurrentlySelectedArtikulli.IDArtikulli) != null) {
                UserDialogs.Instance.Alert("Artikulkli " + CurrentlySelectedArtikulli.Emri + " eshte shtuar njehere, nese eshte bere gabim ne sasi, ju lutem fshini artikullin dhe shtoni perseri.", "Verejtje", "Ok");
                CurrentlySelectedArtikulli = null;
                Sasia = 0;
                if (KthimMalli)
                    Sasia = -1;
                return;
            }
            CurrentlySelectedArtikulli.SasiaMax = (float)CurrentlySelectedArtikulli.Sasia;
            CurrentlySelectedArtikulli.Sasia = Sasia;
            SelectedArikujt.Add(CurrentlySelectedArtikulli);
            if(!KthimMalli)
                TotalPrice = Math.Abs(Math.Round((double)(TotalPrice + CurrentlySelectedArtikulli.CmimiTotal),2));
            else
                TotalPrice = Math.Round((double)(TotalPrice + CurrentlySelectedArtikulli.CmimiTotal), 2);
            TotalBillPrice = TotalPrice;
            CurrentlySelectedArtikulli = null;
            Sasia = 0;
            if (KthimMalli)
                Sasia = -1;
        }

        public void IncreaseSasia() {
            if(KthimMalli) {
                if (Sasia >= 0) {
                    UserDialogs.Instance.Alert("Gabim ne sasi", "Verejtje");
                    Sasia = -1;
                    return;
                }
            }
            Sasia = Sasia + 1;
        }
        public void DecreaseSasia() {
            if (Sasia == 0) {
                if(KthimMalli) {
                    Sasia = Sasia - 1;
                }
                else
                    return;
            }
            Sasia = Sasia - 1;

        }
        public async Task ZgjedhArtikullinAsync() {
            try {
                UserDialogs.Instance.ShowLoading("Duke hapur artikujt");
                ZgjidhArtikullinModalPage zgjidhArtikullinModalPage = new ZgjidhArtikullinModalPage();
                zgjidhArtikullinModalPage.BindingContext = this;
                Artikujt = new ObservableCollection<Artikulli>(await App.Database.GetArtikujtAsync());

                var malliMbetur = await App.Database.GetMalliMbeturAsync();
                MalliMbetur = new ObservableCollection<Malli_Mbetur>(malliMbetur);
                
                SalesPrices = new ObservableCollection<SalesPrice>( await App.Database.GetSalesPriceAsync());
                //SalesPrices = new ObservableCollection<SalesPrice>(await App.Database.GetSalesPriceAsync());
                //MalliMbetur = new ObservableCollection<Malli_Mbetur>(await App.Database.GetMalliMbeturAsync());
                var salePrice = SalesPrices?.FirstOrDefault(x => x.SalesCode == VizitaESelektuar.IDKlientDheLokacion)?.UnitPrice;
                //if(App.Instance.DoIHaveInternetNoAlert()) {
                //    var salesPricesResult = await App.ApiClient.GetAsync("prices/" + VizitaESelektuar.IDKlientDheLokacion);
                //    //var salesPricesResult = await App.ApiClient.GetAsync("prices");
                //    if (salesPricesResult.IsSuccessStatusCode) {
                //        var salesPricesResponse = await salesPricesResult.Content.ReadAsStringAsync();
                //        var SalePrice = JsonConvert.DeserializeObject<List<SalesPrice>>(salesPricesResponse);
                //        SalesPrices = new ObservableCollection<SalesPrice>(SalePrice);
                //        await App.Database.SaveSalesPricesAsync(SalePrice);
                //    }
                //}
                var artikujtPerShfaqje = new ObservableCollection<Artikulli>();
                var SalePrice = new List<SalesPrice>();
                foreach (var artikulli in Artikujt) {
                    var salesPrice = SalesPrices.FirstOrDefault(x => x.ItemNo == artikulli.IDArtikulli && x.SalesCode == VizitaESelektuar.IDKlientDheLokacion);
                    if(salesPrice == null) {
                        if (App.Instance.DoIHaveInternetNoAlert()) {
                            var salesPricesResult = await App.ApiClient.GetAsync("prices/" + VizitaESelektuar.IDKlientDheLokacion);
                            //var salesPricesResult = await App.ApiClient.GetAsync("prices"); 
                            if (salesPricesResult.IsSuccessStatusCode) {
                                var salesPricesResponse = await salesPricesResult.Content.ReadAsStringAsync();
                                SalePrice = JsonConvert.DeserializeObject<List<SalesPrice>>(salesPricesResponse);
                                SalesPrices = new ObservableCollection<SalesPrice>(SalePrice);
                                salesPrice = SalesPrices.FirstOrDefault(x => x.ItemNo == artikulli.IDArtikulli && x.SalesCode == VizitaESelektuar.IDKlientDheLokacion);
                            }
                        }
                    }
                    artikulli.CmimiNjesi = salesPrice?.UnitPrice;
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
                        var MalliiMbetur = MalliMbetur.FirstOrDefault(x => x.IDArtikulli == artikulli.IDArtikulli && x.Depo == LoginData.IDAgjenti);
                        if (MalliiMbetur != null) {
                                artikulli.Sasia = MalliiMbetur.SasiaMbetur;
                                artikulli.Seri = MalliiMbetur.Seri;
                                artikujtPerShfaqje.Add(artikulli);
                        }
                    }
                }
                if(!KthimMalli) {
                    Artikujt = new ObservableCollection<Artikulli>(artikujtPerShfaqje.Where(x => x.Sasia > 0 && x.CmimiNjesi != null));
                }
                else {
                    Artikujt = new ObservableCollection<Artikulli>(artikujtPerShfaqje);
                }
                await App.Instance.PushAsyncNewModal(zgjidhArtikullinModalPage);
                if(SalePrice.Count > 0) {
                    App.Database.SaveSalesPricesAsync(SalePrice);
                }
                UserDialogs.Instance.HideLoading();

            }
            catch (Exception e) {
                var g = e.Message;
                UserDialogs.Instance.HideLoading();
            }

        }
    }
}
