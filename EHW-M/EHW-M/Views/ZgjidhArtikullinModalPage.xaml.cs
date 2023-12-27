using EHW_M;
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
    public partial class ZgjidhArtikullinModalPage : ContentPage {
        public ZgjidhArtikullinModalPage() {
            InitializeComponent();

        }

        private async void testList_ItemTapped(object sender, ItemTappedEventArgs e) {
            if (e.Item != null) {
                if(BindingContext is ShitjaViewModel viewModel) {
                    if (viewModel != null) {
                        viewModel.CurrentlySelectedArtikulli = e.Item as Artikulli;
                        if (string.IsNullOrEmpty(viewModel.CurrentlySelectedArtikulli.Seri)) {
                            viewModel.EnableSeri = true;
                        }
                        (e.Item as Artikulli).Sasia -= viewModel.Sasia;
                        App.Instance.PopAsyncModal();
                        viewModel.Sasia = 0;
                    }
                }
                else if(BindingContext is LevizjetViewModel levizjetViewModel) 
                {
                    if (levizjetViewModel != null) {
                        levizjetViewModel.CurrentlySelectedArtikulli = e.Item as Artikulli;
                        levizjetViewModel.HasAnArticleBeenSelected = true;
                        if (string.IsNullOrEmpty(levizjetViewModel.CurrentlySelectedArtikulli.Seri)) {
                            //viewModel.EnableSeri = true;
                        }
                        App.Instance.PopAsyncModal();
                        levizjetViewModel.Sasia = 0;
                    }
                }
            }
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e) {
            if(!string.IsNullOrEmpty(e.NewTextValue)) {
                var bc = (ShitjaViewModel)BindingContext;
                if(bc != null) {
                    bc.SearchedArtikujt = new System.Collections.ObjectModel.ObservableCollection<Artikulli>(bc.Artikujt.Where(x => x.Emri.ToLower().Contains(e.NewTextValue.ToLower())));
                    searchedArtikujt.IsVisible = true;
                    allArtikujt.IsVisible = false;
                }
            }
            else {
                searchedArtikujt.IsVisible = false;
                allArtikujt.IsVisible = true;
            }
        }
    }
}