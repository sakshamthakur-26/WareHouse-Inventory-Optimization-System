using Microsoft.EntityFrameworkCore;
using VendorManagement.DTOs;
using VendorManagement.Models;
using WareHouse_Optimization_System.Db;

namespace VendorManagement.Services
{
    public class VendorService
    {
        private readonly WarehouseDbContext _context;
        public VendorService(WarehouseDbContext context)
        {
            _context = context;
        }

        // 1. Get All Vendors
        public async Task<IEnumerable<Vendor>> GetVendors()
        {
            return await _context.Vendors.ToListAsync();
        }

        // 2. Get Vendor By ID
        public async Task<Vendor?> GetVendor(int id)
        {
            return await _context.Vendors.FindAsync(id);
        }

        // 3. Create Vendor
        public async Task<Vendor> CreateVendor(Vendor vendor)
        {
            await _context.Vendors.AddAsync(vendor);
            await _context.SaveChangesAsync();
            return vendor;
        }

        // 4. Update Vendor
        public async Task UpdateVendor(Vendor vendor)
        {
            _context.Entry(vendor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // 5. Remove Vendor 
        public async Task<bool> RemoveVendor(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor == null) return false;

            _context.Vendors.Remove(vendor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}