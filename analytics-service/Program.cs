var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/analytics/tasks-completed", () =>
{
    // Replace with actual logic to fetch tasks completed statistics
    var tasksCompletedStats = new
    {
        TotalTasks = 150,
        CompletedTasks = 120,
        PendingTasks = 30
    };
    return tasksCompletedStats;
})
.WithName("GetTasksCompletedStats");

app.MapGet("/analytics/user-activity", () =>
{
    // Replace with actual logic to fetch user activity data
    var userActivityData = new[]
    {
        new { UserId = 1, LastActive = DateTime.Now.AddMinutes(-15) },
        new { UserId = 2, LastActive = DateTime.Now.AddHours(-1) },
        new { UserId = 3, LastActive = DateTime.Now.AddDays(-1) }
    };
    return userActivityData;
})
.WithName("GetUserActivityData");

// POST endpoints to track events
app.MapPost("/analytics/user-logins", (HttpContext context) =>
{
    // Replace with actual logic to track user login events
    return Results.Ok(new { Message = "User login event tracked successfully" });
})
.WithName("TrackUserLogins");

app.MapPost("/analytics/user-registrations", (HttpContext context) =>
{
    // Replace with actual logic to track user registration events
    return Results.Ok(new { Message = "User registration event tracked successfully" });
})
.WithName("TrackUserRegistrations");

app.MapPost("/analytics/task-creations", (HttpContext context) =>
{
    // Replace with actual logic to track task creation events
    return Results.Ok(new { Message = "Task creation event tracked successfully" });
})
.WithName("TrackTaskCreations");

app.MapPost("/analytics/task-completions", (HttpContext context) =>
{
    // Replace with actual logic to track task completion events
    return Results.Ok(new { Message = "Task completion event tracked successfully" });
})
.WithName("TrackTaskCompletions");

app.MapPost("/analytics/notifications-sent", (HttpContext context) =>
{
    // Replace with actual logic to track notification sent events
    return Results.Ok(new { Message = "Notification sent event tracked successfully" });
})
.WithName("TrackNotificationsSent");

// GET endpoints to retrieve aggregated data
app.MapGet("/analytics/user-activity", () =>
{
    // Replace with actual logic to fetch aggregated user activity data
    var userActivityData = new[]
    {
        new { UserId = 1, Logins = 10, Registrations = 1 },
        new { UserId = 2, Logins = 5, Registrations = 0 }
    };
    return userActivityData;
})
.WithName("GetAggregatedUserActivity");

app.MapGet("/analytics/task-activity", () =>
{
    // Replace with actual logic to fetch aggregated task activity data
    var taskActivityData = new
    {
        TotalTasksCreated = 200,
        TotalTasksCompleted = 150
    };
    return taskActivityData;
})
.WithName("GetAggregatedTaskActivity");

app.MapGet("/analytics/notification-activity", () =>
{
    // Replace with actual logic to fetch aggregated notification activity data
    var notificationActivityData = new
    {
        TotalNotificationsSent = 500
    };
    return notificationActivityData;
})
.WithName("GetAggregatedNotificationActivity");

app.Run();
