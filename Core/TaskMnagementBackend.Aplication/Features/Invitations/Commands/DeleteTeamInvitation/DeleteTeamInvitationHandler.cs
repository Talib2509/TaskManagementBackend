using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.Invitations.Commands.DeleteTeamInvitation
{
    public class DeleteTeamInvitationHandler
        : IRequestHandler<DeleteTeamInvitationRequest, DeleteTeamInvitationResponse>
    {
        private readonly ITeamInvitationService _teamInvitationService;

        public DeleteTeamInvitationHandler(ITeamInvitationService teamInvitationService)
        {
            _teamInvitationService = teamInvitationService;
        }

        public async Task<DeleteTeamInvitationResponse> Handle(
            DeleteTeamInvitationRequest request,
            CancellationToken cancellationToken)
        {
            var invitation = await _teamInvitationService.GetByIdAsync(request.Id);

            if (invitation == null)
            {
                return new DeleteTeamInvitationResponse
                {
                    Succeeded = false,
                    Message = "Dəvətnamə tapılmadı.",
                    ErrorType = ResultErrorType.NotFound
                };
            }

            var result = await _teamInvitationService.DeleteAsync(request.Id);

            if (!result)
            {
                return new DeleteTeamInvitationResponse
                {
                    Succeeded = false,
                    Message = "Dəvətnamə silinə bilmədi.",
                    ErrorType = ResultErrorType.Error
                };
            }

            return new DeleteTeamInvitationResponse
            {
                Succeeded = true,
                Message = "Dəvətnamə uğurla silindi."
            };
        }
    }
}