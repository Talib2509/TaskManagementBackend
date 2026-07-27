using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.DTOs.TaskItem;

namespace TaskMnagementBackend.Aplication.Features.TaskItems.Queries.GetMyTasks
{
    public class GetMyTasksResponse : OperationResultBase
    {
        public IEnumerable<TaskItemDto> Tasks { get; set; } = new List<TaskItemDto>();
    }
}