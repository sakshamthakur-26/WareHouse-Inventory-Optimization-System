using Microsoft.EntityFrameworkCore;
using WareHouse_Optimization_System.Db;
using WareHouse_Optimization_System.DTOs.Dasboard;
using WareHouse_Optimization_System.Services.Interfaces;

namespace WareHouse_Optimization_System.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly WarehouseDbContext _context; 

        public DashboardService(WarehouseDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<DashboardDto>> GetDashboardMetricsAsync()
        {
            try
            {
                var totalZones = await _context.Zones.CountAsync();
                var totalVendors = await _context.Vendors.CountAsync(c=>c.IsActive);
                var totalStockItems = await _context.StockItems.CountAsync();

                var totalLowStockAlerts = await _context.StockItems
                    .Where(item => item.Quantity <= item.MinimumThreshold)
                    .CountAsync();

                // REPLACED: now includes ItemId, CategoryName, VendorName
                var lowStockItemsList = await _context.StockItems
                    .Where(item => item.Quantity <= item.MinimumThreshold)
                    .Join(_context.Categories, s => s.CategoryId, c => c.CategoryId, (s, c) => new { s, c })
                    .GroupJoin(_context.Vendors, sc => sc.s.VendorId, v => v.VendorId, (sc, vendors) => new { sc.s, sc.c, vendors })
                    .SelectMany(x => x.vendors.DefaultIfEmpty(), (x, v) => new LowStockItemDto
                    {
                        ItemId = x.s.ItemId,
                        ItemName = x.s.Name,
                        Quantity = x.s.Quantity,
                        CategoryName = x.c.Name,
                        VendorName = v != null ? v.Name : null
                    })
                    .ToListAsync();

                var summaryDto = new DashboardDto
                {
                    TotalZones = totalZones,
                    TotalLowStockAlerts = totalLowStockAlerts,
                    TotalActiveVendors = totalVendors,
                    TotalStockItems = totalStockItems,
                    LowStockItems = lowStockItemsList
                };

                return ServiceResult<DashboardDto>.Success(summaryDto);
            }
            catch (Exception ex)
            {
                return ServiceResult<DashboardDto>.Failure($"Failed to load dashboard metrics: {ex.Message}");
            }
        }



    }
}
