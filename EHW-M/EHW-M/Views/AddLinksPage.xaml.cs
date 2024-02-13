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
    public partial class AddLinksPage : ContentPage {
        public AddLinksPage() {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e) {

        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e) {
            var bc = (MainViewModel)BindingContext;
            apiEdit.IsVisible = true;
            bc.LinkuPerApiPerEdit = e.Item as Linqet;
        }

        private void ListView_ItemTapped_1(object sender, ItemTappedEventArgs e) {
            var bc = (MainViewModel)BindingContext;
            fiskEdit.IsVisible = true;
            bc.LinkuPerFiskPerEdit = e.Item as Linqet;
        }
    }
}