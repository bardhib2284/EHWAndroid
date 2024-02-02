using Acr.UserDialogs;
using EHW_M;
using EHWM.DependencyInjections.FiskalizationExtraModels;
using EHWM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EHWM.ViewModel {

    public class FiskalizimiViewModelNavigationParams {
        public List<CashRegister> CashRegisters { get; set; }
        public List<Liferimi> Liferimet { get; set; }
        public List<LevizjetHeader> Levizjet { get; set; }
        public List<EvidencaPagesave> InkasimetList { get; set; }
    }


    public class FiskalizimiViewModel : BaseViewModel {

        private ObservableCollection<EvidencaPagesave> _InkasimetList;
        public ObservableCollection<EvidencaPagesave> InkasimetList {
            get { return _InkasimetList; }
            set { SetProperty(ref _InkasimetList, value); }
        }
        private ObservableCollection<CashRegister> _cashRegisterList;
        public ObservableCollection<CashRegister> CashRegisterList {
            get { return _cashRegisterList; }
            set { SetProperty(ref _cashRegisterList, value); }
        }

        private ObservableCollection<Liferimi> _liferimet;
        public ObservableCollection<Liferimi> LiferimetList {
            get { return _liferimet; }
            set { SetProperty(ref _liferimet, value); }
        }

        private ObservableCollection<LevizjetHeader> _levizjet;
        public ObservableCollection<LevizjetHeader> LevizjetList {
            get { return _levizjet; }
            set { SetProperty(ref _levizjet, value); }
        }

        public Agjendet Agjendi { get; set; }
        public CashRegister SelectedCashRegister { get; set; }
        public Liferimi SelectedLiferimi { get; set; }
        public LevizjetHeader SelectedLevizja { get; set; }
        public int SelectedIndex { get; set; }
        public ICommand FiskalizoItemEZgjedhurCommand { get; set; }
        public FiskalizimiViewModel(FiskalizimiViewModelNavigationParams fiskalizimiViewModelNavigationParams) {
            if(fiskalizimiViewModelNavigationParams != null) {
                LiferimetList = new ObservableCollection<Liferimi>(fiskalizimiViewModelNavigationParams.Liferimet.Where(x=> x.TCRSyncStatus <= 0));
                CashRegisterList = new ObservableCollection<CashRegister>(fiskalizimiViewModelNavigationParams.CashRegisters.Where(x=> x.TCRSyncStatus <= 0));
                LevizjetList = new ObservableCollection<LevizjetHeader>(fiskalizimiViewModelNavigationParams.Levizjet);
            }
            Agjendi = App.Instance.MainViewModel.LoginData;
            FiskalizoItemEZgjedhurCommand = new Command(async () => await FiskalizoItemEZgjedhurAsync());

        }

        public async Task FiskalizoItemEZgjedhurAsync() {
            UserDialogs.Instance.ShowLoading("Filloi procesi i fiskalizimit");
            

            if(SelectedIndex == 0) {
                if (SelectedCashRegister == null) {
                    UserDialogs.Instance.Alert("Nuk ka element te selektuar per te fiskalizuar, ju lutem selektoni njerin nga elementet ne liste", "Verejtje");
                    UserDialogs.Instance.HideLoading();

                    return;
                }
                if (SelectedCashRegister.TCRSyncStatus == 1) {
                    UserDialogs.Instance.Alert("Cash register eshte e fiskalizuar, ju lutem provoni levizje tjeter", "Verejtje");
                    UserDialogs.Instance.HideLoading();

                    return;
                }
                RegisterCashDepositInputRequestPCL registerCashDepositInputRequestPCL = new RegisterCashDepositInputRequestPCL
                {
                    CashAmount = SelectedCashRegister.Cashamount,
                    DepositType = CashDepositOperationSTypePCL.DEPOSIT,
                    OperatorCode = SelectedCashRegister.TCRCode,
                    SendDateTime = SelectedCashRegister.RegisterDate,
                    SubseqDelivTypeSType = 1,
                    TCRCode = SelectedCashRegister.TCRCode
                };
                var result =  App.Instance.FiskalizationService.RegisterCashDeposit(registerCashDepositInputRequestPCL);
                if (result.Status == StatusPCL.Ok) {
                    SelectedCashRegister.TCRSyncStatus = 1;
                    SelectedCashRegister.Message = result.Message.Replace("'", "");
                }
                else if (result.Status == StatusPCL.TCRAlreadyRegistered) {
                    SelectedCashRegister.TCRSyncStatus = 4;
                    SelectedCashRegister.Message = result.Message.Replace("'", "");
                }
                else {
                    SelectedCashRegister.TCRSyncStatus = -1;
                    SelectedCashRegister.Message = result.Message.Replace("'", "");
                }
                await App.Database.UpdateCashRegisterAsync(SelectedCashRegister);
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert("Procesi mbaroi");
            }

            else if(SelectedIndex == 1) {
                if (SelectedLiferimi == null) {
                    UserDialogs.Instance.Alert("Nuk ka element te selektuar per te fiskalizuar, ju lutem selektoni njerin nga elementet ne liste", "Verejtje");
                    UserDialogs.Instance.HideLoading();

                    return;
                }
                if (SelectedLiferimi.TCRSyncStatus == 1) {
                    UserDialogs.Instance.Alert("Fatura eshte e fiskalizuar, ju lutem provoni fature tjeter", "Verejtje");
                    UserDialogs.Instance.HideLoading();

                    return;
                }
                UserDialogs.Instance.ShowLoading("Filloi procesi i fiskalizimit");
                await FiskalizoTCRInvoice(SelectedLiferimi.IDLiferimi.ToString());
                UserDialogs.Instance.Alert("Procesi mbaroi");
                UserDialogs.Instance.HideLoading();
            }
            else if(SelectedIndex == 2) {
                if (SelectedLevizja == null) {
                    UserDialogs.Instance.Alert("Nuk ka element te selektuar per te fiskalizuar, ju lutem selektoni njerin nga elementet ne liste", "Verejtje");
                    UserDialogs.Instance.HideLoading();

                    return;
                }
                if(SelectedLevizja.TCRSyncStatus == 1) {
                    UserDialogs.Instance.Alert("Levizja eshte e fiskalizuar, ju lutem provoni levizje tjeter", "Verejtje");
                    UserDialogs.Instance.HideLoading();

                    return;
                }
                UserDialogs.Instance.ShowLoading("Filloi procesi i fiskalizimit");
                await RegisterTCRWTN(SelectedLevizja.NumriLevizjes);
                UserDialogs.Instance.Alert("Procesi mbaroi");
                UserDialogs.Instance.HideLoading();
            }
        }

        public async Task FiskalizoTCRInvoice(string idLiferimi) {
            var liferimet = await App.Database.GetLiferimetAsync();
            var liferimetArt = await App.Database.GetLiferimetArtAsync();
            liferimetArt = liferimetArt.Take(1).ToList();
            var query = from l2 in liferimet
                        join la2 in liferimetArt on l2.IDLiferimi equals la2.IDLiferimi
                        where (l2.TCRSyncStatus <= 0 || l2.TCRSyncStatus == null) &&
                              l2.DeviceID == Agjendi.DeviceID &&
                              string.Equals(l2.IDLiferimi.ToString().Trim(), idLiferimi.Trim(), StringComparison.OrdinalIgnoreCase)
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
                var query2 = from l in liferimet
                             join la in liferimetArt on l.IDLiferimi equals la.IDLiferimi
                             join k in klientet on l.IDKlienti equals k.IDKlienti
                             join kl in Klientdhelokacion on k.IDKlienti equals kl.IDKlienti
                             join a in artikujt on la.IDArtikulli equals a.IDArtikulli
                             join ci in companyInfo on "TVSH" equals ci.Item
                             where 
                                   string.Equals(l.IDLiferimi.ToString().Trim(), idLiferimi.Trim(), StringComparison.OrdinalIgnoreCase) &&
                                   l.DeviceID == App.Instance.MainViewModel.LoginData.DeviceID
                             select new MapperLines
                             {
                                 IDLiferimi = l.IDLiferimi.ToString(),
                                 NrLiferimit = l.NrLiferimit,
                                 DeviceID = l.Depo,
                                 LLOJDOK = l.LLOJDOK,
                                 IDKthimi = l.IDKthimi ?? "",
                                 OperatorCode = "OperatorCode",
                                 InvoiceSType = "Cash GJITHMON",
                                 InvNum = l.NumriFisk + "/" + DateTime.Now.Year,
                                 InvOrdNum = Convert.ToInt32(l.NumriFisk),
                                 SendDatetime = l.KohaLiferimit,
                                 IsIssuerInVAT = true,
                                 TaxFreeAmt = Math.Round(0d, 2),
                                 PaymentMethodTypesType = l.PayType,
                                 Buyer_IDNum = k.NIPT ?? "",
                                 Buyer_IDType = "idtypestype.id",
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
                                 IsReverseCharge = false
                             };

                if (mapperHeaderList.Count > 0) {
                    List<MapperLines> mapperLineList = query2.ToList();

                    List<TCRLiferimi> invoiceObject = ClassMapper.MapHeadersAndLines(mapperHeaderList, mapperLineList);
                    foreach (TCRLiferimi inv in invoiceObject) {
                        RegisterInvoiceInputRequestPCL req = new RegisterInvoiceInputRequestPCL();
                        req.Buyer = inv.Buyer;
                        req.InvoiceItems = inv.Items.ToList();

                        req.DeviceID = inv.DeviceID;
                        req.NrLiferimit = inv.NrLiferimit;
                        req.SubseqDelivTypeSType = -1;//NOINTERNET
                        req.IICRef = inv.IICRef;
                        req.InoiceType = InvoiceSTypePCL.CASH;
                        req.InoiceTypeSpecified = true;
                        req.InvOrdNum = inv.InvOrdNum.ToString();
                        req.IsCorrectiveInv = inv.IsCorrectiveInv;
                        req.IsIssuerInVAT = inv.IsIssuerInVAT;
                        req.IsReverseCharge = inv.IsReverseCharge;
                        req.IssueDateTimeRef = inv.IssueDateTimeRef;
                        req.MobileRefId = inv.InvOrdNum.ToString();
                        req.OperatorCode = App.Instance.MainViewModel.LoginData.OperatorCode;
                        req.TCRCode = App.Instance.MainViewModel.Configurimi.KodiTCR;
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
                            req.InvNum = inv.InvNum + "/" + App.Instance.MainViewModel.Configurimi.KodiTCR;
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
                        req.SubseqDelivTypeSType = -1; //ONLINE


                        ResultLogPCL log = App.Instance.FiskalizationService.RegisterInvoice(req);
                        if(log == null) {
                            liferimet = await App.Database.GetLiferimetAsync();
                            liferimetArt = await App.Database.GetLiferimetArtAsync();
                            var liferimiToUpdate = liferimet
                                                .FirstOrDefault(l => l.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                            if (liferimiToUpdate != null) {
                                liferimiToUpdate.TCRSyncStatus = 1;
                                liferimiToUpdate.TCRIssueDateTime = DateTime.Now;
                                liferimiToUpdate.TCRQRCodeLink = null;
                                liferimiToUpdate.TCR = App.Instance.MainViewModel.Configurimi.KodiTCR;
                                liferimiToUpdate.TCROperatorCode = App.Instance.MainViewModel.LoginData.OperatorCode;
                                liferimiToUpdate.TCRBusinessCode = liferimiToUpdate.TCRBusinessCode;
                                liferimiToUpdate.UUID = null;
                                liferimiToUpdate.EIC = null;
                                liferimiToUpdate.TCRNSLF = null;
                                liferimiToUpdate.TCRNIVF = null;
                                liferimiToUpdate.Message = "Fiskalizimi deshtoi, ju lutemi provoni me vone!";

                                await App.Database.SaveLiferimiAsync(liferimiToUpdate);

                                var liferimiArtToUpdate = liferimetArt
                                        .Where(la => la.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                                foreach (var art in liferimiArtToUpdate) {
                                    art.TCRSyncStatus = 2;
                                    await App.Database.SaveLiferimiArtAsync(art);
                                }
                            }
                        }
                        else if (log.Status == StatusPCL.Ok) {
                            liferimet = await App.Database.GetLiferimetAsync();
                            liferimetArt = await App.Database.GetLiferimetArtAsync();
                            var liferimiToUpdate = liferimet
                                                .FirstOrDefault(l => l.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                            if (liferimiToUpdate != null) {
                                liferimiToUpdate.TCRSyncStatus = -1;
                                liferimiToUpdate.TCRIssueDateTime = DateTime.Now;
                                liferimiToUpdate.TCRQRCodeLink = log.QRCodeLink;
                                liferimiToUpdate.TCR = App.Instance.MainViewModel.Configurimi.KodiTCR;
                                liferimiToUpdate.TCROperatorCode = App.Instance.MainViewModel.LoginData.OperatorCode;
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
                                    await App.Database.SaveLiferimiArtAsync(art);
                                }
                            }
                        }
                        else if (log.Status == StatusPCL.FaultCode) {
                            if (string.IsNullOrEmpty(log.Message)) {
                                liferimet = await App.Database.GetLiferimetAsync();
                                liferimetArt = await App.Database.GetLiferimetArtAsync();
                                var liferimiToUpdate = liferimet
                                                    .FirstOrDefault(l => l.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                                if (liferimiToUpdate != null) {
                                    liferimiToUpdate.TCRSyncStatus = -1;
                                    liferimiToUpdate.TCRIssueDateTime = DateTime.Now;
                                    liferimiToUpdate.TCRQRCodeLink = log.QRCodeLink;
                                    liferimiToUpdate.TCR = App.Instance.MainViewModel.Configurimi.KodiTCR;
                                    liferimiToUpdate.TCROperatorCode = App.Instance.MainViewModel.LoginData.OperatorCode;
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
                                        await App.Database.SaveLiferimiArtAsync(art);
                                    }
                                }
                            }
                            else {
                                liferimet = await App.Database.GetLiferimetAsync();
                                liferimetArt = await App.Database.GetLiferimetArtAsync();
                                var liferimiToUpdate = liferimet
                                                    .FirstOrDefault(l => l.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                                if (liferimiToUpdate != null) {
                                    liferimiToUpdate.TCRSyncStatus = 1;
                                    liferimiToUpdate.TCRIssueDateTime = DateTime.Now;
                                    liferimiToUpdate.TCRQRCodeLink = log.QRCodeLink;
                                    liferimiToUpdate.TCR = App.Instance.MainViewModel.Configurimi.KodiTCR;
                                    liferimiToUpdate.TCROperatorCode = App.Instance.MainViewModel.LoginData.OperatorCode;
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
                                        await App.Database.SaveLiferimiArtAsync(art);
                                    }
                                }
                            }
                        }
                        else if (log.Status == StatusPCL.TCRAlreadyRegistered) {
                            liferimet = await App.Database.GetLiferimetAsync();
                            liferimetArt = await App.Database.GetLiferimetArtAsync();
                            var liferimiToUpdate = liferimet
                                .Where(l => l.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                            foreach (var liferi in liferimiToUpdate) {
                                liferi.TCRSyncStatus = 4;
                                liferi.TCRIssueDateTime = DateTime.Now;
                                liferi.TCRQRCodeLink = log.QRCodeLink;
                                liferi.TCR = App.Instance.MainViewModel.Configurimi.KodiTCR;
                                liferi.TCROperatorCode = App.Instance.MainViewModel.LoginData.OperatorCode;
                                liferi.TCRBusinessCode = liferi.TCRBusinessCode;
                                liferi.UUID = log.ResponseUUID;
                                liferi.EIC = log.EIC;
                                liferi.TCRNSLF = log.NSLF;
                                liferi.TCRNIVF = log.NIVF;
                                liferi.Message = log.Message.Replace("'", "");
                            }

                            var liferimiArtToUpdate = liferimetArt
                                .Where(la => la.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                            foreach (var art in liferimiArtToUpdate) {
                                art.TCRSyncStatus = 4;
                                await App.Database.SaveLiferimiArtAsync(art);
                            }
                        }
                        else if (log.Status == StatusPCL.InProcess) {
                            liferimet = await App.Database.GetLiferimetAsync();
                            liferimetArt = await App.Database.GetLiferimetArtAsync();
                            var liferimiToUpdate = liferimet
                                .Where(l => l.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                            foreach (var liferi in liferimiToUpdate) {
                                liferi.TCRSyncStatus = -2;
                                liferi.TCRIssueDateTime = DateTime.Now;
                                liferi.TCRQRCodeLink = log.QRCodeLink;
                                liferi.TCR = App.Instance.MainViewModel.Configurimi.KodiTCR;
                                liferi.TCROperatorCode = App.Instance.MainViewModel.LoginData.OperatorCode;
                                liferi.TCRBusinessCode = liferi.TCRBusinessCode;
                                liferi.UUID = log.ResponseUUID;
                                liferi.EIC = log.EIC;
                                liferi.TCRNSLF = log.NSLF;
                                liferi.TCRNIVF = log.NIVF;
                                liferi.Message = log.Message.Replace("'", "");
                            }

                            var liferimiArtToUpdate = liferimetArt
                                .Where(la => la.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                            foreach (var art in liferimiArtToUpdate) {
                                art.TCRSyncStatus = -2;
                                await App.Database.SaveLiferimiArtAsync(art);
                            }
                        }
                        else {
                            try {
                                liferimet = await App.Database.GetLiferimetAsync();
                                liferimetArt = await App.Database.GetLiferimetArtAsync();
                                var liferimiToUpdate = liferimet
                                    .Where(l => l.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                                foreach (var lif in liferimiToUpdate) {
                                    lif.TCRSyncStatus = -3;
                                    lif.TCRIssueDateTime = DateTime.Now;
                                    lif.TCRQRCodeLink = log.QRCodeLink;
                                    lif.TCR = App.Instance.MainViewModel.Configurimi.KodiTCR;
                                    lif.TCROperatorCode = App.Instance.MainViewModel.LoginData.OperatorCode;
                                    lif.TCRBusinessCode = lif.TCRBusinessCode;
                                    lif.UUID = log.ResponseUUID;
                                    lif.EIC = log.EIC;
                                    lif.TCRNSLF = log.NSLF;
                                    lif.TCRNIVF = log.NIVF;
                                    lif.Message = log.Message.Replace("'", "");
                                }

                                var liferimiArtToUpdate = liferimetArt
                                    .Where(la => la.IDLiferimi.ToString().Trim().ToLower() == inv.IDLiferimi.Trim().ToLower());

                                foreach (var art in liferimiArtToUpdate) {
                                    art.TCRSyncStatus = -1;
                                    await App.Database.SaveLiferimiArtAsync(art);
                                }
                            }
                            catch (Exception ex) {
                                // Handle exception
                            }
                        }
                    }
                }
            }
            else {
                UserDialogs.Instance.Alert("Shitja vecse eshte fiskalizuar, ju lutemi provoni shitje tjeter");
                UserDialogs.Instance.HideLoading();
                return;
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

            List<Models.WTNModels.MapperLines> mapperLinesList = query.ToList();

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
                    req.TCRCode = App.Instance.MainViewModel.Configurimi.KodiTCR;
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
                    if (log == null) {
                        var levizjaHeader = levizjetHeader
                                        .FirstOrDefault(h => h.NumriLevizjes == inv.Numri_Levizjes);

                        if (levizjaHeader != null) {
                            levizjaHeader.TCRSyncStatus = -1;
                            levizjaHeader.TCRIssueDateTime = DateTime.Now;
                            levizjaHeader.TCRQRCodeLink = null;
                            levizjaHeader.TCR = App.Instance.MainViewModel.Configurimi.KodiTCR;
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
                            levizjaHeader.TCR = App.Instance.MainViewModel.Configurimi.KodiTCR;
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
                                    levizjaHeader.TCR = App.Instance.MainViewModel.Configurimi.KodiTCR;
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
                                    levizjaHeader.TCR = App.Instance.MainViewModel.Configurimi.KodiTCR;
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
                            levizjaHeader.TCR = App.Instance.MainViewModel.Configurimi.KodiTCR;
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

    }
}
