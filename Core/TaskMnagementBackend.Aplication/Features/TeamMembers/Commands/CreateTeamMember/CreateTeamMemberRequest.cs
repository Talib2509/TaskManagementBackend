using MediatR;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Aplication.Features.TeamMembers.Commands.CreateTeamMember
{
    public class CreateTeamMemberRequest : IRequest<CreateTeamMemberResponse>
    {
        public int TeamId { get; set; }

        public Guid UserId { get; set; }

        public TeamMemberRole Role { get; set; } = TeamMemberRole.Member;
    }
}