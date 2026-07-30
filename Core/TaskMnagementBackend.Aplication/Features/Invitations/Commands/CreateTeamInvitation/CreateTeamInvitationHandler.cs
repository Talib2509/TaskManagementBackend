using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.DTOs.TeamInvitation;

namespace TaskMnagementBackend.Aplication.Features.TeamInvitations.Commands.CreateTeamInvitation
{
    public class CreateTeamInvitationHandler
        : IRequestHandler<CreateTeamInvitationRequest, CreateTeamInvitationResponse>
    {
        private readonly ITeamInvitationService _teamInvitationService;

        public CreateTeamInvitationHandler(
            ITeamInvitationService teamInvitationService)
        {
            _teamInvitationService = teamInvitationService;
        }

        public async Task<CreateTeamInvitationResponse> Handle(
            CreateTeamInvitationRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _teamInvitationService.CreateAsync(
                new CreateTeamInvitationDto
                {
                    TeamId = request.TeamId,
                    Email = request.Email
                });

            if (!result)
            {
                return new CreateTeamInvitationResponse
                {
                    Succeeded = false,
                    Message = "Dəvətnamə göndərilə bilmədi.",
                    ErrorType = ResultErrorType.Error
                };
            }

            return new CreateTeamInvitationResponse
            {
                Succeeded = true,
                Message = "Dəvətnamə uğurla göndərildi."
            };
        }
    }
}