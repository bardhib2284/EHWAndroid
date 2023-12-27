using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class Vendet {
        [PrimaryKey]
        public int IDVendi { get; set; }
        public string NrPostal { get; set; }
        public string Qyteti { get; set; }
        public int? SyncStatus { get; set; }
        public string Kod_Qyteti { get; set; }
    }
}
