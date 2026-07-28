using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.DTOs.TaskItem;

namespace TaskMnagementBackend.Aplication.Features.TaskItems.Queries.GetTaskItemById
{
    public class GetTaskItemByIdResponse : OperationResultBase
    {
        public TaskItemDto? TaskItem { get; set; }
    }
}