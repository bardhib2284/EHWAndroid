using Acr.UserDialogs;
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
    public partial class LiferimiArtDetailsDevMode : ContentPage {
        public LiferimiArtDetailsDevMode() {
            InitializeComponent();
        }
        private async void Button_Clicked(object sender, EventArgs e) {
            var bc = (MainViewModel)BindingContext;
            var res = await App.Database.UpdateLiferimiArtAsync(bc.SelectedLiferimiArtDevMode);
            if (res > 0) {
                UserDialogs.Instance.Alert("Liferimi Art U Editua Me Sukses");
                await App.Instance.PopPageAsync();
            }
        }

    }
}