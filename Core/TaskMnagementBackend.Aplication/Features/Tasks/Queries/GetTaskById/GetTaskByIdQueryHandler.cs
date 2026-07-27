using MediatR;
using Microsoft.AspNetCore.Http;
using SmartTask.Domain.Entities;
using System.Security.Claims;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;
using TaskMnagementBackend.Aplication.DTOs;

namespace TaskMnagementBackend.Aplication.Features.Tasks.Queries.GetTaskById
{
    public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQueryRequest, GetTaskByIdQueryResponse>
    {
        private readonly IReadRepository<ProjectTask> _taskReadRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetTaskByIdQueryHandler(
            IReadRepository<ProjectTask> taskReadRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _taskReadRepository = taskReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetTaskByIdQueryResponse> Handle(
            GetTaskByIdQueryRequest request,
            CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return new GetTaskByIdQueryResponse { Success = false, Message = "İstifadəçi tapılmadı." };
            }

            var task = await _taskReadRepository.GetSingleAsync(x => x.Id == request.Id && x.UserId == userId);

            if (task is null)
            {
                return new GetTaskByIdQueryResponse { Success = false, Message = "Tapşırıq tapılmadı." };
            }

            return new GetTaskByIdQueryResponse
            {
                Success = true,
                Task = new TaskDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Priority = task.Priority,
                    Status = task.Status,
                    Deadline = task.Deadline,
                    CreatedAt = task.CreatedAt,
                    Type = task.Type,
                    Visibility = task.Visibility
                }
            };
        }
    }
}
