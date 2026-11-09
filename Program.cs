using System.Security.Claims;
using System.Text;
using CodingPortfolio.Data;
using CodingPortfolio.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CodingPortfolio
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.Configure<JwtSettings>(
                builder.Configuration.GetSection(JwtSettings.SectionName));
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            var jwtSettings = builder.Configuration
                .GetSection(JwtSettings.SectionName)
                .Get<JwtSettings>()
                ?? throw new InvalidOperationException("Missing \"jwt\" section in configuration.");

            if (string.IsNullOrWhiteSpace(jwtSettings.Key))
            {
                throw new InvalidOperationException(
                    "Jwt:Key is missing. Set it with: " +
                    "dotnet user-secrets set \"Jwt:Key\" \"<random-key-at-least-32-chars>\"");
            }

            if (Encoding.UTF8.GetByteCount(jwtSettings.Key) < 32)
            {
                throw new InvalidOperationException(
                    "Jwt:Key must be at least 32 characters (256 bits) for HmacSha256.");
            }

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.Key))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            var db = context.HttpContext.RequestServices
                                .GetRequiredService<ApplicationDbContext>();

                            var idValue = context.Principal?
                                .FindFirstValue(ClaimTypes.NameIdentifier);

                            if (!int.TryParse(idValue, out var developerId) ||
                                !await db.Developers.AnyAsync(d => d.DeveloperId == developerId))
                            {
                                context.Fail("This account no longer exists.");
                            }
                        }
                    };
                });

            builder.Services.AddAuthorization();
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
            else
            {
                // HTTPS redirection is skipped in Development because clients
                // drop the Authorization header when following the redirect.
                app.UseHttpsRedirection();
            }

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}