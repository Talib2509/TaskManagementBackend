using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Aplication.DTOs
{
    public record CreatePersonalTaskDto(
    string Title,
    string? Description,
    TaskPriority TaskPriority,
    DateTime? Deadline
);
}
