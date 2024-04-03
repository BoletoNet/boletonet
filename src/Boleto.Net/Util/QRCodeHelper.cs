using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace BoletoNet.Util
{
    public class QRCodeHelper
    {
        public static string GerarQrCode(string strQrCode)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();

            QRCodeData qrCodeData = qrGenerator.CreateQrCode(strQrCode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(5, Color.Black, Color.White, null, 25);

            var qrCodeRet = string.Empty;

            using (var memoryStream = new MemoryStream())
            {
                qrCodeImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                qrCodeRet = Convert.ToBase64String(memoryStream.ToArray());
            }

            return qrCodeRet;
        }
    }
}
