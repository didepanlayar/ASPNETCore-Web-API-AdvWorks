using AdvWorksAPI.EntityLayer;
using AdvWorksAPI.Interfaces;
using AdvWorksAPI.RepositoryLayer;
using Serilog;

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
    lc.WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day);
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
app.UseExceptionHandler("/ProductionError");

app.UseAuthorization();

app.MapControllers();

app.Run();
