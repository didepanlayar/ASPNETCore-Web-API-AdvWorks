using AdvWorksAPI.EntityLayer;
using AdvWorksAPI.Interfaces;
using AdvWorksAPI.RepositoryLayer;
using AdvWorksAPI.ConstantClasses;
using AdvWorksAPI.ExtensionClasses;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.ConfigureGlobalSettings();

builder.Services.AddScoped<IRepository<Product>, ProductRepository>();

// Add CORS
builder.Services.ConfigureCors();

// Configure logging to Console and File using Serilog
builder.Host.ConfigureSeriLog();

// Configure ASP.NET to use the Controller model
builder.Services.AddControllers().ConfigureJsonOptions();

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
