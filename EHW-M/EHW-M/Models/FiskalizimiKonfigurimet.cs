using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class FiskalizimiKonfigurimet {
        [PrimaryKey]
        public int ID { get; set; }
        public string TAGNR { get; set; }
        public string TCRCode { get; set; }
        public string BusinessUnitCode { get; set; }
    }
}
