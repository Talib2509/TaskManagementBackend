using System;
using TaskMnagementBackend.Domain.Common;
using TaskMnagementBackend.Domain.Entities.Identity;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Domain.Entities
{
    
    public class TeamInvitation : BaseEntity
    {
        public int TeamId { get; set; }

        public Team? Team { get; set; }


        public string Email { get; set; } = string.Empty;

        public Guid InvitedByUserId { get; set; }

        public AppUser? InvitedByUser { get; set; }

       
        public Guid? InvitedUserId { get; set; }

        public AppUser? InvitedUser { get; set; }


        public string Token { get; set; } = string.Empty;

        public InvitationStatus Status { get; set; } = InvitationStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

       
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddDays(7);

        public DateTime? RespondedAt { get; set; }
    }
}
