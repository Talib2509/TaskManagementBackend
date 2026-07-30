using TaskMnagementBackend.Aplication.Abstraction.IRepositories.TeamMember;
using TaskMnagementBackend.Persistence.Context;

namespace TaskMnagementBackend.Persistence.Repositories.TeamMember
{
    public class ReadTeamMemberRepository : ReadRepository<Domain.Entities.TeamMember>, IReadTeamMemberRepository
    {
        public ReadTeamMemberRepository(AppDbContext context) : base(context)
        {
        }
    }
}
