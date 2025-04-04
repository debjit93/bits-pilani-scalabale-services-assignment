using Models;

public interface INotificationRepository
{
    void AddNotification(Notification notification);
    void RemoveNotification(Notification notification);
    List<Notification> GetAllNotifications();
}
