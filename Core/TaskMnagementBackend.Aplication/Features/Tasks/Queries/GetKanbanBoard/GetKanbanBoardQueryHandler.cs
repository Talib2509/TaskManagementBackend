using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartTask.Domain.Entities;
using System.Security.Claims;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;
using TaskMnagementBackend.Aplication.DTOs;
using TaskMnagementBackend.Domain.Enums;
using TaskStatus = TaskMnagementBackend.Domain.Enums.TaskStatus;

namespace TaskMnagementBackend.Aplication.Features.Tasks.Queries.GetKanbanBoard
{
    public class GetKanbanBoardQueryHandler : IRequestHandler<GetKanbanBoardQueryRequest, GetKanbanBoardQueryResponse>
    {
        private readonly IReadRepository<ProjectTask> _taskReadRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetKanbanBoardQueryHandler(
            IReadRepository<ProjectTask> taskReadRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _taskReadRepository = taskReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetKanbanBoardQueryResponse> Handle(
            GetKanbanBoardQueryRequest request,
            CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return new GetKanbanBoardQueryResponse { Success = false, Message = "İstifadəçi tapılmadı." };
            }

            // Получаем все личные задачи пользователя
            var tasks = await _taskReadRepository
                .GetWhere(x => x.UserId == userId && x.Type == TaskType.Personal)
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

            // Группируем по колонкам Kanban
            var board = new KanbanBoardDto
            {
                Pending = tasks.Where(t => t.Status == TaskStatus.Pending).ToList(),
                InProgress = tasks.Where(t => t.Status == TaskStatus.InProgress).ToList(),
                Completed = tasks.Where(t => t.Status == TaskStatus.Completed).ToList(),
                Blocked = tasks.Where(t => t.Status == TaskStatus.Blocked).ToList()
            };

            return new GetKanbanBoardQueryResponse
            {
                Success = true,
                Board = board
            };
        }
    }
}
