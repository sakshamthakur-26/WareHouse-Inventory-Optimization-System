using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WareHouse_Optimization_System.DTOs.Zone;
using WareHouse_Optimization_System.Models;
using WareHouse_Optimization_System.Services.Implementations;

namespace WareHouse_Optimization_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZonesController : ControllerBase
    {
    private readonly ZoneService _service  ;   

    public ZonesController(ZoneService service)
    {
        _service = service;
    }


    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var zone = await _service.GetByIdAsync(id);
        if (zone == null) return NotFound();
        return Ok(zone);
    }

    [HttpPost]
    //[Route("/Create")]
    public async Task<IActionResult> Create(CreateZoneRequest request) //////CREATE SERVICE ABHI AND DEFINE ALL THE FUNCTIONS USED IN THE FILE
    {
        try
        {
            var zone = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = zone.ZoneId }, zone);
        }
        catch(InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateZoneRequest request)
    {
        await _service.UpdateAsync(id, request);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return Ok();
    }


}
}