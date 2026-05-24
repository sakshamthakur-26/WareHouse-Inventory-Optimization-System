using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

using WareHouse_Optimization_System.Controllers;
using WareHouse_Optimization_System.Db;
using WareHouse_Optimization_System.DTOs.Stock;
using WareHouse_Optimization_System.DTOs.Transaction;
using WareHouse_Optimization_System.Models;

namespace WareHouse_Optimization_System.Services
{
    public class StockService
    {

        private readonly WarehouseDbContext? _context;

        private readonly DemoService? _dservice = null;
        private readonly ZoneService? _zoneService = null;
        private readonly TransactionService? _transactionService = null;
        public StockService(WarehouseDbContext _dbContext)
        {
            _context = _dbContext;
            _dservice = new DemoService();
            _zoneService = new ZoneService(_context);
            _transactionService = new TransactionService(_context);
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
                var zoneCapacity = await _zoneService.CheckAvailableCapacityAsync(zoneAllocation, addStockDto.Quantity);

                if (!zoneCapacity)
                {
                    throw new Exception("Zone capacity exceeded for category '{addStockDto.CategoryName}'.");
                }

                var stockItem = new StockItem
                {
                    Name = addStockDto.ItemName,
                  
                    CategoryId = categoryId,
                    Quantity = addStockDto.Quantity,
                    ZoneId = zoneAllocation,
                };
                //check via zone capacity is there or not


                _context.StockItems.Add(stockItem);
                var res = await _context.SaveChangesAsync();



                if (res <= 0)
                {

                    throw new Exception("kuch nhi huya");
                    

                }
                await _zoneService.UpdateZoneUsageAsync(zoneAllocation, addStockDto.Quantity);
                var TransactionRequest = new CreateTransactionRequest {
                
                    ItemId = stockItem.ItemId,
                    Quantity = stockItem.Quantity,
                    Type = "Inbound"
                }; 

                var TransactionResponse = await _transactionService.CreateTransactionAsync(TransactionRequest);
                
                if (!TransactionResponse.IsSuccess)
                {
                    throw new InvalidOperationException("Failed to create transaction record");
                }
                return stockItem;
               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG ERROR: {ex.Message}");
                throw;
                
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

                await _zoneService.UpdateZoneUsageAsync(StockItem.ZoneId, Quantity);
              
             

                var transactionRequest = new CreateTransactionRequest { ItemId = StockItem.ItemId,
                    Quantity = Quantity,
                    Type = "Outbound"
                };

                 var TransactionLogResponse = await _transactionService.CreateTransactionAsync(transactionRequest);


            if (!TransactionLogResponse.IsSuccess)
                {
                    throw new Exception("Failed to create transaction record");
                }

                return true;

            
        }


    }
}
