using TaskMnagementBackend.Aplication.Abstraction.IRepositories.Team;
using TaskMnagementBackend.Persistence.Context;

namespace TaskMnagementBackend.Persistence.Repositories.Team
{
    public class ReadTeamRepository : ReadRepository<Domain.Entities.Team>, IReadTeamRepository
    {
        public ReadTeamRepository(AppDbContext context) : base(context)
        {
        }
    }
}
