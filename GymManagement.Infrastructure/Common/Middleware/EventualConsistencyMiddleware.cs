using GymManagement.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace GymManagement.Infrastructure.Common.Middleware;

public class EventualConsistencyMiddleware
{
    private readonly RequestDelegate _next;

    public EventualConsistencyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IPublisher publisher)
    {
        context.Response.OnCompleted(async () =>
        {
            var domainEventsQueue = context.Items["DomainEventsQueue"] as Queue<IDomainEvent>;

            while (domainEventsQueue != null && domainEventsQueue.TryDequeue(out IDomainEvent? domainEvent))
            {
                await publisher.Publish(domainEvent);
            }
        });

        await _next(context);
    }
}
