using System;
using Android.OS;
using Xamarin.Forms;
using System.Threading.Tasks;
using Android.Graphics;
using ZXing.QrCode;
using ZXing;
using ZXing.Mobile;
using ZXing.Common;
using static Android.Icu.Text.ListFormatter;
using System.IO;

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

        public object GenerateQRCode(string qrCode) {
            BitMatrix bitmapMatrix = null;
            
            bitmapMatrix = new MultiFormatWriter().encode(qrCode, BarcodeFormat.QR_CODE, 400, 400);

            var width = bitmapMatrix.Width;
            var height = bitmapMatrix.Height;
            int[] pixelsImage = new int[width * height];

            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    if (bitmapMatrix[j, i])
                        pixelsImage[i * width + j] = (int)Convert.ToInt64(0xff000000);
                    else
                        pixelsImage[i * width + j] = (int)Convert.ToInt64(0xffffffff);

                }
            }

            Bitmap bitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
            bitmap.SetPixels(pixelsImage, 0, width, 0, 0, width, height);
            return bitmap;
        }
    }
}