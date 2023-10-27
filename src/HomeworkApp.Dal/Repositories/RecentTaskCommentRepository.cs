using System.Text.Json;
using HomeworkApp.Dal.Models;
using HomeworkApp.Dal.Repositories.Interfaces;
using HomeworkApp.Dal.Settings;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace HomeworkApp.Dal.Repositories;

public class RecentTaskCommentRepository : RedisRepository, IRecentTaskCommentRepository
{
    protected override TimeSpan KeyTtl => TimeSpan.FromSeconds(5);
    protected override string KeyPrefix => "recent_task_comments";

    public RecentTaskCommentRepository(IOptions<DalOptions> dalSettings) : base(dalSettings.Value)
    {
    }


    public async Task Add(RecentTaskCommentModel model, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        var connection = await GetConnection();

        var key = GetKey(model.TaskId);
        var jsonModel = JsonSerializer.Serialize(model);
        
        await connection.HashSetAsync(key, new HashEntry[]
        {
            new (model.Id, jsonModel)
        });

        await connection.KeyExpireAsync(key, KeyTtl);
    }

    public async Task<RecentTaskCommentModel[]?> Get(long taskId, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        var connection = await GetConnection();

        var key = GetKey(taskId);
        var entries = await connection.HashGetAllAsync(key);

        if (!entries.Any())
        {
            return null;
        }

        var results = new List<RecentTaskCommentModel>();
        // var result = new RecentTaskCommentModel();
        foreach (var entry in entries)
        {
            if (!entry.Value.HasValue)
            {
                continue;
            }

            var result = JsonSerializer.Deserialize<RecentTaskCommentModel>(entry.Value.ToString());

            results.Add(result);

            // field.Value.TryParse(out long longValue);
            // var strValue = field.Value.ToString();
            //
            // result = field.Name.ToString() switch
            // {
            //     // "task_id" => result with {TaskId = longValue},
            //     // "title" => result with {Title = strValue},
            //     // "assigned_to_user_id" => result with {AssignedToUserId = longValue},
            //     // "assigned_to_email" => result with {AssignedToEmail = strValue},
            //     // "assigned_at" => result with{AssignedAt = DateTimeOffset.FromUnixTimeMilliseconds(longValue)},
            //     _ => result
            // };
        }

        return results.ToArray();
    }

    public async Task<DateTime?> GetExpireTimeIfExists(long taskId, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        var connection = await GetConnection();
        var key = GetKey(taskId);
        
        return await connection.KeyExpireTimeAsync(key);
    }

    public async Task Delete(long taskId, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        var connection = await GetConnection();

        var key = GetKey(taskId);
        await connection.KeyDeleteAsync(key);
    }
}