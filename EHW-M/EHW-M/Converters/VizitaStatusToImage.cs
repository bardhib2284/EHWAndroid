using EHW_M;
using EHWM.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace EHWM.Converters {
    public class VizitaStatusToImage : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if((string)value == "1") {
                return ImageSource.FromFile("inProgressTick.png");
            }
            if ((string)value == "0" ) {
                return ImageSource.FromFile("notStartedIcon.png");
            }
            if ((string)value == "6" ) {
                return ImageSource.FromFile("tickIcon.png");
            }
            return ImageSource.FromFile("tickIcon.png");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return !((bool)value);
        }
    }


    public class VizitaStatusToInt : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            int status = 1;
            var statuesEVizites = App.Instance.MainViewModel.StatusetEVizites;
            if(value == statuesEVizites[0]) {
                status = 1;
            }
            else if(value == statuesEVizites[1]) {
                status = 2;
            }
            else if(value == statuesEVizites[2]) {
                status = 3;
            }
            else if(value == statuesEVizites[3]) {
                status = 4;
            }
            else if(value == statuesEVizites) {
                status = 5;
            }
            else if(value == statuesEVizites[5]) {
                status = 6;
            }
            return status;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            int status = 0;
            var statuesEVizites = App.Instance.MainViewModel.StatusetEVizites;
            if (value == statuesEVizites[0]) {
                status = 1;
            }
            else if (value == statuesEVizites[1]) {
                status = 2;
            }
            else if (value == statuesEVizites[2]) {
                status = 3;
            }
            else if (value == statuesEVizites[3]) {
                status = 4;
            }
            else if (value == statuesEVizites[4]) {
                status = 5;
            }
            else if (value == statuesEVizites[5]) {
                status = 6;
            }
            return status;
        }
    }

    public class ArsyejaToString : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            string status = "";
            var arsyejet = App.Instance.MainViewModel.Arsyejet;
            if(value == null) {
                value = "0";
            }
            switch(value.ToString()) {
                case "0":
                    status = arsyejet[0].Pershkrimi;
                    break;
                case "1":
                    status = arsyejet[1].Pershkrimi;
                    break;
            }
            return status;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            string status = "";
            switch (value.ToString()) {
                case "0":
                case "1":
                    status = "Njesi e mbyllur";
                    break;
                case "2":
                    status = "Ka gjendje malli";
                    break;
                case "3":
                    status = "Me vone";
                    break;
                case "4":
                case "5":
                    status = "Marketing";
                    break;
                case "6":
                    status = "Perfunduar";
                    break;
            }
            return status;
        }
    }

    public class VizitaStatusToString : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            string status = "";
            var statusetEVizites = App.Instance.MainViewModel.StatusetEVizites;
            switch(value.ToString()) {
                case "0":
                case "1":
                    status = statusetEVizites[0].Gjendja;
                    break;
                case "2":
                    status = statusetEVizites[1].Gjendja;
                    break;
                case "3":
                    status = statusetEVizites[2].Gjendja;
                    break;
                case "4":
                    status = statusetEVizites[3].Gjendja;
                    break;
                case "5":
                    status = statusetEVizites[4].Gjendja;
                    break;
                case "6":
                    status = statusetEVizites[5].Gjendja;
                    break;
            }
            return status;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            string status = "";
            var statusetEVizites = App.Instance.MainViewModel.StatusetEVizites;
            switch (value.ToString()) {
                case "0":
                case "1":
                    status = statusetEVizites[0].Gjendja;
                    break;
                case "2":
                    status = statusetEVizites[1].Gjendja;
                    break;
                case "3":
                    status = statusetEVizites[2].Gjendja;
                    break;
                case "4":
                    status = statusetEVizites[3].Gjendja;
                    break;
                case "5":
                    status = statusetEVizites[4].Gjendja;
                    break;
                case "6":
                    status = statusetEVizites[5].Gjendja;
                    break;
            }
            return status;
        }
    }

    public class DateTimeToStringConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return ((DateTime)value).ToString("dd.MM.yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return ((DateTime)value).ToString("dd.MM.yyyy");
        }
    }

    public class NegateBooleanConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return !((bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return !((bool)value);
        }
    }
    public class PayTypeVisibility : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value.ToString() == "KESH")
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value.ToString() == "KESH")
                return true;
            else
                return false;
        }
    }

    public class StringEqualConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return value?.ToString().Equals(parameter?.ToString()) ?? false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return value?.ToString().Equals(parameter?.ToString()) ?? false;
        }
    }

    public class BoolToStringConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if ((bool)value)
                return "Po";
            else
                return "Jo";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return value?.ToString().Equals(parameter?.ToString()) ?? false;
        }
    }
}
