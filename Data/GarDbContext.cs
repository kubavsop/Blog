using Blog.API.Entities.Database;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Data;

public class GarDbContext: DbContext
{
    public GarDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AddressElement> AddressElements;
}