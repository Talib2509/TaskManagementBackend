using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.Teams.Queries.GetTeamStatistics
{
    public class GetTeamStatisticsHandler
        : IRequestHandler<GetTeamStatisticsRequest, GetTeamStatisticsResponse>
    {
        private readonly ITeamService _teamService;

        public GetTeamStatisticsHandler(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public async Task<GetTeamStatisticsResponse> Handle(
            GetTeamStatisticsRequest request,
            CancellationToken cancellationToken)
        {
            var statistics = await _teamService.GetStatisticsAsync(request.TeamId);

            if (statistics == null)
            {
                return new GetTeamStatisticsResponse
                {
                    Succeeded = false,
                    Message = "Komanda statistikası tapılmadı.",
                    ErrorType = ResultErrorType.NotFound
                };
            }

            return new GetTeamStatisticsResponse
            {
                Succeeded = true,
                Message = "Komanda statistikası uğurla əldə edildi.",
                Statistics = statistics
            };
        }
    }
}