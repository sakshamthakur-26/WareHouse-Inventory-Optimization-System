using Microsoft.AspNetCore.Mvc;
using WareHouse_Optimization_System.DTOs.Stock;
using WareHouse_Optimization_System.Models;

namespace WareHouse_Optimization_System.Services.Interfaces
{
    public interface IStockService
    {
        Task<StockItem?> GetStockItemByIdAsync(int id);
        Task<ServiceResult<StockItem>> AddStockItemAsync(AddStockDto dto);
        Task<ServiceResult<bool>> RemoveStockAsync(int itemId, int quantity);
        Task<ServiceResult<List<StockItem>>> GetAllStockItems();
    }
}
