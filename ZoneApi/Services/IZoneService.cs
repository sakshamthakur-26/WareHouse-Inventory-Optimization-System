using System.Collections.Generic;
using System.Threading.Tasks;
using Zone.DTOs;
using WareHouse_Optimization_System.Services;
namespace Zone.Services
{
    public interface IZoneService
    {
        Task<ServiceResult<IEnumerable<ZoneResponse>>> GetAllAsync();
        Task<ServiceResult<ZoneResponse>> GetByIdAsync(int id);
        Task<ServiceResult<ZoneResponse>> CreateAsync(CreateZoneRequest request);
        Task<ServiceResult<object>> UpdateAsync(int id, UpdateZoneRequest request);
        Task<ServiceResult<object>> DeleteAsync(int id);
        // Capacity based methods are implementation-private
    }
}
