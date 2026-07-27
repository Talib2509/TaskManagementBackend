using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.Teams.Commands.AssignTeamLead
{
    public class AssignTeamLeadHandler
        : IRequestHandler<AssignTeamLeadRequest, AssignTeamLeadResponse>
    {
        private readonly ITeamService _teamService;

        public AssignTeamLeadHandler(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public async Task<AssignTeamLeadResponse> Handle(
            AssignTeamLeadRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _teamService.AssignLeadAsync(
                request.TeamId,
                request.UserId);

            if (!result)
            {
                return new AssignTeamLeadResponse
                {
                    Succeeded = false,
                    Message = "Team Lead təyin edilərkən xəta baş verdi.",
                    ErrorType = ResultErrorType.Error
                };
            }

            return new AssignTeamLeadResponse
            {
                Succeeded = true,
                Message = "Team Lead uğurla təyin edildi."
            };
        }
    }
}