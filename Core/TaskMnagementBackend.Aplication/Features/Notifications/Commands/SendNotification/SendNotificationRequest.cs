using MediatR;

namespace TaskMnagementBackend.Aplication.Features.Notifications.Commands.SendNotification
{
    public class SendNotificationRequest : IRequest<SendNotificationResponse>
    {
        public int NotificationId { get; set; }
    }
}