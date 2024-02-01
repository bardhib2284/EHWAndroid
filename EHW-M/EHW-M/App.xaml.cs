using EHWM.ViewModel;
using EHWM.Services;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EHW_M.Services;
using EHWM.Views;
using Acr.UserDialogs;
using Plugin.Connectivity;
using Xamarin.Essentials;
using EHWM.DependencyInjections;
using EHWM.DependencyInjections.FiskalizationExtraModels;
using System.Globalization;

namespace EHW_M {
    public partial class App : Application {

        public static App Instance { get; private set; }
        public static HttpClient ApiClient { get; set; }
        private static Database database;
        public static Database Database {
            get {
                if(database == null) {
                    database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"ehw-local.db3"));
                }
                return database;
            }
        }
        //public const string API_URL_BASE = "http://84.22.42.59:8188/ehwapi/";
        public string API_URL_BASE;

        public MainViewModel MainViewModel { get; set; }
        public ShitjaViewModel ShitjaViewModel { get; set; }

        public IFiskalizationService FiskalizationService { get; set; }
        public string WebServerFiskalizimiUrl { get; set; }
        public App() {
            if (Instance == null) {
                Instance = this;
            }
            InitializeComponent();
            ApiClient = new HttpClient();
            ApiClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            LoginPage mp = new LoginPage();
            MainViewModel = new MainViewModel();
            NavigationPage navigationPage = new NavigationPage(mp) { BarBackgroundColor = Color.LightBlue };
            mp.BindingContext = MainViewModel;
            MainPage = navigationPage;
            //RegisterCashDepositInputRequestPCL registerCashDepositInputRequestPCL = new RegisterCashDepositInputRequestPCL
            //{
            //    CashAmount = 0.00m,
            //    DepositType = CashDepositOperationSTypePCL.DEPOSIT,
            //    OperatorCode = "gs818yh908",
            //    SendDateTime = DateTime.Now,
            //    SubseqDelivTypeSType = 1,
            //    TCRCode = "fa248ng165"
            //};
            FiskalizationService = DependencyService.Get<IFiskalizationService>();
        }

        protected override void OnStart() {
        }

        protected override void OnSleep() {
        }

        protected override void OnResume() {
        }

        public byte[] Content;
        public void SetPath(byte[] path) {
            Content = path;
        }

        public async Task PushAsyncNewPage(Page page) {
            if(MainPage is FlyoutPage) {
                await (MainPage as FlyoutPage).Detail.Navigation.PushAsync(page);
                if ((MainPage as FlyoutPage).IsPresented) {
                    (MainPage as FlyoutPage).IsPresented = false;
                }
                return;
            }
            await MainPage.Navigation.PushAsync(page);
        }


        public async Task PushAsyncNewModal(Page page) {
            if (MainPage is FlyoutPage) {
                await (MainPage as FlyoutPage).Detail.Navigation.PushAsync(page);
                if ((MainPage as FlyoutPage).IsPresented) {
                    (MainPage as FlyoutPage).IsPresented = false;
                }
                return;
            }
            await MainPage.Navigation.PushModalAsync(page);
        }

        public async Task PopPageAsync() {
            if (MainPage is FlyoutPage) {
                await (MainPage as FlyoutPage).Detail.Navigation.PopAsync();
                if ((MainPage as FlyoutPage).IsPresented) {
                    (MainPage as FlyoutPage).IsPresented = false;
                }
                return;
            }
            await MainPage.Navigation.PopAsync();
        }

        public async Task PopAsyncModal() {
            if (MainPage is FlyoutPage) {
                await (MainPage as FlyoutPage).Detail.Navigation.PopModalAsync();
                if ((MainPage as FlyoutPage).IsPresented) {
                    (MainPage as FlyoutPage).IsPresented = false;
                }
                return;
            }
            await MainPage.Navigation.PopModalAsync();
        }

        public bool DoIHaveInternet() {
            var connected = CrossConnectivity.Current.IsConnected;
            if (connected) return true;
            else
                UserDialogs.Instance.Alert("Ju lutem kontrolloni interrnetin dhe provoni perseri", "Error", "Ok");
            return false;
        }
        public bool DoIHaveInternetNoAlert() {
            var connected = CrossConnectivity.Current.IsConnected;
            if (connected) return true;
            else
                return false;
        }

    }
}
