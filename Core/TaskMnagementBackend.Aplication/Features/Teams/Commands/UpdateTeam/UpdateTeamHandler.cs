using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.DTOs.Team;

namespace TaskMnagementBackend.Aplication.Features.Teams.Commands.UpdateTeam
{
    public class UpdateTeamHandler
        : IRequestHandler<UpdateTeamRequest, UpdateTeamResponse>
    {
        private readonly ITeamService _teamService;

        public UpdateTeamHandler(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public async Task<UpdateTeamResponse> Handle(
            UpdateTeamRequest request,
            CancellationToken cancellationToken)
        {
            var dto = new UpdateTeamDto
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                CompanyId = request.CompanyId,
                TeamLeadId = request.TeamLeadId
            };

            var result = await _teamService.UpdateAsync(dto);

            if (!result)
            {
                return new UpdateTeamResponse
                {
                    Succeeded = false,
                    Message = "Komanda yenilənərkən xəta baş verdi.",
                    ErrorType = ResultErrorType.Error
                };
            }

            return new UpdateTeamResponse
            {
                Succeeded = true,
                Message = "Komanda uğurla yeniləndi."
            };
        }
    }
}