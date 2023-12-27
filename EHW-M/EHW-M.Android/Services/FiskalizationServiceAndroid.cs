
using Android.Mtp;
using EHW_M.Droid.FiskalizimiService;
using EHW_M.Droid.Services;
using EHWM.DependencyInjections;
using EHWM.DependencyInjections.FiskalizationExtraModels;
using System;

using Xamarin.Forms;
using static Android.Bluetooth.BluetoothClass;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using VouchersSoldType = EHW_M.Droid.FiskalizimiService.VouchersSoldType;

[assembly: Dependency(typeof(FiskalizationServiceAndroid))]
namespace EHW_M.Droid.Services {
    public class FiskalizationServiceAndroid : IFiskalizationService {

        public ResultLogPCL RegisterCashDeposit(RegisterCashDepositInputRequestPCL reqpcl) {
            ResultLog resultlog = new ResultLog();
            ResultLogPCL logpcl = new ResultLogPCL();
            resultlog.Status = Status.InternalError;
            try {
                using (FiscalisationService fs = new FiscalisationService()) {
                    fs.Url = App.Instance.WebServerFiskalizimiUrl;
                    RegisterCashDepositInputRequest req = new RegisterCashDepositInputRequest
                    {
                        CashAmount = reqpcl.CashAmount,
                        DepositType = ((CashDepositOperationSType)(int)reqpcl.DepositType),
                        OperatorCode = reqpcl.OperatorCode,
                        SendDateTime = reqpcl.SendDateTime,
                        SubseqDelivTypeSType = reqpcl.SubseqDelivTypeSType,
                        TCRCode = reqpcl.TCRCode
                    };
                    ResultLog log = fs.RegisterCashDeposit(req);

                    logpcl = new ResultLogPCL
                    {
                        Created = log.Created,
                        Request = log.Request,
                        Response = log.Response,
                        ResponseObject = log.ResponseObject,
                        Status = (StatusPCL)(int)log.Status,
                        RequestType = (RequestTypePCL)(int)log.RequestType,
                        Stacktrace = log.Stacktrace,
                        Message = log.Message,
                        MobileIdRef = log.MobileIdRef,
                        OperatorCode = log.OperatorCode,
                        QRCodeLink = log.QRCodeLink,
                        TCRBusinessCode = log.TCRBusinessCode,
                        NSLF = log.NSLF,
                        NIVF = log.NIVF,
                        ResponseUUID = log.ResponseUUID,
                        EIC = log.EIC,
                        ResponseUUIDSH = log.ResponseUUIDSH,
                        NIVFSH = log.NIVFSH,
                        NSLFSH = log.NSLFSH
                    };
                    return logpcl;
                }
            }
            catch (Exception ex) {
                resultlog.Message = ex.Message;
                resultlog.Stacktrace = ex.StackTrace;
                //PnUtils.DbUtils.WriteExeptionErrorLog(ex);
            }
            return logpcl;

        }

