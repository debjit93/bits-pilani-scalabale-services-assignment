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

app.MapPost("/users/register", (Models.User user) =>
{
    // Logic to register a new user
    return Results.Ok(new { Message = "User registered successfully", User = user });
})
.WithName("RegisterUser");

app.MapPost("/users/login", (Models.LoginRequest loginRequest) =>
{
    // Logic to authenticate a user
    if (loginRequest.Username == "test" && loginRequest.Password == "password") 
    {
        return Results.Ok(new { Message = "Login successful", Token = "example-token" });
    }
    return Results.Unauthorized();
})
.WithName("LoginUser");

app.MapGet("/users/{id}", (int id) =>
{
    // Logic to fetch user profile details
    var user = new Models.User(id,"testuser", "testuser@example.com"); 
    return Results.Ok(user);
})
.WithName("GetUserProfile");

app.Run();