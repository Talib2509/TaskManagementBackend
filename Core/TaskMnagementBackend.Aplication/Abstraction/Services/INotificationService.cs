
﻿using System.Threading;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Aplication.Abstraction.Services
{
    public interface INotificationService
    {
        Task SendNotificationToUserAsync(string userId, object notification, CancellationToken cancellationToken = default);
        Task SendCommentToTaskGroupAsync(int taskId, object comment, CancellationToken cancellationToken = default);
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskMnagementBackend.Domain.Entities;
using TaskMnagementBackend.Aplication.Common.Pagination;
using TaskMnagementBackend.Aplication.DTOs.Notification;


namespace TaskMnagementBackend.Aplication.Abstraction.Services {
    public interface INotificationService
    {
        Task<bool> CreateNotificationAsync(
            CreateNotificationDto dto,
            CancellationToken cancellationToken = default);

        Task<bool> SendNotificationAsync(
            Guid notificationId,
            CancellationToken cancellationToken = default);

        Task<NotificationDto?> GetByIdAsync(
            Guid notificationId,
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            Guid notificationId,
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<bool> MarkAsReadAsync(
            Guid notificationId,
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<int> MarkAllAsReadAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteNotificationAsync(
            Guid notificationId,
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

