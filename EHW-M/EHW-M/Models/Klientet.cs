using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class Klientet : Table {
        [PrimaryKey]
        public string IDKlienti { get; set; }
        public string Emri { get; set; }
        public string EmailKontakt1 { get; set; }
        public string KontaktNumer1 { get; set; }
        public string ShkKlienti { get; set; }
        public string GrRabatet { get; set; }
        public string GrCmimoret { get; set; }
        public int? SyncStatus { get; set; }
        public string Depo { get; set; }
        public string NIPT { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public override string TableName() {
            return "Klientet";
        }
    }
}
