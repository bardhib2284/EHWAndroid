using Newtonsoft.Json;
using SQLite;
using System;
namespace EHWM.Models {
    public class Liferimi : Table{
        [PrimaryKey]
        public Guid IDLiferimi { get; set; }
        public DateTime DataLiferuar { get; set; }
        public DateTime KohaLiferuar { get; set; }
        public string TitulliLiferimit { get; set; }
        public DateTime DataLiferimit { get; set; }
        public DateTime KohaLiferimit { get; set; }
        public Guid IDPorosia { get; set; }
        public byte Liferuar { get; set; }
        public string NrLiferimit { get; set; }
        public Single CmimiTotal { get; set; }
        public string DeviceID { get; set; }
        public int SyncStatus { get; set; }
        public string NrFatures { get; set; }
        public Single ShumaPaguar { get; set; }
        public bool Aprovuar { get; set; }
        [JsonProperty("exportStatus")]
        public int Export_Status { get; set; }
        public string LLOJDOK { get; set; }
        public string PayType { get; set; }
        public Single TotaliPaTVSH { get; set; }
        public int NrDetalet { get; set; }
        public string IDKlienti { get; set; }
        public string Depo { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public int? TCRSyncStatus { get; set; }
        public DateTime TCRIssueDateTime { get; set; }
        public string TCRQRCodeLink { get; set; }
        public string IDKthimi { get; set; }
        public string TCRBusinessCode { get; set; }
        public string TCRNSLF { get; set; }
        public string TCRNIVF { get; set; }
        public string TCR { get; set; }
        public int NumriFisk { get; set; }
        public string TCROperatorCode { get; set; }
        public string UUID { get; set; }
        public string EIC { get; set; }
        public string KODREASON { get; set; }
        public string NrdShoq { get; set; }
        public string PROCES { get; set; }
        public string TIPDOK { get; set; }
        public string Message { get; set; }
        [JsonIgnore]
        public int NrPorosis { get; set; }
        public override string TableName() {
            return "Liferimi";
        }
    }
}
