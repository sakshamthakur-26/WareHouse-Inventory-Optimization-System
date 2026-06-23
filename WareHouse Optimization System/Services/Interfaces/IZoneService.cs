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
        //Task DeleteAsync(int id);

        // Capacity based methods kept internally in implementation (not part of public contract)

        Task<bool> CheckAvailableCapacityAsync(int zoneId, int requiredSpace);
        Task UpdateZoneUsageAsync(int zoneId, int spaceUsed);
    }
}