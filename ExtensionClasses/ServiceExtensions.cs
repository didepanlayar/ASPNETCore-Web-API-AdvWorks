using AdvWorksAPI.ConstantClasses;

namespace AdvWorksAPI.ExtensionClasses;

public static class ServiceExtension
{
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