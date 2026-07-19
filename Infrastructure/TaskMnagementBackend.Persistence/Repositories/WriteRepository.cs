using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;
using TaskMnagementBackend.Domain.Common;
using TaskMnagementBackend.Persistence.Context;

namespace TaskMnagementBackend.Persistence.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;

        public WriteRepository(AppDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public async Task<bool> AddAsync(T t)
        {
            EntityEntry entityEntry = await Table.AddAsync(t);
            return entityEntry.State == EntityState.Added;
        }

        public async Task<bool> AddRange(List<T> datas)
        {
            await Table.AddRangeAsync(datas);
            return true;
        }

        public bool Delete(T model)
        {
            EntityEntry entityEntry = Table.Remove(model);
            return entityEntry.State == EntityState.Deleted;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var data = await Table.FirstOrDefaultAsync(x => x.Id == id);
            return Delete(data);

        }

        public bool DeleteRange(List<T> datas)
        {
            Table.RemoveRange(datas);
            return true;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public bool Update(T t)
        {
            EntityEntry entityEntry = _context.Update(t);
            return entityEntry.State == EntityState.Modified;
        }
    }
}
