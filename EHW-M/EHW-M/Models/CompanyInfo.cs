using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class CompanyInfo {
        [PrimaryKey]
        public int ID { get; set; }
        public string Item { get; set; }
        public string Value { get; set; }
    }
}