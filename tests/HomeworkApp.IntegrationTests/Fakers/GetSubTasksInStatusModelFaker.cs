using AutoBogus;
using Bogus;
using HomeworkApp.Dal.Models;
using HomeworkApp.IntegrationTests.Creators;

namespace HomeworkApp.IntegrationTests.Fakers;

public static class GetSubTasksInStatusModelFaker
{
    private static readonly Faker<GetSubTasksInStatusModel> Faker = new AutoFaker<GetSubTasksInStatusModel>()
        .RuleFor(x => x.Statuses, f => f.Random.Digits(3, 1, 5))
        .RuleForType(typeof(long), f => f.Random.Long(0L));

    public static GetSubTasksInStatusModel[] Generate(int count = 1)
    {
        return Faker.Generate(count).ToArray();
    }

    public static GetSubTasksInStatusModel WithParentTaskId(
        this GetSubTasksInStatusModel src, 
        long parentTaskId)
        => src with { ParentTaskId = parentTaskId };
}