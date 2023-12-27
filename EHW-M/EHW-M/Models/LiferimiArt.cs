using Newtonsoft.Json;
using SQLite;
using System;
namespace EHWM.Models {
    public class LiferimiArt {
        [JsonIgnore]
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }
        public Guid IDLiferimi { get; set; }
        public string IDArtikulli { get; set; }
        public Single Cmimi { get; set; }
        public Single SasiaLiferuar { get; set; }
        public Single SasiaPorositur { get; set; }
        public string ArtEmri { get; set; }
        public Single Totali { get; set; }
        public string DeviceID { get; set; }
        public Single Gratis { get; set; }
        public int SyncStatus { get; set; }
        public int IDArsyeja { get; set; }
        public int IDKthimi { get; set; }
        public Single CmimiPaTVSH { get; set; }
        public Single TotaliPaTVSH { get; set; }
        public Single VlefteTVSH { get; set; }
        public string Seri { get; set; }
        public int TCRSyncStatus { get; set; }
    }
}
