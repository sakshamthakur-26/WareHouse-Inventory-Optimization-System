using WareHouse_Optimization_System.DTOs.Dasboard;

namespace WareHouse_Optimization_System.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<ServiceResult<DashboardDto>> GetDashboardMetricsAsync();
    }
}
