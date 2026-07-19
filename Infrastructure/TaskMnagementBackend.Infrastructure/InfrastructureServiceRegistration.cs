using Microsoft.Extensions.DependencyInjection;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Infrastructure.Services;


namespace TaskMnagementBackend.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
       
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
            

          
            return services;
        }
    }
}
