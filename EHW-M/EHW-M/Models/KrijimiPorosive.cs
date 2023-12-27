using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class KrijimiPorosive {
        public string IDKlienti { get; set; }
        public string Emri { get; set; }
        public string IDAgjenti { get; set; }
        public DateTime? Data_Regjistrimit { get; set; }
        public int? Imp_Status { get; set; }
        [PrimaryKey]
        public Guid KPID { get; set; }
        public string Depo { get; set; }
        public string DeviceID { get; set; }
        public int? SyncStatus { get; set; }

    }
}
