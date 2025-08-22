using Bogus;
using GymManagement.Domain.Subscriptions;

namespace GymManagement.TestCommon.EntityGenerators;

public static class SubscriptionGenerator
{
    public static Subscription Generate(SubscriptionType? subscriptionType = null)
    {
        return new Subscription(
            subscriptionType ?? SubscriptionType.Free,
            Guid.NewGuid()
        );
    }
}
