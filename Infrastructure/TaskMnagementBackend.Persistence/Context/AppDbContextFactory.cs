using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Persistence.Context
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Передай здесь твою реальную строку подключения PostgreSQL
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=TaskManagement;Username=myuser;Password=123456");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
