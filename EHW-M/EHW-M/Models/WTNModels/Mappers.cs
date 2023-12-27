using EHWM.DependencyInjections.FiskalizationExtraModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EHWM.Models.WTNModels
{
    public class MapperHeader {
        public string Numri_Levizjes { get; set; }
        public int InvOrdNum { get; set; }
        public string InvNum { get; set; }
        public decimal ValueOfGoods { get; set; }
        public string FromDevice { get; set; }
        public string ToDevice { get; set; }
        public int ImpStatus { get; set; }
    }

    public class MapperLines {

        public string Numri_Levizjes { get; set; }
        public string OperatorCode { get; set; }
        public string DeviceID { get; set; }
        public int InvOrdNum { get; set; }
        public string InvNum { get; set; }
        public DateTime SendDatetime { get; set; }
        public string StartPointSType { get; set; }
        public string DestinPointSType { get; set; }
        public string MobileRefId { get; set; }
        public decimal ValueOfGoods { get; set; }
        public string FromDevice { get; set; }
        public string ToDevice { get; set; }


        public string Item_N { get; set; }
        public string Item_C { get; set; }
        public string Item_U { get; set; }
        public decimal Item_Q { get; set; }
    }

    public class ClassMapper {
        public static List<TCRLevizjetPCL> MapHeadersAndLines(List<MapperHeader> mapperHeaderList, List<MapperLines> mapperLinesList) {
            List<TCRLevizjetPCL> result = new List<TCRLevizjetPCL>();
            foreach (MapperHeader headervar in mapperHeaderList) {
                TCRLevizjetPCL header = new TCRLevizjetPCL();
                MapperLines line = mapperLinesList.FirstOrDefault(x => x.InvOrdNum == headervar.InvOrdNum);
                if (line != null) {
                    header.Numri_Levizjes = line.Numri_Levizjes;
                    header.InvOrdNum = line.InvOrdNum;
                    header.InvNum = line.InvNum;
                    header.DeviceID = line.DeviceID;
                    header.MobileRefId = line.MobileRefId;
                    header.OperatorCode = line.OperatorCode;
                    header.StartPointSType = WTNStartPointSType.SALE;
                    header.DestinPointSType = headervar.ImpStatus == 3 ? WTNDestinPointSType.WAREHOUSE : WTNDestinPointSType.SALE;
                    header.SendDatetime = line.SendDatetime;
                    header.ValueOfGoods = headervar.ValueOfGoods;
                    header.FromDevice = headervar.FromDevice;
                    header.ToDevice = headervar.ToDevice;

                    header.Items = new List<WTNItemTypePCL>();
                    foreach (MapperLines itemsvar in mapperLinesList.Where(x => x.InvOrdNum == headervar.InvOrdNum).ToList()) {
                        WTNItemTypePCL it = new WTNItemTypePCL();
                        it.C = itemsvar.Item_C;
                        it.N = itemsvar.Item_N;
                        it.Q = (double)itemsvar.Item_Q;
                        it.U = itemsvar.Item_U;
                        header.Items.Add(it);
                    }
                    result.Add(header);
                }
            }
            return result;
        }
    }
}
