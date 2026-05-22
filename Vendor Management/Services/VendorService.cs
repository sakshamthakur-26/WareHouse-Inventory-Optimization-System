using VendorManagement.Models;
using VendorManagement.DTOs;
namespace VendorManagement.Services
{
    

    public class VendorService
    {
        private readonly IVendorRepository _repo;
        public VendorService(IVendorRepository repo) { _repo = repo; }

        public async Task<IEnumerable<Vendor>> GetVendors() => await _repo.GetAllAsync();

        public async Task<Vendor> GetVendor(int id) => await _repo.GetByIdAsync(id);

        public async Task<Vendor> CreateVendor(Vendor vendor)
        {
            await _repo.AddAsync(vendor);
            return vendor;
        }

        public async Task UpdateVendor(Vendor vendor)
        {
            await _repo.UpdateAsync(vendor);
        }

        public async Task<bool> RemoveVendor(int id)
        {
            var exists = await _repo.GetByIdAsync(id);
            if (exists == null) return false;

            await _repo.DeleteAsync(id);
            return true;
        }
    }
}