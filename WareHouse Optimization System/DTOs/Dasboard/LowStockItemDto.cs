namespace WareHouse_Optimization_System.DTOs.Dasboard
{
    public class LowStockItemDto
    {
        //public string ItemName { get; set; }
        //public int Quantity { get; set; }

        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public string CategoryName { get; set; }
        public string? VendorName { get; set; }
    }
}
