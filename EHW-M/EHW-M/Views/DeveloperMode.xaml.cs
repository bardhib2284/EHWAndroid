using EHW_M;
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
	public partial class DeveloperMode : ContentPage
	{
		public DeveloperMode ()
		{
			InitializeComponent ();
			pickerinjo.ItemsSource = new List<string> { "Zgjedh njeren nga modelet", "Numri Fiskal", "Vizitat" };
		}

        protected override async void OnAppearing() {
            base.OnAppearing();
			var bc = (MainViewModel)BindingContext;
			bc.NumratFiskalDevMode = new System.Collections.ObjectModel.ObservableCollection<Models.NumriFisk>( await App.Database.GetNumratFiskalAsync());
			bc.NumratFiskalDevMode = new System.Collections.ObjectModel.ObservableCollection<Models.NumriFisk>( bc.NumratFiskalDevMode.Where(x => x.TCRCode == bc.Configurimi.KodiTCR));
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e) {
            var bc = (MainViewModel)BindingContext;
			bc.SelectedNumriFiskDevMode = bc.NumratFiskalDevMode.FirstOrDefault(x => x.TCRCode == (e.Item as Models.NumriFisk).TCRCode);
            await App.Instance.PushAsyncNewPage(new NumriFiskDevMode() { BindingContext = bc });
        }
    }
}