using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class LogSync_Session {
        public string DeviceID { get; set; }
        [PrimaryKey]
        public Guid SessionID { get; set; }
        public DateTime? StartSessionDT { get; set; }
        public DateTime? EndSessionDT { get; set; }
        public string TableName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int? SyncDir { get; set; }
        public int? NrDownloadedRecs { get; set; }
        public int? NrRecsUpdate { get; set; }
        public int? NrRecsInsert { get; set; }
        public int? NrUploadedRecs { get; set; }

    }
}
