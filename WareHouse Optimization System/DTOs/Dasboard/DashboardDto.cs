namespace WareHouse_Optimization_System.DTOs.Dasboard
{
    public class DashboardDto
    {
        public int TotalZones { get; set; }
        public int TotalLowStockAlerts { get; set; }
        public int TotalActiveVendors { get; set; }
        public int TotalStockItems { get; set; }
        public List<LowStockItemDto> LowStockItems { get; set; } = new List<LowStockItemDto>();
    }
}
