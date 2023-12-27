using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class KlientDheLokacion : Table {
        [PrimaryKey,NotNull]
        public string IDKlientDheLokacion { get; set; }
        public string EmriLokacionit { get; set; }
        public string Adresa { get; set; }
        public string KontaktEmriMbiemri { get; set; }
        public string Tel_Mobil { get; set; }
        public string IDVendi { get; set; }
        public string IDKlienti { get; set; }
        public string Barkodi { get; set; }
        public int? SyncStatus { get; set; }
        public string Depo { get; set; }
        public string KMAG { get; set; }
        public override string TableName() {
            return "KlientDheLokacion";
        }
    }
}
