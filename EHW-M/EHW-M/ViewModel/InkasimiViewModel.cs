using Acr.UserDialogs;
using EHW_M;
using EHWM.Models;
using EHWM.Views;
using Newtonsoft.Json;
using Plugin.BxlMpXamarinSDK.Abstractions;
using Plugin.BxlMpXamarinSDK;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using EHWM.Services;
using System.Diagnostics;

namespace EHWM.ViewModel {

    public class InkasimiViewModelNavigationParameters {
        public List<Klientet> Klientet;
        public List<Detyrimet> Detyrimet;
        public List<EvidencaPagesave> EvidencaPagesave;
        public Agjendet Agjendi;
    }

    public class InkasimiViewModel : BaseViewModel{
        public List<Klientet> Klientet { get; set; }
        public List<Detyrimet> Detyrimet { get; set; }
        public List<EvidencaPagesave> EvidencaEPagesave { get; set; }
        public Agjendet Agjendi;

        public DateTime TodaysDate => DateTime.Now;

        public ICommand RegjistroCommand { get; set; }
        public InkasimiViewModel(InkasimiViewModelNavigationParameters inkasimiViewModelNavigationParameters) {
            Klientet = inkasimiViewModelNavigationParameters.Klientet.OrderBy(x=> x.Emri).ToList();
            Detyrimet = inkasimiViewModelNavigationParameters.Detyrimet;
            EvidencaEPagesave = inkasimiViewModelNavigationParameters.EvidencaPagesave;
            DetyrimetNgaKlienti = new ObservableCollection<Detyrimet>();
            RegjistroCommand = new Command(RegjistroAsync);
            Agjendi = inkasimiViewModelNavigationParameters.Agjendi;
        }

        public ObservableCollection<Detyrimet> DetyrimetNgaKlienti { get; set; }
        private decimal _shumaPaguar;
        public decimal ShumaPaguar {
            get { return _shumaPaguar; }
            set { SetProperty(ref _shumaPaguar, value); }
        }
        
        private string _PayType;
        public string PayType {
            get { return _PayType; }
            set { SetProperty(ref _PayType, value); }
        }        
        private string _coinType;
        public string CoinType {
            get { return _coinType; }
            set { SetProperty(ref _coinType, value); }
        }
        private ObservableCollection<Klientet> _SearchedKlientet;
        public ObservableCollection<Klientet> SearchedKlientet {
            get { return _SearchedKlientet; }
            set { SetProperty(ref _SearchedKlientet, value); }
        }
        private Klientet _selectedKlient;
        public Klientet SelectedKlient {
            get { return _selectedKlient; }
            set { SetProperty(ref _selectedKlient, value); }
        }
        public async Task ZgjedhKlientet() {
            try {
                UserDialogs.Instance.ShowLoading("Duke hapur klientet");
                ZgjidhKlientetModalPage ZgjidhKlientetModalPage = new ZgjidhKlientetModalPage();
                ZgjidhKlientetModalPage.BindingContext = this;

                await App.Instance.PushAsyncNewModal(ZgjidhKlientetModalPage);
                UserDialogs.Instance.HideLoading();

            }
            catch (Exception e) {
                var g = e.Message;
            }
        }

