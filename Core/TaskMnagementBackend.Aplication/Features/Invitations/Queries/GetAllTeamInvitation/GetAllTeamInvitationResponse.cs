using TaskMnagementBackend.Aplication.Common.Pagination;
using TaskMnagementBackend.Aplication.DTOs.TeamInvitation;

namespace TaskMnagementBackend.Aplication.Features.TeamInvitations.Queries.GetAllTeamInvitation
{
    public class GetAllTeamInvitationResponse
        : PagedResult<TeamInvitationDto>
    {
    }
}