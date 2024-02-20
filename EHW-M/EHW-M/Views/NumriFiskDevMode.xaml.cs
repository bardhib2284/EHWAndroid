using Acr.UserDialogs;
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
	public partial class NumriFiskDevMode : ContentPage
	{
		public NumriFiskDevMode (string fiskorfat)
		{
			InitializeComponent ();
			if(fiskorfat == "fisk") {
				fisk.IsVisible = true;
				fat.IsVisible = false;
			}
			else if (fiskorfat == "fat") {
				fat.IsVisible = true;
				fisk.IsVisible = false;
			}
		}

        private async void Button_Clicked(object sender, EventArgs e) {
			if(fisk.IsVisible) {
                var bc = (MainViewModel)BindingContext;
                var res = await App.Database.UpdateNumriFiskalAsync(bc.SelectedNumriFiskDevMode);
                if (res > 0) {
                    UserDialogs.Instance.Alert("Numri Fiskal U Editua Me Sukses");
                    await App.Instance.PopPageAsync();
                }
            }

			if(fat.IsVisible) {
                var bc = (MainViewModel)BindingContext;
                var res = await App.Database.UpdateNumriFaturave(bc.SelectedNumriFatDevMode);
                if (res > 0) {
                    UserDialogs.Instance.Alert("Numri i Fatures U Editua Me Sukses");
                    await App.Instance.PopPageAsync();
                }
            }
        }
    }
}