using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.TeamInvitations.Queries.GetTeamInvitationById
{
    public class GetTeamInvitationByIdHandler
        : IRequestHandler<GetTeamInvitationByIdRequest, GetTeamInvitationByIdResponse>
    {
        private readonly ITeamInvitationService _teamInvitationService;

        public GetTeamInvitationByIdHandler(
            ITeamInvitationService teamInvitationService)
        {
            _teamInvitationService = teamInvitationService;
        }

        public async Task<GetTeamInvitationByIdResponse> Handle(
            GetTeamInvitationByIdRequest request,
            CancellationToken cancellationToken)
        {
            var invitation = await _teamInvitationService.GetByIdAsync(request.Id);

            if (invitation == null)
            {
                return new GetTeamInvitationByIdResponse
                {
                    Succeeded = false,
                    Message = "Dəvətnamə tapılmadı.",
                    ErrorType = ResultErrorType.NotFound
                };
            }

            return new GetTeamInvitationByIdResponse
            {
                Succeeded = true,
                Message = "Dəvətnamə uğurla əldə edildi.",
                Invitation = invitation
            };
        }
    }
}