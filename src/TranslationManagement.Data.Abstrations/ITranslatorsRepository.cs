using TranslationManagement.Data.Model;

namespace TranslationManagement.Data;

/// <summary>Provides database access for <see cref="Translator"/> entity</summary>
public interface ITranslatorsRepository : IRepository<Translator>
{
    /// <summary>Get all translators with the same name</summary>
    /// <param name="name">Name to filter translators by</param>
    /// <returns>All translators with given name</returns>
    Task<IReadOnlyCollection<Translator>> GetAllByNameAsync(string name);

    /// <summary>Updates translator status</summary>
    /// <param name="translatorId">ID of translator to update status for</param>
    /// <param name="status">New status to set for translator identified by <paramref name="translatorId"/></param>
    /// <returns>Task to await to wait for the asynchronous operation to complete</returns>
    Task UpdateTranslatorStatusAsync(int translatorId, string status);
}
