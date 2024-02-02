using Acr.UserDialogs;
using EHW_M;
using EHWM.Models;
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
    public partial class RegjistroLinkunPerFiskalizimiPopup : PopupPage {
        public RegjistroLinkunPerFiskalizimiPopup() {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e) {
            var bc = (MainViewModel)BindingContext;
            if (bc != null) {
                Linqet linqet = new Linqet { Linku = entri.Text, Tipi = "FISKALIZIMI" };
                await App.Database.SaveLinkuAsync(linqet);
                UserDialogs.Instance.Alert("Linku u shtua me sukses");
                await App.Instance.MainPage.Navigation.PopPopupAsync(true);
                bc.LinqetPerFiskalizim.Add(linqet);
            }
        }
    }
}