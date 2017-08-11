using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ThoughtWorks.QRCode
{
    public class QRCodeHelper
    {
        public static Image GenerateQRCode(string data, Image icon = null)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            try
            {               
                var qrCodeEncoder = new QRCodeEncoder
                {
                    QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE,
                    QRCodeScale = 4,// 编码测量度
                    QRCodeVersion = 0,
                    QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L
                };


                var img = qrCodeEncoder.Encode(data);

                if (icon != null)
                {
                    var size = img.Width * img.Height;
                    var iconSize = size * 0.05;

                    var logo = ResizeImage(icon, iconSize);

                    var g = Graphics.FromImage(img);
                    g.DrawImage(logo, new System.Drawing.Point(img.Width / 2 - logo.Width / 2, img.Height / 2 - logo.Height / 2));
                }

                return img;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }
        public static Image ResizeImage(Image bmp, double newSize)
        {
            var rate = Math.Sqrt(newSize / (bmp.Width * bmp.Height));
            return ResizeImage(bmp, (int)(bmp.Width * rate), (int)(bmp.Height * rate));
        }

        public static Image ResizeImage(Image bmp, int newW, int newH)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量   
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }
    }
}
