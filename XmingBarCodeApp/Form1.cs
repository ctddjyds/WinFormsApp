using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace XmingBarCodeApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void CreateBarCode()
        {
            QrCodeEncodingOptions options = new QrCodeEncodingOptions();
            options.CharacterSet = "UTF-8";
            options.DisableECI = true; 
            // Extended Channel Interpretation (ECI) 主要用于特殊的字符集。并不是所有的扫描器都支持这种编码。
            options.ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.H; 
            // 纠错级别
                //L - 约 7 % 纠错能力。
                //M - 约 15 % 纠错能力。
                //Q - 约 25 % 纠错能力。
                //H - 约 30 % 纠错能力。
            options.Width = 300;
            options.Height = 300;
            options.Margin = 1;//码的边距
            // options.Hints
            //options.PureBarcode = false; // 是否是纯码，如果为 false，则会在图片下方显示数字
            BarcodeWriter barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.PDF_417;
            barcodeWriter.Options = options;
            BarcodeWriter writer = barcodeWriter;

            var bitmap = writer.Write("http://www.baidu.com");// Write 具备生成、写入两个功能
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            bitmap.Save("QR-Code.png", System.Drawing.Imaging.ImageFormat.Png);
        }
        public string LoadQRC(string filename)
        {
            BarcodeReader reader = new BarcodeReader();
            //设置读取的格式（一般为UTF-8）
            reader.Options.CharacterSet = "UTF-8";
            Bitmap p = new Bitmap(filename);
            Result result = reader.Decode(p);
            return result.ToString();
        }
    }
}
