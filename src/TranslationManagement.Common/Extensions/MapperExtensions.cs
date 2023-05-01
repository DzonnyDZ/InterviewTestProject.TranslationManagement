using System.Diagnostics.CodeAnalysis;
using AutoMapper;

namespace TranslationManagement.Extensions;

/// <summary>Provides extension methods form working with <see cref="AutoMapper"/></summary>
public static class MapperExtensions
{
    /// <summary>Maps a collection</summary>
    /// <typeparam name="TTo">Type to map items in the collection to</typeparam>
    /// <param name="mapper">Mapper instance to use</param>
    /// <param name="values">The collection to map</param>
    /// <returns>
    /// Collection containing items from <paramref name="values"/> mapped to type <typeparamref name="TTo"/> using <paramref name="mapper"/>;
    /// null if <paramref name="values"/> is null.
    /// </returns>
    [return: NotNullIfNotNull(nameof(values))]
    public static IReadOnlyCollection<TTo>? MapAll<TTo>(this IMapper mapper, IReadOnlyCollection<object>? values)
    {
        if (mapper is null) throw new ArgumentNullException(nameof(mapper));
        if (values is null) return null;
        return values.Select(x => mapper.Map<TTo>(x)).ToArray();
    }
}