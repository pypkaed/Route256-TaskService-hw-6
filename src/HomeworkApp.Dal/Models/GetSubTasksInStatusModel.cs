namespace HomeworkApp.Dal.Models;

public record GetSubTasksInStatusModel
{
    public required long ParentTaskId { get; init; }
    public int[] Statuses { get; init; }
}