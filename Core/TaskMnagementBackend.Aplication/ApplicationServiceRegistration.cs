
using Microsoft.Extensions.DependencyInjection;


namespace TaskMnagementBackend.Aplication
{
    public static class ApplicationServiceRegistration
    {
        public static void AddApplicationService(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(
                    typeof(ApplicationServiceRegistration).Assembly));
        }
    }
}
