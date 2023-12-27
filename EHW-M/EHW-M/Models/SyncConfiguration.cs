using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class SyncConfiguration {
        public int ID { get; set; }
        public string TableName { get; set; }
        public string SyncDirection { get; set; }
        public int? SyncDay { get; set; }
        public int? SyncOrder { get; set; }
        public int? FilterUp { get; set; }
        public int? FilterDown { get; set; }
        [JsonProperty("pkFieldName")]
        public string PK_FieldName { get; set; }
    }
}
