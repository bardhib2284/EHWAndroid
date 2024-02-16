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
		public NumriFiskDevMode ()
		{
			InitializeComponent ();
		}

        private async void Button_Clicked(object sender, EventArgs e) {
			var bc = (MainViewModel)BindingContext;
			var res = await App.Database.UpdateNumriFiskalAsync(bc.SelectedNumriFiskDevMode);
			if(res > 0) {
				UserDialogs.Instance.Alert("Numri Fiskal U Editua Me Sukses");
				await App.Instance.PopPageAsync();
			}
        }
    }
}