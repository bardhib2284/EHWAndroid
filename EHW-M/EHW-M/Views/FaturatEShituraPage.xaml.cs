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
    public partial class FaturatEShituraPage : ContentPage {
        public FaturatEShituraPage() {
            InitializeComponent();
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e) {
            var bc = (MainViewModel)BindingContext;
            if(bc != null) {
                bc.FixDataVizualizimit();
            }
        }

        private void SearchedClientsList_ItemSelected(object sender, SelectedItemChangedEventArgs e) {
            var bc = (MainViewModel)BindingContext;
            if(bc != null) {
                bc.SelectedLiferimetEKryera = (e.SelectedItem as VizualizimiFatures);
            }
        }
    }
}