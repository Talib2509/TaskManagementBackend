using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Aplication.Features.Tasks.Queries.SearchTasks
{
    public class SearchTasksQueryRequest : IRequest<SearchTasksQueryResponse>
    {
        public string Query { get; set; } = string.Empty;
    }
}
