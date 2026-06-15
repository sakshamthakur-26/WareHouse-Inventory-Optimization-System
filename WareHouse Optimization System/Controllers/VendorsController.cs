namespace VendorManagement.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Vendor_Management.Services;
    using VendorManagement.DTOs;
    using VendorManagement.Models;
    using VendorManagement.Services;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class VendorsController : ControllerBase
    {
    
        private readonly IVendorService _service;

        public VendorsController(IVendorService service)
        {
            _service = service;
        }
   

                                        // GET ---- api/v1/vendors

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _service.GetAllAsync();
            if (res == null || !res.IsSuccess) return NotFound(new ErrorResponse { 
                ErrorCode = "VENDORS_NOT_FOUND", 
                Message = res?.ErrorMessage ?? "Vendors not found" });
            return Ok(res.Data);
        }

                                        // GET ---- api/v1/vendors/{id}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _service.GetByIdAsync(id);
            if (res == null || !res.IsSuccess || res.Data == null) return NotFound(new ErrorResponse { 
                ErrorCode = "VENDOR_NOT_FOUND", 
                Message = "Vendor not found" 
            });
            return Ok(res.Data);
        }

        
                                           // POST --- api/v1/vendors

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VendorRequest req)
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
            if (res == null || !res.IsSuccess) return BadRequest(new ErrorResponse { 
                ErrorCode = "VENDOR_CREATE_FAILED", 
                Message = res?.ErrorMessage ?? "Failed to create vendor" 
            });

            return CreatedAtAction(nameof(GetById), new { id = res.Data.VendorId }, res.Data); // 201 Created
        }


                                         // PUT   --- api/v1/vendors/{id}


        [HttpPut("{id}")]
                  public async Task<IActionResult> Update(int id, [FromBody] VendorRequest req)
                       {

                 var existingRes = await _service.GetByIdAsync(id);

            if (existingRes == null || !existingRes.IsSuccess || existingRes.Data == null)
                return NotFound(new ErrorResponse {
                    ErrorCode = "VENDOR_NOT_FOUND", 
                    Message = "Vendor not found to update" 
                });



            var v = existingRes.Data;
            v.Name = req.Name;
            v.Email = req.Email;
            v.PhoneNumber = v.PhoneNumber;
            v.GoodsSupplied = req.GoodsSupplied;
            v.IsActive = req.IsActive;

            var res = await _service.UpdateAsync(v);

            if (res == null || !res.IsSuccess) return BadRequest(new ErrorResponse { 
                ErrorCode = "VENDOR_UPDATE_FAILED", 
                Message = res?.ErrorMessage ?? "Failed to update vendor" 
            });

            return Ok(res.Data); // 200 OK
        }

        
    }
}