
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using TaskMnagementBackend.Aplication.Behaviors;


namespace TaskMnagementBackend.Aplication
{
    public static class ApplicationServiceRegistration
    {
        public static void AddApplicationService(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(
                    typeof(ApplicationServiceRegistration).Assembly);
                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });
        }
    }
}
