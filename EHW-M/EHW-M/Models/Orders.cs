using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class Orders {
        [PrimaryKey]
        public Guid ID { get; set; }
        public string IDOrder { get; set; }
        public string DeviceID { get; set; }
        public DateTime Data { get; set; }
        public string Depo { get; set; }
        public string IDAgjenti { get; set; }
        public int SyncStatus { get; set; }
        public string IDKlientDheLokacion { get; set; }
        public int ImpStatus { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}