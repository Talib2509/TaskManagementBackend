using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Domain.Entities;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Aplication.Features.Invitations.Commands.RejectInvitation
{
    public class RejectInvitationHandler
         : IRequestHandler<RejectInvitationRequest, RejectInvitationResponse>
    {
        private readonly ITeamInvitationService _teamInvitationService;

        public RejectInvitationHandler(ITeamInvitationService teamInvitationService)
        {
            _teamInvitationService = teamInvitationService;
        }

        public async Task<RejectInvitationResponse> Handle(
            RejectInvitationRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _teamInvitationService.RejectAsync(request.InvitationId);

            if (!result)
            {
                return new RejectInvitationResponse
                {
                    Succeeded = false,
                    Message = "Dəvətnamə rədd edilə bilmədi.",
                    ErrorType = ResultErrorType.BadRequest
                };
            }

            return new RejectInvitationResponse
            {
                Succeeded = true,
                Message = "Dəvətnamə uğurla rədd edildi."
            };
        }
    }
}
