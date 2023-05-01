using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TranslationManagement.Data;

namespace TranslationManagement.Api;

/// <summary>Application entry point class</summary>
public class Program
{
    /// <summary>Application entry point</summary>
    /// <param name="args">Command line arguments</param>
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        // automatic startup database migration
        var scope = host.Services.GetService<IServiceScopeFactory>().CreateScope();
        scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();

        host.Run();
    }

    /// <summary>Creates web application host builder</summary>
    /// <param name="args">Command line arguments</param>
    /// <returns>Web application host builder</returns>
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
