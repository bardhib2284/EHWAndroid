using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class Depot {
        [PrimaryKey]
        public int IDDepo { get; set; }
        public string Depo { get; set; }
        public string TAGNR { get; set; }
        public string NIPT { get; set; }
        public string NIPTCERTIFIKATE { get; set; }
        public string TARGE { get; set; }
        public string SN { get; set; }
        public string ADRESA { get; set; }
    }
}
