using Acr.UserDialogs;
using EHW_M;
using EHWM.Models;
using EHWM.ViewModel;
using EHWM.Views.Popups;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EHWM.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShitjaPage : ContentPage {
        public ShitjaPage() {
            InitializeComponent();
        }

        protected override void OnDisappearing() {
            App.Instance.MainViewModel.DissapearingFromShitjaPage = true;
            base.OnDisappearing();
        }
        bool hasBackButtonRequested = false;
        public bool Accepted = false;
        public bool Refused = false;
        protected override bool OnBackButtonPressed() {
            Device.InvokeOnMainThreadAsync(async () => 
            {
                var conf = await UserDialogs.Instance.ConfirmAsync("Jeni te sigurt per kthim mbrapa?", "Kthehu", "Po","Jo");
                if (conf)
                    await App.Instance.PopPageAsync();
            });
            return true;
        }

        private async void Button_Clicked(object sender, EventArgs e) {
            await Navigation.PushAsync(new PrinterSelectionPage());
        }


        private void testList_ItemTapped(object sender, ItemTappedEventArgs e) {
            fshijButton.IsVisible = true;
            editoButton.IsVisible = true;
            var bc = (ShitjaViewModel)BindingContext;
            bc.CurrentlySelectedArtikulli = (sender as ListView).SelectedItem as Artikulli;
            
        }

        private void fshijButton_Clicked(object sender, EventArgs e) {
            fshijButton.IsVisible = false;
        }

        private void editButton_Clicked(object sender, EventArgs e) {
            //editoButton.IsVisible = false;
        }
        private void Entry_TextChanged(object sender, TextChangedEventArgs e) {

        }

        private void Entry_Unfocused(object sender, FocusEventArgs e) {
            if (string.IsNullOrEmpty((sender as Entry).Text))
                return;
            var bc = (ShitjaViewModel)BindingContext;
            if (bc.KthimMalli) {
                if((sender as Entry).Text.Contains("-")) {
                    var valueSplit = (sender as Entry).Text.Split('-');
                    if (valueSplit.Count() > 1) {
                        (sender as Entry).Text = "-" + decimal.Parse(valueSplit[1]).ToString("0.00");
                    }
                }
                else {
                    var stringi = "-" + (sender as Entry).Text;
                    var valueSplit = stringi.Split('-');
                    if (valueSplit.Count() > 1) {
                        (sender as Entry).Text = "-" + decimal.Parse(valueSplit[1]).ToString("0.00");
                    }
                }
            }else
                (sender as Entry).Text = decimal.Parse((sender as Entry).Text).ToString("0.00");

        }

        private void Entry_Focused(object sender, FocusEventArgs e) {
            var bc = (ShitjaViewModel)BindingContext;
            if(bc.KthimMalli) {
                (sender as Entry).Text = "-";
            }
                (sender as Entry).Text = string.Empty;
        }

    }
}