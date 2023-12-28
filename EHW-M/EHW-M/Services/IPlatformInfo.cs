using System;
using System.Threading.Tasks;

namespace EHWM.Services {
    public interface IPlatformInfo {
        object AndroidContext { get; set; }
        object AndroidResource { get; set; }
        string GetPath();
        object GetImgResource();    // Image Resource Type return
        object GenerateQRCode(string qrCode);
    }
}