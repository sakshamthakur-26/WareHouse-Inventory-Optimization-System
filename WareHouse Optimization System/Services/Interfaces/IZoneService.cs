using WareHouse_Optimization_System.DTOs.Zone;
using WareHouse_Optimization_System.Services;

namespace WareHouse_Optimization_System.Services.Interfaces
{
    public interface IZoneService
    {
        Task<ServiceResult<IEnumerable<ZoneResponse>>> GetAllAsync();
        Task<ServiceResult<ZoneResponse>> GetByIdAsync(int id);
        Task<ServiceResult<ZoneResponse>> CreateAsync(CreateZoneRequest request);
        Task<ServiceResult<object>> UpdateAsync(int id, CreateZoneRequest request);
        Task<ServiceResult<object>> DeleteAsync(int id);

        // Capacity based methods kept internally in implementation (not part of public contract)
    }
}