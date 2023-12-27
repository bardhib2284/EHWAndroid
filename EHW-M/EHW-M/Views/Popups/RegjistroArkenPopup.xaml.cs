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
    public partial class RegjistroArkenPopup : PopupPage {
        public RegjistroArkenPopup() {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e) {
            var bc = (MainViewModel)BindingContext;
            bc.EshteRuajtuarArka = true;
            await App.Instance.MainPage.Navigation.PopPopupAsync(true);
        }
    }
}