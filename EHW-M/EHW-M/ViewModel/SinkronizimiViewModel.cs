using Acr.UserDialogs;
using EHW_M;
using EHWM.DependencyInjections.FiskalizationExtraModels;
using EHWM.Models;
using Newtonsoft.Json;
using SQLite;
using Syncfusion.Compression;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.TizenSpecific;
using static EHWM.ViewModel.SinkronizimiViewModel;
using static SQLite.SQLite3;

namespace EHWM.ViewModel {
    public class SinkronizimiNavigationParameters {
        public Agjendet Agjendi { get; set; }
    }
    public class SinkronizimiViewModel : BaseViewModel {
        //Used in SyncAll method in frmSync.cs
        public enum MobSellSyncDirection : int {
            SnapshotSpecial = -2,//won't do the snapshot if the server returns no record
            Snapshot = -1, //snapshot tableData returned by server
            Download = 0, //downloads with Insert if new, and Update if existing
            Upload = 1,   //Uploads with Insert if new, and Update if existing
            Bidirectional = 2, //a).Uploads b.)Downloads with Insert if new, and Update if existing
            BidirectionalSpecial = 3, //a.)Uploads to server in TableName+ _upl b.) Downloads to original table in device
            DownloadWithInsert = 4, //Downloads with insert if new, (no update)
            DownloadSpecial = 5,//downloads with insert new records and update exisiting except update exsiting changes to syncSatus=2
            LogUpload = 6,//When Uploading Log this is the indicator
            UploadWithInsert = 7 //uploads records and insert new only (no update)
        }
        //this is used to set direction of sync
        public enum PnSyncDirection {
            Download,
            Upload,
            SnapshotDownload,
            SnapshotSpecial,//won't do the snapshot if the server returns no record
            DownloadWithInsert,
            SpecialDownload,//downloads with insert new records and update exisiting except update exsiting changes to syncSatus=2
            LogUpload,
            UploadWithInsert
        }

        private Agjendet Agjendi;

        private string _EditorView;
        public string EditorView {
            get { return _EditorView; }
            set { SetProperty(ref _EditorView, value); }
        }

        public ICommand SinkronizoCmimet { get; set; }
        public ICommand SinkronizoKlientet { get; set; }
        public ICommand Azhurno { get; set; }
        public ICommand ShkarkoCommand { get; set; }
        public ICommand SinkronizoCommand { get; set; }
        public ICommand AzhurnoCommand { get; set; }

        public SinkronizimiViewModel(SinkronizimiNavigationParameters np) {
            //SinkronizoCmimet = new Command(async () => await SinkronizoCmimetAsync());
            SinkronizoKlientet = new Command(async () => await SinkronizoKlientetAsync());
            if (np != null) {
                Agjendi = np.Agjendi;
            }
            ShkarkoCommand = new Command(async () => await ShkarkoAsync());
            SinkronizoCommand = new Command(async () => await SinkronizoAsync());
            AzhurnoCommand = new Command(async () => await AzhurnoAsync());
        }

        public async Task SinkronizoAsync() {
            try {
                int rows = (await App.Database.GetMalliMbeturAsync()).Count;

                if (rows > 0) {
                    double? _shitur = await SumDataColumn("Malli_Mbetur", "SasiaShitur");
                    double? _kthyer = await SumDataColumn("Malli_Mbetur", "SasiaKthyer");

                    if ((double)_shitur > 0 || (double)_kthyer < 0) {
                        UserDialogs.Instance.Alert("Nuk lejohet ringarkimi i shënimeve të reja \n Së pari duhet të shkarkoni shënimet \n ekzistojn fatura të pa zëruara", "Sinkronizimi");
                        return;
                    }
                }

                var sync = await SinkronizoFiskalizimin();

                if (!sync) return;
                if (await UserDialogs.Instance.ConfirmAsync("Ky proçes shkarkon sasitë e mbetura të artikujve nga Palmi \n dhe bën zerimin e sasive në Palm. \n  A dëshironi të vazhdoni?", "Konfirmo", "Po", "Jo")) {
                    try {
                        // Perform synchronization
                        await SyncAll();
                    }
                    finally {

                    }


                    try {
                    }
                    catch (Exception ex) {
                    }
                }
            }
            catch (Exception ex) {
            }

        }
        public async Task ShkarkoAsync() {
            if(!App.Instance.DoIHaveInternet()) {
                return;
            }
            UserDialogs.Instance.ShowLoading("Duke shkarkuar");
            var getAllLiferimet = await App.Database.GetLiferimetAsync();
            var _getAllLiferimet = (await App.Database.GetLiferimiAsync()).Where(x => x.SyncStatus == 0).OrderByDescending(x => x.NrLiferimit).Take(1).ToList();

            if (_getAllLiferimet.Count > 0) {
                _getAllLiferimet.Clear();

                double? _shitur = await SumDataColumn("Malli_Mbetur", "SasiaShitur");
                double? _kthyer = await SumDataColumn("Malli_Mbetur", "SasiaKthyer");
                if (_kthyer != null && _shitur != null) {
                    if (_shitur >= 0 && _kthyer <= 0) {
                        App.ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Instance.MainViewModel.Configurimi.Token);
                        App.ApiClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                        var malletEMbeturaResult = await App.ApiClient.GetAsync("malli-mbetur/" + Agjendi.Depo);
                        if (!malletEMbeturaResult.IsSuccessStatusCode) {
                            UserDialogs.Instance.Alert("Problem me server, ju lutem provoni me vone!", "Verejtje");
                            UserDialogs.Instance.HideLoading();
                            return;
                        }
                        var malletEMbeturaResponse = await malletEMbeturaResult.Content.ReadAsStringAsync();
                        var malletEMbetura = JsonConvert.DeserializeObject<List<Malli_Mbetur>>(malletEMbeturaResponse);
                        int countExportStatus = malletEMbetura.Where(x => x.Export_Status == 0 || x.Export_Status == null).Count();

                        if (countExportStatus == 0) {

                            var confirm = await UserDialogs.Instance.ConfirmAsync("Ky proçes shkarkon të gjtha faturat, \n inkasimet nga Palmi dhe rifreskon sasite e artikujve \n nese ka ngarkime të reja për magazinë. \n A dëshironi të vazhdoni?", "Konfirmo", "Po", "Jo");
                            if (confirm) {
                                decimal _LiferimetShuma = 0;

                                _getAllLiferimet = await App.Database.GetLiferimetAsync();
                                var totalShuma = _getAllLiferimet
                                                .Where(l => l.PayType == "KESH")
                                                .Sum(l => l.ShumaPaguar);
                                var roundedTotalShuma = Math.Round(totalShuma, 2);

                                if (roundedTotalShuma > 0) {
                                    _LiferimetShuma = decimal.Parse(roundedTotalShuma.ToString());
                                }
                                else {
                                    _LiferimetShuma = 0;

                                }

                                var cashRegister = await App.Database.GetCashRegisterAsync();
                                var levizjetHeaderList = await App.Database.GetLevizjetHeaderAsync();
                                var checkMalliMbetur = levizjetHeaderList.OrderByDescending(x => x.Data).FirstOrDefault();

                                DateTime DataCashRegister = DateTime.Now.Date.AddDays(-5);
                                if (cashRegister.Count > 0) {
                                    DataCashRegister = cashRegister.FirstOrDefault().RegisterDate;
                                }

                                DateTime DataLevizjes = DateTime.Now.Date.AddDays(-5);
                                if (checkMalliMbetur != null) {
                                    DataLevizjes = (DateTime)checkMalliMbetur.Data;
                                }

                                decimal CashShumaTotale = _LiferimetShuma; //+_getInitialCash;

                                try {
                                    if (DateTime.Now.Date > DataCashRegister.Date) {
                                        RegisterCashDepositInputRequestPCL cashDepositRequest = new RegisterCashDepositInputRequestPCL
                                        {
                                            CashAmount = CashShumaTotale,
                                            DepositType =  CashDepositOperationSTypePCL.WITHDRAW,
                                            TCRCode = Agjendi.TCRCode,
                                            SendDateTime = DateTime.Now,
                                            SubseqDelivTypeSType = -1
                                        };

                                        //register cash deposit
                                        var newCashRegister = new CashRegister
                                        {
                                            Cashamount = CashShumaTotale,
                                            DepositType = 1,
                                            DeviceID = Agjendi.DeviceID,
                                            ID = Guid.NewGuid(),
                                            Message = "",
                                            RegisterDate = DateTime.Now,
                                            SyncStatus = 0,
                                            TCRCode = Agjendi.TCRCode,
                                            TCRSyncStatus = 0,
                                        };
                                        await App.Database.SaveCashRegisterAsync(newCashRegister);
                                        var result = App.Instance.FiskalizationService.RegisterCashDeposit(cashDepositRequest);

                                        if (result.Status == StatusPCL.Ok) {
                                            newCashRegister.TCRSyncStatus = 1;
                                            newCashRegister.Message = result.Message.Replace("'", "");
                                        }
                                        else if (result.Status == StatusPCL.TCRAlreadyRegistered) {
                                            newCashRegister.TCRSyncStatus = 4;
                                            newCashRegister.Message = result.Message.Replace("'", "");
                                        }
                                        else {
                                            newCashRegister.TCRSyncStatus = -1;
                                            newCashRegister.Message = result.Message.Replace("'", "");
                                        }
                                        await App.Database.UpdateCashRegisterAsync(newCashRegister);

                                        //register cash deposit
                                    }

                                    //if (!(checkMalliMbetur.Rows.Count > 0))
                                    if (DateTime.Now > DataLevizjes) {
                                        await FiskalizoMalliMbetur();
                                    }
                                    Sync sync = new Sync();
                                    var SyncSuccesful = await sync.SyncTable("Malli_Mbetur", null, PnSyncDirection.Upload, "", "", "");
                                    //update malli mbetur

                                    await SyncTCRInvoices(1);
                                    await SyncTCRInvoices(2);
                                    //FiskalizimiBL.SyncTCRWTN(1);
                                    //FiskalizimiBL.SyncTCRCashRegisterTable(1);

                                }
                                catch (Exception e) {
                                }

                                await ShkarkoMalliMbetur();
                                UserDialogs.Instance.HideLoading();

                            }
                            else {
                                UserDialogs.Instance.HideLoading();
                            }

                        }
                        else {
                            UserDialogs.Instance.Alert("Nuk mund të vazhdohet me shkarkim! \n Shkarkimi paraprak për këtë pajisje nuk është eksportuar akoma në Financa5!", "Gabim!");
                            UserDialogs.Instance.HideLoading();
                            return;
                        }
                    }
                }
                else {
                    UserDialogs.Instance.HideLoading();
                    UserDialogs.Instance.Alert("\"Psioni është i zëruar \n Nuk ka shënime për zërim\", \"Shkarkimi\"");
                }
            }
            else {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert("\"Nuk mund të vazhdohet me shkarkim! \n Duhet të jetë së paku një shitje për të vazhduar me shkarkim!\", \"Shkarkimi\"");
                return;
            }
        }

        public async Task FiskalizoMalliMbetur() {
            var levizjetHeader = await App.Database.GetLevizjetHeaderAsync();
            var malliMbetur = await App.Database.GetMalliMbeturAsync();
            var salesPrice = await App.Database.GetSalesPriceAsync();
            var numriFisk = await App.Database.GetNumratFiskalAsync();
            var numriFaturave = await App.Database.GetNumriFaturaveAsync();
            var artikujt = await App.Database.GetArtikujtAsync();
            var result = levizjetHeader.Where(header => header.LevizjeNe == "PG1" && header.Longitude == "8" && header.Latitude == "8").ToList();
            if (result.Count == 0) {
                var total = malliMbetur
                            .Join(salesPrice,
                                m => m.IDArtikulli,
                                ss => ss.ItemNo,
                                (m, ss) => new { m, ss })
                            .Join(artikujt,
                                a => a.m.IDArtikulli,
                                m => m.IDArtikulli,
                                (a, m) => new { a, m })
                            .Where(joinedTables => joinedTables.a.ss.SalesCode == "STANDARD" && joinedTables.a.m.SasiaMbetur > 0 && joinedTables.a.m.Depo == Agjendi.Depo)
                            .Sum(joinedTables => joinedTables.a.ss.UnitPrice * joinedTables.a.m.SasiaMbetur);

                var _LevizjeIDN = numriFisk
                        .Where(v => v.Depo == Agjendi.IDAgjenti)
                        .OrderBy(v => v.LevizjeIDN)
                        .Select(v => v.LevizjeIDN)
                        .FirstOrDefault();

                _LevizjeIDN = _LevizjeIDN + 1;

                var _NrFatures = numriFaturave
                    .Where(n => n.KOD == Agjendi.IDAgjenti)
                    .Select(n => n.NRKUFIP_D + n.CurrNrFat_D)
                    .FirstOrDefault();

                var newLevizjetHeader = new LevizjetHeader
                {
                    TransferID = Guid.NewGuid(),
                    NumriLevizjes = _NrFatures.ToString(),
                    LevizjeNga = Agjendi.IDAgjenti,
                    LevizjeNe = "PG1",
                    Data = DateTime.Now, // Assuming you want the current date and time
                    Totali = (decimal?)total,
                    IDAgjenti = Agjendi.IDAgjenti,
                    SyncStatus = 0,
                    ImpStatus = 0,
                    KodiDyqanit = Agjendi.Depo,
                    NumriDaljes = "",
                    Depo = Agjendi.Depo,
                    Longitude = "8",
                    Latitude = "8",
                    NumriFisk = _LevizjeIDN,
                    TCR = Agjendi.TCRCode,
                    TCROperatorCode = Agjendi.OperatorCode,
                    TCRBusinessUnitCode = App.Instance.MainViewModel.Configurimi.KodiINjesiseSeBiznesit
                };
                var res = await App.Database.SaveLevizjeHeaderAsync(newLevizjetHeader);

                var levizjetDetailsList = (
                    from m in malliMbetur
                    join ss in salesPrice on m.IDArtikulli equals ss.ItemNo
                    join a in artikujt on m.IDArtikulli equals a.IDArtikulli
                    where ss.SalesCode == "STANDARD" && m.SasiaMbetur > 0 && m.Depo == Agjendi.Depo
                    select new LevizjetDetails
                    {
                        NumriLevizjes = _NrFatures.ToString(),
                        Artikulli = m.Emri,
                        IDArtikulli = m.IDArtikulli,
                        Sasia = (decimal?)m.SasiaMbetur,
                        Cmimi = (decimal?)ss.UnitPrice,
                        Njesia_matese = a.BUM,
                        Totali = Math.Round((decimal)(ss.UnitPrice * m.SasiaMbetur), 2),
                        SyncStatus = 0,
                        Seri = m.Seri != null ? m.Seri : "EMPTY",
                        Depo = Agjendi.Depo
                    }
                ).ToList();

                var resld = await App.Database.SaveAllLevizjeDetailsAsync(levizjetDetailsList);

                var _UpdatedLevizjeIDN = numriFisk
                            .Where(v => v.Depo == Agjendi.IDAgjenti)
                            .OrderBy(v => v.LevizjeIDN)
                            .FirstOrDefault();
                _UpdatedLevizjeIDN.LevizjeIDN = _LevizjeIDN;
                await App.Database.UpdateNumriFiskalAsync(_UpdatedLevizjeIDN);

                var _UpdatedNrFatures = numriFaturave
                        .Where(n => n.KOD == Agjendi.IDAgjenti)
                        .FirstOrDefault();
                _UpdatedNrFatures.CurrNrFat_D = _UpdatedNrFatures.CurrNrFat_D + 1;
                await App.Database.UpdateNumriFaturave(_UpdatedNrFatures);

                await RegisterTCRWTN(_NrFatures.ToString());

 
            }
        }

