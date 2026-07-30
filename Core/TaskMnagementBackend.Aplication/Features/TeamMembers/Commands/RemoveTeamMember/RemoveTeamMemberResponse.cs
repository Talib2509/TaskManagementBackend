using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.DTOs.TeamMember;

namespace TaskMnagementBackend.Aplication.Features.TeamMembers.Commands.RemoveTeamMember
{
    public class RemoveTeamMemberResponse : OperationResultBase
    {
        public RemoveTeamMemberResultDto? Result { get; set; }
    }
}