using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.DTOs.TeamMember;

namespace TaskMnagementBackend.Aplication.Features.TeamMembers.Queries.GetTeamMemberById
{
    public class GetTeamMemberByIdResponse : OperationResultBase
    {
        public TeamMemberDto? TeamMember { get; set; }
    }
}