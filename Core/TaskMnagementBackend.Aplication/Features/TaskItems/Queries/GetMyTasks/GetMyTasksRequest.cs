using MediatR;

namespace TaskMnagementBackend.Aplication.Features.TaskItems.Queries.GetMyTasks
{
    public class GetMyTasksRequest : IRequest<GetMyTasksResponse>
    {
        public Guid UserId { get; set; }
    }
}