using Zone.DTOs;
namespace Zone.Services
{
    public interface IZoneService
    {
        Task<IEnumerable<ZoneResponse>> GetAllAsync();
        Task<ZoneResponse> GetByIdAsync(int id); // why do we use IEnnumerable and when to ??

        Task<ZoneResponse> CreateAsync(CreateZoneRequest request);
        Task UpdateAsync(int id, CreateZoneRequest request);
        Task DeleteAsync(int id);


        ////////////////////////////////////////////////////
        //                 CAPACITY BASED


        Task<bool> CheckAvailableCapacityAsync(int zoneId, int requiredSpace);
        Task UpdateZoneUsageAsync(int zoneId, int spaceUsed);


    }
}
