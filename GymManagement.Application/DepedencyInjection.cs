using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using GymManagement.Application.Common.Behaviors;

namespace GymManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));
            options.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
        return services;
    }
}
