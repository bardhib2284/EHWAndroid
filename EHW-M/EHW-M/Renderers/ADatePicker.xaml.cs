using EHW_M;
using EHWM.Converters;
using EHWM.Models;
using EHWM.ViewModel;
using EHWM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EHWM.Renderers
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ADatePicker : ContentView, INotifyPropertyChanged {
        public ObservableCollection<DayAndDate> DitetEJaves { get; set; }
        
        private DayAndDate _date;
        public DayAndDate SelectedDayAndDate {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value,
                                        [CallerMemberName] string propertyName = null) {
            if (Object.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ADatePicker ()
		{
			InitializeComponent();
            DitetEJaves = new ObservableCollection<DayAndDate>();
            datepicker.SetBinding(DatePicker.DateProperty, new Binding("CurrentDate", source: this));
            labeltext.SetBinding(Label.TextProperty, new Binding("SelectedDayAndDate.Day", source: this));
            datepicker.DateSelected += DatePicker_DateSelected;
            pickerinho.ItemsSource = DitetEJaves;
            pickerinho.SelectedIndexChanged += Pickerinho_SelectedIndexChanged;
            CreateDitetEJaves();
            pickerinho.SelectedIndex = 0;
            SelectedDayAndDate = pickerinho.SelectedItem as DayAndDate;
        }

        public void CreateDitetEJaves() {
            var bc = App.Instance.MainViewModel;
            DitetEJaves.Clear();
            DitetEJaves.Add(new DayAndDate { Date = DateTime.MinValue, Day = "Java" });
            var index = 0;
            foreach (DateTime day in EachDay(bc.FilterMinDate, bc.FilterMaxDate)) {
                if (index == 6)
                    break;
                DitetEJaves.Add(new DayAndDate { Day = bc.GetDayName(day), Date =  day});
                index++;
            }
        }
        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru) {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
        
        private void Pickerinho_SelectedIndexChanged(object sender, EventArgs e) {
            var bc = App.Instance.MainViewModel;
            SelectedDayAndDate = DitetEJaves[pickerinho.SelectedIndex] as DayAndDate;
            CurrentDate = DitetEJaves[pickerinho.SelectedIndex].Date;
            if(App.Instance.MainPage is NavigationPage np) {
                if(np.CurrentPage is ClientsPage cp) {
                    if (CurrentDate == DateTime.MinValue) {
                        cp.AllClientsListVisibility(true);
                        cp.SearchedClientsListVisibility(false);
                    }
                    else {
                        cp.AllClientsListVisibility(false);
                        cp.SearchedClientsListVisibility(true);
                    }
                }
            }
                
            bc.FilterDate = CurrentDate;
            bc.SearchedVizitat = new System.Collections.ObjectModel.ObservableCollection<Vizita>(bc.VizitatFilteredByDate.Where(x => x.DataPlanifikimit.Value.Day == bc.FilterDate.Day));
            bc.AllClientsList = false;
            bc.SearchedClientsList = true;
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e) {
            var bc = App.Instance.MainViewModel;

            
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e) {
			pickerinho.Focus();
        }

        public static readonly BindableProperty CurrentDateProperty =
        BindableProperty.Create("CurrentDate", typeof(DateTime), typeof(ADatePicker), default(DateTime), BindingMode.TwoWay);

        public DateTime CurrentDate {
            get { return (DateTime)GetValue(CurrentDateProperty); }
            set { SetValue(CurrentDateProperty, value); }
        }
    }
}