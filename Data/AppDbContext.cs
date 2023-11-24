using Blog.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    public DbSet<User> Users { get; set; }
    public DbSet<InvalidTokens> Tokens { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<User>()
            .HasIndex(user => user.Email)
            .IsUnique();
    }
}