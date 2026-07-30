using MediatR;

namespace TaskMnagementBackend.Aplication.Features.Notifications.Commands.MarkNotificationAsRead
{
    public class MarkNotificationAsReadRequest
        : IRequest<MarkNotificationAsReadResponse>
    {
        public Guid NotificationId { get; set; }

        public Guid UserId { get; set; }
    }
}