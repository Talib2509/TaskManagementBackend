using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.IUnitOfWork;
using TaskMnagementBackend.Domain.Entities.Identity;
using TaskMnagementBackend.Persistence.Context;

namespace TaskMnagementBackend.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

        public UserManager<AppUser> UserManager { get; }
        public RoleManager<AppRole> RoleManager { get; }

      

        public UnitOfWork(
            AppDbContext context,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)

           
        {
            _context = context;

            UserManager = userManager;
            RoleManager = roleManager;

           
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is not null)
                return;

            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is null)
                return;

            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is null)
                return;

            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
