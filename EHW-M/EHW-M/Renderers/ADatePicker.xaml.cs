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
        public ObservableCollection<Vizita> LastSearchedVizitat { get; set; }
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
            pickerinho.SelectedIndex = DitetEJaves.IndexOf(DitetEJaves.FirstOrDefault(x=> x.Date.Date == DateTime.Now.Date));
            if(pickerinho.SelectedItem == null) {
                pickerinho.SelectedIndex = 0;
            }
            SelectedDayAndDate = pickerinho.SelectedItem as DayAndDate;
        }

        public void FixData() {
            if (App.Instance.MainPage is NavigationPage np) {
                if (np.CurrentPage is ClientsPage cp) {
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
        }
        public void CreateDitetEJaves() {
            var bc = App.Instance.MainViewModel;
            DitetEJaves.Clear();
            DitetEJaves.Add(new DayAndDate { Date = DateTime.MinValue, Day = "Java" });
            var index = 0;
            foreach (DateTime day in EachDay(bc.FilterMinDate, bc.FilterMaxDate)) {
                if (index == 7)
                    break;
                DitetEJaves.Add(new DayAndDate { Day = bc.GetDayName(day), Date =  day});
                index++;
            }
        }
        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru) {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        public string GetDayName(DateTime date) {
            string _ret = string.Empty;
            var culture = new System.Globalization.CultureInfo("sq-AL");
            _ret = culture.DateTimeFormat.GetDayName(date.DayOfWeek);
            _ret = culture.TextInfo.ToTitleCase(_ret.ToLower());
            return _ret;
        }
        private void Pickerinho_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                var bc = App.Instance.MainViewModel;
                bc.ADatePicker = this;
                SelectedDayAndDate = DitetEJaves[pickerinho.SelectedIndex] as DayAndDate;
                CurrentDate = DitetEJaves[pickerinho.SelectedIndex].Date;
                if (App.Instance.MainPage is NavigationPage np) {
                    if (np.CurrentPage is ClientsPage cp) {
                        bc.SearchedVizitat.Clear();
                        if (CurrentDate == DateTime.MinValue) {
                            foreach (var date in DitetEJaves) {
                                if (date.Day == "Java")
                                    continue;
                                foreach (var viz in bc.VizitatFilteredByDate) {
                                    if (viz.DataPlanifikimit.Value.Date == date.Date.Date && viz.DataPlanifikimit.Value.Year == date.Date.Year && viz.DataPlanifikimit.Value.Month == date.Date.Month) {
                                        bc.SearchedVizitat.Add(viz);
                                    }
                                }
                            }
                            cp.AllClientsListVisibility(false);
                            cp.SearchedClientsListVisibility(true);
                            bc.FilterDate = CurrentDate;
                            bc.AllClientsList = false;
                            bc.SearchedClientsList = true;
                            bc.SearchedVizitat = new ObservableCollection<Vizita>(bc.SearchedVizitat.OrderByDescending(x => x.IDStatusiVizites));
                            LastSearchedVizitat = new ObservableCollection<Vizita>(bc.SearchedVizitat.OrderByDescending(x => x.IDStatusiVizites));
                            return;
                        }
                        else {
                            bc.AllClientsList = true;
                            bc.SearchedClientsList = false;
                        }
                    }
                }

                bc.FilterDate = CurrentDate;
                bc.SearchedVizitat = new System.Collections.ObjectModel.ObservableCollection<Vizita>(bc.VizitatFilteredByDate.Where(x => x.DataPlanifikimit.Value.Date == bc.FilterDate.Date));
                bc.SearchedVizitat = new ObservableCollection<Vizita>(bc.SearchedVizitat.OrderByDescending(x => x.IDStatusiVizites));
                LastSearchedVizitat = new ObservableCollection<Vizita>(bc.SearchedVizitat.OrderByDescending(x => x.IDStatusiVizites));
                bc.AllClientsList = false;
                bc.SearchedClientsList = true;

            }
            catch(Exception ex) {

            }
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