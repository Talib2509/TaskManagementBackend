using MediatR;

namespace TaskMnagementBackend.Aplication.Features.Notifications.Commands.DeleteNotification
{
    public class DeleteNotificationRequest
        : IRequest<DeleteNotificationResponse>
    {
        public Guid NotificationId { get; set; }

        public Guid UserId { get; set; }
    }
}