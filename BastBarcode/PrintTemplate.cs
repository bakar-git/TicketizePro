using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace BastBarcode
{
    public class PrintTemplate
    {
        public static async Task<string> Make(ProductDtls dtls)
        {
            return Header + await Body(dtls) + Footer;
        }
        
        public static string Header = "<!DOCTYPE html><html lang=\"en\"><head><meta charset=\"UTF-8\"><meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><title>Document</title></head><body><style>*{margin: 0;padding: 0;font-family: Arial,Verdana, Geneva, sans-serif;} .Section{display: flex;width: 100%;height: 100vh;flex-direction: column;} .BusinessName{display: block;text-align: center;font-size: 70px;font-weight: bold;text-transform: uppercase;} .ProductName_Barcode{width: 100%;height: 150px;} .ProductName{font-size: 90px;font-weight: bold;text-align: center;text-transform: uppercase;} .SectionFooter{display: flex;align-items: center;justify-content: center;} .ProductColor{font-size: 50px;font-weight: bold;text-transform: uppercase;} .ProductSize{font-size: 80px;font-weight: bold;text-transform: uppercase;margin: auto;} .PriceSection{display: flex;} .ProductOldPrice{position: relative;top: -10px;font-size: 40px;text-transform: uppercase;font-weight: normal;text-decoration: line-through;} .ProductNewPrice{font-size: 50px;font-weight: bold;text-transform: uppercase;}</style>";
        
        public static async Task<string> Body(ProductDtls dtls)
        {
            string body = "";
            string img_name = await SaveFile(MakeBarcode(dtls.ProductName), FileFormat.Png);
            string template;
            foreach (string size in dtls.ProductSizeList)
            {
                template = $"<div class=\"Section\"><h1 class=\"BusinessName\">{dtls.BusinessName}</h1><img class=\"ProductName_Barcode\" src=\"{img_name}\"><h1 class=\"ProductName\">{dtls.ProductName}</h1><div class=\"SectionFooter\"><h1 class=\"ProductColor\">{dtls.ProductColor}</h1><h1 class=\"ProductSize\">{size}</h1><div class=\"PriceSection\"><h1 class=\"ProductNewPrice\">RS.{dtls.ProductNewPrice}</h1><h1 class=\"ProductOldPrice\">{dtls.ProductOldPrice}</h1></div></div></div>";
                body += template; 
            }
            return body;
        }
        
        public static string Footer = "<script>window.print();window.onafterprint = ()=>{window.close()}; </script></body></html>";

        public static async Task<string> SaveFile(WriteableBitmap WB, FileFormat fileFormat)
        {
            //string FileName = RandomString(5)+".";
            string FileName = "barcode" + ".";
            Guid BitmapEncoderGuid = BitmapEncoder.JpegEncoderId;
            switch (fileFormat)
            {
                case FileFormat.Jpeg:
                    FileName += "jpeg";
                    BitmapEncoderGuid = BitmapEncoder.JpegEncoderId;
                    break;
                case FileFormat.Png:
                    FileName += "png";
                    BitmapEncoderGuid = BitmapEncoder.PngEncoderId;
                    break;
            }
            var file = await Windows.Storage.ApplicationData.Current.TemporaryFolder.CreateFileAsync(FileName, CreationCollisionOption.GenerateUniqueName);
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoderGuid, stream);
                Stream pixelStream = WB.PixelBuffer.AsStream();
                byte[] pixels = new byte[pixelStream.Length];
                await pixelStream.ReadAsync(pixels, 0, pixels.Length);
                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)WB.PixelWidth, (uint)WB.PixelHeight,
                    96.0,
                    96.0,
                    pixels);
                await encoder.FlushAsync();
            }
            return FileName;
        }
        
        public static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        
        public enum FileFormat
        {
            Jpeg,
            Png,
        }

        public static WriteableBitmap MakeBarcode(string text)
        {
            text = Regex.Replace(text, @"[^a-zA-Z0-9]", "");
            //if (text.Length % 2 != 0) text = text.Insert(0, "0");
            ZXing.IBarcodeWriter writer = new ZXing.BarcodeWriter
            {
                Format = ZXing.BarcodeFormat.CODE_128, //Mentioning type of bar code generation
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 200,
                    Width = 650,
                    Margin = 0,
                    PureBarcode = true,
                },
                Renderer = new ZXing.Rendering.WriteableBitmapRenderer() { Foreground = Windows.UI.Colors.Black, Background = Windows.UI.Colors.White }//Adding color QR code
            };
            return writer.Write(text);
            //Displaying QRCode Image
            //Img_Barcode.Source = result;

        }
        
    }
}
