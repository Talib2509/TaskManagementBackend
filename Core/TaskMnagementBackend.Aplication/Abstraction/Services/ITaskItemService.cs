using System.Linq;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.DTOs.TaskItem;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Aplication.Abstraction.Services
{
    public interface ITaskItemService
    {
        IQueryable<TaskItemDto> GetAll();

        Task<TaskItemDto?> GetByIdAsync(int id);

        Task<IEnumerable<TaskItemDto>> GetByTeamAsync(int teamId);

        Task<IEnumerable<TaskItemDto>> GetMyTasksAsync(Guid userId);

        Task<bool> CreateAsync(CreateTaskItemDto dto);

        Task<bool> UpdateAsync(UpdateTaskItemDto dto);

        Task<bool> DeleteAsync(int id);

        Task<bool> ChangeStatusAsync(int id, TaskItemStatus status);

        Task<bool> AssignMemberAsync(int taskId, Guid userId);
    }
}
