using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.DTOs.Team;

namespace TaskMnagementBackend.Aplication.Features.Teams.Queries.GetMyTeams
{
    public class GetMyTeamsResponse : OperationResultBase
    {
        public IEnumerable<TeamDto> Teams { get; set; } = new List<TeamDto>();
    }
}