namespace WareHouse_Optimization_System.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using WareHouse_Optimization_System.DTOs;
    using WareHouse_Optimization_System.Models;
    using WareHouse_Optimization_System.Services;
    using WareHouse_Optimization_System.Services.Interfaces;

    [ApiController]
    [Route("api/[controller]")]
    public class VendorsController : ControllerBase
    {
        private readonly IVendorService _service;

        public VendorsController(IVendorService service)
        {
            _service = service;
        }


        [HttpGet("category/{categoryName}")]
        public async Task<ActionResult<List<string>>> GetVendorsByCategory(string categoryName)
        {
            var res = await _service.GetVendorsByCategory(categoryName);

            if(!res.IsSuccess) {
            
                return NotFound();
            }

            return Ok(res.Data);

        }

        // GET ---- api/v1/vendors
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _service.GetAllAsync();
            if (res == null || !res.IsSuccess) return NotFound(new ErrorResponse
            {
                ErrorCode = "VENDORS_NOT_FOUND",
                Message = res?.ErrorMessage ?? "Vendors not found"
            });
            return Ok(res.Data);
        }

        // GET ---- api/v1/vendors/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _service.GetByIdAsync(id);
            if (res == null || !res.IsSuccess || res.Data == null) return NotFound(new ErrorResponse
            {
                ErrorCode = "VENDOR_NOT_FOUND",
                Message = "Vendor not found"
            });
            return Ok(res.Data);
        }

        // POST --- api/v1/vendors
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VendorRequestDto req)
        {
            var v = new Vendor
            {
                Name = req.Name,
                Email = req.Email,
                PhoneNumber = req.PhoneNumber ?? string.Empty,
                GoodsSupplied = req.GoodsSupplied,
                IsActive = req.IsActive
            };

            var res = await _service.AddAsync(v);
            if (res == null || !res.IsSuccess) return BadRequest(new ErrorResponse
            {
                ErrorCode = "VENDOR_CREATE_FAILED",
                Message = res?.ErrorMessage ?? "Failed to create vendor"
            });

            return CreatedAtAction(nameof(GetById), new { id = res.Data.VendorId }, res.Data);
        }

        // PUT   --- api/v1/vendors/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VendorRequestDto req)
        {
            var existingRes = await _service.GetByIdAsync(id);

            if (existingRes == null || !existingRes.IsSuccess || existingRes.Data == null)
                return NotFound(new ErrorResponse
                {
                    ErrorCode = "VENDOR_NOT_FOUND",
                    Message = "Vendor not found to update"
                });

            var v = existingRes.Data;
            v.Name = req.Name;
            v.Email = req.Email;
            v.PhoneNumber = req.PhoneNumber ?? v.PhoneNumber;
            v.GoodsSupplied = req.GoodsSupplied;
            v.IsActive = req.IsActive;

            var res = await _service.UpdateAsync(v);

            if (res == null || !res.IsSuccess) return BadRequest(new ErrorResponse
            {
                ErrorCode = "VENDOR_UPDATE_FAILED",
                Message = res?.ErrorMessage ?? "Failed to update vendor"
            });

            return Ok(res.Data);
        }

        //public async Task<ActionResult<ServiceResult<List<string>>>>
    }
}
