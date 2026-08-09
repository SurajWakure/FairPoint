using System.Text;
using FairPoint.Application.Configuration;
using FairPoint.Application.Interfaces;
using FairPoint.Application.Services;
using FairPoint.Infrastructure.Data;
using FairPoint.Infrastructure.Repositories;
using FairPoint.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Register database connection factory.
builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordService,PasswordService>();
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<IJwtService,JwtService>();
builder.Services.AddScoped<IComplaintRepository,ComplaintRepository>();
builder.Services.AddScoped<IComplaintService,ComplaintService>();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));


builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(
        JwtOptions.SectionName));

builder.Services.AddScoped<
    IJwtService,
    JwtService>();

var jwtKey =
    builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "JWT key is not configured.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer =
                    builder.Configuration["Jwt:Issuer"],

                ValidAudience =
                    builder.Configuration["Jwt:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["Jwt:Key"]!))
            };
    });

builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// HTTPS redirection is temporarily disabled
// because our current development profile uses HTTP.
// app.UseHttpsRedirection();

// Map API controllers.
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.Run();