using Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register NotificationSchedulerService
builder.Services.AddSingleton<INotificationRepository, NotificationRepository>(); 
builder.Services.AddHttpClient(); 
builder.Services.AddHostedService<NotificationSchedulerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add GET /notifications endpoint
app.MapGet("/notifications", (INotificationRepository notifications) =>
{
    return Results.Ok(notifications.GetAllNotifications());
})
.WithName("GetNotifications");

app.Run();