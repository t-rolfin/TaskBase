using System;

namespace TaskBase.Data.NotificationService
{
    record PageNotificationModel(bool IsSuccess, string Message, Guid UserId);
}
