using AutoBogus;
using Bogus;
using HomeworkApp.Dal.Models;
using HomeworkApp.IntegrationTests.Creators;

namespace HomeworkApp.IntegrationTests.Fakers;

public static class UserRateLimitModelFaker
{
    private static readonly object Lock = new();

    private static readonly Faker<UserRateLimitModel> Faker = new AutoFaker<UserRateLimitModel>()
        .RuleFor(x => x.UserIp, f => new UserIp(
            $"{f.Random.Byte()}.{f.Random.Byte()}.{f.Random.Byte()}.{f.Random.Byte()}"))
        .RuleFor(x => x.CurrentLimit, _ => 100)
        .RuleForType(typeof(long), f => f.Random.Long(0L));

    public static UserRateLimitModel[] Generate(int count = 1)
    {
        lock (Lock)
        {
            return Faker.Generate(count).ToArray();
        }
    }

    public static UserRateLimitModel WithUserIp(
        this UserRateLimitModel src, 
        UserIp userIp)
        => src with { UserIp = userIp };
}