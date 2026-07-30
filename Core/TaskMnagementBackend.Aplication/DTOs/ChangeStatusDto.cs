using TaskStatus = TaskMnagementBackend.Domain.Enums.TaskStatus;

namespace TaskMnagementBackend.Aplication.DTOs
{
    public class ChangeStatusDto
    {
        public TaskStatus NewStatus { get; set; }
    }
}
