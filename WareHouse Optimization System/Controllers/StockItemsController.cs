using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WareHouse_Optimization_System.DTOs.Stock;
using WareHouse_Optimization_System.Models;
using WareHouse_Optimization_System.Services;
using System;
using WareHouse_Optimization_System.Services.Implementations;
using WareHouse_Optimization_System.Services.Interfaces;
namespace WareHouse_Optimization_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockItemsController : ControllerBase
    {
        private readonly IStockService _services;

        public StockItemsController(IStockService services)
        {
            _services = services;
        }


        // Get Stock Items

        [HttpGet]
        public async Task<ActionResult<List<StockItem>>> GetStockItems()
        {
            ServiceResult<List<StockItem>> result = await _services.GetAllStockItems();
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
            return Ok(result.Data);
        }


        //Get Stock Item By Id

        [HttpGet("{id}")]
        public async Task<ActionResult<StockItem>> GetStockItem(int id)
        {
            StockItem? stockItem = await _services.GetStockItemByIdAsync(id);
            if (stockItem == null) return NotFound();
            return Ok(stockItem);
        }



        // Create Stock Items

        [HttpPost]
        public async Task<ActionResult<StockItem>> PostStockItem(AddStockDto stockdto)
        {
            ServiceResult<StockItem> result = await _services.AddStockItemAsync(stockdto);
            if (!result.IsSuccess)
            {
            
                return BadRequest(result.ErrorMessage);
            }



            return CreatedAtAction(nameof(GetStockItem), new { id = result.Data.ItemId }, result.Data);
        }


        // DELETE: api/StockItems/5
        [HttpPost("{id}/remove")]
        public async Task<IActionResult> RemoveStockItem(int id,[FromBody]int quantity)
        {

           
                ServiceResult<bool> result = await _services.RemoveStockAsync(id, quantity);
                if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
                return Ok("Stock Successfully removed");
           
            
           
        }

        public async Task<bool> LowStockAlertAsync(int id)
        {
            StockItem? stockItem = await _services.GetStockItemByIdAsync(id);
            if (stockItem == null) return false;
            // Assuming a low stock threshold of 10 for demonstration purposes
            return stockItem.Quantity < 10;
        }
        
      
    }
}
