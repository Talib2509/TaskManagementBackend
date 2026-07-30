using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Aplication.Features.Tasks.Commands.ReassignTask
{
    public class ReassignTaskCommandRequest : IRequest<ReassignTaskCommandResponse>
    {
        public Guid TaskId { get; set; }
        public Guid OldUserId { get; set; }
        public Guid NewUserId { get; set; }
    }
}
