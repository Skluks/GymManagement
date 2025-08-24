using System.Reflection;
using ErrorOr;
using GymManagement.Application.Authorization;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Application.Common.Models;
using MediatR;

namespace GymManagement.Application.Common.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse>(ICurrentUserProvider currentUserProvider)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr

{
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authorizationAttributes = request.GetType()
            .GetCustomAttributes<AuthorizeAttribute>()
            .ToList();

        if (authorizationAttributes.Count == 0)
        {
            return next();
        }

        CurrentUser currentUser = currentUserProvider.GetCurrentUser();

        var neededPermissions = authorizationAttributes
            .SelectMany(x => x.Permissions?.Split(",") ?? [])
            .ToList();

        if (neededPermissions.Except(currentUser.Permissions).Any())
        {
            return (dynamic)Error.Unauthorized(description: "User does not have needed permissions");
        }

        return next();
    }
}
