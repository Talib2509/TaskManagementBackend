using TaskMnagementBackend.Aplication.Abstraction.IRepositories.TeamInvitation;
using TaskMnagementBackend.Persistence.Context;

namespace TaskMnagementBackend.Persistence.Repositories.TeamInvitation
{
    public class WriteTeamInvitationRepository : WriteRepository<Domain.Entities.TeamInvitation>, IWriteTeamInvitationRepository
    {
        public WriteTeamInvitationRepository(AppDbContext context) : base(context)
        {
        }
    }
}
