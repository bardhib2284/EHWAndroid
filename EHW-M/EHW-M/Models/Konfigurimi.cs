using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class Konfigurimi : Table {
        [PrimaryKey]
        public string DeviceID { get; set; }
        public string IDAgent { get; set; }
        public bool? GetPayment { get; set; }
        public bool? PrintBill { get; set; }
        public string ApplicationMode { get; set; }
        public string Depo { get; set; }
        public string DBPassword { get; set; }

        public override string TableName() {
            return "Konfigurimi";
        }
    }
}
