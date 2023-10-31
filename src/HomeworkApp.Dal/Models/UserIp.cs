namespace HomeworkApp.Dal.Models;

public record UserIp
{
    public UserIp(string ip)
    {
        ValidateIp(ip);
        
        Ip = ip;
    }

    public string Ip { get; }

    private static void ValidateIp(string ip)
    {
        if (String.IsNullOrWhiteSpace(ip))
        {
            throw new Exception("Null ip address");
        }

        string[] splitValues = ip.Split('.');
        if (splitValues.Length != 4)
        {
            throw new Exception("Invalid ip.");
        }
        
        if (!splitValues.All(r => byte.TryParse(r, out var tempForParsing)))
        {
            throw new Exception("Invalid ip.");
        }
    }
}