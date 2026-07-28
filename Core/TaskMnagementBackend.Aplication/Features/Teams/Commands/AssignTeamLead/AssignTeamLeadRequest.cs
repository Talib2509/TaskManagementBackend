using MediatR;

namespace TaskMnagementBackend.Aplication.Features.Teams.Commands.AssignTeamLead
{
    /// <summary>
    /// Company Owner mövcud komanda üzvlərindən birini Team Lead-ə "yüksəldir".
    /// </summary>
    public class AssignTeamLeadRequest : IRequest<AssignTeamLeadResponse>
    {
        public int TeamId { get; set; }

        /// <summary>
        /// Lider təyin ediləcək istifadəçi — komandanın artıq mövcud üzvü olmalıdır.
        /// </summary>
        public Guid UserId { get; set; }
    }
}
