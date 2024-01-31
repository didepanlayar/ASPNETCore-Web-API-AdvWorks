namespace AdvWorksAPI.SecurityLayer;

public class AppSecurityToken
{
    public AppSecurityToken()
    {
        User = new() { UserName = "Not Authenticated" };
        BearerToken = string.Empty;
        Claims = new();
    }

    public AppUser User { get; set; }
    public string BearerToken { get; set; }
    public List<AppUserClaim> Claims { get; set; }
}
