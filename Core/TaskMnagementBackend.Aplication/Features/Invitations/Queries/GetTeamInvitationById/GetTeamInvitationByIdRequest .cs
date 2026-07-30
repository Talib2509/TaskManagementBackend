using MediatR;

namespace TaskMnagementBackend.Aplication.Features.TeamInvitations.Queries.GetTeamInvitationById
{
    public class GetTeamInvitationByIdRequest : IRequest<GetTeamInvitationByIdResponse>
    {
        public int Id { get; set; }
    }
}