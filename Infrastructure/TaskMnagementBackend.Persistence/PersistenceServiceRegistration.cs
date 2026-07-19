using Microsoft.Extensions.DependencyInjection;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;
using TaskMnagementBackend.Aplication.IUnitOfWork;
using TaskMnagementBackend.Persistence.Repositories;


namespace TaskMnagementBackend.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
            services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}