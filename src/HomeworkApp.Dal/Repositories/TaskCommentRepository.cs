using Dapper;
using HomeworkApp.Dal.Entities;
using HomeworkApp.Dal.Models;
using HomeworkApp.Dal.Repositories.Interfaces;
using HomeworkApp.Dal.Settings;
using Microsoft.Extensions.Options;

namespace HomeworkApp.Dal.Repositories;

public class TaskCommentRepository : PgRepository, ITaskCommentRepository
{
    public TaskCommentRepository(IOptions<DalOptions> dalSettings) : base(dalSettings.Value)
    {
    }
    
    public async Task<long> Add(TaskCommentEntityV2 model, CancellationToken token)
    {
        const string sqlQuery = @"
insert into task_comments (task_id, author_user_id, message, at) 
values (@TaskId, @AuthorUserId, @Message, @At)
returning id;
";

        await using var connection = await GetConnection();
        var id = await connection.QuerySingleAsync<long>(
            new CommandDefinition(
                sqlQuery,
                new
                {
                    TaskId = model.TaskId,
                    AuthorUserId = model.AuthorUserId,
                    Message = model.Message,
                    At = model.At,
                },
                cancellationToken: token));
        
        return id;
    }

    public async Task Update(TaskCommentEntityV2 model, CancellationToken token)
    {
        const string sqlQuery = @"
update task_comments
set message = @Message
  , modified_at = @ModifiedAt
where id = @Id
";
        
        await using var connection = await GetConnection();
        await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                new
                {
                    Id = model.Id,
                    Message = model.Message,
                    ModifiedAt = DateTimeOffset.Now.UtcDateTime
                },
                cancellationToken: token));
    }

    public async Task SetDeleted(long taskCommentId, CancellationToken token)
    {
        const string sqlQuery = @"
update task_comments
set deleted_at = @DeletedAt
where id = @Id
";
        
        await using var connection = await GetConnection();
        await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                new
                {
                    Id = taskCommentId,
                    DeletedAt = DateTimeOffset.Now.UtcDateTime
                },
                cancellationToken: token));
    }

    public async Task<TaskCommentEntityV2[]?> Get(TaskCommentGetModel model, CancellationToken token)
    {
        var baseSql = @"
select id
     , task_id
     , author_user_id
     , message
     , at
     , modified_at
     , deleted_at
  from task_comments
";
        
        var conditions = new List<string>();
        var @params = new DynamicParameters();
        
        conditions.Add($@"task_id = @TaskId");
        @params.Add($@"TaskId", model.TaskId);

        if (!model.IncludeDeleted)
        {
            conditions.Add($"deleted_at IS NULL");
        }

        baseSql += $@" WHERE {string.Join(" AND ", conditions)} ";
        baseSql += $"\n ORDER BY at DESC";
        
        var cmd = new CommandDefinition(
            baseSql,
            @params,
            commandTimeout: DefaultTimeoutInSeconds,
            cancellationToken: token);
        
        await using var connection = await GetConnection();
        return (await connection.QueryAsync<TaskCommentEntityV2>(cmd))
            .ToArray();
    }
}