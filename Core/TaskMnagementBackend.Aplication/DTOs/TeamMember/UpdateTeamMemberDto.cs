using System;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Aplication.DTOs.TeamMember
{
    public class UpdateTeamMemberDto
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public Guid UserId { get; set; }
        public TeamMemberRole Role { get; set; }
    }
}
