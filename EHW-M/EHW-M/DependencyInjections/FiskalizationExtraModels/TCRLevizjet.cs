using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.DependencyInjections.FiskalizationExtraModels
{
    public class TCRLevizjetPCL
    {
        public string Numri_Levizjes { get; set; }
        public string OperatorCode { get; set; }
        public string DeviceID { get; set; }
        public int InvOrdNum { get; set; }
        public string InvNum { get; set; }
        public DateTime SendDatetime { get; set; }
        public WTNStartPointSType StartPointSType { get; set; }
        public WTNDestinPointSType DestinPointSType { get; set; }
        public decimal ValueOfGoods { get; set; }
        public string FromDevice { get; set; }
        public string ToDevice { get; set; }

        public List<WTNItemTypePCL> Items { get; set; }
        public string MobileRefId { get; set; }
    }
}
