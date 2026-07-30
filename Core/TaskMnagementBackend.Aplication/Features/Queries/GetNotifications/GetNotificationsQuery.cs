using MediatR;
using System;
using System.Collections.Generic;
using TaskMnagementBackend.Aplication.DTOs;

namespace TaskMnagementBackend.Aplication.Features.Queries.GetNotifications
{
    public class GetNotificationsQuery : IRequest<List<NotificationDto>>
    {
        public Guid UserId { get; set; }
        public string? Type { get; set; } 
    }
}