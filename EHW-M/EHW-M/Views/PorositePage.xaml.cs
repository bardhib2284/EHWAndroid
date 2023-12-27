using EHWM.Models;
using EHWM.ViewModel;
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
                VizitatOptionsPopup VizitatOptionsPopup = new VizitatOptionsPopup();
                VizitatOptionsPopup.BindingContext = this.BindingContext;
                await Navigation.PushPopupAsync(VizitatOptionsPopup);
                return;
            }
            if ((sender as Button).Text == "Faturo") {
                VizitatOptionsPopup VizitatOptionsPopup = new VizitatOptionsPopup();
                VizitatOptionsPopup.BindingContext = this.BindingContext;
                await Navigation.PushPopupAsync(VizitatOptionsPopup);
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
                VizitatOptionsPopup VizitatOptionsPopup = new VizitatOptionsPopup();
                VizitatOptionsPopup.BindingContext = this.BindingContext;
                await Navigation.PushPopupAsync(VizitatOptionsPopup);
                return;
            }
            ClientOptionsPopup page = new ClientOptionsPopup();
            await Navigation.PushPopupAsync(page);
        }



    }
}