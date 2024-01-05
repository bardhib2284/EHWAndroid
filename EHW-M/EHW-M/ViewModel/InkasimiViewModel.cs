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
using Xamarin.Forms;

namespace EHWM.ViewModel {

    public class InkasimiViewModelNavigationParameters {
        public List<Klientet> Klientet;
        public List<Detyrimet> Detyrimet;
        public List<EvidencaPagesave> EvidencaPagesave;
        public Agjendet Agjendi;
    }

    public class InkasimiViewModel : BaseViewModel{
        public List<Klientet> Klientet { get; set; }
        public List<Detyrimet> Detyrimet { get; set; }
        public List<EvidencaPagesave> EvidencaEPagesave { get; set; }
        public Agjendet Agjendi;

        public DateTime TodaysDate => DateTime.Now;

        public ICommand RegjistroCommand { get; set; }
        public InkasimiViewModel(InkasimiViewModelNavigationParameters inkasimiViewModelNavigationParameters) {
            Klientet = inkasimiViewModelNavigationParameters.Klientet.OrderBy(x=> x.Emri).ToList();
            Detyrimet = inkasimiViewModelNavigationParameters.Detyrimet;
            EvidencaEPagesave = inkasimiViewModelNavigationParameters.EvidencaPagesave;
            DetyrimetNgaKlienti = new ObservableCollection<Detyrimet>();
            RegjistroCommand = new Command(RegjistroAsync);
            Agjendi = inkasimiViewModelNavigationParameters.Agjendi;
        }

        public ObservableCollection<Detyrimet> DetyrimetNgaKlienti { get; set; }
        private decimal _shumaPaguar;
        public decimal ShumaPaguar {
            get { return _shumaPaguar; }
            set { SetProperty(ref _shumaPaguar, value); }
        }
        
        private string _PayType;
        public string PayType {
            get { return _PayType; }
            set { SetProperty(ref _PayType, value); }
        }        
        private string _coinType;
        public string CoinType {
            get { return _coinType; }
            set { SetProperty(ref _coinType, value); }
        }
        private ObservableCollection<Klientet> _SearchedKlientet;
        public ObservableCollection<Klientet> SearchedKlientet {
            get { return _SearchedKlientet; }
            set { SetProperty(ref _SearchedKlientet, value); }
        }
        private Klientet _selectedKlient;
        public Klientet SelectedKlient {
            get { return _selectedKlient; }
            set { SetProperty(ref _selectedKlient, value); }
        }
        public async Task ZgjedhKlientet() {
            try {
                UserDialogs.Instance.ShowLoading("Duke hapur klientet");
                ZgjidhKlientetModalPage ZgjidhKlientetModalPage = new ZgjidhKlientetModalPage();
                ZgjidhKlientetModalPage.BindingContext = this;

                await App.Instance.PushAsyncNewModal(ZgjidhKlientetModalPage);
                UserDialogs.Instance.HideLoading();

            }
            catch (Exception e) {
                var g = e.Message;
            }
        }
        public async void RegjistroAsync() {
            if(DetyrimetNgaKlienti.Count < 1) {
                return;
            }
            else {
                try {
                    DateTime n = DateTime.Now;
                    string hr = n.Hour.ToString();
                    string min = n.Minute.ToString();
                    string sec = n.Second.ToString();

                    //ie. if Hour is 4 then 04; if minute is 3 then 03             
                    if (hr.Length == 1)
                        hr = "0" + hr;
                    if (min.Length == 1)
                        min = "0" + min;
                    if (sec.Length == 1)
                        sec = "0" + sec;

                    string PagesatID = Agjendi.DeviceID + "-|-" + n.Year.ToString() + n.Month.ToString() + n.Day.ToString() +
                            hr + min + sec;
                    decimal vlera = ((Math.Round(decimal.Parse(ShumaPaguar.ToString()), 2)));
                    //var query = from ep in EvidencaEPagesave
                    //            join k in Klientet on ep.IDKlienti equals k.IDKlienti
                    //            join d in Detyrimet on ep.IDKlienti equals d.KOD
                    //            where ep.NrFatures == null && ep.DeviceID == Agjendi.DeviceID
                    //            select new
                    //            {
                    //                ep.IDKlienti,
                    //                Klienti = k.Emri,
                    //                Detyrimi = decimal.Round(d.Detyrimi, 2),
                    //                Paguar = decimal.Round(ep.ShumaPaguar, 2),
                    //                ep.KMON,
                    //                ep.NrPageses
                    //            };
                    var nrFatures = string.Empty;
                    if(EvidencaEPagesave.Count > 1) {
                        await App.Database.DeleteEvidencaPagesave(EvidencaEPagesave.FirstOrDefault(x => x.NrFatures == ""));
                        var query = EvidencaEPagesave
                                 .OrderByDescending(ep => ep.DataPageses)
                                 .Select(ep => new
                                 {
                                     NrFatures = ep.NrFatures != null ? ep.NrFatures : "",
                                     ep.NrPageses
                                 })
                                 .FirstOrDefault();
                        if (query != null) {
                            if(!string.IsNullOrEmpty(query.NrFatures)) {
                                var currNrFatures = int.Parse(query.NrFatures.Split('/')[0]);
                                currNrFatures = currNrFatures + 1;
                                nrFatures = currNrFatures.ToString() + "/" + query.NrFatures.Split('/')[1];
                            }
                        }
                    }
                    

                    var EvidencaPagesave = new EvidencaPagesave
                    {
                        NrFatures = nrFatures,
                        NrPageses = PagesatID,
                        ShumaPaguar = float.Parse(vlera.ToString()),
                        DataPageses = TodaysDate,
                        IDAgjenti = Agjendi.IDAgjenti,
                        IDKlienti = DetyrimetNgaKlienti.FirstOrDefault().KOD,
                        DeviceID = Agjendi.DeviceID,
                        SyncStatus = 0,
                        PayType = PayType,
                        KMON = "LEK",
                        ExportStatus = 0,
                        DataPerPagese = TodaysDate,
                        ShumaTotale = DetyrimetNgaKlienti.FirstOrDefault().Detyrimi
                    };
                    EvidencaPagesave.Borxhi = EvidencaPagesave.ShumaTotale - EvidencaPagesave.ShumaPaguar;
                    DetyrimetNgaKlienti.FirstOrDefault().Detyrimi = (float)EvidencaPagesave.Borxhi;
                    await App.Database.SaveOrUpdateDetyrimi(DetyrimetNgaKlienti.FirstOrDefault());

                    var result = await App.Database.SaveEvidencaPagesaveAsync(EvidencaPagesave);
                    if(result != -1) {
                        var shtypja = await UserDialogs.Instance.ConfirmAsync(" Inkasimi përfundoi me sukses \n Dëshironi të shtypni fletëpagesën ?", "Shtypja", "Po", "Jo");
                        if(shtypja) {
                            await App.Instance.PushAsyncNewPage(new PrinterSelectionPage());
                        }
                    }
                }catch(Exception e) {

                }
            }
        }

        public void MerrDetyrimet(Klientet klientiSelektuar) {
            DetyrimetNgaKlienti.Clear();
            var detyrimi = (Detyrimet.FirstOrDefault(x=> x.KOD == klientiSelektuar.IDKlienti));
            if (detyrimi == null)
                return;
            else
                DetyrimetNgaKlienti.Add(detyrimi);
        }


    }
}
