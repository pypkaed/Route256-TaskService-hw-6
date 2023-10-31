using HomeworkApp.Bll.Models;
using HomeworkApp.Bll.Services.Interfaces;
using HomeworkApp.Dal.Models;
using HomeworkApp.Dal.Repositories.Interfaces;

namespace HomeworkApp.Bll.Services;

public class UserRateLimitService : IUserRateLimitService
{
    private const int CurrentLimitDefault = 100;
    
    private readonly IUserRateLimitRepository _userRateLimitRepository;

    public UserRateLimitService(IUserRateLimitRepository userRateLimitRepository)
    {
        _userRateLimitRepository = userRateLimitRepository;
    }
    
    public async Task Add(string userIp, CancellationToken cancellationToken)
    {
        var userIpModel = new UserIp(userIp);
        
        var expireTimeExists = await _userRateLimitRepository.GetExpireTimeIfExists(userIpModel, cancellationToken);
        if (expireTimeExists is not null)
        {
            throw new Exception("User's current rate limit is not yet expired.");
        }
        
        await _userRateLimitRepository.Add(new UserRateLimitModel()
        {
            UserIp = userIpModel,
            CurrentLimit = CurrentLimitDefault
        },
            cancellationToken);
    }

    public async Task<DateTime?> GetExpireTimeIfExists(string userIp, CancellationToken cancellationToken)
    {
        var userIpModel = new UserIp(userIp);

        var result = await _userRateLimitRepository.GetExpireTimeIfExists(userIpModel, cancellationToken);
        return result;
    }

    public async Task<GetUserRateLimitModel?> Get(string userIp, CancellationToken cancellationToken)
    {
        var userIpModel = new UserIp(userIp);
        
        var userRateLimit = await _userRateLimitRepository.Get(userIpModel, cancellationToken);
        if (userRateLimit is null)
        {
            return null;
        }

        var result = new GetUserRateLimitModel()
        {
            UserIp = userRateLimit.UserIp.Ip,
            CurrentRateLimit = userRateLimit.CurrentLimit
        };

        return result;
    }

    public async Task<long> Decrement(string userIp, CancellationToken cancellationToken)
    {
        var userIpModel = new UserIp(userIp);

        var currentRateLimit = await _userRateLimitRepository.Decrement(userIpModel, cancellationToken);
        return currentRateLimit;
    }
}