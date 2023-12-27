using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
        public class Stoqet {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }
            public string Shifra { get; set; }
            public string Depo { get; set; }
            public string NjesiaMatse { get; set; }
            public double? Paketimi { get; set; }
            public double? Sasia { get; set; }
            public double? Paketa { get; set; }
            public int? SyncStatus { get; set; }
            public int? Dhurate { get; set; }
            public int? NRDOK { get; set; }
            public string LLOJDOK { get; set; }
            public string Seri { get; set; }
    }
}
