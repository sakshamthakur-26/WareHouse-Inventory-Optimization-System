namespace Transaction_log.DTOs
{
    public class TransactionLogResponse
    {
        public int TransactionId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
