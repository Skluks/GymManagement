using GymManagement.Application.Common.Interfaces;
using GymManagement.Infrastructure.Common.Persistence;
using GymManagement.Infrastructure.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<GymDbContext>((provider, options) =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            string? connectionString = configuration.GetConnectionString("GymManagement");
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<GymDbContext>());
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();

        return services;
    }
}