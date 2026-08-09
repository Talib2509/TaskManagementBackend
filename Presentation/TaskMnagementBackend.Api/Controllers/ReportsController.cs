using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.Services;

namespace TaskMnagementBackend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        private Guid GetCurrentUserId()
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("UserId")?.Value;

            if (string.IsNullOrWhiteSpace(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
                throw new UnauthorizedAccessException("İstifadəçi autentifikasiyadan keçməyib.");

            return userId;
        }

        [HttpGet("team/{teamId}/excel")]
        public async Task<IActionResult> GetTeamReportExcel(
            int teamId,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            var data = await _reportService.GetTeamPerformanceDataAsync(teamId, fromDate, toDate);
            var fileBytes = await _reportService.ExportExcelAsync(data);
            var fileName = $"Team_{teamId}_Performance_{DateTime.UtcNow:yyyyMMdd}.xlsx";

            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("team/{teamId}/pdf")]
        public async Task<IActionResult> GetTeamReportPdf(
            int teamId,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            var data = await _reportService.GetTeamPerformanceDataAsync(teamId, fromDate, toDate);
            var fileBytes = await _reportService.ExportPdfAsync(data);
            var fileName = $"Team_{teamId}_Performance_{DateTime.UtcNow:yyyyMMdd}.pdf";

            return File(fileBytes, "application/pdf", fileName);
        }

        [HttpGet("user/{userId}/excel")]
        public async Task<IActionResult> GetUserReportExcel(
            Guid userId,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            var data = await _reportService.GetUserPerformanceDataAsync(userId, fromDate, toDate);
            var fileBytes = await _reportService.ExportExcelAsync(data);
            var fileName = $"User_{userId}_Performance_{DateTime.UtcNow:yyyyMMdd}.xlsx";

            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("user/{userId}/pdf")]
        public async Task<IActionResult> GetUserReportPdf(
            Guid userId,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            var data = await _reportService.GetUserPerformanceDataAsync(userId, fromDate, toDate);
            var fileBytes = await _reportService.ExportPdfAsync(data);
            var fileName = $"User_{userId}_Performance_{DateTime.UtcNow:yyyyMMdd}.pdf";

            return File(fileBytes, "application/pdf", fileName);
        }

        [HttpGet("me/excel")]
        public async Task<IActionResult> GetMyReportExcel(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            var userId = GetCurrentUserId();
            var data = await _reportService.GetUserPerformanceDataAsync(userId, fromDate, toDate);
            var fileBytes = await _reportService.ExportExcelAsync(data);
            var fileName = $"My_Performance_{DateTime.UtcNow:yyyyMMdd}.xlsx";

            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("me/pdf")]
        public async Task<IActionResult> GetMyReportPdf(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            var userId = GetCurrentUserId();
            var data = await _reportService.GetUserPerformanceDataAsync(userId, fromDate, toDate);
            var fileBytes = await _reportService.ExportPdfAsync(data);
            var fileName = $"My_Performance_{DateTime.UtcNow:yyyyMMdd}.pdf";

            return File(fileBytes, "application/pdf", fileName);
        }

        [HttpGet("company/{companyId}/excel")]
        public async Task<IActionResult> GetCompanyReportExcel(
            int companyId,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            var data = await _reportService.GetCompanyPerformanceDataAsync(companyId, fromDate, toDate);
            var fileBytes = await _reportService.ExportExcelAsync(data);
            var fileName = $"Company_{companyId}_Performance_{DateTime.UtcNow:yyyyMMdd}.xlsx";

            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("company/{companyId}/pdf")]
        public async Task<IActionResult> GetCompanyReportPdf(
            int companyId,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            var data = await _reportService.GetCompanyPerformanceDataAsync(companyId, fromDate, toDate);
            var fileBytes = await _reportService.ExportPdfAsync(data);
            var fileName = $"Company_{companyId}_Performance_{DateTime.UtcNow:yyyyMMdd}.pdf";

            return File(fileBytes, "application/pdf", fileName);
        }
    }
}
