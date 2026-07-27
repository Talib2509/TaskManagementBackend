using MediatR;

namespace TaskMnagementBackend.Aplication.Features.TaskItems.Queries.GetTasksByTeam
{
    public class GetTasksByTeamRequest : IRequest<GetTasksByTeamResponse>
    {
        public int TeamId { get; set; }
    }
}