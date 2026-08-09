using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Common.Pagination;
using TaskMnagementBackend.Aplication.DTOs.Notification;

namespace TaskMnagementBackend.Aplication.Abstraction.Services
{
    public interface INotificationService
    {
        Task SendNotificationToUserAsync(string userId, object notification, CancellationToken cancellationToken = default);
        Task SendCommentToTaskGroupAsync(int taskId, object comment, CancellationToken cancellationToken = default);

        Task<bool> CreateNotificationAsync(
            CreateNotificationDto dto,
            CancellationToken cancellationToken = default);

        Task<bool> SendNotificationAsync(
            int notificationId,
            CancellationToken cancellationToken = default);

        Task<NotificationDto?> GetByIdAsync(
            int notificationId,
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            int notificationId,
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<bool> MarkAsReadAsync(
            int notificationId,
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<int> MarkAllAsReadAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteNotificationAsync(
            int notificationId,
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<PagedResult<NotificationDto>> GetUserNotificationsAsync(
            Guid userId,
            bool onlyUnread,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<NotificationDto>> GetUnreadNotificationsAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<int> GetNotificationCountAsync(
            Guid userId,
            bool onlyUnread = true,
            CancellationToken cancellationToken = default);
    }
}
