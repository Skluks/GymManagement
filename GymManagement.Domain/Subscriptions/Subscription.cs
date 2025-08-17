using ErrorOr;
using GymManagement.Domain.Gyms;
using Throw;

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

    public ErrorOr<Success> AddGym(Gym gym)
    {
        _gymIds.Throw().IfContains(gym.Id);

        if (_gymIds.Count >= _maxGyms) return SubscriptionErrors.CannotHaveMoreGymsThanSubscriptionAllows;

        _gymIds.Add(gym.Id);

        return Result.Success;
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

    public int GetMaxRooms()
    {
        return SubscriptionType switch
        {
            SubscriptionType.Free => 1,
            SubscriptionType.Starter => 3,
            SubscriptionType.Pro => int.MaxValue,
            _ => throw new InvalidOperationException()
        };
    }

    public int GetMaxDailySessions()
    {
        return SubscriptionType switch
        {
            SubscriptionType.Free => 4,
            SubscriptionType.Starter => int.MaxValue,
            SubscriptionType.Pro => int.MaxValue,
            _ => throw new InvalidOperationException()
        };
    }

    public bool HasGym(Guid gymId)
    {
        return _gymIds.Contains(gymId);
    }

    public void RemoveGym(Guid gymId)
    {
        _gymIds.Throw().IfNotContains(gymId);

        _gymIds.Remove(gymId);
    }

    private Subscription()
    {
    }
}