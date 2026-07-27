using MediatR;
using TaskMnagementBackend.Aplication.Features.Company.Commands.CreateCompany;


namespace TaskMnagementBackend.Aplication.Features.Companies.Commands.CreateCompany
{
    public class CreateCompanyRequest : IRequest<CreateCompanyResponse>
    {
        public string Name { get; set; } = default!;

        public string? Description { get; set; }
    }

 
}
