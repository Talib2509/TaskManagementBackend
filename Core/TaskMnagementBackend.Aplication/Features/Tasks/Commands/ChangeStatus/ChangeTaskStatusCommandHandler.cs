using MediatR;
using Microsoft.AspNetCore.Http;
using SmartTask.Domain.Entities;
using System.Security.Claims;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Domain.Entities;
using TaskStatus = TaskMnagementBackend.Domain.Enums.TaskStatus;


namespace TaskMnagementBackend.Aplication.Features.Tasks.Commands.ChangeStatus
{
    public class ChangeTaskStatusCommandHandler : IRequestHandler<ChangeTaskStatusCommandRequest, ChangeTaskStatusCommandResponse>
    {
        private readonly IReadRepository<ProjectTask> _taskReadRepository;
        private readonly IWriteRepository<ProjectTask> _taskWriteRepository;
        private readonly IWriteRepository<TaskStatusHistory> _historyWriteRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotificationService _notificationService;


        public ChangeTaskStatusCommandHandler(
            IReadRepository<ProjectTask> taskReadRepository,
            IWriteRepository<ProjectTask> taskWriteRepository,
            IWriteRepository<TaskStatusHistory> historyWriteRepository,
            IHttpContextAccessor httpContextAccessor,
            INotificationService notificationService)
        {
            _taskReadRepository = taskReadRepository;
            _taskWriteRepository = taskWriteRepository;
            _historyWriteRepository = historyWriteRepository;
            _httpContextAccessor = httpContextAccessor;
            _notificationService = notificationService;
        }

        public async Task<ChangeTaskStatusCommandResponse> Handle(
            ChangeTaskStatusCommandRequest request,
            CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? _httpContextAccessor.HttpContext?.User.FindFirst("UserId")?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return new ChangeTaskStatusCommandResponse { Success = false, Message = "İstifadəçi tapılmadı." };
            }

            // Проверяем существование задачи и права владельца
            var task = await _taskReadRepository.GetSingleAsync(x => x.Id == request.TaskId && x.UserId == userId);

            if (task is null)
            {
                return new ChangeTaskStatusCommandResponse { Success = false, Message = "Tapşırıq tapılmadı və ya icazəniz yoxdur." };
            }

            // Если статус не изменился — ничего не делаем
            if (task.Status == request.NewStatus)
            {
                return new ChangeTaskStatusCommandResponse { Success = false, Message = "Tapşırıq artıq bu statusdadır." };
            }

            var oldStatus = task.Status;

            // 1. Обновляем статус задачи
           
            
            // Если новый статус Blocked — уведомляем Team Lead
            if (request.NewStatus == TaskStatus.Blocked)
            {
                await _notificationService.CreateAsync(new Notification
                {
                    UserId = task.UserId, // Team Lead ID (создатель)
                    Message = $"TƏCİLİ: \"{task.Title}\" tapşırığı BLOKLANDI!"
                }, cancellationToken);
            }

            task.Status = request.NewStatus;
            _taskWriteRepository.Update(task);

            // 2. Пишем логи для Audit Trail (TaskStatusHistory)
            var statusHistory = new TaskStatusHistory
            {
                Id = Guid.NewGuid(),
                TaskId = task.Id,
                UserId = userId,
                OldStatus = oldStatus,
                NewStatus = request.NewStatus,
                ChangedAt = DateTime.UtcNow
            };

            await _historyWriteRepository.AddAsync(statusHistory);

            // Сохраняем изменения
            await _taskWriteRepository.SaveAsync();

            return new ChangeTaskStatusCommandResponse
            {
                Success = true,
                Message = $"Status uğurla dəyişdirildi: {oldStatus} ➔ {request.NewStatus}"
            };
        }
    }
}
