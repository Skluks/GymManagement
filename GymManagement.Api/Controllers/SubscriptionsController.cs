using GymManagement.Application.Subscriptions.Commands.CreateSubscription;
using GymManagement.Application.Subscriptions.Queries.GetSubscription;
using GymManagement.Contracts.Subscriptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DomainSubscriptionType = GymManagement.Domain.Subscriptions.SubscriptionType;

namespace GymManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SubscriptionsController : ControllerBase
{
    private readonly ISender _mediator;

    public SubscriptionsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionRequest request)
    {
        if (!Enum.TryParse(request.SubscriptionType.ToString(), out DomainSubscriptionType subscriptionType))
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                detail: "Invalid subscription type");

        var command = new CreateSubscriptionCommand(subscriptionType, request.AdminId);
        var result = await _mediator.Send(command);

        return result.MatchFirst(
            subscription => CreatedAtAction(
                nameof(GetSubscription),
                new {subscriptionId = subscription.Id},
                new SubscriptionResponse(
                    subscription.Id,
                    ToDto(subscription.SubscriptionType))),
            error => Problem(error.Description));
    }

    [HttpGet("{subscriptionId:guid}")]
    public async Task<IActionResult> GetSubscription(Guid subscriptionId)
    {
        var query = new GetSubscriptionQuery(subscriptionId);

        var getSubscriptionsResult = await _mediator.Send(query);

        return getSubscriptionsResult.MatchFirst(
            subscription => Ok(new SubscriptionResponse(
                subscription.Id,
                ToDto(subscription.SubscriptionType))),
            error => Problem(error.Description));
    }

    private static SubscriptionType ToDto(DomainSubscriptionType subscriptionType)
    {
        return subscriptionType switch
        {
            DomainSubscriptionType.Free => SubscriptionType.Free,
            DomainSubscriptionType.Starter => SubscriptionType.Starter,
            DomainSubscriptionType.Pro => SubscriptionType.Pro,
            _ => throw new InvalidOperationException()
        };
    }
}