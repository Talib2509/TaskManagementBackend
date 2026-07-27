using MediatR;

namespace TaskMnagementBackend.Aplication.Features.Companies.Commands.DeleteCompany
{
    public class DeleteCompanyRequest : IRequest<DeleteCompanyResponse>
    {
        public int Id { get; set; }
    }
}
