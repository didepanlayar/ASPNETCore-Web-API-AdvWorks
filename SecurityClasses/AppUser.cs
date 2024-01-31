using System.Text.Json.Serialization;

namespace AdvWorksAPI.SecurityLayer;

public partial class AppUser
{
    public AppUser()
    {
        UserId = Guid.NewGuid();
        UserName = string.Empty;
        Password = string.Empty;
        IsAuthenticated = false;
    }

    public Guid UserId { get; set; }
    public string UserName { get; set; }
    [JsonIgnore]
    public string Password { get; set; }
    public bool IsAuthenticated { get; set; }
}
