using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.DTOs.Notification;

namespace TaskMnagementBackend.Aplication.Features.Notifications.Queries.GetUnreadNotifications
{
    public class GetUnreadNotificationsResponse : OperationResultBase
    {
        public IReadOnlyList<NotificationDto> Notifications { get; set; }
            = new List<NotificationDto>();
    }
}