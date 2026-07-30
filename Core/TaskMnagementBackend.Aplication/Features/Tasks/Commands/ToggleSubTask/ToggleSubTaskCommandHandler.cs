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

namespace TaskMnagementBackend.Aplication.Features.Tasks.Commands.ToggleSubTask
{
    public class ToggleSubTaskCommandHandler : IRequestHandler<ToggleSubTaskCommandRequest, ToggleSubTaskCommandResponse>
    {
        private readonly IReadRepository<SubTask> _subTaskReadRepository;
        private readonly IWriteRepository<SubTask> _subTaskWriteRepository;
        private readonly IReadRepository<ProjectTask> _taskReadRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ToggleSubTaskCommandHandler(
            IReadRepository<SubTask> subTaskReadRepository,
            IWriteRepository<SubTask> subTaskWriteRepository,
            IReadRepository<ProjectTask> taskReadRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _subTaskReadRepository = subTaskReadRepository;
            _subTaskWriteRepository = subTaskWriteRepository;
            _taskReadRepository = taskReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ToggleSubTaskCommandResponse> Handle(
            ToggleSubTaskCommandRequest request,
            CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return new ToggleSubTaskCommandResponse { Success = false, Message = "İstifadəçi tapılmadı." };
            }

            var subTask = await _subTaskReadRepository.GetSingleAsync(x => x.Id == request.SubTaskId);

            if (subTask is null)
            {
                return new ToggleSubTaskCommandResponse { Success = false, Message = "Alt-tapşırıq tapılmadı." };
            }

            // Проверяем безопасность: родительская задача принадлежит текущему пользователю?
            var task = await _taskReadRepository.GetSingleAsync(x => x.Id == subTask.TaskId && x.UserId == userId);

            if (task is null)
            {
                return new ToggleSubTaskCommandResponse { Success = false, Message = "İcazəniz yoxdur." };
            }

            // Инвертируем статус (true -> false / false -> true)
            subTask.IsCompleted = !subTask.IsCompleted;

            _subTaskWriteRepository.Update(subTask);
            await _subTaskWriteRepository.SaveAsync();

            return new ToggleSubTaskCommandResponse
            {
                Success = true,
                Message = subTask.IsCompleted ? "Alt-tapşırıq icra olundu." : "Alt-tapşırıq gözləməyə qaytarıldı.",
                IsCompleted = subTask.IsCompleted
            };
        }
    }
}
