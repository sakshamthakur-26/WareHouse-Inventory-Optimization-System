using WareHouse_Optimization_System.DTOs.Zone;

namespace WareHouse_Optimization_System.Services.Interfaces
{
    public interface IZoneService
    {
        Task<bool> CheckAvailableCapacityAsync(int zoneId, int requiredSpace);
        Task<ZoneResponse> CreateAsync(CreateZoneRequest request);
        Task DeleteAsync(int id);
        Task<IEnumerable<ZoneResponse>> GetAllAsync();
        Task<ZoneResponse> GetByIdAsync(int id);
        Task UpdateAsync(int id, CreateZoneRequest request);
        Task UpdateZoneUsageAsync(int zoneId, int spaceUsed);
    }
}