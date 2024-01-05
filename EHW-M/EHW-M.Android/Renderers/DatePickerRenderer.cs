using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using EHW_M.Droid.Renderers;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(DatePickerRenderer), typeof(DatePickerRendererAndroid))]
namespace EHW_M.Droid.Renderers {
    public class DatePickerRendererAndroid : DatePickerRenderer {
        public DatePickerRendererAndroid(Context context) : base(context) {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e) {
            base.OnElementChanged(e);
            Locale locale = new Locale("fr");
            Control.TextLocale = locale;
            Android.Content.Res.Configuration config = new Android.Content.Res.Configuration();
            config.Locale = locale;
            Locale.SetDefault(Locale.Category.Format, locale);
            Resources.Configuration.SetLocale(locale);
            Resources.Configuration.Locale = locale;
        }
    }
}