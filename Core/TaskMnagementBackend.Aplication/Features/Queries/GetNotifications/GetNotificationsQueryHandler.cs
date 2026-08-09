using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;
using TaskMnagementBackend.Aplication.DTOs;
using TaskMnagementBackend.Domain.Entities;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Aplication.Features.Queries.GetNotifications
{
    public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, List<NotificationDto>>
    {
        private readonly IReadRepository<Notification> _notificationReadRepository;

        public GetNotificationsQueryHandler(IReadRepository<Notification> notificationReadRepository)
        {
            _notificationReadRepository = notificationReadRepository;
        }

        public async Task<List<NotificationDto>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            var query = _notificationReadRepository.GetWhere(n => n.UserId == request.UserId);

            if (!string.IsNullOrEmpty(request.Type) && Enum.TryParse<NotificationType>(request.Type, true, out var notifType))
            {
                query = query.Where(n => n.Type == notifType);
            }

            var notifications = await query
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Message = n.Text,
                    Type = n.Type.ToString(),
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return notifications;
        }
    }
}