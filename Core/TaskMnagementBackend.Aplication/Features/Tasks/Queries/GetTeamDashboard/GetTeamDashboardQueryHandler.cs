using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartTask.Domain.Entities;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;
using TaskMnagementBackend.Aplication.DTOs;
using TaskMnagementBackend.Domain.Entities.Task;
using TaskStatus = TaskMnagementBackend.Domain.Enums.TaskStatus;

namespace TaskMnagementBackend.Aplication.Features.Tasks.Queries.GetTeamDashboard
{
    public class GetTeamDashboardQueryHandler : IRequestHandler<GetTeamDashboardQueryRequest, GetTeamDashboardQueryResponse>
    {
        private readonly IReadRepository<ProjectTask> _taskReadRepository;
        private readonly IReadRepository<TaskAssignment> _assignmentReadRepository;

        public GetTeamDashboardQueryHandler(
            IReadRepository<ProjectTask> taskReadRepository,
            IReadRepository<TaskAssignment> assignmentReadRepository)
        {
            _taskReadRepository = taskReadRepository;
            _assignmentReadRepository = assignmentReadRepository;
        }

        public async Task<GetTeamDashboardQueryResponse> Handle(
            GetTeamDashboardQueryRequest request,
            CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            var tasks = await _taskReadRepository
                .GetWhere(x => x.TeamId == request.TeamId)
                .ToListAsync(cancellationToken);

            // Расчет статистики
            var dashboard = new TeamDashboardDto
            {
                PendingCount = tasks.Count(x => x.Status == TaskStatus.Pending),
                InProgressCount = tasks.Count(x => x.Status == TaskStatus.InProgress),
                CompletedCount = tasks.Count(x => x.Status == TaskStatus.Completed),
                OverdueCount = tasks.Count(x => x.Deadline.HasValue && x.Deadline.Value < now && x.Status != TaskStatus.Completed)
            };

            // Анализ нагрузки (Workload per member)
            var assignments = await _assignmentReadRepository
                .GetWhere(x => x.Task.TeamId == request.TeamId)
                .GroupBy(x => x.UserId)
                .Select(g => new MemberWorkloadDto
                {
                    UserId = g.Key,
                    TaskCount = g.Count()
                })
                .ToListAsync(cancellationToken);

            dashboard.Workload = assignments;

            return new GetTeamDashboardQueryResponse
            {
                Success = true,
                Dashboard = dashboard
            };
        }
    }
}
