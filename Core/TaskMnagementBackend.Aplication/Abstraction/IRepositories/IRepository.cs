using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Aplication.Abstraction.IRepositories
{
    public interface IRepository<T> where T : class
    {
        public DbSet<T> Table { get; }
    }
}
