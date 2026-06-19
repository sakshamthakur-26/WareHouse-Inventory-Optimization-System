using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zone.DTOs;
using Zone.Services;
using ZoneApi.Model;

[Route("api/[controller]")]
[ApiController]
public class ZonesController : ControllerBase
{
    private readonly IZoneService _service  ;   

    public ZonesController(IZoneService service)
    {
        _service = service;
    }


    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _service.GetAllAsync();

        if (result.IsSuccess) return Ok(result);
        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (!result.IsSuccess) return NotFound(result);
        return Ok(result);
    }

    [HttpPost]
    //[Route("/Create")]
    public async Task<IActionResult> Create(CreateZoneRequest request)
    {
        var result = await _service.CreateAsync(request);

        if (!result.IsSuccess) return BadRequest(result);
        return CreatedAtAction(nameof(GetById), new { id = result.Data.ZoneId }, result);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateZoneRequest request)
    {
        var result = await _service.UpdateAsync(id, request);

        if (!result.IsSuccess) return BadRequest(result);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);

        if (!result.IsSuccess) return NotFound(result);
        return Ok(result);
    }


}