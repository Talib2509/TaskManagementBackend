using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Aplication.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? TaskId { get; set; }
    }
}
