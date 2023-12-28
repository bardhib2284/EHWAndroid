using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EHWM.Models;
using EHWM.ViewModel;
using EHWM.Views.Popups;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EHW_M;

namespace EHWM.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LevizjetListPage : ContentPage {
        public LevizjetListPage() {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e) {
            await Navigation.PushAsync(new ShfaqLevizjenPage() { BindingContext = this.BindingContext });
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e) {
            var bc = (LevizjetViewModel)BindingContext;
            if (bc != null) {
                if (e.Item is LevizjetHeader lh) {
                    bc.CurrentlySelectedLevizjetHeader = lh;
                    var levizjetDetails = await App.Database.GetLevizjeDetailsAsync();
                    bc.CurrentlySelectedLevizjetDetails = new System.Collections.ObjectModel.ObservableCollection<LevizjetDetails>(levizjetDetails.Where(x => x.NumriLevizjes == lh.NumriLevizjes));
                }
            }
        }
    }
}