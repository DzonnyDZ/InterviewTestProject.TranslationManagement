using TranslationManagement.Dto;

namespace TranslationManagement.Business;

/// <summary>Business layer for working with Translators</summary>
public interface ITranslatorsBll
{
    /// <summary>Gets all translators unconditionally</summary>
    /// <returns>All translators from database without filtering</returns>
    //TODO: This may be to much, some filtering or paging may be useful
    Task<IReadOnlyCollection<TranslatorModel>> GetAllAsync();

    /// <summary>Gets all translators with same name</summary>
    /// <param name="name">Name of the translator</param>
    /// <returns>All translators with same name</returns>
    Task<IReadOnlyCollection<TranslatorModel>> GetByNameAsync(string name);

    /// <summary>Saves a new translator to database</summary>
    /// <param name="translator">Translator data to save. <paramref name="translator"/>.<see cref="TranslatorModel.Id">Id</see> will be set to database-generated ID.</param>
    /// <returns>Task to await to wait for the asynchronous operation to complete</returns>
    Task AddTranslatorAsync(TranslatorModel translator);

    /// <summary>Updates translator status</summary>
    /// <param name="translatorId">ID of translator to update status</param>
    /// <param name="status">The status to set</param>
    /// <returns>Task to await to wait for the asynchronous operation to complete</returns>
    Task SetStatusAsync(int translatorId, string status);

    /// <summary>Gets <see cref="TranslatorModel"/> identified by ID</summary>
    /// <param name="id">ID of translator to get</param>
    /// <returns>Object representing requested translator, null if no translator with given <paramref name="id"/> is in database.</returns>
    Task<TranslatorModel?> GetTranslatorByIdAsync(int id);
}
