using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WareHouse_Optimization_System.DTOs.Zone;
<<<<<<< HEAD
using WareHouse_Optimization_System.Models;
using WareHouse_Optimization_System.Services.Implementations;
using WareHouse_Optimization_System.Services.Interfaces;
=======
using WareHouse_Optimization_System.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

>>>>>>> 10d953cc53dd83efddaf84efd8b009c04b708817

namespace WareHouse_Optimization_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ZonesController : ControllerBase
    {
<<<<<<< HEAD
    private readonly IZoneService _service  ;   
=======
    private readonly IZoneService _service;
>>>>>>> 10d953cc53dd83efddaf84efd8b009c04b708817

    public ZonesController(IZoneService service)
    {
        _service = service;
    }


    //[HttpGet]
    //private async Task<IActionResult> Get()
    //{
    //    var zones = await _service.GetAllAsync();
    //    return Ok(zones);
    //}

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var zone = await _service.GetByIdAsync(id);
            return Ok(zone);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    //[Route("/Create")]
    public async Task<IActionResult> Create(CreateZoneRequest request)
    {
        try
        {
            var created = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.ZoneId }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }


     [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(int id, UpdateZoneRequest request)
    {
        var result = await _service.UpdateAsync(id, request);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return Ok();
    }

    //[HttpDelete("{id}")]
    //private async Task<IActionResult> Delete(int id)
    //{
    //    try
    //    {
    //        await _service.DeleteAsync(id);
    //        return NoContent();
    //    }
    //    catch (KeyNotFoundException)
    //    {
    //        return NotFound();
    //    }
    //}


    }
}