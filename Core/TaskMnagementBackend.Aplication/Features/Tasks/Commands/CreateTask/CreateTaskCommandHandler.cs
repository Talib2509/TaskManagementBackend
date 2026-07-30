using MediatR;
using Microsoft.AspNetCore.Http;
using SmartTask.Domain.Entities;
using System.Security.Claims;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;
using TaskMnagementBackend.Domain.Enums;
using TaskStatus = TaskMnagementBackend.Domain.Enums.TaskStatus;

namespace TaskMnagementBackend.Aplication.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommandRequest, CreateTaskCommandResponse>
    {
        private readonly IWriteRepository<ProjectTask> _taskWriteRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateTaskCommandHandler(
            IWriteRepository<ProjectTask> taskWriteRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _taskWriteRepository = taskWriteRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CreateTaskCommandResponse> Handle(
            CreateTaskCommandRequest request,
            CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? _httpContextAccessor.HttpContext?.User.FindFirst("UserId")?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return new CreateTaskCommandResponse { Success = false, Message = "İstifadəçi tapılmadı." };
            }

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return new CreateTaskCommandResponse { Success = false, Message = "Başlıq boş ola bilməz." };
            }

            var task = new ProjectTask
            {
                Id = Guid.NewGuid(),
                Title = request.Title.Trim(),
                Description = request.Description,
                Priority = request.Priority,
                Status = TaskStatus.Pending,
                Type = TaskType.Personal, // Fərdi tapşırıq
                Deadline = request.Deadline,
                CreatedAt = DateTime.UtcNow,
                Visibility = request.Visibility,
                UserId = userId
            };

            await _taskWriteRepository.AddAsync(task);
            await _taskWriteRepository.SaveAsync();

            return new CreateTaskCommandResponse
            {
                Success = true,
                Message = "Fərdi tapşırıq uğurla yaradıldı.",
                TaskId = task.Id
            };
        }
    }
}
