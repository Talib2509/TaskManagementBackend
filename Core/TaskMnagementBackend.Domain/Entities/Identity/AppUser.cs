using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using TaskMnagementBackend.Domain.Entities;

namespace TaskMnagementBackend.Domain.Entities.Identity
{
    public class AppUser : IdentityUser<Guid>
    {
        public string? FullName { get; set; }
        public string? ProfilePicture { get; set; }
        public string CompanyName { get; set; }
        public string? Bio { get; set; }
        public string? JobTitle { get; set; }
        public string? Timezone { get; set; } = "Asia/Baku";
        public bool IsActive { get; set; } = true;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenEndDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? ActiveTeamId { get; set; }

        public UserSettings? Settings { get; set; }
    }
}
