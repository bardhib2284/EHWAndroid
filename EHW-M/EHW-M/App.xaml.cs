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
using System.Diagnostics;
using EHWM.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Net.Http.Headers;

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

            var minutes = TimeSpan.FromMinutes(0.2f);

            Device.StartTimer(minutes, () => {
                Device.BeginInvokeOnMainThread(
                    async () =>
                    {
                        var liferimet = await Database.GetLiferimetAsync();
                        await CreateUpdateScriptLiferimi(liferimet);
                        //var malliMbetur = await Database.GetMalliMbeturAsync();
                        //await CreateUpdateScriptMalli_Mbetur(malliMbetur);
                    });

                return true;
            });
        }
        private async Task<bool> CreateUpdateScriptMalli_Mbetur(List<Malli_Mbetur> OrderDetasails) {
            foreach (var mall in OrderDetasails) {
                mall.Data = DateTime.Now;
                mall.Export_Status = 2;
            }
            var conf = await Database.GetConfigurimiAsync();
            if (string.IsNullOrEmpty(API_URL_BASE)) {
                API_URL_BASE = conf.Serveri;
            }
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(API_URL_BASE);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", conf.Token);
            var OrderDetailsJson = JsonConvert.SerializeObject(OrderDetasails);
            var stringContent = new StringContent(OrderDetailsJson, Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync("malli-mbetur", stringContent);
            if (result.IsSuccessStatusCode) {
                return true;
            }
            foreach (var porosia in OrderDetasails) {
                porosia.SyncStatus = 0;
                porosia.Export_Status = 0;
            }
            await App.Database.UpdateAllMalliMbeturAsync(OrderDetasails);
            return false;
        }

        private async Task<bool> CreateUpdateScriptLiferimi(List<Liferimi> Liferimi) {
            try {
                foreach (var lif in Liferimi) {
                    lif.Export_Status = 2;
                    DateTime MyTime = DateTime.UtcNow;

                    DateTime MyTimeInWesternEurope = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(MyTime, "GMT Standard Time").AddHours(1);
                    lif.KohaLiferimit = MyTimeInWesternEurope;
                }
                var vizitatJson = JsonConvert.SerializeObject(Liferimi);
                var conf = await Database.GetConfigurimiAsync();
                if (string.IsNullOrEmpty(API_URL_BASE)) {
                    API_URL_BASE = conf.Serveri;
                }
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(API_URL_BASE);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", conf.Token);
                var stringContent = new StringContent(vizitatJson, Encoding.UTF8, "application/json");
                var result = await httpClient.PostAsync("liferimi", stringContent);
                if (result.IsSuccessStatusCode) {
                    return true;
                }
                else {
                    foreach (var viz in Liferimi) {
                        viz.SyncStatus = 0;
                        viz.Export_Status = 0;
                        await App.Database.SaveLiferimiAsync(viz);
                    }
                }
                return false;
            }catch(Exception e) {
                if(e is UriFormatException ufe) {
                    UserDialogs.Instance.Alert("Linku I API't eshte gabim, ju lutemi rregullojeni linkun tek konfigurimi");
                    return false;
                }
                else {
                    UserDialogs.Instance.Alert("Gabim i panjohur, ju lutem raportojeni tek ASEE : " + e.Message);
                    return false;
                }
            }
            
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
