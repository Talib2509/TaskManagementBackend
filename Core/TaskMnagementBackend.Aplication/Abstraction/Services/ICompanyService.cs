using System.Linq;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.DTOs.Company;
using TaskMnagementBackend.Domain.Entities;

namespace TaskMnagementBackend.Aplication.Abstraction.Services
{
    public interface ICompanyService
    {
        IQueryable<CompanyDto> GetAll();

        Task<CompanyDto?> GetByIdAsync(int id);

        Task<CompanyDto?> GetByOwnerIdAsync(Guid ownerId);

        Task<Company?> GetMyCompanyAsync(Guid ownerId);

        Task<CompanyStatisticsDto> GetStatisticsAsync(int companyId);

        Task<bool> CreateAsync(CreateCompanyDto company);

        Task<bool> UpdateAsync(UpdateCompanyDto company);

        Task<bool> DeleteAsync(int id);
    }
}
