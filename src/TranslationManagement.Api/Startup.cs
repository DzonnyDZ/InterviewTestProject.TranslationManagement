using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using TranslationManagement.Business;
using TranslationManagement.Data;
using TranslationManagement.Dto;

namespace TranslationManagement.Api;

/// <summary>Configures the web application at startup</summary>
public class Startup
{
    /// <summary>Initializes a new instance of the <see cref="Startup"/> class.</summary>
    /// <param name="configuration">Provides access to application configuration</param>
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>Gets an object which provides access to application configuration</summary>
    public IConfiguration Configuration { get; }

    /// <summary>Registers services for dependency injection</summary>
    /// <param name="services">The service collection to register the services with</param>
    public void ConfigureServices(IServiceCollection services)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));

        services.AddControllers();
        services.AddDataLayer();
        services.AddBusinessLayer();
        services.AddSingleton(new MapperConfiguration(cfg => cfg.ConfigureBusinessMappings()).CreateMapper());
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "TranslationManagement.Api", Version = "v1" });
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, typeof(Startup).Assembly.GetName().Name + ".xml"), true);
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, typeof(TranslatorModel).Assembly.GetName().Name + ".xml"));
        });

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=TranslationAppDatabase.db", x => x.MigrationsAssembly(typeof(Migrations.InitialCreate).Assembly.FullName)));
    }

    /// <summary>Sets up application configuration</summary>
    /// <param name="app">Application builder</param>
    /// <param name="env">Provides access to hosting environment</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (app is null) throw new ArgumentNullException(nameof(app));

        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TranslationManagement.Api v1"));

        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
