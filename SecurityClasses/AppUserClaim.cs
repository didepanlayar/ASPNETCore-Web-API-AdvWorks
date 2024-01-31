namespace AdvWorksAPI.SecurityLayer;

public partial class AppUserClaim
{
    public AppUserClaim()
    {
        ClaimId = Guid.NewGuid();
        UserId = Guid.NewGuid();
        ClaimType = string.Empty;
        ClaimValue = string.Empty;
    }

    public Guid ClaimId { get; set; }
    public Guid UserId { get; set; }
    public string ClaimType { get; set; }
    public string ClaimValue { get; set; }
}
