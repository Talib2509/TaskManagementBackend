using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartTask.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;
using TaskMnagementBackend.Aplication.DTOs;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Aplication.Features.Tasks.Queries.GetMyTeamTasks
{
    public class GetMyTeamTasksQueryHandler : IRequestHandler<GetMyTeamTasksQueryRequest, GetMyTeamTasksQueryResponse>
    {
        private readonly IReadRepository<ProjectTask> _taskReadRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyTeamTasksQueryHandler(
            IReadRepository<ProjectTask> taskReadRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _taskReadRepository = taskReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetMyTeamTasksQueryResponse> Handle(
            GetMyTeamTasksQueryRequest request,
            CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return new GetMyTeamTasksQueryResponse { Success = false, Message = "İstifadəçi tapılmadı." };
            }

            // Фильтруем командные задачи:
            // 1. Посетитель — исполнитель (Assignments.Any(a => a.UserId == userId))
            // 2. ИЛИ задача Публичная (Visibility == Public)
            // 3. ИЛИ пользователь сам является ее создателем (Team Lead)
            var tasks = await _taskReadRepository
                .GetWhere(x => x.Type == TaskType.Team &&
                              (x.Assignments.Any(a => a.UserId == userId) ||
                               x.Visibility == TaskVisibility.Public ||
                               x.UserId == userId))
                .OrderByDescending(x => x.Priority) // Сначала высокие приоритеты
                .ThenBy(x => x.Deadline)            // Затем горящие дедлайны
                .Select(x => new TaskDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Priority = x.Priority,
                    Status = x.Status,
                    Deadline = x.Deadline,
                    CreatedAt = x.CreatedAt,
                    Type = x.Type
                })
                .ToListAsync(cancellationToken);

            return new GetMyTeamTasksQueryResponse
            {
                Success = true,
                Tasks = tasks
            };
        }
    }
}
