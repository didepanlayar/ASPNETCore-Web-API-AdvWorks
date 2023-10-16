using AdvWorksAPI.ConstantClasses;
using AdvWorksAPI.EntityLayer;
using AdvWorksAPI.Interfaces;
using AdvWorksAPI.RepositoryLayer;

namespace AdvWorksAPI.ExtensionClasses;

public static class ServiceExtension
{
    public static void AddRepositoryClasses(this IServiceCollection services)
    {
        // Add Repository Classes
        services.AddScoped<IRepository<Product>, ProductRepository>();
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