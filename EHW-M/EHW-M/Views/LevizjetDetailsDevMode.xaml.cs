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
    public partial class LevizjetDetailsDevMode : ContentPage
    {
        public LevizjetDetailsDevMode()
        {
            InitializeComponent();
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var bc = (MainViewModel)BindingContext;
            if (e.Item is Models.LevizjetDetails)
            {
                bc.SelectedLevizjetDetailsDevMode = bc.LevizjetDetailsDevMode.FirstOrDefault(x => x.IDArtikulli == (e.Item as Models.LevizjetDetails).IDArtikulli);
                await App.Instance.PushAsyncNewPage(new LevizjetDetailsDetailsDevMode() { BindingContext = bc });
            }
        }
    }
}