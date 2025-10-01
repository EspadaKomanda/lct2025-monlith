using Serilog;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LctMonolith.Models.Database;
using Microsoft.AspNetCore.Identity;
using LctMonolith.Application.Middleware;
using LctMonolith.Services;
using LctMonolith.Application.Options;
using LctMonolith.Database.Data;
using LctMonolith.Database.UnitOfWork;
using LctMonolith.Services.Contracts;
using LctMonolith.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, services, loggerConfig) =>
    loggerConfig
        .ReadFrom.Configuration(ctx.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", "LctMonolith"));

var connectionString = builder.Configuration.GetConnectionString("Default") ?? "Host=localhost;Port=5432;Database=lct2025;Username=postgres;Password=postgres";
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"] ?? "Dev_Insecure_Key_Change_Me";
var jwtIssuer = jwtSection["Issuer"] ?? "LctMonolith";
var jwtAudience = jwtSection["Audience"] ?? "LctMonolithAudience";
var accessMinutes = int.TryParse(jwtSection["AccessTokenMinutes"], out var m) ? m : 60;
var refreshDays = int.TryParse(jwtSection["RefreshTokenDays"], out var d) ? d : 7;
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services.Configure<JwtOptions>(o =>
{
    o.Key = jwtKey;
    o.Issuer = jwtIssuer;
    o.Audience = jwtAudience;
    o.AccessTokenMinutes = accessMinutes;
    o.RefreshTokenDays = refreshDays;
});

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(connectionString));

builder.Services.AddIdentityCore<AppUser>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
        options.Password.RequiredLength = 6;
    })
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddSignInManager<SignInManager<AppUser>>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(o =>
    {
        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = signingKey,
            ClockSkew = TimeSpan.FromMinutes(2)
        };
    });

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddOpenApi();

builder.Services.AddHealthChecks();

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddCors(p => p.AddDefaultPolicy(policy =>
    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseErrorHandling();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });
app.MapHealthChecks("/health/ready");

app.Run();
