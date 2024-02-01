using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AdvWorksAPI.EntityLayer;
using Microsoft.IdentityModel.Tokens;

namespace AdvWorksAPI.SecurityLayer;

public class SecurityManager
{
    #region AuthenticateUser Method
    public AppSecurityToken AuthenticateUser(string name, string password, JwtSettings settings)
    {
        AppSecurityToken asToken;

        // Validate the user passed in
        // Create the AppSecurityToken object
        asToken = ValidateUser(name, password);

        if (asToken.User.IsAuthenticated)
        {
            // Load User Claims into Security Token
            LoadUserClaims(asToken);

            // Build Application Security Token
            SetJwtToken(settings, asToken);
        }

        return asToken;
    }
    #endregion

    #region ValidateUser Method
    protected AppSecurityToken ValidateUser(string name, string password)
    {
        AppSecurityToken asToken = new();

        // Validate User - Hard coded for now
        // TODO: Authenticate against a data store
        switch (name.ToLower())
        {
            case "pauls":
                if (password == "password")
                {
                    asToken.User.UserName = name;
                    asToken.User.UserId = new Guid("4df9b2b3-e497-407f-8b84-d0e638bdcdcc");
                    asToken.User.IsAuthenticated = true;
                }
                break;

            case "johnk":
                if (password == "password")
                {
                    asToken.User.UserName = name;
                    asToken.User.UserId = new Guid("1a8418ff-550f-4341-b6f8-1003085ce01b");
                    asToken.User.IsAuthenticated = true;
                }
                break;
        }

        return asToken;
    }
    #endregion

    #region LoadUserClaims
    protected void LoadUserClaims(AppSecurityToken asToken)
    {
        // Get Claims for a user - HARD CODED FOR NOW
        // TODO: Get Claims from a Data Store
        switch (asToken.User.UserName.ToLower())
        {
            case "pauls":
                // Add GetProducts
                asToken.Claims.Add(new AppUserClaim()
                {
                    UserId = asToken.User.UserId,
                    ClaimType = "GetProducts",
                    ClaimValue = "true"
                });
                // Add GetAProduct
                asToken.Claims.Add(new AppUserClaim()
                {
                    UserId = asToken.User.UserId,
                    ClaimType = "GetAProduct",
                    ClaimValue = "true"
                });
                // Add Search
                asToken.Claims.Add(new AppUserClaim()
                {
                    UserId = asToken.User.UserId,
                    ClaimType = "Search",
                    ClaimValue = "true"
                });
                break;

            case "johnk":
                // Add GetAProduct
                asToken.Claims.Add(new AppUserClaim()
                {
                    UserId = asToken.User.UserId,
                    ClaimType = "GetAProduct",
                    ClaimValue = "true"
                });
                // Add AddProduct
                asToken.Claims.Add(new AppUserClaim()
                {
                    UserId = asToken.User.UserId,
                    ClaimType = "AddProduct",
                    ClaimValue = "true"
                });
                // Add UpdateProduct
                asToken.Claims.Add(new AppUserClaim()
                {
                    UserId = asToken.User.UserId,
                    ClaimType = "UpdateProduct",
                    ClaimValue = "true"
                });
                break;
        }
    }
    #endregion

    #region SetJwtToken
    protected void SetJwtToken(JwtSettings settings, AppSecurityToken asToken)
    {
        // Build JWT claims
        List<Claim> claims = BuildJWTClaims(asToken);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Expires = DateTime.UtcNow.AddMinutes(settings.MinutesToExpiration),
            Issuer = settings.Issuer,
            Audience = settings.Audience,
            SigningCredentials = new SigningCredentials
              (new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.Key)),
              SecurityAlgorithms.HmacSha512Signature),
            // Add Claims
            Subject = new ClaimsIdentity(claims)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var bearerToken = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

        // Create a string representation of the Jwt token
        // Stored into BearerToken property
        asToken.BearerToken = bearerToken;
    }
    #endregion

    #region BuildJWTClaims Method
    protected List<Claim> BuildJWTClaims(AppSecurityToken asToken)
    {
        // Create standard JWT claims
        List<Claim> ret = new()
    {
      // Add Unique User Name
      new Claim(JwtRegisteredClaimNames.Sub, asToken.User.UserName),
      // Add Unique JWT Token Identifier
      new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
      // Add IsAuthenticated Claim
      new Claim("IsAuthenticated", asToken.User.IsAuthenticated.ToString())
    };

        // Add Custom Claims for your Application
        foreach (var item in asToken.Claims)
        {
            ret.Add(new Claim(item.ClaimType, item.ClaimValue));
        }

        return ret;
    }
    #endregion
}
