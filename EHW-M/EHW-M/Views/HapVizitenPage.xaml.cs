using EHW_M;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EHWM.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HapVizitenPage : ContentPage {
        public HapVizitenPage() {
            InitializeComponent();
            statusiVizites.ItemsSource = App.Instance.MainViewModel.StatusetEVizites;
        }
    }
}