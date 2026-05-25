using Microsoft.EntityFrameworkCore;
using WareHouse_Optimization_System.Db;
using WareHouse_Optimization_System.DTOs;
using WareHouse_Optimization_System.Models;

namespace WareHouse_Optimization_System.Services.Implementations
{
    public class CategoryService
    {
        public readonly WarehouseDbContext _context;
        public CategoryService(WarehouseDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<int>> GetZoneForCategoryAsync(string categoryName)
        {
           
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == categoryName.ToLower());

            if (category == null)
            {
                throw new Exception($"Category {categoryName} does not exist.");
            }


            return  ServiceResult<int>.Success(category.DedicatedZoneId) ;
        }

        public async Task<ServiceResult<Category>> CreateCategoryAsync(CreateCategoryDto request)
        {
          
            var zoneExists = await _context.Zones.AnyAsync(z => z.ZoneId == request.DedicatedZoneId);
            if (!zoneExists)
            {
                return ServiceResult<Category>.Failure($"Cannot create category. Zone ID {request.DedicatedZoneId} does not exist in the warehouse.");
            }

           
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == request.Name.ToLower());

            if (existingCategory != null)
            {
                return ServiceResult<Category>.Failure("This Category already exists.");
            }

            
            var newCategory = new Category
            {
                Name = request.Name,
                DedicatedZoneId = request.DedicatedZoneId
            };

            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();

            return ServiceResult<Category>.Success(newCategory);
        }
    }
}
