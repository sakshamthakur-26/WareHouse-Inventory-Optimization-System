
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using WareHouse_Optimization_System.Controllers;
using WareHouse_Optimization_System.Db;
using WareHouse_Optimization_System.DTOs.Stock;
using WareHouse_Optimization_System.DTOs.Transaction;
using WareHouse_Optimization_System.DTOs.Zone;
using WareHouse_Optimization_System.Models;
using WareHouse_Optimization_System.Services.Interfaces;

namespace WareHouse_Optimization_System.Services.Implementations
{
    public class StockService : IStockService
    {

        private readonly WarehouseDbContext _context;
        private readonly IZoneService _zoneService;
        private readonly ITransactionService _transactionService;
        private readonly ICategoryService _categoryService;
        private readonly IVendorService _vendorService;





    
    
        public StockService(WarehouseDbContext _dbContext, IZoneService zoneService,ITransactionService transactionService,ICategoryService categoryService,IVendorService vendorService)
        {
            _context = _dbContext;

            _zoneService = zoneService;
            _transactionService = transactionService;
            _categoryService = categoryService;
            _vendorService = vendorService;
        }




        public async Task<ServiceResult<List<StockItemDto>>> GetAllStockItems()
        {

            var query = from stock in _context.StockItems
                        join category in _context.Categories on stock.CategoryId equals category.CategoryId
                        join zone in _context.Zones on stock.ZoneId equals zone.ZoneId
                        join vendor in _context.Vendors on stock.VendorId equals vendor.VendorId into vendorGroup
                        from vendor in vendorGroup.DefaultIfEmpty()

                        select new StockItemDto
                        {
                            ItemId = stock.ItemId,
                            Name = stock.Name,
                            Category = category.Name,
                            Quantity = stock.Quantity,
                            Zone = zone.Name,
                            Status = stock.Quantity == 0 ? "Out of Stock" :
                             (stock.MinimumThreshold.HasValue && stock.Quantity <= stock.MinimumThreshold.Value) ? "Low Stock" :
                             "In Stock"

                        };

            var stocks =  await query.ToListAsync();
            return ServiceResult<List<StockItemDto>>.Success(stocks);
        }

        public async Task<StockItem> GetStockItemByIdAsync(int id)
        {
           
              return await _context.StockItems.FindAsync(id);
                
        }

        public async Task<ServiceResult<StockItem>> AddStockItemAsync(AddStockDto addStockDto)
        {


            if(addStockDto.VendorName == "")
            {
                return ServiceResult<StockItem>.Failure("vendor name empty");


            }

            var vendorResponse = await _vendorService.GetVendorIdByNameAsync(addStockDto.VendorName);

            int vendorId = vendorResponse.Data;




            var vendor = await _context.Vendors.FindAsync(vendorId);

            if (vendor == null)
            {
                return ServiceResult<StockItem>.Failure("Vendor not found.");
            }


            if (addStockDto.Quantity <= 0)
                {
                    return ServiceResult<StockItem>.Failure("Enter a valid quantity greater than 0.");
                }

                var categoryResponse = await _categoryService.GetZoneForCategoryAsync(addStockDto.CategoryName);
                if(!categoryResponse.IsSuccess)
                {
                    return ServiceResult<StockItem>.Failure("zone not found for required category");
                }
                
                
               
               
                var zoneCapacity = await _zoneService.CheckAvailableCapacityAsync(categoryResponse.Data.DedicatedZoneId, addStockDto.Quantity);

                if (!zoneCapacity)
                {
                    return ServiceResult<StockItem>.Failure($"Zone capacity exceeded for category '{addStockDto.CategoryName}'.");
                }

               
              


                 using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    
                var existingStock = await _context.StockItems.FirstOrDefaultAsync(s => s.Name.ToLower() == addStockDto.ItemName.ToLower() && s.CategoryId == categoryResponse.Data.CategoryId && s.ZoneId == categoryResponse.Data.DedicatedZoneId);
                StockItem stockToProcess;

                if (existingStock != null)
                {

                                        
                    existingStock.Quantity += addStockDto.Quantity;
                    existingStock.VendorId = vendorId;
                    existingStock.MinimumThreshold = addStockDto.threshold;

                    _context.StockItems.Update(existingStock);
                    stockToProcess = existingStock;
                }else
                {
                    stockToProcess = new StockItem
                    {
                        Name = addStockDto.ItemName,

                        CategoryId = categoryResponse.Data.CategoryId,
                        Quantity = addStockDto.Quantity,
                        ZoneId = categoryResponse.Data.DedicatedZoneId,
                        MinimumThreshold = addStockDto.threshold,

                        VendorId = vendorId
                    };
                }

                _context.StockItems.Update(stockToProcess);
                    await _context.SaveChangesAsync();
                     await _zoneService.UpdateZoneUsageAsync(categoryResponse.Data.DedicatedZoneId, addStockDto.Quantity);
                var TransactionRequest = new CreateTransactionRequest
                {

                    ItemId = stockToProcess.ItemId,
                    Quantity = addStockDto.Quantity,
                    Type = "Inbound",
                    VendorId = vendorId
                };
                    var TransactionResponse = await _transactionService.CreateTransactionAsync(TransactionRequest);
                    if (!TransactionResponse.IsSuccess)
                    {
                        return ServiceResult<StockItem>.Failure("Failed to create transaction record");
                    }
                    await transaction.CommitAsync();

                    return ServiceResult<StockItem>.Success(stockToProcess);

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
                var zoneResponse = await _zoneService.UpdateZoneUsageAsync(StockItem.ZoneId, -Quantity);
                if (!zoneResponse.IsSuccess)
                {
                    await transaction.RollbackAsync(); 
                    return ServiceResult<bool>.Failure("Failed to free up zone capacity.");
                }
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
                if (StockItem.MinimumThreshold.HasValue && StockItem.Quantity <= StockItem.MinimumThreshold.Value)
                {
                    
                     //await _alertService.TriggerLowStockAlertAsync(stockItem.ItemId);
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
