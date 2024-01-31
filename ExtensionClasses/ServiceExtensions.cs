using System.Text;
using AdvWorksAPI.ConstantClasses;
using AdvWorksAPI.EntityLayer;
using AdvWorksAPI.Interfaces;
using AdvWorksAPI.RepositoryLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace AdvWorksAPI.ExtensionClasses;

public static class ServiceExtension
{
    public static void AddRepositoryClasses(this IServiceCollection services)
    {
        // Add Repository Classes
        services.AddScoped<IRepository<Product>, ProductRepository>();
    }
    public static AuthenticationBuilder ConfigureJwtAuthentication(this IServiceCollection services, AdvWorksAPIDefaults settings)
    {
        // Add Authentication to Services
        return services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(jwtOptions =>
        {
            jwtOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = settings.JWTSettings.Issuer,
                ValidAudience = settings.JWTSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JWTSettings.Key)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.FromMinutes(settings.JWTSettings.MinutesToExpiration)
            };
        });
    }
    public static IServiceCollection ConfigureJwtAuthorization(this IServiceCollection services)
    {
        return services.AddAuthorization();
    }
    public static IServiceCollection ConfigureCors(this IServiceCollection services)
    {
        // Add CORS
        return services.AddCors(options =>
        {
            options.AddPolicy(AdvWorksAPIConstants.CORS_POLICY,
            builder =>
            {
                // builder.AllowAnyOrigin();
                // builder.WithOrigins("http://localhost:5222", "http://www.example.com");
                // builder.WithOrigins("http://localhost:5222", "http://www.example.com").AllowAnyMethod();
                // builder.WithOrigins("http://localhost:5222", "http://www.example.com").WithMethods("GET", "POST", "PUT");
            });
        });
    }
}