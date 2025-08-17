using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain;
using GymManagement.Domain.Subscriptions;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Common.Persistence;

public class GymDbContext : DbContext, IUnitOfWork
{
    public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
    {
    }

    public DbSet<Subscription> Subscriptions { get; set; }

    public async Task CommitChangesAsync()
    {
        await SaveChangesAsync();
    }
}