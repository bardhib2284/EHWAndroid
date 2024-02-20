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
		}

        protected override async void OnAppearing() {
            base.OnAppearing();
			var bc = (MainViewModel)BindingContext;
			bc.NumratFiskalDevMode = new System.Collections.ObjectModel.ObservableCollection<Models.NumriFisk>( await App.Database.GetNumratFiskalAsync());
			bc.NumratFiskalDevMode = new System.Collections.ObjectModel.ObservableCollection<Models.NumriFisk>( bc.NumratFiskalDevMode.Where(x => x.TCRCode == bc.Configurimi.KodiTCR));
			bc.LiferimetDevMode = new System.Collections.ObjectModel.ObservableCollection<Models.Liferimi>(await App.Database.GetLiferimetAsync());
            bc.NumratFaturaveDevMode = new System.Collections.ObjectModel.ObservableCollection<Models.NumriFaturave>(await App.Database.GetNumriFaturaveAsync());
            bc.NumratFaturaveDevMode = new System.Collections.ObjectModel.ObservableCollection<Models.NumriFaturave>(bc.NumratFaturaveDevMode.Where(x=> x.KOD == bc.Configurimi.Shfrytezuesi));
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e) {
            var bc = (MainViewModel)BindingContext;
			if(e.Item is Models.NumriFisk) {
                bc.SelectedNumriFiskDevMode = bc.NumratFiskalDevMode.FirstOrDefault(x => x.TCRCode == (e.Item as Models.NumriFisk).TCRCode);
                await App.Instance.PushAsyncNewPage(new NumriFiskDevMode("fisk") { BindingContext = bc });
            }
			else if (e.Item is Models.Liferimi) {
                bc.SelectedLiferimiDevMode = bc.LiferimetDevMode.FirstOrDefault(x => x.IDLiferimi == (e.Item as Models.Liferimi).IDLiferimi);
                var liferimetArt = await App.Database.GetLiferimetArtAsync();
                bc.LiferimetArtDevMode = new System.Collections.ObjectModel.ObservableCollection<Models.LiferimiArt>(liferimetArt.Where(x => x.IDLiferimi == bc.SelectedLiferimiDevMode.IDLiferimi));
                await App.Instance.PushAsyncNewPage(new LiferimiDevMode() { BindingContext = bc });
            }
			else if (e.Item is Models.NumriFaturave) {
                bc.SelectedNumriFatDevMode = bc.NumratFaturaveDevMode.FirstOrDefault(x => x.KOD == (e.Item as Models.NumriFaturave).KOD);
                await App.Instance.PushAsyncNewPage(new NumriFiskDevMode("fat") { BindingContext = bc });
            }
        }
    }
}