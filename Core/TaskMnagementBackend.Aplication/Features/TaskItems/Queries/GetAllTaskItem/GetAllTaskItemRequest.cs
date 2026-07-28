using MediatR;
using TaskMnagementBackend.Aplication.Common.Pagination;

namespace TaskMnagementBackend.Aplication.Features.TaskItems.Queries.GetAllTaskItem
{
    public class GetAllTaskItemRequest : PagedRequest, IRequest<GetAllTaskItemResponse>
    {
    }
}