using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class NumriFaturave {
        [PrimaryKey]
	    public int IDNumri { get; set; }
        public string KOD { get; set; }
        public int? NRKUFIP { get; set; }
        public int? NRKUFIS { get; set; }
        public int? NRKUFIPJT { get; set; }
        [JsonProperty("nrkufisjt")]
        public int? NRKUFISJT { get; set; }
        public DateTime DataBrezit { get; set; }
        public int? CurrNrFat { get; set; }
        public int? CurrNrFatJT { get; set; }
        public int? SyncStatus { get; set; }
        [JsonProperty("nrkufipD")]
        public int? NRKUFIP_D { get; set; }
        [JsonProperty("nrkufisD")]
        public int? NRKUFIS_D { get; set; }
        [JsonProperty("currNrFatD")]
        public int? CurrNrFat_D { get; set; }
    }
}