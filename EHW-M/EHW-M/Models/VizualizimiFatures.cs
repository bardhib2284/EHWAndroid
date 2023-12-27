using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class VizualizimiFatures {

        // UI 
        public string Kontakt { get; set; }
        public string Klienti { get; set; }
        public DateTime Data { get; set; }
        public string NrFat { get; set; }
        public decimal Totali { get; set; }
        public int NrFisk { get; set; }
        public Guid IDLiferimi { get; set; }
        public Guid IDPorosia { get; set; }
        public Guid IDVizita { get; set; }
        public string IDKlienti { get; set; }
        public string IDKlientDheLokacion { get; set; }

        public List<Artikulli> ListaEArtikujve { get; set; }
    }
}
