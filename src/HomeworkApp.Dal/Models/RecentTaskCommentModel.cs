namespace HomeworkApp.Dal.Models;

public record RecentTaskCommentModel
{
    public long Id { get; init; }
    public long TaskId { get; init; }
    public long AuthorUserId { get; init; }
    public string Message { get; init; }
    public DateTimeOffset At { get; init; }
    public DateTimeOffset? ModifiedAt { get; init; }
    public DateTimeOffset? DeletedAt { get; init; }
}