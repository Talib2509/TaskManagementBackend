using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.DTOs.TeamInvitation;

namespace TaskMnagementBackend.Aplication.Features.TeamInvitations.Queries.GetTeamInvitationById
{
    public class GetTeamInvitationByIdResponse : OperationResultBase
    {
        public TeamInvitationDto? Invitation { get; set; }
    }
}