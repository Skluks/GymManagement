using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Common.Persistence;

public class GymDbContext : DbContext, IUnitOfWork
{
    public GymDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Subscription> Subscriptions { get; set; }

    public async Task CommitChangesAsync()
    {
        await SaveChangesAsync();
    }
}