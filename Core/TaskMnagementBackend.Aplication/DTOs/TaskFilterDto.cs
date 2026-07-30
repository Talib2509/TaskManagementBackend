using TaskMnagementBackend.Domain.Enums;
using TaskStatus = TaskMnagementBackend.Domain.Enums.TaskStatus;

namespace TaskMnagementBackend.Aplication.DTOs
{
    // Filter DTO
    public record TaskFilterDto(
        TaskStatus? Status,
        TaskPriority? TaskPriority,
        DateTime? DateFrom,
        DateTime? DateTo
    );
}
