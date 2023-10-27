using FluentAssertions;
using HomeworkApp.Dal.Repositories.Interfaces;
using HomeworkApp.IntegrationTests.Creators;
using HomeworkApp.IntegrationTests.Fakers;
using HomeworkApp.IntegrationTests.Fixtures;
using Xunit;

namespace HomeworkApp.IntegrationTests.RepositoryTests;

[Collection(nameof(TestFixture))]
public class RecentTaskCommentRepositoryTests
{
    private readonly IRecentTaskCommentRepository _repository;

    public RecentTaskCommentRepositoryTests(TestFixture fixture)
    {
        _repository = fixture.RecentTaskCommentRepository;
    }

    [Fact]
    public async Task Add_RecentTaskComment_Success()
    {
        // Arrange
        var recentTaskComment = RecentTaskCommentModelFaker.Generate()
            .Single();

        // Act
        await _repository.Add(recentTaskComment, default);
        var expiresAt = await _repository.GetExpireTimeIfExists(recentTaskComment.TaskId, default);

        // Assert
        expiresAt.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Get_RecentTaskComment_Success()
    {
        // Arrange
        var recentTaskCommentId = Create.RandomId();
        
        var recentTaskComment = RecentTaskCommentModelFaker.Generate()
            .First()
            .WithTaskId(recentTaskCommentId);
        await _repository.Add(recentTaskComment, default);
        
        // Act
        var results = await _repository.Get(recentTaskCommentId, default);
        var result = results.Single();

        // Asserts
        result.Should().NotBeNull();
        result.TaskId.Should().Be(recentTaskCommentId);
    }

    [Fact]
    public async Task Delete_RecentTaskComment_Success()
    {
        // Arrange
        var takenTask = RecentTaskCommentModelFaker.Generate()
            .First();
        
        // Act
        await _repository.Delete(takenTask.TaskId, default);
    }
}