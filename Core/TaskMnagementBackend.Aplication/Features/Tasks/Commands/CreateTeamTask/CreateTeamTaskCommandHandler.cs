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
using TaskMnagementBackend.Aplication.DTOs.Notification;
using TaskMnagementBackend.Domain.Entities;
using TaskMnagementBackend.Domain.Entities.Task;
using TaskMnagementBackend.Domain.Enums;
using TaskStatus = TaskMnagementBackend.Domain.Enums.TaskStatus;

namespace TaskMnagementBackend.Aplication.Features.Tasks.Commands.CreateTeamTask
{
    public class CreateTeamTaskCommandHandler : IRequestHandler<CreateTeamTaskCommandRequest, CreateTeamTaskCommandResponse>
    {
        private readonly IWriteRepository<ProjectTask> _taskWriteRepository;
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateTeamTaskCommandHandler(
            IWriteRepository<ProjectTask> taskWriteRepository,
            INotificationService notificationService,
            IHttpContextAccessor httpContextAccessor)
        {
            _taskWriteRepository = taskWriteRepository;
            _notificationService = notificationService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CreateTeamTaskCommandResponse> Handle(
            CreateTeamTaskCommandRequest request,
            CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid leadId))
            {
                return new CreateTeamTaskCommandResponse { Success = false, Message = "İstifadəçi tapılmadı." };
            }

            // Проверка прав: Только Team Lead или Owner
            if (userRole != "TeamLead" && userRole != "CompanyOwner")
            {
                return new CreateTeamTaskCommandResponse { Success = false, Message = "Yalnız Team Lead komanda tapşırığı yarada bilər." };
            }

            var task = new ProjectTask
            {
                Id = Guid.NewGuid(),
                Title = request.Title.Trim(),
                Description = request.Description,
                Priority = request.Priority,
                Status = TaskStatus.Pending,
                Type = TaskType.Team,
                Visibility = request.Visibility,
                TeamId = request.TeamId,
                Deadline = request.Deadline,
                CreatedAt = DateTime.UtcNow,
                UserId = leadId // Создатель (Team Lead)
            };

            // Назначаем исполнителей
            if (request.AssigneeIds != null && request.AssigneeIds.Any())
            {
                foreach (var assigneeId in request.AssigneeIds.Distinct())
                {
                    task.Assignments.Add(new TaskAssignment
                    {
                        TaskId = task.Id,
                        UserId = assigneeId,
                        AssignedAt = DateTime.UtcNow
                    });
                }
            }

            await _taskWriteRepository.AddAsync(task);
            await _taskWriteRepository.SaveAsync();

            // Отправка уведомлений назначенным исполнителям
            foreach (var assigneeId in request.AssigneeIds)
            {
                //await _notificationService.CreateNotificationAsync(new Notification
                //{
                //    UserId = assigneeId,
                //    Message = $"Sizə yeni komanda tapşırığı təyin edildi: \"{task.Title}\""
                //}, cancellationToken);


                // Уведомление старому исполнителю
                await _notificationService.CreateNotificationAsync(new CreateNotificationDto
                {
                    UserId = assigneeId,
                    Title = "Tapşırıq sizə təyin edildi",
                    Text = $"Sizə yeni komanda tapşırığı təyin edildi: \"{task.Title}\"",
                    Type = NotificationType.TaskAssigned }, cancellationToken); 
            }


            return new CreateTeamTaskCommandResponse
            {
                Success = true,
                Message = "Komanda tapşırığı uğurla yaradıldı.",
                TaskId = task.Id
            };
        }
    }
}
