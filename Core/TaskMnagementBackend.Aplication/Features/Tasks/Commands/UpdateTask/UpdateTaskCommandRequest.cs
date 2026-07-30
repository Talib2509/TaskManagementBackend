using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Aplication.Features.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommandRequest : IRequest<UpdateTaskCommandResponse>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public TaskPriority Priority { get; set; }
        public DateTime? Deadline { get; set; }
        public TaskVisibility Visibility { get; set; }
    }
}
