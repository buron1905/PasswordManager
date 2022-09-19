using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PasswordManager.WebAPI;
using PasswordManager.WebAPI.Extensions;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureJwtAuthentication(builder.Configuration.GetAppSettings(builder.Services));

builder.Services.ConfigureCors();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseCors("EnableCORS");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