        public ResultLogPCL RegisterInvoice(RegisterInvoiceInputRequestPCL req) {
            try {
                InvoiceItemType[] invoiceItemTypes = new InvoiceItemType[req.InvoiceItems.Count];
                ResultLogPCL logpcl = new ResultLogPCL();

                for (int i = 0; i < req.InvoiceItems.Count; i++) {
                    InvoiceItemType invoiceItemType = new InvoiceItemType();
                    
                    if (req.InvoiceItems[i].VS != null) {
                        if (req.InvoiceItems[i].VS.VD != null) {
                            invoiceItemType.VS = new VouchersSoldType
                            {
                                VD = new FiskalizimiService.VoucherSoldDataType
                                {
                                    D = (DateTime)(req.InvoiceItems[i]?.VS?.VD?.D),
                                    N = (decimal)(req.InvoiceItems[i]?.VS?.VD?.N)
                                },
                            };
                        }
                        
                    }
                    invoiceItemType.N = req.InvoiceItems[i].N;
                    invoiceItemType.C = req.InvoiceItems[i].C;
                    invoiceItemType.U = req.InvoiceItems[i].U;
                    invoiceItemType.Q = req.InvoiceItems[i].Q;
                    invoiceItemType.UPB = req.InvoiceItems[i].UPB;
                    invoiceItemType.UPA = req.InvoiceItems[i].UPA;
                    invoiceItemType.R = req.InvoiceItems[i].R;
                    invoiceItemType.RSpecified = req.InvoiceItems[i].RSpecified;
                    invoiceItemType.RR = req.InvoiceItems[i].RR;
                    invoiceItemType.RRSpecified = req.InvoiceItems[i].RRSpecified;
                    invoiceItemType.PB = req.InvoiceItems[i].PB;
                    invoiceItemType.VR = req.InvoiceItems[i].VR;
                    invoiceItemType.VRSpecified = req.InvoiceItems[i].VRSpecified;
                    invoiceItemType.VA = req.InvoiceItems[i].VA;
                    invoiceItemType.VASpecified = req.InvoiceItems[i].VASpecified;
                    invoiceItemType.IN = req.InvoiceItems[i].IN;
                    invoiceItemType.INSpecified = req.InvoiceItems[i].INSpecified;
                    invoiceItemType.PA = req.InvoiceItems[i].PA;
                    invoiceItemType.EX = (FiskalizimiService.ExemptFromVATSType)req.InvoiceItems[i].EX;
                    invoiceItemType.EXSpecified = req.InvoiceItems[i].EXSpecified;
                    invoiceItemTypes[i] = invoiceItemType;
                }
                RegisterInvoiceInputRequest request = new RegisterInvoiceInputRequest
                {
                    TCRCode = req.TCRCode,
                    OperatorCode = req.OperatorCode,
                    BusinessUnitCode = req.BusinessUnitCode,
                    InoiceTypeSpecified = req.InoiceTypeSpecified,
                    InoiceType = (InvoiceSType)req.InoiceType,
                    InvOrdNum = req.InvOrdNum,
                    InvNum = req.InvNum,
                    SendDatetime = req.SendDatetime,
                    IsIssuerInVAT = req.IsIssuerInVAT,
                    TaxFreeAmtSpecified = req.TaxFreeAmtSpecified,
                    TaxFreeAmt = req.TaxFreeAmt,
                    PriceWithoutVATSpecified = req.PriceWithoutVATSpecified,
                    PriceWithoutVAT = req.PriceWithoutVAT,
                    VATAmountSpecified = req.VATAmountSpecified,
                    VATAmount = req.VATAmount,
                    PriceWithVATSpecified = req.PriceWithVATSpecified,
                    PriceWithVAT = req.PriceWithVAT,
                    DeviceID = req.DeviceID,
                    NrLiferimit = req.NrLiferimit,
                    PaymentMethodTypeSTypeSpecified = req.PaymentMethodTypeSTypeSpecified,
                    PaymentMethodTypeSType = (PaymentMethodTypeSType)req.PaymentMethodTypeSType,
                    Buyer = new BuyerType { Address = req.Buyer.Address, Country = (CountryCodeSType)req.Buyer.Country, CountrySpecified = req.Buyer.CountrySpecified, IDNum = req.Buyer.IDNum, IDType = (IDTypeSType)req.Buyer.IDType, IDTypeSpecified = req.Buyer.IDTypeSpecified, Name = req.Buyer.Name, Town = req.Buyer.Town },
                    InvoiceItems = invoiceItemTypes,
                    CardNumber = req.CardNumber,
                    IsReverseCharge = req.IsReverseCharge,
                    IICRef = req.IICRef,
                    IssueDateTimeRef = req.IssueDateTimeRef,
                    TypeRefSpecified = req.TypeRefSpecified,
                    TypeRef = (CorrectiveInvTypeSType)req.TypeRef,
                    IsCorrectiveInv = req.IsCorrectiveInv,
                    MobileRefId = req.MobileRefId,
                    SubseqDelivTypeSType = req.SubseqDelivTypeSType
                };
                using (FiscalisationService fs = new FiscalisationService()) {
                    fs.Url = App.Instance.WebServerFiskalizimiUrl;

                    ResultLog log = fs.RegisterInvoice(request);
                    logpcl = new ResultLogPCL
                    {
                        Created = log.Created,
                        Request = log.Request,
                        Response = log.Response,
                        ResponseObject = log.ResponseObject,
                        Status = (StatusPCL)(int)log.Status,
                        RequestType = (RequestTypePCL)(int)log.RequestType,
                        Stacktrace = log.Stacktrace,
                        Message = log.Message,
                        MobileIdRef = log.MobileIdRef,
                        OperatorCode = log.OperatorCode,
                        QRCodeLink = log.QRCodeLink,
                        TCRBusinessCode = log.TCRBusinessCode,
                        NSLF = log.NSLF,
                        NIVF = log.NIVF,
                        ResponseUUID = log.ResponseUUID,
                        EIC = log.EIC,
                        ResponseUUIDSH = log.ResponseUUIDSH,
                        NIVFSH = log.NIVFSH,
                        NSLFSH = log.NSLFSH
                    };
                    return logpcl;
                }
            } 
            catch (Exception e) {
                return null;
            }
        }

        public int CheckCorrectiveInvoice(string invId, string DeviceID, string TCRCode, string OperatorCode, string BusinessUnitCode, string NIPT) {
            try {
                using (FiscalisationService fs = new FiscalisationService()) {
                    fs.Url = App.Instance.WebServerFiskalizimiUrl;

                    var res = fs.CheckCorrectiveInvoice(invId, DeviceID, TCRCode, OperatorCode, BusinessUnitCode, NIPT);
                    return res;
                }   
            }catch(Exception e) {
                return 0;
            }
        }

