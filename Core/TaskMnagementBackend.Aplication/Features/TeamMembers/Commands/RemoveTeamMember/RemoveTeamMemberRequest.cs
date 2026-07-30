using MediatR;

namespace TaskMnagementBackend.Aplication.Features.TeamMembers.Commands.RemoveTeamMember
{
    public class RemoveTeamMemberRequest : IRequest<RemoveTeamMemberResponse>
    {
        public int TeamId { get; set; }

        public Guid UserId { get; set; }
    }
}
