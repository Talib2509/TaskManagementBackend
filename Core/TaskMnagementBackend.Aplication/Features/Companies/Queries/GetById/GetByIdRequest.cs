using MediatR;

namespace TaskMnagementBackend.Aplication.Features.Companies.Queries.GetById
{
    public class GetByIdRequest : IRequest<GetByIdResponse>
    {
        public int Id { get; set; }
    }
}
