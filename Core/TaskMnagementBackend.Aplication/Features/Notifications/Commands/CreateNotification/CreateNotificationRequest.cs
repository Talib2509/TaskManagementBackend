using MediatR;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Aplication.Features.Notifications.Commands.CreateNotification
{
    public class CreateNotificationRequest : IRequest<CreateNotificationResponse>
    {
        public string Title { get; set; } = string.Empty;

        public string Text { get; set; } = string.Empty;

        public NotificationType Type { get; set; }

        public Guid UserId { get; set; }

        public int? RelatedEntityId { get; set; }
    }
}