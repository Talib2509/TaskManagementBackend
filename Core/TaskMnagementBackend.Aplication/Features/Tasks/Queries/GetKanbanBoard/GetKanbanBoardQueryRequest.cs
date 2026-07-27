using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Aplication.Features.Tasks.Queries.GetKanbanBoard
{
    public class GetKanbanBoardQueryRequest : IRequest<GetKanbanBoardQueryResponse>
    {
    }
}
