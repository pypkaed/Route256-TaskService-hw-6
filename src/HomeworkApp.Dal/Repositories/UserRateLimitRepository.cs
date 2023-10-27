using HomeworkApp.Dal.Models;
using HomeworkApp.Dal.Repositories.Interfaces;
using HomeworkApp.Dal.Settings;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace HomeworkApp.Dal.Repositories;

public class UserRateLimitRepository : RedisRepository, IUserRateLimitRepository
{
    protected override TimeSpan KeyTtl => TimeSpan.FromMinutes(1);

    protected override string KeyPrefix => "rate_limiter";

    public UserRateLimitRepository(IOptions<DalOptions> dalSettings) : base(dalSettings.Value)
    {
    }

    public async Task Add(UserRateLimitModel model, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        var connection = await GetConnection();

        var key = GetKey(model.UserIp);
        await connection.HashSetAsync(key, new HashEntry[]
        {
            new ("user_ip", model.UserIp),
            new ("current_limit", model.CurrentLimit),
        });

        await connection.KeyExpireAsync(key, KeyTtl);
    }

    public async Task<UserRateLimitModel?> Get(string userIp, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        var connection = await GetConnection();

        var key = GetKey(userIp);
        var fields = await connection.HashGetAllAsync(key);

        if (!fields.Any())
        {
            return null;
        }

        var result = new UserRateLimitModel();
        foreach (var field in fields)
        {
            if (!field.Value.HasValue)
            {
                continue;
            }

            field.Value.TryParse(out int longValue);
            var strValue = field.Value.ToString();
            
            result = field.Name.ToString() switch
            {
                "user_ip" => result with { UserIp = strValue},
                "current_limit" => result with { CurrentLimit = longValue },
                _ => result
            };
        }

        return result;
    }

    public async Task<long> Decrement(string userIp, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        var connection = await GetConnection();

        var key = GetKey(userIp);
        var currentLimit = await connection.HashDecrementAsync(key, "current_limit");

        return currentLimit;
    }

    public async Task<DateTime?> GetExpireTimeIfExists(string userIp, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        var connection = await GetConnection();
        var key = GetKey(userIp);
        
        return await connection.KeyExpireTimeAsync(key);
    }

    public async Task Delete(string userIp, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        var connection = await GetConnection();

        var key = GetKey(userIp);
        await connection.KeyDeleteAsync(key);
    }
}