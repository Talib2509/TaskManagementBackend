using System;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Aplication.DTOs.Notification
{
    public class NotificationDto
    {
        public Guid Id { get; set; } // <--- Измени с int на Guid
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public NotificationType Type { get; set; }
        public Guid UserId { get; set; }
        public Guid? RelatedEntityId { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
