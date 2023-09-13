using Microsoft.EntityFrameworkCore;
using Models.Context;
using Models.UnitsOfWork;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Logger de la aplicación

ILoggerFactory loggerFactory = new LoggerFactory();
loggerFactory.AddSerilog(new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("C:/ProyectosVisualStudio/GameTracker/GameTracker/Logs/").CreateLogger());
builder.Services.AddSingleton(typeof(ILoggerFactory), loggerFactory);
builder.Services.AddSingleton(typeof(Microsoft.Extensions.Logging.ILogger), loggerFactory.CreateLogger("Logger"));

#endregion

#region Contexto de la BBDD y unidad de trabajo

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnect")));
builder.Services.AddScoped<UnitOfWork>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
