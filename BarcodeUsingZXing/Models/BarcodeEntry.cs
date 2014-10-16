using System;
using System.ComponentModel.DataAnnotations;

namespace BarcodeUsingZXing.Models
{
    public class BarcodeEntry
    {
        public int Id { get; set; }
        public DateTime? Created { get; set; }

        public string BarcodeText { get; set; }

        [MaxLength]
        public byte[] BarcodeBytes { get; set; }

        
    }
}