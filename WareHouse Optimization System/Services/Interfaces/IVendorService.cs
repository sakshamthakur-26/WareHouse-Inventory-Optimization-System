namespace Vendor_Management.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VendorManagement.Models;
    using VendorManagement.Services;
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