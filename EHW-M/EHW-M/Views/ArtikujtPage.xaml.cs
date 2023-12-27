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
    public partial class ArtikujtPage : ContentPage {
        public ArtikujtPage() {
            InitializeComponent();
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e) {
            var bc = (ArtikujtViewModel)BindingContext;
            if(!string.IsNullOrEmpty(e.NewTextValue)) {
                bc.SearchedArtikujt = new System.Collections.ObjectModel.ObservableCollection<Models.Artikulli>(bc.Artikujt.Where(x => x.Emri.ToLower().Contains(e.NewTextValue.ToLower())));
                searchedFrame.IsVisible = true;
                unsearchedFrame.IsVisible = false;
            }
            else {
                searchedFrame.IsVisible = false;
                unsearchedFrame.IsVisible = true;
            }
        }
    }
}