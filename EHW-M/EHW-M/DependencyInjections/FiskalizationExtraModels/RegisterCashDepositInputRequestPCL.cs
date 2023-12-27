using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.DependencyInjections.FiskalizationExtraModels {
    public partial class RegisterCashDepositInputRequestPCL {

        private decimal cashAmountField;

        private CashDepositOperationSTypePCL depositTypeField;

        private string tCRCodeField;

        private string operatorCodeField;

        private int subseqDelivTypeSTypeField;

        private System.DateTime sendDateTimeField;

        /// <remarks/>
        public decimal CashAmount {
            get {
                return this.cashAmountField;
            }
            set {
                this.cashAmountField = value;
            }
        }

        /// <remarks/>
        public CashDepositOperationSTypePCL DepositType {
            get {
                return this.depositTypeField;
            }
            set {
                this.depositTypeField = value;
            }
        }

        /// <remarks/>
        public string TCRCode {
            get {
                return this.tCRCodeField;
            }
            set {
                this.tCRCodeField = value;
            }
        }

        /// <remarks/>
        public string OperatorCode {
            get {
                return this.operatorCodeField;
            }
            set {
                this.operatorCodeField = value;
            }
        }

        /// <remarks/>
        public int SubseqDelivTypeSType {
            get {
                return this.subseqDelivTypeSTypeField;
            }
            set {
                this.subseqDelivTypeSTypeField = value;
            }
        }

        /// <remarks/>
        public System.DateTime SendDateTime {
            get {
                return this.sendDateTimeField;
            }
            set {
                this.sendDateTimeField = value;
            }
        }
    }
}
