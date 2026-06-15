namespace WareHouse_Optimization_System.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WareHouse_Optimization_System.Models;
    using WareHouse_Optimization_System.Services;

    public interface IVendorService
    {
        Task<ServiceResult<IEnumerable<Vendor>>> GetAllAsync();
        Task<ServiceResult<Vendor>> GetByIdAsync(int id);
        Task<ServiceResult<Vendor>> AddAsync(Vendor vendor);
        Task<ServiceResult<Vendor>> UpdateAsync(Vendor vendor);
        //Task<ServiceResult<bool>> DeleteAsync(int id);
    }
}
