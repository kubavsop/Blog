namespace Blog.API.Entities.Database;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; }
    public DateTime ExpirationTime { get; set; }

    public bool IsExpired => DateTime.UtcNow > ExpirationTime;

    public Guid UserId { get; set; }
    public User User { get; set; }
}