        public ResultLogPCL RegisterTCRInvoice(RegisterInvoiceInputRequestPCL req) {
            try {
                InvoiceItemType[] invoiceItemTypes = new InvoiceItemType[req.InvoiceItems.Count];
                ResultLogPCL logpcl = new ResultLogPCL();

                for (int i = 0; i < req.InvoiceItems.Count; i++) {
                    InvoiceItemType invoiceItemType = new InvoiceItemType();

                    if (req.InvoiceItems[i].VS != null) {
                        if (req.InvoiceItems[i].VS.VD != null) {
                            invoiceItemType.VS = new VouchersSoldType
                            {
                                VD = new FiskalizimiService.VoucherSoldDataType
                                {
                                    D = (DateTime)(req.InvoiceItems[i]?.VS?.VD?.D),
                                    N = (decimal)(req.InvoiceItems[i]?.VS?.VD?.N)
                                },
                            };
                        }

                    }
                    invoiceItemType.N = req.InvoiceItems[i].N;
                    invoiceItemType.C = req.InvoiceItems[i].C;
                    invoiceItemType.U = req.InvoiceItems[i].U;
                    invoiceItemType.Q = req.InvoiceItems[i].Q;
                    invoiceItemType.UPB = req.InvoiceItems[i].UPB;
                    invoiceItemType.UPA = req.InvoiceItems[i].UPA;
                    invoiceItemType.R = req.InvoiceItems[i].R;
                    invoiceItemType.RSpecified = req.InvoiceItems[i].RSpecified;
                    invoiceItemType.RR = req.InvoiceItems[i].RR;
                    invoiceItemType.RRSpecified = req.InvoiceItems[i].RRSpecified;
                    invoiceItemType.PB = req.InvoiceItems[i].PB;
                    invoiceItemType.VR = req.InvoiceItems[i].VR;
                    invoiceItemType.VRSpecified = req.InvoiceItems[i].VRSpecified;
                    invoiceItemType.VA = req.InvoiceItems[i].VA;
                    invoiceItemType.VASpecified = req.InvoiceItems[i].VASpecified;
                    invoiceItemType.IN = req.InvoiceItems[i].IN;
                    invoiceItemType.INSpecified = req.InvoiceItems[i].INSpecified;
                    invoiceItemType.PA = req.InvoiceItems[i].PA;
                    invoiceItemType.EX = (FiskalizimiService.ExemptFromVATSType)req.InvoiceItems[i].EX;
                    invoiceItemType.EXSpecified = req.InvoiceItems[i].EXSpecified;
                    invoiceItemTypes[i] = invoiceItemType;
                }
                RegisterInvoiceInputRequest request = new RegisterInvoiceInputRequest
                {
                    TCRCode = req.TCRCode,
                    OperatorCode = req.OperatorCode,
                    BusinessUnitCode = req.BusinessUnitCode,
                    InoiceTypeSpecified = req.InoiceTypeSpecified,
                    InoiceType = (InvoiceSType)req.InoiceType,
                    InvOrdNum = req.InvOrdNum,
                    InvNum = req.InvNum,
                    SendDatetime = req.SendDatetime,
                    IsIssuerInVAT = req.IsIssuerInVAT,
                    TaxFreeAmtSpecified = req.TaxFreeAmtSpecified,
                    TaxFreeAmt = req.TaxFreeAmt,
                    PriceWithoutVATSpecified = req.PriceWithoutVATSpecified,
                    PriceWithoutVAT = req.PriceWithoutVAT,
                    VATAmountSpecified = req.VATAmountSpecified,
                    VATAmount = req.VATAmount,
                    PriceWithVATSpecified = req.PriceWithVATSpecified,
                    PriceWithVAT = req.PriceWithVAT,
                    DeviceID = req.DeviceID,
                    NrLiferimit = req.NrLiferimit,
                    PaymentMethodTypeSTypeSpecified = req.PaymentMethodTypeSTypeSpecified,
                    PaymentMethodTypeSType = (PaymentMethodTypeSType)req.PaymentMethodTypeSType,
                    Buyer = new BuyerType { Address = req.Buyer.Address, Country = (CountryCodeSType)req.Buyer.Country, CountrySpecified = req.Buyer.CountrySpecified, IDNum = req.Buyer.IDNum, IDType = (IDTypeSType)req.Buyer.IDType, IDTypeSpecified = req.Buyer.IDTypeSpecified, Name = req.Buyer.Name, Town = req.Buyer.Town },
                    InvoiceItems = invoiceItemTypes,
                    CardNumber = req.CardNumber,
                    IsReverseCharge = req.IsReverseCharge,
                    IICRef = req.IICRef,
                    IssueDateTimeRef = req.IssueDateTimeRef,
                    TypeRefSpecified = req.TypeRefSpecified,
                    TypeRef = (CorrectiveInvTypeSType)req.TypeRef,
                    IsCorrectiveInv = req.IsCorrectiveInv,
                    MobileRefId = req.MobileRefId,
                    SubseqDelivTypeSType = req.SubseqDelivTypeSType
                };
                using (FiscalisationService fs = new FiscalisationService()) {
                    fs.Url = App.Instance.WebServerFiskalizimiUrl;

                    ResultLog log = fs.RegisterInvoice(request);
                    logpcl = new ResultLogPCL
                    {
                        Created = log.Created,
                        Request = log.Request,
                        Response = log.Response,
                        ResponseObject = log.ResponseObject,
                        Status = (StatusPCL)(int)log.Status,
                        RequestType = (RequestTypePCL)(int)log.RequestType,
                        Stacktrace = log.Stacktrace,
                        Message = log.Message,
                        MobileIdRef = log.MobileIdRef,
                        OperatorCode = log.OperatorCode,
                        QRCodeLink = log.QRCodeLink,
                        TCRBusinessCode = log.TCRBusinessCode,
                        NSLF = log.NSLF,
                        NIVF = log.NIVF,
                        ResponseUUID = log.ResponseUUID,
                        EIC = log.EIC,
                        ResponseUUIDSH = log.ResponseUUIDSH,
                        NIVFSH = log.NIVFSH,
                        NSLFSH = log.NSLFSH
                    };
                    return logpcl;
                }
            }
            catch (Exception e) {
                return null;
            }
        }

