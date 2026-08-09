using System;

namespace TaskMnagementBackend.Aplication.DTOs.UserProfile
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? ProfilePicture { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? JobTitle { get; set; }
        public string? Timezone { get; set; }
        public bool IsActive { get; set; }
        public int? ActiveTeamId { get; set; }
        public DateTime CreatedAt { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
    }

    public class UpdateUserProfileDto
    {
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Bio { get; set; }
        public string? JobTitle { get; set; }
        public string? Timezone { get; set; }
        public int? ActiveTeamId { get; set; }
    }

    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
