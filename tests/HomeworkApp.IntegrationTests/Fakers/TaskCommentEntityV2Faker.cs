using AutoBogus;
using Bogus;
using HomeworkApp.Dal.Entities;
using HomeworkApp.IntegrationTests.Creators;

namespace HomeworkApp.IntegrationTests.Fakers;

public static class TaskCommentEntityV2Faker
{
    private static readonly object Lock = new();

    private static readonly Faker<TaskCommentEntityV2> Faker = new AutoFaker<TaskCommentEntityV2>()
        .RuleFor(x => x.Id, _ => Create.RandomId())
        .RuleFor(x => x.AuthorUserId, f => f.Random.Long())
        .RuleFor(x => x.Message, f => f.Random.Words())
        .RuleFor(x => x.At, f => f.Date.RecentOffset().UtcDateTime)
        .RuleFor(x => x.ModifiedAt, _ => null)
        .RuleFor(x => x.DeletedAt, _ => null)
        .RuleForType(typeof(long), f => f.Random.Long(0L));

    public static TaskCommentEntityV2[] Generate(int count = 1)
    {
        lock (Lock)
        {
            return Faker.Generate(count).ToArray();
        }
    }

    public static TaskCommentEntityV2 WithId(
        this TaskCommentEntityV2 src, 
        long id)
        => src with { Id = id };
    
    public static TaskCommentEntityV2 WithMessage(
        this TaskCommentEntityV2 src, 
        string message)
        => src with { Message = message };
    
    public static TaskCommentEntityV2 WithModifiedAt(
        this TaskCommentEntityV2 src, 
        DateTimeOffset modifiedAt)
        => src with { ModifiedAt = modifiedAt };
    
    public static TaskCommentEntityV2 WithDeletedAt(
        this TaskCommentEntityV2 src, 
        DateTimeOffset deletedAt)
        => src with { DeletedAt = deletedAt };
}