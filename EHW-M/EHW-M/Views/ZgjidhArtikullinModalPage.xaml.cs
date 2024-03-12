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
        protected override void OnAppearing() {
            base.OnAppearing();
            var bc = BindingContext;
            if(bc is PorositeViewModel pvm) {
                pvm.RefreshScrollDown = () =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if(pvm.LastKnownArtikulliForScroll != null) {
                            testKrijoList.ScrollTo(pvm.Artikujt.FirstOrDefault(x => x.IDArtikulli == pvm.LastKnownArtikulliForScroll.IDArtikulli), ScrollToPosition.Center, false);
                            testKrijoList.SelectedItem = pvm.Artikujt.FirstOrDefault(x => x.IDArtikulli == pvm.LastKnownArtikulliForScroll.IDArtikulli);
                        }
                            
                    });
                };
            }
            if(bc is ShitjaViewModel shvm) {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (shvm.LastKnownArtikulliForScroll != null) {
                        testList.ScrollTo(shvm.Artikujt.FirstOrDefault(x => x.IDArtikulli == shvm.LastKnownArtikulliForScroll.IDArtikulli), ScrollToPosition.Center, false);
                        testList.SelectedItem = shvm.Artikujt.FirstOrDefault(x => x.IDArtikulli == shvm.LastKnownArtikulliForScroll.IDArtikulli);
                    }

                });
            }
        }
        private async void testList_ItemTapped(object sender, ItemTappedEventArgs e) {
            if (e.Item != null) {
                if(BindingContext is ShitjaViewModel viewModel) {
                    if (viewModel != null) {
                        viewModel.CurrentlySelectedArtikulli = e.Item as Artikulli;
                        viewModel.LastKnownArtikulliForScroll = e.Item as Artikulli;
                        if (string.IsNullOrEmpty(viewModel.CurrentlySelectedArtikulli.Seri)) {
                            viewModel.EnableSeri = true;
                        }
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
                else if (BindingContext is PorositeViewModel porositeViewModel) {
                    if (porositeViewModel != null) {
                        porositeViewModel.CurrentlySelectedArtikulli = e.Item as Artikulli;
                        porositeViewModel.LastKnownArtikulliForScroll = e.Item as Artikulli;

                        if (string.IsNullOrEmpty(porositeViewModel.CurrentlySelectedArtikulli.Seri)) {
                            //viewModel.EnableSeri = true;
                        }
                        App.Instance.PopAsyncModal();
                        porositeViewModel.Sasia = 0;
                    }
                }
            }
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e) {
            try {
                if (BindingContext is PorositeViewModel PorositeViewModel) {
                    PorositeViewModel.SearchedArtikujt = new System.Collections.ObjectModel.ObservableCollection<Artikulli>(PorositeViewModel.Artikujt.Where(x => x.Emri.ToLower().Contains(e.NewTextValue.ToLower())));
                    searchedKrijoArtikujt.IsVisible = true;
                    allKrijoArtikujt.IsVisible = false;
                    if (string.IsNullOrEmpty(e.NewTextValue)) {
                        searchedKrijoArtikujt.IsVisible = false;
                        allKrijoArtikujt.IsVisible = true;
                    }
                }
                    if (BindingContext is LevizjetViewModel levizjetViewModel) {
                    levizjetViewModel.SearchedArtikujt = new System.Collections.ObjectModel.ObservableCollection<Artikulli>(levizjetViewModel.Artikujt.Where(x => x.Emri.ToLower().Contains(e.NewTextValue.ToLower())));
                    searchedArtikujt.IsVisible = true;
                    allArtikujt.IsVisible = false;
                }
                if (!string.IsNullOrEmpty(e.NewTextValue)) {
                    var bc = (ShitjaViewModel)BindingContext;
                    if (bc != null) {
                        bc.SearchedArtikujt = new System.Collections.ObjectModel.ObservableCollection<Artikulli>(bc.Artikujt.Where(x => x.Emri.ToLower().Contains(e.NewTextValue.ToLower())));
                        searchedArtikujt.IsVisible = true;
                        allArtikujt.IsVisible = false;
                    }
                }
                else {
                    searchedArtikujt.IsVisible = false;
                    allArtikujt.IsVisible = true;
                    
                }
                }catch (Exception ex) {

            }

        }

        ViewCell lastCell;
        private void ViewCell_Tapped(object sender, EventArgs e) {
            if(lastCell != null)
                lastCell.View.BackgroundColor = Color.Transparent;
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null) {
                viewCell.View.BackgroundColor = Color.Gray;
                lastCell = viewCell;
            }
        }


    }
}