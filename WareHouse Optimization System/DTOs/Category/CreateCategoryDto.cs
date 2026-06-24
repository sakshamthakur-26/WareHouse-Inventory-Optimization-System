using System.ComponentModel.DataAnnotations;

namespace WareHouse_Optimization_System.DTOs.Category
{
    public class CreateCategoryDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        public int DedicatedZoneId { get; set; }
    }
}
