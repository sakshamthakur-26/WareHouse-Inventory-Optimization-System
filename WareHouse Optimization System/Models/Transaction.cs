using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WareHouse_Optimization_System.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId {  get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;


        public int? VendorId { get; set; } 
        public Vendor Vendor { get; set; } 

    }
}
