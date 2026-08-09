using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.DTOs.AuditLog;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Api.Controllers
{
    [Route("api/audit-logs")]
    [ApiController]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.SuperAdmin}")]
    public class AuditLogsController : ControllerBase
    {
        private readonly IAuditLogService _auditLogService;

        public AuditLogsController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuditLogs([FromQuery] AuditLogFilterDto filter)
        {
            var logs = await _auditLogService.GetLogsAsync(filter);
            return Ok(logs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var log = await _auditLogService.GetByIdAsync(id);
            if (log == null)
                return NotFound("Audit log tapılmadı.");

            return Ok(log);
        }
    }
}
