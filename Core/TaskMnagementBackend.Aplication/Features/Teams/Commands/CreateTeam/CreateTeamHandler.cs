using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.DTOs.Team;

namespace TaskMnagementBackend.Aplication.Features.Teams.Commands.CreateTeam
{
    public class CreateTeamHandler
        : IRequestHandler<CreateTeamRequest, CreateTeamResponse>
    {
        private readonly ITeamService _teamService;

        public CreateTeamHandler(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public async Task<CreateTeamResponse> Handle(
            CreateTeamRequest request,
            CancellationToken cancellationToken)
        {
            var dto = new CreateTeamDto
            {
                Name = request.Name,
                Description = request.Description,
                CompanyId = request.CompanyId,
                TeamLeadId = request.TeamLeadId
            };

            var result = await _teamService.CreateAsync(dto);

            if (!result)
            {
                return new CreateTeamResponse
                {
                    Succeeded = false,
                    Message = "Komanda yaradılarkən xəta baş verdi.",
                    ErrorType = ResultErrorType.Error
                };
            }

            return new CreateTeamResponse
            {
                Succeeded = true,
                Message = "Komanda uğurla yaradıldı."
            };
        }
    }
}