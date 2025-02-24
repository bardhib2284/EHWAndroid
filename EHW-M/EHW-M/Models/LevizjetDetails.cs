using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class LevizjetDetails {
        [JsonIgnore]
        [PrimaryKey,AutoIncrement] 
        public int ID { get; set; }
        public string NumriLevizjes { get; set; }
        public string IDArtikulli { get; set; }
        public decimal? Sasia { get; set; }
        public decimal? Cmimi { get; set; }
        public string Njesia_matese { get; set; }
        public decimal? Totali { get; set; }
        public int? SyncStatus { get; set; }
        public string Artikulli { get; set; }
        public string Seri { get; set; }
        public int? TCRSyncStatus { get; set; }
        public string Depo { get; set; }
    }
}