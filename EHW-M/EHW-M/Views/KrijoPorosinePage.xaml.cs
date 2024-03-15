using Acr.UserDialogs;
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
    public partial class KrijoPorosinePage : ContentPage {
        public KrijoPorosinePage() {
            InitializeComponent();
        }

        private void fshijButton_Clicked(object sender, EventArgs e) {
            fshijButton.IsVisible = false;
        }

        private void testList_ItemTapped(object sender, ItemTappedEventArgs e) {
            fshijButton.IsVisible = true;
            var bc = (PorositeViewModel)BindingContext;
            bc.CurrentlySelectedArtikulli = (sender as ListView).SelectedItem as Artikulli;

        }
        protected override bool OnBackButtonPressed() {
            Device.InvokeOnMainThreadAsync(async () =>
            {
                var conf = await UserDialogs.Instance.ConfirmAsync("Jeni te sigurt per kthim mbrapa?", "Kthehu", "Po", "Jo");
                if (conf)
                    await App.Instance.PopPageAsync();
            });
            return true;
        }
        protected override void OnAppearing() {
            base.OnAppearing();
            var bc = (PorositeViewModel)BindingContext;
            testList.SelectedItem = bc.CurrentlySelectedArtikulli;
        }
    }
}