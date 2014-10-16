using System.Data.Entity;

namespace BarcodeUsingZXing.Models
{
    public class BarcodeContext : DbContext
    {
        public DbSet<BarcodeEntry> BarcodeEntries { get; set; } 
    }
}