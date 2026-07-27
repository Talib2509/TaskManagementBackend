using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMnagementBackend.Domain.Common;

namespace TaskMnagementBackend.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public string Message { get; set; } = null!;
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
