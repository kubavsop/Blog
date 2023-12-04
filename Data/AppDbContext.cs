using Blog.API.Entities.Database;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<InvalidTokens> Tokens { get; set; }

    public DbSet<Post> Posts { get; set; }

    public DbSet<Tag> Tags { get; set; }

    public DbSet<Comment> Comments { get; set; }

    public DbSet<AddressElement> AddressElements { get; set; }

    public DbSet<Community> Communities { get; set; }

    public DbSet<CommunityUser> CommunityUser { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<User>()
            .HasIndex(user => user.Email)
            .IsUnique();

        builder.Entity<Post>()
            .HasOne(p => p.Author)
            .WithMany(u => u.CreatedPosts)
            .IsRequired();

        builder.Entity<Comment>()
            .HasIndex(c => c.ParentId);

        builder.Entity<AddressElement>()
            .HasIndex(a => a.ParentObjId);

        builder.Entity<AddressElement>()
            .HasIndex(a => a.ObjectGuid)
            .IsUnique();

        builder.Entity<AddressElement>()
            .HasIndex(a => a.NormalizedText);

        builder.Entity<AddressElement>()
            .HasKey(a => a.ObjectId);

        builder.Entity<CommunityUser>()
            .HasKey(userCommunity => new { userCommunity.UserId, userCommunity.CommunityId });

        builder.Entity<Community>()
            .HasMany(c => c.Subscribers)
            .WithMany(u => u.Communities)
            .UsingEntity<CommunityUser>();

        builder.Entity<Post>()
            .HasOne(p => p.Address)
            .WithMany()
            .HasForeignKey(p => p.AddressId)
            .HasPrincipalKey(a => a.ObjectGuid);
    }
}