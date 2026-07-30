using TaskMnagementBackend.Domain.Enums;



namespace TaskMnagementBackend.Aplication.DTOs.Notification
{
    public class CreateNotificationDto
    {
        public string Title { get; set; } = string.Empty;

        public string Text { get; set; } = string.Empty;

        public NotificationType Type { get; set; }

        public Guid UserId { get; set; }

        public Guid? RelatedEntityId { get; set; }
    }
}