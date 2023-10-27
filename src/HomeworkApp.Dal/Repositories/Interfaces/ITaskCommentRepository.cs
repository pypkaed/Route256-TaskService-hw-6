using HomeworkApp.Dal.Entities;
using HomeworkApp.Dal.Models;

namespace HomeworkApp.Dal.Repositories.Interfaces;

public interface ITaskCommentRepository
{
    Task<long> Add(TaskCommentEntityV2 model, CancellationToken token);
    Task Update(TaskCommentEntityV2 model, CancellationToken token);
    Task SetDeleted(long taskCommentId, CancellationToken token);
    Task<TaskCommentEntityV2[]?> Get(TaskCommentGetModel model, CancellationToken token);
}