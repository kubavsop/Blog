using Blog.API.Common.Enums;

namespace Blog.API.Entities.Database;

public class UserCommunity
{
    public Guid UserId { get; set; }
    public Guid CommunityId { get; set; }
    public Community Community { get; set; }
    public User User { get; set; }
    public CommunityRole Role { get; set; }
}