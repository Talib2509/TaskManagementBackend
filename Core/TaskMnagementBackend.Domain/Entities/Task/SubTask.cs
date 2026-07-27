using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskMnagementBackend.Domain.Common;

namespace SmartTask.Domain.Entities;

public class SubTask : BaseEntity
{

    public Guid Id { get; set; }

    [Required]
    public Guid TaskId { get; set; }

    [ForeignKey(nameof(TaskId))]
    public ProjectTask Task { get; set; } = null!;

    [Required]
    [MaxLength(500)]
    public string Text { get; set; } = string.Empty;

    public bool IsCompleted { get; set; } = false;
}