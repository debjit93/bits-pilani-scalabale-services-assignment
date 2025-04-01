using System.Collections.Concurrent;
using System.Net.Http.Json;
using Models; // Import the models namespace

public class NotificationSchedulerService : BackgroundService
{
    private readonly ILogger<NotificationSchedulerService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly List<Notification> _notifications = new();

    public NotificationSchedulerService(ILogger<NotificationSchedulerService> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Fetch tasks from task service
                var client = _httpClientFactory.CreateClient();
                var tasks = await client.GetFromJsonAsync<List<TaskItem>>("https://task-service-url/api/tasks", stoppingToken);

                if (tasks != null)
                {
                    var incompleteTasks = tasks.Where(t => !t.IsComplete).ToList();

                    foreach (var task in incompleteTasks)
                    {
                        var notification = new Notification(task.Id, $"Task '{task.Name}' is incomplete.");
                        _notifications.Add(notification);
                        _logger.LogInformation($"Scheduled notification for task: {task.Name}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching tasks: {ex.Message}");
            }

            // Process notifications
            foreach (var notification in _notifications.ToList())
            {
                try
                {
                    // Fetch user info from user-service
                    var client = _httpClientFactory.CreateClient();
                    var user = await client.GetFromJsonAsync<User>("https://user-service-url/api/users/" + notification.Id, stoppingToken);

                    if (user != null)
                    {
                        _logger.LogInformation($"Processing notification for user {user.Name}: {notification.Message}");
                        _notifications.Remove(notification);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error fetching user info for notification {notification.Id}: {ex.Message}");
                }
            }

            // Wait for 24 hours before fetching tasks again
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
