using Microsoft.IdentityModel.Tokens;

namespace AdvWorksAPI.EntityLayer;

public class JwtSettings
{
    public JwtSettings()
    {
        Key = "A_KEY_GOES_HERE";
        Issuer = "http://localhost:nnnn";
        Audience = "Audience";
        MinutesToExpiration = 1;
    }

    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int MinutesToExpiration { get; set; }
}