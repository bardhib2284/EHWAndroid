using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EHWM.Models {
    public class TCRLiferimi {
        public string IDLiferimi { get; set; }
        public double TotaliPaTVSH { get; set; }
        public double TotaliMeTVSH { get; set; }
        public double TVSH { get; set; }


        public string NrLiferimit { get; set; }
        public string DeviceID { get; set; }
        public string OperatorCode { get; set; }
        public string InvoiceSType { get; set; }
        public string InvNum { get; set; }
        public int InvOrdNum { get; set; }
        public double TaxFreeAmt { get; set; }
        public string MobileRefId { get; set; }
        public string IICRef { get; set; }
        public DateTime IssueDateTimeRef { get; set; }
        public string PaymentMethodTypesType { get; set; }
        public DateTime SendDatetime { get; set; }
        public string TypeRef { get; set; }
        public string CardNumber { get; set; }
        public bool IsCorrectiveInv { get; set; }
        public bool IsReverseCharge { get; set; }
        public bool IsIssuerInVAT { get; set; }

        public List<InvoiceItemTypePCL> Items { get; set; }
        public BuyerType Buyer { get; set; }
        public string LLOJDOK { get; set; }
        public bool IsShitje { get; set; }
        public string IDKthimi { get; set; }
    }

    #region DB QUERY MAPPERRS

    public class MapperHeader {
        public string IDLiferimi { get; set; }
        public double TotaliPaTVSH { get; set; }
        public double TotaliMeTVSH { get; set; }
        public double TVSH { get; set; }
    }

    public class MapperLines {
        public string IDLiferimi { get; set; }
        public string NrLiferimit { get; set; }
        public string DeviceID { get; set; }
        public string OperatorCode { get; set; }
        public string InvoiceSType { get; set; }
        public string InvNum { get; set; }
        public int InvOrdNum { get; set; }
        public double TaxFreeAmt { get; set; }
        public string MobileRefId { get; set; }
        public string IICRef { get; set; }
        public DateTime IssueDateTimeRef { get; set; }
        public string PaymentMethodTypesType { get; set; }
        public DateTime SendDatetime { get; set; }
        public string TypeRef { get; set; }
        public string CardNumber { get; set; }
        public bool IsCorrectiveInv { get; set; }
        public bool IsReverseCharge { get; set; }
        public bool IsIssuerInVAT { get; set; }
        public string LLOJDOK { get; set; }
        public string IDKthimi { get; set; }

        public string Buyer_IDNum { get; set; }
        public string Buyer_IDType { get; set; }
        public string Buyer_Name { get; set; }
        public string Buyer_Address { get; set; }
        public int Buyer_Country { get; set; }
        public string Buyer_Town { get; set; }


        public string Item_N { get; set; }
        public string Item_C { get; set; }
        public string Item_U { get; set; }
        public double Item_Q { get; set; }
        public double Item_UPB { get; set; }
        public double Item_UPA { get; set; }
        public double Item_PB { get; set; }
        public double Item_PA { get; set; }
        public double Item_VA { get; set; }
        public double Item_VR { get; set; }
    }

    #endregion



    #region MappClasses

    public class ClassMapper {

        public static List<TCRLiferimi> MapHeadersAndLines(List<MapperHeader> mapperHeaderList, List<MapperLines> mapperLinesList) {
            List<TCRLiferimi> result = new List<TCRLiferimi>();

            foreach (MapperHeader headervar in mapperHeaderList) {
                TCRLiferimi header = new TCRLiferimi();
                MapperLines line = mapperLinesList.FirstOrDefault(x => x.IDLiferimi == headervar.IDLiferimi);
                if (line != null) {
                    int sign = 1;
                    header.IDLiferimi = headervar.IDLiferimi;
                    header.NrLiferimit = line.NrLiferimit;
                    header.DeviceID = line.DeviceID;
                    header.CardNumber = line.CardNumber;
                    header.IICRef = line.IICRef;
                    header.InvoiceSType = line.InvoiceSType;
                    header.InvNum = line.InvNum;
                    header.InvOrdNum = line.InvOrdNum;
                    header.IsCorrectiveInv = line.IsCorrectiveInv;
                    header.IsIssuerInVAT = line.IsIssuerInVAT;
                    header.IsReverseCharge = line.IsIssuerInVAT;
                    header.IssueDateTimeRef = line.IssueDateTimeRef;
                    header.MobileRefId = line.MobileRefId;
                    header.OperatorCode = line.OperatorCode;
                    header.PaymentMethodTypesType = line.PaymentMethodTypesType;
                    header.SendDatetime = line.SendDatetime;
                    header.TaxFreeAmt = line.TaxFreeAmt;
                    header.TotaliMeTVSH = headervar.TotaliMeTVSH;
                    header.TotaliPaTVSH = headervar.TotaliPaTVSH;
                    header.TVSH = headervar.TVSH;
                    header.TypeRef = line.TypeRef;
                    header.LLOJDOK = line.LLOJDOK;
                    header.IDKthimi = line.IDKthimi;

                    if (header.TotaliMeTVSH <= 0) {
                        header.IsShitje = false;
                        sign = -1;
                    }
                    else {
                        header.IsShitje = true;
                    }

                    header.Buyer = new BuyerType();
                    header.Buyer.Address = line.Buyer_Address;
                    header.Buyer.Country = (CountryCodeSType)(line.Buyer_Country);
                    header.Buyer.IDNum = line.Buyer_IDNum;
                    header.Buyer.IDType = IDTypeSType.ID;
                    header.Buyer.Name = line.Buyer_Name;
                    header.Buyer.Town = line.Buyer_Town;

                    header.Items = new List<InvoiceItemTypePCL>();
                    foreach (MapperLines itemsvar in mapperLinesList.Where(x => x.IDLiferimi == headervar.IDLiferimi).ToList()) {
                        InvoiceItemTypePCL it = new InvoiceItemTypePCL();
                        it.C = itemsvar.Item_C;
                        it.N = itemsvar.Item_N;
                        it.PA = (decimal)(itemsvar.Item_PA);
                        it.PB = (decimal)(itemsvar.Item_PB);
                        it.Q = itemsvar.Item_Q;
                        it.U = itemsvar.Item_U;
                        it.UPB = (decimal)itemsvar.Item_UPB;
                        it.UPA = (decimal)itemsvar.Item_UPA;
                        it.VA = (decimal)itemsvar.Item_VA;
                        //it.VR = itemsvar.Item_C == "A3053" ? (decimal)0 : (itemsvar.Item_C == "A3230" ? (decimal)6 : (itemsvar.Item_C == "P111" ? (decimal)10 : (decimal)(itemsvar.Item_VR)));
                        it.VR = (decimal)(itemsvar.Item_VR);
                        it.VASpecified = true;
                        it.VRSpecified = true;
                        it.EXSpecified = true;
                        it.INSpecified = true;
                        it.RRSpecified = true;
                        it.RSpecified = true;
                        header.Items.Add(it);
                    }
                    result.Add(header);
                }
            }
            return result;
        }
    }

    #endregion

    public partial class BuyerType {

        private IDTypeSType iDTypeField;

        private bool iDTypeFieldSpecified;

        private string iDNumField;

        private string nameField;

        private string addressField;

        private string townField;

        private CountryCodeSType countryField;

        private bool countryFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public IDTypeSType IDType {
            get {
                return this.iDTypeField;
            }
            set {
                this.iDTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IDTypeSpecified {
            get {
                return this.iDTypeFieldSpecified;
            }
            set {
                this.iDTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string IDNum {
            get {
                return this.iDNumField;
            }
            set {
                this.iDNumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Address {
            get {
                return this.addressField;
            }
            set {
                this.addressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Town {
            get {
                return this.townField;
            }
            set {
                this.townField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public CountryCodeSType Country {
            get {
                return this.countryField;
            }
            set {
                this.countryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CountrySpecified {
            get {
                return this.countryFieldSpecified;
            }
            set {
                this.countryFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "https://eFiskalizimi.tatime.gov.al/FiscalizationService/schema")]
    public enum IDTypeSType {

        /// <remarks/>
        NUIS,

        /// <remarks/>
        ID,

        /// <remarks/>
        PASS,

        /// <remarks/>
        VAT,

        /// <remarks/>
        TAX,

        /// <remarks/>
        SOC,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "https://eFiskalizimi.tatime.gov.al/FiscalizationService/schema")]
    public enum CountryCodeSType {

        /// <remarks/>
        ABW,

        /// <remarks/>
        AFG,

        /// <remarks/>
        AGO,

        /// <remarks/>
        AIA,

        /// <remarks/>
        ALA,

        /// <remarks/>
        ALB,

        /// <remarks/>
        AND,

        /// <remarks/>
        ARE,

        /// <remarks/>
        ARG,

        /// <remarks/>
        ARM,

        /// <remarks/>
        ASM,

        /// <remarks/>
        ATA,

        /// <remarks/>
        ATF,

        /// <remarks/>
        ATG,

        /// <remarks/>
        AUS,

        /// <remarks/>
        AUT,

        /// <remarks/>
        AZE,

        /// <remarks/>
        BDI,

        /// <remarks/>
        BEL,

        /// <remarks/>
        BEN,

        /// <remarks/>
        BES,

        /// <remarks/>
        BFA,

        /// <remarks/>
        BGD,

        /// <remarks/>
        BGR,

        /// <remarks/>
        BHR,

        /// <remarks/>
        BHS,

        /// <remarks/>
        BIH,

        /// <remarks/>
        BLM,

        /// <remarks/>
        BLR,

        /// <remarks/>
        BLZ,

        /// <remarks/>
        BMU,

        /// <remarks/>
        BOL,

        /// <remarks/>
        BRA,

        /// <remarks/>
        BRB,

        /// <remarks/>
        BRN,

        /// <remarks/>
        BTN,

        /// <remarks/>
        BVT,

        /// <remarks/>
        BWA,

        /// <remarks/>
        CAF,

        /// <remarks/>
        CAN,

        /// <remarks/>
        CCK,

        /// <remarks/>
        CHE,

        /// <remarks/>
        CHL,

        /// <remarks/>
        CHN,

        /// <remarks/>
        CIV,

        /// <remarks/>
        CMR,

        /// <remarks/>
        COD,

        /// <remarks/>
        COG,

        /// <remarks/>
        COK,

        /// <remarks/>
        COL,

        /// <remarks/>
        COM,

        /// <remarks/>
        CPV,

        /// <remarks/>
        CRI,

        /// <remarks/>
        CUB,

        /// <remarks/>
        CUW,

        /// <remarks/>
        CXR,

        /// <remarks/>
        CYM,

        /// <remarks/>
        CYP,

        /// <remarks/>
        CZE,

        /// <remarks/>
        DEU,

        /// <remarks/>
        DJI,

        /// <remarks/>
        DMA,

        /// <remarks/>
        DNK,

        /// <remarks/>
        DOM,

        /// <remarks/>
        DZA,

        /// <remarks/>
        ECU,

        /// <remarks/>
        EGY,

        /// <remarks/>
        ERI,

        /// <remarks/>
        ESH,

        /// <remarks/>
        ESP,

        /// <remarks/>
        EST,

        /// <remarks/>
        ETH,

        /// <remarks/>
        FIN,

        /// <remarks/>
        FJI,

        /// <remarks/>
        FLK,

        /// <remarks/>
        FRA,

        /// <remarks/>
        FRO,

        /// <remarks/>
        FSM,

        /// <remarks/>
        GAB,

        /// <remarks/>
        GBR,

        /// <remarks/>
        GEO,

        /// <remarks/>
        GGY,

        /// <remarks/>
        GHA,

        /// <remarks/>
        GIB,

        /// <remarks/>
        GIN,

        /// <remarks/>
        GLP,

        /// <remarks/>
        GMB,

        /// <remarks/>
        GNB,

        /// <remarks/>
        GNQ,

        /// <remarks/>
        GRC,

        /// <remarks/>
        GRD,

        /// <remarks/>
        GRL,

        /// <remarks/>
        GTM,

        /// <remarks/>
        GUF,

        /// <remarks/>
        GUM,

        /// <remarks/>
        GUY,

        /// <remarks/>
        HKG,

        /// <remarks/>
        HMD,

        /// <remarks/>
        HND,

        /// <remarks/>
        HRV,

        /// <remarks/>
        HTI,

        /// <remarks/>
        HUN,

        /// <remarks/>
        IDN,

        /// <remarks/>
        IMN,

        /// <remarks/>
        IND,

        /// <remarks/>
        IOT,

        /// <remarks/>
        IRL,

        /// <remarks/>
        IRN,

        /// <remarks/>
        IRQ,

        /// <remarks/>
        ISL,

        /// <remarks/>
        ISR,

        /// <remarks/>
        ITA,

        /// <remarks/>
        JAM,

        /// <remarks/>
        JEY,

        /// <remarks/>
        JOR,

        /// <remarks/>
        JPN,

        /// <remarks/>
        KAZ,

        /// <remarks/>
        KEN,

        /// <remarks/>
        KGZ,

        /// <remarks/>
        KHM,

        /// <remarks/>
        KIR,

        /// <remarks/>
        KNA,

        /// <remarks/>
        KOR,

        /// <remarks/>
        KWT,

        /// <remarks/>
        LAO,

        /// <remarks/>
        LBN,

        /// <remarks/>
        LBR,

        /// <remarks/>
        LBY,

        /// <remarks/>
        LCA,

        /// <remarks/>
        LIE,

        /// <remarks/>
        LKA,

        /// <remarks/>
        LSO,

        /// <remarks/>
        LTU,

        /// <remarks/>
        LUX,

        /// <remarks/>
        LVA,

        /// <remarks/>
        MAC,

        /// <remarks/>
        MAF,

        /// <remarks/>
        MAR,

        /// <remarks/>
        MCO,

        /// <remarks/>
        MDA,

        /// <remarks/>
        MDG,

        /// <remarks/>
        MDV,

        /// <remarks/>
        MEX,

        /// <remarks/>
        MHL,

        /// <remarks/>
        MKD,

        /// <remarks/>
        MLI,

        /// <remarks/>
        MLT,

        /// <remarks/>
        MMR,

        /// <remarks/>
        MNE,

        /// <remarks/>
        MNG,

        /// <remarks/>
        MNP,

        /// <remarks/>
        MOZ,

        /// <remarks/>
        MRT,

        /// <remarks/>
        MSR,

        /// <remarks/>
        MTQ,

        /// <remarks/>
        MUS,

        /// <remarks/>
        MWI,

        /// <remarks/>
        MYS,

        /// <remarks/>
        MYT,

        /// <remarks/>
        NAM,

        /// <remarks/>
        NCL,

        /// <remarks/>
        NER,

        /// <remarks/>
        NFK,

        /// <remarks/>
        NGA,

        /// <remarks/>
        NIC,

        /// <remarks/>
        NIU,

        /// <remarks/>
        NLD,

        /// <remarks/>
        NOR,

        /// <remarks/>
        NPL,

        /// <remarks/>
        NRU,

        /// <remarks/>
        NZL,

        /// <remarks/>
        OMN,

        /// <remarks/>
        PAK,

        /// <remarks/>
        PAN,

        /// <remarks/>
        PCN,

        /// <remarks/>
        PER,

        /// <remarks/>
        PHL,

        /// <remarks/>
        PLW,

        /// <remarks/>
        PNG,

        /// <remarks/>
        POL,

        /// <remarks/>
        PRI,

        /// <remarks/>
        PRK,

        /// <remarks/>
        PRT,

        /// <remarks/>
        PRY,

        /// <remarks/>
        PSE,

        /// <remarks/>
        PYF,

        /// <remarks/>
        QAT,

        /// <remarks/>
        REU,

        /// <remarks/>
        ROU,

        /// <remarks/>
        RUS,

        /// <remarks/>
        RWA,

        /// <remarks/>
        SAU,

        /// <remarks/>
        SDN,

        /// <remarks/>
        SEN,

        /// <remarks/>
        SGP,

        /// <remarks/>
        SGS,

        /// <remarks/>
        SHN,

        /// <remarks/>
        SJM,

        /// <remarks/>
        SLB,

        /// <remarks/>
        SLE,

        /// <remarks/>
        SLV,

        /// <remarks/>
        SMR,

        /// <remarks/>
        SOM,

        /// <remarks/>
        SPM,

        /// <remarks/>
        SRB,

        /// <remarks/>
        SSD,

        /// <remarks/>
        STP,

        /// <remarks/>
        SUR,

        /// <remarks/>
        SVK,

        /// <remarks/>
        SVN,

        /// <remarks/>
        SWE,

        /// <remarks/>
        SWZ,

        /// <remarks/>
        SXM,

        /// <remarks/>
        SYC,

        /// <remarks/>
        SYR,

        /// <remarks/>
        TCA,

        /// <remarks/>
        TCD,

        /// <remarks/>
        TGO,

        /// <remarks/>
        THA,

        /// <remarks/>
        TJK,

        /// <remarks/>
        TKL,

        /// <remarks/>
        TKM,

        /// <remarks/>
        TLS,

        /// <remarks/>
        TON,

        /// <remarks/>
        TTO,

        /// <remarks/>
        TUN,

        /// <remarks/>
        TUR,

        /// <remarks/>
        TUV,

        /// <remarks/>
        TWN,

        /// <remarks/>
        TZA,

        /// <remarks/>
        UGA,

        /// <remarks/>
        UKR,

        /// <remarks/>
        UMI,

        /// <remarks/>
        RKS,

        /// <remarks/>
        URY,

        /// <remarks/>
        USA,

        /// <remarks/>
        UZB,

        /// <remarks/>
        VAT,

        /// <remarks/>
        VCT,

        /// <remarks/>
        VEN,

        /// <remarks/>
        VGB,

        /// <remarks/>
        VIR,

        /// <remarks/>
        VNM,

        /// <remarks/>
        VUT,

        /// <remarks/>
        WLF,

        /// <remarks/>
        WSM,

        /// <remarks/>
        YEM,

        /// <remarks/>
        ZAF,

        /// <remarks/>
        ZMB,

        /// <remarks/>
        ZWE,
    }

    public partial class InvoiceItemTypePCL {

        private VouchersSoldType vsField;

        private string nField;

        private string cField;

        private string uField;

        private double qField;

        private decimal uPBField;

        private decimal uPAField;

        private decimal rField;

        private bool rFieldSpecified;

        private bool rrField;

        private bool rrFieldSpecified;

        private decimal pbField;

        private decimal vrField;

        private bool vrFieldSpecified;

        private decimal vaField;

        private bool vaFieldSpecified;

        private bool inField;

        private bool inFieldSpecified;

        private decimal paField;

        private ExemptFromVATSType exField;

        private bool exFieldSpecified;

        /// <remarks/>
        public VouchersSoldType VS {
            get {
                return this.vsField;
            }
            set {
                this.vsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string N {
            get {
                return this.nField;
            }
            set {
                this.nField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string C {
            get {
                return this.cField;
            }
            set {
                this.cField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string U {
            get {
                return this.uField;
            }
            set {
                this.uField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double Q {
            get {
                return this.qField;
            }
            set {
                this.qField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal UPB {
            get {
                return this.uPBField;
            }
            set {
                this.uPBField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal UPA {
            get {
                return this.uPAField;
            }
            set {
                this.uPAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal R {
            get {
                return this.rField;
            }
            set {
                this.rField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RSpecified {
            get {
                return this.rFieldSpecified;
            }
            set {
                this.rFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool RR {
            get {
                return this.rrField;
            }
            set {
                this.rrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RRSpecified {
            get {
                return this.rrFieldSpecified;
            }
            set {
                this.rrFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal PB {
            get {
                return this.pbField;
            }
            set {
                this.pbField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal VR {
            get {
                return this.vrField;
            }
            set {
                this.vrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VRSpecified {
            get {
                return this.vrFieldSpecified;
            }
            set {
                this.vrFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal VA {
            get {
                return this.vaField;
            }
            set {
                this.vaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VASpecified {
            get {
                return this.vaFieldSpecified;
            }
            set {
                this.vaFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool IN {
            get {
                return this.inField;
            }
            set {
                this.inField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool INSpecified {
            get {
                return this.inFieldSpecified;
            }
            set {
                this.inFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal PA {
            get {
                return this.paField;
            }
            set {
                this.paField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ExemptFromVATSType EX {
            get {
                return this.exField;
            }
            set {
                this.exField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EXSpecified {
            get {
                return this.exFieldSpecified;
            }
            set {
                this.exFieldSpecified = value;
            }
        }
    }

    public enum ExemptFromVATSType {

        /// <remarks/>
        TYPE_1,

        /// <remarks/>
        TYPE_2,

        /// <remarks/>
        TAX_FREE,

        /// <remarks/>
        MARGIN_SCHEME,

        /// <remarks/>
        EXPORT_OF_GOODS,
    }
    public partial class VoucherSoldDataType {

        private System.DateTime dField;

        private decimal nField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime D {
            get {
                return this.dField;
            }
            set {
                this.dField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal N {
            get {
                return this.nField;
            }
            set {
                this.nField = value;
            }
        }
    }
    public partial class VoucherType {

        private string numField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Num {
            get {
                return this.numField;
            }
            set {
                this.numField = value;
            }
        }
    }
    public partial class VouchersSoldType {

        private VoucherSoldDataType vdField;

        private VoucherType[] vnField;

        /// <remarks/>
        public VoucherSoldDataType VD {
            get {
                return this.vdField;
            }
            set {
                this.vdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("V", IsNullable = false)]
        public VoucherType[] VN {
            get {
                return this.vnField;
            }
            set {
                this.vnField = value;
            }
        }
    }
}
