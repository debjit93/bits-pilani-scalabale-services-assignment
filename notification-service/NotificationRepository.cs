using Models;

public class NotificationRepository : INotificationRepository
{
    private readonly List<Notification> _notifications = new();

    public void AddNotification(Notification notification)
    {
        _notifications.Add(notification);
    }

    public void RemoveNotification(Notification notification)
    {
        _notifications.Remove(notification);
    }

    public List<Notification> GetAllNotifications()
    {
        return _notifications;
    }
}
