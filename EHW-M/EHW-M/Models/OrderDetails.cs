using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class OrderDetails {
        [PrimaryKey]
        public string IDOrder { get; set; }
        public int? NrRendor { get; set; }
        public string IDArtikulli { get; set; }
        public string Emri { get; set; }
        public Single? SasiaPorositur { get; set; }
        public int? SyncStatus { get; set; }
        public int? ImpStatus { get; set; }
    }
}