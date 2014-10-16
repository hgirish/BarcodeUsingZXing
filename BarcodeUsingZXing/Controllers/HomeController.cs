﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using BarcodeUsingZXing.Models;
using ZXing;
using ZXing.OneD;
using ZXing.QrCode;

namespace BarcodeUsingZXing.Controllers
{
    public class HomeController : Controller
    {
        private readonly BarcodeContext db = new BarcodeContext();


        public ActionResult Index(string id)
        { 
            byte[] image;
            if (string.IsNullOrWhiteSpace(id))
            {
                id = "ABCDEFGHIJKLMNOP";
            }
            var entry = db.BarcodeEntries.FirstOrDefault(x => x.BarcodeText == id);
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
            db.BarcodeEntries.Add(entry);
            db.SaveChanges();
        }

        private static byte[] GenerateBarcode(string input)
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

        public FileContentResult ImageGenerate(string s)
        {
            var writer = new BarcodeWriter();
          var  bitMatrix = new Code128Writer().encode(s, 
              BarcodeFormat.CODE_128, 50, 80, null);
            var result = writer.Write(bitMatrix);
            var image = result.ToByteArray(ImageFormat.Png);
            return new FileContentResult(image, "image/jpeg");
        }

        //public void Encode()
        //{
        //    var writer = new BarcodeWriter
        //                 {
        //                     Format = BarcodeFormat.CODE_128,
        //                     Options = new EncodingOptions
        //                               {
        //                                   Height = 0,
        //                                   Width = 0,
        //                                   Margin = 0
        //                               }
        //                 };
        //    var image = writer.Write("ABCD");
            
        //}

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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null) db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}