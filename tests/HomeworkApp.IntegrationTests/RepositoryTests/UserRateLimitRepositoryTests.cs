using FluentAssertions;
using HomeworkApp.Dal.Repositories.Interfaces;
using HomeworkApp.IntegrationTests.Creators;
using HomeworkApp.IntegrationTests.Fakers;
using HomeworkApp.IntegrationTests.Fixtures;
using Xunit;

namespace HomeworkApp.IntegrationTests.RepositoryTests;

[Collection(nameof(TestFixture))]
public class UserRateLimitRepositoryTests
{
    private readonly IUserRateLimitRepository _repository;

    public UserRateLimitRepositoryTests(TestFixture fixture)
    {
        _repository = fixture.UserRateLimitRepository;
    }

    [Fact]
    public async Task Add_UserRateLimit_Success()
    {
        // Arrange
        var userRateLimit = UserRateLimitModelFaker.Generate()
            .First();
        
        // Act
        await _repository.Add(userRateLimit, default);
        var expiresAt = await _repository.GetExpireTimeIfExists(userRateLimit.UserIp, default);
        
        // Assert
        expiresAt.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Get_UserRateLimit_Success()
    {
        // Arrange
        var userRateLimit = UserRateLimitModelFaker.Generate()
            .Single();
        var userIp = userRateLimit.UserIp;
        
        await _repository.Add(userRateLimit, default);
        
        // Act
        var result = await _repository.Get(userIp, default);
    
        // Asserts
        result.Should().NotBeNull();
        result.UserIp.Should().Be(userIp);
    }
    
    [Fact]
    public async Task Decrement_UserRateLimit_Success()
    {
        // Arrange
        var userRateLimit = UserRateLimitModelFaker.Generate()
            .Single();
        var userIp = userRateLimit.UserIp;
        
        await _repository.Add(userRateLimit, default);
        
        // Act
        await _repository.Decrement(userIp, default);
    
        // Asserts
        var result = await _repository.Get(userIp, default);

        result.Should().NotBeNull();
        result.UserIp.Should().Be(userIp);
        result.CurrentLimit.Should().Be(userRateLimit.CurrentLimit - 1);
    }
    
    [Fact]
    public async Task Delete_TakenTask_Success()
    {
        // Arrange
        var userRateLimit = UserRateLimitModelFaker.Generate()
            .First();
        
        // Act
        await _repository.Delete(userRateLimit.UserIp, default);
    }
}