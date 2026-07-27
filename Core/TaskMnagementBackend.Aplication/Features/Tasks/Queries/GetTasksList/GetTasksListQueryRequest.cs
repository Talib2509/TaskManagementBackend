using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMnagementBackend.Domain.Enums;
using TaskStatus = TaskMnagementBackend.Domain.Enums.TaskStatus;

namespace TaskMnagementBackend.Aplication.Features.Tasks.Queries.GetTasksList
{
    public class GetTasksListQueryRequest : IRequest<GetTasksListQueryResponse>
    {
        public TaskStatus? Status { get; set; }
        public TaskPriority? Priority { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
