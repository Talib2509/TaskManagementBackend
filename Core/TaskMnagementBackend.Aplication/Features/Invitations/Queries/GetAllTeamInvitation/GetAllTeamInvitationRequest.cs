using MediatR;
using TaskMnagementBackend.Aplication.Common.Pagination;

namespace TaskMnagementBackend.Aplication.Features.TeamInvitations.Queries.GetAllTeamInvitation
{
    public class GetAllTeamInvitationRequest
        : PagedRequest, IRequest<GetAllTeamInvitationResponse>
    {
    }
}