        public async Task RegisterTCRWTN(string nrLevizjes) {
            var levizjetHeader = await App.Database.GetLevizjetHeaderAsync();
            var depot = await App.Database.GetDepotAsync();
            var result = from lh in levizjetHeader
                         join d_ne in depot on lh.LevizjeNe equals d_ne.Depo
                         join d_nga in depot on lh.LevizjeNga equals d_nga.Depo
                         where (lh.TCRSyncStatus == null || lh.TCRSyncStatus == 0)
                               && lh.NumriLevizjes == nrLevizjes
                               && lh.LevizjeNga.Trim() == Agjendi.IDAgjenti
                         orderby lh.Data descending
                         select new EHWM.Models.WTNModels.MapperHeader
                         {
                             Numri_Levizjes = lh.NumriLevizjes,
                             InvOrdNum = (int)lh.NumriFisk,
                             InvNum = lh.NumriFisk + "/" + DateTime.Now.Year,
                             ValueOfGoods = Math.Round((decimal)lh.Totali, 2),
                             FromDevice = d_nga.TAGNR,
                             ToDevice = d_ne.TAGNR,
                             ImpStatus = (int)lh.ImpStatus
                         };


            List<EHWM.Models.WTNModels.MapperHeader> mapperHeaderList = result.ToList();
            var levizjetDetails = await App.Database.GetLevizjeDetailsAsync();
            var query = from h in levizjetHeader
                        join l in levizjetDetails on h.NumriLevizjes equals l.NumriLevizjes
                        where (h.TCRSyncStatus == null || h.TCRSyncStatus == 0)
                              && h.NumriLevizjes == nrLevizjes
                              && h.LevizjeNga.Trim() == Agjendi.IDAgjenti
                              && Math.Round((decimal)l.Sasia, 2) >= 0.1m
                        select new EHWM.Models.WTNModels.MapperLines
                        {
                            Numri_Levizjes = h.NumriLevizjes,
                            DeviceID = h.Depo,
                            OperatorCode = "OperatorCode",
                            InvOrdNum = (int)h.NumriFisk,
                            InvNum = h.NumriFisk + "/" + DateTime.Now.Year,
                            SendDatetime = (DateTime)h.Data,
                            ValueOfGoods = Math.Round((decimal)h.Totali, 2),
                            StartPointSType = "SALE",
                            DestinPointSType = "SALE",
                            Item_N = l.Artikulli,
                            Item_C = l.IDArtikulli,
                            Item_U = l.Njesia_matese,
                            Item_Q = Math.Round((decimal)l.Sasia, 2),
                            MobileRefId = "M-06"
                        };

            List<Models.WTNModels.MapperLines> mapperLinesList = query.Distinct().ToList();

            if (mapperHeaderList.Count > 0) {


                List<TCRLevizjetPCL> invoiceObject = Models.WTNModels.ClassMapper.MapHeadersAndLines(mapperHeaderList, mapperLinesList);
                foreach (TCRLevizjetPCL inv in invoiceObject) {
                    RegisterWTNInputRequestPCL req = new RegisterWTNInputRequestPCL();

                    req.TransferItems = inv.Items.ToList();
                    req.InvOrdNum = inv.InvOrdNum.ToString();
                    req.InvNum = inv.InvNum.ToString();
                    req.DeviceID = inv.DeviceID;
                    req.MobileRefId = inv.InvOrdNum.ToString();
                    req.OperatorCode = Agjendi.OperatorCode;
                    req.TCRCode = Agjendi.TCRCode;
                    req.BusinessUnitCode = App.Instance.MainViewModel.Configurimi.KodiINjesiseSeBiznesit;
                    req.SendDatetime = inv.SendDatetime;
                    req.SubseqDelivTypeSType = -1; //ONLINE
                    req.ValueOfGoods = (decimal)inv.ValueOfGoods;

                    req.DestinPointSType = inv.DestinPointSType;
                    req.DestinPointSTypeSpecified = true;
                    req.StartPointSType = inv.StartPointSType;
                    req.StartPointSTypeSpecified = true;
                    req.FromDeviceId = inv.FromDevice;
                    req.ToDeviceId = inv.ToDevice;

                    ResultLogPCL log = App.Instance.FiskalizationService.RegisterWTN(req);
                    if(log == null) {
                        var levizjaHeader = levizjetHeader
                                        .FirstOrDefault(h => h.NumriLevizjes == inv.Numri_Levizjes);

                        if (levizjaHeader != null) {
                            levizjaHeader.TCRSyncStatus = -1;
                            levizjaHeader.TCRIssueDateTime = DateTime.Now;
                            levizjaHeader.TCRQRCodeLink = null;
                            levizjaHeader.TCR = Agjendi.TCRCode;
                            levizjaHeader.TCROperatorCode = Agjendi.OperatorCode;
                            levizjaHeader.TCRBusinessUnitCode = App.Instance.MainViewModel.Configurimi.KodiINjesiseSeBiznesit;
                            levizjaHeader.UUID = null;
                            levizjaHeader.TCRNSLFSH = null;
                            levizjaHeader.TCRNIVFSH = null;
                            levizjaHeader.Message = "Fiskalizimi deshtoi, ju lutemi provoni me vone!";

                            await App.Database.UpdateLevizjeHeaderAsync(levizjaHeader);
                        }

                        var levizjetDetailsToUpdate = levizjetDetails
                                    .Where(d => d.NumriLevizjes == inv.Numri_Levizjes)
                                    .ToList();

                        foreach (var detail in levizjetDetailsToUpdate) {
                            detail.TCRSyncStatus = -1;
                            await App.Database.UpdateLevizjeDetailsAsync(detail);
                        }
                        return;
                    }
                    if (log.Status == StatusPCL.Ok) {
                        var levizjaHeader = levizjetHeader
                                .FirstOrDefault(h => h.NumriLevizjes == inv.Numri_Levizjes);

                        if (levizjaHeader != null) {
                            levizjaHeader.TCRSyncStatus = 1;
                            levizjaHeader.TCRIssueDateTime = DateTime.Now;
                            levizjaHeader.TCRQRCodeLink = log.QRCodeLink;
                            levizjaHeader.TCR = Agjendi.TCRCode;
                            levizjaHeader.TCROperatorCode = Agjendi.OperatorCode;
                            levizjaHeader.TCRBusinessUnitCode = App.Instance.MainViewModel.Configurimi.KodiINjesiseSeBiznesit;
                            levizjaHeader.UUID = log.ResponseUUIDSH;
                            levizjaHeader.TCRNSLFSH = log.NSLFSH;
                            levizjaHeader.TCRNIVFSH = log.NIVFSH;
                            levizjaHeader.Message = log.Message.Replace("'", "");

                            await App.Database.UpdateLevizjeHeaderAsync(levizjaHeader);
                        }

                        var levizjetDetailsToUpdate = levizjetDetails
                                    .Where(d => d.NumriLevizjes == inv.Numri_Levizjes)
                                    .ToList();

                        foreach (var detail in levizjetDetailsToUpdate) {
                            detail.TCRSyncStatus = 1;
                            await App.Database.UpdateLevizjeDetailsAsync(detail);
                        }
                    }
                    else if (log.Status == StatusPCL.FaultCode) {
                        try {
                            if (String.IsNullOrEmpty(log.Message)) {
                                var levizjaHeader = levizjetHeader
                                        .FirstOrDefault(h => h.NumriLevizjes == inv.Numri_Levizjes);

                                if (levizjaHeader != null) {
                                    levizjaHeader.TCRSyncStatus = -1;
                                    levizjaHeader.TCRIssueDateTime = DateTime.Now;
                                    levizjaHeader.TCRQRCodeLink = log.QRCodeLink;
                                    levizjaHeader.TCR = Agjendi.TCRCode;
                                    levizjaHeader.TCROperatorCode = Agjendi.OperatorCode;
                                    levizjaHeader.TCRBusinessUnitCode = App.Instance.MainViewModel.Configurimi.KodiINjesiseSeBiznesit;
                                    levizjaHeader.UUID = log.ResponseUUIDSH;
                                    levizjaHeader.TCRNSLFSH = log.NSLFSH;
                                    levizjaHeader.TCRNIVFSH = log.NIVFSH;
                                    levizjaHeader.Message = "Fiskalizimi deshtoi, ju lutemi provoni me vone!";

                                    await App.Database.UpdateLevizjeHeaderAsync(levizjaHeader);
                                }

                                var levizjetDetailsToUpdate = levizjetDetails
                                            .Where(d => d.NumriLevizjes == inv.Numri_Levizjes)
                                            .ToList();

                                foreach (var detail in levizjetDetailsToUpdate) {
                                    detail.TCRSyncStatus = -1;
                                    await App.Database.UpdateLevizjeDetailsAsync(detail);
                                }
                            }
                            else {
                                var levizjaHeader = levizjetHeader
                                        .FirstOrDefault(h => h.NumriLevizjes == inv.Numri_Levizjes);

                                if (levizjaHeader != null) {
                                    levizjaHeader.TCRSyncStatus = -1;
                                    levizjaHeader.TCRIssueDateTime = DateTime.Now;
                                    levizjaHeader.TCRQRCodeLink = log.QRCodeLink;
                                    levizjaHeader.TCR = Agjendi.TCRCode;
                                    levizjaHeader.TCROperatorCode = Agjendi.OperatorCode;
                                    levizjaHeader.TCRBusinessUnitCode = App.Instance.MainViewModel.Configurimi.KodiINjesiseSeBiznesit;
                                    levizjaHeader.UUID = log.ResponseUUIDSH;
                                    levizjaHeader.TCRNSLFSH = log.NSLFSH;
                                    levizjaHeader.TCRNIVFSH = log.NIVFSH;
                                    levizjaHeader.Message = log.Message.Replace("'", "");

                                    await App.Database.UpdateLevizjeHeaderAsync(levizjaHeader);
                                }

                                var levizjetDetailsToUpdate = levizjetDetails
                                            .Where(d => d.NumriLevizjes == inv.Numri_Levizjes)
                                            .ToList();

                                foreach (var detail in levizjetDetailsToUpdate) {
                                    detail.TCRSyncStatus = -1;
                                    await App.Database.UpdateLevizjeDetailsAsync(detail);
                                }
                            }
                        }
                        catch (Exception ex) {
                        }
                    }
                    else if (log.Status == StatusPCL.TCRAlreadyRegistered) {
                        var levizjaHeader = levizjetHeader
                                        .FirstOrDefault(h => h.NumriLevizjes == inv.Numri_Levizjes);

                        if (levizjaHeader != null) {
                            levizjaHeader.TCRSyncStatus = 4;
                            levizjaHeader.TCRIssueDateTime = DateTime.Now;
                            levizjaHeader.TCRQRCodeLink = log.QRCodeLink;
                            levizjaHeader.TCR = Agjendi.TCRCode;
                            levizjaHeader.TCROperatorCode = Agjendi.OperatorCode;
                            levizjaHeader.TCRBusinessUnitCode = App.Instance.MainViewModel.Configurimi.KodiINjesiseSeBiznesit;
                            levizjaHeader.UUID = log.ResponseUUIDSH;
                            levizjaHeader.TCRNSLFSH = log.NSLFSH;
                            levizjaHeader.TCRNIVFSH = log.NIVFSH;
                            levizjaHeader.Message = log.Message.Replace("'", "");

                            await App.Database.UpdateLevizjeHeaderAsync(levizjaHeader);
                        }

                        var levizjetDetailsToUpdate = levizjetDetails
                                    .Where(d => d.NumriLevizjes == inv.Numri_Levizjes)
                                    .ToList();

                        foreach (var detail in levizjetDetailsToUpdate) {
                            detail.TCRSyncStatus = 4;
                            await App.Database.UpdateLevizjeDetailsAsync(detail);
                        }
                    }
                }

            }

        }

