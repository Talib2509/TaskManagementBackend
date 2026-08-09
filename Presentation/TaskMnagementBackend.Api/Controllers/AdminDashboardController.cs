using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Api.Controllers
{
    [Route("api/admin/dashboard")]
    [ApiController]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.SuperAdmin}")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public AdminDashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats([FromQuery] int days = 30)
        {
            var stats = await _dashboardService.GetAdminDashboardStatsAsync(days);
            return Ok(stats);
        }
    }
}
