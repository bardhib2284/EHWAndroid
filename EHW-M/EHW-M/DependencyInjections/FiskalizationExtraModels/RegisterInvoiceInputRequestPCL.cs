using EHWM.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.DependencyInjections.FiskalizationExtraModels {
    public class RegisterInvoiceInputRequestPCL {
        public string TCRCode { get; set; }

        public string OperatorCode { get; set; }

        public string BusinessUnitCode { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute]
        public bool InoiceTypeSpecified { get; set; }
        public InvoiceSTypePCL InoiceType { get; set; }

        public string InvOrdNum { get; set; }
        public string InvNum { get; set; }
        public DateTime SendDatetime { get; set; }
        public bool IsIssuerInVAT { get; set; }


        [System.Xml.Serialization.XmlIgnoreAttribute]
        public bool TaxFreeAmtSpecified { get; set; }
        public decimal TaxFreeAmt { get; set; }


        [System.Xml.Serialization.XmlIgnoreAttribute]
        public bool PriceWithoutVATSpecified { get; set; }
        public decimal PriceWithoutVAT { get; set; }


        [System.Xml.Serialization.XmlIgnoreAttribute]
        public bool VATAmountSpecified { get; set; }
        public decimal VATAmount { get; set; }


        [System.Xml.Serialization.XmlIgnoreAttribute]
        public bool PriceWithVATSpecified { get; set; }
        public decimal PriceWithVAT { get; set; }

        public string DeviceID { get; set; }

        public string NrLiferimit { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute]
        public bool PaymentMethodTypeSTypeSpecified { get; set; }
        public PaymentMethodTypeSTypePCL PaymentMethodTypeSType { get; set; }
        public BuyerType Buyer { get; set; }
        public List<InvoiceItemTypePCL> InvoiceItems { get; set; }
        public string CardNumber { get; set; } = "";
        public bool IsReverseCharge { get; set; } = false;
        public string IICRef { get; set; } = "";
        public DateTime? IssueDateTimeRef { get; set; } = null;


        [System.Xml.Serialization.XmlIgnoreAttribute]
        public bool TypeRefSpecified { get; set; }
        public CorrectiveInvTypeSTypePCL TypeRef { get; set; } = CorrectiveInvTypeSTypePCL.CORRECTIVE;
        public bool IsCorrectiveInv { get; set; } = false;
        public string MobileRefId { get; set; } = "";
        public int SubseqDelivTypeSType { get; set; }
    }
}