using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.DTOs.Team;

namespace TaskMnagementBackend.Aplication.Features.Teams.Queries.GetTeamById
{
    public class GetTeamByIdHandler
        : IRequestHandler<GetTeamByIdRequest, GetTeamByIdResponse>
    {
        private readonly ITeamService _teamService;

        public GetTeamByIdHandler(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public async Task<GetTeamByIdResponse> Handle(
            GetTeamByIdRequest request,
            CancellationToken cancellationToken)
        {
            var team = await _teamService.GetByIdAsync(request.Id);

            if (team == null)
            {
                return new GetTeamByIdResponse
                {
                    Succeeded = false,
                    Message = "Komanda tapılmadı.",
                    ErrorType = ResultErrorType.NotFound
                };
            }

            return new GetTeamByIdResponse
            {
                Succeeded = true,
                Message = "Komanda uğurla əldə edildi.",
                Team = new TeamDto
                {
                    Id = team.Id,
                    Name = team.Name,
                    Description = team.Description,
                    CompanyId = team.CompanyId,
                    TeamLeadId = team.TeamLeadId,
                    CreatedAt = team.CreatedAt,
                    IsDeleted = team.IsDeleted,
                    DeletedAt = team.DeletedAt
                }
            };
        }
    }
}