using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories.Company;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories.Notfication;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories.TaskItem;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories.Team;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories.TeamInvitation;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories.TeamMember;
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
        public IReadCompnayRepository CompanyReadRepository { get; }
        public IWriteCompanyRepository CompanyWriteRepository { get; }
        public IReadTeamRepository TeamReadRepository { get; }
        public IWriteTeamRepository TeamWriteRepository { get; }
        public IReadTeamMemberRepository TeamMemberReadRepository { get; }
        public IWriteTeamMemberRepository TeamMemberWriteRepository { get; }
        public IReadTaskItemRepository TaskItemReadRepository { get; }
        public IWriteTaskItemRepository TaskItemWriteRepository { get; }
        public IReadTeamInvitationRepository TeamInvitationReadRepository { get; }
        public IWriteTeamInvitationRepository TeamInvitationWriteRepository { get; }
        public INotificationReadRepository NotificationReadRepository { get; }
        public INotificationWriteRepository NotificationWriteRepository { get; }




        public UnitOfWork(
            AppDbContext context,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
        IReadCompnayRepository companyReadRepository,
            IWriteCompanyRepository companyWriteRepository,
            IReadTeamRepository teamReadRepository,
            IWriteTeamRepository teamWriteRepository,
            IReadTeamMemberRepository teamMemberReadRepository,
            IWriteTeamMemberRepository teamMemberWriteRepository,
            IReadTaskItemRepository taskItemReadRepository,
            IWriteTaskItemRepository taskItemWriteRepository,
            IReadTeamInvitationRepository teamInvitationReadRepository,
            IWriteTeamInvitationRepository teamInvitationWriteRepository,
            INotificationReadRepository notificationReadRepository,
            INotificationWriteRepository notificationWriteRepository)
        {
            _context = context;

            UserManager = userManager;
            RoleManager = roleManager;
            CompanyReadRepository = companyReadRepository;
            CompanyWriteRepository = companyWriteRepository;
            TeamReadRepository = teamReadRepository;
            TeamWriteRepository = teamWriteRepository;
            TeamMemberReadRepository = teamMemberReadRepository;
            TeamMemberWriteRepository = teamMemberWriteRepository;
            TaskItemReadRepository = taskItemReadRepository;
            TaskItemWriteRepository = taskItemWriteRepository;
            TeamInvitationReadRepository = teamInvitationReadRepository;
            TeamInvitationWriteRepository = teamInvitationWriteRepository;
            NotificationReadRepository = notificationReadRepository;
            NotificationWriteRepository = notificationWriteRepository;


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
