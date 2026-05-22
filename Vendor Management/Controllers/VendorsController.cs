namespace VendorManagement.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using VendorManagement.Models;
    using VendorManagement.DTOs;
    using VendorManagement.Services;

    [ApiController]
    [Route("api/v1/[controller]")] // Rule 3: Plural naming and v1 prefix
    public class VendorsController : ControllerBase
    {
        private readonly VendorService _service;

        public VendorsController(VendorService service)
        {
            _service = service;
        }

        // GET: api/v1/vendors
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetVendors());

        // GET: api/v1/vendors/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var v = await _service.GetVendor(id);
            if (v == null) return NotFound(new ErrorResponse { ErrorCode = "VENDOR_NOT_FOUND", Message = "Vendor not found" });
            return Ok(v);
        }

        // POST: api/v1/vendors
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VendorRequest req)
        {
            var v = new Vendor
            {
                Name = req.Name,
                ContactDetails = req.ContactDetails,
                GoodsSupllied = req.GoodsSupllied
            };
            await _service.CreateVendor(v);
            return CreatedAtAction(nameof(GetById), new { id = v.VendorId }, v); // 201 Created
        }

        // PUT: api/v1/vendors/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VendorRequest req)
        {
            var v = await _service.GetVendor(id);
            if (v == null) return NotFound(new ErrorResponse { ErrorCode = "VENDOR_NOT_FOUND", Message = "Vendor not found to update" });

            v.Name = req.Name;
            v.ContactDetails = req.ContactDetails;
            v.GoodsSupllied = req.GoodsSupllied;

            await _service.UpdateVendor(v);
            return Ok(v); // 200 OK
        }

        // DELETE: api/v1/vendors/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.RemoveVendor(id);
            if (!success) return NotFound(new ErrorResponse { ErrorCode = "VENDOR_NOT_FOUND", Message = "Vendor not found to delete" });

            return Ok(new { message = "Deleted successfully" });
        }
    }
}