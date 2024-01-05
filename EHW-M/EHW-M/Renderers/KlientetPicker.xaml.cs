using EHW_M;
using EHWM.Models;
using EHWM.ViewModel;
using EHWM.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EHWM.Renderers {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class KlientetPicker : ContentView {
        public KlientetPicker() {
            InitializeComponent();
            labeltext.SetBinding(Entry.TextProperty, new Binding("CurrentKlient.Emri", source: this));

        }


        private void TapGestureRecognizer_Tapped(object sender, EventArgs e) {
            if(App.Instance.MainPage is NavigationPage np) {
                if(np.CurrentPage is KrijoPorosinePage kp) {
                    var bc = (PorositeViewModel)kp.BindingContext;
                    bc.ZgjedhKlientet();
                }
                if(np.CurrentPage is InkasimiPage ip) {
                    var bc = (InkasimiViewModel)ip.BindingContext;
                    bc.ZgjedhKlientet();
                }
            }
        }

        public static readonly BindableProperty CurrentKlientProperty =
        BindableProperty.Create("CurrentKlient", typeof(Klientet), typeof(KlientetPicker), default(Klientet), BindingMode.TwoWay);

        public Klientet CurrentKlient {
            get { return (Klientet)GetValue(CurrentKlientProperty); }
            set { SetValue(CurrentKlientProperty, value); }
        }
    }
}