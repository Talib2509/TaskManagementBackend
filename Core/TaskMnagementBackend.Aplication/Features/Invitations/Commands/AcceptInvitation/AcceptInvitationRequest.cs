using MediatR;
using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.Invitations.Commands.AcceptInvitation
{
    public class AcceptInvitationRequest : IRequest<AcceptInvitationResponse>
    {
        public int InvitationId { get; set; }
    }

   
}
