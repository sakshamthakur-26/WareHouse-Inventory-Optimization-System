namespace WareHouse_Optimization_System.DTOs.Zone
{
    public class ZoneResponse
    {
        public int ZoneId { get; set; }
        public string Name { get; set; }
        public int MaxCapacity { get; set; }
        public int CurrentUsage { get; set; }
    }
}
