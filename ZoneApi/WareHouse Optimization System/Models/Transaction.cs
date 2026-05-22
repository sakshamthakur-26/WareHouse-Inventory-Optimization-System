using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transaction_log.Model
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
    }
}
