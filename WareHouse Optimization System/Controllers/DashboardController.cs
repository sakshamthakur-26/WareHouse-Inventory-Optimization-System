using Microsoft.AspNetCore.Mvc;
using WareHouse_Optimization_System.DTOs.Dasboard;
using WareHouse_Optimization_System.Services;
using WareHouse_Optimization_System.Services.Interfaces;

namespace WareHouse_Optimization_System.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {

        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("summary")]
        public async Task<ActionResult<ServiceResult<DashboardDto>>> GetSummaryMetrics()
        {
            var result = await _dashboardService.GetDashboardMetricsAsync();

            if (!result.IsSuccess)
            {
                return BadRequest(result); // Returns the ServiceResult Error message
            }

            return Ok(result.Data); // Returns just the DashboardSummaryDto as JSON
        }
    }
}
