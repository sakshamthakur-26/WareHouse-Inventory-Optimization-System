
using System.ComponentModel.DataAnnotations;

namespace WareHouse_Optimization_System.Models
{
    public class Staff
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string Role { get; set; } = "Staff"; // Admin / Staff
    }
}
