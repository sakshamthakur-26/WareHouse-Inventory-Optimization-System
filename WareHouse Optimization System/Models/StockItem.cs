using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WareHouse_Optimization_System.Models
{

    [Index(nameof(Name), IsUnique = true)]
    public class StockItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ItemId { get; set; }
       
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int ZoneId { get; set; }
    }
}
