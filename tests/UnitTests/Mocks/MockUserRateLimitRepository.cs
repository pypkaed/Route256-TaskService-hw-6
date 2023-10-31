using HomeworkApp.Dal.Models;
using HomeworkApp.Dal.Repositories.Interfaces;
using Moq;

namespace UnitTests.Mocks;

public static class MockUserRateLimitRepository
{
    private static int _currentRateLimit = 100;

    public static IUserRateLimitRepository Mock()
    {
        _currentRateLimit = 100;
        
        var repositoryMock = new Mock<IUserRateLimitRepository>();
        repositoryMock
            .Setup(repository => repository
                .Decrement(It.IsAny<UserIp>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => _currentRateLimit)
            .Callback(() => _currentRateLimit--);

        var repository = repositoryMock.Object;
        return repository;
    }
}