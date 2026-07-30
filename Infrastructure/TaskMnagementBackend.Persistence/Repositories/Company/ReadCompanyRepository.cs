using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories.Company;
using TaskMnagementBackend.Persistence.Context;

namespace TaskMnagementBackend.Persistence.Repositories.Company
{
    public class ReadCompanyRepository : ReadRepository<Domain.Entities.Company>, IReadCompnayRepository
    {
        public ReadCompanyRepository(AppDbContext context) : base(context)
        {
        }
    }
}
