using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.DTOs.Team;

namespace TaskMnagementBackend.Aplication.Features.Teams.Queries.GetTeamStatistics
{
    public class GetTeamStatisticsResponse : OperationResultBase
    {
        public TeamStatisticsDto? Statistics { get; set; }
    }
}