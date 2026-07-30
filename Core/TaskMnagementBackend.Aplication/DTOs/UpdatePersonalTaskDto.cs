using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Aplication.DTOs
{
    // Update DTO
    public record UpdatePersonalTaskDto(
        string Title,
        string? Description,
        TaskPriority TaskPriority,
        DateTime? Deadline
    );
}
