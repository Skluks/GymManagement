using GymManagement.Application.Common.Interfaces;
using GymManagement.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetService<IConfiguration>();
        
        services.AddDbContext<GymDbContext>(options => options.UseNpgsql(configuration?.GetConnectionString("GymManagement")));
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<GymDbContext>());

        return services;
    }
}