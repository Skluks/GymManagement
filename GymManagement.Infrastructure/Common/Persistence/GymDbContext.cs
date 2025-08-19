using System.Reflection;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Admins;
using GymManagement.Domain.Common;
using GymManagement.Domain.Gyms;
using GymManagement.Domain.Subscriptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Common.Persistence;

public class GymDbContext : DbContext, IUnitOfWork
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GymDbContext(DbContextOptions<GymDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<Admin> Admins { get; set; } = null!;
    public DbSet<Subscription> Subscriptions { get; set; } = null!;
    public DbSet<Gym> Gyms { get; set; } = null!;

    public async Task CommitChangesAsync()
    {
        var domainEvents = ChangeTracker.Entries<Entity>()
            .Select(entry => entry.Entity.PopDomainEvents())
            .SelectMany(x => x)
            .ToList();

        AddDomainEventsToOffliteProcessingQueue(domainEvents);

        await SaveChangesAsync();
    }

    private void AddDomainEventsToOffliteProcessingQueue(List<IDomainEvent> domainEvents)
    {
        Queue<IDomainEvent> domainEventsQueue = _httpContextAccessor.HttpContext.Items
            .TryGetValue("DomainEventsQueue", out object? events) && events is Queue<IDomainEvent> existingDomainEvents
            ? existingDomainEvents
            : new Queue<IDomainEvent>();

        domainEvents.ForEach(domainEvent => domainEventsQueue.Enqueue(domainEvent));

        _httpContextAccessor.HttpContext.Items["DomainEventsQueue"] = domainEventsQueue;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
