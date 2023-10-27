using HomeworkApp.Dal.Models;

namespace HomeworkApp.Dal.Repositories.Interfaces;

public interface IRecentTaskCommentRepository
{
    Task Add(RecentTaskCommentModel model, CancellationToken token);

    Task<RecentTaskCommentModel[]?> Get(long taskId, CancellationToken token);
    
    Task<DateTime?> GetExpireTimeIfExists(long taskId, CancellationToken token);

    Task Delete(long taskId, CancellationToken token);
}