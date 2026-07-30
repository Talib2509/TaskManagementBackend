using TaskMnagementBackend.Aplication.Abstraction.IRepositories.TaskItem;
using TaskMnagementBackend.Persistence.Context;

namespace TaskMnagementBackend.Persistence.Repositories.TaskItem
{
    public class ReadTaskItemRepository : ReadRepository<Domain.Entities.TaskItem>, IReadTaskItemRepository
    {
        public ReadTaskItemRepository(AppDbContext context) : base(context)
        {
        }
    }
}
