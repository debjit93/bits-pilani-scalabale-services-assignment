using Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register NotificationSchedulerService
builder.Services.AddHttpClient(); // Add HttpClient for task service communication
builder.Services.AddHostedService<NotificationSchedulerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add POST /notifications endpoint
var notifications = new List<Notification>();

app.MapPost("/notifications", (Notification notification) =>
{
    notifications.Add(notification);
    return Results.Created($"/notifications/{notification.Id}", notification);
})
.WithName("CreateNotification");

// Add GET /notifications endpoint
app.MapGet("/notifications", () =>
{
    return Results.Ok(notifications);
})
.WithName("GetNotifications");

app.Run();