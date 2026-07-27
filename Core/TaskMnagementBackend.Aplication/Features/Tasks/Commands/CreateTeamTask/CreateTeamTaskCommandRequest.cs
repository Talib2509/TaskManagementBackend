using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Aplication.Features.Tasks.Commands.CreateTeamTask
{
    public class CreateTeamTaskCommandRequest : IRequest<CreateTeamTaskCommandResponse>
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public TaskPriority Priority { get; set; }
        public DateTime? Deadline { get; set; }
        public Guid TeamId { get; set; }
        public TaskVisibility Visibility { get; set; } = TaskVisibility.Public;
        public List<Guid> AssigneeIds { get; set; } = new(); // Исполнители (один или несколько)
    }
}
