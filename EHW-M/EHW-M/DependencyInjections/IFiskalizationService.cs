using EHWM.DependencyInjections.FiskalizationExtraModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.DependencyInjections {
    public interface IFiskalizationService {

        ResultLogPCL RegisterCashDeposit(RegisterCashDepositInputRequestPCL req);
        ResultLogPCL RegisterInvoice(RegisterInvoiceInputRequestPCL req);
        ResultLogPCL RegisterTCRInvoice(RegisterInvoiceInputRequestPCL req);
        int CheckCorrectiveInvoice(string invId, string DeviceID, string TCRCode, string OperatorCode, string BusinessUnitCode, string NIPT);
        ResultLogPCL RegisterWTN(RegisterWTNInputRequestPCL req);
    }
}