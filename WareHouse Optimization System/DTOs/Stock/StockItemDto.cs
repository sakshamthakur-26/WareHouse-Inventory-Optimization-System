namespace WareHouse_Optimization_System.DTOs.Stock
{
    public class StockItemDto
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public string Zone { get; set; }
        public string? Status { get; set; }
        public int? MinimumThreshold { get; set; }
    }
}
