using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class Vizita : IComparable<Vizita> {
        [PrimaryKey]
        public Guid IDVizita { get; set; }
        public DateTime? DataPlanifikimit { get; set; }
        public DateTime? OraPlanifikimit { get; set; }
        public DateTime? DataAritjes { get; set; }
        public DateTime? OraArritjes { get; set; }
        public DateTime? DataRealizimit { get; set; }
        public DateTime? OraRealizimit { get; set; }
        [NotNull]
        public string IDAgjenti { get; set; }
        public int? NrRendor { get; set; }
        [NotNull]
        public string IDStatusiVizites { get; set; }
        public string IDKlientDheLokacion { get; set; }
        public int? MenyraVizites { get; set; }
        public string Komenti { get; set; }
        public string DeviceID { get; set; }
        public int? SyncStatus { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }

        //Klienti field mbushet nga IDKlientDheLokacion -> KontaktEmriMbiemri
        public string Klienti { get; set; }
        //Vendi field mbushet nga IDKlientiDheLokacion -> EmriLokacionit
        public string Vendi { get; set; }
        //Adresa field mbushet nga IDKlientiDheLokacion -> Adresa
        public string Adresa { get; set; }

        public int CompareTo(Vizita other) {
            throw new NotImplementedException();
        }
    }
}
