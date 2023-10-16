using AdvWorksAPI.EntityLayer;
using AdvWorksAPI.Interfaces;
using AdvWorksAPI.RepositoryLayer;
using AdvWorksAPI.ConstantClasses;
using AdvWorksAPI.ExtensionClasses;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<AdvWorksAPIDefaults, AdvWorksAPIDefaults>();
// Read "AdvWorksAPI" section and add as a singleton
AdvWorksAPIDefaults settings = new();
builder.Configuration.GetSection("AdvWorksAPI").Bind(settings);
builder.Services.AddSingleton<AdvWorksAPIDefaults>(settings);
// Use the IOptionsMonitor<AdvWorksAPIDefaults> in controller's constructor
builder.Services.Configure<AdvWorksAPIDefaults>(builder.Configuration.GetSection("AdvWorksAPI"));

builder.Services.AddScoped<IRepository<Product>, ProductRepository>();

// Add CORS
builder.Services.ConfigureCors();

// Configure logging to Console and File using Serilog
builder.Host.UseSerilog((ctx, lc) =>
{
    // Log to Console
    lc.WriteTo.Console();
    // Log to Rolling File
    lc.WriteTo.File("Logs/InfoLog-.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Information);
    lc.WriteTo.File("Logs/ErrorLog-.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Error);
});

// Configure ASP.NET to use the Controller model
builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Make all property names start with upper-case
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    // Ignore "readonly" fields
    options.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
}).AddXmlSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable Exception Handing Middleware
if(app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/DevelopmentError");
}
else {
    app.UseExceptionHandler("/ProductionError");
}

// Handle status code error in the range 400-599
app.UseStatusCodePagesWithReExecute("/StatusCodeHandler/{0}");

// Enable CORS Middleware
app.UseCors(AdvWorksAPIConstants.CORS_POLICY);

app.UseAuthorization();

app.MapControllers();

app.Run();
