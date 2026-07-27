using MediatR;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Aplication.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandRequest : IRequest<CreateTaskCommandResponse>
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public TaskPriority Priority { get; set; }
        public TaskVisibility Visibility { get; set; } = TaskVisibility.Public;


        public DateTime? Deadline { get; set; }
    }
}
