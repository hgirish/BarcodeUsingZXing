using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ZXing;
using ZXing.QrCode;

namespace BarcodeUsingZXing.Controllers
{
    public static  class ImageHelper
    {
        public static  byte[] ToByteArray(this Bitmap bitmap, ImageFormat format = null )
        {
            if (format == null)
            {
                format = ImageFormat.Jpeg;
            }
            using (var memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, format);
                return memoryStream.ToArray();
            }
        }

        public static string ToImageData(this byte[] imageBytes)
        {
            var imageData = "data:image/jpg;base64," + Convert.ToBase64String(imageBytes);
            return imageData;
        }

        public static byte[] GenerateBarcode(this string input)
        {
            var imageSize = new Dimension(100, 100);
            var qrWrite = new QRCodeWriter();
            var matrix = qrWrite.encode(
                input, BarcodeFormat.QR_CODE, imageSize.Width,
                imageSize.Height);
            var result = new BarcodeWriter().Write(matrix);
            var image = result.ToByteArray();
            return image;
        }
    }
}