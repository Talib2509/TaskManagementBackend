using System;
using TaskMnagementBackend.Domain.Common;
using TaskMnagementBackend.Domain.Entities.Identity;

namespace TaskMnagementBackend.Domain.Entities
{
    public class UserSettings : BaseEntity
    {
        public Guid UserId { get; set; }
        public AppUser? User { get; set; }

        public bool EmailNotificationEnabled { get; set; } = true;
        public bool NotifyOnTaskAssigned { get; set; } = true;
        public bool NotifyOnComment { get; set; } = true;
        public bool NotifyOnStatusChange { get; set; } = true;
        public bool NotifyOnInvitation { get; set; } = true;

        public string Language { get; set; } = "az";
        public string Theme { get; set; } = "light";

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
