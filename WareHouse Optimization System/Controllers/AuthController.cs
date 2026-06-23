using System.Security.Cryptography;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WareHouse_Optimization_System.DTOs.Staffs;
using WareHouse_Optimization_System.Models;
using WareHouse_Optimization_System.Db;
using WareHouse_Optimization_System.Services.Implementations;

namespace WareHouse_Optimization_System.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly WarehouseDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(WarehouseDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // REGISTER
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var userExists = _context.Set<Staff>().Any(x => x.Username == request.Username);
            if (userExists)
                return BadRequest("User already exists");

            var staff = new Staff
            {
                Name = request.Name,
                Username = request.Username,
                PasswordHash = HashPassword(request.Password),
                Role = "Staff"
            };

            _context.Staffs.Add(staff);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Registered Successfully" });

        }

        // LOGIN
        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            var staff = _context.Staffs
                .FirstOrDefault(x => x.Username == request.Username);

            if (staff == null || staff.PasswordHash != HashPassword(request.Password))
                return Unauthorized("Invalid credentials");

            var token = _jwtService.GenerateToken(staff.Username, staff.Role);

            return Ok(new AuthResponse
            {
                Token = token,
                Username = staff.Username,
                Role = staff.Role
            });
        }

        // SIMPLE HASH (can upgrade to BCrypt later)
        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
