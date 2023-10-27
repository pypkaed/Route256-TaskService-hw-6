using AutoBogus;
using Bogus;
using HomeworkApp.Dal.Models;
using HomeworkApp.IntegrationTests.Creators;

namespace HomeworkApp.IntegrationTests.Fakers;

public static class RecentTaskCommentModelFaker
{
    private static readonly object Lock = new();

    private static readonly Faker<RecentTaskCommentModel> Faker = new AutoFaker<RecentTaskCommentModel>()
        .RuleFor(x => x.Id, _ => Create.RandomId())
        .RuleFor(x => x.TaskId, f => f.Random.Long())
        .RuleFor(x => x.Message, f => f.Random.Words())
        .RuleFor(x => x.AuthorUserId, f => f.Random.Long())
        .RuleFor(x => x.At, f => f.Date.RecentOffset().UtcDateTime)
        .RuleForType(typeof(long), f => f.Random.Long(0L));

    public static RecentTaskCommentModel[] Generate(int count = 1)
    {
        lock (Lock)
        {
            return Faker.Generate(count).ToArray();
        }
    }

    public static RecentTaskCommentModel WithTaskId(
        this RecentTaskCommentModel src, 
        long taskId)
        => src with { TaskId = taskId };

}