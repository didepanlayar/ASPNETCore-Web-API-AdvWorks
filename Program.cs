using AdvWorksAPI.ConstantClasses;
using AdvWorksAPI.EntityLayer;
using AdvWorksAPI.ExtensionClasses;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.ConfigureGlobalSettings();

// Add & Configure AdventureWorksLT DbContext
builder.Services.ConfigureAdventureWorksDB(builder.Configuration.GetConnectionString("DefaultConnection"));

// Configure Repository Classes
builder.Services.AddRepositoryClasses();

// Add CORS
builder.Services.ConfigureCors();

// Configure logging to Console and File using Serilog
builder.Host.ConfigureSeriLog();

// Add & Configure JWT Authentication
builder.Services.ConfigureJwtAuthentication(builder.Configuration.GetRequiredSection("AdvWorksAPI").Get<AdvWorksAPIDefaults>());

// Add & Configure JWT Authorization
builder.Services.ConfigureJwtAuthorization();

// Configure ASP.NET to use the Controller model
builder.Services.AddControllers().ConfigureJsonOptions();

// Add & Configure Open API (Swagger)
builder.Services.ConfigureOpenAPI();

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

// Enable Authentication and Authorization Middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
