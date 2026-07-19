using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;
using TaskMnagementBackend.Domain.Common;
using TaskMnagementBackend.Persistence.Context;

namespace TaskMnagementBackend.Persistence.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;

        public ReadRepository(AppDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public IQueryable<T> GetAll()
        {
            return Table;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await Table.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method)
        {
            return await Table.FirstOrDefaultAsync(method);
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method)
        {
            return Table.Where(method);
        }
    }
}

