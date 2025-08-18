using System.Text.Json.Serialization;

namespace GymManagement.Contracts.Subscriptions;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SubscriptionType
{
    Free = 10,
    Starter = 20,
    Pro = 30
}