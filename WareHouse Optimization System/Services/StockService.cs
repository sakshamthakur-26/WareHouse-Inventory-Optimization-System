using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

using WareHouse_Optimization_System.Controllers;
using WareHouse_Optimization_System.Db;
using WareHouse_Optimization_System.DTOs.Stock;
using WareHouse_Optimization_System.Models;

namespace WareHouse_Optimization_System.Services
{
    public class StockService
    {

        private readonly WarehouseDbContext? _context;

        private readonly DemoService? _dservice = null;
        public StockService(WarehouseDbContext _dbContext)
        {
            _context = _dbContext;
            _dservice = new DemoService();
        }


        public async Task<List<StockItem>> GetAllStockItems()
        {
            return await _context.StockItems.ToListAsync();
        }

        public async Task<StockItem> GetStockItemById(int id)
        {
           
              return await _context.StockItems.FindAsync(id);
                
        }
        public async Task<StockItem> AddStockItem(AddStockDto addStockDto)
        {
            try
            {

                //write find categordId via category name and zoneId via zone name
                if (addStockDto.Quantity <= 0)
                {
                    throw new ArgumentException("Enter valid quantity");
                }

                int categoryId = await _dservice.GetCategoryId(addStockDto.CategoryName);
                

                //return zone id and zone name
                var zoneAllocation = await _dservice.CheckZoneCapacity(addStockDto.CategoryName, addStockDto.Quantity);

                if (zoneAllocation == null)
                {
                    throw new Exception("Zone capacity exceeded for category '{addStockDto.CategoryName}'.");
                }

                var stockItem = new StockItem
                {
                    Name = addStockDto.ItemName,
                    ProductId = 1,
                    CategoryId = categoryId,
                    Quantity = addStockDto.Quantity,
                    ZoneId = zoneAllocation,
                };
                //check via zone capacity is there or not


                _context.StockItems.Add(stockItem);
                var res = await _context.SaveChangesAsync();

                if (res > 0)
                {
                    bool isTransactionCreated = await _dservice.CreateTransaction(stockItem.ItemId, stockItem.Name, stockItem.CategoryId, stockItem.Quantity, stockItem.ZoneId, "Inbound");
                    if (!isTransactionCreated)
                    {
                        throw new InvalidOperationException("Failed to create transaction record");
                    }
                    return stockItem;

                }
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
                
            }


        }


        public async Task<bool> RemoveStock(int ItemId, int Quantity)
        {
            
                if (Quantity <= 0)
                {
                    throw new ArgumentException("Quantity to remove must be greater than zero.");
                }

                var StockItem = await _context.StockItems.FindAsync(ItemId);
                if (StockItem == null)
                {
                    throw new  Exception("Stock item not found");

                }
                int newQuantity = StockItem.Quantity - Quantity;
                if(newQuantity<0)                 {
                    throw new Exception("Insufficient stock quantity");
                }
                StockItem.Quantity = newQuantity;
                var dbResult = await _context.SaveChangesAsync();

                if(dbResult == 0)
                {
                    throw new Exception("Failed to save the updated quantity to the database.");
                }

                bool zoneCapacity = await _dservice.UpdateZoneCapacity(StockItem.ZoneId, Quantity);
                if (!zoneCapacity)
                {
                    throw new Exception("Zone capacity not updated");
                }

                var transactionLogged = await _dservice.CreateTransaction(ItemId, StockItem.Name, StockItem.CategoryId, Quantity, StockItem.ZoneId,"Outbound");
                if (!transactionLogged)
                {
                    throw new Exception("Failed to create transaction record");
                }

                return true;

            
        }


    }
}
