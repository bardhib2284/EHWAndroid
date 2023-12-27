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
    public partial class CreateClientPage : ContentPage {
        public CreateClientPage() {
            InitializeComponent();
            cmimoret.ItemsSource = new List<string> { "10% Zbritje", "5% Zbritje", "2% Zbritje", "1% Zbritje" };
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            var bc = (MainViewModel)BindingContext;
            if(bc != null)
                ClientPicker.ItemsSource = bc.Clients;
        }

        private void ClientPicker_SelectedIndexChanged(object sender, EventArgs e) {
            var bc = (MainViewModel)BindingContext;
            bc.SelectedVizita.IDKlientDheLokacion =((sender as Picker).SelectedItem as Klientet).IDKlienti;
        }
    }
}