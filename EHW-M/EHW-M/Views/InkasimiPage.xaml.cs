using EHWM.Models;
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
    public partial class InkasimiPage : ContentPage {
        public InkasimiPage() {
            InitializeComponent();
            monedhaPicker.ItemsSource = new List<string> { "LEK", "EUR", "USD" };
            tipiPagesesPicker.ItemsSource = new List<string> { "KESH", "BANK" };
            monedhaPicker.SelectedItem = "LEK";
            tipiPagesesPicker.SelectedItem = "KESH";
            
        }

        private void depotPicker_SelectedIndexChanged(object sender, EventArgs e) {
            //if(depotPicker.SelectedItem != null) {
            //    var bc = (InkasimiViewModel)BindingContext;
            //    bc.MerrDetyrimet(depotPicker.SelectedItem as Klientet);
            //}
        }
    }
}