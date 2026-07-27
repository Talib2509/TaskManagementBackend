using TaskMnagementBackend.Aplication.Common.Pagination;
using TaskMnagementBackend.Aplication.DTOs.Notification;

namespace TaskMnagementBackend.Aplication.Features.Notifications.Queries.GetMyNotifications
{
    public class GetMyNotificationsResponse
        : PagedResult<NotificationDto>
    {
    }
}