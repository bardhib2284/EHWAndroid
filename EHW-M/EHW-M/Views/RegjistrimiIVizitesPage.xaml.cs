using Acr.UserDialogs;
using EHW_M;
using EHWM.Models;
using EHWM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EHWM.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegjistrimiIVizitesPage : ContentPage {
        public RegjistrimiIVizitesPage() {
            InitializeComponent();
        }
        protected override async void OnAppearing() {
            base.OnAppearing();
            var bc = (MainViewModel)BindingContext;
            if(bc.SelectedVizita != null) {
                if(bc.SelectedVizita.IDKlientDheLokacion != null) {
                    var kdl = await App.Database.GetKlientetDheLokacionetAsync();
                    pickerForClients.SelectedIndex = bc.Clients.IndexOf(bc.Clients.FirstOrDefault(x => x.IDKlienti == kdl.FirstOrDefault(y => y.IDKlientDheLokacion == bc.SelectedVizita.IDKlientDheLokacion).IDKlienti));
                }
                else {
                    UserDialogs.Instance.Alert("Vizita e selektuar nuk ka klient te lidhur me viziten, ju lutem raportoni problemin, dhe ri-zgjedhni klientin perseri");
                }
            }
        }
        private async void Picker_SelectedIndexChanged(object sender, EventArgs e) {
           
        }

        private async void Button_Clicked(object sender, EventArgs e) {
            var bc = (MainViewModel)BindingContext;
            UserDialogs.Instance.ShowLoading("Duke Regjistruar Viziten");
            var kdl = await App.Database.GetKlientetDheLokacionetAsync();
            if (kdl != null && kdl.Count > 0) {
                if (pickerForClients.SelectedItem != null) {
                    var client = kdl.FirstOrDefault(x => x.IDKlienti == (bc.Clients[pickerForClients.SelectedIndex].IDKlienti));
                    if (client != null)
                        Adresa.Text = client.Adresa;
                    var res = await Geolocation.GetLastKnownLocationAsync();
                    Guid g = Guid.NewGuid();
                    App.Instance.MainViewModel.RegjistroVizitenVizita = new Vizita
                    {
                        DataAritjes = App.Instance.MainViewModel.RegjistroVizitenDate,
                        DeviceID = App.Instance.MainViewModel.LoginData.DeviceID,
                        IDAgjenti = App.Instance.MainViewModel.LoginData.IDAgjenti,
                        Latitude = res?.Latitude.ToString(),
                        Longitude = res?.Longitude.ToString(),
                        IDVizita = g,
                        IDKlientDheLokacion = client?.IDKlientDheLokacion,
                        Komenti = "",
                        OraPlanifikimit = null,
                        IDStatusiVizites = 0.ToString(),
                        SyncStatus = 0,
                        DataPlanifikimit = App.Instance.MainViewModel.RegjistroVizitenDate
                    };
                        client = kdl.FirstOrDefault(x => x.IDKlienti == (pickerForClients.SelectedItem as Klientet).IDKlienti);
                        if (client != null)
                            Adresa.Text = client.Adresa;
                        App.Instance.MainViewModel.RegjistroVizitenVizita.IDKlientDheLokacion = client?.IDKlientDheLokacion;
                        UserDialogs.Instance.HideLoading();
                        await App.Instance.PopPageAsync();
                        var v = App.Instance.MainViewModel.RegjistroVizitenVizita;
                        App.Instance.MainViewModel.RegjistroVizitenVizita.Klienti = App.Instance.MainViewModel.KlientetDheLokacionet.FirstOrDefault(x => x.IDKlienti == v.IDKlientDheLokacion)?.KontaktEmriMbiemri ?? string.Empty;
                        v.Vendi = App.Instance.MainViewModel.KlientetDheLokacionet.FirstOrDefault(x => x.IDKlienti == v.IDKlientDheLokacion)?.EmriLokacionit ?? string.Empty;
                        v.Klienti = App.Instance.MainViewModel.KlientetDheLokacionet.FirstOrDefault(x => x.IDKlienti == v.IDKlientDheLokacion)?.KontaktEmriMbiemri ?? string.Empty;
                        v.Adresa = App.Instance.MainViewModel.KlientetDheLokacionet.FirstOrDefault(x => x.IDKlienti == v.IDKlientDheLokacion)?.Adresa ?? string.Empty;
                        if (v.Vendi == string.Empty || v.Klienti == string.Empty) {
                            UserDialogs.Instance.Alert("Vizita nuk mund te krijohet per kete klient, nuk gjendet klient dhe lokacion per kete klient");
                            App.Instance.MainViewModel.RegjistroVizitenVizita = null;
                            return;
                        }
                        var result = await App.Database.SaveVizitaAsync(App.Instance.MainViewModel.RegjistroVizitenVizita);
                        if(result != -1) {
                        if (App.Instance.MainViewModel.VizitatFilteredByDate == null) {
                            App.Instance.MainViewModel.VizitatFilteredByDate = new System.Collections.ObjectModel.ObservableCollection<Vizita> { App.Instance.MainViewModel.RegjistroVizitenVizita };
                        }
                        else
                            App.Instance.MainViewModel.VizitatFilteredByDate.Insert(0, App.Instance.MainViewModel.RegjistroVizitenVizita);
                        if (App.Instance.MainViewModel.SearchedVizitat == null) {
                            App.Instance.MainViewModel.SearchedVizitat = new System.Collections.ObjectModel.ObservableCollection<Vizita> { App.Instance.MainViewModel.RegjistroVizitenVizita };
                        }
                        else
                            App.Instance.MainViewModel.SearchedVizitat.Insert(0, App.Instance.MainViewModel.RegjistroVizitenVizita);
                        UserDialogs.Instance.Alert("Vizita U Shtua Me Sukses", "Sukses", "Ok");
                    }


                }

            }
            else {
                UserDialogs.Instance.HideLoading();
                await Task.Delay(20);
                UserDialogs.Instance.Alert("Nuk ka klienta te sinkronizuar, ju lutemi sinkronizoni klientet ne fillim");
            }
        }
    }
}