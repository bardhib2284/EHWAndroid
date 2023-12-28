using System;
using Acr.UserDialogs;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;

using Android.Content;
using Android.Content.Res;
using EHWM.Services;
using Xamarin.Forms;
using System.Collections.Generic;
using Xamarin.Essentials;
using System.IO;

namespace EHW_M.Droid
{
    [Activity(Icon = "@drawable/logo", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            Rg.Plugins.Popup.Popup.Init(this);
            base.OnCreate(savedInstanceState);
            UserDialogs.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            var platform = DependencyService.Get<IPlatformInfo>();
            if ((platform as PlatformInfo) != null) {
                platform.AndroidContext = this;
                platform.AndroidResource = Resources;
            }
            byte[] content;
            const int maxReadSize = 256 * 2048;
            AssetManager assets = this.Assets;
            using (BinaryReader sr = new BinaryReader(assets.Open("ASSECOSEE.p12"))) {
                content = sr.ReadBytes(maxReadSize);
            }
            var App = new App();
            App.SetPath(content);
            LoadApplication(App);
            await Permissions.RequestAsync<BLEPermission>();

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed() {
            Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed);
        }

        public Context GetApplicationContext() {
            return (Context)this;
        }

    }

}