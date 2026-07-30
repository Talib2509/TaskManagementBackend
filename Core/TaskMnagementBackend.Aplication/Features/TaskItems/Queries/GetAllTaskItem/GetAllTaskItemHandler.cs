using MediatR;
using System.Linq.Expressions;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common.Pagination;
using TaskMnagementBackend.Aplication.DTOs.TaskItem;

namespace TaskMnagementBackend.Aplication.Features.TaskItems.Queries.GetAllTaskItem
{
    public class GetAllTaskItemHandler
        : IRequestHandler<GetAllTaskItemRequest, GetAllTaskItemResponse>
    {
        private readonly ITaskItemService _taskItemService;

        public GetAllTaskItemHandler(ITaskItemService taskItemService)
        {
            _taskItemService = taskItemService;
        }

        public async Task<GetAllTaskItemResponse> Handle(
            GetAllTaskItemRequest request,
            CancellationToken cancellationToken)
        {
            var query = _taskItemService.GetAll();

            var sortMap = new Dictionary<string, Expression<Func<TaskItemDto, object>>>
            {
                ["title"] = x => x.Title,
                ["createdat"] = x => x.CreatedAt,
                ["status"] = x => x.Status
            };

            query = query.ApplySort(
                request.SortBy,
                request.Desc,
                sortMap,
                "createdat");

            var result = await query.ToPagedResultAsync(
                request.Page,
                request.PageSize,
                cancellationToken);

            return new GetAllTaskItemResponse
            {
                Items = result.Items,
                TotalCount = result.TotalCount,
                Page = result.Page,
                PageSize = result.PageSize
            };
        }
    }
}