using MediatR;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;
using TaskMnagementBackend.Aplication.Abstraction.Services; 
using TaskMnagementBackend.Domain.Entities;

namespace TaskMnagementBackend.Aplication.Features.Commands.CreateComment
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, bool>
    {
        private readonly IWriteRepository<TaskComment> _commentWriteRepository;
        private readonly IWriteRepository<TaskActivityLog> _activityLogWriteRepository;
        private readonly IWriteRepository<Notification> _notificationWriteRepository;
        private readonly TaskMnagementBackend.Aplication.IUnitOfWork.IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService; 

        public CreateCommentCommandHandler(
            IWriteRepository<TaskComment> commentWriteRepository,
            IWriteRepository<TaskActivityLog> activityLogWriteRepository,
            IWriteRepository<Notification> notificationWriteRepository,
            TaskMnagementBackend.Aplication.IUnitOfWork.IUnitOfWork unitOfWork,
            INotificationService notificationService) 
        {
            _commentWriteRepository = commentWriteRepository;
            _activityLogWriteRepository = activityLogWriteRepository;
            _notificationWriteRepository = notificationWriteRepository;
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        public async Task<bool> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var comment = new TaskComment
                {
                    ProjectTaskId = request.TaskId,
                    Text = request.Text,
                    ParentCommentId = request.ParentCommentId,
                    UserId = request.UserId,
                    CreatedAt = DateTime.UtcNow
                };

                await _commentWriteRepository.AddAsync(comment);

                var mentions = Regex.Matches(request.Text, @"@(\w+)")
                                    .Cast<Match>()
                                    .Select(m => m.Groups[1].Value)
                                    .Distinct()
                                    .ToList();

                foreach (var username in mentions)
                {
                    var mentionedUser = await _unitOfWork.UserManager.FindByNameAsync(username);
                    if (mentionedUser != null)
                    {
                        var notification = new Notification
                        {
                            UserId = mentionedUser.Id,
                            Title = "Yeni Mention",
                            Message = $"Siz bir tapşırığın şərhində mention edildiniz.",
                            Type = "Mention"
                        };
                        await _notificationWriteRepository.AddAsync(notification);

                       
                        await _notificationService.SendNotificationToUserAsync(
                            mentionedUser.Id.ToString(),
                            notification,
                            cancellationToken);
                    }
                }

                var log = new TaskActivityLog
                {
                    ProjectTaskId = request.TaskId,
                    UserId = request.UserId,
                    ActionType = "CommentAdded",
                    Description = request.ParentCommentId == null ? "Yeni şərh əlavə etdi" : "Şərhə cavab yazdı",
                    CreatedAt = DateTime.UtcNow
                };
                await _activityLogWriteRepository.AddAsync(log);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);

                
                await _notificationService.SendCommentToTaskGroupAsync(
                    request.TaskId,
                    comment,
                    cancellationToken);

                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}