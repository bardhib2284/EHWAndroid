using System;
using Android.OS;
using Xamarin.Forms;
using System.Threading.Tasks;


[assembly: Dependency(typeof(EHW_M.Droid.PlatformInfo))]
namespace EHW_M.Droid {
    public class PlatformInfo : EHWM.Services.IPlatformInfo {
        public object AndroidContext { get; set; }
        public string GetPath() {
            Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            if (sdCard != null)
                return sdCard.AbsolutePath;
            return "";
        }
        public object GetImgResource() {
            Android.Graphics.Bitmap bitmap = Android.Graphics.BitmapFactory.DecodeResource((Android.Content.Res.Resources)AndroidResource, Resource.Drawable.logo);
            return bitmap;
        }

        public object AndroidResource { get; set; }


    }
}