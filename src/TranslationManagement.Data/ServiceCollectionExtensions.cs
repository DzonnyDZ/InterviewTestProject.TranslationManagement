using Microsoft.Extensions.DependencyInjection;

namespace TranslationManagement.Data;

/// <summary>Provides extension methods for registration of data layer interfaces and their implementations</summary>
public static class ServiceCollectionExtensions
{
    /// <summary>Registers data layer interfaces and their implementations</summary>
    /// <param name="services">Service collection to register the interfaces with</param>
    /// <returns><paramref name="services"/> to allow fluent API chaining</returns>
    public static IServiceCollection AddDataLayer(this IServiceCollection services)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));

        services.AddTransient<ITranslationJobsRepository, TranslationJobsRepository>();
        services.AddTransient<ITranslatorsRepository, TranslatorsRepository>();

        return services;
    }
}
