using System.ComponentModel.DataAnnotations;

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

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string GoodsSupplied { get; set; }
        public bool IsActive { get; set; }

    }
}
