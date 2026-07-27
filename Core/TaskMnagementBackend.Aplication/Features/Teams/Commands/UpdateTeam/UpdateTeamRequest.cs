using MediatR;

namespace TaskMnagementBackend.Aplication.Features.Teams.Commands.UpdateTeam
{
    public class UpdateTeamRequest : IRequest<UpdateTeamResponse>
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int CompanyId { get; set; }

        public Guid? TeamLeadId { get; set; }
    }
}