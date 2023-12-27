using EHW_M;
using EHWM.ViewModel;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EHWM.Views.Popups {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArsyejaKthimitPopup : PopupPage {
        public ArsyejaKthimitPopup() {
            InitializeComponent();
            arsyejaPicker.ItemsSource = App.Instance.MainViewModel.Arsyejet;
            arsyejaPicker.SelectedIndex = 0;
        }

        private async void Button_Clicked(object sender, EventArgs e) {
            var bc = (ShitjaViewModel)BindingContext;
            bc.WaitForArsyejenEKthimit = true;
            await App.Instance.MainPage.Navigation.PopPopupAsync(true);
        }
    }
}