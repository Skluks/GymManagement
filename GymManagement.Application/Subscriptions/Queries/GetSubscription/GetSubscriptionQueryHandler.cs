using MediatR;
using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;

namespace GymManagement.Application.Subscriptions.Queries.GetSubscription;

public class GetSubscriptionQueryHandler : IRequestHandler<GetSubscriptionQuery, ErrorOr<Subscription>>
{
    private readonly ISubscriptionRepository _subscriptionRepository;

    public GetSubscriptionQueryHandler(ISubscriptionRepository subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<ErrorOr<Subscription>> Handle(GetSubscriptionQuery request, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionRepository.GetByIdAsync(request.Id);
        return subscription is null
            ? Error.NotFound(description: "Subscription not found")
            : subscription;
    }
}