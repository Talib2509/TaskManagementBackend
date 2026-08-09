using System;

namespace TaskMnagementBackend.Aplication.DTOs.UserSettings
{
    public class UserSettingsDto
    {
        public Guid UserId { get; set; }
        public bool EmailNotificationEnabled { get; set; }
        public bool NotifyOnTaskAssigned { get; set; }
        public bool NotifyOnComment { get; set; }
        public bool NotifyOnStatusChange { get; set; }
        public bool NotifyOnInvitation { get; set; }
        public string Language { get; set; } = "az";
        public string Theme { get; set; } = "light";
        public DateTime UpdatedAt { get; set; }
    }

    public class UpdateUserSettingsDto
    {
        public bool? EmailNotificationEnabled { get; set; }
        public bool? NotifyOnTaskAssigned { get; set; }
        public bool? NotifyOnComment { get; set; }
        public bool? NotifyOnStatusChange { get; set; }
        public bool? NotifyOnInvitation { get; set; }
        public string? Language { get; set; }
        public string? Theme { get; set; }
    }
}
