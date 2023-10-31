namespace HomeworkApp.Dal.Models;

public record UserRateLimitModel
{
    public UserIp UserIp { get; init; }
    public int CurrentLimit { get; init; }
}