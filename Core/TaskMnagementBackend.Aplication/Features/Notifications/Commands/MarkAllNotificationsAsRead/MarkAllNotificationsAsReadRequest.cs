using MediatR;


namespace TaskMnagementBackend.Aplication.Features.Notifications.Commands.MarkAllNotificationsAsRead
{
    public class MarkAllNotificationsAsReadRequest
        : IRequest<MarkAllNotificationsAsReadResponse>
    {
        public Guid UserId { get; set; }
    }
}