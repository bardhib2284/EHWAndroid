using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class Artikulli {
        public string IDArtikulli { get; set; }
        public string Emri { get; set; }
        //kurse cmimi mirret prej tabeles SalesPrice   select * from SalesPrice where ItemNo_ like (IDArtikulli)'%A3007%' and SalesCode like (IDKlientDheLokacion || IDKlient)'%K47075%' field UnitPrice
        public double? CmimiNjesi { get; set; }
        public int? SasiaPako { get; set; }
        //Sasia siduket mirret prej Tabeles stoqet
        public float? Sasia { get; set; }
        public float? CmimiPako { get; set; }
        public string Shifra { get; set; }
        public string Barkod { get; set; }
        public int? StokuAktual { get; set; }
        public int? TePorositur { get; set; }
        public int? Standard { get; set; }
        public string BUM { get; set; }
        public decimal? UPP { get; set; }
        public int? SyncStatus { get; set; }
        public string Seri { get; set; }
        public int? UnitPrice { get; set; }

        public double? CmimiTotal => Sasia * CmimiNjesi;

        public string CmimiTotalText => Sasia * CmimiNjesi + "€";

        public string ArsyejaEKthimit { get; set; }

    }
}
