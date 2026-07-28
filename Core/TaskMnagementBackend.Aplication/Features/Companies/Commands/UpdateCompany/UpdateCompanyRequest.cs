using MediatR;

namespace TaskMnagementBackend.Aplication.Features.Companies.Commands.UpdateCompany
{
    public class UpdateCompanyRequest : IRequest<UpdateCompanyResponse>
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public string? Description { get; set; }
    }
}
