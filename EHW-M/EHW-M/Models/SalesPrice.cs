using System;

namespace EHWM.Models {
    public class SalesPrice {
        public string ItemNo { get; set; }
        public int? SalesType { get; set; }
        public string SalesCode { get; set; }
        public DateTime? StartingDate { get; set; }
        public string CurrencyCode { get; set; }
        public string VariantCode { get; set; }
        public string UnitofMeasureCode { get; set; }
        public double? MinimumQuantity { get; set; }
        public double? UnitPrice { get; set; }
        public byte? PriceIncludesVAT { get; set; }
        public byte? AllowInvoiceDisc { get; set; }
        public string VATBus_PostingGr_Price { get; set; }
        public DateTime? EndingDate { get; set; }
        public byte? AllowLineDisc { get; set; }
        public int? SyncStatus { get; set; }
        public string Depo { get; set; }

        
    }
}
