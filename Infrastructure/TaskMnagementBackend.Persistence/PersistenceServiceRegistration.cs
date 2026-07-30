using Microsoft.Extensions.DependencyInjection;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories.Company;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories.Endpoint;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories.Notfication;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories.TaskItem;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories.Team;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories.TeamInvitation;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories.TeamMember;
using TaskMnagementBackend.Aplication.IUnitOfWork;
using TaskMnagementBackend.Persistence.Repositories;
using TaskMnagementBackend.Persistence.Repositories.Company;
using TaskMnagementBackend.Persistence.Repositories.Endpoint;
using TaskMnagementBackend.Persistence.Repositories.Notfication;
using TaskMnagementBackend.Persistence.Repositories.TaskItem;
using TaskMnagementBackend.Persistence.Repositories.Team;
using TaskMnagementBackend.Persistence.Repositories.TeamInvitation;
using TaskMnagementBackend.Persistence.Repositories.TeamMember;


namespace TaskMnagementBackend.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
            services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));
            services.AddScoped<IWriteCompanyRepository, WriteCompanyRepository>();
            services.AddScoped<IReadCompnayRepository, ReadCompanyRepository>();
            services.AddScoped<IWriteTeamRepository, WriteTeamRepository>();
            services.AddScoped<IReadTeamRepository, ReadTeamRepository>();
            services.AddScoped<IWriteEndpointRepository, WriteEndpointRepository>();
            services.AddScoped<IReadEndpointRepository, ReadEndpointRepository>();
            services.AddScoped<IWriteTaskItemRepository, WriteTaskItemRepository>();
            services.AddScoped<IReadTaskItemRepository, ReadTaskItemRepository>();
            services.AddScoped<IWriteTaskItemRepository, WriteTaskItemRepository>();
            services.AddScoped<IWriteTeamInvitationRepository, WriteTeamInvitationRepository>();
            services.AddScoped<IReadTeamInvitationRepository, ReadTeamInvitationRepository>();
            services.AddScoped<IWriteTeamMemberRepository, WriteTeamMemberRepository>();
            services.AddScoped<IReadTeamMemberRepository, ReadTeamMemberRepository>();
            services.AddScoped<INotificationReadRepository, NotificationReadRepository>();
            services.AddScoped<INotificationWriteRepository, NotificationWriteRepository>();


            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}