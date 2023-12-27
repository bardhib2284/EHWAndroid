using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class Arsyejet {
        [JsonProperty("idarsyeja")]
        [PrimaryKey]
        public string IDArsyeja { get; set; }
        [JsonProperty("pershkrimi")]
        public string Pershkrimi { get; set; }
    }
}
