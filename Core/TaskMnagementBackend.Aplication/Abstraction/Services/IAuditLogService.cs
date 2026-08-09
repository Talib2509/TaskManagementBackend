using System;
using System.Threading;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Common.Pagination;
using TaskMnagementBackend.Aplication.DTOs.AuditLog;

namespace TaskMnagementBackend.Aplication.Abstraction.Services
{
    public interface IAuditLogService
    {
        Task LogAsync(
            string action,
            string entityType,
            string? entityId = null,
            string? details = null,
            Guid? userId = null,
            string? userEmail = null,
            string? userName = null,
            string? ipAddress = null,
            CancellationToken cancellationToken = default);

        Task<PagedResult<AuditLogDto>> GetLogsAsync(
            AuditLogFilterDto filter,
            CancellationToken cancellationToken = default);

        Task<AuditLogDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
