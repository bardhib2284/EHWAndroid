using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class FaturatEFiskalizuara {
        [PrimaryKey]
        public int Id { get; set; }
        public string IDFature { get; set; }
        public string IIC { get; set; }
        public string Type { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal VatAmount { get; set; }
        public string OperatorCode { get; set; }
        public string DeviceID { get; set; }
        public DateTime DateCreated { get; set; }
        public string DateCreatedString { get; set; }
        public bool IsCorrected { get; set; }
        public DateTime DateCorrected { get; set; }
        public string IDFatureCorrected { get; set; }
        public string QRCodeLink { get; set; }
        public string TCRBusinessCode { get; set; }
        public string TCRNSLF { get; set; }
        public string TCRNIVF { get; set; }
        public string TCR { get; set; }
        public string UUID { get; set; }
        public string EIC { get; set; }
        public string NrLiferimit { get; set; }
        public int Status { get; set; }
        public string NIPT { get; set; }
        public int Viti { get; set; }
    }
}