        public async Task SyncTCRInvoices(int resync) {

            var liferimi = await App.Database.GetLiferimetAsync();
            var liferimiArt = await App.Database.GetLiferimetArtAsync();
            var query = from l in liferimi
                        join la2 in liferimiArt on l.IDLiferimi equals la2.IDLiferimi
                        where (l.TCRSyncStatus <= 0 || l.TCRSyncStatus == null) &&
                              (l.CmimiTotal >= 0 && resync == 1 || l.CmimiTotal < 0 && resync == 2) &&
                              l.DeviceID == Agjendi.DeviceID
                        group la2 by la2.IDLiferimi into g
                        select new MapperHeader
                        {
                            IDLiferimi = g.Key.ToString(),
                            TotaliPaTVSH = g.Sum(x => Math.Round(x.TotaliPaTVSH, 2)),
                            TotaliMeTVSH = g.Sum(x => Math.Round(x.Totali, 2)),
                            TVSH = g.Sum(x => Math.Round(x.Totali - x.TotaliPaTVSH, 2))
                        };

            List<MapperHeader> mapperHeaderList = query.ToList();

            if (mapperHeaderList.Count > 0) {
                var klientet = await App.Database.GetKlientetAsync();
                var Klientdhelokacion = await App.Database.GetKlientetDheLokacionetAsync();
                var artikujt = await App.Database.GetArtikujtAsync();
                var companyInfo = await App.Database.GetCompanyInfoAsync();

                var query2 = from l in liferimi
                             join la in liferimiArt on l.IDLiferimi equals la.IDLiferimi
                             join k in klientet on l.IDKlienti equals k.IDKlienti
                             join kl in Klientdhelokacion on k.IDKlienti equals kl.IDKlienti
                             join a in artikujt on la.IDArtikulli equals a.IDArtikulli
                             join ci in companyInfo on "TVSH" equals ci.Item
                             where l.TCRSyncStatus != 1 && l.DeviceID == Agjendi.DeviceID && (l.CmimiTotal >= 0 && resync == 1 || l.CmimiTotal < 0 && resync == 2)
                             select new MapperLines
                             {
                                 IDLiferimi = l.IDLiferimi.ToString(),
                                 NrLiferimit = l.NrLiferimit,
                                 DeviceID = l.Depo,
                                 OperatorCode = "OperatorCode", // Replace with the actual operator code
                                 InvoiceSType = "Cash GJITHMON", // Replace with the actual invoice type
                                 InvNum = l.NumriFisk + "/" + DateTime.Now.Year,
                                 InvOrdNum = Convert.ToInt32(l.NumriFisk),
                                 SendDatetime = l.KohaLiferimit,
                                 IsIssuerInVAT = true,
                                 TaxFreeAmt = 0,
                                 PaymentMethodTypesType = l.PayType,
                                 Buyer_IDNum = string.IsNullOrEmpty(k.NIPT) ? "" : k.NIPT,
                                 Buyer_IDType = "idtypestype.id", // Replace with the actual ID type
                                 Buyer_Name = k.Emri,
                                 Buyer_Address = kl.Adresa,
                                 Buyer_Country = 5,
                                 Buyer_Town = kl.EmriLokacionit,
                                 Item_N = a.Emri,
                                 Item_C = a.IDArtikulli,
                                 Item_U = a.BUM,
                                 Item_Q = la.SasiaLiferuar,
                                 Item_UPB = Math.Round(la.CmimiPaTVSH, 2),
                                 Item_UPA = Math.Round(la.Cmimi, 2),
                                 Item_PB = Math.Round(la.TotaliPaTVSH, 2),
                                 Item_PA = Math.Round(la.Totali, 2),
                                 Item_VA = Math.Round(la.Totali - la.TotaliPaTVSH, 2),
                                 Item_VR = (double)Math.Round(decimal.Parse(ci.Value.ToString()), 2),
                                 MobileRefId = "",
                                 IICRef = "",
                                 IssueDateTimeRef = DateTime.Now,
                                 TypeRef = "CORRECTIVE|DEBIT|CREDIT",
                                 IsCorrectiveInv = false,
                                 CardNumber = "",
                                 IsReverseCharge = false,
                                 LLOJDOK = l.LLOJDOK,
                                 IDKthimi = l.IDKthimi ?? ""
                             };

                List<MapperLines> mapperLinesList = query2.ToList();

                List<TCRLiferimi> invoiceObject = ClassMapper.MapHeadersAndLines(mapperHeaderList, mapperLinesList);
                foreach (TCRLiferimi inv in invoiceObject) {
                    RegisterInvoiceInputRequestPCL req = new RegisterInvoiceInputRequestPCL();
                    req.Buyer = inv.Buyer;
                    req.InvoiceItems = inv.Items.ToList();

                    req.DeviceID = inv.DeviceID;
                    req.NrLiferimit = inv.NrLiferimit;
                    req.SubseqDelivTypeSType = 0;//NOINTERNET
                    req.IICRef = inv.IICRef;
                    req.InoiceType = InvoiceSTypePCL.CASH;
                    req.InoiceTypeSpecified = true;
                    req.InvOrdNum = inv.InvOrdNum.ToString();
                    req.IsCorrectiveInv = inv.IsCorrectiveInv;
                    req.IsIssuerInVAT = inv.IsIssuerInVAT;
                    req.IsReverseCharge = inv.IsReverseCharge;
                    req.IssueDateTimeRef = inv.IssueDateTimeRef;
                    req.MobileRefId = inv.InvOrdNum.ToString();
                    req.OperatorCode = Agjendi.OperatorCode;
                    req.TCRCode = Agjendi.TCRCode;
                    req.BusinessUnitCode = App.Instance.MainViewModel.Configurimi.KodiINjesiseSeBiznesit;
                    //if (inv.PaymentMethodTypesType.ToUpper() == "BANK")
                    //    req.PaymentMethodTypeSType = PaymentMethodTypeSType.CARD;
                    //else
                    //    req.PaymentMethodTypeSType = PaymentMethodTypeSType.BANKNOTE;
                    if (inv.PaymentMethodTypesType.ToUpper() == "BANK") {
                        req.PaymentMethodTypeSType = PaymentMethodTypeSTypePCL.ACCOUNT;
                        req.InoiceType = InvoiceSTypePCL.NONCASH;
                        req.InvNum = inv.InvNum;
                    }
                    else {
                        req.PaymentMethodTypeSType = PaymentMethodTypeSTypePCL.BANKNOTE;
                        req.InvNum = inv.InvNum + "/" + Agjendi.TCRCode;
                    }

                    req.PaymentMethodTypeSTypeSpecified = true;

                    req.PriceWithoutVAT = (decimal)inv.TotaliPaTVSH;
                    req.PriceWithoutVATSpecified = true;
                    req.PriceWithVAT = (decimal)inv.TotaliMeTVSH;
                    req.PriceWithVATSpecified = true;
                    req.SendDatetime = inv.SendDatetime;
                    req.TaxFreeAmt = (decimal)inv.TaxFreeAmt;
                    req.TaxFreeAmtSpecified = true;
                    req.TypeRef = CorrectiveInvTypeSTypePCL.CORRECTIVE;
                    req.VATAmount = (decimal)inv.TVSH;
                    req.VATAmountSpecified = true;

                    //Kthim Malli
                    if (!inv.IsShitje) {
                        req.IICRef = inv.IDKthimi;
                        req.TypeRef = CorrectiveInvTypeSTypePCL.CORRECTIVE;
                        req.TypeRefSpecified = true;
                        req.IsCorrectiveInv = true;
                    }
                    else if (!string.IsNullOrEmpty(inv.IDKthimi)) {
                        req.IICRef = inv.IDKthimi.Replace(" ", "");
                        req.TypeRef = CorrectiveInvTypeSTypePCL.CORRECTIVE;
                        req.TypeRefSpecified = true;
                        req.IsCorrectiveInv = true;
                    }

                    ResultLogPCL log = App.Instance.FiskalizationService.RegisterInvoice(req);

                    if (log.Status == StatusPCL.Ok) {
                        var liferimet = await App.Database.GetLiferimetAsync();
                        var liferimetArt = await App.Database.GetLiferimetArtAsync();
                        var liferimiToUpdate = liferimet
                                            .FirstOrDefault(l => l.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                        if (liferimiToUpdate != null) {
                            liferimiToUpdate.TCRSyncStatus = -1;
                            liferimiToUpdate.TCRIssueDateTime = DateTime.Now;
                            liferimiToUpdate.TCRQRCodeLink = log.QRCodeLink;
                            liferimiToUpdate.TCR = Agjendi.TCRCode;
                            liferimiToUpdate.TCROperatorCode = Agjendi.OperatorCode;
                            liferimiToUpdate.TCRBusinessCode = liferimiToUpdate.TCRBusinessCode;
                            liferimiToUpdate.UUID = log.ResponseUUID;
                            liferimiToUpdate.EIC = log.EIC;
                            liferimiToUpdate.TCRNSLF = log.NSLF;
                            liferimiToUpdate.TCRNIVF = log.NIVF;
                            liferimiToUpdate.Message = log.Message.Replace("'", "");

                            await App.Database.SaveLiferimiAsync(liferimiToUpdate);

                            var liferimiArtToUpdate = liferimetArt
                                    .Where(la => la.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                            foreach (var art in liferimiArtToUpdate) {
                                art.TCRSyncStatus = 2;
                                await App.Database.UpdateLiferimiArtAsync(art);
                            }
                        }
                    }
                    else if (log.Status == StatusPCL.FaultCode) {
                        if (string.IsNullOrEmpty(log.Message)) {
                            var liferimet = await App.Database.GetLiferimetAsync();
                            var liferimetArt = await App.Database.GetLiferimetArtAsync();
                            var liferimiToUpdate = liferimet
                                                .FirstOrDefault(l => l.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                            if (liferimiToUpdate != null) {
                                liferimiToUpdate.TCRSyncStatus = -1;
                                liferimiToUpdate.TCRIssueDateTime = DateTime.Now;
                                liferimiToUpdate.TCRQRCodeLink = log.QRCodeLink;
                                liferimiToUpdate.TCR = Agjendi.TCRCode;
                                liferimiToUpdate.TCROperatorCode = Agjendi.OperatorCode;
                                liferimiToUpdate.TCRBusinessCode = liferimiToUpdate.TCRBusinessCode;
                                liferimiToUpdate.UUID = log.ResponseUUID;
                                liferimiToUpdate.EIC = log.EIC;
                                liferimiToUpdate.TCRNSLF = log.NSLF;
                                liferimiToUpdate.TCRNIVF = log.NIVF;
                                liferimiToUpdate.Message = "Fiskalizimi deshtoi, ju lutemi provoni me vone!";

                                await App.Database.SaveLiferimiAsync(liferimiToUpdate);


                                var liferimiArtToUpdate = liferimetArt
                                        .Where(la => la.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                                foreach (var art in liferimiArtToUpdate) {
                                    art.TCRSyncStatus = -1;
                                    await App.Database.UpdateLiferimiArtAsync(art);
                                }
                            }
                        }
                        else {
                            var liferimet = await App.Database.GetLiferimetAsync();
                            var liferimetArt = await App.Database.GetLiferimetArtAsync();
                            var liferimiToUpdate = liferimet
                                                .FirstOrDefault(l => l.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                            if (liferimiToUpdate != null) {
                                liferimiToUpdate.TCRSyncStatus = -1;
                                liferimiToUpdate.TCRIssueDateTime = DateTime.Now;
                                liferimiToUpdate.TCRQRCodeLink = log.QRCodeLink;
                                liferimiToUpdate.TCR = Agjendi.TCRCode;
                                liferimiToUpdate.TCROperatorCode = Agjendi.OperatorCode;
                                liferimiToUpdate.TCRBusinessCode = liferimiToUpdate.TCRBusinessCode;
                                liferimiToUpdate.UUID = log.ResponseUUID;
                                liferimiToUpdate.EIC = log.EIC;
                                liferimiToUpdate.TCRNSLF = log.NSLF;
                                liferimiToUpdate.TCRNIVF = log.NIVF;
                                liferimiToUpdate.Message = log.Message;

                                await App.Database.SaveLiferimiAsync(liferimiToUpdate);

                                var liferimiArtToUpdate = liferimetArt
                                        .Where(la => la.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                                foreach (var art in liferimiArtToUpdate) {
                                    art.TCRSyncStatus = -1;
                                    await App.Database.UpdateLiferimiArtAsync(art);
                                }
                            }
                        }
                    }
                    else if (log.Status == StatusPCL.TCRAlreadyRegistered) {
                        var liferimet = await App.Database.GetLiferimetAsync();
                        var liferimetArt = await App.Database.GetLiferimetArtAsync();
                        var liferimiToUpdate = liferimet
                            .Where(l => l.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                        foreach (var liferi in liferimiToUpdate) {
                            liferi.TCRSyncStatus = 4;
                            liferi.TCRIssueDateTime = DateTime.Now;
                            liferi.TCRQRCodeLink = log.QRCodeLink;
                            liferi.TCR = Agjendi.TCRCode;
                            liferi.TCROperatorCode = Agjendi.OperatorCode;
                            liferi.TCRBusinessCode = liferi.TCRBusinessCode;
                            liferi.UUID = log.ResponseUUID;
                            liferi.EIC = log.EIC;
                            liferi.TCRNSLF = log.NSLF;
                            liferi.TCRNIVF = log.NIVF;
                            liferi.Message = log.Message.Replace("'", "");
                            await App.Database.SaveLiferimiAsync(liferi);

                        }

                        var liferimiArtToUpdate = liferimetArt
                            .Where(la => la.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                        foreach (var art in liferimiArtToUpdate) {
                            art.TCRSyncStatus = 4;
                            await App.Database.UpdateLiferimiArtAsync(art);
                        }
                    }
                    else if (log.Status == StatusPCL.InProcess) {
                        var liferimet = await App.Database.GetLiferimetAsync();
                        var liferimetArt = await App.Database.GetLiferimetArtAsync();
                        var liferimiToUpdate = liferimet
                            .Where(l => l.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                        foreach (var liferi in liferimiToUpdate) {
                            liferi.TCRSyncStatus = -2;
                            liferi.TCRIssueDateTime = DateTime.Now;
                            liferi.TCRQRCodeLink = log.QRCodeLink;
                            liferi.TCR = Agjendi.TCRCode;
                            liferi.TCROperatorCode = Agjendi.OperatorCode;
                            liferi.TCRBusinessCode = liferi.TCRBusinessCode;
                            liferi.UUID = log.ResponseUUID;
                            liferi.EIC = log.EIC;
                            liferi.TCRNSLF = log.NSLF;
                            liferi.TCRNIVF = log.NIVF;
                            liferi.Message = log.Message.Replace("'", "");
                            await App.Database.SaveLiferimiAsync(liferi);

                        }

                        var liferimiArtToUpdate = liferimetArt
                            .Where(la => la.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                        foreach (var art in liferimiArtToUpdate) {
                            art.TCRSyncStatus = -2;
                            await App.Database.UpdateLiferimiArtAsync(art);
                        }
                    }
                    else {
                        try {
                            var liferimet = await App.Database.GetLiferimetAsync();
                            var liferimetArt = await App.Database.GetLiferimetArtAsync();
                            var liferimiToUpdate = liferimet
                                .Where(l => l.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                            foreach (var lif in liferimiToUpdate) {
                                lif.TCRSyncStatus = -3;
                                lif.TCRIssueDateTime = DateTime.Now;
                                lif.TCRQRCodeLink = log.QRCodeLink;
                                lif.TCR = Agjendi.TCRCode;
                                lif.TCROperatorCode = Agjendi.OperatorCode;
                                lif.TCRBusinessCode = lif.TCRBusinessCode;
                                lif.UUID = log.ResponseUUID;
                                lif.EIC = log.EIC;
                                lif.TCRNSLF = log.NSLF;
                                lif.TCRNIVF = log.NIVF;
                                lif.Message = log.Message.Replace("'", "");
                                await App.Database.SaveLiferimiAsync(lif);

                            }

                            var liferimiArtToUpdate = liferimetArt
                                .Where(la => la.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                            foreach (var art in liferimiArtToUpdate) {
                                art.TCRSyncStatus = -1;
                                await App.Database.UpdateLiferimiArtAsync(art);
                            }
                        }
                        catch (Exception ex) {
                            // Handle exception
                        }
                    }
                }
            }
        }


        private async Task ShkarkoMalliMbetur() {

            try {
                var updatedMalliMbetur = await UpdateMalli_Mbetur();
                    if (!updatedMalliMbetur) {
                        UserDialogs.Instance.Alert("Gabmi gjate azhurimit te [Malli_Mbetur]. Shkarkimi nuk mund te vazhdoje");
                        return;
                    }
                string[] KeyFields = new string[1];

                KeyFields[0] = "PKeyIDArtDepo";

                //@BE 02.11.2018 Rreshti Poshte eshte shtuar ashtu qe te behet se pari fshirja e mallit te mbetur dhe tek me pas edhe sinkronizimi nga pajisja
                //MalliMbeturSync.ExecuteServerQuery("DELETE FROM Malli_Mbetur where Depo = '" + c.DeviceID.Replace("-", "") + "'");

                //SyncSuccesful = Snc.SyncTable(strTableName, KeyFields, PnSyncDirection.Upload, strFilterUp, StatusField, "");
                Sync MalliMbeturSync = new Sync();
                var success = await MalliMbeturSync.SyncTable("Malli_Mbetur", KeyFields, PnSyncDirection.UploadWithInsert, "1=1", "0", Agjendi.Depo);
                //@BE KOMENTUAR ME 27.08.2018 eshte kthyer prap rreshti lart //bool success = MalliMbeturSync.SyncTable("Malli_Mbetur", null, PnSyncDirection.Upload, "1=1", "0", "");

                //@BE 02.11.2018 rreshti poshte eshte komentuar dhe zevendesuar me ate pasardhes
                //if (success && MalliMbeturSync.ExecuteServerQuery("UPDATE Malli_Mbetur SET Export_Status = 0 where Depo = '" + c.DeviceID.Replace("-", "") + "' AND Export_Status = 2 "))
                if (success) {
                    if (await Delete_Malli_Mbetur_Stoqet(Agjendi.Depo)) {
                        UserDialogs.Instance.Alert("Te gjitha mallrat jane shkarkuar");
                        UserDialogs.Instance.HideLoading();
                        return;
                    }
                    else {
                        UserDialogs.Instance.Alert("Mallrat jane shkarkuar por nuk jane rifreskuar");
                        return;
                    }
                }
                else {
                    UserDialogs.Instance.Alert("Gabim gjate shkarkimit ne SERVER, provoni perseri ");
                    return;
                }

            }
            catch (Exception ex) {
                UserDialogs.Instance.Alert("Gabim gjate shkarkimit: " +ex.Message);
            }
            finally {
                UserDialogs.Instance.HideLoading();
            }
        }

        public async Task<bool> Delete_Malli_Mbetur_Stoqet(string depo) {
            bool result = false;
            try {

                var malliMbeturDeleteResult = await App.Database.ClearAllMalliMbetur();
                if (malliMbeturDeleteResult != -1)
                    result = true;
                var stoqetDeleteResult = await App.Database.ClearAllStoqetAsync();
                if (stoqetDeleteResult != -1)
                    result = true;
                //TODO : Do this locally first then double check for server
                //var malliMbeturDeleteResut = await App.ApiClient.DeleteAsync("mallimbetur?depo=" + depo);
                //if(malliMbeturDeleteResut.IsSuccessStatusCode) {
                //    result = true;
                //}
                //var stoqetDeleteResut = await App.ApiClient.DeleteAsync("stoqet?depo=" + depo);
                //if (stoqetDeleteResut.IsSuccessStatusCode) {
                //    result = true;
                //}
            }
            catch (Exception es) {
                result = false;
            }
            return result;
        }

        private async Task SyncAll() {
            try {
                if (!App.Instance.DoIHaveInternet()) {
                    return;
                }
                UserDialogs.Instance.ShowLoading("Inicializimi... ");
                string[] KeyFields = null;//Used in sync
                string strFilterDwn = "1=1";//Used in sync
                string strFilterUp = "1=1"; //Used in sync           
                string strTableName = "";//Used in sync            
                App.ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Instance.MainViewModel.Configurimi.Token);
                App.ApiClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                int _CurrNrFat = 0; //used for fixNrKufijt
                int _CurrNrFatJT = 0;//used for fixNrKufijt
                int _CurrNrFat_D = 0; //used for fixNrKufijt
                StringBuilder strbSyncLog = new StringBuilder();//used for Log in TextBox Control

                bool SyncSuccesful = true; //Used in sync
                bool SyncSendSuccessful = true; //Used in Sync
                MobSellSyncDirection MobSyncDirection; //Used in Sync
                int _stoqetServer = 0;
                Sync sync = new Sync();
                var SyncConfigSynced = await sync.SyncTable("SyncConfiguration", null, PnSyncDirection.SnapshotDownload, "", "", "");
                //MessageBox.Show("SyncConfigSynced " + SyncConfigSynced.ToString());
                var ConfigSynced = await sync.SyncTable("Konfigurimi", null, PnSyncDirection.SnapshotDownload, "", "", Agjendi.DeviceID);

                if(!SyncConfigSynced || !ConfigSynced) {
                    UserDialogs.Instance.Alert("Sinkronizimi nuk mund te vazhdoje [KOMUNIKIMI ME MSQLMOBILESERVER deshtoi]", "Gabim");
                    UserDialogs.Instance.HideLoading();
                    return;
                }
                EditorView = "";
                EditorView += "Sync Config dhe Konfig SnapShotDownload Sukses\n";
                var konfigurimi = (await App.Database.GetKonfigurimetAsync()).FirstOrDefault(x => x.DeviceID == Agjendi.DeviceID);
                Agjendi.IDAgjenti = konfigurimi.IDAgent;
                Agjendi.Depo = konfigurimi.IDAgent;

                try {
                    var Sinkronizimet = (await App.Database.GetSyncConfigurationsAsync()).OrderBy(x => x.ID).ToList();
                    if (Sinkronizimet != null && Sinkronizimet.Count > 0) {
                        var strSessionID = new Guid();
                        await InsertLog(Agjendi.DeviceID, strSessionID, DateTime.Now, null, "START_SESSION", "", "", 0, 0, 0, 0, 0);
                        var indexTable = 0;
                        //FOR EACH TABLE IN SYNCCONFIG
                        foreach(var config in Sinkronizimet) {
                            MobSyncDirection = (MobSellSyncDirection)Convert.ToInt16(config.SyncDirection);
                            strTableName = config.TableName;
                            KeyFields = config.PK_FieldName.Split(',');
                            indexTable++;
                            UserDialogs.Instance.ShowLoading("Sinkronizimi Tabela: " + strTableName + " " + indexTable + "/" + Sinkronizimet.Count);
                            if (strTableName == "Detyrimet") {

                            }
                            strFilterUp = config.FilterUp.ToString();
                            strFilterDwn = config.FilterDown.ToString();

                            if (strFilterUp.Trim() == "")
                                strFilterUp = "1=1";

                            if (strFilterDwn.Trim() == "")
                                strFilterDwn = "1=1";
                            if(strTableName == "Krijimi_Porosive") {

                            }
                            switch (strTableName) {

                                case "KlientDheLokacion":
                                case "Klientet":
                                case "Stoqet": {
                                        strFilterDwn = "(Depo='" + Agjendi.Depo + "' OR Depo='' OR Depo IS NULL)";
                                        strFilterUp = "(Depo='" + Agjendi.Depo + "' OR Depo='' OR Depo IS NULL)";
                                        break;
                                    }
                                case "SalesPrice": {
                                        string getOnlyThisWeekKlients = @"SalesCode IN 
                                                                    (  SELECT IDKlientDheLokacion FROM Vizitat WHERE IDAgjenti ='" + Agjendi.IDAgjenti + @"' ) OR SalesCode='STANDARD' ";
                                        strFilterDwn = "(Depo='" + Agjendi.Depo + "' OR Depo='' OR Depo IS NULL) and (" + getOnlyThisWeekKlients + ")";
                                        strFilterUp = "(Depo='" + Agjendi.Depo + "' OR Depo='' OR Depo IS NULL) and (" + getOnlyThisWeekKlients + ")";
                                        break;
                                    }
                                case "Vizitat": {
                                        string AddUPFilter = config.FilterUp.ToString();
                                        string AddDwnFilter = config.FilterDown.ToString();

                                        strFilterDwn = "DeviceID='" + Agjendi.DeviceID + "'";
                                        strFilterUp = "DeviceID='" + Agjendi.DeviceID + "'";

                                        if (AddDwnFilter.Trim() != "")
                                            strFilterDwn += " AND '" + AddDwnFilter + "'";
                                        if (AddUPFilter.Trim() != "") //if Filter from SyncCOnfig not empty
                                            strFilterUp += " AND '" + AddUPFilter + "'";

                                        break;
                                    }
                                case "Malli_Mbetur": {
                                        await UpdateMalli_Mbetur();
                                        break;
                                    }
                            }
                            await Task.Delay(200);
                            SaveValuesKufijtFaturave(strTableName);
                            await Task.Delay(200);
                            #region DoPnSync
                            switch (MobSyncDirection) {

                                case MobSellSyncDirection.SnapshotSpecial: {
                                        //Wont't do the snapshot if server returns nothing
                                        EditorView += "Syncing Mob Sync Direction SnapshotSpecial " + strTableName + " STARTED \n";
                                        if (strTableName == "Stoqet") {
                                            _stoqetServer = await GetStoqetCount(Agjendi.Depo);
                                        }
                                        SyncSuccesful = await sync.SyncTable(strTableName, KeyFields, PnSyncDirection.SnapshotSpecial, strFilterDwn, "SyncStatus", Agjendi.Depo);
                                        var sasiaShitur = (double)await SumDataColumn("Malli_Mbetur", "SasiaShitur");
                                        var SasiaKthyer = await SumDataColumn("Malli_Mbetur", "SasiaKthyer");

                                        if ((int)sasiaShitur == 0 && (int)SasiaKthyer == 0) {
                                            if (_stoqetServer != 0) //Nese ka stoke per ngarkim
                                            {
                                                CopyStock();
                                            }
                                        }
                                        EditorView += "Syncing Mob Sync SnapshotSpecial Snapshot " + strTableName + "  " + (SyncSuccesful ? "Success" : "False") + "\n";
                                        break;
                                    }

                                case MobSellSyncDirection.Snapshot: {
                                        EditorView += "Syncing Mob Sync Direction Snapshot "  + strTableName + " STARTED \n";
                                        SyncSuccesful = await sync.SyncTable(strTableName, null, PnSyncDirection.SnapshotDownload, strFilterUp, "", Agjendi.Depo);
                                        EditorView += "Syncing Mob Sync Direction Snapshot " + strTableName + "  " + (SyncSuccesful ? "Success" : "False") + "\n";
                                        break;
                                    }

                                case MobSellSyncDirection.Download:  //download with insert and Update
                                    {
                                        EditorView += "Syncing Mob Sync Direction Download " + strTableName + " STARTED \n";
                                        SyncSuccesful = await sync.SyncTable(strTableName, KeyFields, PnSyncDirection.Download, strFilterDwn, "SyncStatus", "");
                                        EditorView += "Syncing Mob Sync Direction Download " + strTableName + "  " + (SyncSuccesful ? "Success" : "False") + "\n";

                                        break;
                                    }
                                case MobSellSyncDirection.Upload: //upload with insert and update
                                    {
                                        EditorView += "Syncing Mob Sync Direction Upload " + strTableName + " STARTED \n";
                                        string StatusField = "SyncStatus";
                                        if (strTableName == "Malli_Mbetur") {
                                            StatusField = "0";
                                        }
                                        else {
                                            KeyFields = null;
                                        }
                                        SyncSuccesful = await sync.SyncTable(strTableName, KeyFields, PnSyncDirection.Upload, strFilterUp, StatusField, "");
                                        if (SyncSuccesful) {
                                            switch (strTableName) {
                                                case "Liferimi": {
                                                        //SafeDelete("Liferimi", "IDLiferimi");
                                                        //TODO : UPDATE LIFERIMI API
                                                        var liferimet = await App.Database.ClearAllLiferimet();
                                                        //var updateLiferimetResult = await App.ApiClient.PutAsync("")
                                                        break;
                                                    }
                                                case "LiferimiArt": {
                                                        var liferimetArt = await App.Database.ClearAllLiferimetArt();
                                                        break;

                                                    }
                                                case "Porosite": {
                                                        var porosite = await App.Database.ClearAllPorosite();
                                                        break;
                                                    }
                                                case "PorosiaArt": {
                                                        var porositeArt = await App.Database.ClearAllPorositeArt();
                                                        break;
                                                    }
                                                case "LEVIZJET_HEADER": {
                                                        //SafeDelete("LEVIZJET_HEADER", "TransferID");
                                                        //Snc.ExecuteServerQuery("UPDATE LEVIZJET_HEADER set ImpStatus = 0 where Depo = '" + SyncParams.DeviceID.Replace("-", "") + "' AND ImpStatus =2");
                                                        var levizjetHeader = await App.Database.GetLevizjetHeaderAsync();
                                                        var query = from header in levizjetHeader
                                                                    where header.Depo == Agjendi.Depo && header.ImpStatus == 2
                                                                    select header;
                                                        foreach (var res in query) {
                                                            res.ImpStatus = 0;
                                                            await App.Database.SaveLevizjeHeaderAsync(res);
                                                        }
                                                        levizjetHeader = await App.Database.GetLevizjetHeaderAsync();
                                                        var levizjetHeaderJson = JsonConvert.SerializeObject(levizjetHeader);
                                                        var stringContent = new StringContent(levizjetHeaderJson, Encoding.UTF8, "application/json");
                                                        var result = await App.ApiClient.PutAsync("levizje-header", stringContent);
                                                        if (!result.IsSuccessStatusCode) {
                                                            SyncSuccesful = false;
                                                        }
                                                        await App.Database.ClearAllLevizjetHeaderAsync();
                                                        break;
                                                    }
                                                case "LEVIZJET_DETAILS": {
                                                        await App.Database.ClearAllLevizjetDetailsAsync();
                                                        break;
                                                    }
                                                case "Krijimi_Porosive": {
                                                        await App.Database.ClearAllKrijimiPorosive();
                                                        break;
                                                    }
                                                case "Orders": {
                                                        await App.Database.ClearAllOrdersAsync();
                                                        break;
                                                    }
                                                case "Order_Details": {
                                                        //TODO UPDATE ORDER DETAILS
                                                        
                                                        await App.Database.ClearAllOrderDetailsAsync();
                                                        SyncSuccesful = true;

                                                        break;
                                                    }

                                            }
                                        }
                                        EditorView += "Syncing Mob Sync Direction Upload " + strTableName + "  " + (SyncSuccesful ? "Success" : "False") + "\n";

                                        break;
                                    }
                                case MobSellSyncDirection.Bidirectional: //Download and Upload with Update and insert (bidirectional)
                                    {
                                        EditorView += "Syncing Mob Sync Direction Bidirectional " + strTableName + " STARTED \n";

                                        //***Upload***
                                        if(strTableName == "Vizitat") {
                                            SyncSuccesful = await sync.SyncTable(strTableName, KeyFields, PnSyncDirection.Upload, strFilterUp, "SyncStatus", Agjendi.DeviceID);
                                        }
                                        else
                                            SyncSuccesful = await sync.SyncTable(strTableName, KeyFields, PnSyncDirection.Upload, strFilterUp, "SyncStatus", Agjendi.Depo);
                                        //LiveLog(SyncParams, strbSyncLog, strTableName, SyncSuccesful, MobSyncDirection, con, strSessionID);

                                        //****Now download****
                                        if(strTableName == "Vizitat") {
                                            SyncSuccesful = await sync.SyncTable(strTableName, KeyFields, PnSyncDirection.Download, strFilterDwn, "SyncStatus", Agjendi.DeviceID);
                                        }
                                        else
                                            SyncSuccesful = await sync.SyncTable(strTableName, KeyFields, PnSyncDirection.Download, strFilterDwn, "SyncStatus", Agjendi.Depo);
                                        //LiveLog(SyncParams, strbSyncLog, strTableName, SyncSuccesful, MobSyncDirection, con, strSessionID);
                                        EditorView += "Syncing Mob Sync Direction Bidirectional " + strTableName + "  " + (SyncSuccesful ? "Success" : "False") + "\n";
                                        break;
                                    }

                                case MobSellSyncDirection.BidirectionalSpecial: //this Downloads to Device (insert,update) to spec. table, Uploads to tableName+"_upl" (insert,update)
                                    {
                                        EditorView += "Syncing Mob Sync Direction BidirectionalSpecial " + strTableName + "STARTED \n";
                                        //***Upload***SEND DATA TO SPECIFIED TABLENAME+"_upl" WITH INSERT AND UPDATE
                                        //****Now download**** NORMAL DOWNLOAD WITH INSERT AND UPDATE
                                        SyncSuccesful = await sync.SyncTable(strTableName, KeyFields, PnSyncDirection.Download, strFilterDwn, "SyncStatus", "");
                                        EditorView += "Syncing Mob Sync Direction BidirectionalSpecial " + strTableName + "  " + (SyncSuccesful ? "Success" : "False") + "\n";
                                        break;
                                    }
                            } //END SWRITCH MobSellDirection
                            #endregion
                        }
                    }
                    else {
                        UserDialogs.Instance.Alert("Nuk ka tabela per sinkronizim ne SyncConfig");
                    }
                }
                catch (Exception ex) 
                {
                    UserDialogs.Instance.HideLoading();
                }
            }
            catch(Exception e) {
                UserDialogs.Instance.HideLoading();
            }
            UserDialogs.Instance.HideLoading();
            UserDialogs.Instance.Alert("Sinkronizimi perfundoi");
        }

        //**********************************SYNCALL METHOD (Application Specific)*********************************************
        public async Task<bool> AzhurnoAsync() {
            bool success = true;
            #region InitializeSync
            GC.Collect();
            StringBuilder strbSyncLog = new StringBuilder();//used for Log in TextBox Control
            string[] KeyFields = null;//Used in sync
            string strFilterDwn = "1=1";//Used in sync
            string strFilterUp = "1=1"; //Used in sync           
            string strTableName = "";//Used in sync            

            Sync sync = new Sync();

            bool SyncSuccesful = true; //Used in sync
            bool SyncSendSuccessful = true; //Used in Sync
            MobSellSyncDirection MobSyncDirection; //Used in Sync

            //*****************Configs SYNC******************************************************************

            var SyncConfigSynced = await sync.SyncTable("SyncConfiguration", null, PnSyncDirection.SnapshotDownload, "", "", "");
            //MessageBox.Show("SyncConfigSynced " + SyncConfigSynced.ToString());
            var ConfigSynced = await sync.SyncTable("Konfigurimi", null, PnSyncDirection.SnapshotDownload, "", "", Agjendi.DeviceID);

            if (!SyncConfigSynced || !ConfigSynced) {
                UserDialogs.Instance.Alert("Sinkronizimi nuk mund te vazhdoje [KOMUNIKIMI ME MSQLMOBILESERVER deshtoi]", "Gabim");
                UserDialogs.Instance.HideLoading();
                return false;
            }
            // //MessageBox.Show("ConfigSynced " + ConfigSynced.ToString());
            //if ((!ConfigSynced) || (!SyncConfigSynced))
            //{
            //    ////MessageBox.Show("Sinkronizimi nuk mund te vazhdoje [Komunikimi me SqlMobileServer deshtoi!]", "Gabim", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            //    DbUtils.WriteErrorLog(PnSync.Sync.SyncLog.ErrorMsg);
            //    DbUtils.WriteErrorLog(PnSync.Sync.SyncLog.ErrorCode);

            //    Cursor.Current = Cursors.Default;
            //    return false;
            //}

            #endregion

            var tableNames = new[]
            {
                "Liferimi",
                "LiferimiArt",
                "Porosite",
                "PorosiaArt",
                "Orders",
                "Order_Details",
                "LEVIZJET_HEADER",
                "LEVIZJET_DETAILS"
            };
            var syncConfigs = await App.Database.GetSyncConfigurationsAsync();
            if(syncConfigs.Count == 0) {
                var SyncConfigurationResponse = await App.ApiClient.GetAsync("sync-configuration");
                var SyncConfigurationResult = await SyncConfigurationResponse.Content.ReadAsStringAsync();
                if (SyncConfigurationResponse.IsSuccessStatusCode) {
                    var listOfSyncConfiguration = JsonConvert.DeserializeObject<List<SyncConfiguration>>(SyncConfigurationResult);
                    await App.Database.ClearAllSyncConfigs();
                    await App.Database.SaveSyncConfigs(listOfSyncConfiguration);
                    syncConfigs = listOfSyncConfiguration;
                }
            }
            var syncConfigurations = syncConfigs
                .Where(sc => tableNames.Contains(sc.TableName))
                .Concat(new[]
                {
                new SyncConfiguration
                {
                    // Assuming these properties match the ones in your SyncConfiguration entity
                     ID = 100,
                    TableName = "Malli_Mbetur",
                    SyncDirection = "1",
                    SyncDay = 0,
                    SyncOrder = 100,
                    // Other properties set to default values
                }
                        })
                        .OrderBy(sc => sc.SyncOrder)
                        .ToList();

                //*********************Sync Tables***************************************************************
                try {

                    if (syncConfigurations.Count < 1) {
                        ////MessageBox.Show("Nuk ka tabela per sinkronizim ne SyncConfig");
                        return false;
                    }

                    string strSessionID = System.Guid.NewGuid().ToString();

                 
                    //InserLog START SESSION

                    #region FOREACH_TABLE_IN_SYNCCONFIG
                    //******************FOR EACH TABLE IN SyncConfig*******************************************

                    //this var is used later to know which table has succeeded or failed, number of uploaded down. insert or updt records.
                    //SyncInfo = SyncRecords.GetTables(con.ConnectionString); 

                    for (int i = 0; i < syncConfigurations.Count; i++) {

                        MobSyncDirection = (MobSellSyncDirection)Convert.ToInt16(syncConfigurations[i].SyncDirection);
                        strTableName = syncConfigurations[i].TableName;
                        if(syncConfigurations[i].PK_FieldName != null)
                            KeyFields = syncConfigurations[i].PK_FieldName.Split(',');//get PK_Field1,PK_Field2 in array, if more than 1                    

                    #region BuildFilters
                    //******************Filters*******************************************
                    //if FilterUp FilterDown, are null, make 1=1, which is no filter just to fill the gap

                    strFilterUp = syncConfigurations[i].FilterUp.ToString();
                    strFilterDwn = syncConfigurations[i].FilterDown.ToString();

                        if (strFilterUp.Trim() == "")
                            strFilterUp = "1=1";

                        if (strFilterDwn.Trim() == "")
                            strFilterDwn = "1=1";
                        switch (strTableName) {

                            case "Malli_Mbetur": {
                                var malliMbetur = await App.Database.GetMalliMbeturAsync();
                                foreach (var malli in malliMbetur) {
                                    malli.Data = DateTime.Now;
                                }
                                    break;
                                }

                        }

                        #endregion

                        #region RefreshSyncProgress

                        #endregion

                        //SaveValuesKufijtFaturave(ref strFilterUp, strTableName, con, ref _CurrNrFat, ref _CurrNrFatJT, ref _CurrNrFat_D);

                        #region DoPnSync

                        switch (MobSyncDirection) {


                            case MobSellSyncDirection.Snapshot: {
                                    SyncSuccesful = await sync.SyncTable(strTableName, null, PnSyncDirection.SnapshotDownload, strFilterUp, "", "");
                                    break;
                                }

                            case MobSellSyncDirection.Download:  //download with insert and Update
                                {
                                    SyncSuccesful = await sync.SyncTable(strTableName, KeyFields, PnSyncDirection.Download, strFilterDwn, "SyncStatus", "");
                                    break;
                                }
                            case MobSellSyncDirection.Upload: //upload with insert and update
                                {
                                    string StatusField = "SyncStatus";
                                    string sexec = string.Empty;
                                    if (strTableName == "Malli_Mbetur") 
                                    {
                                        StatusField = "0";
                                        SyncSuccesful = await sync.SyncTable(strTableName, KeyFields, PnSyncDirection.Upload, strFilterUp, StatusField, "");
                                    }
                                    else {
                                        SyncSuccesful = await sync.SyncTable(strTableName, KeyFields, PnSyncDirection.Upload, strFilterUp, StatusField, "");
                                        //DbUtils.ExecSql("Update " + strTableName + " set SyncStatus = 1 ");
                                    }


                                    sexec = string.Empty;
                                    break;
                                }
                            case MobSellSyncDirection.Bidirectional: //Download and Upload with Update and insert (bidirectional)
                                {
                                    //***Upload***
                                    SyncSuccesful = await sync.SyncTable(strTableName, KeyFields, PnSyncDirection.Upload, strFilterUp, "SyncStatus", "");

                                    //****Now download****
                                    SyncSuccesful = await sync.SyncTable(strTableName, KeyFields, PnSyncDirection.Download, strFilterDwn, "SyncStatus", "");
                                    break;
                                }

                            case MobSellSyncDirection.BidirectionalSpecial: //this Downloads to Device (insert,update) to spec. table, Uploads to tableName+"_upl" (insert,update)
                                {
                                    //***Upload***SEND DATA TO SPECIFIED TABLENAME+"_upl" WITH INSERT AND UPDATE
                                    //SyncSendSuccessful = await sync.SendData(strTableName, strTableName + "_upl", KeyFields, strFilterUp, "SyncStatus");
                                    //****Now download**** NORMAL DOWNLOAD WITH INSERT AND UPDATE
                                    SyncSuccesful = await sync.SyncTable(strTableName, KeyFields, PnSyncDirection.Download, strFilterDwn, "SyncStatus", "");
                                    break;
                                }
                        } //END SWRITCH MobSellDirection
                        #endregion

                        //FixKufijtFaturave(SyncSuccesful, strTableName, con, ref _CurrNrFat, ref _CurrNrFatJT, ref _CurrNrFat_D);

                        //DbUtils.ExecSql("UPDATE "+strTableName+" SET SYNCSTATUS=0 ");

                        #region LiveLogProcedures

                }//******************FOR EACH TABLE IN SyncConfig END*******************************************  

                #endregion


                #endregion

                #region SendLogToRemoteServer

                strbSyncLog = null;
                    //InserLog END SESSION                              

                    KeyFields = ("SessionID").Split(',');

                    #endregion

                    #region CompactLocalDB

                    #endregion

                    #region Smart Delete

                    if (SyncSuccesful) {
                        GC.Collect();

                        /*
                        DbUtils.ExecSql("Update Liferimi set SyncStatus = 1 ");
                        DbUtils.ExecSql("Update LiferimiArt set SyncStatus = 1 ");
                        DbUtils.ExecSql("Update Porosite set SyncStatus = 1 ");
                        DbUtils.ExecSql("Update PorosiaArt set SyncStatus = 1 ");
                        DbUtils.ExecSql("Update LEVIZJET_HEADER set SyncStatus = 1 ");
                        DbUtils.ExecSql("Update LEVIZJET_DETAILS set SyncStatus = 1 ");
                        DbUtils.ExecSql("Update Malli_Mbetur set SyncStatus = 1 ");
                        DbUtils.ExecSql("Update Orders set SyncStatus = 1 ");
                        DbUtils.ExecSql("Update Order_Details set SyncStatus = 1 ");
                        */
                    }

                    #endregion


                } //try
                catch (Exception ex) {
                    //Cursor.Current = Cursors.Default;
                    ////MessageBox.Show(ex.Message);
                    success = false;
                }
                finally {
                }
            return success;
        }

        //**********************************SYNCALL METHOD END*********************************************
        private static async void CopyStock() {
            try {
                string depo = "";
                string CurrentDir;
                string DevIDConfig;

                var deleteResult = await App.Database.ClearAllMalliMbetur();

                var artikujt = await App.Database.GetArtikujtAsync();
                var stoqet = await App.Database.GetAllStoqetAsync();
                stoqet = stoqet.Where(x => x.Depo == App.Instance.MainViewModel.LoginData.Depo).ToList();
                var selectedAritkulli = artikujt.FirstOrDefault();
                var selectedStoku = artikujt.FirstOrDefault();
                var query = from s in stoqet
                            join a in artikujt on s.Shifra equals a.IDArtikulli
                            select new
                            {
                                IDArtikulli = s.Shifra,
                                Emri = a.Emri,
                                SasiaPranuar = s.Sasia,
                                NrDoc = int.Parse("119" + s.NRDOK.ToString()),
                                SyncStatus = 0,
                                LLOJDOK = "KM",
                                SasiaMbetur = s.Sasia,
                                Depo = s.Depo,
                                Export_status = 1,
                                PKeyIDArtDepo = s.Shifra + s.Depo + (s.Seri == null ? "" : s.Seri.ToString()),
                                Seri = s.Seri
                            };
                foreach (var mall in query) {
                    var malliMbeturToAdd = new Malli_Mbetur
                    {
                        IDArtikulli = mall.IDArtikulli,
                        Emri = mall.Emri,
                        SasiaPranuar = (float)mall.SasiaPranuar,
                        NrDoc = (int)mall.NrDoc,
                        SyncStatus = mall.SyncStatus,
                        LLOJDOK = mall.LLOJDOK,
                        SasiaMbetur = (float)mall.SasiaMbetur,
                        Depo = mall.Depo,
                        Export_Status = 1,
                        PKeyIDArtDepo = mall.PKeyIDArtDepo,
                        Seri = mall.Seri,

                    };
                    await App.Database.SaveMalliMbeturNoReplaceAsync(malliMbeturToAdd);
                    var stoqetDeleteResut = await App.ApiClient.DeleteAsync("stoqet?depo=" + depo);
                }
            }
            catch(Exception e) {

            }

        }

        private async Task<int> GetStoqetCount(string Depo) {
            var result = await App.ApiClient.GetAsync("stoqet/"+ Depo);
            var response = await result.Content.ReadAsStringAsync();
            if(result.IsSuccessStatusCode) {
                var stoqet = JsonConvert.DeserializeObject<List<Stoqet>>(response);
                await App.Database.ClearAllStoqetAsync();
                await App.Database.SaveAllStoqetAsync(stoqet);
                var currentStoqet = await App.Database.GetAllStoqetAsync();
                return stoqet.Count;
            }
            else {
                return 0;
            }
        }


        private async Task<bool> UpdateMalli_Mbetur() {
            bool a = false;
            var malliMbetur = await App.Database.GetMalliMbeturAsyncWithDepo(Agjendi.Depo);
            foreach(var malli in malliMbetur) {
                malli.Data = DateTime.Now;
                malli.SyncStatus = 0;
            }
            var result = await App.Database.UpdateAllMalliMbeturAsync(malliMbetur);
            if (result != -1)
                a = true;
            else {
                a = false;
            }
            return a;
        }

        public List<Depot> Depot { get; set; }
        public List<Agjendet> Agjendet { get; set; }
        public List<NumriFisk> NumratFiskal { get; set; }
        public List<FiskalizimiKonfigurimet> FiskalizimiKonfigurimet { get; set; }

        private async Task<bool> InsertLog(string DeviceID, Guid SessionID, DateTime? StartSessionDT, DateTime? EndSessionDT, string TableName, string StartTime, string EndTime, int SyncDir, int NrDownloadedRect, int NrRecsUpdate, int NrRecsInsert, int NrUploadedRecs) {
            bool Success = false;

            if (StartTime == "") StartTime = "NULL";
            else StartTime = "'" + StartTime + "'";

            if (EndTime == "") EndTime = "NULL";
            else EndTime = "'" + EndTime + "'";

            var logSyncSession = new LogSync_Session
            {
                DeviceID = DeviceID,
                SessionID = SessionID,
                StartSessionDT = StartSessionDT,
                EndSessionDT = EndSessionDT,
                TableName = TableName,
                StartTime = StartTime,
                EndTime = EndTime,
                SyncDir = SyncDir,
                NrDownloadedRecs = NrDownloadedRect,
                NrRecsUpdate = NrRecsUpdate,
                NrRecsInsert = NrRecsInsert,
                NrUploadedRecs = NrUploadedRecs
            };

            var insertLogResult = await App.Database.SaveLogSyncSession(logSyncSession);
            if (insertLogResult != -1) {
                Success = true;
            }
            else
                Success = false;


            return Success;
        }

        public async Task<string[]> GetConfigurationByIDAgjentiAsync() {
            string[] result = new string[7];
            if(Depot == null) {
                var depotResult = await App.ApiClient.GetAsync("depot");
                if(depotResult.IsSuccessStatusCode) {
                    var depotResponse = await depotResult.Content.ReadAsStringAsync();
                    Depot = JsonConvert.DeserializeObject<List<Depot>>(depotResponse);
                }
            }
            if(Agjendet == null) {
                Agjendet = new List<Agjendet> { Agjendi };
            }
            NumratFiskal = await App.Database.GetNumratFiskalAsync();
            if(NumratFiskal.Count < 1) {
                var numratFiskalResult = await App.ApiClient.GetAsync("numri-fisk");
                var numratFiskalResponse = await numratFiskalResult.Content.ReadAsStringAsync();
                if (numratFiskalResult.IsSuccessStatusCode) {
                    var numratFiskal = JsonConvert.DeserializeObject<List<NumriFisk>>(numratFiskalResponse);
                    NumratFiskal = numratFiskal;
                    await App.Database.SaveNumratFiskalAsync(NumratFiskal);
                }
            }
            FiskalizimiKonfigurimet = await App.Database.GetFiskalizimiKonfigurimetAsync();
            if (FiskalizimiKonfigurimet.Count < 1) {
                var FiskalizimiKonfigurimetresult = await App.ApiClient.GetAsync("fiskalizimi-konfigurimet");
                if (FiskalizimiKonfigurimetresult.IsSuccessStatusCode) {
                    var FiskalizimiKonfigurimetResponse = await FiskalizimiKonfigurimetresult.Content.ReadAsStringAsync();
                    FiskalizimiKonfigurimet = JsonConvert.DeserializeObject<List<FiskalizimiKonfigurimet>>(FiskalizimiKonfigurimetResponse);
                    await App.Database.SaveFiskalizimiKonfigurimetAsync(FiskalizimiKonfigurimet);
                }
                else {
                    FiskalizimiKonfigurimet = new List<FiskalizimiKonfigurimet>
                    {
                        new FiskalizimiKonfigurimet
                        {
                        ID = 6,
                        TAGNR = "AA795IN",
                        TCRCode = "fa248ng165",
                        BusinessUnitCode = "oq060zj804"
                        }
                    };
                }
                
            }
            try {
                

                //TODO : FIX FISKALIZIMI KONFIGURIMET FROM API
                var query = from a in Agjendet
                            join d in Depot on a.Depo equals d.Depo
                            join fk in FiskalizimiKonfigurimet on d.TAGNR equals fk.TAGNR
                            join nf in NumratFiskal on fk.TCRCode equals nf.TCRCode into nfGroup
                            from nf in nfGroup.DefaultIfEmpty()
                            where a.IDAgjenti.ToLower() == Agjendi.IDAgjenti.ToLower()
                            select new
                            {
                                a.IDAgjenti,
                                d.TAGNR,
                                fk.TCRCode,
                                a.OperatorCode,
                                fk.BusinessUnitCode,
                                nf.IDN,
                                LevizjeIDN = nf != null ? nf.LevizjeIDN : (int?)null,
                                nf.Viti
                            };
                foreach (var quer in query) {
                    result[0] = quer.TAGNR;
                    result[1] = quer.TCRCode;
                    result[2] = quer.OperatorCode;
                    result[3] = quer.BusinessUnitCode;
                    result[4] = quer.IDN.ToString();
                    result[5] = quer.LevizjeIDN.ToString();
                    result[6] = quer.Viti.ToString();
                    App.Instance.MainViewModel.Configurimi.KodiINjesiseSeBiznesit = quer.BusinessUnitCode;
                    await App.Database.SaveConfigurimiAsync(App.Instance.MainViewModel.Configurimi);
                }
                return result;
            }
            catch (Exception ex) {
                //PnUtils.DbUtils.WriteExeptionErrorLog(ex);
            }
            return null;
        }

        public async Task<bool> SinkronizoFiskalizimin() {
            //SyncLloji = 1 -- update all
            //SyncLloji = 2 -- mos bej update numrinFisk
            string[] config = await GetConfigurationByIDAgjentiAsync();

            string TAGNR, TCRCode, OperatorCode, BusinessUnitCode, IDN, LevijeIDN, Viti;

            var result = "";

            if (config != null) {
                TAGNR = config[0];
                TCRCode = config[1];
                OperatorCode = config[2];
                BusinessUnitCode = config[3];
                IDN = config[4];
                LevijeIDN = config[5];
                Viti = config[6];

                DataSet dsConfig = null;
                try {
                    var numriFiskal = await App.Database.GetNumratFiskalAsync();
                    var numriFisk = numriFiskal.FirstOrDefault(x=>x.Depo == Agjendi.Depo);
                    if(numriFisk == null) {
                        var numratFiskalResult = await App.ApiClient.GetAsync("numri-fisk/" + Agjendi.Depo);
                        var numratFiskalResponse = await numratFiskalResult.Content.ReadAsStringAsync();
                        if(numratFiskalResult.IsSuccessStatusCode) {
                            var numratFiskal = JsonConvert.DeserializeObject<NumriFisk>(numratFiskalResponse);
                            numriFisk = numratFiskal;
                            var resultInsert = await App.Database.SaveNumriFiskalAsync(numratFiskal);
                        }
                    }
                    if (numriFisk == null) {
                        // Insert new record
                        var newNumriFisk = new NumriFisk
                        {
                            IDN = int.Parse(config[4]),
                            LevizjeIDN = int.Parse(config[5]),
                            Viti = int.Parse(config[6]),
                            Depo = Agjendi.IDAgjenti,
                            TCRCode = config[1]
                        };

                        var resultInsert = await App.Database.SaveNumriFiskalAsync(newNumriFisk);
                        if(resultInsert != -1) {
                            return true;
                        }
                        else {
                            UserDialogs.Instance.Alert("Shenimet nuk u shtuan me sukses");
                            return false;
                        }
                    }
                    else {
                        // Update existing record

                        var jsonRequest = JsonConvert.SerializeObject(numriFisk);
                        var stringContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                        var putResult = await App.ApiClient.PutAsync("numri-fisk", stringContent);
                        if (putResult.IsSuccessStatusCode) {
                            return true;

                        }
                        //else {
                        //    UserDialogs.Instance.Alert("Shenimet nuk u shtuan me sukses");
                        //    return false;

                        //}
                        return false;
                    }
                }
                catch (Exception ex) {

                    //MessageBox.Show("Gabim, Nuk mund te ruhen konfigurimet e fiskalizimit!");
                    UserDialogs.Instance.Alert("Gabim, Nuk mund te ruhen konfigurimet e fiskalizimit!");
                    return false;
                }
            }
            else {
                //MessageBox.Show("Lidhja me MobSales ka deshtura ose nuk jane regjistruar konfigurimet per fiskalizim ne MobSales");
                UserDialogs.Instance.Alert("Lidhja me MobSales ka deshtuar ose nuk jane regjistruar konfigurimet per fiskalizim ne MobSales");
                return false;
            }
        }

        public async Task<Nullable<double>> SumDataColumn(string TableName, string Column) {
            Nullable<double> result = null;

            try {
                //MALLIMBETUR
                if(TableName == "Malli_Mbetur" && Column == "SasiaShitur") {
                    var malliMbetur = await App.Database.GetMalliMbeturAsyncWithDepo(Agjendi.Depo);
                    var sumResult = malliMbetur.Where(x => x.SasiaShitur != null).Sum(item => (decimal)item.SasiaShitur);
                    float resDouble = 0f;

                    double yourValue = resDouble; // Replace this with your actual value

                    return Math.Round(double.Parse(sumResult.ToString()), 2);

                }
                //MALLIMBETUR
                if (TableName == "Malli_Mbetur" && Column == "SasiaKthyer") {
                    var malliMbetur = await App.Database.GetMalliMbeturAsyncWithDepo(Agjendi.Depo);
                    var sumResult = malliMbetur.Sum(item => (decimal)item.SasiaKthyer);
                    float resDouble = 0f;

                    double yourValue = resDouble; // Replace this with your actual value

                    return Math.Round(double.Parse(sumResult.ToString()), 2);

                }

            }
            catch (Exception e) {

            }
            return result;
        }

        public void SinkronizoCmimetAsync() {
        }

        public async Task SinkronizoKlientetAsync() {
            UserDialogs.Instance.ShowLoading("Duke sinkronizuar klientet...");
            try {
                Sync Snc = new Sync();

                StringBuilder strbSyncLog = new StringBuilder();//used for Log in TextBox Control
                string[] KeyFields = null;//Used in sync
                string strFilterDwn = "1=1";//Used in sync
                string strFilterUp = "1=1"; //Used in sync           
                string strTableName = "";//Used in sync            
                MobSellSyncDirection MobSyncDirection; //Used in Sync

                int _CurrNrFat = 0; //used for fixNrKufijt
                int _CurrNrFatJT = 0;//used for fixNrKufijt
                int _CurrNrFat_D = 0; //used for fixNrKufijt

                bool SyncSuccesful = true; //Used in sync
                bool SyncSendSuccessful = true; //Used in Sync
                int _stoqetServer = 0;
                strbSyncLog.Append("Filloj sinkronizimi i klienteve ..");
                strbSyncLog.AppendLine("Inicializimi...");

                if (!(App.Database.IsDBConnected() && App.Instance.DoIHaveInternet())) {
                    strbSyncLog.AppendLine("Sinkronizimi deshtoi shkak konektimi ne rrjet, ju lutemi shikoni komunikimin ne rrjet online");
                    EditorView = strbSyncLog.ToString();
                    UserDialogs.Instance.HideLoading();
                    return;
                }
                var IDAgjenti = Agjendi.IDAgjenti;
                var strDepo = Agjendi.Depo;

                var tableSyncConfig = await App.Database.GetSyncConfigurationsAsync();

                if (tableSyncConfig.Count < 1) {
                    strbSyncLog.AppendLine("\nNuk ka tabela per sinkronizim ne SyncConfig");
                    strbSyncLog.AppendLine("\nShtimi manual filloi");
                    EditorView = strbSyncLog.ToString();
                    UserDialogs.Instance.HideLoading();
                    return;
                }
                var listOfSyncConfigedTwoThreeFour = new List<SyncConfiguration>
                {
                    tableSyncConfig.FirstOrDefault(x => x.ID == 2),
                    tableSyncConfig.FirstOrDefault(x => x.ID == 3),
                    tableSyncConfig.FirstOrDefault(x => x.ID == 4),
                };

                EditorView = strbSyncLog.ToString();

                foreach (var sync in listOfSyncConfigedTwoThreeFour) {
                    MobSyncDirection = (MobSellSyncDirection)Convert.ToInt16(sync.SyncDirection);
                    strTableName = sync.TableName;
                    KeyFields = sync.PK_FieldName.Split(',');

                    strFilterUp = sync.FilterUp.ToString();
                    strFilterDwn = sync.FilterDown.ToString();

                    if (strFilterUp.Trim() == "")
                        strFilterUp = "1=1";

                    if (strFilterDwn.Trim() == "")
                        strFilterDwn = "1=1";

                    switch (strTableName) {

                        case "KlientDheLokacion": {
                                strFilterDwn = "(Depo='" + strDepo + "' OR Depo='' OR Depo IS NULL)";
                                strFilterUp = "(Depo='" + strDepo + "' OR Depo='' OR Depo IS NULL)";
                                break;
                            }
                        case "Klientet": {
                                strFilterDwn = "(Depo='" + strDepo + "' OR Depo='' OR Depo IS NULL)";
                                strFilterUp = "(Depo='" + strDepo + "' OR Depo='' OR Depo IS NULL)";
                                break;
                            }
                        case "Vizitat": {
                                string AddUPFilter = sync.FilterUp.ToString();
                                string AddDwnFilter = sync.FilterDown.ToString();

                                strFilterDwn = "DeviceID='" + Agjendi.DeviceID + "'";
                                strFilterUp = "DeviceID='" + Agjendi.DeviceID + "'";

                                if (AddDwnFilter.Trim() != "")
                                    strFilterDwn += " AND '" + AddDwnFilter + "'";
                                if (AddUPFilter.Trim() != "") //if Filter from SyncCOnfig not empty
                                    strFilterUp += " AND '" + AddUPFilter + "'";

                                break;
                            }
                    }

                    switch (MobSyncDirection) {

                        //case MobSellSyncDirection.SnapshotSpecial: {
                        //        SyncSuccesful = Snc.SyncTable(strTableName, KeyFields, PnSyncDirection.SnapshotSpecial, strFilterDwn, "SyncStatus", strDepo);

                        //        break;
                        //    }

                        case MobSellSyncDirection.Snapshot: {
                                strbSyncLog.AppendLine("\n Filloi synci " + strTableName + " Snapshot Download ");
                                SyncSuccesful = await Snc.SyncTable(strTableName, null, PnSyncDirection.SnapshotDownload, strFilterUp, "", Agjendi.Depo);
                                strbSyncLog.AppendLine("\n Synci " + strTableName + " mbaroi : " + SyncSuccesful);
                                EditorView = strbSyncLog.ToString();
                                break;
                            }

                        //case MobSellSyncDirection.Download:  //download with insert and Update
                        //    {
                        //        SyncSuccesful = Snc.SyncTable(strTableName, KeyFields, PnSyncDirection.Download, strFilterDwn, "SyncStatus", "");
                        //        break;
                        //    }
                        //case MobSellSyncDirection.Upload: //upload with insert and update
                        //    {
                        //        string StatusField = "SyncStatus";

                        //        KeyFields = null;

                        //        SyncSuccesful = Snc.SyncTable(strTableName, KeyFields, PnSyncDirection.Upload, strFilterUp, StatusField, "");

                        //        break;
                        //    }
                        case MobSellSyncDirection.Bidirectional: //Download and Upload with Update and insert (bidirectional)
                        {
                                strbSyncLog.AppendLine("\n Filloi synci " + strTableName + " Upload");
                                //***Upload***
                                    SyncSuccesful = await Snc.SyncTable(strTableName, KeyFields, PnSyncDirection.Upload, strFilterUp, "SyncStatus", Agjendi.DeviceID);

                                strbSyncLog.AppendLine("\n Synci " + strTableName + " mbaroi : " + SyncSuccesful);
                                EditorView = strbSyncLog.ToString();

                                strbSyncLog.AppendLine("\n Filloi synci " + strTableName + " Download");
                                
                                //****Now download****
                                    SyncSuccesful = await Snc.SyncTable(strTableName, KeyFields, PnSyncDirection.Download, strFilterDwn, "SyncStatus", Agjendi.DeviceID);

                                strbSyncLog.AppendLine("\n Synci " + strTableName + " mbaroi : " + SyncSuccesful);
                                EditorView = strbSyncLog.ToString();

                                break;
                            }

                            //case MobSellSyncDirection.BidirectionalSpecial: //this Downloads to Device (insert,update) to spec. table, Uploads to tableName+"_upl" (insert,update)
                            //    {
                            //        //***Upload***SEND DATA TO SPECIFIED TABLENAME+"_upl" WITH INSERT AND UPDATE
                            //        SyncSendSuccessful = Snc.SendData(strTableName, strTableName + "_upl", KeyFields, strFilterUp, "SyncStatus");
                            //        //****Now download**** NORMAL DOWNLOAD WITH INSERT AND UPDATE
                            //        SyncSuccesful = Snc.SyncTable(strTableName, KeyFields, PnSyncDirection.Download, strFilterDwn, "SyncStatus", "");
                            //        break;
                            //    }
                    } //END SWRITCH MobSellDirection

                    FixKufijtFaturave(SyncSuccesful,strTableName);
                }
                UserDialogs.Instance.HideLoading();

            }
            catch (Exception e) {
                UserDialogs.Instance.HideLoading();
            }
        }

        public int _CurrNrFat;
        public int _CurrNrFatJT;
        public int _CurrNrFat_D;

        private async void FixKufijtFaturave(bool SyncSuccess, string strTable) {

            if (strTable == "NumriFaturave") {
                NumriFaturave nf = await App.Database.GetNumriFaturaveIDAsync(Agjendi.IDAgjenti);
                int tempNRKUFIP = (int)nf.NRKUFIP;

                int tempNRKUFIS = (int)nf.NRKUFIS;

                int tempNRKUFIPJT = (int)nf.NRKUFIPJT;

                int tempNRKUFISJT = int.Parse(nf.NRKUFISJT.ToString());

                int tempNRKUFIP_D = int.Parse(nf.NRKUFIP_D.ToString());

                int tempNRKUFIS_D = int.Parse(nf.NRKUFIS_D.ToString());

                if (_CurrNrFat < tempNRKUFIP || _CurrNrFat > tempNRKUFIS)
                    _CurrNrFat = 0;
                else
                    _CurrNrFat = _CurrNrFat - tempNRKUFIP;

                if (_CurrNrFatJT < tempNRKUFIPJT || _CurrNrFatJT > tempNRKUFISJT)
                    _CurrNrFatJT = 0;
                else
                    _CurrNrFatJT = _CurrNrFatJT - tempNRKUFIPJT;

                if (_CurrNrFat_D < tempNRKUFIP_D || _CurrNrFat_D > tempNRKUFIS_D)
                    _CurrNrFat_D = 0;
                else
                    _CurrNrFat_D = _CurrNrFat_D - tempNRKUFIP_D;

                nf.CurrNrFat = _CurrNrFat;
                nf.CurrNrFatJT = _CurrNrFatJT;
                nf.CurrNrFat_D = _CurrNrFat_D;
                await App.Database.UpdateNumriFaturave(nf);
            }
        }

        private async void SaveValuesKufijtFaturave(string strTableName) {
            if (strTableName == "NumriFaturave") {
                var nfl = await App.Database.GetNumriFaturaveAsync();
                var nf = nfl.FirstOrDefault(x => x.KOD == Agjendi.Depo);
                if ( nf != null) {
                    int tempNRKUFIP = int.Parse(nf.NRKUFIP.ToString());
                    int tempNRKUFIPJT = int.Parse(nf.NRKUFIPJT.ToString());
                    int tempNRKUFIP_D = int.Parse(nf.NRKUFIP_D.ToString());
                    this._CurrNrFat = int.Parse(nf.CurrNrFat.ToString());
                    this._CurrNrFat_D = int.Parse(nf.CurrNrFat_D.ToString());
                    this._CurrNrFatJT = int.Parse(nf.CurrNrFatJT.ToString());
                    this._CurrNrFat = _CurrNrFat + tempNRKUFIP;
                    this._CurrNrFatJT = _CurrNrFatJT + tempNRKUFIPJT;
                    this._CurrNrFat_D = _CurrNrFat_D + tempNRKUFIP_D;
                }
                else {
                    this._CurrNrFat = 0;
                    this._CurrNrFatJT = 0;
                }
            }
        }

    }
    public class Sync {
        private static string SqlServerDbName = "";
        private static string UserID = "";
        private static string Password = "";
        private static string ServerName = "";
        private static string dbPath = "";
        private static string dbPassword = "";
        private static string MobileServerUrl = "";

        public static StringBuilder SdfExecuteCommand = new StringBuilder("");

        internal struct SyncLog {
            public static string TableName = "";
            public static string StartTime = "";
            public static string EndTime = "";
            public static string Updated = "0";
            public static string Inserted = "0";
            public static string Direction = "0";
            public static string DownloadedRecCount = "0";
            public static string UploadedRecCount = "0";

            public static string ErrorMsg = "";
            public static string ErrorCode = "";
            public static string ErrorModule = "";
            public static string ErrorLine = "";

        }

        public Sync(string PdaDbPath, string PdaDbPassword, string serverName, string mobileServerUrl, string sqlServerDbName, string userID, string password) {
            UserID = userID;
            Password = password;
            ServerName = serverName;
            dbPath = PdaDbPath;
            MobileServerUrl = mobileServerUrl;
            SqlServerDbName = sqlServerDbName;
            dbPassword = PdaDbPassword;
        }

        public Sync() { }

        private static async Task<bool> TableExists(string tableObject) {
            if(tableObject != null) {
                return await App.Database.TableExists(tableObject);
            }
            return false;
        }

        private static async void DropTable(string tableName) {
            await App.Database.DropTable(tableName);
        }

        //function that clears all data in a specified table
        private static async void ClearAllData(string tableName) {
            //await App.Database.ClearTable(tableName);
        }

        //this functino updates status field of a given table, with a given value,
        private static async void UpdateStatusField(string tableName, string statusField, string CurrentValue, string updateValue) {
            await App.Database.UpdateStatusField(tableName, statusField, CurrentValue, updateValue);
        }

        public async Task<bool> SyncTable(string tableName, string[] KeyFields, PnSyncDirection Direction, string SqlCondition, string StatusField, string Depo)
        {
            SyncLog.StartTime = DateTime.Now.ToString("hh:mm:ss");
            if(tableName == "LiferimiArt") {

            }
            if(tableName == "NumriFaturave") {

            }
            bool result = false;
                try {
                    SyncLog.TableName = tableName;
                    //prepare to get data in a table called "temp", table will be created during process
                    //*********************************DOWNLOAD****************************************************************
                    switch (Direction) {
                        #region Download
                    //Klientet IDKlienti kushti : depo = depo
                        case PnSyncDirection.Download: {

                                //testing  ClearAllData(tableName, conn);

                                //we need to drop table, cause new one will be created

                                if(tableName == "Vizitat") {
                                    if (SqlCondition != null) {
                                    if (SqlCondition.Contains("DeviceID=")) {
                                        var vizitatWithDeviceIdResponse = await App.ApiClient.GetAsync("vizitat?deviceid=" + Depo + "&status=0");
                                        var vizitatWithDeviceIdResult = await vizitatWithDeviceIdResponse.Content.ReadAsStringAsync();
                                        if (vizitatWithDeviceIdResponse.IsSuccessStatusCode) {
                                            var listOfClientsWithDepo = JsonConvert.DeserializeObject<List<Vizita>>(vizitatWithDeviceIdResult);
                                            var currentVizitat = await App.Database.GetVizitatAsync();
                                            currentVizitat = await App.Database.GetVizitatAsync();
                                            foreach(var viz in listOfClientsWithDepo) {
                                                viz.IDStatusiVizites = "0";
                                            }
                                            var saved = await App.Database.SaveVizitatAsync(listOfClientsWithDepo);

                                            if (saved != -1) {
                                                return true;
                                            }
                                            else {
                                                UserDialogs.Instance.HideLoading();
                                                UserDialogs.Instance.Alert("Error ne ruajtjen e vizitave ne DB Lokale ");
                                                return false;
                                            }
                                        }
                                        else {
                                            UserDialogs.Instance.HideLoading();
                                            UserDialogs.Instance.Alert("Error ne marrjen e vizitave nga serveri : SYNC KLIENTET");
                                            return false;
                                        }
                                    }
                                    else {
                                        var vizitatWithDeviceIdResponse = await App.ApiClient.GetAsync("vizitat/all");
                                        var vizitatWithDeviceIdResult = await vizitatWithDeviceIdResponse.Content.ReadAsStringAsync();
                                        if (vizitatWithDeviceIdResponse.IsSuccessStatusCode) {
                                            var listOfClientsWithDepo = JsonConvert.DeserializeObject<List<Vizita>>(vizitatWithDeviceIdResult);
                                            var currentVizitat = await App.Database.GetVizitatAsync();
                                            currentVizitat = await App.Database.GetVizitatAsync();
                                            await App.Database.SaveVizitatAsync(listOfClientsWithDepo);
                                            currentVizitat = await App.Database.GetVizitatAsync();
                                            foreach (var vizita in currentVizitat) {
                                                vizita.SyncStatus = 1;
                                            }
                                            var jsonRequest = JsonConvert.SerializeObject(currentVizitat);
                                            var stringContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                                            var vizitatPostResponse = await App.ApiClient.PutAsync("vizitat", stringContent);
                                            if (vizitatPostResponse.IsSuccessStatusCode) {
                                                return true;
                                            }
                                            return false;
                                        }
                                        else {
                                            UserDialogs.Instance.HideLoading();
                                            UserDialogs.Instance.Alert("Error ne marrjen e vizitave nga serveri : SYNC KLIENTET");
                                            return false;
                                        }
                                    }
                                }
                                }
                                if (tableName == "EvidencaPagesave") {
                                if (SqlCondition != null) {
                                    if (SqlCondition.Contains("DeviceID=")) {
                                        if (string.IsNullOrEmpty(Depo)) {
                                            Depo = SqlCondition.Split('\'')[1];
                                        }
                                        var EvidencaPagesaveIdResponse = await App.ApiClient.GetAsync("pagesa");
                                        var EvidencaPagesaveIdResult = await EvidencaPagesaveIdResponse.Content.ReadAsStringAsync();
                                        if (EvidencaPagesaveIdResponse.IsSuccessStatusCode) {
                                            var listOfClientsWithDepo = JsonConvert.DeserializeObject<List<EvidencaPagesave>>(EvidencaPagesaveIdResult);
                                            var currentEvidencaPagesave = await App.Database.GetEvidencaPagesaveAsync();
                                            await App.Database.ClearAllEvidencaPagesave();
                                            currentEvidencaPagesave = await App.Database.GetEvidencaPagesaveAsync();
                                        }
                                        else {
                                            UserDialogs.Instance.HideLoading();
                                            UserDialogs.Instance.Alert("Error ne marrjen e evidencalokale nga serveri : SYNC DOWNLOAD");
                                            return false;
                                        }
                                    }
                                    else {
                                        var currentEvidencaPagesave = await App.Database.GetEvidencaPagesaveAsync();
                                        foreach(var ev in currentEvidencaPagesave) {
                                            ev.SyncStatus = 1;
                                        }
                                        var jsonRequest = JsonConvert.SerializeObject(currentEvidencaPagesave);
                                        var stringContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                                        var vizitatPostResponse = await App.ApiClient.PostAsync("pagesa", stringContent);
                                        if (vizitatPostResponse.IsSuccessStatusCode) {
                                            var EvidencaPagesaveIdResponse = await App.ApiClient.GetAsync("pagesa");
                                            var EvidencaPagesaveIdResult = await EvidencaPagesaveIdResponse.Content.ReadAsStringAsync();
                                            if (EvidencaPagesaveIdResponse.IsSuccessStatusCode) {
                                                var listOfClientsWithDepo = JsonConvert.DeserializeObject<List<EvidencaPagesave>>(EvidencaPagesaveIdResult);
                                                currentEvidencaPagesave = await App.Database.GetEvidencaPagesaveAsync();
                                                await App.Database.ClearAllEvidencaPagesave();
                                                currentEvidencaPagesave = await App.Database.GetEvidencaPagesaveAsync();
                                            }
                                        }
                                        return false;
                                        
                                    }
                                }
                            }
                            if (tableName == "CashRegister")
                                return true;
                            break;
                            }
                        #endregion

                        #region Upload
                        //*********************************UPLOAD****************************************************************
                        case PnSyncDirection.Upload: {
                            //if given parametere StatusField=0 means no statusField, CreateInserScript uses ...where StatusField=0, so 0=0 (all)
                            if (tableName == "Vizitat") {
                                var vizitatEUpdatuara = await CreateInsertScriptVizitat(tableName, KeyFields, SqlCondition, StatusField);
                                if (vizitatEUpdatuara != null && vizitatEUpdatuara.Count >= 1) {
                                    result = await CreateUpdateScriptVizitat(vizitatEUpdatuara);
                                    break;
                                }
                                result = true;
                                break;
                            }
                            if (tableName == "Liferimi") {
                                var LiferimiEUpdatuara = await CreateInsertScriptLiferimi(tableName, KeyFields, SqlCondition, StatusField);
                                if (LiferimiEUpdatuara != null && LiferimiEUpdatuara.Count >= 1) {
                                    result = await CreateUpdateScriptLiferimi(LiferimiEUpdatuara);
                                    break;
                                }
                                result = true;
                                break;
                            }
                            if (tableName == "LiferimiArt") {
                                var LiferimiArtEUpdatuara = await CreateInsertScriptLiferimiArt(tableName, KeyFields, SqlCondition, StatusField);
                                if (LiferimiArtEUpdatuara != null && LiferimiArtEUpdatuara.Count >= 1) {
                                    result = await CreateUpdateScriptLiferimiArt(LiferimiArtEUpdatuara);
                                    break;
                                }
                                result = true;
                                break;
                            }
                            if (tableName == "Porosite") {
                                var PorositeEUpdatuara = await CreateInsertScriptPorosite(tableName, KeyFields, SqlCondition, StatusField);
                                if (PorositeEUpdatuara != null && PorositeEUpdatuara.Count >= 1) {
                                    result = await CreateUpdateScriptPorosite(PorositeEUpdatuara);
                                    break;
                                }
                                result = true;
                                break;
                            }                            
                            if (tableName == "PorosiaArt") {
                                var PorosiaArtEUpdatuara = await CreateInsertScriptPorositeArt(tableName, KeyFields, SqlCondition, StatusField);
                                if (PorosiaArtEUpdatuara != null && PorosiaArtEUpdatuara.Count >= 1) {
                                    result = await CreateUpdateScriptPorosiaArt(PorosiaArtEUpdatuara);
                                    break;
                                }
                                result = true;
                                break;
                            }                            
                            if (tableName == "EvidencaPagesave") {
                                var PorosiaArtEUpdatuara = await CreateInsertScriptEvidencaPagesave(tableName, KeyFields, SqlCondition, StatusField);
                                if (PorosiaArtEUpdatuara != null && PorosiaArtEUpdatuara.Count >= 1) {
                                    result = await CreateUpdateScriptEvidencaPagesave(PorosiaArtEUpdatuara);
                                    break;
                                }
                                result = true;
                                break;
                            }
                            if (tableName == "Orders") {
                                var PorosiaArtEUpdatuara = await CreateInsertScriptOrders(tableName, KeyFields, SqlCondition, StatusField);
                                if (PorosiaArtEUpdatuara != null && PorosiaArtEUpdatuara.Count >= 1) {
                                    result = await CreateUpdateScriptOrders(PorosiaArtEUpdatuara);
                                    break;
                                }
                                result = true;
                                break;
                            }
                            if (tableName == "Order_Details") {
                                var PorosiaArtEUpdatuara = await CreateInsertScriptOrderDetails(tableName, KeyFields, SqlCondition, StatusField);
                                if (PorosiaArtEUpdatuara != null && PorosiaArtEUpdatuara.Count >= 1) {
                                    result = await CreateUpdateScriptOrderDetails(PorosiaArtEUpdatuara);
                                    break;
                                }
                                result = true;
                                break;
                            }                            
                            if (tableName == "LEVIZJET_HEADER") {
                                var LEVIZJET_HEADEREUpdatuara = await CreateInsertScriptLevizjetHeader(tableName, KeyFields, SqlCondition, StatusField);
                                if (LEVIZJET_HEADEREUpdatuara != null && LEVIZJET_HEADEREUpdatuara.Count >= 1) {
                                    result = await CreateUpdateScriptLevizjetHeader(LEVIZJET_HEADEREUpdatuara);
                                    break;
                                }
                                result = true;
                                break;
                            }                              
                            if (tableName == "LEVIZJET_DETAILS") {
                                var LEVIZJET_HEADEREUpdatuara = await CreateInsertScriptLevizjetHeaderDetails(tableName, KeyFields, SqlCondition, StatusField);
                                if (LEVIZJET_HEADEREUpdatuara != null && LEVIZJET_HEADEREUpdatuara.Count >= 1) {
                                    result = await CreateUpdateScriptLevizjetHeaderDetails(LEVIZJET_HEADEREUpdatuara);
                                    break;
                                }
                                result = true;
                                break;
                            }                            
                            if (tableName == "Krijimi_Porosive") {
                                var LEVIZJET_HEADEREUpdatuara = await CreateInsertScriptKrijimiPorosive(tableName, KeyFields, SqlCondition, StatusField);
                                if (LEVIZJET_HEADEREUpdatuara != null && LEVIZJET_HEADEREUpdatuara.Count >= 1) {
                                    result = await CreateUpdateScriptKrijimiPorosive(LEVIZJET_HEADEREUpdatuara);
                                    break;
                                }
                                result = false;
                                break;
                            }                            
                            if (tableName == "Malli_Mbetur") {
                                var Malli_MbeturUpdatuara = await CreateInsertScriptMalli_MbeturDetails(tableName, KeyFields, SqlCondition, StatusField);
                                if (Malli_MbeturUpdatuara != null && Malli_MbeturUpdatuara.Count >= 1) {
                                    result = await CreateUpdateScriptMalli_Mbetur(Malli_MbeturUpdatuara);
                                    break;
                                }
                                result = false;
                                break;
                            }                            
                            if (tableName == "CashRegister") {
                                var CashRegisterUpdatuara = await CreateInsertScriptCashRegister(tableName, KeyFields, SqlCondition, StatusField);
                                if (CashRegisterUpdatuara != null && CashRegisterUpdatuara.Count >= 1) {
                                    result = await CreateUpdateScriptCashRegister(CashRegisterUpdatuara);
                                    break;
                                }
                                result = false;
                                break;
                            }
                            break;
                        }
                    #endregion

                    #region SnapshotDownload
                    //*********************************SNAPSHOT DOWNLOAD*****************************************
                    case PnSyncDirection.SnapshotDownload: {
                            // KLIENTET
                            if (tableName == "Klientet") {
                                if (SqlCondition != null) {
                                    if (SqlCondition.Contains("Depo")) {
                                        var ClientWithDepoResponse = await App.ApiClient.GetAsync("clients?depo=" + Depo);
                                        var ClientWithDepoResult = await ClientWithDepoResponse.Content.ReadAsStringAsync();
                                        if (ClientWithDepoResponse.IsSuccessStatusCode) {
                                            var listOfClientsWithDepo = JsonConvert.DeserializeObject<List<Klientet>>(ClientWithDepoResult);
                                            await App.Database.ClearAllKlientet();
                                            var currentClients = await App.Database.GetKlientetAsync();
                                            await App.Database.SaveKlienteteAsync(listOfClientsWithDepo);
                                            currentClients = await App.Database.GetKlientetAsync();
                                            return true;
                                        }
                                        else {
                                            UserDialogs.Instance.HideLoading();
                                            UserDialogs.Instance.Alert("Error ne marrjen e klienteve nga serveri : SYNC KLIENTET");
                                            return false;
                                        }
                                    }
                                    else {
                                        var ClientWithDepoResponse = await App.ApiClient.GetAsync("clients/all");
                                        var ClientWithDepoResult = await ClientWithDepoResponse.Content.ReadAsStringAsync();
                                        if (ClientWithDepoResponse.IsSuccessStatusCode) {
                                            var listOfClientsWithDepo = JsonConvert.DeserializeObject<List<Klientet>>(ClientWithDepoResult);
                                            await App.Database.ClearAllKlientet();
                                            var currentClients = await App.Database.GetKlientetAsync();
                                            await App.Database.SaveKlienteteAsync(listOfClientsWithDepo);
                                            currentClients = await App.Database.GetKlientetAsync();
                                            return true;
                                        }
                                        else {
                                            UserDialogs.Instance.HideLoading();
                                            UserDialogs.Instance.Alert("Error ne marrjen e klienteve nga serveri : SYNC KLIENTET");
                                            return false;
                                        }
                                    }
                                }
                            }

                            // KLIENTDHELOKACION
                            if(tableName == "KlientDheLokacion") {
                                if (SqlCondition != null) {
                                    if (SqlCondition.Contains("Depo")) {
                                        var ClientWithDepoResponse = await App.ApiClient.GetAsync("location?depo=" + Depo);
                                        var ClientWithDepoResult = await ClientWithDepoResponse.Content.ReadAsStringAsync();
                                        if (ClientWithDepoResponse.IsSuccessStatusCode) {
                                            var listOfClientsWithDepo = JsonConvert.DeserializeObject<List<KlientDheLokacion>>(ClientWithDepoResult);
                                            await App.Database.ClearAllKlientetDheLokacion();
                                            var currentClients = await App.Database.GetKlientetDheLokacionetAsync();
                                            await App.Database.SaveKlientetDheLokacionet(listOfClientsWithDepo);
                                            currentClients = await App.Database.GetKlientetDheLokacionetAsync();
                                            return true;
                                        }
                                        else {
                                            UserDialogs.Instance.HideLoading();
                                            UserDialogs.Instance.Alert("Error ne marrjen e klienteve dhe lokacioneve nga serveri : SYNC KLIENTET");
                                            return false;
                                        }
                                    }
                                    else {
                                        var ClientWithDepoResponse = await App.ApiClient.GetAsync("location/all");
                                        var ClientWithDepoResult = await ClientWithDepoResponse.Content.ReadAsStringAsync();
                                        if (ClientWithDepoResponse.IsSuccessStatusCode) {
                                            var listOfClientsWithDepo = JsonConvert.DeserializeObject<List<KlientDheLokacion>>(ClientWithDepoResult);
                                            await App.Database.ClearAllKlientetDheLokacion();
                                            var currentClients = await App.Database.GetKlientetDheLokacionetAsync();
                                            await App.Database.SaveKlientetDheLokacionet(listOfClientsWithDepo);
                                            currentClients = await App.Database.GetKlientetDheLokacionetAsync();
                                            return true;
                                        }
                                        else {
                                            UserDialogs.Instance.HideLoading();
                                            UserDialogs.Instance.Alert("Error ne marrjen e klienteve dhe lokacioneve nga serveri : SYNC KLIENTET");
                                            return false;
                                        }
                                    }
                                }
                            }

                            //VIZITAT
                            if(tableName == "Vizitat") {
                                if (SqlCondition != null) {
                                    if (SqlCondition.Contains("Depo")) {
                                        var ClientWithDepoResponse = await App.ApiClient.GetAsync("vizitat?deviceid=" + Depo);
                                        var ClientWithDepoResult = await ClientWithDepoResponse.Content.ReadAsStringAsync();
                                        if (ClientWithDepoResponse.IsSuccessStatusCode) {
                                            var listOfClientsWithDepo = JsonConvert.DeserializeObject<List<Vizita>>(ClientWithDepoResult);
                                            var currentClients = await App.Database.GetVizitatAsync();
                                            await App.Database.SaveVizitatAsync(listOfClientsWithDepo);
                                            currentClients = await App.Database.GetVizitatAsync();
                                            return true;
                                        }
                                        else {
                                            UserDialogs.Instance.HideLoading();
                                            UserDialogs.Instance.Alert("Error ne marrjen e vizitave nga serveri : SYNC KLIENTET");
                                            return false;
                                        }
                                    }
                                    else {
                                        var ClientWithDepoResponse = await App.ApiClient.GetAsync("vizitat/all");
                                        var ClientWithDepoResult = await ClientWithDepoResponse.Content.ReadAsStringAsync();
                                        if (ClientWithDepoResponse.IsSuccessStatusCode) {
                                            var listOfClientsWithDepo = JsonConvert.DeserializeObject<List<KlientDheLokacion>>(ClientWithDepoResult);
                                            await App.Database.ClearAllKlientetDheLokacion();
                                            var currentClients = await App.Database.GetKlientetDheLokacionetAsync();
                                            await App.Database.SaveKlientetDheLokacionet(listOfClientsWithDepo);
                                            currentClients = await App.Database.GetKlientetDheLokacionetAsync();
                                            return true;
                                        }
                                        else {
                                            UserDialogs.Instance.HideLoading();
                                            UserDialogs.Instance.Alert("Error ne marrjen e vizitave nga serveri : SYNC KLIENTET");
                                            return false;
                                        }
                                    }
                                }
                            }

                            //SyncConfiguration
                            if(tableName == "SyncConfiguration") {
                                var SyncConfigurationResponse = await App.ApiClient.GetAsync("sync-configuration");
                                var SyncConfigurationResult = await SyncConfigurationResponse.Content.ReadAsStringAsync();
                                if (SyncConfigurationResponse.IsSuccessStatusCode) {
                                    var listOfSyncConfiguration = JsonConvert.DeserializeObject<List<SyncConfiguration>>(SyncConfigurationResult);
                                    await App.Database.ClearAllSyncConfigs();
                                    var currentClients = await App.Database.GetSyncConfigurationsAsync();
                                    await App.Database.SaveSyncConfigs(listOfSyncConfiguration);
                                    currentClients = await App.Database.GetSyncConfigurationsAsync();
                                    return true;
                                }
                                else {
                                    UserDialogs.Instance.HideLoading();
                                    UserDialogs.Instance.Alert("Error ne SYNCCONFIGURATION : SyncTable");
                                    return false;
                                }
                            }
                            //Konfigurimi"Konfigurimi"
                            if (tableName == "Konfigurimi") {
                                var KonfigurimiResponse = await App.ApiClient.GetAsync("konfigurimet/"+Depo);
                                var KonfigurimiResult = await KonfigurimiResponse.Content.ReadAsStringAsync();
                                if (KonfigurimiResponse.IsSuccessStatusCode) {
                                    var listOfConfiguration = JsonConvert.DeserializeObject<Konfigurimi>(KonfigurimiResult);
                                    await App.Database.ClearAllKonfigurimet();
                                    var currentClients = await App.Database.GetKonfigurimetAsync();
                                    await App.Database.SaveKonfigurimiAsync(listOfConfiguration);
                                    currentClients = await App.Database.GetKonfigurimetAsync();
                                    return true;
                                }
                                else {
                                    UserDialogs.Instance.HideLoading();
                                    UserDialogs.Instance.Alert("Error ne KONFIGURIMET : SyncTable");
                                    return false;
                                }
                            }
                            //ARTIKUJT
                            if(tableName == "Artikujt") {
                                var artikujtResponse = await App.ApiClient.GetAsync("artikujt/all");
                                var artikujtResult = await artikujtResponse.Content.ReadAsStringAsync();
                                if (artikujtResponse.IsSuccessStatusCode) {
                                    var listOfArtikuj = JsonConvert.DeserializeObject<List<Artikulli>>(artikujtResult);
                                    await App.Database.ClearAllArtikujtAsync();
                                    var currentArtikujt = await App.Database.GetArtikujtAsync();
                                    await App.Database.SaveArtikujtAsync(listOfArtikuj);
                                     currentArtikujt = await App.Database.GetArtikujtAsync();
                                    return true;
                                }
                                else {
                                    UserDialogs.Instance.HideLoading();
                                    UserDialogs.Instance.Alert("Error ne Artikuj : SyncTable");
                                    return false;
                                }
                            }
                            //VENDET
                            if (tableName == "Vendet") {
                                var VendetResponse = await App.ApiClient.GetAsync("vendet");
                                var VendetResult = await VendetResponse.Content.ReadAsStringAsync();
                                if (VendetResponse.IsSuccessStatusCode) {
                                    var listOfVendet = JsonConvert.DeserializeObject<List<Vendet>>(VendetResult);
                                    await App.Database.ClearAllVendetAsync();
                                    var currentVendet = await App.Database.GetVendetAsync();
                                    await App.Database.SaveVendetAsync(listOfVendet);
                                    currentVendet = await App.Database.GetVendetAsync();
                                    return true;
                                }
                                else {
                                    UserDialogs.Instance.HideLoading();
                                    UserDialogs.Instance.Alert("Error ne Vendet : SyncTable");
                                    return false;
                                }
                            }
                            //AGJENDET
                            if (tableName == "Agjendet") {
                                var AgjendetResponse = await App.ApiClient.GetAsync("agjendi");
                                var AgjendetResult = await AgjendetResponse.Content.ReadAsStringAsync();
                                if (AgjendetResponse.IsSuccessStatusCode) {
                                    var listOfAgjendi = JsonConvert.DeserializeObject<List<Agjendet>>(AgjendetResult);
                                    await App.Database.ClearAllAgjendetAsync();
                                    var currentVendet = await App.Database.GetAgjendetAsync();
                                    await App.Database.SaveAgjendetAsync(listOfAgjendi);
                                    currentVendet = await App.Database.GetAgjendetAsync();
                                    return true;
                                }
                                else {
                                    UserDialogs.Instance.HideLoading();
                                    UserDialogs.Instance.Alert("Error ne Agjendet : SyncTable");
                                    return false;
                                }
                            }
                            //STATUSIVIZITES
                            if (tableName == "StatusiVizites") {
                                var StatusiVizitesResponse = await App.ApiClient.GetAsync("vizitat/statuset");
                                var StatusiVizitesResult = await StatusiVizitesResponse.Content.ReadAsStringAsync();
                                if (StatusiVizitesResponse.IsSuccessStatusCode) {
                                    var listOfStatusiVizites = JsonConvert.DeserializeObject<List<StatusiVizites>>(StatusiVizitesResult);
                                    await App.Database.ClearAllStatusiVizitesAsync();
                                    var currentStatusiVizites = await App.Database.GetStatusiVizitesAsync();
                                    await App.Database.SaveAlllistOfStatusiVizitesAsync(listOfStatusiVizites);
                                    currentStatusiVizites = await App.Database.GetStatusiVizitesAsync();
                                    return true;
                                }
                                else {
                                    UserDialogs.Instance.HideLoading();
                                    UserDialogs.Instance.Alert("Error ne StatusiVizites : SyncTable");
                                    return false;
                                }
                            }
                            //Arsyejet
                            if (tableName == "Arsyejet") {
                                await App.Database.ClearAllArsyejetAsync();
                                var arsyejetResponse = await App.ApiClient.GetAsync("arsyejet");
                                if(arsyejetResponse.IsSuccessStatusCode) {
                                    var arsyejetres = await arsyejetResponse.Content.ReadAsStringAsync();
                                    var arsyejet = JsonConvert.DeserializeObject<List<Arsyejet>>(arsyejetres);
                                    var saveRes = await App.Database.SaveArsyejetAsync(arsyejet);
                                    return true;
                                }
                                return false;
                            }

                            //SALESPRICE
                            if(tableName == "SalesPrice") {
                                var clear = await App.Database.ClearAllSalesPrice();

                                var klientetDheLokacionet = await App.Database.GetKlientetDheLokacionetAsync();
                                var salesPrices = new List<SalesPrice>();
                                foreach (var klient in klientetDheLokacionet) {
                                    var salesPricesResult = await App.ApiClient.GetAsync("prices/" + klient.IDKlientDheLokacion);
                                    if (salesPricesResult.IsSuccessStatusCode) {
                                        var salesPricesResponse = await salesPricesResult.Content.ReadAsStringAsync();
                                        var SalePrice = JsonConvert.DeserializeObject<List<SalesPrice>>(salesPricesResponse);
                                        salesPrices.AddRange(SalePrice);
                                    }
                                    else
                                        return false;
                                }
                                var standardSalesPriceResult = await App.ApiClient.GetAsync("prices/STANDARD");
                                if(standardSalesPriceResult.IsSuccessStatusCode) {
                                    var standardSalesPriceResponse = await standardSalesPriceResult.Content.ReadAsStringAsync();
                                    var salePricesStandard = JsonConvert.DeserializeObject<List<SalesPrice>>(standardSalesPriceResponse);
                                    salesPrices.AddRange(salePricesStandard);
                                }

                                var resulti = await App.Database.SaveSalesPricesAsync(salesPrices);
                                if (resulti > 0) {
                                    result = true;
                                }
                                //TODO UNCOMMENT WHEN SYNC ALL IS FINISHED IMPLEMENTED BECAUSE TAKES A LOT OF TIME TO FINISH
                                return true;
                            }
                            //NumriFaturave
                            if (tableName == "NumriFaturave") {
                                var numratEFaturave = await App.Database.GetNumriFaturaveAsync();
                                var numri = numratEFaturave.FirstOrDefault(x => x.KOD == Depo);
                                if(numri != null) {
                                    var numriIfATURES = new List<NumriFaturave> { numri };
                                    var jsonRequest = JsonConvert.SerializeObject(numriIfATURES);
                                    var stringContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                                    var NumriFaturavePutResponse = await App.ApiClient.PutAsync("fatura", stringContent);
                                    if (NumriFaturavePutResponse.IsSuccessStatusCode) {
                                        var NumriFaturaveResponse = await App.ApiClient.GetAsync("fatura");
                                        var NumriFaturaveResult = await NumriFaturaveResponse.Content.ReadAsStringAsync();
                                        if (NumriFaturaveResponse.IsSuccessStatusCode) {
                                            var listOfNumriFaturave = JsonConvert.DeserializeObject<List<NumriFaturave>>(NumriFaturaveResult);
                                            await App.Database.ClearAllNumriFaturaveAsync();
                                            var currentNumriFaturave = await App.Database.GetNumriFaturaveAsync();
                                            await App.Database.SaveNumriFaturaveAllAsync(listOfNumriFaturave);
                                            currentNumriFaturave = await App.Database.GetNumriFaturaveAsync();
                                            return true;
                                        }
                                        else {
                                            UserDialogs.Instance.HideLoading();
                                            UserDialogs.Instance.Alert("Error ne NumriFaturave : SyncTable");
                                            return false;
                                        }
                                    }
                                }
                                else {
                                    var NumriFaturaveResponse = await App.ApiClient.GetAsync("fatura");
                                    var NumriFaturaveResult = await NumriFaturaveResponse.Content.ReadAsStringAsync();
                                    if (NumriFaturaveResponse.IsSuccessStatusCode) {
                                        var listOfNumriFaturave = JsonConvert.DeserializeObject<List<NumriFaturave>>(NumriFaturaveResult);
                                        await App.Database.ClearAllNumriFaturaveAsync();
                                        var currentNumriFaturave = await App.Database.GetNumriFaturaveAsync();
                                        await App.Database.SaveNumriFaturaveAllAsync(listOfNumriFaturave);
                                        currentNumriFaturave = await App.Database.GetNumriFaturaveAsync();
                                        return true;
                                    }
                                    else {
                                        UserDialogs.Instance.HideLoading();
                                        UserDialogs.Instance.Alert("Error ne NumriFaturave : SyncTable");
                                        return false;
                                    }
                                }
                                    return true;
                                
                            }
                            //CompanyInfo
                            if (tableName == "CompanyInfo") {
                                var CompanyInfoResponse = await App.ApiClient.GetAsync("company-info");
                                var CompanyInfoResult = await CompanyInfoResponse.Content.ReadAsStringAsync();
                                if (CompanyInfoResponse.IsSuccessStatusCode) {
                                    var listOfCompanyInfo = JsonConvert.DeserializeObject<List<CompanyInfo>>(CompanyInfoResult);
                                    await App.Database.ClearAllCompanyInfo();
                                    var currentCompanyInfo = await App.Database.GetCompanyInfoAsync();
                                    await App.Database.SaveCompanyInfoAsync(listOfCompanyInfo);
                                    currentCompanyInfo = await App.Database.GetCompanyInfoAsync();
                                    return true;
                                }
                                else {
                                    UserDialogs.Instance.HideLoading();
                                    UserDialogs.Instance.Alert("Error ne CompanyInfo : SyncTable");
                                    return false;
                                }
                            }
                            //DEPOT
                            if (tableName == "Depot") {
                                var DepotResponse = await App.ApiClient.GetAsync("depot");
                                var DepotResult = await DepotResponse.Content.ReadAsStringAsync();
                                if (DepotResponse.IsSuccessStatusCode) {
                                    var listOfDepot = JsonConvert.DeserializeObject<List<Depot>>(DepotResult);
                                    await App.Database.ClearAllDepotAsync();
                                    var DepotInfo = await App.Database.GetDepotAsync();
                                    await App.Database.SaveDepotAsync(listOfDepot);
                                    DepotInfo = await App.Database.GetDepotAsync();
                                    return true;
                                }
                                else {
                                    UserDialogs.Instance.HideLoading();
                                    UserDialogs.Instance.Alert("Error ne Depot : SyncTable");
                                    return false;
                                }
                            }
                            if(tableName == "Detyrimet") {
                                var DetyrimetResponse = await App.ApiClient.GetAsync("detyrimet");
                                var DetyrimetResult = await DetyrimetResponse.Content.ReadAsStringAsync();
                                if (DetyrimetResponse.IsSuccessStatusCode) {
                                    var listOfDetyrimet = JsonConvert.DeserializeObject<List<Detyrimet>>(DetyrimetResult);
                                    await App.Database.ClearAllDetyrimetAsync();
                                    var DepotInfo = await App.Database.GetDetyrimetAsync();
                                    await App.Database.SaveAllDetyrimetAsync(listOfDetyrimet);
                                    DepotInfo = await App.Database.GetDetyrimetAsync();
                                    return true;
                                }
                                else {
                                    UserDialogs.Instance.HideLoading();
                                    UserDialogs.Instance.Alert("Error ne Detyrimet : SyncTable");
                                    return false;
                                }
                            }
                            break;
                            }
                        #endregion

                        #region DownloadWithInsert

                        case PnSyncDirection.DownloadWithInsert: {
                                
                                break;
                            }
                        #endregion

                        #region UploadWithInsert
                        //*********************************UPLOAD****************************************************************
                        case PnSyncDirection.UploadWithInsert: {

                            if(tableName == "Malli_Mbetur") {
                                if (tableName == "Malli_Mbetur") {
                                    var Malli_MbeturUpdatuara = await CreateInsertScriptMalli_MbeturDetails(tableName, KeyFields, SqlCondition, StatusField);
                                    if (Malli_MbeturUpdatuara != null && Malli_MbeturUpdatuara.Count >= 1) {
                                        result = await CreateUpdateScriptMalli_Mbetur(Malli_MbeturUpdatuara);
                                        break;
                                    }
                                    result = false;
                                    break;
                                }
                            }
                            break;
                        }
                        #endregion

                        #region SpecialDownload

                        case PnSyncDirection.SpecialDownload: {

                                
                                break;
                            }
                        #endregion

                        #region SnapshotSpecial

                        case PnSyncDirection.SnapshotSpecial: {
                            try {
                                if (tableName == "Stoqet") {
                                    var query = App.Database._database.Table<Stoqet>();// Replace with your actual entity type

                                    if (!string.IsNullOrEmpty(SqlCondition)) {
                                        
                                    }
                                    if (await query.CountAsync() > 0) {

                                    }
                                    result = true;
                                }
                            }
                            catch (Exception ex) {
                                // If pull fails, rename back to the original name
                                SyncLog.ErrorMsg = ex.Message;
                                result = false;
                            }
                            result = true;

                            break;
                            }
                        }
                            #endregion
                }
                catch (Exception ex) {
                return result;
            }
            return result;
        }

        private async Task UpdateStatusFieldsVizita(List<Vizita> tables) {
            int index = 0;
            foreach(var table in tables) {
                table.SyncStatus = 1;
                await App.Database.SaveVizitaAsync(table);
                index++;
            }
        }


        private async Task<List<CashRegister>> CreateInsertScriptCashRegister(string tableName, string[] KeyFields, string sqlCondition, string StatusField) {
            if (tableName == "CashRegister") {
                StringBuilder strScriptBuilder = new StringBuilder();
                var CashRegisterUpdatedSync = new List<CashRegister>();
                if (sqlCondition != null && sqlCondition.Contains("1=1")) {
                    var CashRegister = await App.Database.GetCashRegisterAsync();
                    int rows = 0;
                    // Create insert script with records that have not been sent StatusField=0
                    if (StatusField == "SyncStatus") {
                        var CashRegister0 = CashRegister.Where(x => x.SyncStatus == 0).ToList();
                        CashRegisterUpdatedSync = CashRegister0;
                        if (StatusField != "0") {
                            // Assuming you have a method to update the status
                            await UpdateStatusFieldsCashRegister(CashRegister0);
                        }
                    }
                    return CashRegisterUpdatedSync;
                }
                return null;
            }

            return null;
        }
        private async Task<List<Malli_Mbetur>> CreateInsertScriptMalli_MbeturDetails(string tableName, string[] KeyFields, string sqlCondition, string StatusField) {
            if (tableName == "Malli_Mbetur") {
                StringBuilder strScriptBuilder = new StringBuilder();
                var Malli_MbeturUpdatedSync = new List<Malli_Mbetur>();
                if (sqlCondition != null && sqlCondition.Contains("1=1")) {
                    var Malli_Mbetur = await App.Database.GetMalliMbeturAsync();
                    int rows = 0;
                    foreach(var mall in Malli_Mbetur) {
                        if (mall.Depo == "M06" && mall.IDArtikulli == "A3054") {

                        }
                    }
                    
                    // Create insert script with records that have not been sent StatusField=0
                    var Malli_Mbetur0 = Malli_Mbetur.Where(x => x.SyncStatus == 0).ToList();
                    Malli_MbeturUpdatedSync = Malli_Mbetur0;
                    // Assuming you have a method to update the status
                    await UpdateStatusFieldsMalli_Mbetur(Malli_Mbetur0);
                    Malli_MbeturUpdatedSync = await App.Database.GetMalliMbeturAsync();
                    return Malli_MbeturUpdatedSync;
                }
                return null;
            }
            return null;
        }
        private async Task<List<LevizjetHeader>> CreateInsertScriptLevizjetHeader(string tableName, string[] KeyFields, string sqlCondition, string StatusField) {
            if (tableName == "LEVIZJET_HEADER") {
                StringBuilder strScriptBuilder = new StringBuilder();
                var LevizjetHeaderUpdatedSync = new List<LevizjetHeader>();
                if (sqlCondition != null && sqlCondition.Contains("1=1")) {
                    var LevizjetHeader = await App.Database.GetLevizjetHeaderAsync();
                    int rows = 0;
                    // Create insert script with records that have not been sent StatusField=0
                    if (StatusField == "SyncStatus") {
                        var LevizjetHeader0 = LevizjetHeader.Where(x => x.SyncStatus == 0).ToList();
                        LevizjetHeaderUpdatedSync = LevizjetHeader0;
                        if (StatusField != "0") {
                            // Assuming you have a method to update the status
                            await UpdateStatusFieldsLevizjetHeader(LevizjetHeader0);
                        }
                    }
                    return LevizjetHeaderUpdatedSync;
                }
                return null;
            }

            return null;
        }        

        private async Task<List<LevizjetDetails>> CreateInsertScriptLevizjetHeaderDetails(string tableName, string[] KeyFields, string sqlCondition, string StatusField) {
            if (tableName == "LEVIZJET_DETAILS") {
                StringBuilder strScriptBuilder = new StringBuilder();
                var LevizjetDetailsUpdatedSync = new List<LevizjetDetails>();
                if (sqlCondition != null && sqlCondition.Contains("1=1")) {
                    var LevizjetDetails = await App.Database.GetLevizjeDetailsAsync();
                    int rows = 0;
                    // Create insert script with records that have not been sent StatusField=0
                    if (StatusField == "SyncStatus") {
                        var LevizjetDetails0 = LevizjetDetails.Where(x => x.SyncStatus == 0).ToList();
                        LevizjetDetailsUpdatedSync = LevizjetDetails0;
                        if (StatusField != "0") {
                            // Assuming you have a method to update the status
                            await UpdateStatusFieldsLevizjetHeaderDetails(LevizjetDetails0);
                        }
                    }
                    return LevizjetDetailsUpdatedSync;
                }
                return null;
            }

            return null;
        }

        private async Task<List<KrijimiPorosive>> CreateInsertScriptKrijimiPorosive(string tableName, string[] KeyFields, string sqlCondition, string StatusField) {
            if (tableName == "Krijimi_Porosive") {
                StringBuilder strScriptBuilder = new StringBuilder();
                var OrderDetailsUpdatedSync = new List<KrijimiPorosive>();
                if (sqlCondition != null && sqlCondition.Contains("1=1")) {
                    var KrijimiPorosive = await App.Database.GetKrijimiPorosiveAsync();
                    int rows = 0;
                    // Create insert script with records that have not been sent StatusField=0
                    if (StatusField == "SyncStatus") {
                        var KrijimiPorosive0 = KrijimiPorosive.Where(x => x.SyncStatus == 0).ToList();
                        OrderDetailsUpdatedSync = KrijimiPorosive0;
                        if (StatusField != "0") {
                            // Assuming you have a method to update the status
                            await UpdateStatusFieldsKrijimiPorosive(KrijimiPorosive0);
                        }
                    }
                    return OrderDetailsUpdatedSync;
                }
                return null;
            }
            return null;
        }

        private async Task<List<OrderDetails>> CreateInsertScriptOrderDetails(string tableName, string[] KeyFields, string sqlCondition, string StatusField) {
            if (tableName == "Order_Details") {
                StringBuilder strScriptBuilder = new StringBuilder();
                var OrderDetailsUpdatedSync = new List<OrderDetails>();
                if (sqlCondition != null && sqlCondition.Contains("1=1")) {
                    var OrderDetails = await App.Database.GetOrderDetailsAsync();
                    int rows = 0;
                    // Create insert script with records that have not been sent StatusField=0
                    if (StatusField == "SyncStatus") {
                        var orderDetails0 = OrderDetails.Where(x => x.SyncStatus == 0).ToList();
                        OrderDetailsUpdatedSync = orderDetails0;
                        if (StatusField != "0") {
                            // Assuming you have a method to update the status
                            await UpdateStatusFieldsOrderDetails(orderDetails0);
                        }
                    }
                    return OrderDetailsUpdatedSync;
                }
                return null;
            }
            return null;
        }

        private async Task<List<Orders>> CreateInsertScriptOrders(string tableName, string[] KeyFields, string sqlCondition, string StatusField) {
            if (tableName == "Orders") {
                StringBuilder strScriptBuilder = new StringBuilder();
                var OrdersUpdatedSync = new List<Orders>();
                if (sqlCondition != null && sqlCondition.Contains("1=1")) {
                    var Orders = await App.Database.GetOrdersAsync();
                    int rows = 0;
                    // Create insert script with records that have not been sent StatusField=0
                    if (StatusField == "SyncStatus") {
                        var EvidencaPagesaveStatus0 = Orders.Where(x => x.SyncStatus == 0).ToList();
                        OrdersUpdatedSync = EvidencaPagesaveStatus0;
                        if (StatusField != "0") {
                            // Assuming you have a method to update the status
                            await UpdateStatusFieldsOrders(EvidencaPagesaveStatus0);
                        }
                    }
                    return OrdersUpdatedSync;
                }
                return null;
            }
            return null;
        }
        private async Task<List<EvidencaPagesave>> CreateInsertScriptEvidencaPagesave(string tableName, string[] KeyFields, string sqlCondition, string StatusField) {
            if (tableName == "EvidencaPagesave") {
                StringBuilder strScriptBuilder = new StringBuilder();
                var EvidencaPagesaveUpdatedSync = new List<EvidencaPagesave>();
                if (sqlCondition != null && sqlCondition.Contains("1=1")) {
                    var EvidencaPagesave = await App.Database.GetEvidencaPagesaveAsync();
                    int rows = 0;
                    // Create insert script with records that have not been sent StatusField=0
                    if (StatusField == "SyncStatus") {
                        var EvidencaPagesaveStatus0 = EvidencaPagesave.Where(x => x.SyncStatus == 0).ToList();
                        EvidencaPagesaveUpdatedSync = EvidencaPagesaveStatus0;
                        if (StatusField != "0") {
                            // Assuming you have a method to update the status
                            await UpdateStatusFieldsEvidencaPagesave(EvidencaPagesaveStatus0);
                        }
                    }
                    return EvidencaPagesaveUpdatedSync;
                }
                return null;
            }
            return null;
        }
        private async Task<List<PorosiaArt>> CreateInsertScriptPorositeArt(string tableName, string[] KeyFields, string sqlCondition, string StatusField) {
            if (tableName == "PorosiaArt") {
                StringBuilder strScriptBuilder = new StringBuilder();
                var porositeUpdatedSync = new List<PorosiaArt>();
                if (sqlCondition != null && sqlCondition.Contains("1=1")) {
                    var porosite = await App.Database.GetPorositeArtAsync();
                    int rows = 0;
                    // Create insert script with records that have not been sent StatusField=0
                    if (StatusField == "SyncStatus") {
                        var porosiaStatus0 = porosite.Where(x => x.SyncStatus == 0).ToList();
                        porositeUpdatedSync = porosiaStatus0;
                        if (StatusField != "0") {
                            // Assuming you have a method to update the status
                            await UpdateStatusFieldsPorositeArt(porosiaStatus0);
                        }
                    }
                    return porositeUpdatedSync;
                }
                return null;
            }
            return null;
        }
        private async Task<List<Porosite>> CreateInsertScriptPorosite(string tableName, string[] KeyFields, string sqlCondition, string StatusField)    {
            if (tableName == "Porosite") {
                StringBuilder strScriptBuilder = new StringBuilder();
                var porositeUpdatedSync = new List<Porosite>();
                if (sqlCondition != null && sqlCondition.Contains("1=1")) {
                    var porosite = await App.Database.GetPorositeAsync();
                    int rows = 0;
                    // Create insert script with records that have not been sent StatusField=0
                    if (StatusField == "SyncStatus") {
                        var porosiaStatus0 = porosite.Where(x => x.SyncStatus == 0).ToList();
                        porositeUpdatedSync = porosiaStatus0;
                        if (StatusField != "0") {
                            // Assuming you have a method to update the status
                            await UpdateStatusFieldsPorosite(porosiaStatus0);
                        }
                    }
                    return porositeUpdatedSync;
                }
                return null;
            }
            return null;
        }
        private async Task<List<LiferimiArt>> CreateInsertScriptLiferimiArt(string tableName, string[] KeyFields, string sqlCondition, string StatusField) {
            if (tableName == "LiferimiArt") {
                StringBuilder strScriptBuilder = new StringBuilder();
                var liferimiUpdatedSync = new List<LiferimiArt>();
                if (sqlCondition != null && sqlCondition.Contains("1=1")) {
                    var liferimet = await App.Database.GetLiferimetArtAsync();
                    int rows = 0;
                    // Create insert script with records that have not been sent StatusField=0
                    if (StatusField == "SyncStatus") {
                        var liferimiStatus0 = liferimet.Where(x => x.SyncStatus == 0).ToList();
                        liferimiUpdatedSync = liferimiStatus0;
                        if (StatusField != "0") {
                            // Assuming you have a method to update the status
                            await UpdateStatusFieldsLiferimiArt(liferimiStatus0);
                        }
                    }
                    return liferimiUpdatedSync;
                }
                return null;
            }
            return null;
        }
        private async Task<List<Liferimi>> CreateInsertScriptLiferimi(string tableName, string[] KeyFields, string sqlCondition, string StatusField) {
            if(tableName == "Liferimi") {
                StringBuilder strScriptBuilder = new StringBuilder();
                var liferimiUpdatedSync = new List<Liferimi>();
                var liferimet = await App.Database.GetLiferimetAsync();
                int rows = 0;
                // Create insert script with records that have not been sent StatusField=0
                if(StatusField == "SyncStatus") {
                    var liferimiStatus0 = liferimet.Where(x => x.SyncStatus == 0).ToList();
                    liferimiUpdatedSync = liferimiStatus0;
                    if (StatusField != "0") {
                        // Assuming you have a method to update the status
                        await UpdateStatusFieldsLiferimi(liferimiStatus0);
                    }
                }
                return liferimiUpdatedSync;
            }
            return null;
        }
        private async Task<List<Vizita>> CreateInsertScriptVizitat(string tableName, string[] KeyFields, string sqlCondition, string StatusField) {
            if(tableName == "Vizitat") {
                StringBuilder strScriptBuilder = new StringBuilder();
                var vizitatUpdatedSync = new List<Vizita>();
                if (sqlCondition != null && sqlCondition.Contains("DeviceID=")) {
                    //"DeviceID='M-06'"
                    var deviceId = sqlCondition.Split('\'')[1];
                    var vizitat = await App.Database.GetVizitatAsyncWithDeviceID(deviceId);
                    int rows = 0;
                    // Create insert script with records that have not been sent StatusField=0
                    if (StatusField == "SyncStatus") {
                        var vizitatStatus0 = vizitat.Where(x => x.SyncStatus == 0).ToList();
                        vizitatUpdatedSync = vizitatStatus0;
                        if (StatusField != "0") {
                            // Assuming you have a method to update the status
                            await UpdateStatusFieldsVizita(vizitatStatus0);
                        }
                    }
                    return vizitatUpdatedSync;
                }
                return null;
            }
            return null;
            
        }

        private async Task UpdateStatusFieldsPorositeArt(List<PorosiaArt> tables) {
            int index = 0;
            foreach (var table in tables) {
                table.SyncStatus = 1;
                await App.Database.SavePorosiaArtAsync(table);
                index++;
            }
        }
        private async Task UpdateStatusFieldsCashRegister(List<CashRegister> tables) {
            int index = 0;
            foreach (var table in tables) {
                table.SyncStatus = 1;
                await App.Database.SaveCashRegisterAsync(table);
                index++;
            }
        }
        private async Task UpdateStatusFieldsMalli_Mbetur(List<Malli_Mbetur> tables) {
            int index = 0;
            foreach (var table in tables) {
                table.SyncStatus = 1;
                await App.Database.UpdateMalliMbeturAsync(table);
                index++;
            }
        }
        private async Task UpdateStatusFieldsKrijimiPorosive(List<KrijimiPorosive> tables) {
            int index = 0;
            foreach (var table in tables) {
                table.SyncStatus = 1;
                await App.Database.SaveKrijimiPorosiveAsync(table);
                index++;
            }
        }
        private async Task UpdateStatusFieldsOrderDetails(List<OrderDetails> tables) {
            int index = 0;
            foreach (var table in tables) {
                table.SyncStatus = 1;
                await App.Database.SaveOrderDetailsAsync(table);
                index++;
            }
        }
        private async Task UpdateStatusFieldsOrders(List<Orders> tables) {
            int index = 0;
            foreach (var table in tables) {
                table.SyncStatus = 1;
                await App.Database.SaveOrderAsync(table);
                index++;
            }
        }
        private async Task UpdateStatusFieldsEvidencaPagesave(List<EvidencaPagesave> tables) {
            int index = 0;
            foreach (var table in tables) {
                table.SyncStatus = 1;
                await App.Database.SaveEvidencaPagesaveAsync(table);
                index++;
            }
        }
        private async Task UpdateStatusFieldsPorosite(List<Porosite> tables) {
            int index = 0;
            foreach (var table in tables) {
                table.SyncStatus = 1;
                await App.Database.SavePorositeAsync(table);
                index++;
            }
        }
        private async Task UpdateStatusFieldsLiferimiArt(List<LiferimiArt> tables) {
            int index = 0;
            foreach (var table in tables) {
                table.SyncStatus = 1;
                await App.Database.SaveLiferimiArtAsync(table);
                index++;
            }
        }
        private async Task UpdateStatusFieldsLevizjetHeader(List<LevizjetHeader> tables) {
            int index = 0;
            foreach (var table in tables) {
                table.SyncStatus = 1;
                await App.Database.SaveLevizjeHeaderAsync(table);
                index++;
            }
        }        
        private async Task UpdateStatusFieldsLevizjetHeaderDetails(List<LevizjetDetails> tables) {
            int index = 0;
            foreach (var table in tables) {
                table.SyncStatus = 1;
                await App.Database.UpdateLevizjeDetailsAsync(table);
                index++;
            }
        }

        private async Task UpdateStatusFieldsLiferimi(List<Liferimi> tables) {
            int index = 0;
            foreach (var table in tables) {
                table.SyncStatus = 1;
                await App.Database.SaveLiferimiAsync(table);
                index++;
            }
        }

        private async Task<bool> CreateUpdateScriptLevizjetHeaderDetails(List<LevizjetDetails> OrderDetails) {
            foreach (var levizjeDetail in OrderDetails) {
                if (levizjeDetail.Seri == null) {
                    levizjeDetail.Seri = levizjeDetail.IDArtikulli;
                }
                if (levizjeDetail.NumriLevizjes == null) {
                    levizjeDetail.NumriLevizjes = "1";
                }
                await App.Database.UpdateLevizjeDetailsAsync(levizjeDetail);
            }
            var LevizjetHeaderJson = JsonConvert.SerializeObject(OrderDetails);
            var stringContent = new StringContent(LevizjetHeaderJson, Encoding.UTF8, "application/json");
            var result = await App.ApiClient.PostAsync("levizje-detial", stringContent);
            
            if (result.IsSuccessStatusCode) {
                return true;
            }
            foreach (var porosia in OrderDetails) {
                porosia.SyncStatus = 0;
                await App.Database.UpdateLevizjeDetailsAsync(porosia);
            }
            return false;
        }       
        private async Task<bool> CreateUpdateScriptLevizjetHeader(List<LevizjetHeader> OrderDetails) {
            var LevizjetHeaderJson = JsonConvert.SerializeObject(OrderDetails);
            var stringContent = new StringContent(LevizjetHeaderJson, Encoding.UTF8, "application/json");
            var result = await App.ApiClient.PostAsync("levizje-header", stringContent);
            if (result.IsSuccessStatusCode) {
                return true;
            }
            foreach (var porosia in OrderDetails) {
                porosia.SyncStatus = 0;
                await App.Database.SaveLevizjeHeaderAsync(porosia);
            }
            return false;
        }
        private async Task<bool> CreateUpdateScriptCashRegister(List<CashRegister> OrderDetasails) {

            var OrderDetailsJson = JsonConvert.SerializeObject(OrderDetasails);
            var stringContent = new StringContent(OrderDetailsJson, Encoding.UTF8, "application/json");
            var result = await App.ApiClient.PostAsync("cash-register", stringContent);
            if (result.IsSuccessStatusCode) {
                return true;
            }
            foreach (var porosia in OrderDetasails) {
                porosia.SyncStatus = 0;
                await App.Database.SaveCashRegisterAsync(porosia);
            }
            return false;
        }
        private async Task<bool> CreateUpdateScriptMalli_Mbetur(List<Malli_Mbetur> OrderDetasails) {
            foreach(var mall in OrderDetasails) {
                mall.Data = DateTime.Now;
                mall.Export_Status = 0;
            }
            var OrderDetailsJson = JsonConvert.SerializeObject(OrderDetasails);
            var stringContent = new StringContent(OrderDetailsJson, Encoding.UTF8, "application/json");
            var result = await App.ApiClient.PostAsync("malli-mbetur", stringContent);
            if (result.IsSuccessStatusCode) {
                return true;
            }
            foreach (var porosia in OrderDetasails) {
                porosia.SyncStatus = 0;
            }
            await App.Database.UpdateAllMalliMbeturAsync(OrderDetasails);
            return false;
        }
        private async Task<bool> CreateUpdateScriptOrderDetails(List<OrderDetails> OrderDetails) {

            var OrderDetailsJson = JsonConvert.SerializeObject(OrderDetails);
            var stringContent = new StringContent(OrderDetailsJson, Encoding.UTF8, "application/json");
            var result = await App.ApiClient.PostAsync("orders/details", stringContent);
            if (result.IsSuccessStatusCode) {
                return true;
            }
            foreach (var porosia in OrderDetails) {
                porosia.SyncStatus = 0;
                await App.Database.UpdateOrderDetailsAsync(porosia);
            }
            return false;
        }
        private async Task<bool> CreateUpdateScriptKrijimiPorosive(List<KrijimiPorosive> OrderDetails) {

            var OrderDetailsJson = JsonConvert.SerializeObject(OrderDetails);
            var stringContent = new StringContent(OrderDetailsJson, Encoding.UTF8, "application/json");
            var result = await App.ApiClient.PostAsync("krijimi-porosive", stringContent);
            if (result.IsSuccessStatusCode) {
                return true;
            }
            foreach (var porosia in OrderDetails) {
                porosia.SyncStatus = 0;
                await App.Database.UpdateKrijimiPorosiveAsync(porosia);
            }
            return false;
        }
        private async Task<bool> CreateUpdateScriptOrders(List<Orders> porositeArt) {

            var porositeArtJson = JsonConvert.SerializeObject(porositeArt);
            var stringContent = new StringContent(porositeArtJson, Encoding.UTF8, "application/json");
            var result = await App.ApiClient.PostAsync("orders", stringContent);
            if (result.IsSuccessStatusCode) {
                return true;
            }
            foreach (var porosia in porositeArt) {
                porosia.SyncStatus = 0;
            }
            await App.Database.SaveOrdersAsync(porositeArt);
            return false;
        }
        private async Task<bool> CreateUpdateScriptEvidencaPagesave(List<EvidencaPagesave> porositeArt) {

            var porositeArtJson = JsonConvert.SerializeObject(porositeArt);
            var stringContent = new StringContent(porositeArtJson, Encoding.UTF8, "application/json");
            var result = await App.ApiClient.PostAsync("pagesa", stringContent);
            if (result.IsSuccessStatusCode) {
                return true;
            }
            return false;
        }
        private async Task<bool> CreateUpdateScriptPorosiaArt(List<PorosiaArt> porositeArt) {

            foreach (var porosiaArt in porositeArt) {
                if (porosiaArt.Seri == null) {
                    porosiaArt.Seri = porosiaArt.IDArtikulli;
                }
            }
            var porositeArtJson = JsonConvert.SerializeObject(porositeArt);
            var stringContent = new StringContent(porositeArtJson, Encoding.UTF8, "application/json");

            var result = await App.ApiClient.PostAsync("porosia/items", stringContent);
            if (result.IsSuccessStatusCode) {
                return true;
            }
            foreach (var porosia in porositeArt) {
                porosia.SyncStatus = 0;
            }
            await App.Database.SaveAllPorositeArtAsync(porositeArt);
            return false;
        }
        private async Task<bool> CreateUpdateScriptLiferimi(List<Liferimi> Liferimi) {
            foreach (var lif in Liferimi) {
                if (lif.TCRIssueDateTime.Year < 2000) {
                    lif.TCRIssueDateTime = DateTime.Now;
                }
            }
            var vizitatJson = JsonConvert.SerializeObject(Liferimi);

            var stringContent = new StringContent(vizitatJson, Encoding.UTF8, "application/json");
            var result = await App.ApiClient.PostAsync("liferimi", stringContent);
            if (result.IsSuccessStatusCode) {
                return true;
            }
            else {
                foreach (var viz in Liferimi) {
                    viz.SyncStatus = 0;
                    await App.Database.SaveLiferimiAsync(viz);
                }
            }
            return false;
        }

        private async Task<bool> CreateUpdateScriptPorosite(List<Porosite> porosite) {

            var porositeJson = JsonConvert.SerializeObject(porosite);
            var stringContent = new StringContent(porositeJson, Encoding.UTF8, "application/json");
            var result = await App.ApiClient.PostAsync("porosia", stringContent);
            if (result.IsSuccessStatusCode) {
                return true;
            }
            foreach(var porosia in porosite) {
                porosia.SyncStatus = 0;
            }
            //REVERT THE STATUS IF NOT UPLOADED TO SERVER
            await App.Database.SaveAllPorositeAsync(porosite);
            return false;
        }        

        private async Task<bool> CreateUpdateScriptLiferimiArt(List<LiferimiArt> vizitat) {

            //TODO CHECK LIFERIMI UPDATE AGAIN WHEN API FINISH
            var vizitatJson = JsonConvert.SerializeObject(vizitat);
            var stringContent = new StringContent(vizitatJson, Encoding.UTF8, "application/json");
            var result = await App.ApiClient.PostAsync("liferimi/items", stringContent);
            if (result.IsSuccessStatusCode) {

                return true;
            }
            else {
                foreach(var viz in vizitat) {
                    viz.SyncStatus = 0;
                    await App.Database.SaveLiferimiArtAsync(viz);
                }
            }
            return false;
        }

        private async Task<bool> CreateUpdateScriptVizitat(List<Vizita> vizitat) {
            //TODO CHECK VIZITA UPDATE AGAIN
            var vizitatJson = JsonConvert.SerializeObject(vizitat);
            var stringContent = new StringContent(vizitatJson, Encoding.UTF8, "application/json");
            var result = await App.ApiClient.PostAsync("vizitat", stringContent);
            if(result.IsSuccessStatusCode) {
                return true;
            }
            return false;
        }

        //TODO UPDATE LIFERIMI
        //public async bool UpdateLiferimi(string deviceId) {
        //    var updateResult = await App.ApiClient.PutAsync()
        //}


    }
}
