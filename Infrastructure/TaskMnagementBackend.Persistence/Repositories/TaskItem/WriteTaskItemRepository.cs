using TaskMnagementBackend.Aplication.Abstraction.IRepositories.TaskItem;
using TaskMnagementBackend.Persistence.Context;

namespace TaskMnagementBackend.Persistence.Repositories.TaskItem
{
    public class WriteTaskItemRepository : WriteRepository<Domain.Entities.TaskItem>, IWriteTaskItemRepository
    {
        public WriteTaskItemRepository(AppDbContext context) : base(context)
        {
        }
    }
}
