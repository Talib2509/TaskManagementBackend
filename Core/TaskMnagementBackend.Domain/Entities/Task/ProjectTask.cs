using System.ComponentModel.DataAnnotations;
using TaskMnagementBackend.Domain.Enums;
using TaskStatus = TaskMnagementBackend.Domain.Enums.TaskStatus;

namespace SmartTask.Domain.Entities;

using TaskMnagementBackend.Domain.Common; // Укажи правильный namespace, где лежит твой BaseEntity
using TaskMnagementBackend.Domain.Entities.Task;

public class ProjectTask : BaseEntity
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }

    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    public TaskStatus Status { get; set; } = TaskStatus.Pending;

    public TaskType Type { get; set; } = TaskType.Personal;

    public DateTime? Deadline { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Владелец задачи
    [Required]
    public Guid UserId { get; set; }

    // Навигационные свойства
    public List<SubTask> SubTasks { get; set; } = new();
    public List<TaskStatusHistory> StatusHistories { get; set; } = new();

    public Guid TeamId { get; set; } // Связь с командой

    public TaskVisibility Visibility { get; set; } = TaskVisibility.Public;

    public List<TaskAssignment> Assignments { get; set; } = new();

}