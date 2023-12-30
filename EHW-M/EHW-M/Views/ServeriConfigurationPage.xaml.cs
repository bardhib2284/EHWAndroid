using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EHWM.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServeriConfigurationPage : ContentPage {
        public ServeriConfigurationPage() {
            InitializeComponent();
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            if(string.IsNullOrEmpty(serveri.Text)) {
                serveri.Text = "http://84.22.42.59:8188/ehwapi/";
                //serveri.Text = "http://95.107.161.233:4448/ehwapi/";
                fiskalizimi.Text = "http://84.22.42.59:5010/FiscalisationService.asmx";
                //fiskalizimi.Text = "http://95.107.161.233:4446/FiscalisationService.asmx";
            }
        }
    }
}