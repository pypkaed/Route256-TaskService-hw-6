namespace HomeworkApp.Bll.Helpers;

public static class CacheHelper
{
    private const string CacheTaskCommentsPrefix = "cache_task_comments";

    public static string GetTaskCommentCacheKey(long taskId)
    {
        var cacheKey = $"{CacheTaskCommentsPrefix}:{taskId}";
        return cacheKey;
    }
}