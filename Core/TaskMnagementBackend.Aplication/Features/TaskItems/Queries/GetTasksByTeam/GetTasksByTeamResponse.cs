using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.DTOs.TaskItem;

namespace TaskMnagementBackend.Aplication.Features.TaskItems.Queries.GetTasksByTeam
{
    public class GetTasksByTeamResponse : OperationResultBase
    {
        public IEnumerable<TaskItemDto> Tasks { get; set; } = new List<TaskItemDto>();
    }
}