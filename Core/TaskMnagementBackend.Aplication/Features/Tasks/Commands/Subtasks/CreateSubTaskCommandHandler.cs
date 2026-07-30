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

namespace TaskMnagementBackend.Aplication.Features.Tasks.Commands.Subtasks
{
    public class CreateSubTaskCommandHandler : IRequestHandler<CreateSubTaskCommandRequest, CreateSubTaskCommandResponse>
    {
        private readonly IReadRepository<ProjectTask> _taskReadRepository;
        private readonly IWriteRepository<SubTask> _subTaskWriteRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateSubTaskCommandHandler(
            IReadRepository<ProjectTask> taskReadRepository,
            IWriteRepository<SubTask> subTaskWriteRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _taskReadRepository = taskReadRepository;
            _subTaskWriteRepository = subTaskWriteRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CreateSubTaskCommandResponse> Handle(
            CreateSubTaskCommandRequest request,
            CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return new CreateSubTaskCommandResponse { Success = false, Message = "İstifadəçi tapılmadı." };
            }

            // Проверяем, существует ли родительская задача и принадлежит ли пользователю
            var task = await _taskReadRepository.GetSingleAsync(x => x.Id == request.TaskId && x.UserId == userId);

            if (task is null)
            {
                return new CreateSubTaskCommandResponse { Success = false, Message = "Alaşdırılan tapşırıq tapılmadı." };
            }

            if (string.IsNullOrWhiteSpace(request.Text))
            {
                return new CreateSubTaskCommandResponse { Success = false, Message = "Alt-tapşırıq mətni boş ola bilməz." };
            }

            var subTask = new SubTask
            {
                Id = Guid.NewGuid(),
                TaskId = request.TaskId,
                Text = request.Text.Trim(),
                IsCompleted = false
            };

            await _subTaskWriteRepository.AddAsync(subTask);
            await _subTaskWriteRepository.SaveAsync();

            return new CreateSubTaskCommandResponse
            {
                Success = true,
                Message = "Alt-tapşırıq uğurla əlavə edildi.",
                SubTaskId = subTask.Id
            };
        }
    }
}
