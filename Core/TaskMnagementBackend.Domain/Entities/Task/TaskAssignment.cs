using SmartTask.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMnagementBackend.Domain.Common;

namespace TaskMnagementBackend.Domain.Entities.Task
{
    public class TaskAssignment : BaseEntity
    {
        public Guid Id { get; set; }

        public Guid TaskId { get; set; }

        [ForeignKey(nameof(TaskId))]
        public ProjectTask Task { get; set; } = null!;

        public Guid UserId { get; set; }

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}
