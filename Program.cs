using AdvWorksAPI.EntityLayer;
using AdvWorksAPI.Interfaces;
using AdvWorksAPI.RepositoryLayer;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<AdvWorksAPIDefaults, AdvWorksAPIDefaults>();
builder.Services.AddScoped<IRepository<Product>, ProductRepository>();

// Configure logging to Console and File using Serilog
builder.Host.UseSerilog((ctx, lc) =>
{
    // Log to Console
    lc.WriteTo.Console();
    // Log to Rolling File
    lc.WriteTo.File("Logs/InfoLog-.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Information);
    lc.WriteTo.File("Logs/ErrorLog-.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Error);
});

builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
