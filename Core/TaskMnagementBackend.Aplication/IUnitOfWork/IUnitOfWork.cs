using Microsoft.AspNetCore.Identity;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories.Company;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories.Notfication;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories.TaskItem;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories.Team;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories.TeamInvitation;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories.TeamMember;
using TaskMnagementBackend.Domain.Entities.Identity;

namespace TaskMnagementBackend.Aplication.IUnitOfWork
{
    public interface IUnitOfWork
    {
        UserManager<AppUser> UserManager { get; }
        RoleManager<AppRole> RoleManager { get; }

        IReadCompnayRepository CompanyReadRepository { get; }
        IWriteCompanyRepository CompanyWriteRepository { get; }
        IWriteTeamRepository TeamWriteRepository { get; }
        IReadTeamRepository TeamReadRepository { get; }
        IReadTeamMemberRepository TeamMemberReadRepository { get; }
        IWriteTeamMemberRepository TeamMemberWriteRepository { get; }
        IReadTaskItemRepository TaskItemReadRepository { get; }
        IWriteTaskItemRepository TaskItemWriteRepository { get; }
        IReadTeamInvitationRepository TeamInvitationReadRepository { get; }
        IWriteTeamInvitationRepository TeamInvitationWriteRepository { get; }
        INotificationReadRepository NotificationReadRepository { get; }
        INotificationWriteRepository NotificationWriteRepository { get; }
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}