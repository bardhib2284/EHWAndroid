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
    public partial class KthehuNgaShitjaPopup : PopupPage {
        public KthehuNgaShitjaPopup() {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e) {
            var bc = (ShitjaPage)BindingContext;
            bc.Accepted = true;
            bc.Refused = false;
            await App.Instance.MainPage.Navigation.PopPopupAsync(true);
        }

        private async void Button_Clicked1(object sender, EventArgs e) {
            var bc = (ShitjaPage)BindingContext;
            bc.Refused = true;
            bc.Accepted = false;
            await App.Instance.MainPage.Navigation.PopPopupAsync(true);
        }
    }
}