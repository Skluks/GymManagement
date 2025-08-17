using GymManagement.Domain.Subscriptions;

namespace GymManagement.Application.Common.Interfaces;

public interface ISubscriptionRepository
{
    public Task AddSubscriptionAsync(Subscription subscription);
    Task<Subscription?> GetByIdAsync(Guid id);
}