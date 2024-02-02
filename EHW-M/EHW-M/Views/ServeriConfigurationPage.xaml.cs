using EHW_M;
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
            if(linkuPerApi != null) {
                apiPicker.SelectedIndex = bc.LinqetPerAPI.IndexOf(linkuPerApi);
                serveri.Text = linkuPerApi.Linku;
            }
            if (string.IsNullOrEmpty(serveri.Text)) {
                if(bc.LinqetPerAPI.Count > 0) {
                    var activeLink = bc.LinqetPerAPI.FirstOrDefault(x => x.IsActive);
                    if(activeLink != null)
                        serveri.Text = activeLink.Linku;
                }
                //serveri.Text = "http://95.107.161.233:4448/ehwapi/";
                fiskalizimi.Text = "http://84.22.42.59:5010/FiscalisationService.asmx";
                //fiskalizimi.Text = "http://95.107.161.233:4446/FiscalisationService.asmx";
            }
        }

        private async void Picker_SelectedIndexChanged(object sender, EventArgs e) {
            var bc = (MainViewModel)BindingContext;
            foreach(var linku in bc.LinqetPerAPI) {
                linku.IsActive = false;
                await App.Database.UpdateLinkuAsync(linku);
            }
            var activeLink = bc.LinqetPerAPI.FirstOrDefault(x => x.Id == ((sender as Picker).SelectedIndex + 1 )).IsActive = true;
            var linkuPerApi = bc.LinqetPerAPI.FirstOrDefault(x => x.IsActive);
            await App.Database.UpdateLinkuAsync(linkuPerApi);
            serveri.Text = linkuPerApi.Linku;
        }
    }
}