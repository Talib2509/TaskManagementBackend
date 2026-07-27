using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.DTOs.Team;

namespace TaskMnagementBackend.Aplication.Features.Teams.Queries.GetTeamById
{
    public class GetTeamByIdResponse : OperationResultBase
    {
        public TeamDto? Team { get; set; }
    }
}