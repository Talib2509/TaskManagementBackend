using MediatR;

namespace TaskMnagementBackend.Aplication.Features.Invitations.Commands.RejectInvitation
{
    public class RejectInvitationRequest : IRequest<RejectInvitationResponse>
    {
        public int InvitationId { get; set; }
    }
}
