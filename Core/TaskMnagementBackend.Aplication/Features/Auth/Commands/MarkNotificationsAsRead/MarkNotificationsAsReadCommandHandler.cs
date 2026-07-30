using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;
using TaskMnagementBackend.Domain.Entities;

namespace TaskMnagementBackend.Aplication.Features.Commands.MarkNotificationsAsRead
{
    public class MarkNotificationsAsReadCommandHandler : IRequestHandler<MarkNotificationsAsReadCommand, bool>
    {
        private readonly IReadRepository<Notification> _notificationReadRepository;
        private readonly IWriteRepository<Notification> _notificationWriteRepository;
        private readonly TaskMnagementBackend.Aplication.IUnitOfWork.IUnitOfWork _unitOfWork;

        public MarkNotificationsAsReadCommandHandler(
            IReadRepository<Notification> notificationReadRepository,
            IWriteRepository<Notification> notificationWriteRepository,
            TaskMnagementBackend.Aplication.IUnitOfWork.IUnitOfWork unitOfWork)
        {
            _notificationReadRepository = notificationReadRepository;
            _notificationWriteRepository = notificationWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(MarkNotificationsAsReadCommand request, CancellationToken cancellationToken)
        {
            var unreadNotifications = await _notificationReadRepository
                .GetWhere(n => n.UserId == request.UserId && !n.IsRead)
                .ToListAsync(cancellationToken);

            if (!unreadNotifications.Any()) return true;

            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
                _notificationWriteRepository.Update(notification);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}