using System;
using System.Collections.Generic;

namespace TaskMnagementBackend.Aplication.DTOs.Dashboard
{
    public class AdminDashboardStatsDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int DeactivatedUsers { get; set; }
        public int TotalCompanies { get; set; }
        public int TotalTeams { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int PendingTasks { get; set; }
        public int OverdueTasks { get; set; }
        public double OverallCompletionRate { get; set; }

        public List<ActivityChartPointDto> ActivityChart { get; set; } = new();
    }

    public class ActivityChartPointDto
    {
        public string Date { get; set; } = string.Empty;
        public int NewUsersCount { get; set; }
        public int CreatedTasksCount { get; set; }
        public int CompletedTasksCount { get; set; }
        public int ActivityCount { get; set; }
    }
}
