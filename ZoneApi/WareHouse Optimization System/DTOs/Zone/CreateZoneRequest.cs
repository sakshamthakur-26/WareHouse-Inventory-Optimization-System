using System.ComponentModel.DataAnnotations;

namespace Zone.DTOs
{
    public class CreateZoneRequest
    {
        [Required]
        public string Name { get; set; }
        [Range(1, int.MaxValue)]
        public int MaxCapacity { get; set;}

        //public int CurrentUsage { get; set; }

    }
}
