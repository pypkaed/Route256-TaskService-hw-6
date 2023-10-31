using HomeworkApp.Dal.Models;

namespace HomeworkApp.Dal.Repositories.Interfaces;

public interface IUserRateLimitRepository
{
    Task Add(UserRateLimitModel model, CancellationToken token);
    
    Task<UserRateLimitModel?> Get(UserIp userIp, CancellationToken token);
    
    Task<long> Decrement(UserIp userIp, CancellationToken token);
    
    Task<DateTime?> GetExpireTimeIfExists(UserIp userIp, CancellationToken token);
    
    Task Delete(UserIp userIp, CancellationToken token);
}