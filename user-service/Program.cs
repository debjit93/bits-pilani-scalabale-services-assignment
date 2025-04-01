using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var users = new List<Models.User>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

string HashPassword(string password)
{
    using var sha256 = SHA256.Create();
    return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
}

app.MapPost("/users/register", (Models.User user) =>
{
    // Hash the password before saving
    var hashedPassword = HashPassword(user.Password);
    var userWithHashedPassword = user with { Password = hashedPassword };

    users.Add(userWithHashedPassword); 
    return Results.Ok(new { Message = "User registered successfully", User = userWithHashedPassword });
})
.WithName("RegisterUser");

app.MapPost("/users/login", (Models.LoginRequest loginRequest) =>
{
    // Hash the incoming password before matching
    var hashedPassword = HashPassword(loginRequest.Password);
    var user = users.FirstOrDefault(u => u.Username == loginRequest.Username && u.Password == hashedPassword);
    if (user != null)
    {
        return Results.Ok(new { Message = "Login successful", Id = user.Id });
    }
    return Results.Unauthorized();
})
.WithName("LoginUser");

app.MapGet("/users/{id}", (Guid id) =>
{
    // Fetch user details from the users list
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user != null)
    {
        // Omit the password in the response
        var userWithoutPassword = new { user.Id, user.Username, user.Email };
        return Results.Ok(userWithoutPassword);
    }
    return Results.NotFound(new { Message = "User not found" });
})
.WithName("GetUserProfile");

app.Run();