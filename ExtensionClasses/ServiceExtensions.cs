using System.Text;
using AdvWorksAPI.ConstantClasses;
using AdvWorksAPI.EntityLayer;
using AdvWorksAPI.Interfaces;
using AdvWorksAPI.RepositoryLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using AdvWorksAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AdvWorksAPI.ExtensionClasses;

public static class ServiceExtension
{
    public static void AddRepositoryClasses(this IServiceCollection services)
    {
        // Add Repository Classes
        services.AddScoped<IRepository<Product, ProductSearch>, ProductRepository>();
    }
    public static IServiceCollection ConfigureAdventureWorksDB(this IServiceCollection services, string? cnn)
    {
        // Setup the DbContext object
        return services.AddDbContext<AdvWorksLTDbContext>(options => options.UseSqlServer(cnn));
    }
    public static IServiceCollection ConfigureOpenAPI(this IServiceCollection services)
    {
        // Configure Open API (Swagger)
        // More Info: https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        return services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Description = "Bearer Authentication with JWT Token",
                Type = SecuritySchemeType.Http
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            });
        });
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
        return services.AddAuthorization(options =>
        {
            options.AddPolicy("GetProductsClaim", policy => policy.RequireClaim("GetProducts"));
            options.AddPolicy("GetAProductClaim", policy => policy.RequireClaim("GetAProduct"));
            options.AddPolicy("SearchClaim", policy => policy.RequireClaim("Search"));
            options.AddPolicy("AddProductClaim", policy => policy.RequireClaim("AddProduct"));
            options.AddPolicy("UpdateProductClaim", policy => policy.RequireClaim("UpdateProduct"));
        });
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