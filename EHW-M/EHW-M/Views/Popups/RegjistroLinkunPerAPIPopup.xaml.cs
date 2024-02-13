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
    public partial class RegjistroLinkunPerAPIPopup : PopupPage {
        public RegjistroLinkunPerAPIPopup() {
            InitializeComponent();
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            var bc = (MainViewModel)BindingContext;
            if (bc != null) {
                if (bc.EditLinks) {
                    entri.Text = bc.LinkuPerApiPerEdit.Linku;
                    titulli.Text = bc.LinkuPerApiPerEdit.Titulli;
                    edit.IsVisible = true;
                }
                else
                    save.IsVisible = true;
            }
        }
        private async void Button_Clicked(object sender, EventArgs e) {
            var bc = (MainViewModel)BindingContext;
            if(bc.EditLinks) {
                Linqet linqet = bc.LinqetPerAPI.FirstOrDefault(x => x.Id == bc.LinkuPerApiPerEdit.Id);
                linqet.Linku = entri.Text;
                linqet.Titulli = titulli.Text;
                await App.Database.UpdateLinkuAsync(linqet);
                UserDialogs.Instance.Alert("Linku u editua me sukses");
                await App.Instance.MainPage.Navigation.PopPopupAsync(true);
                bc.LinqetPerAPI.Remove(linqet);
                bc.LinqetPerAPI.Insert(0, linqet);
            }
            else {
                if (bc != null) {
                    Linqet linqet = new Linqet { Linku = entri.Text, Tipi = "API", Titulli = titulli.Text };
                    await App.Database.SaveLinkuAsync(linqet);
                    UserDialogs.Instance.Alert("Linku u shtua me sukses");
                    await App.Instance.MainPage.Navigation.PopPopupAsync(true);
                    bc.LinqetPerAPI.Add(linqet);
                }
            }

        }
    }
}