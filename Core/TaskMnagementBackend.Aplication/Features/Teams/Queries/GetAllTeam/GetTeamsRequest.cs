using MediatR;
using TaskMnagementBackend.Aplication.Common.Pagination;

namespace TaskMnagementBackend.Aplication.Features.Teams.Queries.GetAllTeam
{
    public class GetAllTeamRequest : PagedRequest, IRequest<GetAllTeamResponse>
    {
    }
}