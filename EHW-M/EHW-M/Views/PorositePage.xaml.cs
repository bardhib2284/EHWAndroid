using EHWM.Models;
using EHWM.ViewModel;
using EHWM.Views.Popups;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EHWM.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PorositePage : ContentPage {
        public PorositePage() {
            InitializeComponent();
            
        }

        private async void Button_Clicked(object sender, EventArgs e) {
            // Open a PopupPage
            if ((sender as Button).Text == "Vizualizo") {
                VizualizoPorositePage VizualizoPorositePage = new VizualizoPorositePage();
                VizualizoPorositePage.BindingContext = this.BindingContext;
                await Navigation.PushPopupAsync(VizualizoPorositePage);
                return;
            }
            if ((sender as Button).Text == "Faturo") {
                var bc = (PorositeViewModel )BindingContext;
                if(bc!=null) {
                    bc.KonvertoPorosine();
                }
                
                return;
            }            
            if ((sender as Button).Text == "Krijo liste") {
                var bc = (PorositeViewModel)BindingContext;
                if(bc != null) {
                    await bc.GoToKrijoListenAsync();
                    return;
                }
            }            
            if ((sender as Button).Text == "Shto") {
                var bc = (PorositeViewModel)BindingContext; if(bc != null) { await bc.GoToKrijoPorosineAsync(); }
                //KrijoPorosinePage kpPorosia = new KrijoPorosinePage();
                //kpPorosia.BindingContext = this.BindingContext;
                //await Navigation.PushPopupAsync(kpPorosia);
                return;
            }
            ClientOptionsPopup page = new ClientOptionsPopup();
            await Navigation.PushPopupAsync(page);
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e) {
            var bc = (PorositeViewModel)BindingContext;
            if(bc!=null) {
                bc.CurrentlySelectedOrder = e.Item as Orders;
            }
        }
    }
}