namespace Identity.API.Models.Requests;

public class ManagerGetPortionedUsersRequest
{
    public int CurrentPortion { get; set; }

    public int UsersPerPortion { get; set; }
}