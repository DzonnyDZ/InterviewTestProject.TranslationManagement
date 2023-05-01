using AutoMapper;
using External.ThirdParty.Services;
using Microsoft.Extensions.DependencyInjection;
using TranslationManagement.Business.JobReaders;
using TranslationManagement.Data.Model;
using TranslationManagement.Dto;

namespace TranslationManagement.Business;

/// <summary>Provides extension methods to be used during application startup / configuration phase</summary>
public static class StartupExtensions
{
    /// <summary>Registers business layer interfaces and their implementations</summary>
    /// <param name="services">Service collection to register the interfaces with</param>
    /// <returns><paramref name="services"/> to allow fluent API chaining</returns>
    public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));

        services.AddSingleton<IJobFileReaderFactory, JobFileReaderFactory>();
        services.AddTransient<INotificationService, UnreliableNotificationService>();
        services.AddTransient<ITranslationJobsBll, TranslationJobsBll>();
        services.AddTransient<ITranslatorsBll, TranslatorsBll>();

        return services;
    }

    /// <summary>Sets up business layer <see cref="AutoMapper"/> configuration</summary>
    /// <param name="cfg"><see cref="AutoMapper"/> configuration to register the mappings with</param>
    /// <returns><paramref name="cfg"/> to allow fluent API chaining</returns>
    public static IMapperConfigurationExpression ConfigureBusinessMappings(this IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Translator, TranslatorModel>();
        cfg.CreateMap<TranslatorModel, Translator>();

        cfg.CreateMap<TranslationJob, TranslationJobModel>();
        cfg.CreateMap<TranslationJobCreationModel, TranslationJob>();
        cfg.CreateMap<TranslationJobModel, TranslationJob>().IncludeBase<TranslationJobCreationModel, TranslationJob>();
        return cfg;
    }
}
