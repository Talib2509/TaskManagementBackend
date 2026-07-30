using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Aplication.Features.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommandRequest : IRequest<DeleteTaskCommandResponse>
    {
        public Guid Id { get; set; }
    }
}
