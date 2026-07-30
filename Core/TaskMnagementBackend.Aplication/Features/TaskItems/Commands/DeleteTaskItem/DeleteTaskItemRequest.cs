using MediatR;

namespace TaskMnagementBackend.Aplication.Features.TaskItems.Commands.DeleteTaskItem
{
    public class DeleteTaskItemRequest : IRequest<DeleteTaskItemResponse>
    {
        public int Id { get; set; }
    }
}
