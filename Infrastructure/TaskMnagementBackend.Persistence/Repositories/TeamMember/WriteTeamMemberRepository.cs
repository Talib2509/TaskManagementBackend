using TaskMnagementBackend.Aplication.Abstraction.IRepositories.TeamMember;
using TaskMnagementBackend.Persistence.Context;

namespace TaskMnagementBackend.Persistence.Repositories.TeamMember
{
    public class WriteTeamMemberRepository : WriteRepository<Domain.Entities.TeamMember>, IWriteTeamMemberRepository
    {
        public WriteTeamMemberRepository(AppDbContext context) : base(context)
        {
        }
    }
}
