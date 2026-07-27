using System;
using TaskMnagementBackend.Domain.Common;
using TaskMnagementBackend.Domain.Entities.Identity;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public string Title { get; set; } = string.Empty;

        public string Text { get; set; } = string.Empty;

        public NotificationType Type { get; set; }

        public bool IsRead { get; set; } = false;

        public Guid UserId { get; set; }

        public AppUser? User { get; set; }

        public int? RelatedEntityId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
