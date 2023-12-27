using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EHWM.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class KonfirmoPagesenPage : ContentPage {
        public KonfirmoPagesenPage() {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
        }

        private async void Button_Clicked(object sender, EventArgs e) {
            await Navigation.PushAsync(new PrinterSelectionPage());
        }

        protected override bool OnBackButtonPressed() {
            return true;
        }
    }
}