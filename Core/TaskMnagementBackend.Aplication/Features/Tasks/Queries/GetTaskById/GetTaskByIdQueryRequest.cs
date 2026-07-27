using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Aplication.Features.Tasks.Queries.GetTaskById
{
    public class GetTaskByIdQueryRequest : IRequest<GetTaskByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }
}
