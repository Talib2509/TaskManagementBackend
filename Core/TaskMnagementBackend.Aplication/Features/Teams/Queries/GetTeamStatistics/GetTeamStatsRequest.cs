using MediatR;

namespace TaskMnagementBackend.Aplication.Features.Teams.Queries.GetTeamStatistics
{
    public class GetTeamStatisticsRequest : IRequest<GetTeamStatisticsResponse>
    {
        public int TeamId { get; set; }
    }
}