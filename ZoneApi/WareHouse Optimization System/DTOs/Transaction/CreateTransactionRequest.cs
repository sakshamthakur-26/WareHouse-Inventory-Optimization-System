namespace Transaction_log.DTOs
{
    public class CreateTransactionRequest
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; } // Inbound / Outbound
    }
}
