using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class OrderDetails {
        [JsonIgnore]
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string IDOrder { get; set; }
        public int? NrRendor { get; set; }
        public string IDArtikulli { get; set; }
        public string Emri { get; set; }
        public float? SasiaPorositur { get; set; }
        public int? SyncStatus { get; set; }
        public int? ImpStatus { get; set; }
    }
}