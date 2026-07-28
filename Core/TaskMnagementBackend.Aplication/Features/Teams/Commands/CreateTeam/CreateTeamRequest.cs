using MediatR;

namespace TaskMnagementBackend.Aplication.Features.Teams.Commands.CreateTeam
{
    public class CreateTeamRequest : IRequest<CreateTeamResponse>
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int CompanyId { get; set; }

      
        public Guid? TeamLeadId { get; set; }
    }
}