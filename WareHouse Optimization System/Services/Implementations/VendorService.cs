namespace VendorManagement.Services
{
    using Microsoft.EntityFrameworkCore;
    using Vendor_Management.Services;
    using VendorManagement.Models;
    using WareHouse_Optimization_System.Db;
    using WareHouse_Optimization_System.Services;

    public class VendorService : IVendorService
    {
        private readonly WarehouseDbContext _context;

        public VendorService(WarehouseDbContext context)
        {
            _context = context;
        }

        //  Get All Vendors wrapped in ServiceResult
        public async Task<ServiceResult<IEnumerable<Vendor>>> GetAllAsync()
        {
            var vendors = await _context.Vendors.ToListAsync();
            return ServiceResult<IEnumerable<Vendor>>.Success(vendors);
        }

        // Get Vendor By ID with Not Found validation
        public async Task<ServiceResult<Vendor>> GetByIdAsync(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor == null)
            {
                return ServiceResult<Vendor>.Failure("Vendor not found.");
            }
            return ServiceResult<Vendor>.Success(vendor);
        }

        public async Task<ServiceResult<Vendor>> AddAsync(Vendor vendor)
        {
            // Email must contain an "@" character
            if (string.IsNullOrWhiteSpace(vendor.Email) || !vendor.Email.Contains("@"))
            {
                return ServiceResult<Vendor>.Failure("Invalid email address. It must contain an '@' character.");
            }

            // Validation for 10 digit phone number
            string phoneStr = vendor.PhoneNumber?.Trim() ?? string.Empty;
            if (phoneStr.Length != 10 || !phoneStr.All(char.IsDigit))
            {
                return ServiceResult<Vendor>.Failure("Phone number must contain exactly 10 digits and only numbers.");
            }

                                       //  Ensure no duplicate vendor PhoneNumber or email exists 

            bool duplicateExists = await _context.Vendors.AnyAsync(v =>
              v.PhoneNumber == vendor.PhoneNumber ||
              v.Email.ToLower() == vendor.Email.ToLower()
                );

            if (duplicateExists)
            {
                return ServiceResult<Vendor>.Failure("A vendor with the same phoneNumber or email already exists.");
            }

            await _context.Vendors.AddAsync(vendor);
            await _context.SaveChangesAsync();
            return ServiceResult<Vendor>.Success(vendor);
        }

        // Update Vendor details safely
        public async Task<ServiceResult<Vendor>> UpdateAsync(Vendor vendor)
        {
            var existingVendor = await _context.Vendors.FindAsync(vendor.VendorId);
            if (existingVendor == null)
            {
                return ServiceResult<Vendor>.Failure("Vendor not found to update.");
            }

            // Validation check during update
            if (string.IsNullOrWhiteSpace(vendor.Email) || !vendor.Email.Contains("@"))
            {
                return ServiceResult<Vendor>.Failure("Invalid email address format during update.");
            }

            bool duplicateExists = await _context.Vendors.AnyAsync(v =>
            v.VendorId != vendor.VendorId && 
              (v.PhoneNumber == vendor.PhoneNumber ||
              v.Email.ToLower() == vendor.Email.ToLower()
                ));

            if (duplicateExists)
            {
                return ServiceResult<Vendor>.Failure("A vendor with the same phoneNumber or email already exists.");
            }

            // Map updated details
            existingVendor.Name = vendor.Name;
            existingVendor.Email = vendor.Email;
            existingVendor.PhoneNumber = vendor.PhoneNumber;
            existingVendor.GoodsSupplied = vendor.GoodsSupplied;
            existingVendor.IsActive = vendor.IsActive;

            _context.Entry(existingVendor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return ServiceResult<Vendor>.Success(existingVendor);
        }

        // 5. Remove Vendor from system
        //public async Task<ServiceResult<bool>> DeleteAsync(int id)
        //{
        //    var vendor = await _context.Vendors.FindAsync(id);
        //    if (vendor == null)
        //    {
        //        return ServiceResult<bool>.Failure("Vendor not found to delete.");
        //    }

        //    _context.Vendors.Remove(vendor);
        //    await _context.SaveChangesAsync();
        //    return ServiceResult<bool>.Success(true);
        //}
    }
}
