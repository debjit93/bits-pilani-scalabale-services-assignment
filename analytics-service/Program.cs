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

app.Run();
