using System;

namespace TaskMnagementBackend.Aplication.DTOs.TeamInvitation
{
    public class CreateTeamInvitationDto
    {
        public int TeamId { get; set; }
        public string Email { get; set; } = string.Empty;
        public Guid InvitedByUserId { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
