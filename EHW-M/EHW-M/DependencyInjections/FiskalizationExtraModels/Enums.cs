using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.DependencyInjections.FiskalizationExtraModels {

    public enum StatusPCL {

        /// <remarks/>
        Ok,

        /// <remarks/>
        FaultCode,

        /// <remarks/>
        CertificateNotValid,

        /// <remarks/>
        InternalError,

        /// <remarks/>
        TCRAlreadyRegistered,

        /// <remarks/>
        InProcess,
    }

    public enum CashDepositOperationSTypePCL {

        /// <remarks/>
        INITIAL,

        /// <remarks/>
        WITHDRAW,

        /// <remarks/>
        DEPOSIT,
    }

    public enum RequestTypePCL {

        /// <remarks/>
        Fault,

        /// <remarks/>
        Undefiend,

        /// <remarks/>
        RegisterTCR,

        /// <remarks/>
        RegisterCashDeposit,

        /// <remarks/>
        RegisterInvoice,

        /// <remarks/>
        RegisterWTN,

        /// <remarks/>
        RegisterEinvoice,
    }

    public enum PaymentMethodTypeSTypePCL {

        /// <remarks/>
        BANKNOTE,

        /// <remarks/>
        CARD,

        /// <remarks/>
        CHECK,

        /// <remarks/>
        SVOUCHER,

        /// <remarks/>
        COMPANY,

        /// <remarks/>
        ORDER,

        /// <remarks/>
        ACCOUNT,

        /// <remarks/>
        FACTORING,

        /// <remarks/>
        COMPENSATION,

        /// <remarks/>
        TRANSFER,

        /// <remarks/>
        WAIVER,

        /// <remarks/>
        KIND,

        /// <remarks/>
        OTHER,
    }

    public enum InvoiceSTypePCL {

        /// <remarks/>
        CASH,

        /// <remarks/>
        NONCASH,
    }

    public enum CorrectiveInvTypeSTypePCL {

        /// <remarks/>
        CORRECTIVE,

        /// <remarks/>
        DEBIT,

        /// <remarks/>
        CREDIT,
    }
}
