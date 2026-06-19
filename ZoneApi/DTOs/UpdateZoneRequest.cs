using System.ComponentModel.DataAnnotations;

namespace Zone.DTOs
{
    public class UpdateZoneRequest
    {
        public string? Name { get; set; }
        public int? MaxCapacity { get; set; }
    }
}
