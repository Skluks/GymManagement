using AwesomeAssertions;
using ErrorOr;
using GymManagement.Domain.Subscriptions;
using GymManagement.TestCommon.EntityGenerators;

namespace GymManagement.Domain.UnitTests.Subscriptions;

public class SubscriptionTests
{
    [Fact]
    public void AddGym_WhenMoreThanSubscriptionAllows_ShouldFail()
    {
        // Arrange

        Subscription subscription = SubscriptionGenerator.Generate(SubscriptionType.Free);
        int allowedGyms = subscription.GetMaxGyms();
        var extraGyms = Enumerable.Range(0, allowedGyms + 1).Select(_ => GymGenerator.Generate(subscription.Id)).ToList();

        // Act

        List<ErrorOr<Success>> results = extraGyms.ConvertAll(subscription.AddGym);

        // Assert

        ErrorOr<Success> errorResult = results.Last();

        errorResult.IsError.Should().BeTrue();
        errorResult.Errors.Should().HaveCount(1);
        
        Error expectedError = SubscriptionErrors.CannotHaveMoreGymsThanSubscriptionAllows;
        errorResult.Errors.Single().Should().BeEquivalentTo(expectedError);

        var successResults = results.SkipLast(1).ToList();
        successResults.Should().HaveCount(allowedGyms);
        successResults.Should().AllSatisfy(x => x.IsError.Should().BeFalse());
    }
}
