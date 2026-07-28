using System;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Aplication.DTOs.TeamInvitation
{
    public class TeamInvitationDto
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string Email { get; set; } = string.Empty;
        public Guid InvitedByUserId { get; set; }
        public Guid? InvitedUserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public InvitationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime? RespondedAt { get; set; }
    }
}
