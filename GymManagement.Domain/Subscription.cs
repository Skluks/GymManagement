using GymManagement.Domain.Enums;

namespace GymManagement.Domain;

public class Subscription
{
    public Guid Id { get; set; }
    public SubscriptionType SubscriptionType { get; set; }
}