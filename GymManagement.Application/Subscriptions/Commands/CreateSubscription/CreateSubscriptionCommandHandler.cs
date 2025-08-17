using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, ErrorOr<Subscription>>
{
    private readonly ISubscriptionRepository _subscriptionRepository;

    public CreateSubscriptionCommandHandler(ISubscriptionRepository subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<ErrorOr<Subscription>> Handle(CreateSubscriptionCommand request,
        CancellationToken cancellationToken)
    {
        var subscription = new Subscription(request.SubscriptionType, request.AdminId);
        await _subscriptionRepository.AddSubscriptionAsync(subscription);

        return subscription;
    }
}