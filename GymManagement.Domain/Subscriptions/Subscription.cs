namespace GymManagement.Domain.Subscriptions;

public class Subscription
{
    private readonly List<Guid> _gymIds = new();
    private readonly int _maxGyms;

    public Guid Id { get; private set; }
    public SubscriptionType SubscriptionType { get; private set; }
    public Guid AdminId { get; private set; }

    public Subscription(
        SubscriptionType subscriptionType,
        Guid adminId,
        Guid? id = null)
    {
        SubscriptionType = subscriptionType;
        AdminId = adminId;
        Id = id ?? Guid.NewGuid();

        _maxGyms = GetMaxGyms();
    }

    public int GetMaxGyms()
    {
        return SubscriptionType switch
        {
            SubscriptionType.Free => 1,
            SubscriptionType.Starter => 1,
            SubscriptionType.Pro => 3,
            _ => throw new InvalidOperationException()
        };
    }
    
    private Subscription()
    {
    }
}