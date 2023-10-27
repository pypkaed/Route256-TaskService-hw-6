using AutoBogus;
using Bogus;
using HomeworkApp.Bll.Models;
using UnitTests.Creators;
using TaskStatus = HomeworkApp.Bll.Enums.TaskStatus;

namespace UnitTests.Fakers;

public static class GetTaskModelFaker
{
    private static readonly object Lock = new();

    private static readonly Faker<GetTaskModel> Faker = new AutoFaker<GetTaskModel>()
        .RuleFor(x => x.TaskId, _ => Create.RandomId())
        .RuleFor(x => x.Status, f => TaskStatus.InProgress)
        .RuleFor(x => x.CreatedAt, f => f.Date.RecentOffset().UtcDateTime)
        .RuleFor(x => x.CompletedAt, f => f.Date.RecentOffset().UtcDateTime)
        .RuleForType(typeof(long), f => f.Random.Long(0L));

    public static GetTaskModel[] Generate(int count = 1)
    {
        lock (Lock)
        {
            return Faker.Generate(count).ToArray();
        }
    }
}