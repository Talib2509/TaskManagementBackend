using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common.Pagination;
using TaskMnagementBackend.Aplication.DTOs.AuditLog;
using TaskMnagementBackend.Domain.Entities;
using TaskMnagementBackend.Persistence.Context;

namespace TaskMnagementBackend.Infrastructure.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly AppDbContext _dbContext;

        public AuditLogService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LogAsync(
            string action,
            string entityType,
            string? entityId = null,
            string? details = null,
            Guid? userId = null,
            string? userEmail = null,
            string? userName = null,
            string? ipAddress = null,
            CancellationToken cancellationToken = default)
        {
            var auditLog = new AuditLog
            {
                Action = action,
                EntityType = entityType,
                EntityId = entityId,
                Details = details,
                UserId = userId,
                UserEmail = userEmail,
                UserName = userName,
                IpAddress = ipAddress,
                Timestamp = DateTime.UtcNow
            };

            await _dbContext.AuditLogs.AddAsync(auditLog, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<PagedResult<AuditLogDto>> GetLogsAsync(
            AuditLogFilterDto filter,
            CancellationToken cancellationToken = default)
        {
            var query = _dbContext.AuditLogs.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Action))
                query = query.Where(x => x.Action == filter.Action);

            if (!string.IsNullOrWhiteSpace(filter.EntityType))
                query = query.Where(x => x.EntityType == filter.EntityType);

            if (filter.UserId.HasValue)
                query = query.Where(x => x.UserId == filter.UserId.Value);

            if (filter.FromDate.HasValue)
                query = query.Where(x => x.Timestamp >= filter.FromDate.Value);

            if (filter.ToDate.HasValue)
                query = query.Where(x => x.Timestamp <= filter.ToDate.Value);

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var term = filter.SearchTerm.ToLower();
                query = query.Where(x => (x.Action != null && x.Action.ToLower().Contains(term))
                                      || (x.Details != null && x.Details.ToLower().Contains(term))
                                      || (x.UserEmail != null && x.UserEmail.ToLower().Contains(term))
                                      || (x.UserName != null && x.UserName.ToLower().Contains(term)));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var page = filter.Page <= 0 ? 1 : filter.Page;
            var pageSize = filter.PageSize <= 0 ? 20 : filter.PageSize;

            var items = await query
                .OrderByDescending(x => x.Timestamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new AuditLogDto
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    UserEmail = x.UserEmail,
                    UserName = x.UserName,
                    Action = x.Action,
                    EntityType = x.EntityType,
                    EntityId = x.EntityId,
                    Details = x.Details,
                    IpAddress = x.IpAddress,
                    Timestamp = x.Timestamp
                })
                .ToListAsync(cancellationToken);

            return PagedResult<AuditLogDto>.Create(items, totalCount, page, pageSize);
        }

        public async Task<AuditLogDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var log = await _dbContext.AuditLogs
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (log == null) return null;

            return new AuditLogDto
            {
                Id = log.Id,
                UserId = log.UserId,
                UserEmail = log.UserEmail,
                UserName = log.UserName,
                Action = log.Action,
                EntityType = log.EntityType,
                EntityId = log.EntityId,
                Details = log.Details,
                IpAddress = log.IpAddress,
                Timestamp = log.Timestamp
            };
        }
    }
}
