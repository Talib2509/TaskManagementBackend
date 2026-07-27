using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Aplication.Features.Tasks.Commands.ClaimTask
{
    public class ClaimTaskCommandRequest : IRequest<ClaimTaskCommandResponse>
    {
        public Guid TaskId { get; set; }
    }
}