        public MPosControllerPrinter _printer;
        public MposConnectionInformation _connectionInfo;
        public static SemaphoreSlim _printSemaphore = new SemaphoreSlim(1, 1);
        async Task<MPosControllerDevices> OpenPrinterService(MposConnectionInformation connectionInfo) {
            if (connectionInfo == null)
                return null;

            if (_printer != null)
                return _printer;

            if (_printSemaphore == null) {
                _printSemaphore = new SemaphoreSlim(1, 1);
            }
            _printer = MPosDeviceFactory.Current.createDevice(MPosDeviceType.MPOS_DEVICE_PRINTER) as MPosControllerPrinter;

            switch (connectionInfo.IntefaceType) {
                case MPosInterfaceType.MPOS_INTERFACE_BLUETOOTH:
                case MPosInterfaceType.MPOS_INTERFACE_WIFI:
                case MPosInterfaceType.MPOS_INTERFACE_ETHERNET:
                    _printer.selectInterface((int)connectionInfo.IntefaceType, connectionInfo.Address);
                    _printer.selectCommandMode((int)(MPosCommandMode.MPOS_COMMAND_MODE_BYPASS));
                    break;
                default:
                    UserDialogs.Instance.Alert("Connection Fail", "Not Supported Interface", "OK");
                    return null;
            }

            await _printSemaphore.WaitAsync();

            try {
                var result = await _printer.openService();
                if (result != (int)MPosResult.MPOS_SUCCESS) {
                    _printer = null;
                    UserDialogs.Instance.Alert("Connection Fail", "openService failed. (" + result.ToString() + ")", "OK");
                }
            }
            finally {
                _printSemaphore.Release();
            }

            return _printer;
        }

        public async Task OnDeviceOpenClicked() {
            try {
                if (_printer == null) {
                    // Prepares to communicate with the printer 
                    _printer = await OpenPrinterService(_connectionInfo) as MPosControllerPrinter;
                    OnPrintTextClickedInkasimet();
                }
            }
            catch (Exception ex) {
                UserDialogs.Instance.Alert("Problem me servis te printerit, ju lutem provoni perseri ose me vone !", "Verejtje", "Ok");
                await App.Instance.PopPageAsync();
            }

        }
        public ObservableCollection<EvidencaPagesave> InkasimetList { get; set; }

