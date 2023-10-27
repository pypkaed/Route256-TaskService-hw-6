using HomeworkApp.Dal.Models;

namespace HomeworkApp.Dal.Repositories.Interfaces;

public interface IUserRateLimitRepository
{
    Task Add(UserRateLimitModel model, CancellationToken token);
    
    Task<UserRateLimitModel?> Get(string userIp, CancellationToken token);
    
    Task<long> Decrement(string userIp, CancellationToken token);
    
    Task<DateTime?> GetExpireTimeIfExists(string userIp, CancellationToken token);
    
    Task Delete(string userIp, CancellationToken token);
}