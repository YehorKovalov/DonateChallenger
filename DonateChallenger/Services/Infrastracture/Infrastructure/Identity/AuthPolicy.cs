namespace Infrastructure.Identity;

public class AuthPolicy
{
    public const string AdminOnlyPolicy = "AdminMinimum";
    public const string ManagerMinimumPolicy = "ManagerMinimum";
    public const string StreamerPolicy = "Streamer";
    public const string AuthorizedWithScope = "Donater";
}