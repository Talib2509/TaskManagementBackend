using MediatR;
using System;

namespace TaskMnagementBackend.Aplication.Features.Commands.MarkNotificationsAsRead
{
    public class MarkNotificationsAsReadCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }
    }
}