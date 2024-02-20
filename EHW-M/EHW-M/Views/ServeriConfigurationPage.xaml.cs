using EHW_M;
using EHWM.Models;
using EHWM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EHWM.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServeriConfigurationPage : ContentPage {
        public ServeriConfigurationPage() {
            InitializeComponent();
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            var bc = (MainViewModel)BindingContext;
            var linkuPerApi = bc.LinqetPerAPI.FirstOrDefault(x => x.IsActive);
            var linkuPerFisk = bc.LinqetPerFiskalizim.FirstOrDefault(x => x.IsActive);
            if(linkuPerApi != null) {
                apiPicker.SelectedIndex = bc.LinqetPerAPI.IndexOf(linkuPerApi);
                serveri.Text = linkuPerApi.Linku;
            }
            if(linkuPerFisk != null) {
                fiskPicker.SelectedIndex = bc.LinqetPerFiskalizim.IndexOf(linkuPerFisk);
                fiskalizimi.Text = linkuPerFisk.Linku;
            }
            if (string.IsNullOrEmpty(serveri.Text)) {
                if(bc.LinqetPerAPI.Count > 0) {
                    var activeLink = bc.LinqetPerAPI.FirstOrDefault(x => x.IsActive);
                    if(activeLink != null)
                        serveri.Text = activeLink.Linku;
                }
                //serveri.text = "http://84.22.42.59:8188/ehwapi/"
                //serveri.Text = "http://95.107.161.233:4448/ehwapi/";
                //fiskalizimi.Text = "http://84.22.42.59:5010/FiscalisationService.asmx";
                //fiskalizimi.Text = "http://95.107.161.233:4446/FiscalisationService.asmx";
            }
            if (string.IsNullOrEmpty(fiskalizimi.Text)) {
                if(bc.LinqetPerFiskalizim.Count > 0) {
                    var activeLink = bc.LinqetPerFiskalizim.FirstOrDefault(x => x.IsActive);
                    if(activeLink != null)
                        fiskalizimi.Text = activeLink.Linku;
                }
                //serveri.Text = "http://95.107.161.233:4448/ehwapi/";
                //fiskalizimi.Text = "http://84.22.42.59:5010/FiscalisationService.asmx";
                //fiskalizimi.Text = "http://95.107.161.233:4446/FiscalisationService.asmx";
            }
        }

        private async void Picker_SelectedIndexChanged(object sender, EventArgs e) {
            var bc = (MainViewModel)BindingContext;
            foreach(var linku in bc.LinqetPerAPI) {
                linku.IsActive = false;
                await App.Database.UpdateLinkuAsync(linku);
            }
            var activeLink = bc.LinqetPerAPI.FirstOrDefault(x => x.Id == ((sender as Picker).SelectedItem as Linqet).Id).IsActive = true;
            var linkuPerApi = bc.LinqetPerAPI.FirstOrDefault(x => x.IsActive);
            await App.Database.UpdateLinkuAsync(linkuPerApi);
            serveri.Text = linkuPerApi.Linku;
        }

        private async void fiskPicker_SelectedIndexChanged(object sender, EventArgs e) {
            var bc = (MainViewModel)BindingContext;
            foreach (var linku in bc.LinqetPerFiskalizim) {
                linku.IsActive = false;
                await App.Database.UpdateLinkuAsync(linku);
            }
            var activeLinki  = ((sender as Picker).SelectedIndex + 1);
            var activeLink = bc.LinqetPerFiskalizim.FirstOrDefault(x => x.Id == ((sender as Picker).SelectedItem as Linqet).Id).IsActive = true;
            var linkuPerFisk = bc.LinqetPerFiskalizim.FirstOrDefault(x => x.IsActive);
            await App.Database.UpdateLinkuAsync(linkuPerFisk);
            fiskalizimi.Text = linkuPerFisk.Linku;
        }
    }
}