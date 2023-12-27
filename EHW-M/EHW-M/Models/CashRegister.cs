using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class CashRegister : Table{
        public DateTime RegisterDate { get; set; }
        public decimal Cashamount { get; set; }
        public int DepositType { get; set; }
        public string DeviceID { get; set; }
        [PrimaryKey]
        public Guid ID { get; set; }
        public int? SyncStatus { get; set; }
        public int? TCRSyncStatus { get; set; }
        public string TCRCode { get; set; }
        public string Message { get; set; }
        public override string TableName() {
            return "CashRegister";
        }
    }
}