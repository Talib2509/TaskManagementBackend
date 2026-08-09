using System;
using System.Collections.Generic;

namespace TaskMnagementBackend.Aplication.DTOs.AuditLog
{
    public class AuditLogDto
    {
        public int Id { get; set; }
        public Guid? UserId { get; set; }
        public string? UserEmail { get; set; }
        public string? UserName { get; set; }
        public string Action { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty;
        public string? EntityId { get; set; }
        public string? Details { get; set; }
        public string? IpAddress { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class AuditLogFilterDto
    {
        public string? Action { get; set; }
        public string? EntityType { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? SearchTerm { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
