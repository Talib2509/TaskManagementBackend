using MediatR;

namespace TaskMnagementBackend.Aplication.Features.Teams.Queries.GetMyTeams
{
    public class GetMyTeamsRequest : IRequest<GetMyTeamsResponse>
    {
        public Guid UserId { get; set; }
    }
}