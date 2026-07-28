using MediatR;

namespace TaskMnagementBackend.Aplication.Features.TeamMembers.Queries.GetMembersByTeam
{
    public class GetMembersByTeamRequest : IRequest<GetMembersByTeamResponse>
    {
        public int TeamId { get; set; }
    }
}