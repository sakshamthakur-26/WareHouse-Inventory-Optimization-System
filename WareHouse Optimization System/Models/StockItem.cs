using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WareHouse_Optimization_System.Models
{
    public class StockItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ItemId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
        public int ZoneId { get; set; }
    }
}
