using System;
using System.Collections.Generic;

namespace TaskMnagementBackend.Aplication.DTOs.Dashboard
{
    public class CompanyDashboardStatsDto
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public int TotalTeams { get; set; }
        public int TotalMembers { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int OverdueTasks { get; set; }
        public double CompletionPercentage { get; set; }
        public double OverduePercentage { get; set; }

        public List<TeamProductivityDto> TeamsProductivity { get; set; } = new();
        public List<ActiveMemberDto> TopActiveMembers { get; set; } = new();
    }

    public class TeamProductivityDto
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public string? TeamLeadName { get; set; }
        public int MemberCount { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int OverdueTasks { get; set; }
        public double CompletionPercentage { get; set; }
        public double OverduePercentage { get; set; }
    }

    public class ActiveMemberDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
        public string? JobTitle { get; set; }
        public int CompletedTasksCount { get; set; }
        public int TotalAssignedTasks { get; set; }
        public int ActivityCount { get; set; }
    }
}
