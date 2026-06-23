using Microsoft.EntityFrameworkCore;
using WareHouse_Optimization_System.Db;
using WareHouse_Optimization_System.DTOs.Dasboard;
using WareHouse_Optimization_System.Services.Interfaces;

namespace WareHouse_Optimization_System.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly WarehouseDbContext _context; // Adjust to your actual DbContext name

        public DashboardService(WarehouseDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<DashboardDto>> GetDashboardMetricsAsync()
        {
            try
            {
                // Execute all queries asynchronously
                var totalZones = await _context.Zones.CountAsync();
                var totalVendors = await _context.Vendors.CountAsync();
                var totalStockItems = await _context.StockItems.CountAsync();

                // Count items where stock is at or below the threshold
                var totalLowStockAlerts = await _context.StockItems
                    .Where(item => item.Quantity <= item.MinimumThreshold)
                    .CountAsync();

                var lowStockItemsList = await _context.StockItems
                    .Where(item => item.Quantity <= item.MinimumThreshold)
                    .Select(item => new LowStockItemDto
                    {
                        ItemName = item.Name,
                        Quantity = item.Quantity
                    })
                    .ToListAsync();

                // Map to DTO
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
                // If the database is down or a query fails, return a safe failure result
                return ServiceResult<DashboardDto>.Failure($"Failed to load dashboard metrics: {ex.Message}");
            }
        }

       
        
    }
}
