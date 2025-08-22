using Bogus;
using GymManagement.Domain.Gyms;

namespace GymManagement.TestCommon.EntityGenerators;

public static class GymGenerator
{
    public static Gym Generate(Guid? subscriptionId = null)
    {
        return new Gym(
            new Faker().Name.FirstName(),
            1,
            subscriptionId ?? Guid.NewGuid()
        );
    }
}
