using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskMnagementBackend.Domain.Common;
using TaskStatus = TaskMnagementBackend.Domain.Enums.TaskStatus;


namespace SmartTask.Domain.Entities;

public class TaskStatusHistory : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid TaskId { get; set; }

    [ForeignKey(nameof(TaskId))]
    public ProjectTask Task { get; set; } = null!;

    [Required]
    public Guid UserId { get; set; }

    public TaskStatus? OldStatus { get; set; }

    public TaskStatus NewStatus { get; set; }

    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}