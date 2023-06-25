using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using WebAppi_Diplom;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace WebApi_HW7
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Реєстрація сервісів
            builder.Services.AddControllers();
            builder.Services.AddSingleton<TeamRepository>(); // Реєстрація TeamRepository
            builder.Services.AddScoped<ITeamService, TeamService>(); // Реєстрація TeamService

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Конфігурація конвеєра HTTP-запиту
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();

            // Дублювання ендпоїнта з використанням Minimal API
            app.MapGet("/v2/s/{name}",
            (HttpContext requestDelegate) =>
            {
                var name = requestDelegate.GetRouteValue("name")!.ToString()!;
                var service = requestDelegate.RequestServices.GetService<ITeamService>()!;
                var teams = service.GetAllTeams();
                if (teams == null) return Results.NoContent();
                return Results.Ok(teams);
            })
            .WithName("Test")
            .WithOpenApi();


            app.Run();
        }
    }
}
