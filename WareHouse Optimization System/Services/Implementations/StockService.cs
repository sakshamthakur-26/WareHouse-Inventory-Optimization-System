using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using WareHouse_Optimization_System.Controllers;
using WareHouse_Optimization_System.Db;
using WareHouse_Optimization_System.DTOs.Stock;
using WareHouse_Optimization_System.DTOs.Transaction;
using WareHouse_Optimization_System.Models;
using WareHouse_Optimization_System.Services.Interfaces;

namespace WareHouse_Optimization_System.Services.Implementations
{
    public class StockService : IStockService
    {

        private readonly WarehouseDbContext? _context;

        private readonly DemoService? _dservice = null;
        private readonly ZoneService? _zoneService = null;
        private readonly TransactionService? _transactionService = null;
        public StockService(WarehouseDbContext _dbContext,ZoneService zoneService,TransactionService transactionService)
        {
            _context = _dbContext;
            _dservice = new DemoService();
            _zoneService = zoneService;
            _transactionService = transactionService;
        }


        public async Task<List<StockItem>> GetAllStockItems()
        {
            return await _context.StockItems.ToListAsync();
        }

        public async Task<StockItem> GetStockItemByIdAsync(int id)
        {
           
              return await _context.StockItems.FindAsync(id);
                
        }

        public async Task<ServiceResult<StockItem>> AddStockItemAsync(AddStockDto addStockDto)
        {
          

                //write find categordId via category name and zoneId via zone name
                if (addStockDto.Quantity <= 0)
                {
                    return ServiceResult<StockItem>.Failure("Enter a valid quantity greater than 0.");
                }

                int categoryId = await _dservice.GetCategoryId(addStockDto.CategoryName);
                //return zone id and zone name
                var zoneAllocation = await _dservice.CheckZoneCapacity(addStockDto.CategoryName, addStockDto.Quantity);
                var zoneCapacity = await _zoneService.CheckAvailableCapacityAsync(zoneAllocation, addStockDto.Quantity);

                if (!zoneCapacity)
                {
                    return ServiceResult<StockItem>.Failure($"Zone capacity exceeded for category '{addStockDto.CategoryName}'.");
                }

                var stockItem = new StockItem
                {
                    Name = addStockDto.ItemName,
                  
                    CategoryId = categoryId,
                    Quantity = addStockDto.Quantity,
                    ZoneId = zoneAllocation,
                };
                //check via zone capacity is there or not


                 using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    _context.StockItems.Add(stockItem);
                    await _context.SaveChangesAsync();
                     await _zoneService.UpdateZoneUsageAsync(zoneAllocation, addStockDto.Quantity);
                    var TransactionRequest = new CreateTransactionRequest
                    {

                        ItemId = stockItem.ItemId,
                        Quantity = stockItem.Quantity,
                        Type = "Inbound"
                    };
                    var TransactionResponse = await _transactionService.CreateTransactionAsync(TransactionRequest);
                    if (!TransactionResponse.IsSuccess)
                    {
                        return ServiceResult<StockItem>.Failure("Failed to create transaction record");
                    }
                    await transaction.CommitAsync();

                    return ServiceResult<StockItem>.Success(stockItem);

                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
        }


        public async Task<ServiceResult<bool>> RemoveStockAsync(int ItemId, int Quantity)
        {
            
                if (Quantity <= 0)
                {
                return ServiceResult<bool>.Failure("Quantity to remove must be greater than zero.");
                }

                var StockItem = await _context.StockItems.FindAsync(ItemId);
                if (StockItem == null)
                {
                return ServiceResult<bool>.Failure("Stock item not found");

                }
                int newQuantity = StockItem.Quantity - Quantity;
                if(newQuantity<0)                 {
                return ServiceResult<bool>.Failure("Insufficient stock quantity");
                }
                StockItem.Quantity = newQuantity;


            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.SaveChangesAsync();
                await _zoneService.UpdateZoneUsageAsync(StockItem.ZoneId, Quantity);
                var transactionRequest = new CreateTransactionRequest
                {
                    ItemId = StockItem.ItemId,
                    Quantity = Quantity,
                    Type = "Outbound"
                };

                var TransactionLogResponse = await _transactionService.CreateTransactionAsync(transactionRequest);
                if (!TransactionLogResponse.IsSuccess)
                {
                   return ServiceResult<bool>.Failure("Failed to create transaction record");
                }

                await transaction.CommitAsync();
                return ServiceResult<bool>.Success(true);



            }
            catch
            {
                await transaction.RollbackAsync();
                throw;

            }


            
        }


    }
}
