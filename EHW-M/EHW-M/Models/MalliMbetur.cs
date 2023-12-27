using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class Malli_Mbetur {
        public string IDArtikulli { get; set; }
        public string Emri { get; set; }
        public float SasiaPranuar { get; set; }
        public float SasiaShitur { get; set; }
        public float SasiaKthyer { get; set; }
        public float SasiaMbetur { get; set; }
        public int? NrDoc { get; set; }
        public int? SyncStatus { get; set; }
        public DateTime Data { get; set; }
        public string LLOJDOK { get; set; }
        public string Depo { get; set; }
        [JsonProperty("exportStatus")]
        public int? Export_Status { get; set; }
        public string PKeyIDArtDepo { get; set; }
        [JsonIgnore]
        [PrimaryKey,AutoIncrement]
        public int? ID { get; set; }
        public Single LevizjeStoku { get; set; }
        public string Seri { get; set; }
    }
}
