using MediatR;
using Microsoft.AspNetCore.Http;
using SmartTask.Domain.Entities;
using System.Security.Claims;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Aplication.Features.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommandRequest, UpdateTaskCommandResponse>
    {
        private readonly IReadRepository<ProjectTask> _taskReadRepository;
        private readonly IWriteRepository<ProjectTask> _taskWriteRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateTaskCommandHandler(
            IReadRepository<ProjectTask> taskReadRepository,
            IWriteRepository<ProjectTask> taskWriteRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _taskReadRepository = taskReadRepository;
            _taskWriteRepository = taskWriteRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UpdateTaskCommandResponse> Handle(
            UpdateTaskCommandRequest request,
            CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return new UpdateTaskCommandResponse { Success = false, Message = "İstifadəçi tapılmadı." };
            }

            var task = await _taskReadRepository.GetSingleAsync(x => x.Id == request.Id && x.UserId == userId);

            if (task is null)
            {
                return new UpdateTaskCommandResponse { Success = false, Message = "Tapşırıq tapılmadı və ya icazəniz yoxdur." };
            }

            task.Title = request.Title.Trim();
            task.Description = request.Description;
            task.Priority = request.Priority;
            task.Deadline = request.Deadline;
            task.Visibility = request.Visibility;


        _taskWriteRepository.Update(task);
            await _taskWriteRepository.SaveAsync();

            return new UpdateTaskCommandResponse
            {
                Success = true,
                Message = "Tapşırıq uğurla yeniləndi."
            };
        }
    }
}
