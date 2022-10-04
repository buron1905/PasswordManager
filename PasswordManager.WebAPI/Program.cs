using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NLog;
using PasswordManager.WebAPI;
using PasswordManager.WebAPI.Data;
using PasswordManager.WebAPI.Extensions;
using PasswordManager.WebAPI.Helpers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureJwtAuthentication(builder.Configuration.GetAppSettings(builder.Services));

builder.Services.AddApplicationServices();

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
builder.Services.ConfigureLoggerService();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers();

builder.Services.AddDbContext<DataContext>(opt =>
    opt.UseSqlServer(""));

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
    //app.UseSpa(builder =>
    //{
    //    builder.UseProxyToSpaDevelopmentServer("http://localhost:4200/");
    //});
}

app.Run();
