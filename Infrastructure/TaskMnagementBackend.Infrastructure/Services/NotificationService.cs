using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common.Pagination;
using TaskMnagementBackend.Aplication.DTOs.Notification;
using TaskMnagementBackend.Aplication.IUnitOfWork;
using TaskMnagementBackend.Domain.Entities;
using TaskMnagementBackend.Infrastructure.Hubs;

namespace TaskMnagementBackend.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(
            IUnitOfWork unitOfWork,
            IHubContext<NotificationHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        public async Task SendNotificationToUserAsync(string userId, object notification, CancellationToken cancellationToken = default)
        {
            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", notification, cancellationToken);
        }

        public async Task SendCommentToTaskGroupAsync(int taskId, object comment, CancellationToken cancellationToken = default)
        {
            await _hubContext.Clients.Group($"Task_{taskId}").SendAsync("ReceiveNewComment", comment, cancellationToken);
        }

        public async Task<bool> CreateNotificationAsync(
            CreateNotificationDto dto,
            CancellationToken cancellationToken = default)
        {
            var notification = new Notification
            {
                Title = dto.Title,
                Text = dto.Text,
                Type = dto.Type,
                UserId = dto.UserId,
                RelatedEntityId = dto.RelatedEntityId,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.NotificationWriteRepository.AddAsync(notification);
            return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<NotificationDto?> GetByIdAsync(
            int notificationId,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.NotificationReadRepository
                .GetWhere(x => x.Id == notificationId && x.UserId == userId)
                .Select(x => new NotificationDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Text = x.Text,
                    Type = x.Type,
                    UserId = x.UserId,
                    RelatedEntityId = x.RelatedEntityId,
                    IsRead = x.IsRead,
                    CreatedAt = x.CreatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> SendNotificationAsync(
            int notificationId,
            CancellationToken cancellationToken = default)
        {
            var notification = await _unitOfWork.NotificationReadRepository
                .GetWhere(x => x.Id == notificationId)
                .Select(x => new NotificationDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Text = x.Text,
                    Type = x.Type,
                    UserId = x.UserId,
                    RelatedEntityId = x.RelatedEntityId,
                    IsRead = x.IsRead,
                    CreatedAt = x.CreatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (notification == null)
                return false;

            await _hubContext.Clients
                .User(notification.UserId.ToString())
                .SendAsync("ReceiveNotification", notification, cancellationToken);

            return true;
        }

        public async Task<bool> MarkAsReadAsync(
            int notificationId,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var notification = await _unitOfWork.NotificationReadRepository
                .GetSingleAsync(x => x.Id == notificationId && x.UserId == userId);

            if (notification == null)
                return false;

            if (notification.IsRead)
                return true;

            notification.IsRead = true;
            _unitOfWork.NotificationWriteRepository.Update(notification);

            return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<int> MarkAllAsReadAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var notifications = await _unitOfWork.NotificationReadRepository
                .GetWhere(x => x.UserId == userId && !x.IsRead)
                .ToListAsync(cancellationToken);

            if (!notifications.Any())
                return 0;

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                _unitOfWork.NotificationWriteRepository.Update(notification);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return notifications.Count;
        }

        public async Task<bool> DeleteNotificationAsync(
            int notificationId,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var notification = await _unitOfWork.NotificationReadRepository
                .GetSingleAsync(x => x.Id == notificationId && x.UserId == userId);

            if (notification == null)
                return false;

            _unitOfWork.NotificationWriteRepository.Delete(notification);
            return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<PagedResult<NotificationDto>> GetUserNotificationsAsync(
            Guid userId,
            bool onlyUnread,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var query = _unitOfWork.NotificationReadRepository
                .GetWhere(x => x.UserId == userId);

            if (onlyUnread)
            {
                query = query.Where(x => !x.IsRead);
            }

            query = query
                .OrderBy(x => x.IsRead)
                .ThenByDescending(x => x.CreatedAt);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new NotificationDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Text = x.Text,
                    Type = x.Type,
                    IsRead = x.IsRead,
                    UserId = x.UserId,
                    RelatedEntityId = x.RelatedEntityId,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return PagedResult<NotificationDto>.Create(items, totalCount, page, pageSize);
        }

        public async Task<IReadOnlyList<NotificationDto>> GetUnreadNotificationsAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.NotificationReadRepository
                .GetWhere(x => x.UserId == userId && !x.IsRead)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new NotificationDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Text = x.Text,
                    Type = x.Type,
                    IsRead = x.IsRead,
                    UserId = x.UserId,
                    RelatedEntityId = x.RelatedEntityId,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetNotificationCountAsync(
            Guid userId,
            bool onlyUnread = true,
            CancellationToken cancellationToken = default)
        {
            var query = _unitOfWork.NotificationReadRepository
                .GetWhere(x => x.UserId == userId);

            if (onlyUnread)
            {
                query = query.Where(x => !x.IsRead);
            }

            return await query.CountAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(
            int notificationId,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.NotificationReadRepository
                .GetWhere(x => x.Id == notificationId && x.UserId == userId)
                .AnyAsync(cancellationToken);
        }
    }
}