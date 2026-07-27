using MediatR;

namespace TaskMnagementBackend.Aplication.Features.Notifications.Queries.GetNotificationCount
{
    public class GetNotificationCountRequest
        : IRequest<GetNotificationCountResponse>
    {
        public Guid UserId { get; set; }

        public bool OnlyUnread { get; set; } = true;
    }
}