using System;
using System.Drawing.Imaging;
using System.Linq;
using System.Web.Mvc;
using BarcodeUsingZXing.Models;
using ZXing;
using ZXing.Common;
using ZXing.OneD;
using ZXing.QrCode;

namespace BarcodeUsingZXing.Controllers
{
    public class HomeController : Controller
    {
        private readonly BarcodeContext _db = new BarcodeContext();


        public ActionResult Index(string id)
        { 
            byte[] image;
            if (string.IsNullOrWhiteSpace(id))
            {
                id = "ABCDEFGHIJKLMNOP";
            }
            var entry = _db.BarcodeEntries.FirstOrDefault(x => x.BarcodeText == id);
            if (entry != null)
            {
                image = entry.BarcodeBytes;
                ViewBag.Message = "Retrieved from Database";
            }
            else
            {
                image = id.GenerateBarcode();
                ViewBag.Message = "Generate new barcocee";
            }


            if (image != null)
            {
                ViewBag.ImageData = image.ToImageData();
                SaveBarcodeToDatabase(id, image);
            }

            return View();
        }

        private void SaveBarcodeToDatabase(string id, byte[] image)
        {
            BarcodeEntry entry;
            entry = new BarcodeEntry {BarcodeBytes = image, BarcodeText = id, Created = DateTime.UtcNow};
            _db.BarcodeEntries.Add(entry);
            _db.SaveChanges();
        }

      

        public FileContentResult ImageGenerate(string s)
        {
            var writer = new BarcodeWriter();
          var  bitMatrix = new Code128Writer().encode(s, 
              BarcodeFormat.CODE_128, 50, 80, null);
            var result = writer.Write(bitMatrix);
            var image = result.ToByteArray(ImageFormat.Png);
            return new FileContentResult(image, "image/jpeg");
        }

     

        public  FileContentResult CreateQrCodeImage(string toEncode)
        {
             var imageSize = new Dimension(100,100);
            var qrWrite = new QRCodeWriter();
            var matrix = qrWrite.encode(
                toEncode, BarcodeFormat.QR_CODE, imageSize.Width, imageSize.Height);
            var result = new BarcodeWriter().Write(matrix);
            var image = result.ToByteArray(ImageFormat.Bmp);
            ViewBag.ImageData = image.ToImageData();
            return new FileContentResult(image, "image/jpeg");
        }

        public ActionResult About(string contents)
        {
            ViewBag.Message = "Your application description page.";
            if (string.IsNullOrWhiteSpace(contents))
            {
                 contents = ViewBag.Message;
            }
            
            contents = contents.ToUpper();
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                //Options = new EncodingOptions { Height = 0,Width = 0}

            };
            var code39Writer = new Code39Writer();
            var matrix = code39Writer.encode(contents, BarcodeFormat.CODE_39, 0, 50);
            var result = new BarcodeWriter().Write(matrix);
            ViewBag.ImageData = result.ToByteArray().ToImageData();

          
            //var bitmap = writer.Write(contents.ToUpper());
            //ViewBag.ImageData = bitmap.ToByteArray().ToImageData();

            return View();
        }

        public ActionResult Contact(string contents)
        {
            ViewBag.Message = "Your contact page.";
            if (string.IsNullOrWhiteSpace(contents))
            {
                contents = ViewBag.Message;
            }

            contents = contents.ToUpper();
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                //Options = new EncodingOptions { Height = 0,Width = 0}

            };
            var code39Writer = new UPCAReader();
            var matrix = code39Writer.encode(contents, BarcodeFormat.UPC_A, 0, 50);
            var result = new BarcodeWriter().Write(matrix);
            ViewBag.ImageData = result.ToByteArray().ToImageData();
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_db != null) _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}