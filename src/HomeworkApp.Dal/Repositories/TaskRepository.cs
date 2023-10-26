using Dapper;
using HomeworkApp.Dal.Entities;
using HomeworkApp.Dal.Models;
using HomeworkApp.Dal.Repositories.Interfaces;
using HomeworkApp.Dal.Settings;
using Microsoft.Extensions.Options;

namespace HomeworkApp.Dal.Repositories;

public class TaskRepository : PgRepository, ITaskRepository
{
    public TaskRepository(
        IOptions<DalOptions> dalSettings) : base(dalSettings.Value)
    {
    }

    public async Task<long[]> Add(TaskEntityV1[] tasks, CancellationToken token)
    {
        const string sqlQuery = @"
insert into tasks (parent_task_id, number, title, description, status, created_at, created_by_user_id, assigned_to_user_id, completed_at) 
select parent_task_id, number, title, description, status, created_at, created_by_user_id, assigned_to_user_id, completed_at
  from UNNEST(@Tasks)
returning id;
";

        await using var connection = await GetConnection();
        var ids = await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                new
                {
                    Tasks = tasks
                },
                cancellationToken: token));
        
        return ids
            .ToArray();
    }

    public async Task<TaskEntityV1[]> Get(TaskGetModel query, CancellationToken token)
    {
        var baseSql = @"
select id
     , parent_task_id
     , number
     , title
     , description
     , status
     , created_at
     , created_by_user_id
     , assigned_to_user_id
     , completed_at
  from tasks
";
        
        var conditions = new List<string>();
        var @params = new DynamicParameters();

        if (query.TaskIds.Any())
        {
            conditions.Add($"id = ANY(@TaskIds)");
            @params.Add($"TaskIds", query.TaskIds);
        }
        
        var cmd = new CommandDefinition(
            baseSql + $" WHERE {string.Join(" AND ", conditions)} ",
            @params,
            commandTimeout: DefaultTimeoutInSeconds,
            cancellationToken: token);
        
        await using var connection = await GetConnection();
        return (await connection.QueryAsync<TaskEntityV1>(cmd))
            .ToArray();
    }

    public async Task Assign(AssignTaskModel model, CancellationToken token)
    {
        const string sqlQuery = @"
update tasks
   set assigned_to_user_id = @AssignToUserId
     , status = @Status
 where id = @TaskId
";

        await using var connection = await GetConnection();
        await connection.ExecuteAsync(
            new CommandDefinition(
                sqlQuery,
                new
                {
                    TaskId = model.TaskId,
                    AssignToUserId = model.AssignToUserId,
                    Status = model.Status
                },
                cancellationToken: token));
    }

    public async Task<SubTaskModel[]> GetSubTasksInStatus(GetSubTasksInStatusModel query, CancellationToken token)
    {
        const string sqlQuery = @"
with recursive cte as (select t.id
                            , t.parent_task_id
                            , array[t.parent_task_id] as parent_task_ids
                            , t.status as status
                            , t.title as title
                       from tasks t
                       where t.parent_task_id = @ParentTaskId
                       union all
                       select t1.id
                            , t1.parent_task_id
                            , c.parent_task_ids || array[t1.parent_task_id]
                            , t1.status as status
                            , t1.title as title
                       from tasks t1
                                join cte c on c.id = t1.parent_task_id)
select c.id
     , c.title
     , c.parent_task_ids
     , c.status
  from cte c
 where c.status = any(@Statuses)
";

        
        
        await using var connection = await GetConnection();

        var cmd = new CommandDefinition(
            sqlQuery,
            new
            {
                ParentTaskId = query.ParentTaskId,
                Statuses = query.Statuses
            },
            cancellationToken: token);
        
        return (await connection.QueryAsync<SubTaskModel>(cmd))
            .ToArray();
    }
}