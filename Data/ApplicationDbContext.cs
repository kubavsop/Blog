using Blog.API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Data;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}
    public DbSet<User> Users { get; set; }
}