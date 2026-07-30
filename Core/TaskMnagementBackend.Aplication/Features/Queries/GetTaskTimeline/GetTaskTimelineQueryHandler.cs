using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;
using TaskMnagementBackend.Aplication.DTOs;
using TaskMnagementBackend.Domain.Entities;

namespace TaskMnagementBackend.Aplication.Features.Queries.GetTaskTimeline
{
    public class GetTaskTimelineQueryHandler : IRequestHandler<GetTaskTimelineQuery, List<TaskTimelineDto>>
    {
        private readonly IReadRepository<TaskActivityLog> _activityLogReadRepository;

        public GetTaskTimelineQueryHandler(IReadRepository<TaskActivityLog> activityLogReadRepository)
        {
            _activityLogReadRepository = activityLogReadRepository;
        }

        public async Task<List<TaskTimelineDto>> Handle(GetTaskTimelineQuery request, CancellationToken cancellationToken)
        {
            var logs = await _activityLogReadRepository
                .GetWhere(l => l.ProjectTaskId == request.TaskId)
                .OrderByDescending(l => l.CreatedAt) 
                .Select(l => new TaskTimelineDto
                {
                    Id = l.Id,
                    ProjectTaskId = l.ProjectTaskId,
                    UserId = l.UserId,
                    UserName = l.User != null ? l.User.UserName : null,
                    ActionType = l.ActionType,
                    Description = l.Description,
                    CreatedAt = l.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return logs;
        }
    }
}