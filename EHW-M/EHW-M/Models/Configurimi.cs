using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class Configurimi {
        [PrimaryKey]
        public int ID { get; set; }
        public string Paisja { get; set; }
        public string KodiTCR { get; set; }
        public string TAGNR { get; set; }
        public string KodiIOperatorit { get; set; }
        public string KodiINjesiseSeBiznesit { get; set; }
        public string PortiPerPrintim { get; set; }
        public string Serveri { get; set; }
        public string Databaza { get; set; }
        public string Shfrytezuesi { get; set; }
        public string Fjalekalimi { get; set; }
        public string URLFotoWebServer { get; set; }
        public string URLFiskalizim { get; set; }
        public string Token { get; set; }
        public bool VetemPerPorosi { get; set; }
    }
}
