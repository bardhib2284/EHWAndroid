using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class LevizjetHeader {
        [PrimaryKey]
        public Guid TransferID { get; set; }
        public string NumriLevizjes { get; set; }
        public string LevizjeNga { get; set; }
        public string LevizjeNe { get; set; }
        public DateTime? Data { get; set; }
        public decimal? Totali { get; set; }
        public string IDAgjenti { get; set; }
        public int? SyncStatus { get; set; }
        public string KodiDyqanit { get; set; }
        public string NumriDaljes { get; set; }
        public string Depo { get; set; }
        public int? ImpStatus { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public int? TCRSyncStatus { get; set; }
        public DateTime? TCRIssueDateTime { get; set; }
        public string TCRQRCodeLink { get; set; }
        public int? NumriFisk { get; set; }
        public string TCR { get; set; }
        public string TCROperatorCode { get; set; }
        public string TCRBusinessUnitCode { get; set; }
        public string UUID { get; set; }
        public string TCRNSLFSH { get; set; }
        public string TCRNIVFSH { get; set; }
        public string Message { get; set; }
    }
}