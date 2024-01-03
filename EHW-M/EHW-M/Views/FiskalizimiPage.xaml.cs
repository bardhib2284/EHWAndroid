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
    public partial class FiskalizimiPage : ContentPage {
        public FiskalizimiPage() {
            InitializeComponent();
            pickerItemSource.ItemsSource = new List<string> { "REGJISTRIMI I ARKES", "SHITJET" ,"LEVIZJET"};
            pickerItemSource.SelectedIndexChanged += PickerItemSource_SelectedIndexChanged;
        }

        private void PickerItemSource_SelectedIndexChanged(object sender, EventArgs e) {
            var bc = (FiskalizimiViewModel)BindingContext;

            if (pickerItemSource.SelectedIndex == 0) {
                ShitjetStackList.IsVisible = false;
                CashRegisterStackList.IsVisible = true;
                LevizjetStackList.IsVisible = false;
                bc.SelectedIndex = 0;
            }

            if (pickerItemSource.SelectedIndex == 1) {
                ShitjetStackList.IsVisible = true;
                CashRegisterStackList.IsVisible = false;
                LevizjetStackList.IsVisible = false;
                bc.SelectedIndex = 1;
            }
            if (pickerItemSource.SelectedIndex == 2) {
                ShitjetStackList.IsVisible = false;
                CashRegisterStackList.IsVisible = false;
                LevizjetStackList.IsVisible = true;
                bc.SelectedIndex = 2;
            }
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e) {

            var bc = (FiskalizimiViewModel)BindingContext;
            if(e.Item is Liferimi l) {
                bc.SelectedLiferimi = l;
            }
            else if(e.Item is CashRegister cr) {
                bc.SelectedCashRegister = cr;
            }
            else if(e.Item is LevizjetHeader lh) {
                bc.SelectedLevizja = lh;
            }
        }
    }
}