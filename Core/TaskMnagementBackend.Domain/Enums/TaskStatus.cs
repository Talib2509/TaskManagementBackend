using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Domain.Enums
{
    public enum TaskStatus
    {
        Pending = 1,
        InProgress = 2,
        Completed = 3,
        Blocked = 4
    }
}
