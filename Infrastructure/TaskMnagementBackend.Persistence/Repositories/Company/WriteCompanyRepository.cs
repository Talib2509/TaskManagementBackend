using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories.Company;
using TaskMnagementBackend.Persistence.Context;

namespace TaskMnagementBackend.Persistence.Repositories.Company
{
    public class WriteCompanyRepository : WriteRepository<Domain.Entities.Company>, IWriteCompanyRepository
    {
        public WriteCompanyRepository(AppDbContext context) : base(context)
        {
        }
    }
}
