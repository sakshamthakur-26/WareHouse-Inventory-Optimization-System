using WareHouse_Optimization_System.DTOs.Category;
using WareHouse_Optimization_System.Models;

namespace WareHouse_Optimization_System.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<ServiceResult<CategoryResponseDto>> GetZoneForCategoryAsync(string categoryName);
        Task<ServiceResult<Category>> CreateCategoryAsync(CreateCategoryDto request);
        Task<ServiceResult<List<string>>> GetAllCategoriesAsync();

        Task<ServiceResult<Category>> AssignCategoryToZoneAsync(AssignCategoryDto request);
    }
}
