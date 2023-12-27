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

        private void SasiaEntry_TextChanged(object sender, TextChangedEventArgs e) {
            var bc = (LevizjetViewModel)BindingContext;
            if(bc != null) {
                if(bc.CurrentlySelectedArtikulli != null) {
                    if (bc.CurrentlySelectedArtikulli.BUM == "COP") {
                        if (e.NewTextValue.Contains(".") || e.NewTextValue.Contains(",")) {
                            sasiaEntry.Text = e.OldTextValue;
                            return;
                        }
                    }
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

        protected override void OnAppearing() {
            base.OnAppearing();
            depotPicker.SelectedIndex = 0;
        }

        private void checkNga_CheckedChanged(object sender, CheckedChangedEventArgs e) {
            var bc = (LevizjetViewModel)BindingContext;
            if (checkNga.IsChecked) {
                checkNe.IsEnabled = false;
                checkNe.IsChecked = false;
                bc.Nga =true;
                bc.Ne =false;
            }
            if(checkNe.IsChecked) {
                checkNga.IsEnabled = false;
                checkNga.IsChecked = false;
                bc.Ne = true;
                bc.Nga = false;
            }
            if(checkNe.IsChecked == false && checkNga.IsChecked == false) {
                checkNe.IsEnabled = true;
                checkNga.IsEnabled = true;
            }
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