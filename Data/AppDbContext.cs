using Blog.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    public DbSet<User> Users { get; set; }
}