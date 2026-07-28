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
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ITaskItemService, TaskItemService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<ITeamMemberService, TeamMemberService>();
            services.AddScoped<ITeamInvitationService, TeamInvitationService>();
            services.AddScoped<INotificationService, NotificationService>();
            

          
            return services;
        }
    }
}
