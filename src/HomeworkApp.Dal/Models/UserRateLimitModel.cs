namespace HomeworkApp.Dal.Models;

public record UserRateLimitModel
{
    public string UserIp { get; init; }
    public int CurrentLimit { get; init; }
}