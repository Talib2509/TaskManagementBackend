
    using MediatR;

    namespace TaskMnagementBackend.Aplication.Features.TaskItems.Queries.GetTaskItemById
    {
        public class GetTaskItemByIdRequest : IRequest<GetTaskItemByIdResponse>
        {
            public int Id { get; set; }
        }
    }
