using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Testing;
using HomeworkApp.Bll.Services;
using HomeworkApp.Interceptors;
using UnitTests.Mocks;
using WorkshopApp.Proto.Client;
using Xunit;

namespace UnitTests.ApplicationTests;

public class UserRateLimitTests
{
    [Fact]
    public async Task Test()
    {
        // Arrange
        var serviceservice = new UserRateLimitService(MockUserRateLimitRepository.Mock());
        var interceptor = new UserRateLimitInterceptor(serviceservice);
        var request = new V1AssignTaskRequest();

        var context = TestServerCallContext.Create(
            "V1CreateTask",
            null,
            DateTimeOffset.Now.UtcDateTime,
            new Metadata()
            {
                { "X-R256-USER-IP", "userIp" }
            },
            default, 
            default, 
            default, 
            default,
            default, 
            default, 
            default);
        // Act
        var response = await interceptor
            .UnaryServerHandler(request, context, 
                (req, ctx) => Task.FromResult(new Empty()));

        // Assert
        Assert.NotNull(response);
    }
    
    [Fact]
    public async Task Test_Fail()
    {
        // Arrange
        var serviceservice = new UserRateLimitService(MockUserRateLimitRepository.Mock());
        var interceptor = new UserRateLimitInterceptor(serviceservice);
        var request = new V1AssignTaskRequest();

        var context = TestServerCallContext.Create(
            "V1CreateTask",
            null,
            DateTimeOffset.Now.UtcDateTime,
            new Metadata()
            {
                { "X-R256-USER-IP", "userIp" }
            },
            default, 
            default, 
            default, 
            default,
            default, 
            default, 
            default);
        
        // Act
        for (int i = 0; i < 100; i++)
        {
            await interceptor
                .UnaryServerHandler(request, context, 
                    (req, ctx) => Task.FromResult(new Empty()));
        }
        
        var failAction = async () => await interceptor
            .UnaryServerHandler(request, context, 
                (req, ctx) => Task.FromResult(new Empty()));
        // Assert
        
        await failAction.Should().ThrowAsync<Exception>();
    }
}