using Blog.API.Entities;
using Blog.API.Entities.Database;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    public DbSet<User> Users { get; set; }
    public DbSet<InvalidTokens> Tokens { get; set; }
    
    public DbSet<Post> Posts { get; set; }
    
    public DbSet<Tag> Tags { get; set; }
    
    public DbSet<Author> Authors { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<User>()
            .HasIndex(user => user.Email)
            .IsUnique();

        builder.Entity<Author>()
            .HasKey(a => a.UserId);
    }
}