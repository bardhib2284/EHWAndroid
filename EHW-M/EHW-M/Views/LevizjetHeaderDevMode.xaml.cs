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
    public partial class LevizjetHeaderDevMode : ContentPage
    {
        public LevizjetHeaderDevMode()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var bc = (MainViewModel)BindingContext;
            var res = await App.Database.UpdateLevizjeHeaderAsync(bc.SelectedLevizjaHeaderDevMode);
            if (res > 0)
            {
                UserDialogs.Instance.Alert("Levizja Header U Editua Me Sukses");
                await App.Instance.PopPageAsync();
            }
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            var bc = (MainViewModel)BindingContext;
            await App.Instance.PushAsyncNewPage(new LevizjetDetailsDevMode() { BindingContext = bc });
        }
    }
}