        public ResultLogPCL RegisterWTN(RegisterWTNInputRequestPCL req) {
            ResultLog resultlog = new ResultLog();
            ResultLogPCL logpcl = new ResultLogPCL();
            resultlog.Status = Status.InternalError;
            try {
                using (FiscalisationService fs = new FiscalisationService()) {
                    fs.Url = App.Instance.WebServerFiskalizimiUrl;

                    RegisterWTNInputRequest request = new RegisterWTNInputRequest
                    {
                        TCRCode = req.TCRCode,
                        OperatorCode = req.OperatorCode,
                        BusinessUnitCode = req.BusinessUnitCode,
                        DeviceID = req.DeviceID,
                        InvOrdNum = req.InvOrdNum,
                        InvNum = req.InvNum,
                        SendDatetime = req.SendDatetime,
                        ValueOfGoods = req.ValueOfGoods,
                        StartPointSTypeSpecified = req.StartPointSTypeSpecified,
                        StartPointSType = (FiskalizimiService.WTNStartPointSType)req.StartPointSType,
                        DestinPointSTypeSpecified = req.DestinPointSTypeSpecified,
                        DestinPointSType = (FiskalizimiService.WTNDestinPointSType)req.DestinPointSType,
                        MobileRefId = req.MobileRefId,
                        SubseqDelivTypeSType = req.SubseqDelivTypeSType,
                        FromDeviceId = req.FromDeviceId,
                        ToDeviceId = req.ToDeviceId
                    };
                    request.TransferItems = new WTNItemType[req.TransferItems.Count];
                    for (int i = 0; i < req.TransferItems.Count; i++) {
                        request.TransferItems[i] = new WTNItemType
                        {
                            C = req.TransferItems[i].C,
                            N = req.TransferItems[i].N,
                            Q = req.TransferItems[i].Q,
                            U = req.TransferItems[i].U,
                        };

                    }
                    ResultLog log = fs.RegisterWTN(request);

                    logpcl = new ResultLogPCL
                    {
                        Created = log.Created,
                        Request = log.Request,
                        Response = log.Response,
                        ResponseObject = log.ResponseObject,
                        Status = (StatusPCL)(int)log.Status,
                        RequestType = (RequestTypePCL)(int)log.RequestType,
                        Stacktrace = log.Stacktrace,
                        Message = log.Message,
                        MobileIdRef = log.MobileIdRef,
                        OperatorCode = log.OperatorCode,
                        QRCodeLink = log.QRCodeLink,
                        TCRBusinessCode = log.TCRBusinessCode,
                        NSLF = log.NSLF,
                        NIVF = log.NIVF,
                        ResponseUUID = log.ResponseUUID,
                        EIC = log.EIC,
                        ResponseUUIDSH = log.ResponseUUIDSH,
                        NIVFSH = log.NIVFSH,
                        NSLFSH = log.NSLFSH
                    };
                    return logpcl;
                }
            }catch(Exception e) {
                return null;
            }
        }
    }
}