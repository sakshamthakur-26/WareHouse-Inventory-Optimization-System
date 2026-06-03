using System.Collections.Generic;
using Zone.DTOs;
using Zone.Services;
namespace Zone.Services
{
    public interface IZoneService
    {
        Task<ServiceResult<IEnumerable<ZoneResponse>>> GetAllAsync();
        Task<ServiceResult<ZoneResponse>> GetByIdAsync(int id);
        Task<ServiceResult<ZoneResponse>> CreateAsync(CreateZoneRequest request);
        Task<ServiceResult<object>> UpdateAsync(int id, CreateZoneRequest request);
        Task<ServiceResult<object>> DeleteAsync(int id);

        // Capacity based methods preserved
        Task<bool> CheckAvailableCapacityAsync(int zoneId, int requiredSpace);
        Task UpdateZoneUsageAsync(int zoneId, int spaceUsed);
    }
}
