namespace Blog.API.Entities.Database;

public class Community
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; } = DateTime.UtcNow;
    public string Name { get; set; }
    public string Description { get; set; }
    public Boolean IsClosed { get; set; }
    public int SubscribersCount { get; set; }
    public ICollection<User> Subscribers { get; set; }
}