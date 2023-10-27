using HomeworkApp.Bll.Models;

namespace HomeworkApp.Bll.Services.Interfaces;

public interface IUserRateLimitService
{
    Task Add(string userIp, CancellationToken cancellationToken);

    Task<DateTime?> GetExpireTimeIfExists(string userIp, CancellationToken cancellationToken);
    
    Task<GetUserRateLimitModel?> Get(string userIp, CancellationToken cancellationToken);
    
    Task<long> Decrement(string userIp, CancellationToken cancellationToken);
}