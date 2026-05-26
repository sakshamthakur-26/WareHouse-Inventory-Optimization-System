namespace VendorManagement.DTOs
{
    public class ErrorResponse
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        
    }

    public class VendorRequest
    {
        public string Name { get; set; }
        public string ContactDetails { get; set; }
        public string GoodsSupllied { get; set; }
    }
}
