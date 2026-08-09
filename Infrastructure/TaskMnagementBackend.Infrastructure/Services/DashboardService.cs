using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.DTOs.Dashboard;
using TaskMnagementBackend.Domain.Entities;
using TaskMnagementBackend.Domain.Entities.Identity;
using TaskMnagementBackend.Domain.Enums;
using TaskMnagementBackend.Persistence.Context;

namespace TaskMnagementBackend.Infrastructure.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IStorageService _storageService;

        public DashboardService(
            AppDbContext dbContext,
            UserManager<AppUser> userManager,
            IStorageService storageService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _storageService = storageService;
        }

        public async Task<AdminDashboardStatsDto> GetAdminDashboardStatsAsync(int days = 30, CancellationToken cancellationToken = default)
        {
            if (days <= 0) days = 30;
            if (days > 365) days = 365;

            var totalUsers = await _userManager.Users.CountAsync(cancellationToken);
            var activeUsers = await _userManager.Users.CountAsync(u => u.IsActive && !u.IsDeleted, cancellationToken);
            var deactivatedUsers = totalUsers - activeUsers;

            var totalCompanies = await _dbContext.Companies.CountAsync(c => !c.IsDeleted, cancellationToken);
            var totalTeams = await _dbContext.Teams.CountAsync(t => !t.IsDeleted, cancellationToken);

            var now = DateTime.UtcNow;

            // Task statistics
            var totalTasks = await _dbContext.TaskItems.CountAsync(cancellationToken);
            var completedTasks = await _dbContext.TaskItems.CountAsync(t => t.Status == TaskItemStatus.Done, cancellationToken);
            var inProgressTasks = await _dbContext.TaskItems.CountAsync(t => t.Status == TaskItemStatus.InProgress, cancellationToken);
            var pendingTasks = await _dbContext.TaskItems.CountAsync(t => t.Status == TaskItemStatus.Todo, cancellationToken);
            var overdueTasks = await _dbContext.TaskItems.CountAsync(t => t.Status != TaskItemStatus.Done && t.DueDate.HasValue && t.DueDate.Value < now, cancellationToken);

            double completionRate = totalTasks > 0
                ? Math.Round((double)completedTasks / totalTasks * 100, 2)
                : 0.0;

            // Activity chart for the past 'days'
            var startDate = now.Date.AddDays(-(days - 1));
            var activityChart = new List<ActivityChartPointDto>();

            var usersRegistered = await _userManager.Users
                .Where(u => u.CreatedAt >= startDate)
                .GroupBy(u => u.CreatedAt.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Date, x => x.Count, cancellationToken);

            var tasksCreated = await _dbContext.TaskItems
                .Where(t => t.CreatedAt >= startDate)
                .GroupBy(t => t.CreatedAt.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Date, x => x.Count, cancellationToken);

            var tasksCompleted = await _dbContext.TaskItems
                .Where(t => t.CompletedAt.HasValue && t.CompletedAt.Value >= startDate)
                .GroupBy(t => t.CompletedAt!.Value.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Date, x => x.Count, cancellationToken);

            var activityLogs = await _dbContext.TaskActivityLogs
                .Where(l => l.CreatedAt >= startDate)
                .GroupBy(l => l.CreatedAt.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Date, x => x.Count, cancellationToken);

            for (int i = 0; i < days; i++)
            {
                var currentDate = startDate.AddDays(i);
                var dateKey = currentDate.ToString("yyyy-MM-dd");

                activityChart.Add(new ActivityChartPointDto
                {
                    Date = dateKey,
                    NewUsersCount = usersRegistered.TryGetValue(currentDate, out var uCount) ? uCount : 0,
                    CreatedTasksCount = tasksCreated.TryGetValue(currentDate, out var cCount) ? cCount : 0,
                    CompletedTasksCount = tasksCompleted.TryGetValue(currentDate, out var compCount) ? compCount : 0,
                    ActivityCount = activityLogs.TryGetValue(currentDate, out var aCount) ? aCount : 0
                });
            }

            return new AdminDashboardStatsDto
            {
                TotalUsers = totalUsers,
                ActiveUsers = activeUsers,
                DeactivatedUsers = deactivatedUsers,
                TotalCompanies = totalCompanies,
                TotalTeams = totalTeams,
                TotalTasks = totalTasks,
                CompletedTasks = completedTasks,
                InProgressTasks = inProgressTasks,
                PendingTasks = pendingTasks,
                OverdueTasks = overdueTasks,
                OverallCompletionRate = completionRate,
                ActivityChart = activityChart
            };
        }

        public async Task<CompanyDashboardStatsDto> GetCompanyDashboardStatsAsync(Guid userId, int? companyId = null, CancellationToken cancellationToken = default)
        {
            Company? company = null;

            if (companyId.HasValue)
            {
                company = await _dbContext.Companies
                    .Include(c => c.Teams.Where(t => !t.IsDeleted))
                        .ThenInclude(t => t.TeamMembers.Where(m => m.IsActive))
                    .Include(c => c.Teams.Where(t => !t.IsDeleted))
                        .ThenInclude(t => t.TaskItems)
                    .FirstOrDefaultAsync(c => c.Id == companyId.Value && !c.IsDeleted, cancellationToken);
            }
            else
            {
                company = await _dbContext.Companies
                    .Include(c => c.Teams.Where(t => !t.IsDeleted))
                        .ThenInclude(t => t.TeamMembers.Where(m => m.IsActive))
                    .Include(c => c.Teams.Where(t => !t.IsDeleted))
                        .ThenInclude(t => t.TaskItems)
                    .FirstOrDefaultAsync(c => c.OwnerId == userId && !c.IsDeleted, cancellationToken);

                if (company == null)
                {
                    var userTeamMember = await _dbContext.TeamMembers
                        .Include(m => m.Team)
                            .ThenInclude(t => t!.Company)
                        .FirstOrDefaultAsync(m => m.UserId == userId && m.IsActive, cancellationToken);

                    if (userTeamMember?.Team?.Company != null)
                    {
                        company = await _dbContext.Companies
                            .Include(c => c.Teams.Where(t => !t.IsDeleted))
                                .ThenInclude(t => t.TeamMembers.Where(m => m.IsActive))
                            .Include(c => c.Teams.Where(t => !t.IsDeleted))
                                .ThenInclude(t => t.TaskItems)
                            .FirstOrDefaultAsync(c => c.Id == userTeamMember.Team.CompanyId && !c.IsDeleted, cancellationToken);
                    }
                }
            }

            if (company == null)
                throw new Exception("Şirkət məlumatı tapılmadı.");

            var teams = company.Teams.ToList();
            var allTasks = teams.SelectMany(t => t.TaskItems).ToList();
            var allMembers = teams.SelectMany(t => t.TeamMembers).ToList();

            var now = DateTime.UtcNow;

            var totalTasks = allTasks.Count;
            var completedTasks = allTasks.Count(t => t.Status == TaskItemStatus.Done);
            var inProgressTasks = allTasks.Count(t => t.Status == TaskItemStatus.InProgress);
            var overdueTasks = allTasks.Count(t => t.Status != TaskItemStatus.Done && t.DueDate.HasValue && t.DueDate.Value < now);

            double companyCompletionRate = totalTasks > 0
                ? Math.Round((double)completedTasks / totalTasks * 100, 2)
                : 0.0;

            double companyOverdueRate = totalTasks > 0
                ? Math.Round((double)overdueTasks / totalTasks * 100, 2)
                : 0.0;

            // Teams productivity
            var teamsProductivity = new List<TeamProductivityDto>();
            foreach (var team in teams)
            {
                var tTasks = team.TaskItems.ToList();
                var tTotal = tTasks.Count;
                var tCompleted = tTasks.Count(t => t.Status == TaskItemStatus.Done);
                var tInProgress = tTasks.Count(t => t.Status == TaskItemStatus.InProgress);
                var tOverdue = tTasks.Count(t => t.Status != TaskItemStatus.Done && t.DueDate.HasValue && t.DueDate.Value < now);

                string? teamLeadName = null;
                if (team.TeamLeadId.HasValue)
                {
                    var lead = await _userManager.FindByIdAsync(team.TeamLeadId.Value.ToString());
                    teamLeadName = lead?.FullName ?? lead?.UserName;
                }

                teamsProductivity.Add(new TeamProductivityDto
                {
                    TeamId = team.Id,
                    TeamName = team.Name,
                    TeamLeadName = teamLeadName,
                    MemberCount = team.TeamMembers.Count(m => m.IsActive),
                    TotalTasks = tTotal,
                    CompletedTasks = tCompleted,
                    InProgressTasks = tInProgress,
                    OverdueTasks = tOverdue,
                    CompletionPercentage = tTotal > 0 ? Math.Round((double)tCompleted / tTotal * 100, 2) : 0.0,
                    OverduePercentage = tTotal > 0 ? Math.Round((double)tOverdue / tTotal * 100, 2) : 0.0
                });
            }

            // Top active members across all teams
            var distinctUserIds = allMembers.Select(m => m.UserId).Distinct().ToList();
            var topActiveMembers = new List<ActiveMemberDto>();

            foreach (var memberUserId in distinctUserIds)
            {
                var user = await _userManager.FindByIdAsync(memberUserId.ToString());
                if (user == null || user.IsDeleted) continue;

                var userAssignedTasks = allTasks.Where(t => t.AssignedUserId == memberUserId).ToList();
                var userCompleted = userAssignedTasks.Count(t => t.Status == TaskItemStatus.Done);
                var userTotal = userAssignedTasks.Count;

                var userActivityCount = await _dbContext.TaskActivityLogs
                    .CountAsync(l => l.UserId == memberUserId, cancellationToken);

                var avatarUrl = !string.IsNullOrWhiteSpace(user.ProfilePicture)
                    ? _storageService.GetFileUrl(user.ProfilePicture, "avatars")
                    : null;

                topActiveMembers.Add(new ActiveMemberDto
                {
                    UserId = user.Id,
                    FullName = user.FullName ?? user.UserName ?? "İstifadəçi",
                    Email = user.Email ?? string.Empty,
                    ProfilePictureUrl = avatarUrl,
                    JobTitle = user.JobTitle,
                    CompletedTasksCount = userCompleted,
                    TotalAssignedTasks = userTotal,
                    ActivityCount = userActivityCount
                });
            }

            // Sort top active members by completed tasks then activity count
            topActiveMembers = topActiveMembers
                .OrderByDescending(m => m.CompletedTasksCount)
                .ThenByDescending(m => m.ActivityCount)
                .Take(10)
                .ToList();

            return new CompanyDashboardStatsDto
            {
                CompanyId = company.Id,
                CompanyName = company.Name,
                TotalTeams = teams.Count,
                TotalMembers = distinctUserIds.Count,
                TotalTasks = totalTasks,
                CompletedTasks = completedTasks,
                InProgressTasks = inProgressTasks,
                OverdueTasks = overdueTasks,
                CompletionPercentage = companyCompletionRate,
                OverduePercentage = companyOverdueRate,
                TeamsProductivity = teamsProductivity,
                TopActiveMembers = topActiveMembers
            };
        }
    }
}
