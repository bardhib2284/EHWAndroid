using EHW_M;
using EHWM.Converters;
using EHWM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EHWM.Renderers
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ADatePickerDate : ContentView
	{
		public ADatePickerDate ()
		{
			InitializeComponent ();
            datepicker.SetBinding(DatePicker.DateProperty, new Binding("CurrentDate", source: this));
            labeltext.SetBinding(Label.TextProperty, new Binding("CurrentDate",converter: new DateTimeToStringConverter(), source: this));
            datepicker.DateSelected += Datepicker_DateSelected;
        }

        private void Datepicker_DateSelected(object sender, DateChangedEventArgs e) {
            var bc = App.Instance.MainViewModel;
            if (bc != null) {
                bc.FixDataVizualizimit();
            }
        }

        public static readonly BindableProperty CurrentDateProperty =
BindableProperty.Create("CurrentDate", typeof(DateTime), typeof(ADatePicker), default(DateTime), BindingMode.TwoWay);


        private void TapGestureRecognizer_Tapped(object sender, EventArgs e) {
            datepicker.Focus();
        }

        public DateTime CurrentDate {
            get { return (DateTime)GetValue(CurrentDateProperty); }
            set { SetValue(CurrentDateProperty, value); }
        }
    }
}