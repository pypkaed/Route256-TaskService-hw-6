using FluentAssertions;
using HomeworkApp.Dal.Models;
using HomeworkApp.Dal.Repositories.Interfaces;
using HomeworkApp.IntegrationTests.Fakers;
using HomeworkApp.IntegrationTests.Fixtures;
using Xunit;

namespace HomeworkApp.IntegrationTests.RepositoryTests;

[Collection(nameof(TestFixture))]
public class TaskCommentRepositoryTests
{
    private readonly ITaskCommentRepository _repository;

    public TaskCommentRepositoryTests(TestFixture fixture)
    {
        _repository = fixture.TaskCommentRepository;
    }

    [Fact]
    public async Task Add_TaskComment_Success()
    {
        // Arrange
        var taskComment = TaskCommentEntityV2Faker.Generate()
            .Single();

        // Act
        var taskCommentId = await _repository.Add(taskComment, default);
        var result = await _repository.Get(new TaskCommentGetModel()
        {
            TaskId = taskComment.TaskId,
            IncludeDeleted = false
        }, default);

        // Assert
        taskCommentId.Should().BeGreaterThan(0);
        result.Should().Contain(tc => tc.Id == taskCommentId);
        result.Should().Contain(tc => tc.Message == taskComment.Message);
    }

    [Fact]
    public async Task Update_Success()
    {
        // Arrange
        var taskComment = TaskCommentEntityV2Faker.Generate()
            .Single();
        var taskCommentId = await _repository.Add(taskComment, default);
        var taskId = taskComment.TaskId;

        var newMessage = "Some new message";
        taskComment = taskComment
            .WithId(taskCommentId)
            .WithMessage(newMessage);
        
        // Act
        await _repository.Update(taskComment, default);
        
        var result = await _repository.Get(new TaskCommentGetModel()
        {
            TaskId = taskId,
            IncludeDeleted = true
        },
            default);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().AllSatisfy(r => r.ModifiedAt.Should()
            .BeCloseTo(DateTimeOffset.Now.UtcDateTime, TimeSpan.FromMilliseconds(100)));
        result.Should().AllSatisfy(r => r.TaskId.Should().Be(taskId));
        result.Should().AllSatisfy(r => r.Message.Should().Be(newMessage));
    }
    
    [Fact]
    public async Task SetDeleted_Success()
    {
        // Arrange
        var taskComment = TaskCommentEntityV2Faker.Generate()
            .Single();
        var taskCommentId = await _repository.Add(taskComment, default);
        var taskId = taskComment.TaskId;
        
        // Act
        await _repository.SetDeleted(taskCommentId, default);

        // Assert
        var result = await _repository.Get(new TaskCommentGetModel()
            {
                TaskId = taskId,
                IncludeDeleted = true
            },
            default);
        result.Should().NotBeNull();
        result.Should().AllSatisfy(r => r.DeletedAt.Should()
            .BeCloseTo(DateTimeOffset.Now.UtcDateTime, TimeSpan.FromMilliseconds(100)));
        result.Should().AllSatisfy(r => r.TaskId.Should().Be(taskId));
    }
    
    [Fact]
    public async Task GetTaskComments_DoNotIncludeDeleted_Success()
    {
        // Arrange
        var taskComment = TaskCommentEntityV2Faker.Generate()
            .Single();
        var taskCommentId = await _repository.Add(taskComment, default);
        
        // Act
        var result = await _repository.Get(new TaskCommentGetModel()
            {
                TaskId = taskComment.TaskId,
                IncludeDeleted = false
            },
            default);

        // Assert
        result.Should().NotBeNull();
        result.Should().AllSatisfy(r => r.TaskId.Should().Be(taskComment.TaskId));
        result.Should().AllSatisfy(r => r.DeletedAt.Should().BeNull());
    }
    
    [Fact]
    public async Task GetTaskComments_IncludeDeleted_Success()
    {
        // Arrange
        var taskComment = TaskCommentEntityV2Faker.Generate()
            .Single();
        var taskCommentId = await _repository.Add(taskComment, default);
        await _repository.SetDeleted(taskCommentId, default);
        
        // Act
        var result = await _repository.Get(new TaskCommentGetModel()
            {
                TaskId = taskComment.TaskId,
                IncludeDeleted = true
            },
            default);

        // Assert
        result.Should().NotBeNull();
        result.Should().AllSatisfy(r => r.TaskId.Should().Be(taskComment.TaskId));
        result.Should().AllSatisfy(r => r.DeletedAt.Should().NotBeNull());
    }
}