using System;

namespace EHWM.Models {
    public class PorosiaArt {
        public string IDArtikulli { get; set; }
        public Single SasiaPorositur { get; set; }
        public float? CmimiAktual { get; set; }
        public Single Rabatet { get; set; }
        public Guid IDPorosia { get; set; }
        public Single SasiLiferuar { get; set; }
        public string Emri { get; set; }
        public Single Gratis { get; set; }
        public Single SasiaPako { get; set; }
        public string DeviceID { get; set; }
        public int SyncStatus { get; set; }
        public int IDArsyeja { get; set; }
        public Single CmimiPaTVSH { get; set; }
        public string BUM { get; set; }
        public string Seri { get; set; }
    }
}
