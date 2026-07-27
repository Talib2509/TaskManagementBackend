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

namespace TaskMnagementBackend.Aplication.Features.Tasks.Queries.SearchTasks
{
    public class SearchTasksQueryHandler : IRequestHandler<SearchTasksQueryRequest, SearchTasksQueryResponse>
    {
        private readonly IReadRepository<ProjectTask> _taskReadRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SearchTasksQueryHandler(
            IReadRepository<ProjectTask> taskReadRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _taskReadRepository = taskReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<SearchTasksQueryResponse> Handle(
            SearchTasksQueryRequest request,
            CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return new SearchTasksQueryResponse { Success = false, Message = "İstifadəçi tapılmadı." };
            }

            if (string.IsNullOrWhiteSpace(request.Query))
            {
                return new SearchTasksQueryResponse { Success = true, Tasks = new() };
            }

            var searchTerm = request.Query.Trim().ToLower();

            // Поиск по названию или описанию задачи для текущего пользователя
            var tasks = await _taskReadRepository
                .GetWhere(x => x.UserId == userId
                            && x.Type == TaskType.Personal
                            && (x.Title.ToLower().Contains(searchTerm) || (x.Description != null && x.Description.ToLower().Contains(searchTerm))))
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

            return new SearchTasksQueryResponse
            {
                Success = true,
                Tasks = tasks
            };
        }
    }
}
