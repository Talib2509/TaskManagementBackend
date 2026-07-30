using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Aplication.Features.Tasks.Commands.Subtasks
{
    public class CreateSubTaskCommandRequest : IRequest<CreateSubTaskCommandResponse>
    {
        public Guid TaskId { get; set; }
        public string Text { get; set; } = null!;
    }
}
