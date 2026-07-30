using Microsoft.Extensions.DependencyInjection;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Infrastructure.Services;
using TaskMnagementBackend.Infrastructure.Services.Storage;


namespace TaskMnagementBackend.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
       
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IStorageService, LocalDiskStorageService>();


            return services;
        }
    }
}
