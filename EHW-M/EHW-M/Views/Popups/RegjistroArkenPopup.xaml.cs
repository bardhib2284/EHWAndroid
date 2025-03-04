﻿using EHW_M;
using EHWM.ViewModel;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EHWM.Views.Popups {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegjistroArkenPopup : PopupPage {
        public RegjistroArkenPopup() {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e) {            

            if(BindingContext is MainViewModel mvm) {
                mvm.EshteRuajtuarArka = true;
                await App.Instance.MainPage.Navigation.PopPopupAsync(true);
            }

            if(BindingContext is SinkronizimiViewModel svm) {
                svm.EshteRuajtuarArka = true;
                await App.Instance.MainPage.Navigation.PopPopupAsync(true);
            }
            if (BindingContext is ShitjaViewModel lvm) {
                lvm.EditActive = false;
                lvm.ClosedEditPopup = true;
                await App.Instance.MainPage.Navigation.PopPopupAsync(true);
            }
        }

        public void Shitja(string newText) {
            shitjaStack.IsVisible = true;
            regjistroArkenStack.IsVisible = false;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (BindingContext is MainViewModel mvm)
            {
                mvm.EshteRuajtuarArka = true;
            }

            if (BindingContext is SinkronizimiViewModel svm)
            {
                svm.EshteRuajtuarArka = true;
            }
            if (BindingContext is ShitjaViewModel lvm)
            {
                lvm.EditActive = false;
            }
        }

    }
}