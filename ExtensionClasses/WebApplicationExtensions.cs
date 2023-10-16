using AdvWorksAPI.EntityLayer;

namespace AdvWorksAPI.ExtensionClasses
{
    public static class WebApplicationBuilderExtensions
    {
        public static void ConfigureGlobalSettings(this WebApplicationBuilder builder)
        {
            // Configure Global Settings

            // Read "AdvWorksAPI" section
            // Use the IOptionsMonitor<AdvWorksAPIDefaults> in controller's constructor
            builder.Services.Configure<AdvWorksAPIDefaults>(builder.Configuration.GetSection("AdvWorksAPI"));

            // NOTE: The following lines are only used for the ConfigTestController
            builder.Services.AddSingleton<AdvWorksAPIDefaults, AdvWorksAPIDefaults>();
            // Read "AdvWorksAPI" section and add as a singleton
            AdvWorksAPIDefaults settings = new();
            builder.Configuration.GetSection("AdvWorksAPI").Bind(settings);
            builder.Services.AddSingleton<AdvWorksAPIDefaults>(settings);
        }
    }
}