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
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Domain.Entities;
using TaskMnagementBackend.Domain.Entities.Task;

namespace TaskMnagementBackend.Aplication.Features.Tasks.Commands.ReassignTask
{
    public class ReassignTaskCommandHandler : IRequestHandler<ReassignTaskCommandRequest, ReassignTaskCommandResponse>
    {
        private readonly IReadRepository<TaskAssignment> _assignmentReadRepository;
        private readonly IWriteRepository<TaskAssignment> _assignmentWriteRepository;
        private readonly IReadRepository<ProjectTask> _taskReadRepository;
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReassignTaskCommandHandler(
            IReadRepository<TaskAssignment> assignmentReadRepository,
            IWriteRepository<TaskAssignment> assignmentWriteRepository,
            IReadRepository<ProjectTask> taskReadRepository,
            INotificationService notificationService,
            IHttpContextAccessor httpContextAccessor)
        {
            _assignmentReadRepository = assignmentReadRepository;
            _assignmentWriteRepository = assignmentWriteRepository;
            _taskReadRepository = taskReadRepository;
            _notificationService = notificationService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ReassignTaskCommandResponse> Handle(
            ReassignTaskCommandRequest request,
            CancellationToken cancellationToken)
        {
            var userRole = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole != "TeamLead" && userRole != "CompanyOwner")
            {
                return new ReassignTaskCommandResponse { Success = false, Message = "Yalnız Team Lead yenidən təyin edə bilər." };
            }

            var task = await _taskReadRepository.GetSingleAsync(x => x.Id == request.TaskId);
            if (task is null)
                return new ReassignTaskCommandResponse { Success = false, Message = "Tapşırıq tapılmadı." };

            // Находим старое назначение
            var oldAssignment = await _assignmentReadRepository.GetSingleAsync(x => x.TaskId == request.TaskId && x.UserId == request.OldUserId);
            if (oldAssignment != null)
            {
                _assignmentWriteRepository.Delete(oldAssignment);
            }

            // Создаем новое назначение
            var newAssignment = new TaskAssignment
            {
                Id = Guid.NewGuid(),
                TaskId = request.TaskId,
                UserId = request.NewUserId,
                AssignedAt = DateTime.UtcNow
            };

            await _assignmentWriteRepository.AddAsync(newAssignment);
            await _assignmentWriteRepository.SaveAsync();

            // Уведомление старому исполнителю
            await _notificationService.CreateAsync(new Notification
            {
                UserId = request.OldUserId,
                Message = $"\"{task.Title}\" tapşırığı üzərinizdən götürüldü."
            }, cancellationToken);

            // Уведомление новому исполнителю
            await _notificationService.CreateAsync(new Notification
            {
                UserId = request.NewUserId,
                Message = $"Sizə yeni tapşırıq təyin olundu: \"{task.Title}\"."
            }, cancellationToken);

            return new ReassignTaskCommandResponse
            {
                Success = true,
                Message = "Tapşırıq uğurla yenidən təyin edildi."
            };
        }
    }
}
