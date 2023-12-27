using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.DependencyInjections.FiskalizationExtraModels {
    public partial class ResultLogPCL {

        private System.DateTime createdField;

        private string requestField;

        private string responseField;

        private string responseObjectField;

        private StatusPCL statusField;

        private RequestTypePCL requestTypeField;

        private string stacktraceField;

        private string messageField;

        private string mobileIdRefField;

        private string operatorCodeField;

        private string qRCodeLinkField;

        private string tCRBusinessCodeField;

        private string nSLFField;

        private string nIVFField;

        private string responseUUIDField;

        private string eICField;

        private string responseUUIDSHField;

        private string nIVFSHField;

        private string nSLFSHField;

        /// <remarks/>
        public System.DateTime Created {
            get {
                return this.createdField;
            }
            set {
                this.createdField = value;
            }
        }

        /// <remarks/>
        public string Request {
            get {
                return this.requestField;
            }
            set {
                this.requestField = value;
            }
        }

        /// <remarks/>
        public string Response {
            get {
                return this.responseField;
            }
            set {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public string ResponseObject {
            get {
                return this.responseObjectField;
            }
            set {
                this.responseObjectField = value;
            }
        }

        /// <remarks/>
        public StatusPCL Status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public RequestTypePCL RequestType {
            get {
                return this.requestTypeField;
            }
            set {
                this.requestTypeField = value;
            }
        }

        /// <remarks/>
        public string Stacktrace {
            get {
                return this.stacktraceField;
            }
            set {
                this.stacktraceField = value;
            }
        }

        /// <remarks/>
        public string Message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
            }
        }

        /// <remarks/>
        public string MobileIdRef {
            get {
                return this.mobileIdRefField;
            }
            set {
                this.mobileIdRefField = value;
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
        public string QRCodeLink {
            get {
                return this.qRCodeLinkField;
            }
            set {
                this.qRCodeLinkField = value;
            }
        }

        /// <remarks/>
        public string TCRBusinessCode {
            get {
                return this.tCRBusinessCodeField;
            }
            set {
                this.tCRBusinessCodeField = value;
            }
        }

        /// <remarks/>
        public string NSLF {
            get {
                return this.nSLFField;
            }
            set {
                this.nSLFField = value;
            }
        }

        /// <remarks/>
        public string NIVF {
            get {
                return this.nIVFField;
            }
            set {
                this.nIVFField = value;
            }
        }

        /// <remarks/>
        public string ResponseUUID {
            get {
                return this.responseUUIDField;
            }
            set {
                this.responseUUIDField = value;
            }
        }

        /// <remarks/>
        public string EIC {
            get {
                return this.eICField;
            }
            set {
                this.eICField = value;
            }
        }

        /// <remarks/>
        public string ResponseUUIDSH {
            get {
                return this.responseUUIDSHField;
            }
            set {
                this.responseUUIDSHField = value;
            }
        }

        /// <remarks/>
        public string NIVFSH {
            get {
                return this.nIVFSHField;
            }
            set {
                this.nIVFSHField = value;
            }
        }

        /// <remarks/>
        public string NSLFSH {
            get {
                return this.nSLFSHField;
            }
            set {
                this.nSLFSHField = value;
            }
        }
    }

}
