using TaskMnagementBackend.Aplication.Abstraction.IRepositories.Team;
using TaskMnagementBackend.Persistence.Context;

namespace TaskMnagementBackend.Persistence.Repositories.Team
{
    public class WriteTeamRepository : WriteRepository<Domain.Entities.Team>, IWriteTeamRepository
    {
        public WriteTeamRepository(AppDbContext context) : base(context)
        {
        }
    }
}
