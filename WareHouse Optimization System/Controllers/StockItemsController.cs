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
namespace WareHouse_Optimization_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockItemsController : ControllerBase
    {
        private readonly StockService _services;

        public StockItemsController(StockService services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockItem>>> GetStockItems()
        {
            return await _services.GetAllStockItems();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StockItem>> GetStockItem(int id)
        {
            StockItem? stockItem = await _services.GetStockItemById(id);
            if (stockItem == null)
            {
                return NotFound();
            }
            return stockItem;
        }

        [HttpPost]
        public async Task<ActionResult<StockItem>> PostStockItem(AddStockDto stockdto)
        {
            StockItem? stockItem = await _services.AddStockItem(stockdto);
            


            return CreatedAtAction("GetStockItem", new { id = stockItem.ItemId }, stockItem);
        }

        // DELETE: api/StockItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStockItem(int id,[FromBody]int quantity)
        {

            try
            {
                var stockItem = await _services.RemoveStock(id, quantity);
                if (!stockItem) throw new Exception("not deleted");
                return Ok("deleted");
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            


           
        }
        
      
    }
}
