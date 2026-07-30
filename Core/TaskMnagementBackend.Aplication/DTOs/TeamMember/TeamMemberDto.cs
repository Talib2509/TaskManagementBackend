using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Aplication.DTOs.TeamMember
{
    public class TeamMemberDto
    {
        public int Id { get; set; }

        public int TeamId { get; set; }

        public Guid UserId { get; set; }

        public TeamMemberRole Role { get; set; }

        public DateTime JoinedAt { get; set; }
    }
}