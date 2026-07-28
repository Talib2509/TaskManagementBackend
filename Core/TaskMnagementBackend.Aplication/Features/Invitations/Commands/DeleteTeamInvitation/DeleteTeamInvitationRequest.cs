using MediatR;

namespace TaskMnagementBackend.Aplication.Features.Invitations.Commands.DeleteTeamInvitation
{
    public class DeleteTeamInvitationRequest : IRequest<DeleteTeamInvitationResponse>
    {
        public int Id { get; set; }
    }
}