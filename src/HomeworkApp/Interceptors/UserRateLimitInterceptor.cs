using Grpc.Core;
using Grpc.Core.Interceptors;
using HomeworkApp.Bll.Services.Interfaces;

namespace HomeworkApp.Interceptors;

public class UserRateLimitInterceptor : Interceptor
{
    private readonly IUserRateLimitService _userRateLimitService;

    public UserRateLimitInterceptor(IUserRateLimitService userRateLimitService)
    {
        _userRateLimitService = userRateLimitService;
    }
    
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var userIp = context.RequestHeaders.Get("X-R256-USER-IP")?.Value;

        if (userIp is null) return await continuation(request, context);
        
        var userRateLimitExpirationTime = await _userRateLimitService.GetExpireTimeIfExists(userIp, context.CancellationToken);
        if (userRateLimitExpirationTime is null)
        {
            await _userRateLimitService.Add(userIp, context.CancellationToken);
        }
        
        var currentRateLimit = await _userRateLimitService.Decrement(userIp, context.CancellationToken);
        
        Console.WriteLine(currentRateLimit);

        if (currentRateLimit <= 0)
        {
            throw new Exception("User has exceeded their rate limit.");
        }

        return await continuation(request, context);
    }
}