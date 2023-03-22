using Microsoft.EntityFrameworkCore;
using PasswordManager.WebAPI.Extensions;
using PasswordManager.WebAPI.Middleware;
using Persistance;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var appSettings = builder.Configuration.GetAppSettings(builder.Services);

builder.Services.AddApplicationServices();

//LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
//builder.Services.ConfigureLoggerService();

builder.Services.AddControllers();

builder.Services.AddDbContext<DataContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseRouting();

app.UseMiddleware<JwtMiddleware>();

app.UseAuthorization();

app.UseEndpoints(configuration => { });

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = "api/swagger";
    });
    app.UseSpa(builder =>
    {
        builder.UseProxyToSpaDevelopmentServer("http://localhost:4200/");
    });
}

app.Run();
