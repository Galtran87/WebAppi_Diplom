using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using WebAppi_Diplom;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using WebAppi_HW7;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace WebApi_HW7
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Читання конфігурації
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var mySecret = configuration.GetValue<string>("Auth:Secret");
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

            var myIssuer = configuration.GetValue<string>("Auth:Issuer");
            var myAudience = configuration.GetValue<string>("Auth:Audience");

            builder.Services.AddControllers();
            builder.Services.AddScoped<TeamRepository>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = myIssuer,
                    ValidAudience = myAudience,
                    IssuerSigningKey = mySecurityKey
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("User", policy => policy.RequireRole("User"));
            });

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<FootballDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddScoped<ITeamService, TeamService>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                // Додайте цей рядок для Security Definitions
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API");

                    
                    c.OAuthClientId("swagger");
                    c.OAuthClientSecret("swagger_secret");
                    c.OAuthRealm("Swagger UI");
                    c.OAuthAppName("Swagger UI");
                    c.OAuthUseBasicAuthenticationWithAccessCodeGrant(); // Використовуйте OAuth2 авторизацію
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Use(async (context, next) =>
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("First step (Exception Middleware)");

                    await next();

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Second step (Exception Middleware)");
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                }
            });

            app.Run();
        }
    }
}
