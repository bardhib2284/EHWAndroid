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

namespace EHWM.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ZgjidhKlientetModalPage : ContentPage
	{
		public ZgjidhKlientetModalPage ()
		{
			InitializeComponent ();
		}

        private async void testList_ItemTapped(object sender, ItemTappedEventArgs e) {
            if (e.Item != null) {
                if (BindingContext is ShitjaViewModel viewModel) {
                }
                else if (BindingContext is LevizjetViewModel levizjetViewModel) {
                    if (levizjetViewModel != null) {
                        levizjetViewModel.SelectedKlientet = e.Item as Depot;
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
                        porositeViewModel.SelectedKlient = e.Item as Klientet;
                        App.Instance.PopAsyncModal();
                    }
                }                
                else if (BindingContext is InkasimiViewModel InkasimiViewModel) {
                    InkasimiViewModel.SelectedKlient = e.Item as Klientet;
                    InkasimiViewModel.MerrDetyrimet(e.Item as Klientet);
                    App.Instance.PopAsyncModal();
                }
            }
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e) {
            try {
                if (!string.IsNullOrEmpty(e.NewTextValue)) {
                    if(BindingContext is PorositeViewModel) {
                        var bc = (PorositeViewModel)BindingContext;
                        if (bc != null) {
                            bc.SearchedKlientet = new System.Collections.ObjectModel.ObservableCollection<Klientet>(bc.Klientet.Where(x => x.Emri.ToLower().Contains(e.NewTextValue.ToLower())));
                            searchedArtikujt.IsVisible = true;
                            allArtikujt.IsVisible = false;
                        }
                    }
                    else if (BindingContext is InkasimiViewModel) {
                        var bc = (InkasimiViewModel)BindingContext;
                        if (bc != null) {
                            bc.SearchedKlientet = new System.Collections.ObjectModel.ObservableCollection<Klientet>(bc.Klientet.Where(x => x.Emri.ToLower().Contains(e.NewTextValue.ToLower())));
                            searchedArtikujt.IsVisible = true;
                            allArtikujt.IsVisible = false;
                        }
                    }
                }
                else {
                    searchedArtikujt.IsVisible = false;
                    allArtikujt.IsVisible = true;
                }
            }catch(Exception x) { }

        }
    }
}