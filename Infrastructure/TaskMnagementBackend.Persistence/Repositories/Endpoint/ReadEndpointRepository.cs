using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories.Endpoint;
using TaskMnagementBackend.Persistence.Context;

namespace TaskMnagementBackend.Persistence.Repositories.Endpoint
{
    public class ReadEndpointRepository : ReadRepository<Domain.Entities.Endpoint>, IReadEndpointRepository
    {
        public ReadEndpointRepository(AppDbContext context) : base(context)
        {
        }
    }
}
