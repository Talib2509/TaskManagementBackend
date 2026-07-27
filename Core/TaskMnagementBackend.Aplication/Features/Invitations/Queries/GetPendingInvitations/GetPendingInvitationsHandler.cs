using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.Features.Invitations.Queries.GetPendingInvitations;

namespace TaskMnagementBackend.Aplication.Features.TeamInvitations.Queries.GetPendingInvitations
{
    public class GetPendingInvitationsHandler
        : IRequestHandler<GetPendingInvitationsRequest, GetPendingInvitationsResponse>
    {
        private readonly ITeamInvitationService _teamInvitationService;

        public GetPendingInvitationsHandler(
            ITeamInvitationService teamInvitationService)
        {
            _teamInvitationService = teamInvitationService;
        }

        public async Task<GetPendingInvitationsResponse> Handle(
            GetPendingInvitationsRequest request,
            CancellationToken cancellationToken)
        {
            var invitations = await _teamInvitationService
                .GetPendingInvitationsAsync(request.UserId);

            return new GetPendingInvitationsResponse
            {
                Succeeded = true,
                Message = "Gözləyən dəvətnamələr uğurla əldə edildi.",
                Invitations = invitations
            };
        }
    }
}