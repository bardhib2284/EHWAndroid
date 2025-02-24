using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EHWM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DebugModalPage : ContentPage
    {
        public DebugModalPage(string textToWrite)
        {
            InitializeComponent();
            debug.Text = textToWrite;
        }
    }
}