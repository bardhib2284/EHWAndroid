using SQLite;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace EHWM.Models {
    public interface ITable {
        string TableName();
    }

    public class Table : ITable {
        public virtual string TableName() {
            return "Table";
        }
    }

    public class Agjendet  {
        [PrimaryKey]
        public string IDAgjenti { get; set; }
        public string Emri { get; set; }
        public string Mbiemri { get; set; }
        public string Perdoruesti { get; set; }
        public string Fjalekalimi { get; set; }
        public string Gjendja { get; set; }
        public string DeviceID { get; set; }
        public string Depo { get; set; }
        public int? SyncStatus { get; set; }
        public int? AprovimFaturash { get; set; }
        public int? ID_User_Group { get; set; }
        public string MeAprovim { get; set; }
        public string PaAprovim { get; set; }
        public string KOD_AgjentMarketingu { get; set; }
        public string TCRCode { get; set; }
        public DateTime? TCRRegisteredDateTime { get; set; }
        public int? TCRGeneratedId { get; set; }
        public string OperatorCode { get; set; }
        public string token { get; set; }

    }

    public class AgjendiLogin {
        public string idagjenti { get; set; }
        public string perdoruesi { get; set; }
    }
}
