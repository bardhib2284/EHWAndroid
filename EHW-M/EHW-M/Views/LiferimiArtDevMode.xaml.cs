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
    public partial class LiferimiArtDevMode : ContentPage {
        public LiferimiArtDevMode() {
            InitializeComponent();
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e) {
            var bc = (MainViewModel)BindingContext;
            if (e.Item is Models.LiferimiArt) {
                bc.SelectedLiferimiArtDevMode = bc.LiferimetArtDevMode.FirstOrDefault(x => x.ID == (e.Item as Models.LiferimiArt).ID);
                await App.Instance.PushAsyncNewPage(new LiferimiArtDetailsDevMode() { BindingContext = bc });
            }
        }
    }
}