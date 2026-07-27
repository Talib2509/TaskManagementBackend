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

namespace TaskMnagementBackend.Aplication.Features.Tasks.Queries.GetTasksList
{
    public class GetTasksListQueryHandler : IRequestHandler<GetTasksListQueryRequest, GetTasksListQueryResponse>
    {
        private readonly IReadRepository<ProjectTask> _taskReadRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetTasksListQueryHandler(
            IReadRepository<ProjectTask> taskReadRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _taskReadRepository = taskReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetTasksListQueryResponse> Handle(
            GetTasksListQueryRequest request,
            CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return new GetTasksListQueryResponse { Success = false, Message = "İstifadəçi tapılmadı." };
            }

            // Фильтруем только личные задачи текущего пользователя
            var query = _taskReadRepository.GetWhere(x => x.UserId == userId && x.Type == TaskType.Personal);

            if (request.Status.HasValue)
                query = query.Where(x => x.Status == request.Status.Value);

            if (request.Priority.HasValue)
                query = query.Where(x => x.Priority == request.Priority.Value);

            if (request.DateFrom.HasValue)
                query = query.Where(x => x.CreatedAt >= request.DateFrom.Value);

            if (request.DateTo.HasValue)
                query = query.Where(x => x.CreatedAt <= request.DateTo.Value);

            var tasks = await query
                .Select(x => new TaskDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Priority = x.Priority,
                    Status = x.Status,
                    Deadline = x.Deadline,
                    CreatedAt = x.CreatedAt,
                    Type = x.Type,
                    Visibility = x.Visibility
                })
                .ToListAsync(cancellationToken);

            return new GetTasksListQueryResponse
            {
                Success = true,
                Tasks = tasks
            };
        }
    }
}
