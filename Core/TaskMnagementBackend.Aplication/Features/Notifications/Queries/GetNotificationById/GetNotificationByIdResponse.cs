using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.DTOs.Notification;

namespace TaskMnagementBackend.Aplication.Features.Notifications.Queries.GetNotificationById
{
    public class GetNotificationByIdResponse : OperationResultBase
    {
        public NotificationDto? Notification { get; set; }
    }
}