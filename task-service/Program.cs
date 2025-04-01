using TaskService.Models;

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

var tasks = new List<TaskItem>();

app.MapPost("/tasks", (TaskItem task) =>
{
    task.Id = Guid.NewGuid();
    tasks.Add(task);
    return Results.Created($"/tasks/{task.Id}", task);
})
.WithName("CreateTask");

app.MapGet("/tasks", () =>
{
    return Results.Ok(tasks);
})
.WithName("GetTasks");

app.MapPut("/tasks/{id}", (Guid id, TaskItem updatedTask) =>
{
    var task = tasks.FirstOrDefault(t => t.Id == id);
    if (task is null)
    {
        return Results.NotFound();
    }
    task.Name = updatedTask.Name;
    task.IsComplete = updatedTask.IsComplete;
    return Results.NoContent();
})
.WithName("UpdateTask");

app.MapDelete("/tasks/{id}", (Guid id) =>
{
    var task = tasks.FirstOrDefault(t => t.Id == id);
    if (task is null)
    {
        return Results.NotFound();
    }
    tasks.Remove(task);
    return Results.NoContent();
})
.WithName("DeleteTask");

app.Run();