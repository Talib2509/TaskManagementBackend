using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.Features.Invitations.Commands.AcceptInvitation;

namespace TaskMnagementBackend.Aplication.Features.Invitations.Commands.AcceptInvitation
{
    public class AcceptInvitationHandler
        : IRequestHandler<AcceptInvitationRequest, AcceptInvitationResponse>
    {
        private readonly ITeamInvitationService _teamInvitationService;

        public AcceptInvitationHandler(ITeamInvitationService teamInvitationService)
        {
            _teamInvitationService = teamInvitationService;
        }

        public async Task<AcceptInvitationResponse> Handle(
            AcceptInvitationRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _teamInvitationService.AcceptAsync(request.InvitationId);

            if (!result)
            {
                return new AcceptInvitationResponse
                {
                    Succeeded = false,
                    Message = "Dəvətnamə qəbul edilə bilmədi.",
                    ErrorType = ResultErrorType.BadRequest
                };
            }

            return new AcceptInvitationResponse
            {
                Succeeded = true,
                Message = "Dəvətnamə uğurla qəbul edildi."
            };
        }
    }
}