        async Task OnPrintTextClickedInkasimet() {

            // Prepares to communicate with the printer 
            _printer = await OpenPrinterService(_connectionInfo) as MPosControllerPrinter;

            if (_printer == null)
                return;

            try {
                await _printSemaphore.WaitAsync();

                uint textCount = 0;
                string printText = string.Empty;


                //sd.AddToPreview();
                // sd.Preview();
                //lRet = await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosCodePage.MPOS_CODEPAGE_WPC1252 });

                // note : Page mode and transaction mode cannot be used together between IN and OUT.
                // When "setTransaction" function called with "MPOS_PRINTER_TRANSACTION_IN", print data are stored in the buffer.
                //await _printer.setTransaction((int)MPosTransactionMode.MPOS_PRINTER_TRANSACTION_IN);
                // Printer Setting Initialize
                await _printer.directIO(new byte[] { 0x1b, 0x40 });

                // Code Pages for the contries in east Asia. Please note that the font data downloading is required to print characters for Korean, Japanese and Chinese.
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosEastAsiaCodePage.MPOS_CODEPAGE_KSC5601 });   // Korean
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosEastAsiaCodePage.MPOS_CODEPAGE_SHIFTJIS });  // Japanese
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosEastAsiaCodePage.MPOS_CODEPAGE_GB2312 });    // Simplifies Chinese
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosEastAsiaCodePage.MPOS_CODEPAGE_BIG5 });      // Traditional Chinese
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosCodePage.MPOS_CODEPAGE_FARSI });     // Persian 
                //await _printer.printText((textCount++).ToString() + printText, new MPosFontAttribute() { CodePage = (int)MPosCodePage.MPOS_CODEPAGE_FARSI_II });  // Persian 

                await _printer.setTransaction((int)MPosTransactionMode.MPOS_PRINTER_TRANSACTION_IN);


                await _printer.printBitmap(DependencyService.Get<IPlatformInfo>().GetImgResource(),
                            120/*(int)MPosImageWidth.MPOS_IMAGE_WIDTH_ASIS*/,   // Image Width
                            (int)MPosAlignment.MPOS_ALIGNMENT_CENTER,           // Alignment
                            60,                                                 // brightness
                            true,                                               // Image Dithering
                            true);
                await _printer.printLine(1, 1, 1, 1, 1);
                await _printer.printText("\nR A P O R T I   I   I N K A S I M E V E (Faturave) \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_CENTER, Bold = false, });
                await _printer.printLine(0, 0, 1, 1, 1);
                await _printer.printText(
"---------------------------------------------------------------------\n");
                await _printer.printText("", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });
                await _printer.printText(" \n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });
                await _printer.printText("\n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });
                await _printer.printText("\n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });

                var agjender = await App.Database.GetAgjendetAsync();
                var agjendi = agjender.FirstOrDefault(x => x.Depo == Agjendi.Depo);
                await _printer.printText("          " + agjendi.Emri + " " + agjendi.Mbiemri + "         " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "\n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });

                await _printer.printText("------------------------------------------------------------------------------------------------------------------------------------------");

                await _printer.printText("\nKlienti             Tipi    Monedhe    Fatura     Totali     Paguar\n");
                await _printer.printText("---------------------------------------------------------------------\n");
                float teGjithaSasit = 0f;
                float teGjithaCmimetNjesi = 0f;
                double teGjitheCmimetTotale = 0f;
                var tvsh = 0m;
                var nrBarkodi = 0;
                var klientet = await App.Database.GetKlientetAsync();
                foreach (var art in InkasimetList) {
                    foreach (var klient in klientet) {
                        if (klient.IDKlienti != art.IDKlienti)
                            continue;
                        string prntBuilder = string.Empty;
                        if (klient.Emri.Trim().Length > 19) {
                            klient.Emri = klient.Emri.Trim().Remove(19);
                        }
                        prntBuilder += klient.Emri.Trim();

                        if (klient.Emri.Length == 21) {
                            prntBuilder += art.PayType;
                        }
                        else if (klient.Emri.Length == 20) {
                            prntBuilder += " " + art.PayType;
                        }
                        else if (klient.Emri.Length == 19) {
                            prntBuilder += " " + art.PayType;
                        }
                        else if (klient.Emri.Length == 18) {
                            prntBuilder += "  " + art.PayType;
                        }
                        else if (klient.Emri.Length == 17) {
                            prntBuilder += "   " + art.PayType;
                        }
                        else if (klient.Emri.Length == 16) {
                            prntBuilder += "    " + art.PayType;
                        }
                        else if (klient.Emri.Length == 15) {
                            prntBuilder += "     " + art.PayType;
                        }
                        else if (klient.Emri.Length == 14) {
                            prntBuilder += "      " + art.PayType;
                        }
                        else if (klient.Emri.Length == 13) {
                            prntBuilder += "       " + art.PayType;
                        }
                        else if (klient.Emri.Length == 12) {
                            prntBuilder += "        " + art.PayType;
                        }
                        else if (klient.Emri.Length == 11) {
                            prntBuilder += "           " + art.PayType;
                        }
                        else if (klient.Emri.Length == 10) {
                            prntBuilder += "            " + art.PayType;
                        }
                        else if (klient.Emri.Length == 9) {
                            prntBuilder += "             " + art.PayType;
                        }
                        else if (klient.Emri.Length == 8) {
                            prntBuilder += "             " + art.PayType;
                        }
                        else if (klient.Emri.Length == 7) {
                            prntBuilder += "              " + art.PayType;
                        }
                        else if (klient.Emri.Length == 6) {
                            prntBuilder += "               " + art.PayType;
                        }
                        else if (klient.Emri.Length == 5) {
                            prntBuilder += "                " + art.PayType;
                        }
                        else if (klient.Emri.Length == 4) {
                            prntBuilder += "                 " + art.PayType;
                        }
                        else if (klient.Emri.Length == 3) {
                            prntBuilder += "                  " + art.PayType;
                        }

                        prntBuilder += "    " + art.KMON;

                        if (string.IsNullOrEmpty(art.NrFatures)) {
                            prntBuilder += "       " + "        ";
                        }
                        else {
                            prntBuilder += "       " + art.NrFatures;
                        }
                        var shumaPaguarString = String.Format("{0:0.00}", art.ShumaPaguar);

                        if (art.ShumaTotale == null) {
                            if (shumaPaguarString.Length >= 9) {
                                prntBuilder += "    " + "    " + "  ";
                            }
                            else
                                prntBuilder += "    " + "    " + "   ";
                        }
                        else {
                            art.ShumaTotale = 0.00f;
                            var shumaTotale = String.Format("{0:0.00}", art.ShumaTotale);

                            if (shumaTotale.Length >= 9) {
                                prntBuilder += "  " + shumaTotale + "";
                            }
                            else if (shumaTotale.Length >= 8) {
                                prntBuilder += "   " + shumaTotale + "";
                            }
                            else if (shumaTotale.Length >= 7) {
                                prntBuilder += "   " + shumaTotale + " ";
                            }
                            else if (shumaTotale.Length >= 6) {
                                prntBuilder += "    " + shumaTotale + " ";
                            }
                            else if (shumaTotale.Length >= 5) {
                                prntBuilder += "    " + shumaTotale + "  ";
                            }
                            else if (shumaTotale.Length >= 4) {
                                prntBuilder += "    " + shumaTotale + "   ";
                            }
                        }

                        prntBuilder += "    " + String.Format("{0:0.00}", art.ShumaPaguar);

                        await _printer.printText(prntBuilder + "\n", new MPosFontAttribute { Alignment = MPosAlignment.MPOS_ALIGNMENT_DEFAULT });
                        teGjithaCmimetNjesi += (float)art.ShumaPaguar;
                        Debug.WriteLine(prntBuilder + " Length : " + prntBuilder.Length);
                    }

                }


                await _printer.printText("\n---------------------------------------------------------------------");



                //printText = "A. 1. عدد ۰۱۲۳۴۵۶۷۸۹" + "\nB. 2. عدد 0123456789" + "\nC. 3. به" + "\nD. 4. نه" + "\nE. 5. مراجعه" + "\n";// 
                //await _printer.printText(printText, new MPosFontAttribute() { CodePage = (int)MPosCodePage.MPOS_CODEPAGE_FARSI, Alignment = MPosAlignment.MPOS_ALIGNMENT_LEFT });     // Persian 
                await _printer.printText("\n");
                await _printer.printText("\n");
                await _printer.printText("Llojet e pagesave \n");
                await _printer.printText("Shuma e paguar ne EURO :                               0.00\n");
                await _printer.printText("Shuma e paguar ne LEK  :                               " + String.Format("{0:0.00}", teGjithaCmimetNjesi) + "\n");
                await _printer.printText("Shuma e paguar ne USD  :                               0.00\n");


                // Feed to tear-off position (Manual Cutter Position)
                await _printer.directIO(new byte[] { 0x1b, 0x4a, 0xaf });
            }
            catch (Exception ex) {
                UserDialogs.Instance.Alert("Exception", ex.Message, "OK");
            }
            finally {
                // Printer starts printing by calling "setTransaction" function with "MPOS_PRINTER_TRANSACTION_OUT"
                await _printer.setTransaction((int)MPosTransactionMode.MPOS_PRINTER_TRANSACTION_OUT);
                // If there's nothing to do with the printer, call "closeService" method to disconnect the communication between Host and Printer.
                await _printer.closeService();
                _printSemaphore.Release();
                _printer = null;
                _printSemaphore = null;
            }
        }

        public async void RegjistroAsync() {
            if(string.IsNullOrEmpty(PayType)) {
                UserDialogs.Instance.Alert("Ju lutem zgjedhni menyren e pageses para se te regjistroni inkasimin");
                return;
            }
            if(DetyrimetNgaKlienti.Count < 1) {
                UserDialogs.Instance.Alert("Nuk ka detyrim, ju lutem zgjedhni nje klient me detyrim");
                return;
            }
            else {
                try {
                    DateTime n = DateTime.Now;
                    string hr = n.Hour.ToString();
                    string min = n.Minute.ToString();
                    string sec = n.Second.ToString();

                    //ie. if Hour is 4 then 04; if minute is 3 then 03             
                    if (hr.Length == 1)
                        hr = "0" + hr;
                    if (min.Length == 1)
                        min = "0" + min;
                    if (sec.Length == 1)
                        sec = "0" + sec;

                    string PagesatID = Agjendi.DeviceID + "-|-" + n.Year.ToString() + n.Month.ToString() + n.Day.ToString() +
                            hr + min + sec;
                    decimal vlera = ((Math.Round(decimal.Parse(ShumaPaguar.ToString()), 2)));
                    //var query = from ep in EvidencaEPagesave
                    //            join k in Klientet on ep.IDKlienti equals k.IDKlienti
                    //            join d in Detyrimet on ep.IDKlienti equals d.KOD
                    //            where ep.NrFatures == null && ep.DeviceID == Agjendi.DeviceID
                    //            select new
                    //            {
                    //                ep.IDKlienti,
                    //                Klienti = k.Emri,
                    //                Detyrimi = decimal.Round(d.Detyrimi, 2),
                    //                Paguar = decimal.Round(ep.ShumaPaguar, 2),
                    //                ep.KMON,
                    //                ep.NrPageses
                    //            };
                    var nrFatures = string.Empty;
                    if(EvidencaEPagesave.Count > 1) {
                        await App.Database.DeleteEvidencaPagesave(EvidencaEPagesave.FirstOrDefault(x => x.NrFatures == ""));
                        var query = EvidencaEPagesave
                                 .OrderByDescending(ep => ep.DataPageses)
                                 .Select(ep => new
                                 {
                                     NrFatures = ep.NrFatures != null ? ep.NrFatures : "",
                                     ep.NrPageses
                                 })
                                 .FirstOrDefault();
                        if (query != null) {
                            if(!string.IsNullOrEmpty(query.NrFatures)) {
                                var currNrFatures = int.Parse(query.NrFatures.Split('/')[0]);
                                currNrFatures = currNrFatures + 1;
                                nrFatures = currNrFatures.ToString() + "/" + query.NrFatures.Split('/')[1];
                            }
                        }
                    }
                    

                    var EvidencaPagesave = new EvidencaPagesave
                    {
                        NrPageses = PagesatID,
                        ShumaPaguar = float.Parse(vlera.ToString()),
                        DataPageses = TodaysDate,
                        IDAgjenti = Agjendi.IDAgjenti,
                        IDKlienti = DetyrimetNgaKlienti.FirstOrDefault().KOD,
                        DeviceID = Agjendi.DeviceID,
                        SyncStatus = 0,
                        PayType = PayType,
                        KMON = "LEK",
                        ExportStatus = 0,
                    };
                    EvidencaPagesave.Borxhi = DetyrimetNgaKlienti.FirstOrDefault().Detyrimi - EvidencaPagesave.ShumaPaguar;
                    DetyrimetNgaKlienti.FirstOrDefault().Detyrimi = (float)EvidencaPagesave.Borxhi;
                    await App.Database.UpdateDetyrimi(DetyrimetNgaKlienti.FirstOrDefault());

                    var result = await App.Database.SaveEvidencaPagesaveAsync(EvidencaPagesave);
                    if(result != -1) {
                        var shtypja = await UserDialogs.Instance.ConfirmAsync(" Inkasimi përfundoi me sukses \n Dëshironi të shtypni fletëpagesën ?", "Shtypja", "Po", "Jo");
                        if(shtypja) {
                            InkasimetList = new ObservableCollection<EvidencaPagesave> { EvidencaPagesave };
                            await App.Instance.PushAsyncNewPage(new PrinterSelectionPage() { BindingContext = this});
                        }
                    }
                }catch(Exception e) {

                }
            }
        }

        public void MerrDetyrimet(Klientet klientiSelektuar) {
            DetyrimetNgaKlienti.Clear();
            var detyrimi = (Detyrimet.FirstOrDefault(x=> x.KOD == klientiSelektuar.IDKlienti));
            if (detyrimi == null)
                return;
            else
                DetyrimetNgaKlienti.Add(detyrimi);
        }


    }
}
