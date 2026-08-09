using System;
using System.Collections.Generic;

namespace TaskMnagementBackend.Aplication.DTOs.Reporting
{
    public class PerformanceReportDataDto
    {
        public string Title { get; set; } = string.Empty;
        public string ScopeName { get; set; } = string.Empty;
        public string ScopeType { get; set; } = string.Empty; // "Team", "User", "Company"
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int PendingTasks { get; set; }
        public int OverdueTasks { get; set; }
        public double CompletionPercentage { get; set; }
        public double OverduePercentage { get; set; }

        public List<ReportTaskItemDto> Tasks { get; set; } = new();
        public List<ReportMemberPerformanceDto> Members { get; set; } = new();
    }

    public class ReportTaskItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string? AssignedUserName { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsOverdue { get; set; }
    }

    public class ReportMemberPerformanceDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Role { get; set; }
        public int AssignedTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int OverdueTasks { get; set; }
        public double CompletionRate { get; set; }
    }
}
