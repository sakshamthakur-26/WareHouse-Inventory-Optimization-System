using Microsoft.AspNetCore.Mvc;
using WareHouse_Optimization_System.DTOs.Stock;
using WareHouse_Optimization_System.Models;
using WareHouse_Optimization_System.Services;
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
        public async Task<ActionResult<List<StockItemDto>>> GetStockItems()
        {
            ServiceResult<List<StockItemDto>> result = await _services.GetAllStockItems();
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
            return Ok(result.Data);
        }

        // Get Stock Item By Id
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

        // Dispatch / Remove Stock (Our resolved architecture)
        [HttpPatch("dispatch")]
        public async Task<IActionResult> DispatchStockItem([FromBody] RemoveStockDto removeDto)
        {
            ServiceResult<bool> result = await _services.RemoveStockAsync(removeDto.ItemId, removeDto.Quantity);

            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
            return Ok("Stock Successfully removed");
        }

        // Low Stock Alert
        [HttpGet("{id}/lowstock")]
        public async Task<bool> LowStockAlertAsync(int id)
        {
            StockItem? stockItem = await _services.GetStockItemByIdAsync(id);
            if (stockItem == null) return false;
            // Assuming a low stock threshold of 10 for demonstration purposes
            return stockItem.Quantity < 10;
        }
    }
}