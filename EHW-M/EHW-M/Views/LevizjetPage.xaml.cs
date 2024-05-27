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
    public partial class LevizjetPage : ContentPage {
        public LevizjetPage() {
            InitializeComponent();
            sasiaEntry.TextChanged += SasiaEntry_TextChanged;
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
        private void SasiaEntry_TextChanged(object sender, TextChangedEventArgs e) {
            var bc = (LevizjetViewModel)BindingContext;
            if(bc != null) {
                if(bc.CurrentlySelectedArtikulli != null) {

                }
            }
        }

        private void testList_ItemTapped(object sender, ItemTappedEventArgs e) {
            fshijButton.IsVisible = true;
            var bc = (LevizjetViewModel)BindingContext;
            bc.CurrentlySelectedArtikulli = (sender as ListView).SelectedItem as Artikulli;

        }
        private void fshijButton_Clicked(object sender, EventArgs e) {
            fshijButton.IsVisible = false;
        }
        bool hasAppearedOnce = false;


        private void checkNga_CheckedChanged(object sender, CheckedChangedEventArgs e) {
            var bc = (LevizjetViewModel)BindingContext;
            bc.Nga = true;
            bc.Ne = false;
            checkNe.IsChecked = false;
        }

        private void checkNga_CheckedChangedi(object sender, CheckedChangedEventArgs e) {
            var bc = (LevizjetViewModel)BindingContext;
            bc.Ne = true;
            bc.Nga = false;
            checkNga.IsChecked = false;
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e) {
            var bc = (LevizjetViewModel)BindingContext;

            if(bc != null) {
                if((sender as Picker).SelectedItem != null) {
                    if((sender as Picker).SelectedItem is Artikulli a) {
                        bc.SelectedArtikulli = a;
                    }
                    else if ((sender as Picker).SelectedItem is Depot d) {
                        bc.SelectedKlientet = d;
                    }
                }
            }
        }


    }
}