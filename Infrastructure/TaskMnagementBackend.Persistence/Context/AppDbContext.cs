using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartTask.Domain.Entities;
using TaskMnagementBackend.Domain.Entities;
using TaskMnagementBackend.Domain.Entities.Identity;
using TaskMnagementBackend.Domain.Entities.Task;

namespace TaskMnagementBackend.Persistence.Context
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Endpoint> Endpoints { get; set; }
        public DbSet<ProjectTask> ProjectTasks => Set<ProjectTask>();
        public DbSet<SubTask> SubTasks => Set<SubTask>();
        public DbSet<TaskStatusHistory> TaskStatusHistories => Set<TaskStatusHistory>();
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<TaskAssignment> TaskAssignments => Set<TaskAssignment>();
        public DbSet<Company> Companies { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<TeamInvitation> TeamInvitations { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        public DbSet<CommentReaction> CommentReactions { get; set; }
        public DbSet<TaskActivityLog> TaskActivityLogs { get; set; }
        public DbSet<TaskAttachment> TaskAttachments { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TaskComment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CommentReaction>()
                .HasOne(r => r.TaskComment)
                .WithMany(c => c.Reactions)
                .HasForeignKey(r => r.TaskCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CommentReaction>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TaskActivityLog>()
                .HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<ProjectTask>()
                .HasMany(t => t.SubTasks)
                .WithOne(s => s.Task)
                .HasForeignKey(s => s.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<ProjectTask>()
                .HasMany(t => t.StatusHistories)
                .WithOne(h => h.Task)
                .HasForeignKey(h => h.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<TaskAssignment>()
                .HasOne(a => a.Task)
                .WithMany(t => t.Assignments)
                .HasForeignKey(a => a.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProjectTask>().Property(t => t.Priority).HasConversion<string>();
            builder.Entity<ProjectTask>().Property(t => t.Status).HasConversion<string>();
            builder.Entity<ProjectTask>().Property(t => t.Type).HasConversion<string>();
            builder.Entity<TaskStatusHistory>().Property(h => h.OldStatus).HasConversion<string>();
            builder.Entity<TaskStatusHistory>().Property(h => h.NewStatus).HasConversion<string>();
            builder.Entity<ProjectTask>().Property(t => t.Visibility).HasConversion<string>();

            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}