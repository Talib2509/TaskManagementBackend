using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Aplication.Features.Tasks.Commands.ToggleSubTask
{
    public class ToggleSubTaskCommandRequest : IRequest<ToggleSubTaskCommandResponse>
    {
        public Guid SubTaskId { get; set; }
    }
}
