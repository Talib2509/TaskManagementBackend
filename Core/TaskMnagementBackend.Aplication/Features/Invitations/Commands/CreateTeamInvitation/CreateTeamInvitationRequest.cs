using MediatR;

namespace TaskMnagementBackend.Aplication.Features.TeamInvitations.Commands.CreateTeamInvitation
{
    public class CreateTeamInvitationRequest : IRequest<CreateTeamInvitationResponse>
    {
        public int TeamId { get; set; }

        public string Email { get; set; } = string.Empty;
    }
}