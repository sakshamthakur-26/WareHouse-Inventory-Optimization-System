using System.ComponentModel.DataAnnotations;

namespace WareHouse_Optimization_System.DTOs.Zone
{
    public class UpdateZoneRequest
    {

        public string? Name { get; set; }

        public int? MaxCapacity { get; set; }

    }
}
