using EHW_M;
using EHWM.Models;
using EHWM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EHWM.ViewModel {

    public class PorositeViewModelNavigationParameters {
        public List<Klientet> Klientet { get; set; }
        public List<KrijimiPorosive> KrijimiPorosives { get; set; }
        public Agjendet Agjendi { get; set; }
    }

    public class PorositeViewModel : BaseViewModel{

        public List<KrijimiPorosive> KrijimiPorosives { get; set; }

        public List<Klientet> Klientet { get; set; }
        public Agjendet Agjendi { get; set; }

        public ICommand ShtoKlientinCommand { get; set; }
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
        public PorositeViewModel(PorositeViewModelNavigationParameters PorositeViewModelNavigationParameters) {
            Klientet = PorositeViewModelNavigationParameters.Klientet;
            Agjendi = PorositeViewModelNavigationParameters.Agjendi;
            Data = DateTime.Now;
            SubTitle = "Lista e porosive "  + Data.AddDays(-7).ToString("dd/MM/yyyy") + " - " + Data.ToString("dd/MM/yyyy");
            KrijimiPorosives = PorositeViewModelNavigationParameters.KrijimiPorosives;
            ShtoKlientinCommand = new Command(ShtoKlientinNeListe);
            KrijimiPorositeKlientet = new ObservableCollection<KrijimiPorosive>();
        }

        public void ShtoKlientinNeListe() {
            KrijimiPorosive krijimiPorosive = new KrijimiPorosive
            {
                KPID = Guid.NewGuid(),
                Data_Regjistrimit = DateTime.Now,
                IDKlienti = SelectedKlient.IDKlienti,
                Emri = SelectedKlient.Emri,
                Depo = Agjendi.Depo,
                DeviceID = Agjendi.DeviceID,
                IDAgjenti = Agjendi.IDAgjenti,
                Imp_Status = 0,
                SyncStatus = 0
            };
            KrijimiPorositeKlientet.Add(krijimiPorosive);
        }

        public async Task GoToKrijoListenAsync() {
            KrijoPorositePage krijoPorositePage = new KrijoPorositePage();
            krijoPorositePage.BindingContext = this;
            await App.Instance.PushAsyncNewPage(krijoPorositePage);
        }
    }
}
