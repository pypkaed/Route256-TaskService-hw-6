namespace HomeworkApp.Bll.Models;

public record GetUserRateLimitModel
{
    public required string UserIp { get; init; }
    public int CurrentRateLimit { get; init; }
}