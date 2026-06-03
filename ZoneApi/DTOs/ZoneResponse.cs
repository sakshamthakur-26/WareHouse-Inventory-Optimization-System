namespace Zone.DTOs
{
    public class ZoneResponse
    {
        public int ZoneId { get; set; }
        public string Name { get; set; }
        public int MaxCapacity { get; set; }
        public int CurrentUsage { get; set; }
    }
    public class CreateZoneRequest
    {
        public string Name { get; set; }
        public int MaxCapacity { get; set; }
    }

    public class UpdateZoneRequest
    {
        public int ZoneId { get; set; }
        public string Name { get; set; }
        public int MaxCapacity { get; set; }
    }
}
