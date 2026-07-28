using MediatR;
using TaskMnagementBackend.Aplication.Common.Pagination;

namespace TaskMnagementBackend.Aplication.Features.TeamMembers.Queries.GetAllTeamMember
{
    public class GetAllTeamMemberRequest : PagedRequest, IRequest<GetAllTeamMemberResponse>
    {
    }
}