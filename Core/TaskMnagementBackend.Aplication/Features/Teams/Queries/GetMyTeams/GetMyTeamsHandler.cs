using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.Teams.Queries.GetMyTeams
{
    public class GetMyTeamsHandler
        : IRequestHandler<GetMyTeamsRequest, GetMyTeamsResponse>
    {
        private readonly ITeamService _teamService;

        public GetMyTeamsHandler(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public async Task<GetMyTeamsResponse> Handle(
            GetMyTeamsRequest request,
            CancellationToken cancellationToken)
        {
            var teams = await _teamService.GetMyTeamsAsync(request.UserId);

            if (teams == null || !teams.Any())
            {
                return new GetMyTeamsResponse
                {
                    Succeeded = false,
                    Message = "İstifadəçinin aid olduğu komanda tapılmadı.",
                    ErrorType = ResultErrorType.NotFound
                };
            }

            return new GetMyTeamsResponse
            {
                Succeeded = true,
                Message = "Komandalar uğurla əldə edildi.",
                Teams = teams
            };
        }
    }
}