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
    public partial class KrijoPorositePage : ContentPage {
        public KrijoPorositePage() {
            InitializeComponent();
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            depotPicker.SelectedIndex = 0;
        }

        private void depotPicker_SelectedIndexChanged(object sender, EventArgs e) {
            if (depotPicker.SelectedItem != null) {
                var bc = (PorositeViewModel)BindingContext;
                bc.SelectedKlient = depotPicker.SelectedItem as Klientet;
            }
        }

        private void testList_ItemTapped(object sender, ItemTappedEventArgs e) {
            if(e.Item is KrijimiPorosive kp) {

            }
        }
    }
}