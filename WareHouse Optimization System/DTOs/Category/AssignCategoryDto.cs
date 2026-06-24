using System.ComponentModel.DataAnnotations;

namespace WareHouse_Optimization_System.DTOs.Category
{
    public class AssignCategoryDto
    {
        [Required]
        public string CategoryName { get; set; } = null!;

        [Required]
        public int ZoneId { get; set; }
    }
}
