using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Admins.Events;
using GymManagement.Domain.Subscriptions;
using MediatR;

namespace GymManagement.Application.Subscriptions.Events;

public class SubscriptionDeletedEventHandler : INotificationHandler<SubscriptionDeletedEvent>
{
    private readonly ISubscriptionsRepository _subscriptionsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SubscriptionDeletedEventHandler(ISubscriptionsRepository subscriptionsRepository, IUnitOfWork unitOfWork)
    {
        _subscriptionsRepository = subscriptionsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(SubscriptionDeletedEvent notification, CancellationToken cancellationToken)
    {
        Subscription? subscription = await _subscriptionsRepository.GetByIdAsync(notification.SubscriptionId);
        if (subscription is null)
        {
            throw new NullReferenceException($"Subscription not found");
        }

        await _subscriptionsRepository.RemoveSubscriptionAsync(subscription);

        await _unitOfWork.CommitChangesAsync();
    }
}
