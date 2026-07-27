using TaskMnagementBackend.Aplication.Abstraction.IRepositories.TeamInvitation;
using TaskMnagementBackend.Persistence.Context;

namespace TaskMnagementBackend.Persistence.Repositories.TeamInvitation
{
    public class ReadTeamInvitationRepository : ReadRepository<Domain.Entities.TeamInvitation>, IReadTeamInvitationRepository
    {
        public ReadTeamInvitationRepository(AppDbContext context) : base(context)
        {
        }
    }
}
