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
    public partial class PagesaPage : ContentPage {
        public PagesaPage() {
            InitializeComponent();
            monedhaPicker.ItemsSource = new List<string> { "LEK", "EUR" };
            menyraPicker.ItemsSource = new List<string> { "ZGJEDH", "KESH", "BANK" };
            monedhaPicker.SelectedIndex = 0;
            menyraPicker.SelectedIndex = 0;

            menyraPicker.SelectedIndexChanged += MenyraPicker_SelectedIndexChanged;
            vazhdoButton.IsEnabled = false;

        }

        private void MenyraPicker_SelectedIndexChanged(object sender, EventArgs e) {
            if (menyraPicker.SelectedIndex != 0) {
                vazhdoButton.IsEnabled = true;
            }
            else
                vazhdoButton.IsEnabled = false;

            var bc = (ShitjaViewModel)BindingContext;
            bc.PagesaType = menyraPicker.SelectedItem.ToString();
            if (menyraPicker.SelectedIndex == 2) {
                bc.TotalPrice = 0;
                cashShuma.IsVisible = false;
                bankShuma.IsVisible = true;
            }
            else if(menyraPicker.SelectedIndex == 1) {
                if(bc.TotalPrice == 0) {
                    bc.TotalPrice = bc.TotalBillPrice;
                }
                cashShuma.IsVisible = true;
                bankShuma.IsVisible = false;
            }
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            App.Instance.MainViewModel.DissapearingFromShitjaPage = false;
            vazhdoButton.IsEnabled = false;
        }

        private void vazhdoButton_Clicked(object sender, EventArgs e) {
            vazhdoButton.IsEnabled = false;
        }
    }
}