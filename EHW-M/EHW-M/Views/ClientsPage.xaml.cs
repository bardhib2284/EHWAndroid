using EHWM.Models;
using EHWM.ViewModel;
using EHWM.Views.Popups;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EHWM.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientsPage : ContentPage {
        public ClientsPage() {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("sq-AL");
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            AllClientsList.IsVisible = true;
            SearchedClientsList.IsVisible = false;
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            var bc = (MainViewModel)BindingContext;
            if(bc.DissapearingFromShitjaPage) {
                bc.SelectedVizita = null;
                bc.DissapearingFromShitjaPage = false;
            }
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e) {
            var bc = (MainViewModel)BindingContext;
            bc.SelectedVizita = (e.Item as Vizita);
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e) {
            if (string.IsNullOrEmpty(e.NewTextValue)) {
                AllClientsList.IsVisible = true;
                SearchedClientsList.IsVisible = false;
                return;
            }
            AllClientsList.IsVisible = false;
            SearchedClientsList.IsVisible = true;
            var bc = (MainViewModel)BindingContext;
            bc.SearchedVizitat = new System.Collections.ObjectModel.ObservableCollection<Vizita>(bc.VizitatFilteredByDate.Where(x => x.Klienti.Contains(e.NewTextValue.ToUpper())).ToList());
            bc.SelectedVizita = null;
        }
        public void AllClientsListVisibility(bool visible) {
            AllClientsList.IsVisible = visible;
        }

        public void SearchedClientsListVisibility(bool visible) {  SearchedClientsList.IsVisible = visible; }
        private async void Button_Clicked(object sender, EventArgs e) {
            // Open a PopupPage
            if((sender as Button).Text == "Vizitat") {
                VizitatOptionsPopup VizitatOptionsPopup = new VizitatOptionsPopup();
                VizitatOptionsPopup.BindingContext = this.BindingContext;
                await Navigation.PushPopupAsync(VizitatOptionsPopup);
                return;
            }
            if((sender as Button).Text == "Shitje") {
                var bc = (MainViewModel)BindingContext;
                bc.ShtijeKorrigjim();
                return;
            }            
            if((sender as Button).Text == "Kthimi") {
                var bc = (MainViewModel)BindingContext;
                bc.KthimMalli();
                return;
            }            
            if((sender as Button).Text == "Vizualizo") {
                VizualizoShitjetPopup VizualizoShitjetPopup = new VizualizoShitjetPopup();
                VizualizoShitjetPopup.BindingContext = this.BindingContext;
                await Navigation.PushPopupAsync(VizualizoShitjetPopup);
                return;
            }

            ClientOptionsPopup page = new ClientOptionsPopup();
            await Navigation.PushPopupAsync(page);
        }

        ViewCell lastCell;
        private void ViewCell_Tapped(object sender, EventArgs e) {
            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.Transparent;
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null) {
                viewCell.View.BackgroundColor = Color.Gray;
                lastCell = viewCell;
            }
        }



        protected override void OnDisappearing() {

            base.OnDisappearing();
            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.Transparent;
        }
    }
}