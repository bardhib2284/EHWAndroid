using EHW_M;
using EHWM.ViewModel;
using Plugin.BxlMpXamarinSDK;
using Plugin.BxlMpXamarinSDK.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EHWM.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrinterSelectionPage : ContentPage {
        public PrinterSelectionPage() {
            InitializeComponent();
            GetConnectionInfo(0);
        }

        public async void GetConnectionInfo(int selectedIndex) {
            MposLookup mPosLookup = MPosLookupUtil.Current;

            var interfaceType = MPosInterfaceType.MPOS_INTERFACE_BLUETOOTH;
            switch (selectedIndex) {
                case 0: interfaceType = MPosInterfaceType.MPOS_INTERFACE_BLUETOOTH; break;
                case 1: interfaceType = MPosInterfaceType.MPOS_INTERFACE_WIFI; break;
                case 2: interfaceType = MPosInterfaceType.MPOS_INTERFACE_ETHERNET; break;
            }

            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;
            var result = await mPosLookup.refreshDeivcesList((int)interfaceType, 3 /*second*/);
            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;

            if (result == (int)MPosResult.MPOS_SUCCESS) {
                var deviceList = mPosLookup.getDeviceList((int)interfaceType);
                lstView.ItemsSource = deviceList;
            }
            else {
                lstView.ItemsSource = null;
                await DisplayAlert("No Items", "No Device found", "OK");
                //await Navigation.PopAsync();
                //return;
            }
        }
        async void OnSelection(object sender, SelectedItemChangedEventArgs e) {
            
            
            
            if (e.SelectedItem == null)
                return;
            var bc = BindingContext;
            if(bc is MainViewModel mv) {
                var info = e.SelectedItem as MposConnectionInformation;
                if(info != null) {
                    mv._connectionInfo = info;
                    await mv.OnDeviceOpenClicked();
                    await Navigation.PopAsync();
                }
            }            
            else if(bc is LevizjetViewModel lmv) {
                var info = e.SelectedItem as MposConnectionInformation;
                if(info != null) {
                    lmv._connectionInfo = info;
                    await lmv.OnDeviceOpenClicked();
                    await Navigation.PopAsync();
                }
            }            
            else if(bc is InkasimiViewModel imv) {
                var info = e.SelectedItem as MposConnectionInformation;
                if(info != null) {
                    imv._connectionInfo = info;
                    await imv.OnDeviceOpenClicked();
                    await Navigation.PopAsync();
                }
            }            
            else if(bc is PorositeViewModel pmv) {
                var info = e.SelectedItem as MposConnectionInformation;
                if(info != null) {
                    pmv._connectionInfo = info;
                    await pmv.OnDeviceOpenClicked();
                    await Navigation.PopAsync();
                }
            }
            else {
                var info = e.SelectedItem as MposConnectionInformation;

                if (info != null) {
                    App.Instance.ShitjaViewModel._connectionInfo = info;
                }
                await App.Instance.ShitjaViewModel.OnDeviceOpenClicked();
                await Navigation.PopAsync();
            }
            
        }
    }
}