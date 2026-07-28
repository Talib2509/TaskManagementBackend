using MediatR;

namespace TaskMnagementBackend.Aplication.Features.Notifications.Commands.SendNotification
{
    public class SendNotificationRequest : IRequest<SendNotificationResponse>
    {
        public Guid NotificationId { get; set; }
    }
}