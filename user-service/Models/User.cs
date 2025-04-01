namespace Models;

public record User(Guid Id, string Username, string Email, string Password)
{
    public User(string Username, string Email, string Password) 
        : this(Guid.NewGuid(), Username, Email, Password) { }
}
