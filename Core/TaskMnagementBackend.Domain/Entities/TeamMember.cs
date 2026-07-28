using System;
using TaskMnagementBackend.Domain.Common;
using TaskMnagementBackend.Domain.Entities.Identity;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Domain.Entities
{
    
    public class TeamMember : BaseEntity
    {
        public int TeamId { get; set; }

        public Team? Team { get; set; }

        public Guid UserId { get; set; }

        public AppUser? User { get; set; }

        public TeamMemberRole Role { get; set; } = TeamMemberRole.Member;
        public bool IsActive { get; set; } = true;
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
