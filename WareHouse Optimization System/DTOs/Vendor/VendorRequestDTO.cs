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

        [EmailAddress(ErrorMessage = "Invalid email format. The email must contain an '@' character.")]
        public string Email { get; set; }

        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid phone number phone number should contain 10 numbers")]
        public double? PhoneNumber { get; set; }
        public string GoodsSupplied { get; set; }
        
    }
}
