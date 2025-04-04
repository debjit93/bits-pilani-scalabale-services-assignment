using Models; 
public class NotificationSchedulerService : BackgroundService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly INotificationRepository _notificationRepository;
    private readonly string _taskServiceUrl;
    private readonly string _userServiceUrl;

    public NotificationSchedulerService(IHttpClientFactory httpClientFactory, INotificationRepository notificationRepository, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _notificationRepository = notificationRepository;
        _taskServiceUrl = configuration["TaskServiceUrl"] ?? throw new ArgumentNullException("TaskServiceUrl is not configured.");
        _userServiceUrl = configuration["UserServiceUrl"] ?? throw new ArgumentNullException("UserServiceUrl is not configured.");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Fetch tasks from task service
                var client = _httpClientFactory.CreateClient();
                var tasks = await client.GetFromJsonAsync<List<TaskItem>>($"{_taskServiceUrl}/api/tasks", stoppingToken);

                if (tasks != null)
                {
                    var incompleteTasks = tasks.Where(t => !t.IsComplete).ToList();

                    foreach (var task in incompleteTasks)
                    {
                        var notification = new Notification(task.Id, $"Task '{task.Name}' is incomplete.");
                        _notificationRepository.AddNotification(notification);
                    }
                }
            }
            catch
            {
                // Handle errors silently or add error handling logic if needed
            }

            // Process notifications
            foreach (var notification in _notificationRepository.GetAllNotifications().ToList())
            {
                try
                {
                    // Fetch user info from user-service
                    var client = _httpClientFactory.CreateClient();
                    var user = await client.GetFromJsonAsync<User>($"{_userServiceUrl}/api/users/" + notification.Id, stoppingToken);

                    if (user != null)
                    {
                        _notificationRepository.RemoveNotification(notification);
                    }
                }
                catch
                {
                    // Handle errors silently or add error handling logic if needed
                }
            }

            // Wait for 24 hours before fetching tasks again
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
