using System;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Aplication.DTOs.TeamMember
{
    public class CreateTeamMemberDto
    {
        public int TeamId { get; set; }
        public Guid UserId { get; set; }
        public TeamMemberRole Role { get; set; } = TeamMemberRole.Member;
    }
}
