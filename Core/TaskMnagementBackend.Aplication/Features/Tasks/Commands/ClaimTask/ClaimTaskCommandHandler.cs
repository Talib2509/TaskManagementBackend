using MediatR;
using Microsoft.AspNetCore.Http;
using SmartTask.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;
using TaskMnagementBackend.Domain.Entities.Task;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Aplication.Features.Tasks.Commands.ClaimTask
{
    public class ClaimTaskCommandHandler : IRequestHandler<ClaimTaskCommandRequest, ClaimTaskCommandResponse>
    {
        private readonly IReadRepository<ProjectTask> _taskReadRepository;
        private readonly IWriteRepository<TaskAssignment> _assignmentWriteRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimTaskCommandHandler(
            IReadRepository<ProjectTask> taskReadRepository,
            IWriteRepository<TaskAssignment> assignmentWriteRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _taskReadRepository = taskReadRepository;
            _assignmentWriteRepository = assignmentWriteRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ClaimTaskCommandResponse> Handle(
            ClaimTaskCommandRequest request,
            CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return new ClaimTaskCommandResponse { Success = false, Message = "İstifadəçi tapılmadı." };
            }

            var task = await _taskReadRepository.GetSingleAsync(x => x.Id == request.TaskId && x.Type == TaskType.Team);

            if (task is null)
            {
                return new ClaimTaskCommandResponse { Success = false, Message = "Komanda tapşırığı tapılmadı." };
            }

            if (task.Visibility == TaskVisibility.Private)
            {
                return new ClaimTaskCommandResponse { Success = false, Message = "Bu gizli tapşırıqdır, özünüzə götürə bilməzsiniz." };
            }

            // Добавляем текущего пользователя в исполнители
            var assignment = new TaskAssignment
            {
                Id = Guid.NewGuid(),
                TaskId = task.Id,
                UserId = userId,
                AssignedAt = DateTime.UtcNow
            };

            await _assignmentWriteRepository.AddAsync(assignment);
            await _assignmentWriteRepository.SaveAsync();

            return new ClaimTaskCommandResponse
            {
                Success = true,
                Message = "Tapşırıq öhdəliyinizə götürüldü."
            };
        }
    }
}
