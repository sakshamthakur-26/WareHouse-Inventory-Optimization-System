namespace VendorManagement.DTOs
{
    // Rule 5: Common Error Response Format for consistent error handling
    public class ErrorResponse
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string CorrelationId { get; set; }
    }

    // DTO for both Create and Update requests
    public class VendorRequest
    {
        public string Name { get; set; }
        public string ContactDetails { get; set; }
        public string GoodsSupllied { get; set; }
    }
}
