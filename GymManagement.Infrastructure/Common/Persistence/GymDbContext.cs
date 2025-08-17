using GymManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Common.Persistence;

public class GymDbContext : DbContext
{
    public DbSet<Subscription> Subscriptions { get; set; }
    
    public GymDbContext(DbContextOptions options) : base(options)
    {
    }

    public async Task CommitChangesAsync()
    {
        await SaveChangesAsync();
    }
}