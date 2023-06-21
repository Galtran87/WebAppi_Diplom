using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using WebAppi_Diplom;

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

            app.Run();
        }
    }
}
