namespace WareHouse_Optimization_System.DTOs.Transaction
{
    public class CreateTransactionRequest
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; } // Inbound / Outbound
    }
}
