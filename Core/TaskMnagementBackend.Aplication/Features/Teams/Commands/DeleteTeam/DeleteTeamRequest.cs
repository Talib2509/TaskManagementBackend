using MediatR;

namespace TaskMnagementBackend.Aplication.Features.Teams.Commands.DeleteTeam
{
    public class DeleteTeamRequest : IRequest<DeleteTeamResponse>
    {
        public int TeamId { get; set; }
    }
}
