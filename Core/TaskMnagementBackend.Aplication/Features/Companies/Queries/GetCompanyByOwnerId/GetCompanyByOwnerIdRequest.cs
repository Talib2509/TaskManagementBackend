using MediatR;

namespace TaskMnagementBackend.Aplication.Features.Companies.Queries.GetCompanyByOwnerId
{
    public class GetCompanyByOwnerIdRequest : IRequest<GetCompanyByOwnerIdResponse>
    {
        public Guid OwnerId { get; set; }
    }
}