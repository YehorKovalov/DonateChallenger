namespace Identity.API.Models.Responses;

public class ManagerGetPortionedUsersResponse<TUser>
{
    public int CurrentPortion { get; set; }

    public int UsersPerPortion { get; set; }
    public bool MorePortionsExist { get; set; }

    public IEnumerable<TUser> Users { get; set; }
}