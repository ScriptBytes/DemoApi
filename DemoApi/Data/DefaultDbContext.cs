using DemoApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Data;

public class DefaultDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DefaultDbContext(DbContextOptions options) : base(options)
    {
        
    }
}