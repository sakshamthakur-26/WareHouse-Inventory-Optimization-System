using WareHouse_Optimization_System.DTOs.Zone;
using WareHouse_Optimization_System.Services;

namespace WareHouse_Optimization_System.Services.Interfaces
{
    public interface IZoneService
    {
        Task<IEnumerable<ZoneResponse>> GetAllAsync();
        Task<ZoneResponse> GetByIdAsync(int id);
        Task<ZoneResponse> CreateAsync(CreateZoneRequest request);
        Task<ServiceResult<object>> UpdateAsync(int id, UpdateZoneRequest request);
        Task<bool> CheckAvailableCapacityAsync(int zoneId, int requiredSpace);
        Task<ServiceResult<bool>> UpdateZoneUsageAsync(int zoneId, int spaceUsed);
    }
}