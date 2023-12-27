using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class EvidencaPagesave {
        public string NrFatures { get; set; }
        public Single? ShumaTotale { get; set; }
        public Single? ShumaPaguar { get; set; }
        public Single? Borxhi { get; set; }
        public DateTime? DataPageses { get; set; }
        public DateTime? DataPerPagese { get; set; }
        public string IDKlienti { get; set; }
        public string IDAgjenti { get; set; }
        public string DeviceID { get; set; }
        public int? SyncStatus { get; set; }
        public string KMON { get; set; }
        [PrimaryKey]
        public string NrPageses { get; set; }
        public int? ExportStatus { get; set; }
        public string PayType { get; set; }
    }
}