using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.DTOs.TeamMember;

namespace TaskMnagementBackend.Aplication.Features.TeamMembers.Queries.GetMembersByTeam
{
    public class GetMembersByTeamResponse : OperationResultBase
    {
        public IEnumerable<TeamMemberDto> Members { get; set; } = new List<TeamMemberDto>();
    }
}