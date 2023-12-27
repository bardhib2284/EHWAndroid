using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.DependencyInjections.FiskalizationExtraModels
{
    public class RegisterWTNInputRequestPCL
    {
        public string TCRCode { get; set; }
        public string OperatorCode { get; set; }
        public string BusinessUnitCode { get; set; }
        public string DeviceID { get; set; }
        public string InvOrdNum { get; set; }
        public string InvNum { get; set; }
        public DateTime SendDatetime { get; set; }
        public decimal ValueOfGoods { get; set; }
        public bool StartPointSTypeSpecified { get; set; }
        public WTNStartPointSType StartPointSType { get; set; }
        public bool DestinPointSTypeSpecified { get; set; }
        public WTNDestinPointSType DestinPointSType { get; set; }
        public List<WTNItemTypePCL> TransferItems { get; set; }
        public string MobileRefId { get; set; } = "";
        public int SubseqDelivTypeSType { get; set; }
        public string FromDeviceId { get; set; } = "";
        public string ToDeviceId { get; set; } = "";
    }

    public partial class WTNItemTypePCL : object, System.ComponentModel.INotifyPropertyChanged {

        private string nField;

        private string cField;

        private string uField;

        private double qField;

        public string N {
            get {
                return this.nField;
            }
            set {
                this.nField = value;
                this.RaisePropertyChanged("N");
            }
        }

        public string C {
            get {
                return this.cField;
            }
            set {
                this.cField = value;
                this.RaisePropertyChanged("C");
            }
        }

        public string U {
            get {
                return this.uField;
            }
            set {
                this.uField = value;
                this.RaisePropertyChanged("U");
            }
        }

        public double Q {
            get {
                return this.qField;
            }
            set {
                this.qField = value;
                this.RaisePropertyChanged("Q");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    public enum WTNDestinPointSType {

        /// <remarks/>
        WAREHOUSE,

        /// <remarks/>
        EXHIBITION,

        /// <remarks/>
        STORE,

        /// <remarks/>
        SALE,

        /// <remarks/>
        OTHER,
    }

    public enum WTNStartPointSType {

        /// <remarks/>
        WAREHOUSE,

        /// <remarks/>
        EXHIBITION,

        /// <remarks/>
        STORE,

        /// <remarks/>
        SALE,

        /// <remarks/>
        ANOTHER,

        /// <remarks/>
        CUSTOMS,

        /// <remarks/>
        OTHER,
    }
}
