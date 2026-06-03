namespace WareHouse_Optimization_System.DTOs.Category
{
    public class CreateCategoryDto
    {
        public string Name { get; set; } = null!;
    
        public int DedicatedZoneId { get; set; }
    }
}
