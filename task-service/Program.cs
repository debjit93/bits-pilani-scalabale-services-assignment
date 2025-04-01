using Microsoft.EntityFrameworkCore;
using TaskService.Data;
using TaskService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("TaskDatabase")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/tasks", async (TaskItem task, TaskDbContext dbContext) =>
{
    task.Id = Guid.NewGuid();
    await dbContext.TaskItems.AddAsync(task);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/tasks/{task.Id}", task);
})
.WithName("CreateTask");

app.MapGet("/tasks", async (TaskDbContext dbContext) =>
{
    var tasks = await dbContext.TaskItems.ToListAsync();
    return Results.Ok(tasks);
})
.WithName("GetTasks");

app.MapPut("/tasks/{id}", async (Guid id, TaskItem updatedTask, TaskDbContext dbContext) =>
{
    var task = await dbContext.TaskItems.FindAsync(id);
    if (task is null)
    {
        return Results.NotFound();
    }
    task.Name = updatedTask.Name;
    task.IsComplete = updatedTask.IsComplete;
    await dbContext.SaveChangesAsync();
    return Results.NoContent();
})
.WithName("UpdateTask");

app.MapDelete("/tasks/{id}", async (Guid id, TaskDbContext dbContext) =>
{
    var task = await dbContext.TaskItems.FindAsync(id);
    if (task is null)
    {
        return Results.NotFound();
    }
    dbContext.TaskItems.Remove(task);
    await dbContext.SaveChangesAsync();
    return Results.NoContent();
})
.WithName("DeleteTask");

app.Run();