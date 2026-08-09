using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.Services;

namespace TaskMnagementBackend.Api.Controllers
{
    [Route("api/company-dashboard")]
    [ApiController]
    [Authorize]
    public class CompanyDashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public CompanyDashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        private Guid GetCurrentUserId()
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("UserId")?.Value;

            if (string.IsNullOrWhiteSpace(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
                throw new UnauthorizedAccessException("İstifadəçi autentifikasiyadan keçməyib.");

            return userId;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetMyCompanyStats()
        {
            var userId = GetCurrentUserId();
            var stats = await _dashboardService.GetCompanyDashboardStatsAsync(userId);
            return Ok(stats);
        }

        [HttpGet("{companyId}/stats")]
        public async Task<IActionResult> GetCompanyStats(int companyId)
        {
            var userId = GetCurrentUserId();
            var stats = await _dashboardService.GetCompanyDashboardStatsAsync(userId, companyId);
            return Ok(stats);
        }
    }
}
